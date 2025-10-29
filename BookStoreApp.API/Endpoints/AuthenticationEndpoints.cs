using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.User;
using BookStoreApp.API.Services;
using Microsoft.AspNetCore.Identity;

namespace BookStoreApp.API.Endpoints
{
    public static class AuthenticationEndpoints
    {
        public static IEndpointRouteBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder endpointBuilder)
        {
            var group = endpointBuilder.MapGroup("/authentication").WithTags("Authentication");


            group.MapPost("register", async Task<IResult> (UserDto userDto, UserManager<ApiUser> userManager, IMapper mapper, ILogger<AuthenticationApi> logger, HttpContext context) =>
            {
                try
                {
                    var user = mapper.Map<ApiUser>(userDto);
                    user.UserName = userDto.Email;
                    var result = await userManager.CreateAsync(user, userDto.Password);

                    if (result.Succeeded == false)
                    {
                        var errors = result.Errors
                                    .GroupBy(e => e.Code)
                                    .ToDictionary(
                                        g => g.Key,
                                        g => g.Select(e => e.Description).ToArray()
                                    );
                        
                        return Results.ValidationProblem(errors);
                    }

                    await userManager.AddToRoleAsync(user, "User");
                    return Results.Accepted();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error Performing task in {context.GetEndpoint()}");
                    return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
                }
            })
            .WithOpenApi();




            group.MapPost("login", async Task<IResult> (UserDto userDto, UserManager<ApiUser> userManager, JwtTokenService tokenService, ILogger<AuthenticationApi> logger, HttpContext context) =>
            {
                logger.LogInformation($"Login Attempt for {userDto.Email} ");
                try
                {
                    var user = await userManager.FindByEmailAsync(userDto.Email);
                    var passwordValid = await userManager.CheckPasswordAsync(user, userDto.Password);

                    if (user == null || passwordValid == false)
                    {
                        return TypedResults.Unauthorized();
                    }

                    string tokenString = await tokenService.GenerateTokenAsync(user);

                    var response = new AuthResponse
                    {
                        Email = userDto.Email,
                        Token = tokenString,
                        UserId = user.Id,
                    };

                    return TypedResults.Ok(response);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error Performing task in {context.GetEndpoint()}");
                    return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
                }
            })
            .WithOpenApi();


            return endpointBuilder;
        }

        private partial class AuthenticationApi;
    }
}
