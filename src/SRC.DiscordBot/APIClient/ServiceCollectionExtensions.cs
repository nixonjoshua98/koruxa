using System;
using Microsoft.Extensions.DependencyInjection;

namespace SRC.DiscordBot.APIClient;

public static class ServiceCollectionExtensions
{
    public static void AddKoruxaHttpClient(this IServiceCollection services)
    {
        services.AddTransient<KoruxaCookieStore>();
        
        services.AddHttpClient<KoruxaHttpClient>(client =>
        {
            client.BaseAddress = new Uri("https://koruxa.com/api");
        });
    }
}