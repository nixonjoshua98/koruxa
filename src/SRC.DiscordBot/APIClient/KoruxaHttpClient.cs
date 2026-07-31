using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SRC.DiscordBot.APIClient;

public sealed class KoruxaHttpClient(
    HttpClient httpClient,
    KoruxaCookieStore cookieStore)
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };
    
    public async Task<KoruxaUserLeaderboardResponse> GetLeaderboardPageAsync(
        KoruxaLeaderboardCategory category,
        KoruxaSkill skill,
        int page,
        CancellationToken cancellationToken = default)
    {
        var cookie = await cookieStore.GetSessionCookieAsync(cancellationToken);
        
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/leaderboard?" +
            $"category={category.ToString().ToLower()}&" +
            $"mode=all&" +
            $"page={page}&" +
            $"skill={skill.ToString().ToLower()}"
        );
        
        request.Headers.Add(
            "Cookie",
            $"koruxa_go_sid={cookie}; " +
            $"koruxa_go_sid_n={cookie}"
        );

        using var response = await httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        return await ReadFromResponseAsync<KoruxaUserLeaderboardResponse>(response)
            ?? throw new Exception("Failed loading leaderboard");
    }

    private async Task<T?> ReadFromResponseAsync<T>(HttpResponseMessage response) where T : class
    {
        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(content, _serializerOptions);
    }
}