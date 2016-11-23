using System;
using System.Collections.Generic;
using System.Linq;
using Timer = System.Timers.Timer;
using System.IO;

namespace shave
{
    internal static class SystemCommands
    {
        private static readonly List<SystemCommand> CommandsList = new List<SystemCommand>();

        private static void AddCommand(string trigger, SystemCommand.OnSystemCommand ontrigger, string description = null)
        {
            var command = new SystemCommand
            {
                Trigger = trigger,
                Description = description
            };

            command.OnTrigger += ontrigger;
            CommandsList.Add(command);
        }

        public static bool TriggerCommand(string command)
        {
            var cmd = command.Split(new[] { ' ' }, 2);

            if (cmd.Length == 0)
            {
                return false;
            }

            var found = CommandsList.FirstOrDefault(x => x.Trigger == cmd[0]);

            if (found == null)
            {
                return false;
            }

            found.TriggerCommand(cmd.Length == 2 ? cmd[1] : null);

            return true;
        }

        public static void AddHelpCommands()
        {
            AddCommand("help", OnHelpCommand, "Displays the commands menu.");
            AddCommand("commands", OnHelpCommand, "Displays the commands menu.");
            AddCommand("clear", OnClearCommand, "Clears the console.");
            AddCommand("q", OnQuitCommand, "Exits the program safely.");
            AddCommand("quit", OnQuitCommand, "Exits the program safely.");
            AddCommand("exit", OnQuitCommand, "Exits the program safely.");
            AddCommand("invite", OnInviteCommand, "Returns an invite link.");
            AddCommand("prefix", OnPrefixCommand, "Changes the chat bot's command prefix.");
            AddCommand("suffix", OnSuffixCommand, "Changes the chat bot's command suffix.");
            AddCommand("game", OnGameCommand, "Changes the bot's current playing game on Discord.");
            AddCommand("setgame", OnGameCommand, "Changes the bot's current playing game on Discord.");
            AddCommand("setavatar", OnAvatarCommand, "Changes the bot's avatar to the file (full path) specified.");


            CommandsList.Sort((x, y) => string.Compare(x.Trigger, y.Trigger, StringComparison.Ordinal));
        }

        private static void OnHelpCommand(string arguments)
        {
            foreach (var command in CommandsList)
            {
                string output = command.Trigger;

                if (command.Description != null)
                {
                    output += $": {command.Description}";
                }

                Console.WriteLine($"{Prefix.Info} {output}");
            }
        }

        private static void OnClearCommand(string arguments)
        {
            Console.Clear();
        }

        private static void OnQuitCommand(string arguments)
        {
            Console.WriteLine($"{Prefix.Alert} Exiting program safely in 3 seconds.");

            ChatBot.Client.Disconnect();

            var exitTimer = new Timer(3000);
            exitTimer.Start();

            exitTimer.Elapsed += (sender, args) =>
            {
                Environment.Exit(1);
            };
        }

        private static void OnInviteCommand(string arguments)
        {
            Console.WriteLine($"{Prefix.Info} {ChatBot.InviteLink}");
        }

        private static void OnPrefixCommand(string arguments)
        {
            if (string.IsNullOrWhiteSpace(arguments))
            {
                Console.WriteLine($"{Prefix.Info} The current command prefix is \"{Program.Settings.CommandPrefix}\". You can use \"prefix clear\" to clear it.");

                return;
            }

            if (arguments.Contains(' '))
            {
                Console.WriteLine($"{Prefix.Info} Prefixes cannot contain spaces.");

                return;
            }

            if (arguments == "clear")
            {
                Program.Settings.CommandPrefix = string.Empty;
                Console.WriteLine($"{Prefix.Info} Removed command prefix.");
                Program.Settings.SaveConfig(General.ConfigPath);
            }

            else
            {
                Program.Settings.CommandPrefix = arguments;
                Console.WriteLine($"{Prefix.Info} The new command prefix is \"{Program.Settings.CommandPrefix}\".");
                Program.Settings.SaveConfig(General.ConfigPath);
            }

            ChatCommands.AddChatCommands();
        }

        private static void OnSuffixCommand(string arguments)
        {
            if (string.IsNullOrWhiteSpace(arguments))
            {
                Console.WriteLine($"{Prefix.Info} The current command suffix is \"{Program.Settings.CommandSuffix}\". You can use \"suffix clear\" to clear it.");

                return;
            }

            if (arguments.Contains(' '))
            {
                Console.WriteLine($"{Prefix.Info} Suffixes cannot contain spaces.");

                return;
            }

            if (arguments == "clear")
            {
                Program.Settings.CommandSuffix = string.Empty;
                Console.WriteLine($"{Prefix.Info} Removed command suffix.");
                Program.Settings.SaveConfig(General.ConfigPath);
            }

            else
            {
                Program.Settings.CommandSuffix = arguments;
                Console.WriteLine($"{Prefix.Info} The new command suffix is \"{Program.Settings.CommandSuffix}\".");
                Program.Settings.SaveConfig(General.ConfigPath);
            }

            ChatCommands.AddChatCommands();
        }

        private static void OnGameCommand(string arguments)
        {
            Program.Settings.Game = arguments;
            ChatBot.Client.SetGame(Program.Settings.Game);
            Program.Settings.SaveConfig(General.ConfigPath);

            if (string.IsNullOrWhiteSpace(arguments))
            {
                Console.WriteLine($"{Prefix.Info} Current playing game cleared.");

                return;
            }

            Console.WriteLine($"{Prefix.Info} The bot's is now playing {Program.Settings.Game}.");
        }

        private static void OnAvatarCommand(string arguments)
        {
            if (File.Exists(arguments))
            {
                ChatBot.Client.CurrentUser.Edit(null, null, null, null, File.OpenRead(arguments), Discord.ImageType.Png);
                Console.WriteLine("Changed successfully.");
            }

            else
                Console.WriteLine("Couldn't locate file.");
        }
    }
}
