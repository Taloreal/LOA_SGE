using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LOA_SGE.Profile_Components
{
    public class Mission
    {
        public int Hard = 0;
        public int Normal = 0;
        public int Completeness = 0;
        public int Mission_Number = 0;
        public string Name = "";
        public List<int> Easy_Objectives = new List<int>();
        public List<int> Hard_Objectives = new List<int>();
        /// <summary>
        /// Loads a mission from a string feed.
        /// </summary>
        /// <param name="text">The string feed to create mission from.</param>
        /// <param name="LeftOver">The left over text for next feed.</param>
        /// <returns>A mission object ready to be edited.</returns>
        public static Mission Load(string text, out string LeftOver)
        {
            Mission M = new Mission();
            List<string> parts = General_Usage_Methods.Split(text, ' ');
            M.Mission_Number = Convert.ToInt32(parts[0]);
            M.Name = GetName(M.Mission_Number);
            M.Completeness = Convert.ToInt32(parts[1]);
            M.Normal = Convert.ToInt32(parts[2]);
            int HardIndex = 3 + M.Normal;
            M.Hard = Convert.ToInt32(parts[HardIndex]);
            for (int i = 3; i != HardIndex; i++)
                M.Easy_Objectives.Add(Convert.ToInt32(parts[i]));
            int LastIndex = 4 + (M.Normal + M.Hard);
            for (int i = HardIndex + 1; i != LastIndex; i++)
                M.Hard_Objectives.Add(Convert.ToInt32(parts[i]));
            string temp = "";
            for (int i = 0; i != LastIndex; i++)
                temp = temp + parts[i] + " ";
            LeftOver = text.Substring(temp.Length);
            return M;
        }

        /// <summary>
        /// Updates the Hard and Normal Objectives completed.
        /// </summary>
        public void UpdateCount()
        {
            Hard = Hard_Objectives.Count;
            Normal = Easy_Objectives.Count;
        }

        /// <summary>
        /// Gets the name of a mission.
        /// </summary>
        /// <param name="index">The mission index to retrieve the name of.</param>
        /// <returns>The fully qualified mission name.</returns>
        public static string GetName(int index)
        {
            List<string> Names = General_Usage_Methods.Split(global::LOA_SGE.Properties.Resources.MissionIndexs, "\r\n");
            string Name = "";
            foreach (string s in Names)
                if (Convert.ToInt32(General_Usage_Methods.Split(s, ',')[0]) == index)
                    Name = General_Usage_Methods.Split(s, ',')[1];
            return Name;
        }

        /// <summary>
        /// Summarizes the mission data for saving.
        /// </summary>
        /// <returns>The summary of the mission.</returns>
        public string Summarize()
        {
            string Data = Mission_Number.ToString() + " " + Completeness.ToString() + " " + Normal.ToString() + " ";
            foreach (int i in Easy_Objectives)
                Data = Data + i.ToString() + " ";
            Data = Data + Hard.ToString() + " ";
            foreach (int i in Hard_Objectives)
                Data = Data + i.ToString() + " ";
            return Data;
        }
    }
}
