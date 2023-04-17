using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace BookManager.API.Mapper
{
    public class BookProfile:
        Profile
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookProfile()
        {
            _httpContextAccessor = new HttpContextAccessor();
            //Model to ViewModel
            CreateMap<Models.Book, DTOs.BookDTO>()
                .ForMember(dst => dst.ImageUrl, opt => opt
                .MapFrom(src => GetApiUrl(src.ImageUrl)));
            CreateMap<DTOs.BookForm, Models.Book>();
            CreateMap<DTOs.BookEditForm, Models.Book>();

            //Model to DataModel
            CreateMap<Models.Book, Data.Models.Book>().ReverseMap();
        }

        private string GetApiUrl(string? imageUrl)
        {
            if (!String.IsNullOrWhiteSpace(imageUrl))
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var host = httpContext.Request.Host.Host;
                var port = httpContext.Request.Host.Port ?? 80;
                var baseUrl = new Uri($"https://{host}:{port}/api");
                var fullUrl = new Uri(baseUrl, imageUrl);
                return fullUrl.AbsoluteUri;
            }
            return String.Empty;
        }
    }
}
