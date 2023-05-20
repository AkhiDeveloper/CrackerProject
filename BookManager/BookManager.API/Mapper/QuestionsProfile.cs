using AutoMapper;
using BookManager.API.DTOs;

namespace BookManager.API.Mapper
{
    public class QuestionsProfile
        : Profile
    {
        public QuestionsProfile()
        {
            CreateMap<QuestionForm, Models.Question>();
            CreateMap<Models.Question, QuestionDTO>();
            CreateMap<Models.Question, Data.Models.Question>().ReverseMap();

            CreateMap<OptionForm, Models.Option>();
            CreateMap<Models.Option, OptionDTO>();
            CreateMap<Models.Option, Data.Models.Option>().ReverseMap();
        }
    }
}
