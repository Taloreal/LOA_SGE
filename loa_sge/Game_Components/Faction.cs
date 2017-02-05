using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using LOA_SGE.Game_Components;

namespace LOA_SGE.Game_Components
{
    public class Faction
    {
        public static List<string> Factions = new List<string> { "UEO", "Colonial", "European", "Asian", "American", "African", "Independant", "Pirate", "Altairians", "Seekers" };
        public List<double> Spending_List = new List<double>();
        public List<double> Income_List = new List<double>();
        public int Faction_Index = 0;
        public string Faction_Data = "";
        public string Faction_Name = "";
        public Image Faction_Flag;
        public const double Initializer = 27;
        public double money = 10000000;
        public double reserve_Funds = 100000;
        public double Total_Spending = 0;
        public double building_cost = 0;
        public double Research_spending = 0;
        public double fleet_cost = 0;
        public double production_cost = 0;
        public double transport_cost = 0;
        public double upkeep_cost = 0;
        public double UEO_Taxes = 0;
        public double Altarian_Trade_Taxes = 0;
        public double Unrest_cost = 0;
        public double Total_Income = 0;
        public double Colony_Tax = 0;
        public double City_Tax = 0;
        public double Trade_Income = 0;
        public double Martial_Law_Funding = 0;
        public double UEO_Support_Income = 0;
        public double Colonial_Support_Income = 0;
        public double European_Support_Income = 0;
        public double Asian_Support_Income = 0;
        public double American_Support_Income = 0;
        public double African_Support_Income = 0;
        public double Independant_Support_Income = 0;
        public double Pirates_Support_Income = 0;
        public double Alterian_Support_Income = 0;
        public double Seekers_Support_Income = 0;
        public double unknown_one = 0;
        public double Extra_happyness = 0;
        public double unknown_two = 0;
        public double Controled_Planets = 0;
        public double Base_happyness = 50;
        public double unknown_three = 0;
        public double Fuel_Used = 0;
        public double faction_fuel = 0;
        public static string Enders = "0 10 ";

        /// <summary>
        /// Combines the items of spending and income information.
        /// </summary>
        public void Compile_lists()
        {
            Spending_List.Add(Total_Spending);
            Spending_List.Add(building_cost);
            Spending_List.Add(Research_spending);
            Spending_List.Add(fleet_cost);
            Spending_List.Add(production_cost);
            Spending_List.Add(transport_cost);
            Spending_List.Add(upkeep_cost);
            Spending_List.Add(UEO_Taxes);
            Spending_List.Add(Altarian_Trade_Taxes);
            Spending_List.Add(Unrest_cost);
            Income_List.Add(Total_Income);
            Income_List.Add(Colony_Tax);
            Income_List.Add(City_Tax);
            Income_List.Add(Trade_Income);
            Income_List.Add(Martial_Law_Funding);
            Income_List.Add(UEO_Support_Income);
            Income_List.Add(Colonial_Support_Income);
            Income_List.Add(European_Support_Income);
            Income_List.Add(Asian_Support_Income);
            Income_List.Add(American_Support_Income);
            Income_List.Add(African_Support_Income);
            Income_List.Add(Independant_Support_Income);
            Income_List.Add(Pirates_Support_Income);
            Income_List.Add(Alterian_Support_Income);
            Income_List.Add(Seekers_Support_Income);
        }

        /// <summary>
        /// Reads text to generate a faction with a specified index.
        /// </summary>
        /// <param name="Faction_Index">The "Faction" identity essentally. </param>
        /// <param name="Faction_D">The string of which to split to get info for faction.</param>
        /// <returns>A faction which is in the game.</returns>
        public static Faction Create_Faction_From_Feed(int Faction_Index, string Faction_D)
        {
            Faction F = new Faction();
            F.Faction_Index = Faction_Index;
            F.Faction_Data = Faction_D;
            F.Faction_Flag = F.Get_Faction_Flag(Faction_Index);
            F.Faction_Name = Faction.Factions[Faction_Index];
            F.money = Convert.ToDouble(Faction_D.Split(' ')[1]);
            F.reserve_Funds = Convert.ToDouble(Faction_D.Split(' ')[2]);
            F.Total_Spending = Convert.ToDouble(Faction_D.Split(' ')[3]);
            F.building_cost = Convert.ToDouble(Faction_D.Split(' ')[4]);
            F.Research_spending = Convert.ToDouble(Faction_D.Split(' ')[5]);
            F.fleet_cost = Convert.ToDouble(Faction_D.Split(' ')[6]);
            F.production_cost = Convert.ToDouble(Faction_D.Split(' ')[7]);
            F.transport_cost = Convert.ToDouble(Faction_D.Split(' ')[8]);
            F.upkeep_cost = Convert.ToDouble(Faction_D.Split(' ')[9]);
            F.UEO_Taxes = Convert.ToDouble(Faction_D.Split(' ')[10]);
            F.Altarian_Trade_Taxes = Convert.ToDouble(Faction_D.Split(' ')[11]);
            F.Unrest_cost = Convert.ToDouble(Faction_D.Split(' ')[12]);
            F.Total_Income = Convert.ToDouble(Faction_D.Split(' ')[13]);
            F.Colony_Tax = Convert.ToDouble(Faction_D.Split(' ')[14]);
            F.City_Tax = Convert.ToDouble(Faction_D.Split(' ')[15]);
            F.Trade_Income = Convert.ToDouble(Faction_D.Split(' ')[16]);
            F.Martial_Law_Funding = Convert.ToDouble(Faction_D.Split(' ')[17]);
            F.UEO_Support_Income = Convert.ToDouble(Faction_D.Split(' ')[18]);
            F.Colonial_Support_Income = Convert.ToDouble(Faction_D.Split(' ')[19]);
            F.European_Support_Income = Convert.ToDouble(Faction_D.Split(' ')[20]);
            F.Asian_Support_Income = Convert.ToDouble(Faction_D.Split(' ')[21]);
            F.American_Support_Income = Convert.ToDouble(Faction_D.Split(' ')[22]);
            F.African_Support_Income = Convert.ToDouble(Faction_D.Split(' ')[23]);
            F.Independant_Support_Income = Convert.ToDouble(Faction_D.Split(' ')[24]);
            F.Pirates_Support_Income = Convert.ToDouble(Faction_D.Split(' ')[25]);
            F.Alterian_Support_Income = Convert.ToDouble(Faction_D.Split(' ')[26]);
            F.Seekers_Support_Income = Convert.ToDouble(Faction_D.Split(' ')[27]);
            F.unknown_one = Convert.ToDouble(Faction_D.Split(' ')[28]);
            F.Extra_happyness = Convert.ToDouble(Faction_D.Split(' ')[29]);
            F.unknown_two = Convert.ToDouble(Faction_D.Split(' ')[30]);
            F.Controled_Planets = Convert.ToDouble(Faction_D.Split(' ')[31]);
            F.Base_happyness = Convert.ToDouble(Faction_D.Split(' ')[32]);
            F.unknown_three = Convert.ToDouble(Faction_D.Split(' ')[33]);
            F.Fuel_Used = Convert.ToDouble(Faction_D.Split(' ')[34]);
            F.faction_fuel = Convert.ToDouble(Faction_D.Split(' ')[35]);
            F.Compile_lists();
            return F;
        }

        /// <summary>
        /// Gets the factions flag then draws it.
        /// </summary>
        /// <param name="index">The faction index or identity</param>
        /// <returns>The faction flag.</returns>
        public Image Get_Faction_Flag(int index)
        {
            int Row = ((int)(index / 4)) * 64;
            int Column = (index % 4) * 64;
            Point P = new Point(Column, Row);
            Size S = new Size(64, 64);
            return General_Usage_Methods.cropImage((Image)(global::LOA_SGE.Properties.Resources.flags), P, S);
        }

        /// <summary>
        /// Updates faction information.
        /// </summary>
        public void Update_Faction_Data()
        {
            Faction_Data = Initializer.ToString() + " " + money.ToString() + " " + reserve_Funds.ToString() + " " + 
                Total_Spending.ToString() + " " + building_cost.ToString() + " " + Research_spending.ToString() + " " + 
                fleet_cost.ToString() + " " + production_cost.ToString() + " " + transport_cost.ToString() + " " + 
                upkeep_cost.ToString() + " " + UEO_Taxes.ToString() + " " + Altarian_Trade_Taxes.ToString() + " " + 
                Unrest_cost.ToString() + " " + Total_Income.ToString() + " " + Colony_Tax.ToString() + " " + 
                City_Tax.ToString() + " " + Trade_Income.ToString() + " "  + Martial_Law_Funding.ToString() + " " + 
                UEO_Support_Income.ToString() + " " + Colonial_Support_Income.ToString() + " " + 
                European_Support_Income.ToString() + " " + Asian_Support_Income.ToString() + " " + 
                American_Support_Income.ToString() + " " + African_Support_Income.ToString() + " " + 
                Independant_Support_Income.ToString() + " " + Pirates_Support_Income.ToString() + " " + 
                Alterian_Support_Income.ToString() + " " + Seekers_Support_Income.ToString() + " " + 
                unknown_one.ToString() + " " + Extra_happyness.ToString() + " " + unknown_two.ToString() + " " + 
                Controled_Planets.ToString() + " " + Base_happyness.ToString() + " " + unknown_three.ToString() + " " + 
                Fuel_Used.ToString() + " " + faction_fuel.ToString() + " ";
        }

        /// <summary>
        /// Combines the factions summaries for saving.
        /// </summary>
        /// <param name="Factions">The factions to save.</param>
        /// <returns>The summaries of faction info.</returns>
        public static List<string> List_Summary(Faction[] Factions)
        {
            List<string> Fs = new List<string>();
            foreach (Faction F in Factions)
                Fs.Add(F.Faction_Data);
            return Fs;
        }

        /// <summary>
        /// Summarizes the summaries and prepares them to save.
        /// </summary>
        /// <param name="Factions">The factions which will be saved.</param>
        /// <returns>The altogether summary.</returns>
        public static string Summary(Faction[] Factions)
        {
            string summary = "";
            foreach (Faction F in Factions)
                summary = summary + F.Faction_Data;
            return summary + Enders;
        }
    }
}
