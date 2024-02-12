using Application.Interfaces;
using Application.Interfaces.Mailing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WebDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("AppConnectionString")));

        services.AddScoped<IWebDbContext>(provider => provider.GetRequiredService<WebDbContext>());

        services.AddScoped<IMailSender, MailSenderMailKit>();
        return services;
    }
}
