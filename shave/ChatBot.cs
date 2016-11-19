using System;
using Discord;

namespace shave
{
	internal static class ChatBot
	{
		internal static DiscordClient Client { get; } = new DiscordClient();

		public static string InviteLink => Client.CurrentUser == null? @"<INVALID>":$"https://discordapp.com/oauth2/authorize?client_id={Client.CurrentUser.Id}&scope=bot&permissions=8192";

		public static async void Login()
		{
			Console.WriteLine($"{Prefix.Info} Connecting to Discord.");

			Client.Ready += ClientOnReady;
			Client.MessageReceived += ClientOnMessageReceived;

			await Client.Connect(Program.Settings.Token, TokenType.Bot);
		}

		private static void ClientOnReady(object sender, EventArgs eventArgs)
		{
			Console.WriteLine($"{Prefix.Info} Connected to Discord sucessfully! ({Client.CurrentUser.Name}#{Client.CurrentUser.Discriminator})");
			ChatCommands.AddChatCommands();
			Console.WriteLine($"{Prefix.Info} Registered chat commands.");
		}

		private static void ClientOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
		{
			// I don't want to catch my own commands!
			if(messageEventArgs.Message.IsAuthor)
			{
				return;
			}

			ChatCommands.TriggerCommand(messageEventArgs.Message, messageEventArgs.User, messageEventArgs.Channel);
		}
	}
}
