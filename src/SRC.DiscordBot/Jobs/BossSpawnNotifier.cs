using System.Threading;
using System.Threading.Tasks;
using NetCord.Rest;
using Nixon.Extensions.Hosting.Jobs;
using Microsoft.Extensions.Logging;

namespace SRC.DiscordBot;

internal sealed class BossSpawnNotifier(
    AppDbContext dbContext, 
    RestClient restClient,
    KoruxaBossService bossService,
    ILogger<BossSpawnNotifier> logger) : IScheduledJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var boss = await bossService.GetCurrentBossAsync(cancellationToken);

        if (boss is null || boss.HasNotifiedSpawn) return;

        boss.MarkSpawnNotified();
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        await restClient.SendMessageAsync(
            KoruxaConstant.KoruxaChannelId,
            "⚔️ Boss is up! Go fight it!",
            cancellationToken: cancellationToken
        );
        
        logger.LogInformation("Announced boss spawn");
    }
}
