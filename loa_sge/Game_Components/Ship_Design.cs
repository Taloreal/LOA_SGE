using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using LOA_SGE.Game_Components;

namespace LOA_SGE.Game_Components
{
    public class Ship_Design
    {
        List<Ship_Design> Self_Implication = new List<Ship_Design>();
        public string Design_Data = "";
        public string Ship_Name = "";
        public double D_Ship_Image = 0;
        public Image I_Ship_Image;
        public int DesIndex = 0;
        public Image Ship_Image;
        public static List<string> Names = new List<string> { "Fighter", "Corvette", "Friggate", "Cruiser", "Starbase", "non", "non", "non", "non", "non" };
        public static List<string> Listed_Weapons = new List<string> { "RailGun", "Gauss Guns", "missile", "Rocket", "laser", "laser turret", "Gauss Turret", "Rocket pack", "Defensive Missiles", "Torpedoe", "Lance", "Lance Turret" };
        public static List<int> W_Indexs = new List<int> { 40, 37, 1, 8, 19, 20, 38, 2, 3, 14, 28, 29 };
        public static List<string> Listed_Defenses = new List<string> { "Shield Generator", "Armor", "Energized Plating", "Fighter Bay", "ECM System" };
        public static List<int> D_Indexs = new List<int> { 6, 1, 3, 2, 4 };
        public const int initializer = 101;
        public int Ship_type = 0;
        public int unknownone = 0;
        public bool Prototyped = true;
        public int Prototype = 0;
        public int[] Offense = new int[8];
        public string[] Weapons = new string[8];
        public int[] Defense = new int[8];
        public string[] Defenses = new string[8];

        /// <summary>
        /// Sets the weapons of the ship design.
        /// </summary>
        /// <param name="SD">The ship design to assign to.</param>
        /// <param name="Design_D">The design configuration.</param>
        private static void Set_Weapons(Ship_Design SD, string Design_D)
        {
            SD.Offense[0] = Convert.ToInt32(Design_D.Split(' ')[4]);
            for (int i = 0; i != W_Indexs.Count; i++)
                if (W_Indexs[i] == SD.Offense[0])
                    SD.Weapons[0] = Ship_Design.Listed_Weapons[i];
            SD.Offense[1] = Convert.ToInt32(Design_D.Split(' ')[6]);
            for (int i = 0; i != W_Indexs.Count; i++)
                if (W_Indexs[i] == SD.Offense[1])
                    SD.Weapons[1] = Ship_Design.Listed_Weapons[i];
            SD.Offense[2] = Convert.ToInt32(Design_D.Split(' ')[8]);
            for (int i = 0; i != W_Indexs.Count; i++)
                if (W_Indexs[i] == SD.Offense[2])
                    SD.Weapons[2] = Ship_Design.Listed_Weapons[i];
            SD.Offense[3] = Convert.ToInt32(Design_D.Split(' ')[10]);
            for (int i = 0; i != W_Indexs.Count; i++)
                if (W_Indexs[i] == SD.Offense[3])
                    SD.Weapons[3] = Ship_Design.Listed_Weapons[i];
            SD.Offense[4] = Convert.ToInt32(Design_D.Split(' ')[12]);
            for (int i = 0; i != W_Indexs.Count; i++)
                if (W_Indexs[i] == SD.Offense[4])
                    SD.Weapons[4] = Ship_Design.Listed_Weapons[i];
            SD.Offense[5] = Convert.ToInt32(Design_D.Split(' ')[14]);
            for (int i = 0; i != W_Indexs.Count; i++)
                if (W_Indexs[i] == SD.Offense[5])
                    SD.Weapons[5] = Ship_Design.Listed_Weapons[i];
            SD.Offense[6] = Convert.ToInt32(Design_D.Split(' ')[16]);
            for (int i = 0; i != W_Indexs.Count; i++)
                if (W_Indexs[i] == SD.Offense[6])
                    SD.Weapons[6] = Ship_Design.Listed_Weapons[i];
            SD.Offense[7] = Convert.ToInt32(Design_D.Split(' ')[18]);
            for (int i = 0; i != W_Indexs.Count; i++)
                if (W_Indexs[i] == SD.Offense[7])
                    SD.Weapons[7] = Ship_Design.Listed_Weapons[i];
        }

        /// <summary>
        /// Sets the defenses of the ship design.
        /// </summary>
        /// <param name="SD">The ship design to assign to.</param>
        /// <param name="Design_D">The design configuration.</param>
        private static void Set_Defenses(Ship_Design SD, string Design_D)
        {
            SD.Defense[0] = Convert.ToInt32(Design_D.Split(' ')[5]);
            for (int i = 0; i != D_Indexs.Count; i++)
                if (D_Indexs[i] == SD.Defense[0])
                    SD.Defenses[0] = Ship_Design.Listed_Defenses[i];
            SD.Defense[1] = Convert.ToInt32(Design_D.Split(' ')[7]);
            for (int i = 0; i != D_Indexs.Count; i++)
                if (D_Indexs[i] == SD.Defense[1])
                    SD.Defenses[1] = Ship_Design.Listed_Defenses[i];
            SD.Defense[2] = Convert.ToInt32(Design_D.Split(' ')[9]);
            for (int i = 0; i != D_Indexs.Count; i++)
                if (D_Indexs[i] == SD.Defense[2])
                    SD.Defenses[2] = Ship_Design.Listed_Defenses[i];
            SD.Defense[3] = Convert.ToInt32(Design_D.Split(' ')[11]);
            for (int i = 0; i != D_Indexs.Count; i++)
                if (D_Indexs[i] == SD.Defense[3])
                    SD.Defenses[3] = Ship_Design.Listed_Defenses[i];
            SD.Defense[4] = Convert.ToInt32(Design_D.Split(' ')[13]);
            for (int i = 0; i != D_Indexs.Count; i++)
                if (D_Indexs[i] == SD.Defense[4])
                    SD.Defenses[4] = Ship_Design.Listed_Defenses[i];
            SD.Defense[5] = Convert.ToInt32(Design_D.Split(' ')[15]);
            for (int i = 0; i != D_Indexs.Count; i++)
                if (D_Indexs[i] == SD.Defense[5])
                    SD.Defenses[5] = Ship_Design.Listed_Defenses[i];
            SD.Defense[6] = Convert.ToInt32(Design_D.Split(' ')[17]);
            for (int i = 0; i != D_Indexs.Count; i++)
                if (D_Indexs[i] == SD.Defense[6])
                    SD.Defenses[6] = Ship_Design.Listed_Defenses[i];
            SD.Defense[7] = Convert.ToInt32(Design_D.Split(' ')[19]);
            for (int i = 0; i != D_Indexs.Count; i++)
                if (D_Indexs[i] == SD.Defense[7])
                    SD.Defenses[7] = Ship_Design.Listed_Defenses[i];
        }

        /// <summary>
        /// Creates a new Ship Design object used in the saved game.
        /// </summary>
        /// <param name="Design_Index">The design index.</param>
        /// <param name="Design_D">The design configuration.</param>
        /// <returns>A new Ship Design object.</returns>
        public static Ship_Design Create_Design_From_Feed(int Design_Index, string Design_D)
        {
            Ship_Design SD = new Ship_Design();
            SD.Self_Implication.Add(SD);
            SD.DesIndex = Design_Index;
            SD.Design_Data = Design_D;
            SD.Ship_Name = Names[Design_Index];
            SD.D_Ship_Image = Convert.ToInt32(Design_D.Split(' ')[1]);
            SD.unknownone = Convert.ToInt32(Design_D.Split(' ')[2]);
            SD.Prototyped = (Convert.ToInt32(Design_D.Split(' ')[3]) == 1);
            SD.Prototype = Convert.ToInt32(Design_D.Split(' ')[3]);
            SD.Ship_Image = SD.GetShipImage(Convert.ToInt32(SD.D_Ship_Image));
            Set_Weapons(SD, Design_D);
            Set_Defenses(SD, Design_D);
            return SD;
        }

        /// <summary>
        /// Gets the image for the ship.
        /// </summary>
        /// <param name="Ship_Index">The ship index.</param>
        /// <returns>The ship image.</returns>
        public Image GetShipImage(int Ship_Index)
        {
            if (Ship_Index == -1)
                return null;
            int row = 0;
            int column = Ship_Index * 68;
            Point P = new Point(column, row);
            Size S = new Size(68, 68);
            return General_Usage_Methods.cropImage((Image)(global::LOA_SGE.Properties.Resources.ships), P, S);
        }

        /// <summary>
        /// Draws a new Weapon.
        /// </summary>
        /// <param name="Weapon_Index">The weapon index.</param>
        /// <returns>The weapon image.</returns>
        public Image DrawWeapon(int Weapon_Index)
        {
            Weapon_Index--;
            if (Weapon_Index == -1)
                return null;
            int Row = ((int)(Weapon_Index / 10)) * 68;
            int Column = (Weapon_Index % 10) * 68;
            Point P = new Point(Column, Row);
            Size S = new Size(68, 68);
            return General_Usage_Methods.cropImage((Image)(global::LOA_SGE.Properties.Resources.Weapons), P, S);
        }

        /// <summary>
        /// Draws a new defense.
        /// </summary>
        /// <param name="Weapon_Index">The defense index.</param>
        /// <returns>The defense image.</returns>
        public Image DrawDefense(int Defense_Index)
        {
            Defense_Index--;
            if (Defense_Index == -1)
                return null;
            int row = 0;
            int column = Defense_Index * 68;
            Point P = new Point(column, row);
            Size S = new Size(68, 68);
            return General_Usage_Methods.cropImage((Image)(global::LOA_SGE.Properties.Resources.Defenses), P, S);
        }

        /// <summary>
        /// Updates the design summary.
        /// </summary>
        public void Update_Design_Data()
        {
            Design_Data = initializer + " " + D_Ship_Image + " " + unknownone + " " + Prototype + " " +
                Offense[0] + " " + Defense[0] + " " + Offense[1] + " " + Defense[1] + " " +
                Offense[2] + " " + Defense[2] + " " + Offense[3] + " " + Defense[3] + " " +
                Offense[4] + " " + Defense[4] + " " + Offense[5] + " " + Defense[5] + " " +
                Offense[6] + " " + Defense[6] + " " + Offense[7] + " " + Defense[7] + " ";
            Set_Weapons(this, Design_Data);
            Set_Defenses(this, Design_Data);
        }

        /// <summary>
        /// Summarizes the data foreach ship design for saving.
        /// </summary>
        /// <param name="Designs">The ship designs to summarize.</param>
        /// <returns>The summary to which to save.</returns>
        public static string Summary(Ship_Design[] Designs)
        {
            string temp = "";
            foreach (Ship_Design SD in Designs)
                temp = temp + SD.Design_Data;
            return temp;
        }
    }
}
