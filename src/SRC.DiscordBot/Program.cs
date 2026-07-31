using SRC.DiscordBot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Hosting.Services.ComponentInteractions;
using Nixon.Extensions.Hosting.Jobs;
using Nixon.Extensions.Serilog.AspNetCore;
using SRC.DiscordBot.APIClient;
using SRC.DiscordBot.Jobs;
using SRC.DiscordBot.Modules;
using SRC.DiscordBot.Services;

var builder = Host.CreateApplicationBuilder(args)
    .AddSerilogConfiguration();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddDiscordGateway(options =>
{
    options.Token = builder.Configuration["BOT_TOKEN"];
});

builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
    
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddKoruxaHttpClient();
builder.Services.AddApplicationCommands();
builder.Services.AddComponentInteractions();

builder.Services.AddCronJob<BossAttackNotifier>("0 * * * * ?");
builder.Services.AddCronJob<BossSpawnNotifier>("0 * * * * ?");

builder.Services.TryAddScoped<KoruxaBossService>();

var host = builder.Build();

host.AddApplicationCommandModule<UserLeaderboardSlashCommandModule>();
host.AddApplicationCommandModule<BossTimerSlashCommandModule>();

host.AddComponentInteractionModule<BossTimerComponentModule>();

using (var scope = host.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
}

await host.RunAsync();