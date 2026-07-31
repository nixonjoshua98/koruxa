using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nixon.Extensions.EntityFrameworkCore;
using SRC.DiscordBot.DataModels;

namespace SRC.DiscordBot.Configuration;

internal sealed class DataModelConfiguration :
    IEntityTypeConfiguration<KoruxaBoss>,
    IEntityTypeConfiguration<KoruxaUser>,
    IEntityTypeConfiguration<KoruxaBossAttack>
{
    public void Configure(EntityTypeBuilder<KoruxaBoss> builder)
    {
        builder.ToTable("koruxa_boss");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        
        builder.DefineOneToMany(x => x.Attacks, x => x.BossId);
    }

    public void Configure(EntityTypeBuilder<KoruxaBossAttack> builder)
    {
        builder.ToTable("koruxa_boss_attack");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }

    public void Configure(EntityTypeBuilder<KoruxaUser> builder)
    {
        builder.ToTable("koruxa_user");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasIndex(x => x.DiscordUserId).IsUnique();
    }
}