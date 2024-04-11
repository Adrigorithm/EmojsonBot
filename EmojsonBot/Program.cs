using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
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

        _client.Ready += ReadyAsync;
        _client.SlashCommandExecuted += SlashCommandExecutedAsync;
        _client.MessageReceived += MessageReceivedAsync;

        await Task.Delay(-1);
    }

    private static async Task SlashCommandExecutedAsync(SocketSlashCommand command)
    {
        var cmdName = command.Data.Name;

        switch (cmdName)
        {
            // case "datapack":
            //     await command.RespondAsync($"Slashcommand `{cmdName}` has no implementation yet");
            //     break;
            default:
                await command.RespondAsync($"Slashcommand `{cmdName}` has no implementation yet");
                break;
        }
    }

    private static async Task ReadyAsync()
    {
        if ((bool)!_config.ReloadGlobalCommands)
            return;

        var globalCommand = new SlashCommandBuilder();
        globalCommand.WithName("datapack");
        globalCommand.WithDescription("Discord emojis -> Emojiful datapack!");
        globalCommand.WithDefaultMemberPermissions(GuildPermission.SendMessages);

        SlashCommandOptionBuilder categoryOption = new()
        {
            Name = "category",
            Type = ApplicationCommandOptionType.String,
            Description = "a name to group the emoji by in-game",
            IsRequired = true,
        };

        SlashCommandOptionBuilder emojisOption = new()
        {
            Name = "emojis",
            Type = ApplicationCommandOptionType.String,
            Description = "a list of emojis",
            IsRequired = true,
        };

        globalCommand.AddOptions(categoryOption, emojisOption);

        try
        {
            await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
            _config.ReloadGlobalCommands = false;
            await SaveConfigurationAsync();
        }
        catch (HttpException exception)
        {
            Console.WriteLine(exception.Message);
        }
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

    private static async Task SaveConfigurationAsync()
    {
        var fileStream = File.OpenWrite(ConfigPath);
        await JsonSerializer.SerializeAsync(fileStream, _config);
    }
}