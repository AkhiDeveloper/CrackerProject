using AutoMapper;
using CrackerProject.API.Model.Book;

namespace CrackerProject.API.AutoMapper
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            //Question
            CreateMap<Question, Data.MongoDb.SchemaOne.Model.Question>();
            CreateMap<Data.MongoDb.SchemaOne.Model.Question, Question>();
            CreateMap<Question, ViewModel.QuestionResponse>();
            CreateMap<ViewModel.QuestionForm, Question>();
            CreateMap<ViewModel.OptionForm, Option>();
            CreateMap<Option, ViewModel.OptionResponse>();
            CreateMap<Option, Data.MongoDb.SchemaOne.Model.Option>().ReverseMap();  
            CreateMap<OptionSet, Data.MongoDb.SchemaOne.Model.OptionSet>().ReverseMap();
        }
    }
}
