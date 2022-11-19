using AutoMapper;
using CrackerProject.API.DataModels;
using CrackerProject.API.Models;
using CrackerProject.API.ViewModel;

namespace CrackerProject.API.AutoMapper
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookCreationForm>().ReverseMap();
            CreateMap<Book, BookResponse>();
            CreateMap<Section, SectionCreationForm>().ReverseMap();
            CreateMap<Section, SectionResponse>();
            CreateMap<BookSection, SectionCreationForm>().ReverseMap();
            CreateMap<BookSection, SectionResponse>();
            CreateMap<SubSection, SectionCreationForm>().ReverseMap();
            CreateMap<SubSection, SectionResponse>();
            CreateMap<Section, SectionCreationForm>().ReverseMap();
            CreateMap<Section, SectionResponse>();
            CreateMap<Section, BookSection>().ReverseMap();
            CreateMap<Section, SubSection>().ReverseMap();
            CreateMap<QuestionSet,QuestionSetResponse>();
            CreateMap<QuestionSetForm, QuestionSet>();
        }
    }
}
