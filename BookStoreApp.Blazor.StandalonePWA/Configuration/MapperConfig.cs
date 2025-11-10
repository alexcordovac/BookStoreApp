using AutoMapper;
using BookStoreApp.Blazor.StandalonePWA.Services.Base;

namespace BookStoreApp.Blazor.StandalonePWA.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<AuthorDetailsDto, AuthorUpdateDto>().ReverseMap();
            CreateMap<BookDetailsDto, BookUpdateDto>().ReverseMap();
        }
    }
}
