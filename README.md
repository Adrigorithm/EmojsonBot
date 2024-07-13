# What is this?
A discord bot that translates discord emoji into json files following a format that the minecraft mod [Emojiful](https://www.curseforge.com/minecraft/mc-mods/emojiful) can understand.

## Deployment
First create a bot on the discord dev platform. Under the OAuth2 section, it needs the `bot` and `applications.commands` scopes. 
All it does is respond to a slash command with a file using interactions so a minimal permission set like this should do (I think): 
![image](https://github.com/Adrigorithm/EmojsonBot/assets/12832161/5c98e8ea-5c9e-4cc8-a8dd-e2de53601707)


### Now to build and deploy the app:
- Install the `docker engine` (the CLI tool should come with it). Docker desktop or docker compose are not required.
- `clone` the repository
- `cd` into the it :3 (you should be in the same directory as the `Dockerfile`)
- `docker build -t adrigorithm/emojson-bot`
- `docker run --name emojson-bot -e BOT_TOKEN=YourBotTokenHere -e DEV_ID=135081249017430016 adrigorithm/emojson-bot`

Note that in `docker build -t adrigorithm/emojson-bot`, the value for `-t` doesn't much matter, but it should be in the format author/imagename and eventually a tag after that. In `docker run --name emojson-bot -e BOT_TOKEN=YourBotTokenHere -e DEV_ID=135081249017430016 adrigorithm/emojson-bot` the DEV_ID environment variable is not needed as it does not do anything, it is perhaps for testing purposes in the future.

### Without docker
If you don't want to use docker you can reverse engineer the Dockerfile and manually do the steps, it should be easy :^)
