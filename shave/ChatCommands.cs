using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;

namespace shave
{
	internal static class ChatCommands
	{
		private static readonly List<ChatCommand> CommandsList = new List<ChatCommand>();
		private static readonly Random Random = new Random();

		private static void AddCommand(string trigger, ChatCommand.OnChatCommand ontrigger, string description = null)
		{
			var command = new ChatCommand
			{
				Trigger = Program.Settings.CommandPrefix + trigger + Program.Settings.CommandSuffix,
				Description = description
			};

			command.OnTrigger += ontrigger;
			CommandsList.Add(command);
		}

		public static void TriggerCommand(string command, User user, Channel channel)
		{
			var cmd = command.Split(new[] { ' ' }, 2);

			if(cmd.Length == 0)
			{
				return;
			}

			var found = CommandsList.FirstOrDefault(x => x.Trigger == cmd[0]);
			found?.TriggerCommand(user, channel, cmd.Length == 2? cmd[1]:null);
		}

		public static void AddChatCommands()
		{
			CommandsList.Clear();

			AddCommand("help", OnHelpCommand, "Display a list of chat commands.");
			AddCommand("ping", OnPingCommand, "Pong!");
			AddCommand("clean", OnClearCommand, "Deletes the last messages sent by the bot. Limited to the past 100 messages.");
			AddCommand("clear", OnClearCommand, "Deletes the last messages sent by the bot. Limited to the past 100 messages.");
			AddCommand("adidas", OnAdidasCommand, "Hard bass))) adidas))");

			CommandsList.Sort((x, y) => string.Compare(x.Trigger, y.Trigger, StringComparison.Ordinal));
		}

		private static async void OnHelpCommand(User user, Channel channel, string arguments)
		{
			var sb = new StringBuilder();
			sb.AppendLine($"```rb" + Environment.NewLine + "Commands:");

			foreach(var command in CommandsList)
			{
				string output = "\t\"" + command.Trigger + "\"";

				if(command.Description != null)
				{
					output += $"\t{command.Description}";
				}

				sb.AppendLine($"{output}");
			}

			sb.Append(@"```");

			await user.SendMessage(sb.ToString());
		}

		private static async void OnPingCommand(User user, Channel channel, string arguments)
		{
			await channel.SendMessage(user.Mention + " - pong!");
		}

		private static async void OnClearCommand(User user, Channel channel, string arguments)
		{
			if(channel.IsPrivate)
			{
				await channel.SendMessage("I cannot do bulk message deletion in private chats, sorry.");

				return;
			}

			if(!channel.Server.CurrentUser.GetPermissions(channel).ManageMessages)
			{
				await channel.SendMessage("I cannot delete messages without the *Manage Messages* permission.");

				return;
			}

			var msgs = (await channel.DownloadMessages()).Where(m => m.User?.Id == ChatBot.Client.CurrentUser.Id).ToArray();

			if(msgs.Any())
			{
				await channel.DeleteMessages(msgs);
			}
		}

		private static async void OnAdidasCommand(User user, Channel channel, string arguments)
		{
			var adidas = new List<string>
			{
				@"https://www.youtube.com/watch?v=sZKYSsz_v2c",
				@"https://www.youtube.com/watch?v=McdX0aLz_Ss",
				@"https://www.youtube.com/watch?v=ZJg-oiCMSt4",
				@"https://www.youtube.com/watch?v=QfznpaDe7u8",
				@"https://www.youtube.com/watch?v=-0y1BY7UcaI",
				@"https://www.youtube.com/watch?v=hksRnR4oIfg",
				@"https://www.youtube.com/watch?v=vOP1iAzWhkk",
				@"https://www.youtube.com/watch?v=rZxxH76tUBg",
				@"https://www.youtube.com/watch?v=lQIrL_xH7tM",
				@"https://www.youtube.com/watch?v=WdI5y3iwhLU",
				@"https://www.youtube.com/watch?v=V1tg021PCTg",
				@"https://www.youtube.com/watch?v=xXyWk7DvhGI",
				@"https://www.youtube.com/watch?v=rSYkUGeU1RQ",
				@"https://www.youtube.com/watch?v=-RdFna7r28I",
				@"https://www.youtube.com/watch?v=6j74OtnnSP8",
				@"https://www.youtube.com/watch?v=_aHdvd74DWM",
				@"https://www.youtube.com/watch?v=kWxwgPwdu-M",
				@"https://www.youtube.com/watch?v=_d1y9CqWy-U",
				@"https://www.youtube.com/watch?v=QiFBgtgUtfw",
				@"https://www.youtube.com/watch?v=i6diGdrVAdQ",
				@"https://www.youtube.com/watch?v=TIVijJWiIrQ"
			};

			await channel.SendMessage($"{user.Mention} - {adidas[Random.Next(0, adidas.Count)]}");
		}
	}
}
