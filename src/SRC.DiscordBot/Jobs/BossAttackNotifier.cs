using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCord.Rest;
using Nixon.Extensions.Hosting.Jobs;
using SRC.DiscordBot.Common;
using SRC.DiscordBot.Services;

namespace SRC.DiscordBot.Jobs;

internal sealed class BossAttackNotifier(
    AppDbContext dbContext, 
    RestClient restClient, 
    KoruxaBossService bossService) : IScheduledJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var currentBoss = await bossService.GetCurrentBossAsync(cancellationToken);
        
        if (currentBoss is null) return;
        
        var now = DateTimeOffset.UtcNow.Add(-KoruxaConstant.AttackReminderDelay);
        
        var usersToNotify = dbContext.User
            .AsEnumerable()
            .Where(x => x.LastAlertSendAt < now)
            .ToList();

        foreach (var user in usersToNotify)
        {
            user.MarkAsNotified();
            
            await dbContext.SaveChangesAsync(cancellationToken);
            
            // await restClient.SendMessageAsync(
            //     KoruxaConstant.KoruxaChannelId,
            //     $"Oi {DiscordUtil.MentionUser(user.DiscordUserId)}. Go fight the boss", 
            //     cancellationToken: cancellationToken);
        }
    }
}