using AutoMapper;
using CrackerProject.API.Model;

namespace CrackerProject.API.AutoMapper
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            //Book
            CreateMap<Model.Book, Data.MongoDb.SchemaOne.Model.Book>();
            CreateMap<Data.MongoDb.SchemaOne.Model.Book, Model.Book>();
            CreateMap<Model.Book, ViewModel.BookResponse>();
            CreateMap<ViewModel.BookCreationForm, Model.Book>();         
        }
    }
}
