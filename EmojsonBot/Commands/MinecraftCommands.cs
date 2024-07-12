using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord.Interactions;
using EmojsonBot.Enums;

namespace EmojsonBot.Commands;

public class MinecraftCommands : InteractionModuleBase
{
    private readonly Dictionary<MinecraftVersion, string> VersionMapper = new() { { MinecraftVersion.Legacy, "temp/datapack/data/emojiful/recipes" }, { MinecraftVersion.Modern, "temp/datapack/data/emojiful/recipe" } };

    [SlashCommand("datapack", "Discord emojis -> Emojiful datapack!")]
    public async Task GenerateDatapackAsync(InteractionContext ctx, [Summary("minecraft-version")] MinecraftVersion mcVersion, [Summary("category", "Name to group the emoji by in-game")] string category, [Summary("emojis", "List of emoji")] string emojiList, [Summary("hide", "hide this message from public view")] bool isHidden = false)
    {
        var datapackParent = "temp/";
        MatchCollection emojiMatches = Regex.Matches(emojiList, ConstantStrings.EmojiRegex);

        if (emojiMatches.Count > 0)
        {
            foreach (var filePath in Directory.GetFiles(datapackParent, "*?.zip"))
            {
                File.Delete(filePath);
            }

            Directory.Delete(VersionMapper[mcVersion], true);
            Directory.CreateDirectory(VersionMapper[mcVersion]);

            for (var i = 0; i < emojiMatches.Count; i++)
            {
                var emoji = new EmojifulEmoji
                {
                    Category = category,
                    Name = emojiMatches[i].Groups[2].Value,
                    Url = emojiMatches[i].Groups[1].Value.Contains('a') ? "https://cdn.discordapp.com/emojis/" + emojiMatches[i].Groups[3].Value + ".gif" : "https://cdn.discordapp.com/emojis/" + emojiMatches[i].Groups[3].Value + ".png",
                    Type = "emojiful:emoji_recipe"
                };

                FileStream fs = File.Create($"{VersionMapper[mcVersion]}{emojiMatches[i].Groups[2].Value.ToLower()}.json");

                await JsonSerializer.SerializeAsync(fs, emoji);
                await fs.DisposeAsync();
            }

            var fileName = $"{ctx.User.GlobalName ?? "notch"}-{category}-emojiful-datapack.zip";

            ZipFile.CreateFromDirectory(datapackParent + "datapack/", datapackParent + fileName);

            await RespondWithFileAsync(File.OpenRead(datapackParent + fileName), fileName, ephemeral: isHidden);
        }
    }
}