using AutoMapper;
using CrackerProject.API.Model;

namespace CrackerProject.API.AutoMapper
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            //Book
            CreateMap<Model.Book, DataModels.Book>();
            CreateMap<DataModels.Book, Model.Book>();
            CreateMap<Model.Book, ViewModel.BookResponse>();
            CreateMap<ViewModel.BookCreationForm, Model.Book>();         
        }
    }
}
