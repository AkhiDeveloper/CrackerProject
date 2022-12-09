using AutoMapper;
using CrackerProject.API.Model;

namespace CrackerProject.API.AutoMapper
{
    public class SectionProfile : Profile
    {
        public SectionProfile()
        {
            //Section
            CreateMap<Model.Section, DataModels.Section>();
            CreateMap<Model.Section, DataModels.BookSection>().ReverseMap();
            CreateMap<Model.Section, DataModels.SubSection>().ReverseMap();
            CreateMap<DataModels.Section, Model.Section>();
            CreateMap<Model.Section, ViewModel.SectionResponse>();
            CreateMap<ViewModel.SectionCreationForm, Model.Section>();

        }
    }
}
