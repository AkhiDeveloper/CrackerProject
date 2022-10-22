using AutoMapper;
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
            CreateMap<BookSection, BookSectionCreationForm>().ReverseMap();
            CreateMap<BookSection, BookSectionResponse>();
        }
    }
}
