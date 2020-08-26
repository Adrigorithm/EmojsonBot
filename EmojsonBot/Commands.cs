using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using EmojsonBot.Config;
using EmojsonBot.Helpers;

namespace EmojsonBot
{
    public class Commands : BaseCommandModule
    {
        [Command("tojson"), Description("Converts emojis to a JSON format"), RequirePermissions(Permissions.SendMessages)]
        public async Task Convert(CommandContext ctx, [Description("Separated List of emojis (use spaces)")] params DiscordEmoji[] emojis)
        {
            EmojiList sEmojiList = new EmojiList();
            sEmojiList.emojis = new List<Emoji>();
            
            foreach (var emoji in emojis)
            {
                Emoji sEmoji = new Emoji();
                sEmoji.name = emoji.Name;
                try
                {
                    sEmoji.url = emoji.Url;
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine($"Unicode Emojis don't have an URL - using {sEmoji.name} instead.");
                    sEmoji.url = emoji.Name;
                }
                
                sEmojiList.emojis.Add(sEmoji);
            }
            
            await ctx.RespondAsync($"```json\n{JsonPrettyPrint.JsonPP(sEmojiList.toJson())}```");
        }
    }
}