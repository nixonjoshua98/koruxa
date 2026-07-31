using System.Collections.Generic;

namespace SRC.DiscordBot.APIClient;

public sealed record KoruxaUserLeaderboardResponse(
    IReadOnlyList<KoruxaLeaderboardUser> Entries,
    int Page,
    int TotalPages
);