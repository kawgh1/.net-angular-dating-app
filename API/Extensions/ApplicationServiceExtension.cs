using System;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        // DB Connection
        services.AddDbContext<DatabaseContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        // Add CORS
        services.AddCors();

        // Add Services
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        // AutoMapper ORM
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        // Cloudinary
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
        // Photo Service using Cloudinary
        services.AddScoped<IPhotoService, PhotoService>();

        return services;
    }
}