# [Download](https://github.com/shavitush/shave/releases/latest)

### Build status
[![Winows Build status](https://ci.appveyor.com/api/projects/status/5mp336mkterpsam6?svg=true)](https://ci.appveyor.com/project/shavitush/shave) [![Mono Build status](https://travis-ci.org/shavitush/shave.svg?branch=master)](https://travis-ci.org/shavitush/shave)

# shave (C#)
A cross-platform Discord bot based on [Discord.Net](https://github.com/RogueException/Discord.Net).  
May want to build with Mono to run on Linux.
___

Made for educational purposes, made open-source so I can get Travis and AppVeyor for free.

No support will be given for now, at least until it's very functional (or useful).

Depends on
--
* .NET Framework 4.6 (or Mono)
* Discord.Net
* Newtonsoft.Json

All of those are obtainable from NuGet.

___

Structure
--
* `ChatBot.cs` - Connects to Discord, hooks chat messages and generates a bot invite link.
* `ChatCommands.cs` - Handles chat commands in a way that is *really easy to manage*, inspired by [SourceMod](https://sourcemod.net/)'s way of registering commands. Also has a few basic commands like `clean`. Includes `help` which prints all of the existing commands.
* `Command.cs` - The commands class. Includes derived classes for system commands (console commands) and chat commands.
* `Config.cs` - Configuration class, has a save method and is loaded in a separate file.
* `General.cs` - Some "constant" settings. Such as the application's name, author's name, console prefixes or the application's version.
* `Input.cs` - A separate thread which handles console input and passes it to `SystemCommands`.
* `Program.cs` - Entry point for the program. Launches threads, has the configuration file object and also loads it.
* `SystemCommands.cs` - Includes a few basic console commands, write `help` to list them.

License
--
GNU GPL v3, see **[LICENSE](https://github.com/shavitush/shave/blob/master/LICENSE)**.
