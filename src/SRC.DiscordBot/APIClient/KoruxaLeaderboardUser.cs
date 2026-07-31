namespace SRC.DiscordBot.APIClient;

public sealed record KoruxaLeaderboardUser(
    int Rank,
    int UserId,
    string Username,
    int Level,
    long Xp,
    int Value
);