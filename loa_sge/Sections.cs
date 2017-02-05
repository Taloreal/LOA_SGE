using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LOA_SGE
{
    public class Section
    {
        public string Name = "";
        public string Data = "";
        public int Min_Variables = 0;
        public string Remembered = "";
        public int Max_Variables = 100000;
        public Section(string data, string name)
        {
            Data = data;
            Name = name;
            Remembered = data;
        }

        public Section(string data, string name, int VarCount)
        {
            Data = data;
            Name = name;
            Remembered = data;
            Max_Variables = VarCount;
            Min_Variables = VarCount;
        }

        public bool Fits(string data)
        {
            int count = General_Usage_Methods.Split(data, ' ').Count;
            return (count >= Min_Variables && count <= Max_Variables);
        }

        public bool CanSet(string data)
        {
            string temp = data.Replace("\r\n", "");
            if (!Fits(temp))
                return false;
            Remembered = data;
            Data = temp;
            return true;
        }
    }
}
