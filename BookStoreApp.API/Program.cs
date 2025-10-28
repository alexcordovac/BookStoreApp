using BookStoreApp.API.Configurations;
using BookStoreApp.API.Contracts;
using BookStoreApp.API.Data;
using BookStoreApp.API.Endpoints;
using BookStoreApp.API.Options;
using BookStoreApp.API.Repositories;
using BookStoreApp.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("BookStoreDbConnection");
builder.Services.Configure<StorageOptions>(builder.Configuration.GetSection(StorageOptions.Storage));

builder.Services.AddDbContext<BookStoreDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentityCore<ApiUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BookStoreDbContext>();

builder.Services.AddTransient<IAuthorsRepository, AuthorsRepository>();
builder.Services.AddTransient<IBooksRepository, BooksRepository>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddAutoMapper(cfg => { cfg.LicenseKey = builder.Configuration["LuckyPennyLicense"]; }, typeof(MapperConfig));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p =>
    {
        p.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});


//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//        options.Authority = "https://your-auth-server"; // e.g. Keycloak, IdentityServer, Auth0
//        options.Audience = "your-api";
//        options.RequireHttpsMetadata = false;
//    });
builder.Services.AddAuthorization();


var app = builder.Build();

var apiV1Group = app.MapGroup("/api/v1");
apiV1Group.MapAuthorEndpoints();
apiV1Group.MapBooksEndpoints();

// Static files
var storageOptions = app.Services.GetRequiredService<IOptions<StorageOptions>>().Value;
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(storageOptions.BookCoversPath),
    RequestPath = "/bookcovers"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.MapTestEndpoints();

app.Run();
