using System.Collections.Generic;
using System.Threading.Tasks;
using NetCord;
using NetCord.Rest;

namespace SRC.DiscordBot.Extensions;

public static class NetCordExtensions
{
    public static async Task RespondWithMessageAsync(
        this Interaction interaction, 
        string content,
        MessageFlags? flags = null,
        IEnumerable<IMessageComponentProperties>? components = null)
    {
        await interaction.SendResponseAsync(
            InteractionCallback.Message(
                new InteractionMessageProperties()
                    .WithContent(content)
                    .WithFlags(flags)
                    .WithComponents(components)
            )
        );
    }
    
    public static async Task RespondWithModalAsync(
        this Interaction interaction, 
        ModalProperties modalProperties)
    {
        await interaction.SendResponseAsync(
            InteractionCallback.Modal(modalProperties)
        );
    }
    
    public static async Task RespondWithModifyMessageAsync(
        this Interaction interaction, 
        string? content,
        IEnumerable<IMessageComponentProperties> components)
    {
        await interaction.SendResponseAsync(
            InteractionCallback.ModifyMessage(msg =>
            {
                msg.Content = content;
                msg.Components = components;
            })
        );
    }
}