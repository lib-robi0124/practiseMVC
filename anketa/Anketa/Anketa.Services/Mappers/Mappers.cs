using Anketa.Domain.Models;
using Anketa.ViewModels;

namespace Anketa.Services.Mappers
{
    public static class Mappers
    {
        public static QuestionViewModel ToViewModel(this Question question)
        {
            return new QuestionViewModel
            {
                Id = question.Id,
                Text = question.Text,
                QuestionType = question.QuestionType?.Name,
                IsRequired = question.IsRequired
            };
        }

        public static QuestionFormViewModel ToViewModel(this QuestionForm form)
        {
            return new QuestionFormViewModel
            {
                Id = form.Id,
                Title = form.Title,
                Description = form.Description,
                Questions = form.Questions?.Select(q => q.ToViewModel())
                .ToList() ?? new List<QuestionViewModel>()
            };
        }
    }
}
