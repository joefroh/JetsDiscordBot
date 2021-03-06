# JetsDiscordBot

A bot for the Winnipeg Jets Discord. Written in C# using [Discord.net](https://discord.foxbot.me/docs/) libraries.

This code base SHOULD work on Win/Mac/*nix but I have only tested with Win10.

## Prerequisites

* [DotNet Core](https://www.microsoft.com/net/download)
* [Discord Bot App Token](https://discordapp.com/developers/applications/me)
* (Optional) Visual Studio Code

The repo has configuration files to make building and debugging easy with VS Code, but there is essentially nothing stopping you from using your environment of choice, less convenience.

## Getting Started

To get started with this project, just fork or clone the repo, then open a powershell prompt in the root folder of the repo and run:

``` bash
git submodule -q update --init --recursive
```

That will properly initialize all the submodules (NHLApiDotNet and ClassLocator) before you build the project. You only need to run that once after cloning.

Then restore all the dependencies:


``` bash
dotnet restore
```

To build you can use VS Code or in a powershell prompt run:

``` bash
dotnet build 
```

Once everything is building properly, in the DiscordBot folder, copy `config.json.sample` to `config.json` and fill in your bot token,  command prefix, etc.

After you've filled out the config.json, run 

```bash 
dotnet run
```

from the "DiscordBot" directory and the bot should come online (assuming you've completed the below steps). 

## Discord Bot App Token and Server Integration
You can find this information in more detail on the DiscordAPI and discord.net docs, but a summary: 

To get a token: 
1. Go to https://discordapp.com/developers/applications/ 
2. "Create a new application"
3. "Bot" on the sidebar on the left and click "Add Bot" 
4. Click "Reveal Token" to get/see your token. 
This token should go into the corresponding field in config.json.

To add your bot to a server: 

Take the client ID of your bot (the process for getting this is similar to the above) and add your bot to a server using that (put it in here: https://discordapp.com/oauth2/authorize?client_id=CLIENTIDHERE&scope=bot). 
