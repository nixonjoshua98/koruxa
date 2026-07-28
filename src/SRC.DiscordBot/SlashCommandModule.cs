using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace SRC.DiscordBot;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
internal sealed class SlashCommandModule(KoruxaBossService bossService) : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("attack", "Register your boss attack")]
    public async Task AttackAsync()
    {
        await bossService.AttackAsync(Context.User.Id, CancellationToken.None);
        
        await Context.Interaction.ResponseWithMessageAsync("Time to slap your bosses ass");
    }

    [SlashCommand("killed", "Register the boss death")]
    public async Task KillAsync()
    {
        await bossService.KillBossAsync(CancellationToken.None);

        await Context.Interaction.ResponseWithMessageAsync("Got it, boss is dead");
    }

    [SlashCommand("timer", "Start a boss timer")]
    public async Task TimerAsync(int hours = 0, int minutes = 0, int seconds = 0)
    {
        if (hours == 0 && minutes == 0 && seconds == 0)
        {
            await Context.Interaction.ResponseWithMessageAsync("Please specify a duration!");
            return;
        }

        var endTime = DateTimeOffset.UtcNow
            .AddHours(hours)
            .AddMinutes(minutes)
            .AddSeconds(seconds);

        long unixTime = endTime.ToUnixTimeSeconds();

        await Context.Interaction.ResponseWithMessageAsync($"Boss event started! Ends **<t:{unixTime}:R>**");
    }

    [SlashCommand("say", "Send a message as the bot")]
    public async Task SayAsync(string message)
    {
        await Context.Channel.SendMessageAsync(new MessageProperties
        {
            Content = message
        });

        await Context.Interaction.ResponseWithMessageAsync("Message sent!");
    }
}
