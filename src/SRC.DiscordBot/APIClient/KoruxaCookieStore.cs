using System.Threading;
using System.Threading.Tasks;

namespace SRC.DiscordBot.APIClient;

public sealed class KoruxaCookieStore
{
    public Task<string> GetSessionCookieAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult("33e67225065e5e8a7ebb96ea5753375a12b31c5d1bb2c0dc882c2d2bd0804505");
    }
}