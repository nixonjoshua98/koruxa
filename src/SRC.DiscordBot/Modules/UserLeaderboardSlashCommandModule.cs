using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NetCord.Services.ApplicationCommands;
using SRC.DiscordBot.APIClient;
using SRC.DiscordBot.Common;
using SRC.DiscordBot.Extensions;

namespace SRC.DiscordBot.Modules;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
internal sealed class UserLeaderboardSlashCommandModule(KoruxaHttpClient httpClient) : 
    ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("lb", "Show user leaderboard")]
    public async Task ShowUserLeaderboardAsync()
    {
        var response = await httpClient.GetLeaderboardPageAsync(
            KoruxaLeaderboardCategory.Total,
            KoruxaSkill.Total,
            1
        );

        var block = new DiscordCodeBlockTable([3, null, null])
            .AddRows(
                response.Entries,
                user => [$"#{user.Rank:D2}", user.Username, user.Value.ToString()]
            );
        
        await Context.Interaction.RespondWithMessageAsync(block.ToString());
    }
}