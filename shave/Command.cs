using Discord;

namespace shave
{
	internal class Command
	{
		public string Trigger { get; set; }

		public string Description { get; set; }
	}

	internal class SystemCommand : Command
	{
		internal delegate void OnSystemCommand(string arguments);
		public event OnSystemCommand OnTrigger;

		public void TriggerCommand(string arguments)
		{
			OnTrigger?.Invoke(arguments);
		}
	}

	internal class ChatCommand : Command
	{
		internal delegate void OnChatCommand(User user, Channel channel, string arguments);
		public event OnChatCommand OnTrigger;

		public void TriggerCommand(User user, Channel channel, string arguments)
		{
			OnTrigger?.Invoke(user, channel, arguments);
		}
	}
}