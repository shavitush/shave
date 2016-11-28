using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace shave
{
    internal static class Program
    {
        /// <summary>
        /// Configuration file for the program.
        /// </summary>
        public static Config Settings { get; private set; } = new Config();

        /// <summary>
        /// Starting the program here.
        /// </summary>
        private static void Main()
        {
            // Sets the window title and prettifies the console.
            Console.Title = General.Title;
            Console.ForegroundColor = ConsoleColor.Cyan;

            // Welcome message from our maid, shave.
            Console.WriteLine($"Welcome to {General.Title}! I'm a Discord chat bot, made by {General.Author}.");

            // Loads the config file.
            if (!File.Exists(General.ConfigPath))
            {
                Settings.SaveConfig(General.ConfigPath);
            }

            using (var file = File.OpenText(General.ConfigPath))
            {
                Settings = JsonConvert.DeserializeObject<Config>(file.ReadToEnd());
            }

            if (Settings.Token == @"<EMPTY>")
            {
                Console.WriteLine($"{Prefix.Alert} Please exit the program and fill in your bot's token in {General.ConfigPath}.");
                Console.ReadLine();

                return;
            }

            // Save config here, in case we have any new setting that needs to be written into the config file.
            Settings.SaveConfig(General.ConfigPath);

            // Starts the help input thread.
            new Thread(Input.WaitForInput).Start();

            Console.WriteLine($"{Prefix.Info} You may write 'help' to list commands, or 'q' to exit.");

            // Logins to Discord.
            new Thread(ChatBot.Login).Start();
        }
    }
}
