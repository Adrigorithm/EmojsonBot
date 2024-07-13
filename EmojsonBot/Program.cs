using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using EmojsonBot.Secrets;
using Microsoft.Extensions.Configuration;

namespace EmojsonBot;

public class Program
{
    private static DiscordSocketClient _client;
    private static InteractionService _interactionService;
    private static Config _config;

    public static async Task Main()
    {
        IConfiguration env = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            UseInteractionSnowflakeDate = false
        });

        _interactionService = new(_client);
        _config = LoadConfiguration(env);

        _client.Log += Log;

        await _client.LoginAsync(TokenType.Bot, _config.BotToken);
        await _client.StartAsync();

        _client.Ready += ReadyAsync;
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
        await _interactionService.RegisterCommandsGloballyAsync();
    }

    private static Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }

    private static Config LoadConfiguration(IConfiguration env) =>
        new()
        {
            BotToken = env["BOT_TOKEN"],
            DevId = string.IsNullOrEmpty(env["DEV_ID"])
                ? null
                : ulong.Parse(env["DEV_ID"])
        };
}