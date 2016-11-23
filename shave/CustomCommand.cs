using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Newtonsoft.Json;
using System.IO;

namespace shave
{
    public class CustomCommand //aka pasta management
    {
        private string name;
        private string text;

        #region Properties

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        #endregion

        public CustomCommand(string name, string text)
        {
            this.Name = name;
            this.Text = text;
        }

        private static List<CustomCommand> CstmComandsList = ReadCommands();


        private static List<CustomCommand> ReadCommands()
        {

            List<CustomCommand> commands = new List<CustomCommand>();
            commands.Add(new CustomCommand("help", @"Type ""c <command>"" to activate a custom command.Register or update existing commands by typing ""c !<command> <text>"". Delete by typing ""c -<command>""."));
            commands.Add(new CustomCommand("pong", "ping!"));

            if (!File.Exists(Program.Settings.CstmSavePath))
            {
                SaveCommands(commands);
            }

            using (var file = File.OpenText(Program.Settings.CstmSavePath))
            {
                string json = file.ReadToEnd();
                commands = JsonConvert.DeserializeObject<List<CustomCommand>>(json);
            }

            return commands;
        }

        private static void SaveCommands(List<CustomCommand> commands)
        {
            try
            {
                string json = JsonConvert.SerializeObject(commands, Formatting.Indented);
                File.WriteAllText(Program.Settings.CstmSavePath, json);
            }

            catch (Exception n)
            {
                Console.WriteLine($"{Prefix.Error} {n.Message}");
            }
        }

        public static string Process(string arguments, User user)
        {
            string name = arguments.Split(new[] { ' ' })[0];
            string text = arguments.TrimStart(name.ToCharArray());

            if (name.StartsWith("!") && user.ServerPermissions.ManageMessages)
            {
                name = name.TrimStart('!');
                for(int i = 0; i < CstmComandsList.Count; i++)
                {
                    if(CstmComandsList[i].Name == name)
                    {
                        CstmComandsList[i].Text = text;
                        SaveCommands(CstmComandsList);
                        return ("The command " + name + " was updated.");
                    }
                }
                CstmComandsList.Add(new CustomCommand(name, text));
                SaveCommands(CstmComandsList);
                return ("The command " + name + " was added.");
            }

            else
            {
                for (int i = 0; i < CstmComandsList.Count; i++)
                {
                    if (CstmComandsList[i].Name == name)
                    {
                        return (CstmComandsList[i].Text);
                    }
                }
                return ("The command " + name + " wasn't found.");
            }
            
        }

        


    }
}
