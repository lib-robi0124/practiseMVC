using System.ComponentModel.DataAnnotations;

namespace GlasAnketa.ViewModels.Models
{
    public class SatisfactionComparisonVM
    {
        public int SelectedQuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public int FormId { get; set; }
        public string FormTitle { get; set; }
        public List<UserSatisfactionVM> UserSatisfactions { get; set; } = new List<UserSatisfactionVM>();
        public List<QuestionVM> AvailableQuestions { get; set; } = new List<QuestionVM>();
        public SatisfactionStatisticsVM Statistics { get; set; }
        public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;
    }

    public class UserSatisfactionVM
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string FullName { get; set; }
        public string OU { get; set; }
        public string OU2 { get; set; }
        public int? ScaleValue { get; set; }
        public string TextValue { get; set; }
        public DateTime AnsweredDate { get; set; }
        public bool HasResponded { get; set; }
    }

    public class SatisfactionStatisticsVM
    {
        public double AverageScore { get; set; }
        public double MedianScore { get; set; }
        public int MinScore { get; set; }
        public int MaxScore { get; set; }
        public double StandardDeviation { get; set; }
        public int TotalResponses { get; set; }
        public int TotalUsers { get; set; }
        public double ResponseRate => TotalUsers > 0 ? (double)TotalResponses / TotalUsers * 100 : 0;
        public Dictionary<int, int> ScoreDistribution { get; set; } = new Dictionary<int, int>();
        public Dictionary<string, OU2StatisticsVM> OU2Statistics { get; set; } = new Dictionary<string, OU2StatisticsVM>();
    }

    public class OU2StatisticsVM
    {
        public string OU2Name { get; set; }
        public double AverageScore { get; set; }
        public int ResponseCount { get; set; }
        public int UserCount { get; set; }
        public double ResponseRate => UserCount > 0 ? (double)ResponseCount / UserCount * 100 : 0;
        public List<int> Scores { get; set; } = new List<int>();
    }

    public class QuestionComparisonVM
    {
        public int SelectedFormId { get; set; }
        public List<QuestionSatisfactionSummaryVM> QuestionSummaries { get; set; } = new List<QuestionSatisfactionSummaryVM>();
        public List<QuestionFormVM> AvailableForms { get; set; } = new List<QuestionFormVM>();
        public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;
    }

    public class QuestionSatisfactionSummaryVM
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public double AverageScore { get; set; }
        public int TotalResponses { get; set; }
        public Dictionary<string, double> OU2Averages { get; set; } = new Dictionary<string, double>();
        public double VarianceAcrossOU2 { get; set; }
        public string HighestScoringOU2 { get; set; }
        public string LowestScoringOU2 { get; set; }
    }
}