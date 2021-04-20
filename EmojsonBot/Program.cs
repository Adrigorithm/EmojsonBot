using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EmojsonBot
{
    class Program
    {
        private static DiscordClient _discord;
        private static CommandsNextExtension _commands;
        
        static async Task Main(string[] args)
        {
            // Load config.json
            string json = await GetConfigJson();
            var cfgJson = JsonConvert.DeserializeObject<ConfigJson>(json);
            
            // Setup DiscordClient
            _discord = new DiscordClient(new DiscordConfiguration
            {
                Token = cfgJson.Token
            });

            // Attach CommandsNext module
            _commands = _discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new string[]{cfgJson.CommandPrefix},
                EnableMentionPrefix = false
            });
            
            // Register commands
            _commands.RegisterCommands<Commands>();

            // Event handlers
            _discord.MessageCreated += MessageCreated;
            
            // Infinite Task
            await _discord.ConnectAsync(activity: new DiscordActivity("cat GIFs", ActivityType.Watching));
            await Task.Delay(-1);
        }

        private static async Task MessageCreated(DiscordClient cl
            ,MessageCreateEventArgs e)
        {
            foreach(var user in e.MentionedUsers)
            {
                if (!user.Id.Equals(135081249017430016) &&
                    (!user.Id.Equals(96921693489995776) || e.Author.Id == 608275633218519060)) continue;
                try
                {
                    await e.Message.CreateReactionAsync(DiscordEmoji.FromName(_discord, ":catree:"));
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Specified emoji not found, using a vanilla one :(");
                    await e.Message.CreateReactionAsync(DiscordEmoji.FromName(_discord, ":anger:"));
                }
            }
        }

        private static async Task<string> GetConfigJson()
        {
            using var sr = File.OpenText("config.json");
            return await sr.ReadToEndAsync();
        }
        
    }
    
    public struct ConfigJson
    {
        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonProperty("prefix")]
        public string CommandPrefix { get; private set; }
    }
}