using AutoMapper;
using CrackerProject.API.Data.MongoDb.SchemaOne.Model;
using CrackerProject.API.Model;

namespace CrackerProject.API.AutoMapper
{
    public class SectionProfile : Profile
    {
        public SectionProfile()
        {
            //Section
            CreateMap<Model.Section, Data.MongoDb.SchemaOne.Model.Section>();
            CreateMap<Model.Section, BookSection>().ReverseMap();
            CreateMap<Model.Section, SubSection>().ReverseMap();
            CreateMap<Data.MongoDb.SchemaOne.Model.Section, Model.Section>();
            CreateMap<Model.Section, ViewModel.SectionResponse>();
            CreateMap<ViewModel.SectionCreationForm, Model.Section>();

        }
    }
}
