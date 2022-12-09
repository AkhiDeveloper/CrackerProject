using AutoMapper;
using CrackerProject.API.Model;

namespace CrackerProject.API.AutoMapper
{
    public class QuestionSetProfile : Profile
    {
        public QuestionSetProfile()
        {
            //QuestionSet
            CreateMap<Model.QuestionSet, DataModels.QuestionSet>();
            CreateMap<DataModels.QuestionSet, Model.QuestionSet>();
            CreateMap<Model.QuestionSet, ViewModel.QuestionSetResponse>();
            CreateMap<ViewModel.QuestionSetForm, Model.QuestionSet>();
        }
    }
}
