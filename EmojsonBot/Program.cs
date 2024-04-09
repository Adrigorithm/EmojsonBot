using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

public class Program
{
    private readonly static DiscordSocketClient _client = new();
    private static Config _config;
    private const string ConfigPath = "Secrets/config.json";

    public static async Task Main()
    {
        _config = await LoadConfigurationAsync();
        _client.Log += Log;

        await _client.LoginAsync(TokenType.Bot, _config.BotToken);
        await _client.StartAsync();

        _client.MessageReceived += MessageReceived;

        await Task.Delay(-1);
    }

    private static async Task MessageReceived(SocketMessage message)
    {
        if (message is SocketUserMessage socketUserMessage 
            && (!(socketUserMessage.Author as SocketGuildUser).GuildPermissions.Administrator || socketUserMessage.Id == _config.DevId)
            && message.MentionedUsers.Any(su => !su.IsBot && (su as SocketGuildUser).GuildPermissions.Administrator))
        {
            var reaction = new Emoji("\uD83D\uDCA2");
            await message.AddReactionAsync(reaction);
        }
    }

    private static Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }

    private static async Task<Config> LoadConfigurationAsync()
    {
        var fileStream = File.OpenRead(ConfigPath);
        return await JsonSerializer.DeserializeAsync<Config>(fileStream);
    }
}