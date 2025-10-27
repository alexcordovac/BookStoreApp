using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Endpoints
{
    public static class AuthorEndpoints
    {

        public static IEndpointRouteBuilder MapAuthorEndpoints(this IEndpointRouteBuilder endpointBuilder)
        {
            var group = endpointBuilder.MapGroup("/authors");


            group.MapGet("{id:int}", async Task<IResult> (int id, HttpContext context, IAuthorsRepository authorsRepository, ILogger<AuthorApi> logger) =>
            {
                try
                {
                    var author = await authorsRepository.GetAuthorDetailsAsync(id);
                    if (author == null)
                    {
                        logger.LogWarning($"Record Not Found: {nameof(Author)} - ID: {id}");
                        return TypedResults.NotFound();
                    }
                    return TypedResults.Ok(author);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error Performing task in {context.GetEndpoint()}");
                    return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
                }
            })
            .WithName("getAuthorById")
            .WithOpenApi();


            group.MapGet("", async Task<IResult> ([AsParameters] QueryParameters queryParameters, HttpContext context, IAuthorsRepository authorsRepository, ILogger<AuthorApi> logger) =>
            {
                try
                {
                    var authorsList = await authorsRepository.GetAllAsync<AuthorReadOnlyDto>(queryParameters);
                    return TypedResults.Ok(authorsList);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error Performing task in {context.GetEndpoint()}");
                    return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
                }
            })
           .WithOpenApi();


            group.MapPut("{id:int}", async Task<IResult> (int id, AuthorUpdateDto authorDto, 
                HttpContext context, 
                IAuthorsRepository authorsRepository, 
                IMapper _mapper, 
                ILogger<AuthorApi> logger) =>
            {
                if (id != authorDto.Id)
                {
                    logger.LogWarning($"Update ID invalid in {context.GetEndpoint()} - ID: {id}");
                    return TypedResults.BadRequest();
                }

                var author = await authorsRepository.GetAsync(id);

                if (author == null)
                {
                    logger.LogWarning($"{nameof(Author)} record not found in {context.GetEndpoint()} - ID: {id}");
                    return TypedResults.NotFound();
                }

                _mapper.Map(authorDto, author);

                try
                {
                    await authorsRepository.UpdateAsync(author);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!await authorsRepository.Exists(id))
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



            group.MapPost("", async Task<IResult> (AuthorCreateDto authorDto, HttpContext context, IAuthorsRepository authorsRepository, IMapper _mapper, ILogger<AuthorApi> logger) =>
            {
                try
                {
                    var author = _mapper.Map<Author>(authorDto);
                    await authorsRepository.AddAsync(author);

                    return TypedResults.CreatedAtRoute(author, "getAuthorById", new { id = author.Id });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error Performing task in {context.GetEndpoint()}");
                    return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
                }
            })
           .WithOpenApi();


            group.MapDelete("{id:int}", async Task<IResult> (int id, IAuthorsRepository authorsRepository, IMapper _mapper, ILogger<AuthorApi> logger, HttpContext context) =>
            {
                try
                {
                    var author = await authorsRepository.GetAsync(id);
                    if (author == null)
                    {
                        logger.LogWarning($"{nameof(Author)} record not found in {context.GetEndpoint()} - ID: {id}");
                        return TypedResults.NotFound();
                    }

                    await authorsRepository.DeleteAsync(id);

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
        private partial class AuthorApi;
    }

}
