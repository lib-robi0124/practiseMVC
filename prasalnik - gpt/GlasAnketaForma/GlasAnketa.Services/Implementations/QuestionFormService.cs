using AutoMapper;
using GlasAnketa.DataAccess.Interfaces;
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

        public async Task<QuestionFormVM> GetFormWithQuestionsAsync(int formId)
        {
            var qForm = await _questionFormRepository.GetByIdAsync(formId);
            return _mapper.Map<QuestionFormVM>(qForm);
        }
    }
}
