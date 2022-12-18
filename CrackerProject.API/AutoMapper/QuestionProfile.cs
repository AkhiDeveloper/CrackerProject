using AutoMapper;
using CrackerProject.API.Model;

namespace CrackerProject.API.AutoMapper
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            //Question
            CreateMap<Model.Question, DataModels.Question>();
            CreateMap<DataModels.Question, Model.Question>();
            CreateMap<Model.Question, ViewModel.QuestionResponse>();
            CreateMap<ViewModel.QuestionForm, Model.Question>();
            CreateMap<ViewModel.OptionForm, Model.Option>();
            CreateMap<Model.Option, ViewModel.OptionResponse>();
            CreateMap<Model.Option, DataModels.Option>().ReverseMap();  
            CreateMap<Model.OptionSet, DataModels.OptionSet>().ReverseMap();
        }
    }
}
