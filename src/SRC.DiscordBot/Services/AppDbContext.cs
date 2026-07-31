using Microsoft.EntityFrameworkCore;
using SRC.DiscordBot.DataModels;

namespace SRC.DiscordBot;

internal sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<KoruxaBoss> Boss => Set<KoruxaBoss>();
    public DbSet<KoruxaUser> User => Set<KoruxaUser>();
    public DbSet<KoruxaBossAttack> BossAttack => Set<KoruxaBossAttack>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}