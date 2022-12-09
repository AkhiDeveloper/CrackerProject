using AutoMapper;
using CrackerProject.API.Model;

namespace CrackerProject.API.AutoMapper
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            //Question
            CreateMap<Model.Question, DataModels.ObjectiveQuestion>();
            CreateMap<DataModels.ObjectiveQuestion, Model.Question>();
            CreateMap<Model.Question, ViewModel.QuestionResponse>();
            CreateMap<ViewModel.QuestionForm, Model.Question>();
        }
    }
}
