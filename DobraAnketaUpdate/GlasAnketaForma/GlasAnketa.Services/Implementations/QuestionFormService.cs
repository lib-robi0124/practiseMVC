using AutoMapper;
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

        public async Task<int> CreateFormAsync(QuestionFormVM formVm)
        {
            var form = _mapper.Map<QuestionForm>(formVm);
            form.CreatedDate = DateTime.UtcNow;
            form.IsActive = true;

            var addedForm = await _questionFormRepository.AddAsync(form);
            return addedForm.Id;
        }

        public async Task<bool> DeleteFormAsync(int formId)
        {
            try
            {
                var form = await _questionFormRepository.GetQuestionFormByIdAsync(formId);
                if (form == null) return false;

                await _questionFormRepository.Remove(form);
                return true;
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                return false;
            }
        }

        public async Task<QuestionFormVM> GetActiveFormAsync()
        {
            var forms = await _questionFormRepository.GetAllFormQuestionsAsync();
            var activeForm = forms.FirstOrDefault(f => f.IsActive);
            return _mapper.Map<QuestionFormVM>(activeForm);
        }

        public async Task<List<QuestionFormVM>> GetAllActiveFormsAsync()
        {
            var forms = await _questionFormRepository.GetAllFormQuestionsAsync();
            var active = forms.Where(f => f.IsActive).ToList();
            return _mapper.Map<List<QuestionFormVM>>(active);
        }

        public async Task<List<QuestionFormVM>> GetAllFormsAsync()
        {
            var forms = _questionFormRepository.GetAllFormQuestionsAsync();
            return await Task.FromResult(_mapper.Map<List<QuestionFormVM>>(forms));
        }

        public async Task<QuestionFormVM> GetFormByIdAsync(int id)
        {
            var qForm = await _questionFormRepository.GetQuestionFormByIdAsync(id);
            return _mapper.Map<QuestionFormVM>(qForm);
        }

        public async Task<QuestionFormVM> GetFormWithQuestionsAsync(int formId)
        {
            var qForm = await _questionFormRepository.GetActiveAsync(formId);
            return _mapper.Map<QuestionFormVM>(qForm);
        }

        public async Task<QuestionFormVM?> GetNextActiveFormAsync(int currentFormId)
        {
            // Find the next active form ID
            var nextForm = await _questionFormRepository.GetNextActiveFormAsync(currentFormId);
            if (nextForm == null)
                return null;

            // Map fresh form with questions (loaded directly from DB)
            return _mapper.Map<QuestionFormVM>(nextForm);
        }

        public async Task<bool> ToggleFormStatusAsync(int formId, bool isActive)
        {
            return await _questionFormRepository.ToggleFormStatusAsync(formId, isActive);
        }

        public async Task<bool> UpdateFormAsync(QuestionFormVM formVm)
        {
            var existingForm = await _questionFormRepository.GetQuestionFormByIdAsync(formVm.Id);
            if (existingForm == null)
                return false;

            _mapper.Map(formVm, existingForm);
            await _questionFormRepository.Update(existingForm);

            return true;
        }
    }
}
