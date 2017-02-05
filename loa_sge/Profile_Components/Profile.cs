using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LOA_SGE.Profile_Components
{
    public class Profile
    {
        public int Easy = 0;
        public int Hard = 0;
        static int max = 16;
        public string Name = "Profile";
        public string Save_Directory = "";
        public List<Mission> Missions = new List<Mission>();
        /// <summary>
        /// Loads a profile from a string feed and it's subsequent missions.
        /// </summary>
        /// <param name="Path">The path to the profile file.</param>
        /// <returns>A profile thats loaded for the editor.</returns>
        public static Profile Load(string Path)
        {
            Profile P = new Profile();
            P.Save_Directory = Path;
            string text = "";
            string temp = "";
            try
            {
                TextReader TR = new StreamReader(Path);
                text = TR.ReadToEnd();
                TR.Close();
                List<string> parts = General_Usage_Methods.Split(text, ' ');
                P.Easy = Convert.ToInt32(parts[0]);
                P.Hard = Convert.ToInt32(parts[1]);
                if (P.Hard > max)
                    P.Hard = max;
                if (P.Easy > max)
                    P.Easy = max;
                parts[0] = P.Easy.ToString();
                parts[1] = P.Hard.ToString();
                temp = parts[0] + " " + parts[1] + " ";
                text = text.Substring(temp.Length);
                while (text != "" && P.Missions.Count <= max)
                    P.Missions.Add(Mission.Load(text, out text));
            }
            catch { return null; }
            return P;
        }

        /// <summary>
        /// Saves the current profile configuration.
        /// </summary>
        public void Save()
        {
            string Data = Easy.ToString() + " " + Hard.ToString() + " ";
            foreach (Mission M in Missions)
                Data = Data + M.Summarize();
            TextWriter TW = new StreamWriter(Save_Directory);
            TW.Write(Data);
            TW.Close();
        }
    }
}
