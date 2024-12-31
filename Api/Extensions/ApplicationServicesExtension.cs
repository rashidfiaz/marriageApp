using Api.Data;
using Api.Helpers;
using Api.Interfaces;
using Api.Services;
using API.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class ApplicationServicesExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection Services, IConfiguration config) {
        Services.AddControllers();
        Services.AddSwaggerGen();
        Services.AddDbContext<DataContext>(opt => 
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        Services.AddCors();

        Services.AddScoped<ITokenService, TokenService>();
        Services.AddScoped<IUserRepository, UserRepository>();
        Services.AddScoped<IPhotoService, PhotoService>();
        Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        Services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

        return Services;
    }
}
