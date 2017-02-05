using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Net.Mail;
using System.Net;
using LOA_SGE.Game_Components;
using LOA_SGE.Profile_Components;

namespace LOA_SGE
{
    public partial class SaveEditor : Form
    {
        public SaveEditor()
        {
            InitializeComponent();
        }

        string Game_Directory = Path.GetPathRoot(Environment.SystemDirectory) + @"Documents and Settings\" + Environment.UserName + @"\Application Data\SaintXi\LightOfAltair\Save\";
        Game G = new Game();
        int Faction_Index = 0;
        int Spending_Index = 0;
        int Income_Index = 0;
        int Ship_Index = 1;
        bool Allow_Update = true;
        bool First_Load = true;
        int previousIndex = 0;
        int previousCount = 0;
        int previousInterval = 0;
        int CursorPosition = 0;
        string BeforeSplit = "";
        string previousHighlight = "";

        /// <summary>
        /// Loads game directory and subsequent files.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(Game_Directory))
            { MessageBox.Show("ERROR : The Light of Altair save directory does not exist at : " + Game_Directory); return; }
            SaveDirectory.Text = Game_Directory;
            try
            {
                foreach (string file in Directory.GetFiles(Game_Directory))
                    if (new FileInfo(file).Name.StartsWith("Game"))
                        GameList.Nodes.Add(new FileInfo(file).Name);
            }
            catch
            {
                MessageBox.Show("Could not find save directory change the save directory to where your light of altair save files are...");
                return;
            }
        }

        /// <summary>
        /// Loads a specified save game.
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            if (GameList.SelectedNode == null)
            {
                MessageBox.Show("Select a game or refresh the list.");
                return;
            }
            G = Game.Load_Game(Game_Directory + GameList.SelectedNode.Text);
            if (G == null)
            { MessageBox.Show("ERROR : The selected game is corrupt or in use by the game currently."); return; }
            DisableArrows();
            MissionName.Text = G.Name;
            MissionNumber.Text = G.mission_number.ToString();
            MissionDifficulty.Text = G.difficulty.ToString();
            MissionSpecifics.Text = G.Mission_specific_variables;
            Year.Text = G.year.ToString();
            Month.Text = G.month.ToString();
            Day.Text = G.day.ToString();
            PlayTime.Text = G.play_time.ToString();
            Factions.SelectedIndex = 0;
            G.Loaded = true;
            SaveGame.Enabled = true;
            tabControl1.Enabled = true;
            Update_Ships();
            Ship_Click((object)Corvette, new EventArgs());
            SectionSelector.SelectedIndex = previousIndex;
            G.ReloadSections();
            ReloadData();
            if (First_Load == true)
                SectionSelector.SelectedIndex = 0;
            First_Load = false;
            MessageBox.Show("Load successful...");
        }

        /// <summary>
        /// Changes the games save name.
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (G.Loaded)
                G.Name = MissionName.Text;
        }

        /// <summary>
        /// Changes the mission number.
        /// </summary>
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (G.Loaded)
            {
                try
                {
                    G.mission_number = Convert.ToInt32(MissionNumber.Text);
                }
                catch
                {
                    G.mission_number = 0;
                    MissionNumber.Text = "0";
                }
            }
        }

        /// <summary>
        /// Changes the difficulty setting.
        /// </summary>
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (G.Loaded)
            {
                if (MissionDifficulty.Text != "1" && MissionDifficulty.Text != "0")
                    MissionDifficulty.Text = "0";
                G.difficulty = Convert.ToInt32(MissionDifficulty.Text);
            }
        }

        /// <summary>
        /// Changes the mission specific variables.
        /// </summary>
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (G.Loaded)
                G.Mission_specific_variables = MissionSpecifics.Text;
        }

        /// <summary>
        /// Changes the year.
        /// </summary>
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (G.Loaded)
            {
                try
                {
                    G.year = Convert.ToInt32(Year.Text);
                }
                catch
                {
                    G.year = 1;
                    Year.Text = "1";
                }
            }
        }

        /// <summary>
        /// Changes the month.
        /// </summary>
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (G.Loaded)
            {
                try
                {
                    G.month = Convert.ToInt32(Month.Text);
                }
                catch
                {
                    G.month = 1;
                    Month.Text = "1";
                }
            }
        }

        /// <summary>
        /// Changes the day.
        /// </summary>
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (G.Loaded)
            {
                try
                {
                    G.day = Convert.ToInt32(Day.Text);
                }
                catch
                {
                    G.day = 1;
                    Day.Text = "1";
                }
            }
        }

        /// <summary>
        /// Changes the play time.
        /// </summary>
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (G.Loaded)
            {
                try
                {
                    G.play_time = Convert.ToDouble(PlayTime.Text).ToString();
                }
                catch
                {
                    G.play_time = "0";
                    PlayTime.Text = "0";
                }
            }
        }

        /// <summary>
        /// Updates the spending and income areas.
        /// </summary>
        public void Update_Transactions(object sender, EventArgs e)
        {
            if (Factions.SelectedIndex == -1)
                return;
            if (Spending.SelectedIndex <= -1)
                Spending.SelectedIndex = 0;
            if (Revenue.SelectedIndex <= -1)
                Revenue.SelectedIndex = 0;
            L_Spending.Text = G.Factions[Factions.SelectedIndex].Spending_List[Spending.SelectedIndex].ToString() + "$";
            L_Revenue.Text = G.Factions[Factions.SelectedIndex].Income_List[Revenue.SelectedIndex].ToString() + "$";
        }

        /// <summary>
        /// Saves the game.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            G.Save_Game(Game_Directory + GameList.SelectedNode.Text);
            MessageBox.Show("Save successful...");
        }

        /// <summary>
        /// Loads the financial info for a faction.
        /// </summary>
        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            if (G.Loaded)
            {
                Faction_Index = Factions.SelectedIndex;
                Spending.SelectedIndex = 0;
                Revenue.SelectedIndex = 0;
                Spending_Index = 0;
                Income_Index = 0;
                Allow_Update = false;
                L_Spending.Text = G.Factions[Faction_Index].Total_Spending.ToString() + "$";
                L_Revenue.Text = G.Factions[Faction_Index].Total_Income.ToString() + "$";
                UnknownOne.Text = G.Factions[Faction_Index].unknown_one.ToString();
                ExtraHappyness.Text = G.Factions[Faction_Index].Extra_happyness.ToString();
                UnknownTwo.Text = G.Factions[Faction_Index].unknown_two.ToString();
                ControledPlanets.Text = G.Factions[Faction_Index].Controled_Planets.ToString();
                BaseHappyness.Text = G.Factions[Faction_Index].Base_happyness.ToString();
                UnknownThree.Text = G.Factions[Faction_Index].unknown_three.ToString();
                UsedFuel.Text = G.Factions[Faction_Index].Fuel_Used.ToString();
                AvailableFuel.Text = G.Factions[Faction_Index].faction_fuel.ToString();
                Money.Text = G.Factions[Faction_Index].money.ToString();
                Faction_Flag.Image = G.Factions[Faction_Index].Faction_Flag;
                Allow_Update = true;
            }
        }

        /// <summary>
        /// Updates faction information.
        /// </summary>
        public void Update_Game(object sender, EventArgs e)
        {
            if (!Allow_Update)
                return;
            try
            {
                G.Factions[Faction_Index].unknown_one = Convert.ToDouble(UnknownOne.Text);
                G.Factions[Faction_Index].Extra_happyness = Convert.ToDouble(ExtraHappyness.Text);
                G.Factions[Faction_Index].unknown_two = Convert.ToDouble(UnknownTwo.Text);
                G.Factions[Faction_Index].Controled_Planets = Convert.ToDouble(ControledPlanets.Text);
                G.Factions[Faction_Index].Base_happyness = Convert.ToDouble(BaseHappyness.Text);
                G.Factions[Faction_Index].unknown_three = Convert.ToDouble(UnknownThree.Text);
                G.Factions[Faction_Index].Fuel_Used = Convert.ToDouble(UsedFuel.Text);
                G.Factions[Faction_Index].faction_fuel = Convert.ToDouble(AvailableFuel.Text);
                G.Factions[Faction_Index].money = Convert.ToDouble(Money.Text);
                G.Factions[Faction_Index].Update_Faction_Data();
            }
            catch
            {
                ((TextBox)sender).Undo();
            }
        }

        /// <summary>
        /// Refreshes the save game list.
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                GameList.Nodes.Clear();
                foreach (string file in Directory.GetFiles(Game_Directory))
                    if (new FileInfo(file).Name.StartsWith("Game"))
                        GameList.Nodes.Add(new FileInfo(file).Name);
            }
            catch
            {
                MessageBox.Show("Could not find save directory change the save directory to where your light of altair save files are...");
                return;
            }
        }

        /// <summary>
        /// Deletes a game.
        /// </summary>
        private void button5_Click(object sender, EventArgs e)
        {
            if (GameList.SelectedNode == null)
            {
                MessageBox.Show("Select a game or refresh the list.");
                return;
            }
            File.Delete(Game_Directory + GameList.SelectedNode.Text);
            GameList.Nodes.Clear();
            foreach (string file in Directory.GetFiles(Game_Directory))
                if (new FileInfo(file).Name.StartsWith("Game"))
                    GameList.Nodes.Add(new FileInfo(file).Name);
        }

        /// <summary>
        /// Updates the game directory and the save game list.
        /// </summary>
        private void textBox21_Leave(object sender, EventArgs e)
        {
            Game_Directory = SaveDirectory.Text;
            try
            {
                GameList.Nodes.Clear();
                foreach (string file in Directory.GetFiles(Game_Directory))
                    if (new FileInfo(file).Name.StartsWith("Game"))
                        GameList.Nodes.Add(new FileInfo(file).Name);
            }
            catch
            {
                MessageBox.Show("Could not find save directory change the save directory to where your light of altair save files are...");
                return;
            }
        }

        /// <summary>
        /// Updates/loads the ship images.
        /// </summary>
        void Update_Ships()
        {
            Fighter.Image = G.Designs[0].Ship_Image;
            Corvette.Image = G.Designs[1].Ship_Image;
            Frigate.Image = G.Designs[2].Ship_Image;
            Capital.Image = G.Designs[3].Ship_Image;
            Starbase.Image = G.Designs[4].Ship_Image;
        }

        /// <summary>
        /// Updates the save game list and ship images.
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                int selected = -1;
                if (!(GameList.SelectedNode == null))
                    selected = GameList.SelectedNode.Index;
                GameList.Nodes.Clear();
                foreach (string file in Directory.GetFiles(Game_Directory))
                    if (new FileInfo(file).Name.StartsWith("Game"))
                        GameList.Nodes.Add(new FileInfo(file).Name);
                if (selected != -1)
                    GameList.SelectedNode = GameList.Nodes[selected];
                Update_Ships();
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// Switches out a weapon and in a new one.
        /// </summary>
        private void WeaponsLeftArrow_Click(object sender, EventArgs e)
        {
            int TagIndex = Convert.ToInt32(((PictureBox)sender).Tag);
            if (G.Designs[Ship_Index].Offense[TagIndex] <= 1)
                return;
            G.Designs[Ship_Index].Offense[TagIndex]--;
            DrawWeapons();
            G.Designs[Ship_Index].Update_Design_Data();
        }

        /// <summary>
        /// Switches out a Defense and in a new one.
        /// </summary>
        private void DefenseLeftArrow_Click(object sender, EventArgs e)
        {
            int TagIndex = Convert.ToInt32(((PictureBox)sender).Tag);
            if (G.Designs[Ship_Index].Defense[TagIndex] <= 1)
                return;
            G.Designs[Ship_Index].Defense[TagIndex]--;
            DrawDefenses();
            G.Designs[Ship_Index].Update_Design_Data();
        }

        /// <summary>
        /// Switches out a weapon and in a new one.
        /// </summary>
        private void WeaponsRightArrow_Click(object sender, EventArgs e)
        {
            int TagIndex = Convert.ToInt32(((PictureBox)sender).Tag);
            if (G.Designs[Ship_Index].Offense[TagIndex] == 0 || G.Designs[Ship_Index].Offense[TagIndex] == 48)
                return;
            G.Designs[Ship_Index].Offense[TagIndex]++;
            DrawWeapons();
            G.Designs[Ship_Index].Update_Design_Data();
        }

        /// <summary>
        /// Switches out a Defense and in a new one.
        /// </summary>
        private void DefenseRightArrow_Click(object sender, EventArgs e)
        {
            int TagIndex = Convert.ToInt32(((PictureBox)sender).Tag);
            if (G.Designs[Ship_Index].Defense[TagIndex] == 0 || G.Designs[Ship_Index].Defense[TagIndex] == 11)
                return;
            G.Designs[Ship_Index].Defense[TagIndex]++;
            DrawDefenses();
            G.Designs[Ship_Index].Update_Design_Data();
        }

        /// <summary>
        /// Redraws the weapons.
        /// </summary>
        void DrawWeapons()
        {
            Weapon1.Image = G.Designs[Ship_Index].DrawWeapon(G.Designs[Ship_Index].Offense[0]);
            Weapon2.Image = G.Designs[Ship_Index].DrawWeapon(G.Designs[Ship_Index].Offense[1]);
            Weapon3.Image = G.Designs[Ship_Index].DrawWeapon(G.Designs[Ship_Index].Offense[2]);
            Weapon4.Image = G.Designs[Ship_Index].DrawWeapon(G.Designs[Ship_Index].Offense[3]);
            Weapon5.Image = G.Designs[Ship_Index].DrawWeapon(G.Designs[Ship_Index].Offense[4]);
            Weapon6.Image = G.Designs[Ship_Index].DrawWeapon(G.Designs[Ship_Index].Offense[5]);
            Weapon7.Image = G.Designs[Ship_Index].DrawWeapon(G.Designs[Ship_Index].Offense[6]);
            Weapon8.Image = G.Designs[Ship_Index].DrawWeapon(G.Designs[Ship_Index].Offense[7]);
        }

        /// <summary>
        /// Redraws the Defenses.
        /// </summary>
        void DrawDefenses()
        {
            Defense1.Image = G.Designs[Ship_Index].DrawDefense(G.Designs[Ship_Index].Defense[0]);
            Defense2.Image = G.Designs[Ship_Index].DrawDefense(G.Designs[Ship_Index].Defense[1]);
            Defense3.Image = G.Designs[Ship_Index].DrawDefense(G.Designs[Ship_Index].Defense[2]);
            Defense4.Image = G.Designs[Ship_Index].DrawDefense(G.Designs[Ship_Index].Defense[3]);
            Defense5.Image = G.Designs[Ship_Index].DrawDefense(G.Designs[Ship_Index].Defense[4]);
            Defense6.Image = G.Designs[Ship_Index].DrawDefense(G.Designs[Ship_Index].Defense[5]);
            Defense7.Image = G.Designs[Ship_Index].DrawDefense(G.Designs[Ship_Index].Defense[6]);
            Defense8.Image = G.Designs[Ship_Index].DrawDefense(G.Designs[Ship_Index].Defense[7]);
        }

        /// <summary>
        /// Enables the ship editor for the user.
        /// </summary>
        void EnableArrows()
        {
            ChangeLeft1.Enabled = true;
            ChangeRight1.Enabled = true;
            ChangeLeft2.Enabled = true;
            ChangeRight2.Enabled = true;
            ChangeRight9.Enabled = true;
            ChangeLeft9.Enabled = true;
            ChangeRight10.Enabled = true;
            ChangeLeft10.Enabled = true;
            ChangeRight3.Enabled = true;
            ChangeLeft3.Enabled = true;
            ChangeRight11.Enabled = true;
            ChangeLeft11.Enabled = true;
            ChangeRight4.Enabled = true;
            ChangeLeft4.Enabled = true;
            ChangeRight12.Enabled = true;
            ChangeLeft12.Enabled = true;
            ChangeRight5.Enabled = true;
            ChangeLeft5.Enabled = true;
            ChangeRight13.Enabled = true;
            ChangeLeft13.Enabled = true;
            ChangeRight6.Enabled = true;
            ChangeLeft6.Enabled = true;
            ChangeRight14.Enabled = true;
            ChangeLeft14.Enabled = true;
            ChangeRight7.Enabled = true;
            ChangeLeft7.Enabled = true;
            ChangeRight15.Enabled = true;
            ChangeLeft15.Enabled = true;
            ChangeRight8.Enabled = true;
            ChangeLeft8.Enabled = true;
            ChangeRight16.Enabled = true;
            ChangeLeft16.Enabled = true;
            Prototyped.Enabled = true;
        }

        /// <summary>
        /// Disables the arrows.
        /// </summary>
        void DisableArrows()
        {
            ChangeLeft1.Enabled = false;
            ChangeRight1.Enabled = false;
            ChangeLeft2.Enabled = false;
            ChangeRight2.Enabled = false;
            ChangeRight9.Enabled = false;
            ChangeLeft9.Enabled = false;
            ChangeRight10.Enabled = false;
            ChangeLeft10.Enabled = false;
            ChangeRight3.Enabled = false;
            ChangeLeft3.Enabled = false;
            ChangeRight11.Enabled = false;
            ChangeLeft11.Enabled = false;
            ChangeRight4.Enabled = false;
            ChangeLeft4.Enabled = false;
            ChangeRight12.Enabled = false;
            ChangeLeft12.Enabled = false;
            ChangeRight5.Enabled = false;
            ChangeLeft5.Enabled = false;
            ChangeRight13.Enabled = false;
            ChangeLeft13.Enabled = false;
            ChangeRight6.Enabled = false;
            ChangeLeft6.Enabled = false;
            ChangeRight14.Enabled = false;
            ChangeLeft14.Enabled = false;
            ChangeRight7.Enabled = false;
            ChangeLeft7.Enabled = false;
            ChangeRight15.Enabled = false;
            ChangeLeft15.Enabled = false;
            ChangeRight8.Enabled = false;
            ChangeLeft8.Enabled = false;
            ChangeRight16.Enabled = false;
            ChangeLeft16.Enabled = false;
            Prototyped.Enabled = false;
        }

        /// <summary>
        /// Selects a new ship to modify.
        /// </summary>
        private void Ship_Click(object sender, EventArgs e)
        {
            if (((PictureBox)sender).Image == null)
                return;
            EnableArrows();
            int index = Convert.ToInt32(((PictureBox)sender).Tag);
            Prototyped.Checked = G.Designs[index].Prototyped;
            CenterStage.Image = ((PictureBox)sender).Image;
            Ship_Index = index;
            ShipSelected.Text = "Ship selected : " + G.Designs[index].Ship_Name;
            DrawWeapons();
            DrawDefenses();
        }

        /// <summary>
        /// Changes the prototype status.
        /// </summary>
        private void Prototyped_CheckedChanged(object sender, EventArgs e)
        {
            G.Designs[Ship_Index].Prototyped = Prototyped.Checked;
            if (Prototyped.Checked)
                G.Designs[Ship_Index].Prototype = 1;
            else
                G.Designs[Ship_Index].Prototype = 0;
            G.Designs[Ship_Index].Update_Design_Data();
        }

        /// <summary>
        /// Sends feedback to naate@taloreal.com for me to review bugs or other potential suggestions.
        /// </summary>
        private void Send_Click(object sender, EventArgs e)
        {
            if (MailFrom.Text == "" || MailSubject.Text == "" || MailMessage.Text == "")
            {
                MessageBox.Show("Missing a field...");
                return;
            }
            string From = MailFrom.Text;
            string Subject = MailSubject.Text;
            string Message = MailMessage.Text;
            try
            {
                SmtpClient client2 = new SmtpClient();
                client2.Port = 587;
                client2.Host = "smtp.gmail.com";
                client2.EnableSsl = true;
                client2.Timeout = 10000;
                client2.DeliveryMethod = SmtpDeliveryMethod.Network;
                client2.UseDefaultCredentials = false;
                client2.Credentials = new System.Net.NetworkCredential("naate222@gmail.com", "deadless");
                MailMessage mm = new MailMessage("naate222@gmail.com", "naate@taloreal.com", Subject, From + " says : " + "\r\n" + Message);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client2.Send(mm);
                MessageBox.Show("Feedback sent.");
            }
            catch
            {
                MessageBox.Show("We're sorry but your message could not be sent...");
                return;
            }
        }

        /// <summary>
        /// Loads the active profile and then opens up the profile editor in a new window.
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Game_Directory + "Profile.prf"))
            { MessageBox.Show("Error profile file does not exist."); return; }
            Profile P = Profile.Load(Game_Directory + "Profile.prf");
            if (P == null)
            { MessageBox.Show("ERROR : The profile file either doesn't exist or is in use currently."); return; }
            new ProfileEditor(P).Show();
        }

        /// <summary>
        /// Loads the keyboard shortcut editor.
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            string settingsPath = Game_Directory;
            List<string> parts = General_Usage_Methods.Split(settingsPath, '\\');
            settingsPath = "";
            for (int i = 0; i != parts.Count - 1; i++)
                settingsPath = settingsPath + parts[i] +"\\";
            settingsPath = settingsPath + "settings.ini";
            new KeyboardSettings(settingsPath).Show();
        }

        /// <summary>
        /// Updates the game directory.
        /// </summary>
        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            if (!Directory.Exists(SaveDirectory.Text))
                return;
            Game_Directory = SaveDirectory.Text;
        }

        private void SectionSelector_SelectedItemChanged(object sender, EventArgs e)
        {
            if (!G.Loaded)
                return;
            if (!G.Game_Sections[previousIndex].CanSet(DataEditor.Text))
            {
                MessageBox.Show("You must adhere to the data constraints for section : " + G.Game_Sections[previousIndex].Name);
                SectionSelector.SelectedIndex = previousIndex; 
                return; 
            } 
            if (SectionSelector.SelectedIndex == -1)
                SectionSelector.SelectedIndex = 0;
            previousIndex = SectionSelector.SelectedIndex;
            ReloadData();
        }

        void ReloadData()
        {
            int index = SectionSelector.SelectedIndex;
            DataEditor.Text = G.Game_Sections[index].Remembered;
        }

        private void SaveData_Click(object sender, EventArgs e)
        {
            if (!G.Game_Sections[previousIndex].CanSet(DataEditor.Text))
            {
                MessageBox.Show("You must adhere to the data constraints for section : " + G.Game_Sections[previousIndex].Name);
                return;
            }
            Game G2 = Game.Load_Game(G.Game_Sections);
            if (G2 == null)
            {
                MessageBox.Show("ERROR : could not load changes please ensure placeholders are not changed.");
                return;
            }
            G = G2;
            MessageBox.Show("Changes successfully applied.");
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            G.ReloadSections();
            MissionName.Text = G.Name;
            MissionNumber.Text = G.mission_number.ToString();
            MissionDifficulty.Text = G.difficulty.ToString();
            MissionSpecifics.Text = G.Mission_specific_variables;
            Year.Text = G.year.ToString();
            Month.Text = G.month.ToString();
            Day.Text = G.day.ToString();
            PlayTime.Text = G.play_time.ToString();
            UnknownOne.Text = G.Factions[Faction_Index].unknown_one.ToString();
            ExtraHappyness.Text = G.Factions[Faction_Index].Extra_happyness.ToString();
            UnknownTwo.Text = G.Factions[Faction_Index].unknown_two.ToString();
            ControledPlanets.Text = G.Factions[Faction_Index].Controled_Planets.ToString();
            BaseHappyness.Text = G.Factions[Faction_Index].Base_happyness.ToString();
            UnknownThree.Text = G.Factions[Faction_Index].unknown_three.ToString();
            UsedFuel.Text = G.Factions[Faction_Index].Fuel_Used.ToString();
            AvailableFuel.Text = G.Factions[Faction_Index].faction_fuel.ToString();
            Money.Text = G.Factions[Faction_Index].money.ToString();
            Faction_Flag.Image = G.Factions[Faction_Index].Faction_Flag;
            DrawWeapons();
            DrawDefenses(); 
            Update_Ships();
        }

        private void DataEditor_Click(object sender, EventArgs e)
        {
            int position = ((RichTextBox)sender).SelectionStart;
            CursorPos.Text = "Cursor Position : " + position;
            CursorPosition = position;
        }

        private void DataEditor_KeyUp(object sender, KeyEventArgs e)
        {
            int position = ((RichTextBox)sender).SelectionStart;
            CursorPos.Text = "Cursor Position : " + position;
            CursorPosition = position;
        }

        private void Seperator_TextChanged(object sender, EventArgs e)
        {
            Seperator.Text = Seperator.Text[0].ToString();
        }

        private void Count_TextChanged(object sender, EventArgs e)
        {
            if ((!IsNumber(Count.Text) || Convert.ToInt32(Count.Text) < 1) || !FailSafe(DataEditor.Text.Substring(CursorPosition), Seperator.Text[0], Convert.ToInt32(Interval.Text), Convert.ToInt32(Count.Text)))
            { Count.Text = (previousCount + 1).ToString(); return; }
            previousCount = Convert.ToInt32(Count.Text) - 1;
        }

        bool IsNumber(string text)
        {
            try
            {
                Convert.ToInt32(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!FailSafe(DataEditor.Text.Substring(CursorPosition), Seperator.Text[0], Convert.ToInt32(Interval.Text), Convert.ToInt32(Count.Text)))
                return;
            string splitbuffer = DataEditor.Text.Substring(0, CursorPosition);
            string text = DataEditor.Text.Substring(CursorPosition);
            List<string> items = General_Usage_Methods.UserSplit(text, Seperator.Text[0], previousInterval, previousCount);
            string collect = "";
            foreach (string s in items)
                collect = collect + s + "\r\n";
            if (!G.Game_Sections[previousIndex].CanSet(collect))
            {
                MessageBox.Show("Please fix variable count errors for this section before spliting.");
                return;
            }
            BeforeSplit = DataEditor.Text;
            Undo.Enabled = true;
            DataEditor.Text = splitbuffer + collect;
        }

        private void Interval_TextChanged(object sender, EventArgs e)
        {
            if ((!IsNumber(Interval.Text) || Convert.ToInt32(Interval.Text) < 1) || !FailSafe(DataEditor.Text.Substring(CursorPosition), Seperator.Text[0], Convert.ToInt32(Interval.Text), Convert.ToInt32(Count.Text)))
            { Interval.Text = (previousInterval + 1).ToString(); return; }
            previousInterval = Convert.ToInt32(Interval.Text) - 1;
        }

        bool FailSafe(string text, char seperator, int interval, int count)
        {
            return (General_Usage_Methods.Split(text, seperator).Count >= (interval * count));
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            Undo.Enabled = false;
            DataEditor.Text = BeforeSplit;
        }

        private void GameList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Delete.Enabled = true;
            LoadGame.Enabled = true;
            Selected_Save.Text = "@ " + GameList.SelectedNode.Text;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (Combiner.Text == "")
                return;
            DataEditor.Text = DataEditor.Text.Replace(Combiner.Text, "");
        }

        /// <summary>
        /// Highlights paterns in the data editor.
        /// </summary>
        /// <param name="pattern">The text patern to follow.</param>
        /// <param name="colorin">The color to highlight.</param>
        void Highlight(string pattern, Color colorin)
        {
            MatchCollection matches = Regex.Matches(DataEditor.Text, pattern);
            foreach (Match match in matches)
            {
                DataEditor.SelectionStart = match.Index;
                DataEditor.SelectionLength = match.Length;
                DataEditor.SelectionBackColor = colorin;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (Highlighter.Text == "")
                return;
            Highlight(previousHighlight, Color.White);
            Highlight(Highlighter.Text, Color.Yellow);
            previousHighlight = Highlighter.Text;
        }
    }
}
