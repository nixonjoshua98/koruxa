using System;

namespace SRC.DiscordBot;

internal sealed class KoruxaUser
{
    public int Id { get; init; }
    public required ulong DiscordUserId { get; init; }
    public DateTimeOffset LastAlertSendAt { get; private set; }

    public static KoruxaUser CreateNew(ulong discordUserId)
    {
        return new KoruxaUser()
        {
            DiscordUserId = discordUserId,
            LastAlertSendAt = DateTimeOffset.UtcNow
        };
    }
    
    public void MarkAsNotified()
    {
        LastAlertSendAt = DateTimeOffset.UtcNow;
    }
}