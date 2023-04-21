using AutoMapper;

namespace BookManager.API.Mapper
{
    public class ChapterProfile
        : Profile
    {
        public ChapterProfile()
        {
            CreateMap<Data.Models.Chapter, Models.Chapter>();
            CreateMap<Models.Chapter, Data.Models.Chapter>();
            CreateMap<Models.Chapter, DTOs.ChapterDTO>();
            CreateMap<DTOs.ChapterDTO, Models.Chapter>();
            CreateMap<DTOs.ChapterForm, Models.Chapter>();
        }
    }
}
