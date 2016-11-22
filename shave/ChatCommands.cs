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

		public static void TriggerCommand(Message message, User user, Channel channel)
		{
			var cmd = message.RawText.Split(new[] { ' ' }, 2);

			if(cmd.Length == 0)
			{
				return;
			}

			var found = CommandsList.FirstOrDefault(x => x.Trigger == cmd[0]);
			found?.TriggerCommand(user, channel, cmd.Length == 2? cmd[1]:null, message);
		}

		public static void AddChatCommands()
		{
			CommandsList.Clear();

			AddCommand("help", OnHelpCommand, "Displays a list of chat commands.");
			AddCommand("ping", OnPingCommand, "Pong!");
			AddCommand("clean", OnClearCommand, "Deletes the last messages sent by the bot. Limited to the past 100 messages.");
			AddCommand("clear", OnClearCommand, "Deletes the last messages sent by the bot. Limited to the past 100 messages.");
			AddCommand("adidas", OnAdidasCommand, "Hard bass))) adidas))");
            AddCommand("tip", OnTipCommand, "Displays a random compliment dedicated to the argument.");

			CommandsList.Sort((x, y) => string.Compare(x.Trigger, y.Trigger, StringComparison.Ordinal));
		}

		private static async void OnHelpCommand(User user, Channel channel, string arguments, Message message)
		{
			var sb = new StringBuilder();
			sb.AppendLine("```rb" + Environment.NewLine + "Commands:");

			foreach(var command in CommandsList)
			{
				string output = "\t\"" + command.Trigger + "\"";

				if(command.Description != null)
				{
					output += "\t" + command.Description;
				}

				sb.AppendLine(output);
			}

			sb.Append("```");

			await user.SendMessage(sb.ToString());
		}

		private static async void OnPingCommand(User user, Channel channel, string arguments, Message message)
		{
			await channel.SendMessage($"{user.Mention} - pong!");
		}

		private static async void OnClearCommand(User user, Channel channel, string arguments, Message message)
		{
			if(channel.IsPrivate)
			{
				await channel.SendMessage("I cannot do bulk message deletion in private chats, sorry.");

				return;
			}

			if(!channel.Server.CurrentUser.GetPermissions(channel).ManageMessages)
			{
				await channel.SendMessage($"{user.Mention} - I cannot delete messages without the *Manage Messages* permission.");

				return;
			}

			var msgs = (await channel.DownloadMessages()).Where(m => m.User?.Id == ChatBot.Client.CurrentUser.Id).ToArray();

			if(msgs.Any())
			{
				await channel.DeleteMessages(msgs);
			}
		}

		private static async void OnAdidasCommand(User user, Channel channel, string arguments, Message message)
		{
			var adidas = new List<string>
			{
				@"sZKYSsz_v2c", @"McdX0aLz_Ss", @"ZJg-oiCMSt4", @"QfznpaDe7u8", @"-0y1BY7UcaI", @"hksRnR4oIfg", @"vOP1iAzWhkk",
				@"rZxxH76tUBg", @"lQIrL_xH7tM", @"WdI5y3iwhLU", @"V1tg021PCTg", @"xXyWk7DvhGI", @"rSYkUGeU1RQ", @"-RdFna7r28I",
				@"6j74OtnnSP8", @"_aHdvd74DWM", @"kWxwgPwdu-M", @"_d1y9CqWy-U", @"QiFBgtgUtfw", @"i6diGdrVAdQ", @"TIVijJWiIrQ"
			};

			await channel.SendMessage($"{user.Mention} - https://www.youtube.com/watch?v={adidas[Random.Next(0, adidas.Count)]}");
		}

        private static async void OnTipCommand(User user, Channel channel, string arguments, Message message)
        {
            var verbs = new List<string>
            {
             " finds ",
             " points out that",
             " implies "
            };

            var adj = new List<String>
            {
                "incredibly ",
                "especially ",
                "very ",
                "extremely "
            };

            var fin = new List<string>
            {
                 "well written.",
                 "funny.",
                 "insightful.",
                 "enlightening."
            };

            await channel.SendMessage(user.Name + verbs[Random.Next(0, verbs.Count)] + arguments + "'s message to be " +
                    adj[Random.Next(0, adj.Count)] + fin[Random.Next(0, fin.Count)]);
        }
    }
}
