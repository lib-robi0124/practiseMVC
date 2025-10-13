using AutoMapper;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;
using GlasAnketa.Services.Interfaces;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Implementations
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionFormRepository _formRepository;
        private readonly IMapper _mapper;

        public AnswerService(IAnswerRepository answerRepository, IUserRepository userRepository, 
            IQuestionRepository questionRepository, IQuestionFormRepository formRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _formRepository = formRepository;
            _mapper = mapper;
        }

        public async Task<Dictionary<int, AnswerSummaryVM>> GetAnswerSummariesAsync(int formId)
        {
            var summaries = await _answerRepository.GetAnswerSummariesAsync(formId);
            return _mapper.Map<Dictionary<int, AnswerSummaryVM>>(summaries);
        }
        public async Task<List<AnswerVM>> GetFormAnswersAsync(int formId)
        {
            var answers = await _answerRepository.GetAnswersByQuestionFormIdAsync(formId);
            return _mapper.Map<List<AnswerVM>>(answers);
        }

        public async Task<List<AnswerVM>> GetUserAnswersAsync(int userId, int formId)
        {
            var answers = await _answerRepository.GetAnswersByUserIdAsync(userId, formId);
            return _mapper.Map<List<AnswerVM>>(answers);

        }

        public async Task SaveAnswersAsync(List<AnswerVM> answers)
        {
            var mappedAnswers = _mapper.Map<List<Answer>>(answers);
            await _answerRepository.SaveAnswersAsync(mappedAnswers);
        }

        public async Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers)
        {
            return await _answerRepository.SubmitAnswersAsync(userId, formId, answers);
        }

        public async Task<OU2AnalyticsVM> GetOU2AnalyticsAsync(int formId)
        {
            var form = await _formRepository.GetQuestionFormByIdAsync(formId);
            var answers = await _answerRepository.GetAnswersWithUsersByFormIdAsync(formId);
            var userCounts = await _answerRepository.GetUserCountsByOU2Async();
            
            var ou2Groups = answers.GroupBy(a => a.User.OU2);
            var ou2Results = new List<OU2ResultsVM>();

            foreach (var ou2Group in ou2Groups)
            {
                var ou2Name = ou2Group.Key;
                var ou2Answers = ou2Group.ToList();
                var uniqueUsers = ou2Answers.Select(a => a.UserId).Distinct().Count();
                
                var questionResults = ou2Answers
                    .GroupBy(a => a.QuestionId)
                    .Select(qGroup => new QuestionResultVM
                    {
                        QuestionId = qGroup.Key,
                        QuestionText = qGroup.First().Question.Text,
                        QuestionType = qGroup.First().Question.QuestionType.Name,
                        ResponseCount = qGroup.Count(),
                        AverageScore = qGroup.Where(a => a.ScaleValue.HasValue)
                                            .Select(a => a.ScaleValue.Value)
                                            .DefaultIfEmpty(0)
                                            .Average(),
                        TextResponses = qGroup.Where(a => !string.IsNullOrEmpty(a.TextValue))
                                             .Select(a => a.TextValue)
                                             .ToList(),
                        ScaleDistribution = qGroup.Where(a => a.ScaleValue.HasValue)
                                                 .GroupBy(a => a.ScaleValue.Value)
                                                 .ToDictionary(g => g.Key, g => g.Count())
                    })
                    .ToList();

                var scaleQuestions = questionResults.Where(q => q.QuestionType == "Scale" && q.AverageScore.HasValue);
                var overallAverage = scaleQuestions.Any() ? scaleQuestions.Average(q => q.AverageScore.Value) : 0;

                ou2Results.Add(new OU2ResultsVM
                {
                    OU2Name = ou2Name,
                    TotalResponses = ou2Answers.Count,
                    QuestionResults = questionResults,
                    OverallSatisfactionAverage = overallAverage,
                    EmployeeCount = userCounts.GetValueOrDefault(ou2Name, 0)
                });
            }

            return new OU2AnalyticsVM
            {
                OU2Results = ou2Results.OrderBy(r => r.OU2Name).ToList(),
                SelectedFormId = formId,
                FormTitle = form?.Title ?? "Unknown Form",
                AvailableForms = _mapper.Map<List<QuestionFormVM>>(await _formRepository.GetAllAsync())
            };
        }

        public async Task<SatisfactionComparisonVM> GetSatisfactionComparisonAsync(int questionId)
        {
            var question = await _questionRepository.GetActiveAsync(questionId);
            var answers = await _answerRepository.GetAnswersWithUsersByQuestionIdAsync(questionId);
            var allUsers = await _userRepository.GetAllUsersAsync();

            var userSatisfactions = allUsers.Select(user =>
            {
                var userAnswer = answers.FirstOrDefault(a => a.UserId == user.Id);
                return new UserSatisfactionVM
                {
                    UserId = user.Id,
                    CompanyId = user.CompanyId,
                    FullName = user.FullName,
                    OU = user.OU,
                    OU2 = user.OU2,
                    ScaleValue = userAnswer?.ScaleValue,
                    TextValue = userAnswer?.TextValue,
                    AnsweredDate = userAnswer?.AnsweredDate ?? DateTime.MinValue,
                    HasResponded = userAnswer != null
                };
            }).OrderBy(u => u.OU2).ThenBy(u => u.FullName).ToList();

            var statistics = CalculateStatistics(userSatisfactions, allUsers);

            return new SatisfactionComparisonVM
            {
                SelectedQuestionId = questionId,
                QuestionText = question?.Text ?? "Unknown Question",
                QuestionType = "Scale", // Assuming we'll mainly compare scale questions
                FormId = question?.QuestionFormId ?? 0,
                UserSatisfactions = userSatisfactions,
                Statistics = statistics
            };
        }

        public async Task<QuestionComparisonVM> GetQuestionComparisonAsync(int formId)
        {
            var form = await _formRepository.GetQuestionFormByIdAsync(formId);
            var answers = await _answerRepository.GetAnswersWithUsersByFormIdAsync(formId);
            
            var questionGroups = answers.GroupBy(a => a.QuestionId);
            var questionSummaries = new List<QuestionSatisfactionSummaryVM>();

            foreach (var questionGroup in questionGroups)
            {
                var question = questionGroup.First().Question;
                if (question.QuestionType.Name != "Scale") continue; // Focus on scale questions for comparison

                var questionAnswers = questionGroup.ToList();
                var ou2Groups = questionAnswers.GroupBy(a => a.User.OU2);
                var ou2Averages = new Dictionary<string, double>();

                foreach (var ou2Group in ou2Groups)
                {
                    var scaleValues = ou2Group.Where(a => a.ScaleValue.HasValue)
                                              .Select(a => a.ScaleValue.Value);
                    if (scaleValues.Any())
                    {
                        ou2Averages[ou2Group.Key] = scaleValues.Average();
                    }
                }

                var overallAverage = questionAnswers.Where(a => a.ScaleValue.HasValue)
                                                   .Select(a => a.ScaleValue.Value)
                                                   .DefaultIfEmpty(0)
                                                   .Average();

                var variance = ou2Averages.Values.Any() ? 
                    ou2Averages.Values.Sum(avg => Math.Pow(avg - overallAverage, 2)) / ou2Averages.Count : 0;

                questionSummaries.Add(new QuestionSatisfactionSummaryVM
                {
                    QuestionId = question.Id,
                    QuestionText = question.Text,
                    QuestionType = question.QuestionType.Name,
                    AverageScore = overallAverage,
                    TotalResponses = questionAnswers.Count,
                    OU2Averages = ou2Averages,
                    VarianceAcrossOU2 = variance,
                    HighestScoringOU2 = ou2Averages.Any() ? ou2Averages.OrderByDescending(x => x.Value).First().Key : "",
                    LowestScoringOU2 = ou2Averages.Any() ? ou2Averages.OrderBy(x => x.Value).First().Key : ""
                });
            }

            return new QuestionComparisonVM
            {
                SelectedFormId = formId,
                QuestionSummaries = questionSummaries.OrderBy(q => q.QuestionId).ToList(),
                AvailableForms = _mapper.Map<List<QuestionFormVM>>(await _formRepository.GetAllAsync())
            };
        }

        private SatisfactionStatisticsVM CalculateStatistics(List<UserSatisfactionVM> userSatisfactions, List<Domain.Models.User> allUsers)
        {
            var responses = userSatisfactions.Where(u => u.HasResponded && u.ScaleValue.HasValue)
                                           .Select(u => u.ScaleValue.Value)
                                           .ToList();

            var ou2Stats = userSatisfactions.GroupBy(u => u.OU2)
                .ToDictionary(g => g.Key, g => new OU2StatisticsVM
                {
                    OU2Name = g.Key,
                    ResponseCount = g.Count(u => u.HasResponded),
                    UserCount = g.Count(),
                    Scores = g.Where(u => u.HasResponded && u.ScaleValue.HasValue)
                              .Select(u => u.ScaleValue.Value)
                              .ToList(),
                    AverageScore = g.Where(u => u.HasResponded && u.ScaleValue.HasValue)
                                    .Select(u => u.ScaleValue.Value)
                                    .DefaultIfEmpty(0)
                                    .Average()
                });

            return new SatisfactionStatisticsVM
            {
                AverageScore = responses.Any() ? responses.Average() : 0,
                MedianScore = responses.Any() ? CalculateMedian(responses) : 0,
                MinScore = responses.Any() ? responses.Min() : 0,
                MaxScore = responses.Any() ? responses.Max() : 0,
                StandardDeviation = CalculateStandardDeviation(responses),
                TotalResponses = responses.Count,
                TotalUsers = allUsers.Count,
                ScoreDistribution = responses.GroupBy(r => r).ToDictionary(g => g.Key, g => g.Count()),
                OU2Statistics = ou2Stats
            };
        }

        private double CalculateMedian(List<int> values)
        {
            var sortedValues = values.OrderBy(x => x).ToList();
            var count = sortedValues.Count;
            if (count % 2 == 0)
            {
                return (sortedValues[count / 2 - 1] + sortedValues[count / 2]) / 2.0;
            }
            return sortedValues[count / 2];
        }

        private double CalculateStandardDeviation(List<int> values)
        {
            if (!values.Any()) return 0;
            var average = values.Average();
            var sumOfSquaresOfDifferences = values.Select(val => (val - average) * (val - average)).Sum();
            return Math.Sqrt(sumOfSquaresOfDifferences / values.Count);
        }
    }
}
