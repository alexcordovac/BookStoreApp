using BookStoreApp.API.Configurations;
using BookStoreApp.API.Data;
using BookStoreApp.API.Endpoints;
using BookStoreApp.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("BookStoreDbConnection");
builder.Services.AddDbContext<BookStoreDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<IAuthorsRepository, AuthorsRepository>();
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

var apiGroup = app.MapGroup("/api/v1");
apiGroup.MapAuthorEndpoints();

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
