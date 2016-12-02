using System.Collections.Generic;
using System.Linq;
using Discord;

namespace shave
{
	internal static class ChatFilter
	{
		private const int RequiredHits = 3;

		private static readonly List<string> FilteredWords = new List<string>
		{
			@"please", @"use", @"direct", @"links", @"or", @"get", @"sharex"
		};

		public static bool IsFiltered(Message message)
		{
			if(string.IsNullOrWhiteSpace(message.Text))
			{
				return false;
			}

			var wordstring = message.Text.ToLower().Split(' ').Where(s => !string.IsNullOrWhiteSpace(s));
			int hits = FilteredWords.Count(filteredword => wordstring.Contains(filteredword));

			return hits >= RequiredHits;
		}
	}
}
