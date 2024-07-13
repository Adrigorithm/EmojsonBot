using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using EmojsonBot.Commands;

public class Program
{
    private static DiscordSocketClient _client;
    private static InteractionService _interactionService;
    private static Config _config;
    private const string ConfigPath = "../../../Secrets/config.json";

    public static async Task Main()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            UseInteractionSnowflakeDate = false
        });
        
        _interactionService = new(_client);
        _config = await LoadConfigurationAsync();
        _client.Log += Log;

        await _client.LoginAsync(TokenType.Bot, _config.BotToken);
        await _client.StartAsync();

        _client.Ready += ReadyAsync;
        _client.MessageReceived += MessageReceivedAsync;
        _client.InteractionCreated += InteractionCreatedAsync;

        await Task.Delay(-1);
    }

    private static async Task InteractionCreatedAsync(SocketInteraction interaction)
    {
        var ctx = new SocketInteractionContext(_client, interaction);
        await _interactionService.ExecuteCommandAsync(ctx, null);
    }

    private static async Task ReadyAsync()
    {
        await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        await _interactionService.RegisterCommandsToGuildAsync(574341132826312736);
    }

    private static async Task MessageReceivedAsync(SocketMessage message)
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