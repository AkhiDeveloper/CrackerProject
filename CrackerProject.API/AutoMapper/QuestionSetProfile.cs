using AutoMapper;
using CrackerProject.API.Model.Book;

namespace CrackerProject.API.AutoMapper
{
    public class QuestionSetProfile : Profile
    {
        public QuestionSetProfile()
        {
            //QuestionSet
            CreateMap<QuestionSet, Data.MongoDb.SchemaOne.Model.QuestionSet>();
            CreateMap<Data.MongoDb.SchemaOne.Model.QuestionSet, QuestionSet>();
            CreateMap<QuestionSet, ViewModel.QuestionSetResponse>();
            CreateMap<ViewModel.QuestionSetForm, QuestionSet>();
        }
    }
}
