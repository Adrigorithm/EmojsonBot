# What is this?
A discord bot that translates discord emoji into json files following a format that the minecraft mod [Emojiful](https://www.curseforge.com/minecraft/mc-mods/emojiful) can understand.

# Deployment
First create a bot on the discord dev platform. Under the OAuth2 section, it needs the `bot` and `applications.commands` scopes. 
All it does is respond to a slash command with a file using interactions so a minimal permission set like this should do (Read Messages/View Channels is possibly needed but I don't think so 🙃): 
![image](https://github.com/Adrigorithm/EmojsonBot/assets/12832161/5c98e8ea-5c9e-4cc8-a8dd-e2de53601707)


While there are quite [a few ways](https://learn.microsoft.com/en-us/dotnet/core/deploying/) to deploy this app the most simple way is as follows:
- Create EmojsonBot/Secrets/config.json with the following contents (devId is not strictly necessary, it is my id in this case (leave it on 0 or something if you want) it is for testing):
```json
{
    "botToken": "a token generated from the Bot tab on the discord dev platform",
    "devId": 135081249017430016,
    "reloadGlobalCommands": true
}
```
- Make sure the machine you host it on has a dotnet8 runtime
- The Discord.NET framework that is used by this bot requires a synchronized time (this may not be needed anymore later). On GNU/Linux systems with systemd you can sync with a ntp server with one command: `sudo timedatectl set-ntp true`
- `dotnet deploy`
- `dotnet <outputDll.dll>`
