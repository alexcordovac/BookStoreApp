using BookStoreApp.Blazor.StandalonePWA.Services.Base;
using BookStoreApp.Blazor.StandalonePWA.Models;

namespace BookStoreApp.Blazor.StandalonePWA.Services
{
    public interface IAuthorService
    {
        Task<Response<AuthorReadOnlyDtoVirtualizeResponse>> Get(QueryParameters queryParams);
        Task<Response<AuthorDetailsDto>> Get(int id);
        Task<Response<AuthorUpdateDto>> GetForUpdate(int id);
        Task<Response<int>> Create(AuthorCreateDto author);
        Task<Response<int>> Edit(int id, AuthorUpdateDto author);
        Task<Response<int>> Delete(int id);
    }
}