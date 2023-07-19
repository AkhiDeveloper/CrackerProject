using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace BookManager.API.Mapper.Automapper
{
    public class BookProfile :
        Profile
    {

        public BookProfile()
        {
            //Model to ViewModel
            CreateMap<Models.Book, DTOs.BookDTO>();
            CreateMap<DTOs.BookForm, Models.Book>();
            CreateMap<DTOs.BookEditForm, Models.Book>();

            //Model to DataModel
            CreateMap<Models.Book, Data.Models.Book>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Data.Models.Book, Models.Book>();
        }
    }
}
