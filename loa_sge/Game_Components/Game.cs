using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.IO;
using LOA_SGE.Game_Components;

namespace LOA_SGE.Game_Components
{
    public class Game
    {
        public string Name = "";
        public const string initilizer = "114";
        public int mission_number = 0;
        public int difficulty = 0;
        public string Mission_specific_variables = "";
        public int year = 2013;
        public int month = 4;
        public int day = 1;
        public string play_time = "";
        public string End = "101 110 106 106 10 ";
        public Faction[] Factions = new Faction[10];
        public Ship_Design[] Designs = new Ship_Design[10];
        public string Left_Overs = "";
        public bool Loaded = false;
        public List<Section> Game_Sections = new List<Section>();

        /// <summary>
        /// Loads the global variables for the mission.
        /// </summary>
        /// <param name="G">The game object to place loaded contents.</param>
        /// <param name="Text">The text from the save file.</param>
        /// <param name="buffer">The buffer to keep track of what has been interpreted alread.</param>
        /// <returns>An updated buffer.</returns>
        public static string Load_Globals(Game G, string Text, string buffer)
        {
            string Global = Regex.Split(Text, G.End)[0];//General_Usage_Methods.Split(Text, End)[0];
            List<string> Parts = General_Usage_Methods.Split(Global, ' ');
            G.play_time = Parts[Parts.Count - 1];
            G.day = Convert.ToInt32(Parts[Parts.Count - 2]);
            G.month = Convert.ToInt32(Parts[Parts.Count - 3]);
            G.year = Convert.ToInt32(Parts[Parts.Count - 4]);
            G.Name = Parts[0];
            G.mission_number = Convert.ToInt32(Parts[2]);
            G.difficulty = Convert.ToInt32(Parts[3]);
            for (int i = 4; i != Parts.Count - 4; i++)
                G.Mission_specific_variables = G.Mission_specific_variables + Parts[i] + " ";
            return buffer + G.Summary();
        }

        /// <summary>
        /// Loads the various factions.
        /// </summary>
        /// <param name="G">The game object to place loaded contents.</param>
        /// <param name="Text">The text from the save file.</param>
        /// <param name="buffer">The buffer to keep track of what has already been interpreted.</param>
        /// <returns>An updated buffer.</returns>
        public static string Load_Factions(Game G, string Text, string buffer)
        {
            List<string> Factions = General_Usage_Methods.Split(Text.Replace(buffer, ""), ' ', 35, 10);
            Faction.Enders = General_Usage_Methods.Split(Factions[10], ' ')[0] + " " + General_Usage_Methods.Split(Factions[10], ' ')[1] + " ";
            for (int i = 0; i != G.Factions.Count(); i++)
                G.Factions[i] = Faction.Create_Faction_From_Feed(i, Factions[i]);
            return buffer + Faction.Summary(G.Factions);
        }

        /// <summary>
        /// Loads the various ship designs in the game.
        /// </summary>
        /// <param name="G">THe game object to place loaded contents.</param>
        /// <param name="Text">The text from the save file.</param>
        /// <param name="buffer">The buffer to keep track of what has already been interpreted.</param>
        /// <returns>An updated buffer.</returns>
        public static string Load_Designs(Game G, string Text, string buffer)
        {
            List<string> Designs = General_Usage_Methods.Split(Text.Replace(buffer, ""), ' ', 19, 9);
            for (int i = 0; i != G.Designs.Count(); i++)
                G.Designs[i] = Ship_Design.Create_Design_From_Feed(i, Designs[i]);
            return buffer + Ship_Design.Summary(G.Designs);
        }

        /// <summary>
        /// Loads a game from direct data.
        /// </summary>
        /// <param name="Data">The data to load.</param>
        /// <returns>A directly edited game.</returns>
        public static Game Load_Game(List<Section> Data)
        {
            string Text = "";
            string buffer = "";
            Game G = new Game();
            for (int i = 0; i != Data.Count; i++)
                Text = Text + Data[i].Data;
            try {
                buffer = Load_Globals(G, Text, "");
                buffer = Load_Factions(G, Text, buffer);
                buffer = Load_Designs(G, Text, buffer);
            }
            catch {
                try {
                    G.Mission_specific_variables = "";
                    G.End = "101 111 106 106 10 ";
                    buffer = "";
                    buffer = Load_Globals(G, Text, "");
                    buffer = Load_Factions(G, Text, buffer);
                    buffer = Load_Designs(G, Text, buffer);
                }
                catch { return null; }
            }
            G.Left_Overs = Text.Replace(buffer, "");
            G.Game_Sections.Add(new Section(G.Summary(), "General Info"));
            G.Game_Sections.Add(new Section(Faction.Summary(G.Factions), "Factions", General_Usage_Methods.Split(Faction.Summary(G.Factions), ' ').Count));
            G.Game_Sections.Add(new Section(Ship_Design.Summary(G.Designs), "Ship Designs", General_Usage_Methods.Split(Ship_Design.Summary(G.Designs), ' ').Count));
            G.Game_Sections.Add(new Section(G.Left_Overs, "Left Overs"));
            G.Loaded = true;
            return G;
        }

        /// <summary>
        /// Loads a game from a specified file.
        /// </summary>
        /// <param name="Directory">The path to the file to read.</param>
        /// <returns>A game object containing the game information.</returns>
        public static Game Load_Game(string Directory)
        {
            if (!File.Exists(Directory))
                return null;
            Game G = new Game();
            string Collector = "";
            string buffer = "";
            string Text = "";
            try {
                TextReader TR = new StreamReader(Directory);
                Text = TR.ReadToEnd();
                TR.Close();
            }
            catch { return null; }
            try {
                buffer = Load_Globals(G, Text, "");
                buffer = Load_Factions(G, Text, buffer);
                buffer = Load_Designs(G, Text, buffer);
            }
            catch { 
                try {
                    G.Mission_specific_variables = "";
                    G.End = "101 111 106 106 10 ";
                    buffer = "";
                    buffer = Load_Globals(G, Text, "");
                    buffer = Load_Factions(G, Text, buffer);
                    buffer = Load_Designs(G, Text, buffer);
                } 
                catch { return null; } 
            }
            G.Left_Overs = Text.Replace(buffer, "");
            G.Game_Sections.Add(new Section(G.Summary(), "General Info"));
            G.Game_Sections.Add(new Section(Faction.Summary(G.Factions), "Factions", General_Usage_Methods.Split(Faction.Summary(G.Factions), ' ').Count));
            G.Game_Sections.Add(new Section(Ship_Design.Summary(G.Designs), "Ship Designs", General_Usage_Methods.Split(Ship_Design.Summary(G.Designs), ' ').Count));
            G.Game_Sections.Add(new Section(G.Left_Overs, "Left Overs"));
            return G;
        }

        /// <summary>
        /// Updates section datas.
        /// </summary>
        public void ReloadSections()
        {
            Game_Sections[0] = new Section(Summary(), "General Info");
            Game_Sections[1] = new Section(Faction.Summary(Factions), "Factions", General_Usage_Methods.Split(Faction.Summary(Factions), ' ').Count);
            Game_Sections[2] = new Section(Ship_Design.Summary(Designs), "Ship Designs", General_Usage_Methods.Split(Ship_Design.Summary(Designs), ' ').Count);
            Game_Sections[3] = new Section(Left_Overs, "Left Overs");
        }

        /// <summary>
        /// Summarizes the global variables.
        /// </summary>
        /// <returns>The global variables for a save.</returns>
        public string Summary()
        {
            return Name + " " + initilizer + " " + mission_number.ToString() + " " +
                difficulty.ToString() + " " + Mission_specific_variables +
                year.ToString() + " " + month.ToString() + " " + day.ToString() + " " + play_time.ToString() + " " + End;
        }

        /// <summary>
        /// Saves the current game object to a specified file path.
        /// </summary>
        /// <param name="Directory">The file path to save to.</param>
        public void Save_Game(string Directory)
        {
            string data = Summary() + Faction.Summary(Factions) + Ship_Design.Summary(Designs) + Left_Overs;
            TextWriter TW = new StreamWriter(Directory);
            TW.Write(data);
            TW.Close();
        }
    }
}
