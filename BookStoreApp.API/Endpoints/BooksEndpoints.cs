using AutoMapper;
using BookStoreApp.API.Contracts;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Models.Book;
using BookStoreApp.API.Options;
using BookStoreApp.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BookStoreApp.API.Endpoints
{
    public static class BooksEndpoints
    {
        public static IEndpointRouteBuilder MapBooksEndpoints(this IEndpointRouteBuilder endpointBuilder)
        {
            var group = endpointBuilder.MapGroup("/books");

            group.MapGet("{id:int}", async Task<IResult> (int id, HttpContext context, IBooksRepository booksRepository, ILogger<BooksApi> logger) =>
            {
                try
                {
                    var book = await booksRepository.GetBookAsync(id);
                    if (book == null)
                    {
                        logger.LogWarning($"Record Not Found: {nameof(Book)} - ID: {id}");
                        return TypedResults.NotFound();
                    }
                    return TypedResults.Ok(book);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error Performing task in {context.GetEndpoint()}");
                    return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
                }
            })
            .WithName("getBookById")
            .WithOpenApi();


            group.MapGet("", async Task<IResult> ([AsParameters] QueryParameters queryParameters, HttpContext context, IBooksRepository booksRepository, ILogger<BooksApi> logger) =>
            {
                try
                {
                    var bookList = await booksRepository.GetAllAsync<BookDetailsDto>(queryParameters);
                    return TypedResults.Ok(bookList);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error Performing task in {context.GetEndpoint()}");
                    return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
                }
            })
           .WithOpenApi();



            group.MapPut("{id:int}", async Task<IResult> (int id, BookUpdateDto bookDto,
                HttpContext context,
                IBooksRepository booksRepository,
                IFileService fileService,
                IOptions<StorageOptions> storageOptions,
                IMapper _mapper,
                ILogger<BooksApi> logger) =>
            {
                if (id != bookDto.Id)
                {
                    logger.LogWarning($"Update ID invalid in {context.GetEndpoint()} - ID: {id}");
                    return TypedResults.BadRequest();
                }

                var book = await booksRepository.GetAsync(id);

                if (book == null)
                {
                    logger.LogWarning($"{nameof(Author)} record not found in {context.GetEndpoint()} - ID: {id}");
                    return TypedResults.NotFound();
                }

                if (string.IsNullOrEmpty(bookDto.ImageData) == false)
                {
                    // Create new image file
                    var newImageFileName = await fileService.SaveFileAsync(bookDto.ImageData, bookDto.OriginalImageName, storageOptions.Value.BookCoversPath);
                    bookDto.Image = $"{storageOptions.Value.BookCoversPublicUrl}{newImageFileName}";

                    // Delete old image file
                    var oldImageFileName = Path.GetFileName(book.Image);
                    var oldImageFilePath = Path.Combine(storageOptions.Value.BookCoversPath, oldImageFileName);
                    if (System.IO.File.Exists(oldImageFilePath))
                    {
                        System.IO.File.Delete(oldImageFilePath);
                    }
                }

                _mapper.Map(bookDto, book);

                try
                {
                    await booksRepository.UpdateAsync(book);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!await booksRepository.Exists(id))
                    {
                        return TypedResults.NotFound();
                    }
                    else
                    {
                        logger.LogError(ex, $"Error Performing GET in {context.GetEndpoint()}");
                        return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }

                return TypedResults.NoContent();
            })
           .WithOpenApi();


            group.MapPost("", async Task<IResult> (BookCreateDto bookDto, HttpContext context, IBooksRepository booksRepository, IFileService fileService, IOptions<StorageOptions> storageOptions, IMapper _mapper, ILogger<BooksApi> logger) =>
            {
                try
                {
                    var book = _mapper.Map<Book>(bookDto);
                    if (string.IsNullOrEmpty(bookDto.ImageData) == false)
                    {
                        var fileName = await fileService.SaveFileAsync(bookDto.ImageData, bookDto.OriginalImageName, storageOptions.Value.BookCoversPath);
                        book.Image = $"{storageOptions.Value.BookCoversPublicUrl}{fileName}";
                    }

                    await booksRepository.AddAsync(book);

                    return TypedResults.CreatedAtRoute(book, "getBookById", new { id = book.Id });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error Performing task in {context.GetEndpoint()}");
                    return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
                }
            })
           .WithOpenApi();


            group.MapDelete("{id:int}", async Task<IResult> (int id, IBooksRepository booksRepository, IMapper _mapper, ILogger<BooksApi> logger, HttpContext context) =>
            {
                try
                {
                    var book = await booksRepository.GetAsync(id);
                    if (book == null)
                    {
                        logger.LogWarning($"{nameof(Author)} record not found in {context.GetEndpoint()} - ID: {id}");
                        return TypedResults.NotFound();
                    }

                    await booksRepository.DeleteAsync(id);

                    return TypedResults.NoContent();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error Performing task in {context.GetEndpoint()}");
                    return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
                }
            })
           .RequireAuthorization(new AuthorizeAttribute { Roles = "Administrator" })
           .WithOpenApi();



            return endpointBuilder;
        }

        private partial class BooksApi;
    }
}
