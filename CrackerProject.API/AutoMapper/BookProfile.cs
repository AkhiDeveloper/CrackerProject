using AutoMapper;
using CrackerProject.API.Model.Book;

namespace CrackerProject.API.AutoMapper
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            //Book
            CreateMap<Book, Data.MongoDb.SchemaOne.Model.Book>();
            CreateMap<Data.MongoDb.SchemaOne.Model.Book, Book>();
            CreateMap<Book, ViewModel.BookResponse>();
            CreateMap<ViewModel.BookCreationForm, Book>();         
        }
    }
}
