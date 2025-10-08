using AutoMapper;
using GlasAnketa.DataAccess.Implementations;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;
using GlasAnketa.Services.Interfaces;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Implementations
{
    public class QuestionFormService : IQuestionFormService
    {
        private readonly IQuestionFormRepository _questionFormRepository;
        private readonly IMapper _mapper;

        public QuestionFormService(IQuestionFormRepository questionFormRepository, IMapper mapper)
        {
            _questionFormRepository = questionFormRepository;
            _mapper = mapper;
        }

        public async Task<List<QuestionFormVM>> GetActiveFormsAsync()
        {
            // Since GetActiveForms is synchronous in repository, wrap it
            var forms = _questionFormRepository.GetActiveForms();
            return await Task.FromResult(_mapper.Map<List<QuestionFormVM>>(forms));
        }

        public async Task<QuestionFormVM> GetFormWithQuestionsAsync(int formId)
        {
            try
            {
                Console.WriteLine($"GetFormWithQuestionsAsync called for form {formId}");

                var form = _questionFormRepository.GetFormWithQuestions(formId);
                Console.WriteLine($"Repository returned form: {form != null}");

                if (form == null)
                {
                    Console.WriteLine($"Form {formId} not found in repository");
                    throw new Exception($"Form with id {formId} not found.");
                }

                Console.WriteLine($"Form found: {form.Title}, Questions: {form.Questions?.Count}");

                if (form.Questions != null)
                {
                    foreach (var question in form.Questions)
                    {
                        Console.WriteLine($"  Question {question.Id}: {question.Text}, Type: {question.QuestionType?.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("  No questions found for this form");
                }

                var result = _mapper.Map<QuestionFormVM>(form);
                Console.WriteLine($"Mapped to VM: {result != null}, Questions in VM: {result.Questions?.Count}");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR in GetFormWithQuestionsAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<List<QuestionFormVM>> GetAllFormsAsync()
        {
            var forms = _questionFormRepository.GetAllFormQuestions();
            return await Task.FromResult(_mapper.Map<List<QuestionFormVM>>(forms));
        }

        public async Task<QuestionFormVM> GetFormByIdAsync(int formId)
        {
            var form = _questionFormRepository.GetQuestionFormById(formId);
            if (form == null)
                throw new Exception($"Form with id {formId} not found.");

            return await Task.FromResult(_mapper.Map<QuestionFormVM>(form));
        }

        public async Task<int> CreateFormAsync(QuestionFormVM formVm)
        {
            var form = _mapper.Map<QuestionForm>(formVm);
            form.CreatedDate = DateTime.UtcNow;
            form.IsActive = true;

            return await _questionFormRepository.InsertFormAsync(form);
        }

        public async Task<bool> UpdateFormAsync(QuestionFormVM formVm)
        {
            var existingForm = _questionFormRepository.GetQuestionFormById(formVm.Id);
            if (existingForm == null)
                return false;

            _mapper.Map(formVm, existingForm);
            return await _questionFormRepository.UpdateFormAsync(existingForm);
        }

        public async Task<bool> DeleteFormAsync(int formId)
        {
            try
            {
                _questionFormRepository.DeleteQuestionForm(formId);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error deleting form {formId}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ToggleFormStatusAsync(int formId, bool isActive)
        {
            return await _questionFormRepository.ToggleFormStatusAsync(formId, isActive);
        }

        public async Task<bool> FormExistsAsync(int formId)
        {
            var form = _questionFormRepository.GetQuestionFormById(formId);
            return form != null;
        }

        public async Task<int> GetFormQuestionCountAsync(int formId)
        {
            var form = _questionFormRepository.GetFormWithQuestions(formId);
            return form?.Questions?.Count ?? 0;
        }
    }
}
