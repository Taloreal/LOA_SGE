using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace LOA_SGE
{
    public partial class KeyboardSettings : Form
    {
        public KeyboardSettings(string Loc)
        {
            InitializeComponent();
            Path = Loc;
        }

        string Path = "";
        int[] Shortcuts = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, };
        List<PictureBox> Image_Control = new List<PictureBox>();
        List<string> Miscellaneous_Data = new List<string>();
        /// <summary>
        /// Loads the currently designated shortcuts.
        /// </summary>
        void LoadShortcuts()
        {
            if (!File.Exists(Path))
                return;
            string Data = "";
            try { TextReader TR = new StreamReader(Path); Data = TR.ReadToEnd(); TR.Close(); }
            catch { MessageBox.Show("ERROR : Could not read LOA settings file."); SetMisc(); return; }
            List<string> Lines = General_Usage_Methods.Split(Data.Replace("\r", ""), '\n');
            int index = 0;
            for (int i = 0; i != Lines.Count; i++)
                if (Lines[i].StartsWith("HOTKEY_" + index.ToString()))
                {
                    Shortcuts[index] = Convert.ToInt32(Lines[i].Replace("HOTKEY_" + index.ToString() + " ", ""));
                    index++;
                }
                else
                    Miscellaneous_Data.Add(Lines[i]);
        }

        /// <summary>
        /// Sets all the remaining data left over then shortcuts.
        /// </summary>
        void SetMisc()
        {
            List<int> Indexes = new List<int>();
            List<string> lines = General_Usage_Methods.Split(global::LOA_SGE.Properties.Resources.LOA_Settings.Replace("\r", ""), '\n');
            for (int i = 0; i != lines.Count; i++)
                if (lines[i].Contains("HOTKEY_"))
                    Indexes.Add(Convert.ToInt32(i));
            for (int i = 0; i != Indexes.Count; i++)
                lines.RemoveAt(Indexes[i]);
            Miscellaneous_Data = lines;
        }

        /// <summary>
        /// Updates the controls on the user interface.
        /// </summary>
        void DrawGUI()
        {
            for (int i = 0; i != Shortcuts.Count(); i++)
            {
                if (Shortcuts[i] == -1)
                { Image_Control[i].Image = null; continue; }
                int Row = ((int)(Shortcuts[i] / 8)) * 128;
                int Column = (Shortcuts[i] % 8) * 128;
                Point P = new Point(Column, Row);
                Size S = new Size(128, 128);
                Image_Control[i].Image = General_Usage_Methods.cropImage((Image)(global::LOA_SGE.Properties.Resources.buildingimages), P, S);
            }
        }

        /// <summary>
        /// Adds pictureboxes to a localized hub so they can be changed easier.
        /// </summary>
        void AddControls()
        {
            Image_Control.Add(Shortcut1);
            Image_Control.Add(Shortcut2);
            Image_Control.Add(Shortcut3);
            Image_Control.Add(Shortcut4);
            Image_Control.Add(Shortcut5);
            Image_Control.Add(Shortcut6);
            Image_Control.Add(Shortcut7);
            Image_Control.Add(Shortcut8);
            Image_Control.Add(Shortcut9);
            Image_Control.Add(Shortcut10);
        }

        /// <summary>
        /// Starts up the userinterface after loading the needed data.
        /// </summary>
        private void KeyboardSettings_Load(object sender, EventArgs e)
        {
            AddControls();
            LoadShortcuts();
            DrawGUI();
        }
        
        /// <summary>
        /// Changes a building selection.
        /// </summary>
        private void ArrowClickRight(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(((PictureBox)sender).Tag);
            if (Shortcuts[index] == 29)
                return;
            Shortcuts[index]++;
            DrawGUI();
        }

        /// <summary>
        /// Changes a building selection.
        /// </summary>
        private void ArrowClickLeft(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(((PictureBox)sender).Tag);
            if (Shortcuts[index] == -1)
                return;
            Shortcuts[index]--;
            DrawGUI();
        }

        /// <summary>
        /// Puts an interger in proper format for the game.
        /// </summary>
        /// <param name="i">The shortcut integer.</param>
        /// <returns>The needed formated integer.</returns>
        string IntToString(int i)
        {
            if (i.ToString().Length == 1)
                return "0" + i;
            return i.ToString();
        }

        /// <summary>
        /// Saves the current configuration.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            string all = "";
            foreach (string s in Miscellaneous_Data)
                all = all + s + "\r\n";
            for (int i = 0; i != Shortcuts.Count(); i++)
                if (i == Shortcuts.Count() - 1)
                    all = all + "HOTKEY_" + i + " " + IntToString(Shortcuts[i]);
                else
                    all = all + "HOTKEY_" + i + " " + IntToString(Shortcuts[i]) + "\r\n";
            try
            {
                TextWriter TW = new StreamWriter(Path);
                TW.Write(all);
                TW.Close();
            }
            catch { MessageBox.Show("ERROR : Could not save settings file."); return; }
            MessageBox.Show("Save successful.");
        }
    }
}
