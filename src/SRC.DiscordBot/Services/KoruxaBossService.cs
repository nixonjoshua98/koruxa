using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SRC.DiscordBot.DataModels;

namespace SRC.DiscordBot;

internal sealed class KoruxaBossService(AppDbContext dbContext)
{
    public async Task AttackAsync(ulong discordUserId, CancellationToken cancellationToken)
    {
        _ = await GetOrAddUserAsync(discordUserId, cancellationToken);
        
        var boss = await GetCurrentBossAsync(cancellationToken)
            ?? throw new InvalidOperationException("No boss found, it needs to be marked as killed");
        
        var attack = KoruxaBossAttack.CreateNew(discordUserId);

        boss.Attacks.Add(attack);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ScheduleBossAsync(DateTimeOffset scheduledAt, CancellationToken cancellationToken)
    {
        var pendingScheduledBosses = dbContext.Boss
            .AsEnumerable()
            .Where(x => x.CreatedAt > DateTimeOffset.UtcNow)
            .ToList();

        if (pendingScheduledBosses.Count > 0)
        {
            dbContext.Boss.RemoveRange(pendingScheduledBosses);
        }

        var boss = KoruxaBoss.CreateNew(scheduledAt);
        
        dbContext.Boss.Add(boss);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task KillBossAsync(CancellationToken cancellationToken)
    {
        var boss = dbContext.Boss
            .AsEnumerable()
            .SingleOrDefault(x => !x.KilledAt.HasValue && x.CreatedAt <= DateTimeOffset.UtcNow);
        
        if (boss is null) return;

        boss.MarkAsKilled(DateTimeOffset.UtcNow);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<KoruxaUser> GetOrAddUserAsync(ulong discordUserId, CancellationToken cancellationToken)
    {
        var user = await dbContext.User
            .SingleOrDefaultAsync(x => x.DiscordUserId == discordUserId, cancellationToken);
        
        if (user is not null) return user;
        
        user = KoruxaUser.CreateNew(discordUserId);
        
        dbContext.User.Add(user);
        
        return user;
    }

    public Task<KoruxaBoss?> GetCurrentBossAsync(CancellationToken cancellationToken)
    {
        var boss = dbContext.Boss
            .AsEnumerable()
            .FirstOrDefault(x => !x.KilledAt.HasValue && 
                                  x.CreatedAt <= DateTimeOffset.UtcNow);
        
        return Task.FromResult(boss);
    }
}