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
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public AnswerService(IAnswerRepository answerRepository, IQuestionRepository questionRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<AnswerSubmissionResult> SubmitAnswersAsync(List<AnswerVM> answers, int userId)
        {
            var result = new AnswerSubmissionResult();

            Console.WriteLine($"=== ANSWER SERVICE STARTED ===");
            Console.WriteLine($"Processing {answers?.Count} answers for user {userId}");

            if (answers == null || !answers.Any())
            {
                result.Success = false;
                result.Errors.Add("No answers provided.");
                Console.WriteLine("No answers provided - returning error");
                return result;
            }

            try
            {
                // Get form ID from first answer
                var formId = answers.First().QuestionFormId;
                Console.WriteLine($"Form ID: {formId}");

                // Check if user already submitted this form
                Console.WriteLine("Checking if user already submitted this form...");
                var hasSubmitted = await _answerRepository.HasUserSubmittedFormAsync(userId, formId);
                Console.WriteLine($"Has submitted: {hasSubmitted}");

                if (hasSubmitted)
                {
                    result.Success = false;
                    result.Errors.Add("You have already submitted answers for this form.");
                    Console.WriteLine("User already submitted this form - returning error");
                    return result;
                }

                // Map to entities
                var answerEntities = new List<Answer>();
                Console.WriteLine("Mapping answers to entities...");

                foreach (var answerVm in answers)
                {
                    Console.WriteLine($"  Mapping - Q{answerVm.QuestionId}: Scale={answerVm.ScaleValue}, Text='{answerVm.TextValue}'");

                    var answerEntity = new Answer
                    {
                        UserId = userId,
                        QuestionId = answerVm.QuestionId,
                        QuestionFormId = formId,
                        ScaleValue = answerVm.ScaleValue,
                        TextValue = answerVm.TextValue,
                        AnsweredDate = DateTime.UtcNow
                    };

                    answerEntities.Add(answerEntity);
                    Console.WriteLine($"  Created entity: User{answerEntity.UserId}, Q{answerEntity.QuestionId}, Scale: {answerEntity.ScaleValue}");
                }

                Console.WriteLine($"Attempting to save {answerEntities.Count} entities to database...");

                // Save to database
                var savedCount = await _answerRepository.InsertAnswersAsync(answerEntities);

                Console.WriteLine($"Database save completed. Rows affected: {savedCount}");

                result.Success = savedCount > 0;
                result.SubmittedAnswersCount = savedCount;
                result.SubmissionDate = DateTime.UtcNow;

                if (result.Success)
                {
                    Console.WriteLine($"✅ SUCCESS: Saved {savedCount} answers to database");
                }
                else
                {
                    result.Errors.Add("Failed to save answers to database. No rows were affected.");
                    Console.WriteLine("❌ FAILED: No rows affected in database");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR in AnswerService: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                result.Success = false;
                result.Errors.Add($"Database error: {ex.Message}");
                return result;
            }
        }
        public async Task<List<AnswerVM>> GetUserAnswersAsync(int userId)
        {
            var answers = await _answerRepository.GetAnswersByUserIdAsync(userId);
            return _mapper.Map<List<AnswerVM>>(answers);
        }

        public async Task<List<AnswerVM>> GetFormAnswersAsync(int formId)
        {
            var answers = await _answerRepository.GetFormAnswersWithDetailsAsync(formId);
            return _mapper.Map<List<AnswerVM>>(answers);
        }

        public async Task<List<AnswerVM>> GetQuestionAnswersAsync(int questionId)
        {
            var answers = await _answerRepository.GetAnswersByQuestionIdAsync(questionId);
            return _mapper.Map<List<AnswerVM>>(answers);
        }

        public async Task<AnswerVM> GetAnswerByIdAsync(int answerId)
        {
            var answer = await _answerRepository.GetAnswerByIdAsync(answerId);
            return _mapper.Map<AnswerVM>(answer);
        }

        public async Task<List<UserFormAnswersVM>> GetUserFormAnswersAsync(int userId, int formId)
        {
            var answers = await _answerRepository.GetAnswersByUserIdAsync(userId);
            var formAnswers = answers.Where(a => a.QuestionFormId == formId).ToList();

            return _mapper.Map<List<UserFormAnswersVM>>(formAnswers);
        }

        public async Task<bool> HasUserSubmittedFormAsync(int userId, int formId)
        {
            return await _answerRepository.HasUserSubmittedFormAsync(userId, formId);
        }

        public async Task<DateTime?> GetUserLastSubmissionDateAsync(int userId, int formId)
        {
            var answers = await _answerRepository.GetAnswersByUserIdAsync(userId);
            return answers.Where(a => a.QuestionFormId == formId)
                         .OrderByDescending(a => a.AnsweredDate)
                         .FirstOrDefault()?.AnsweredDate;
        }

        public async Task<Dictionary<int, AnswerSummaryVM>> GetAnswerSummariesAsync(int formId)
        {
            var summaries = await _answerRepository.GetAnswerSummariesAsync(formId);
            return _mapper.Map<Dictionary<int, AnswerSummaryVM>>(summaries);
        }

        public async Task<List<AnswerVM>> GetRecentAnswersAsync(int count = 50)
        {
            var answers = await _answerRepository.GetRecentAnswersAsync(count);
            return _mapper.Map<List<AnswerVM>>(answers);
        }

        public async Task<bool> DeleteAnswerAsync(int answerId)
        {
            return await _answerRepository.DeleteAnswerAsync(answerId);
        }

        public async Task<bool> UpdateAnswerAsync(AnswerVM answer)
        {
            var answerEntity = _mapper.Map<Answer>(answer);
            return await _answerRepository.UpdateAnswerAsync(answerEntity);
        }

        public async Task<int> GetFormResponseCountAsync(int formId)
        {
            return await _answerRepository.GetFormResponseCountAsync(formId);
        }

        // Private helper methods
        private async Task<AnswerValidationResult> ValidateAnswersAsync(List<AnswerVM> answers, int userId)
        {
            var result = new AnswerValidationResult();

            foreach (var answer in answers)
            {
                var question = _questionRepository.GetQuestionById(answer.QuestionId);
                if (question == null)
                {
                    result.Errors.Add($"Invalid question ID: {answer.QuestionId}");
                    continue;
                }

                // Check required questions
                if (question.IsRequired)
                {
                    if (question.QuestionType.Name == "Scale" && !answer.ScaleValue.HasValue)
                    {
                        result.Errors.Add($"Required question not answered: {question.Text}");
                    }
                    else if (question.QuestionType.Name == "Text" && string.IsNullOrWhiteSpace(answer.TextValue))
                    {
                        result.Errors.Add($"Required question not answered: {question.Text}");
                    }
                }

                // Validate scale range
                if (question.QuestionType.Name == "Scale" && answer.ScaleValue.HasValue)
                {
                    if (answer.ScaleValue < 1 || answer.ScaleValue > 10)
                    {
                        result.Errors.Add($"Invalid scale value for question: {question.Text}. Must be between 1-10.");
                    }
                }
            }

            result.IsValid = !result.Errors.Any();
            return result;
        }
    }
}
