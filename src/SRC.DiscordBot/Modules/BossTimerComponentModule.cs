using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NetCord;
using NetCord.Services.ComponentInteractions;
using SRC.DiscordBot.Common;
using SRC.DiscordBot.Components;
using SRC.DiscordBot.Extensions;
using SRC.DiscordBot.Services;

namespace SRC.DiscordBot.Modules;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
internal sealed class BossTimerComponentModule(KoruxaBossService bossService) : ComponentInteractionModule<ComponentInteractionContext>
{
    [ComponentInteraction(BossTimerActionRow.ResetButtonCustomId)]
    public async Task Reset12HAsync()
    {
        var newSpawnAt = DateTimeOffset.UtcNow.AddHours(11).AddMinutes(59);

        await bossService.ScheduleBossAsync(newSpawnAt, CancellationToken.None);

        await Context.Interaction.RespondWithModifyMessageAsync(
            $"💀 Boss defeated by {Context.User.Username}! " +
            $"Next spawn ETA: {DiscordUtil.Countdown(newSpawnAt)}",
            [BossTimerActionRow.CreateNew()]
        );
    }

    [ComponentInteraction(BossTimerActionRow.OpenTimerButtonCustomId)]
    public async Task OpenCustomModalAsync()
    {
        await Context.Interaction.RespondWithModalAsync(
            BossTimerModal.CreateModal()
        );
    }
    
    [ComponentInteraction(BossTimerModal.CustomId)]
    public async Task SubmitCustomTimerAsync()
    {
        var parsed = BossTimerModal.Parse((ModalInteraction)Context.Interaction);

        if (parsed is { Hours: 0, Minutes: 0 })
        {
            await Context.Interaction.RespondWithMessageAsync(
                "Invalid duration entered!",
                MessageFlags.Ephemeral
            );
            return;
        }

        var newSpawnAt = DateTimeOffset.UtcNow.Add(parsed.TimeSpan);

        await bossService.ScheduleBossAsync(newSpawnAt, CancellationToken.None);

        await Context.Interaction.RespondWithModifyMessageAsync(
            $"⏳ Timer updated by {Context.User.Username}! " +
            $"Next spawn ETA: {DiscordUtil.Countdown(newSpawnAt)}",
            [BossTimerActionRow.CreateNew()]
        );
    }
}