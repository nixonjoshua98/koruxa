using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NetCord;
using NetCord.Services.ApplicationCommands;
using SRC.DiscordBot.Common;
using SRC.DiscordBot.Components;
using SRC.DiscordBot.Extensions;
using SRC.DiscordBot.Services;

namespace SRC.DiscordBot.Modules;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
internal class BossTimerSlashCommandModule(KoruxaBossService bossService) : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("attack", "Register your boss attack")]
    public async Task AttackAsync()
    {
        await bossService.AttackAsync(Context.User.Id, CancellationToken.None);
        
        await Context.Interaction.RespondWithMessageAsync("Time to slap your bosses ass");
    }

    [SlashCommand("killed", "Register the boss death")]
    public async Task KillAsync()
    {
        await bossService.KillBossAsync(CancellationToken.None);

        await Context.Interaction.RespondWithMessageAsync("Got it, boss is dead");
    }

    [SlashCommand("timer", "Start a boss timer")]
    public async Task TimerAsync(int hours = 0, int minutes = 0, int seconds = 0)
    {
        if (hours == 0 && minutes == 0 && seconds == 0)
        {
            await Context.Interaction.RespondWithMessageAsync("Please specify a duration!");
            return;
        }

        var spawnAt = DateTimeOffset.UtcNow
            .AddHours(hours)
            .AddMinutes(minutes)
            .AddSeconds(seconds);

        await bossService.ScheduleBossAsync(spawnAt, CancellationToken.None);

        await Context.Interaction.RespondWithMessageAsync(
            $"Boss spawn scheduled for {DiscordUtil.Countdown(spawnAt)}",
            components: [BossTimerActionRow.CreateNew()]
        );
    }

    [SlashCommand("say", "Send a message as the bot")]
    public async Task SayAsync(string message)
    {
        await Context.Channel.SendMessageAsync(message);

        await Context.Interaction.RespondWithMessageAsync(
            "Message sent!",
            flags: MessageFlags.Ephemeral
        );
    }
}