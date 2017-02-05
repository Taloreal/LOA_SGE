using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LOA_SGE.Profile_Components;

namespace LOA_SGE
{
    public partial class ProfileEditor : Form
    {
        string Previous = "0";
        string Previous2 = "0";
        int Selected_Mission = 0;
        List<Profile> LoadedProfile = new List<Profile>();
        char seperator = ' ';
        public ProfileEditor(Profile P)
        {
            InitializeComponent();
            LoadedProfile.Add(P);
        }

        /// <summary>
        /// Updates all of the controls on the user interface.
        /// </summary>
        public void DrawGUI()
        {
            label23.Text = "Selected Mission : " + LoadedProfile[0].Missions[Selected_Mission].Name;
            textBox2.Text = LoadedProfile[0].Missions[Selected_Mission].Completeness.ToString();
            for (int i = 0; i != LoadedProfile[0].Missions[Selected_Mission].Easy_Objectives.Count; i++)
                listView1.Items.Add("Normal" + seperator + LoadedProfile[0].Missions[Selected_Mission].Easy_Objectives[i].ToString() + seperator + i);
            for (int i = 0; i != LoadedProfile[0].Missions[Selected_Mission].Hard_Objectives.Count; i++)
                listView1.Items.Add("Hard" + seperator + LoadedProfile[0].Missions[Selected_Mission].Hard_Objectives[i].ToString() + seperator + i);
        }

        /// <summary>
        /// Makes the missions available to add.
        /// </summary>
        public void AddMisisons()
        {
            List<string> Names = General_Usage_Methods.Split(global::LOA_SGE.Properties.Resources.MissionIndexs, "\r\n");
            foreach (string s in Names)
                Missions.Items.Add(General_Usage_Methods.Split(s, ',')[1]);
        }

        /// <summary>
        /// Loads the interface.
        /// </summary>
        private void ProfileEditor_Load(object sender, EventArgs e)
        {
            DrawGUI();
            AddMisisons();
            Missions.SelectedIndex = 0;
        }

        /// <summary>
        /// Removes an objective from the listview control.
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            string text = listView1.SelectedItems[0].Text;
            List<string> Parts = General_Usage_Methods.Split(text, seperator);
            int index = Convert.ToInt32(Parts[2]);
            if (Parts[0] == "Hard")
                LoadedProfile[0].Missions[Selected_Mission].Hard_Objectives.RemoveAt(index);
            else
                LoadedProfile[0].Missions[Selected_Mission].Easy_Objectives.RemoveAt(index);
            listView1.Items.RemoveAt(index);
            LoadedProfile[0].Missions[Selected_Mission].UpdateCount();
            updateHardIndexs();
            updateNormalIndex();
        }

        /// <summary>
        /// Updates the normal difficulty indexes.
        /// </summary>
        void updateNormalIndex()
        {
            int index = 0;
            for (int i = 0; i != listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Text.StartsWith("Normal"))
                {
                    string text = listView1.Items[i].Text;
                    List<string> Parts = General_Usage_Methods.Split(text, seperator);
                    Parts[2] = index.ToString();
                    index++;
                    listView1.Items[i].Text = Parts[0] + seperator + Parts[1] + seperator + Parts[2];
                }
            }
        }

        /// <summary>
        /// Updates the hard difficulty indexes.
        /// </summary>
        void updateHardIndexs()
        {
            int index = 0;
            for (int i = 0; i != listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Text.StartsWith("Hard"))
                {
                    string text = listView1.Items[i].Text;
                    List<string> Parts = General_Usage_Methods.Split(text, seperator);
                    Parts[2] = index.ToString();
                    index++;
                    listView1.Items[i].Text = Parts[0] + seperator + Parts[1] + seperator + Parts[2];
                }
            }
        }

        /// <summary>
        /// Adds an objective to the listview control.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            string difficulty = "";
            int index = 0;
            if (radioButton1.Checked) {
                difficulty = "Normal"; 
                index = LoadedProfile[0].Missions[Selected_Mission].Easy_Objectives.Count;
                LoadedProfile[0].Missions[Selected_Mission].Easy_Objectives.Add(Convert.ToInt32(textBox1.Text));
            }
            else { 
                difficulty = "Hard"; 
                index = LoadedProfile[0].Missions[Selected_Mission].Hard_Objectives.Count;
                LoadedProfile[0].Missions[Selected_Mission].Hard_Objectives.Add(Convert.ToInt32(textBox1.Text));
            }
            string text = difficulty + seperator + textBox1.Text + seperator + index;
            listView1.Items.Add(text);
            LoadedProfile[0].Missions[Selected_Mission].UpdateCount();
            updateHardIndexs();
            updateNormalIndex();
        }

        /// <summary>
        /// Manages a new value that'll be added as an objective
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumber(textBox1.Text)) { textBox1.Text = Previous; return; }
            Previous = textBox1.Text;
        }

        /// <summary>
        /// Checks if a string is a number.
        /// </summary>
        /// <param name="Val">a string value.</param>
        /// <returns>if the string is a number.</returns>
        bool IsNumber(string Val)
        {
            try { Convert.ToInt32(Val); return true; }
            catch { return false; }
        }

        /// <summary>
        /// Manages the overall missions "completeness".
        /// </summary>
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumber(textBox1.Text)) { textBox2.Text = Previous2; return; }
            Previous2 = textBox2.Text;
            LoadedProfile[0].Missions[Selected_Mission].Completeness = Convert.ToInt32(textBox2.Text);
        }

        /// <summary>
        /// Updates the mission sletected.
        /// </summary>
        private void pictureBox18_Click(object sender, EventArgs e)
        {
            if (Selected_Mission == 16 || Selected_Mission == (LoadedProfile[0].Missions.Count - 1))
                return;
            listView1.Items.Clear();
            Selected_Mission++;
            DrawGUI();
        }

        /// <summary>
        /// Updates the mission sletected.
        /// </summary>
        private void pictureBox17_Click(object sender, EventArgs e)
        {
            if (Selected_Mission == 0)
                return;
            listView1.Items.Clear();
            Selected_Mission--;
            DrawGUI();
        }

        /// <summary>
        /// Saves the altered profile.
        /// </summary>
        private void Save_Click(object sender, EventArgs e)
        {
            try { LoadedProfile[0].Save(); }
            catch { MessageBox.Show("ERROR : Could not save the profile."); return; }
            MessageBox.Show("Save successful.");
        }

        /// <summary>
        /// Adds a selected mission to the profile.
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            int MissionIndex = Mission_Index();
            string temp = "";
            foreach (Mission M in LoadedProfile[0].Missions)
                if (M.Mission_Number == MissionIndex)
                    return;
            LoadedProfile[0].Missions.Add(Mission.Load(MissionIndex.ToString() + " 0 0 0 ", out temp));
        }

        int Mission_Index()
        {
            List<string> Names = General_Usage_Methods.Split(global::LOA_SGE.Properties.Resources.MissionIndexs, "\r\n");
            foreach (string s in Names)
                if (s.Contains(Missions.SelectedItem.ToString()))
                    return Convert.ToInt32(General_Usage_Methods.Split(s, ',')[0]);
            return -1;
        }

        private void UP_Click(object sender, EventArgs e)
        {
            int ObjectiveNum = Convert.ToInt32(textBox1.Text);
            ObjectiveNum = ObjectiveNum - 1;
            textBox1.Text = ObjectiveNum.ToString();
        }
        private void DOWN_Click(object sender, EventArgs e)
        {
            int ObjectiveNum = Convert.ToInt32(textBox1.Text);
            ObjectiveNum = ObjectiveNum + 1;
            textBox1.Text = ObjectiveNum.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int temp = Missions.SelectedIndex;
            Missions.SelectedIndex = 0;
            button3_Click(null, null);
            for (int i = 0; i != temp; i++)
            {
                Missions.SelectedIndex++;
                button3_Click(null, null);
            }
            int MissionIndex = Mission_Index();
            LoadedProfile[0].Hard = MissionIndex;
            for (int i = 0; i != General_Usage_Methods.Split(global::LOA_SGE.Properties.Resources.MissionIndexs, "\r\n").Count; i++)
                LoadedProfile[0].Missions[i].Completeness = 2;
        }
    }
}
