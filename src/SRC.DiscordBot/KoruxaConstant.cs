using System;

namespace SRC.DiscordBot;

internal static class KoruxaConstant
{
    public static readonly TimeSpan AttackReminderDelay = TimeSpan.FromHours(4);

    public const ulong KoruxaChannelId = 1532818564472373358;
    
    public static readonly TimeZoneInfo LocalTimeZone = OperatingSystem.IsWindows()
        ? TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")
        : TimeZoneInfo.FindSystemTimeZoneById("America/Chicago");
}
