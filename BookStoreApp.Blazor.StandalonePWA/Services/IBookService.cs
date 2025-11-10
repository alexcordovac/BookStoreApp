using BookStoreApp.Blazor.StandalonePWA.Models;
using BookStoreApp.Blazor.StandalonePWA.Services.Base;

namespace BookStoreApp.Blazor.StandalonePWA.Services
{
    public interface IBookService
    {
        Task<Response<BookReadOnlyDtoVirtualizeResponse>> Get(QueryParameters queryParams);
        Task<Response<BookDetailsDto>> Get(int id);
        Task<Response<BookUpdateDto>> GetForUpdate(int id);
        Task<Response<int>> Create(BookCreateDto author);
        Task<Response<int>> Edit(int id, BookUpdateDto author);
        Task<Response<int>> Delete(int id);
    }
}