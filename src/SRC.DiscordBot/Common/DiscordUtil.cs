using System;

namespace SRC.DiscordBot;

internal static class DiscordUtil
{
    public static string MentionUser(ulong id) => $"<@{id}>";
    
    public static string Bold(string text) => $"**{text}**";

    public static string RelativeTime(DateTimeOffset datetime) => $"<t:{datetime.ToUnixTimeSeconds()}:R>";

    public static string LocalTime(DateTimeOffset datetime)
    {
        var localTime = TimeZoneInfo.ConvertTime(datetime, KoruxaConstant.LocalTimeZone);
        
        return $"**{localTime:h:mm tt}**";
    }
    
    public static string Countdown(DateTimeOffset datetime)
    {
        return $"{LocalTime(datetime)} ({Bold(RelativeTime(datetime))})";
    }
}