using System;
using System.IO;
using Newtonsoft.Json;

namespace shave
{
    internal class Config
    {
        public string Token { get; set; } = @"<EMPTY>";
        public string CommandPrefix { get; set; } = string.Empty;
        public string CommandSuffix { get; set; } = @"))";
        public string CstmSavePath { get; set; } = @"custom.json";
        public string Game { get; set; } = @"/home/shavit/shave/";

        public void SaveConfig(string path)
        {
            try
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(path, json);
            }

            catch (Exception n)
            {
                Console.WriteLine($"{Prefix.Error} {n.Message}");
            }
        }
    }
}
