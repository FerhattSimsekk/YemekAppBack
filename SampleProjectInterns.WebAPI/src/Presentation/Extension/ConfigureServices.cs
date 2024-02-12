using Microsoft.AspNetCore.Authorization;
using SampleProjectInterns.WebAPI.Presentation.Middlewares;
using SampleProjectInterns.WebAPI.Presentation.Services;
using SampleProjectInterns.WebAPI.Presentation.Settings;
using Infrastructure.Settings;
using Application.Interfaces;
using Application.Settings;
using Presentation.Services;
using Microsoft.VisualStudio.Web.CodeGeneration;

namespace SampleProjectInterns.WebAPI.Presentation.Extension;

public static class ConfigureServices
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddScoped<ExceptionMiddleware>();
        services.AddSingleton<JwtService>();
        services.AddScoped<IAuthorizationHandler, VerificationRequirementHandler>();
        services.AddSingleton<IFileSystem, DefaultFileSystem>();

        return services;
    }

    public static IServiceCollection AddPresentationSettings(
        this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(nameof(JwtSettings)))
            .Validate(s => s.Validate());

        services.AddOptions<MailServerSettings>()
            .Bind(configuration.GetSection(nameof(MailServerSettings)));

        services.AddOptions<StorageSettings>()
            .Bind(configuration.GetSection(nameof(StorageSettings)));

        return services;
    }
    public static IServiceCollection AddFileSystemStorage(this IServiceCollection services, IWebHostEnvironment? hostEnvironment = null)
    {
        services.AddSingleton<IStorageProvider, FileSystemStorageProvider>();

        if (hostEnvironment != null)
        {
            services.PostConfigure<StorageSettings>(options =>
            {
                if (options.UseWebRootPath)
                {
                    options.Path = $"{hostEnvironment.WebRootPath}/{options.Path}";
                }
            });
        }

        return services;
    }
}