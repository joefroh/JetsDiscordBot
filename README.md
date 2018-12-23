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

That will properly initialize all the submodules, before you build the project. You only need to run that once after cloning.

Then restore all the dependencies:


``` bash
dotnet restore
```

To build you can use VS Code or in a powershell prompt run:

``` bash
dotnet build
```

Once everything is building properly, in the DiscordBot folder, copy `config.json.sample` to `config.json` and fill in your bot token etc.

## Discord Bot App Token 
To get a token: 

Go to https://discordapp.com/developers/applications/ --> "Create a new application" --> "Bot" on the sidebar on the left --> "Add Bot" --> click "Reveal Token" to get/see your token. 

## Feature Requests

* Team Stats Functions - In Progress
* Team Video Publisher
