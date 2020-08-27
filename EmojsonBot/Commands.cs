using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using EmojsonBot.Config;
using EmojsonBot.Helpers;
using Newtonsoft.Json.Linq;

namespace EmojsonBot
{
    public class Commands : BaseCommandModule
    {
        [Command("datapack"), Description("Converts emojis to a datapack format"), RequirePermissions(Permissions.SendMessages)]
        public async Task Convert(CommandContext ctx, [Description("List of emojis")] string emojis)
        {
            EmojiList sEmojiList = new EmojiList();
            sEmojiList.emojis = new List<Emoji>();
            foreach (var s in emojis.Split(">"))
            {
                if (s.Length > 0)
                {
                    Emoji emoji = new Emoji();
                    bool isAnimated = s.StartsWith("<a");
                    emoji.name = s.Substring(s.IndexOf(":") + 1,s.LastIndexOf(":") - s.IndexOf(":") - 1);
                    try
                    {
                        emoji.url = "https://cdn.discordapp.com/emojis/" + s.Substring(s.LastIndexOf(":") +1)+ (isAnimated ? ".gif" : ".png") + "?v=1";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Unicode Emojis don't have an URL - using {emoji.name} instead.");
                        emoji.url = emoji.name;
                    }
                    if (emoji.name.Length > 0) sEmojiList.emojis.Add(emoji);
                }
            }
            string folder = ctx.User.Username.ToLower() + "-emojiful-datapack";
            Directory.CreateDirectory(folder);
            Directory.CreateDirectory(folder+ @"\emojiful");
            Directory.CreateDirectory(folder+ @"\emojiful\data");
            foreach (var emoji in sEmojiList.emojis)
            {
                JsonPrettyPrint.WriteFile(folder+ @"\emojiful\data\" + emoji.name + ".json", new JObject(new JProperty("category", ""), new JProperty("name", emoji.name.ToLower()),new JProperty("url", emoji.url),new JProperty("type", "emojiful:emoji_recipe")));
            }
            JsonPrettyPrint.WriteFile(folder+@"\emojiful\pack.mcmeta", new JObject(new JProperty("pack", new JObject(new JProperty("pack_format", 6), new JProperty("description", "Emojiful emojis!")))));
            ZipFile.CreateFromDirectory(folder, ctx.User.Username.ToLower() + "-emojiful-datapack.zip");
            await ctx.RespondWithFileAsync(ctx.User.Username.ToLower() + "-emojiful-datapack.zip");
            Directory.Delete(folder, true);
            File.Delete(ctx.User.Username.ToLower() + "-emojiful-datapack.zip");
        }
    }
}