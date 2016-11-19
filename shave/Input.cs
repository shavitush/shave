using System;

namespace shave
{
	internal static class Input
	{
		public static void WaitForInput()
		{
			SystemCommands.AddHelpCommands();
			
			while(true)
			{
				if(!SystemCommands.TriggerCommand(Console.ReadLine()))
				{
					Console.WriteLine($"{Prefix.Error} Invalid command! Write 'help' to list all the commands.");
				}
			}
		}
	}
}
