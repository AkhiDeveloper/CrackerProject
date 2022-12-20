using AutoMapper;
using CrackerProject.API.Model;

namespace CrackerProject.API.AutoMapper
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            //Question
            CreateMap<Model.Question, Data.MongoDb.SchemaOne.Model.Question>();
            CreateMap<Data.MongoDb.SchemaOne.Model.Question, Model.Question>();
            CreateMap<Model.Question, ViewModel.QuestionResponse>();
            CreateMap<ViewModel.QuestionForm, Model.Question>();
            CreateMap<ViewModel.OptionForm, Model.Option>();
            CreateMap<Model.Option, ViewModel.OptionResponse>();
            CreateMap<Model.Option, Data.MongoDb.SchemaOne.Model.Option>().ReverseMap();  
            CreateMap<Model.OptionSet, Data.MongoDb.SchemaOne.Model.OptionSet>().ReverseMap();
        }
    }
}
