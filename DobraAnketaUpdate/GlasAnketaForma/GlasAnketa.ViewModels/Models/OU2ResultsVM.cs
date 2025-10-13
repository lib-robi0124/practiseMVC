using System.ComponentModel.DataAnnotations;

namespace GlasAnketa.ViewModels.Models
{
    public class OU2ResultsVM
    {
        public string OU2Name { get; set; }
        public int TotalResponses { get; set; }
        public List<QuestionResultVM> QuestionResults { get; set; } = new List<QuestionResultVM>();
        public double OverallSatisfactionAverage { get; set; }
        public int EmployeeCount { get; set; }
        public double ResponseRate => EmployeeCount > 0 ? (double)TotalResponses / EmployeeCount * 100 : 0;
    }

    public class QuestionResultVM
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public double? AverageScore { get; set; }
        public int ResponseCount { get; set; }
        public List<string> TextResponses { get; set; } = new List<string>();
        public Dictionary<int, int> ScaleDistribution { get; set; } = new Dictionary<int, int>();
    }

    public class OU2AnalyticsVM
    {
        public List<OU2ResultsVM> OU2Results { get; set; } = new List<OU2ResultsVM>();
        public int SelectedFormId { get; set; }
        public string FormTitle { get; set; }
        public List<QuestionFormVM> AvailableForms { get; set; } = new List<QuestionFormVM>();
        public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;
    }

    public class OU2ComparisonVM
    {
        public string QuestionText { get; set; }
        public int QuestionId { get; set; }
        public string QuestionType { get; set; }
        public List<OU2ScoreVM> OU2Scores { get; set; } = new List<OU2ScoreVM>();
        public double OverallAverage { get; set; }
        public double StandardDeviation { get; set; }
    }

    public class OU2ScoreVM
    {
        public string OU2Name { get; set; }
        public double AverageScore { get; set; }
        public int ResponseCount { get; set; }
        public double? MedianScore { get; set; }
        public List<int> AllScores { get; set; } = new List<int>();
    }
}