using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using EmojsonBot.Config;
using EmojsonBot.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EmojsonBot
{
    public class Commands : BaseCommandModule
    {
        private const string CatToken = "718a65df-e519-4117-bb8b-774c4242f49a";
        private HttpClient _catClient;
        private HttpClient _foxClient;

        public Commands()
        {
            SetupApis();
        }

        private void SetupApis()
        {
            _catClient = new HttpClient();
            _catClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("x-api-key", CatToken);
            _foxClient = new HttpClient();
        }

        [Command("datapack"), Description("Converts emojis to a datapack format"),
         RequirePermissions(Permissions.SendMessages)]
        public async Task Convert(CommandContext ctx, [Description("A category for the list of emojis")]
            string category, [Description("List of emojis")] params string[] emojis)
        {
            EmojiList sEmojiList = new EmojiList();
            sEmojiList.emojis = new List<Emoji>();
            foreach (var s in string.Join("", emojis).Split(">"))
            {
                if (s.Length > 0)
                {
                    Emoji emoji = new Emoji();
                    bool isAnimated = s.StartsWith("<a");
                    emoji.name = s.Substring(s.IndexOf(":") + 1, s.LastIndexOf(":") - s.IndexOf(":") - 1);
                    try
                    {
                        emoji.url = "https://cdn.discordapp.com/emojis/" + s.Substring(s.LastIndexOf(":") + 1) +
                                    (isAnimated ? ".gif" : ".png") + "?v=1";
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
            Directory.CreateDirectory(folder + @"/emojiful");
            Directory.CreateDirectory(folder + @"/emojiful/data");
            foreach (var emoji in sEmojiList.emojis)
            {
                JsonPrettyPrint.WriteFile(folder + @"/emojiful/data/" + emoji.name + ".json",
                    new JObject(new JProperty("category", category), new JProperty("name", emoji.name.ToLower()),
                        new JProperty("url", emoji.url), new JProperty("type", "emojiful:emoji_recipe")));
            }

            JsonPrettyPrint.WriteFile(folder + @"/emojiful/pack.mcmeta",
                new JObject(new JProperty("pack",
                    new JObject(new JProperty("pack_format", 6), new JProperty("description", "Emojiful emojis!")))));
            ZipFile.CreateFromDirectory(folder,
                ctx.User.Username.ToLower() + "-" + category + "-emojiful-datapack.zip");
            await ctx.RespondWithFileAsync(ctx.User.Username.ToLower() + "-" + category + "-emojiful-datapack.zip");
            Directory.Delete(folder, true);
            File.Delete(ctx.User.Username.ToLower() + "-" + category + "-emojiful-datapack.zip");
        }

        [Command("cat"), Description("Gets a random cat"),
         RequirePermissions(Permissions.SendMessages)]
        public async Task Cat(CommandContext ctx)
        {
            string catJson = await _catClient.GetStringAsync("https://api.thecatapi.com/v1/images/search");
            var catUrl = JsonConvert.DeserializeObject<List<dynamic>>(catJson)[0].url;

            string[] titles =
                {"Cute floof!", "Found one!", "Cat.", "Cat = Life", "Daily cats!", "Cuteness overload!", "Kitty"};
            var random = new Random();

            // Load cat in embed
            var catImageBuilder = new DiscordEmbedBuilder();
            catImageBuilder.WithTitle(titles[random.Next(0, titles.Length)]);
            catImageBuilder.WithImageUrl(catUrl.Value);

            var catEmbed = catImageBuilder.Build();

            await ctx.RespondAsync(null, false, catEmbed);
        }

        [Command("fox"), Description("Gets a random fox"),
         RequirePermissions(Permissions.SendMessages)]
        public async Task Fox(CommandContext ctx)
        {
            var foxJson = await _foxClient.GetStringAsync("https://randomfox.ca/floof/");
            var foxUrl = JsonConvert.DeserializeObject<dynamic>(foxJson).image;

            string[] titles =
                {"Cute floof!", "Found one!", "Fox.", "Fox = Life", "Daily Foxes!", "Cuteness overload!", "Foxie!"};
            var random = new Random();

            // Load cat in embed
            var foxImageBuilder = new DiscordEmbedBuilder();
            foxImageBuilder.WithTitle(titles[random.Next(0, titles.Length)]);
            foxImageBuilder.WithImageUrl(foxUrl.Value);

            var foxEmbed = foxImageBuilder.Build();

            await ctx.RespondAsync(null, false, foxEmbed);
        }
    }
}