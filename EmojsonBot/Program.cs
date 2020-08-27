using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
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
                Token = cfgJson.Token,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
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
            
            
            // Infinite Task
            await _discord.ConnectAsync(activity: new DiscordActivity("cat GIFs", ActivityType.Watching));
            await Task.Delay(-1);
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