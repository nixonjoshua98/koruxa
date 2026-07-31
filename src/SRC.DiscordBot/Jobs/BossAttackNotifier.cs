using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCord.Rest;
using Nixon.Extensions.Hosting.Jobs;

namespace SRC.DiscordBot;

internal sealed class BossAttackNotifier(AppDbContext dbContext, RestClient restClient) : IScheduledJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var currentBoss = await dbContext.Boss
            .SingleOrDefaultAsync(x => !x.KilledAt.HasValue, cancellationToken);
        
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
            
            await restClient.SendMessageAsync(
                KoruxaConstant.KoruxaChannelId,
                $"Oi {DiscordUtil.MentionUser(user.DiscordUserId)}. Go fight the boss", 
                cancellationToken: cancellationToken);
        }
    }
}