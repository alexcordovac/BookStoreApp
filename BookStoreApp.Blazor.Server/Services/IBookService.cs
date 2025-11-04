using BookStoreApp.Blazor.Server.Models;
using BookStoreApp.Blazor.Server.Services.Base;

namespace BookStoreApp.Blazor.Server.Services
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