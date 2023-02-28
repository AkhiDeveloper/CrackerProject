using AutoMapper;
using CrackerProject.API.Data.MongoDb.SchemaOne.Model;

namespace CrackerProject.API.AutoMapper
{
    public class SectionProfile : Profile
    {
        public SectionProfile()
        {
            //Section
            CreateMap<Model.Book.Section, Data.MongoDb.SchemaOne.Model.Section>();
            CreateMap<Model.Book.Section, BookSection>().ReverseMap();
            CreateMap<Model.Book.Section, SubSection>().ReverseMap();
            CreateMap<Data.MongoDb.SchemaOne.Model.Section, Model.Book.Section>();
            CreateMap<Model.Book.Section, ViewModel.SectionResponse>();
            CreateMap<ViewModel.SectionCreationForm, Model.Book.Section>();

        }
    }
}
