namespace shave
{
	internal static class General
	{
		public const string Author = @"shavit";
		public const string ConfigPath = @"shave.json";

		private const string Name = @"shave";
		private const float Version = 1.0f;

		public static string Title => $@"{Name} ~ {Version:0.0}";
	}

	internal static class Prefix
	{
		public const string Info = @"[INFO]";
		public const string Error = @"[ERROR]";
		public const string Alert = @"[ALERT]";
		public const string Hint = @"[HINT]";
	}
}
