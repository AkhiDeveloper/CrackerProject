using AutoMapper;

namespace BookManager.API.Mapper
{
    public class ChapterProfile
        : Profile
    {
        public ChapterProfile()
        {
            CreateMap<Data.Models.Chapter, Models.Chapter>();
            CreateMap<Models.Chapter, Data.Models.Chapter>().ForMember(x => x.Id, opt => opt.MapFrom((src, dst) =>
            {
                if (dst.Id == Guid.Empty)
                {
                    return src.Id;
                }
                return dst.Id;
            }));
            CreateMap<Models.Chapter, DTOs.ChapterDTO>();
            CreateMap<DTOs.ChapterDTO, Models.Chapter>();
            CreateMap<DTOs.ChapterForm, Models.Chapter>();
        }
    }
}
