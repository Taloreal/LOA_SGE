using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LOA_SGE
{
    public class General_Usage_Methods
    {
        /// <summary>
        /// Splits a string by a seperator but every interval rather then every appearance.
        /// </summary>
        /// <param name="Text">The string to split.</param>
        /// <param name="Seperator">The char to split by.</param>
        /// <param name="Interval">The interval at which to split.</param>
        /// <returns>A split string by a char and an interval of that char.</returns>
        public static List<string> Split(string Text, char Seperator, int Interval, int Request)
        {
            List<string> Presplit = Split(Text, Seperator);
            List<string> compile = new List<string>();
            compile.Add(GetInterval(Presplit, Seperator, Interval, out Presplit) + Seperator);
            for (int i = 0; Presplit.Count != 0 && i != Request; i++)
                compile.Add(GetInterval(Presplit, Seperator, Interval, out Presplit) + Seperator);
            return compile;
        }

        public static List<string> UserSplit(string Text, char Seperator, int Interval, int Request)
        {
            List<string> Presplit = Split(Text, Seperator);
            List<string> compile = new List<string>();
            compile.Add(GetInterval(Presplit, Seperator, Interval, out Presplit) + Seperator);
            for (int i = 0; Presplit.Count != 0 && i != Request; i++)
                compile.Add(GetInterval(Presplit, Seperator, Interval, out Presplit) + Seperator);
            string remains = "";
            foreach (string s in Presplit)
                remains = remains + Seperator + s;
            if (remains.Count() != 0)
                remains = remains.Substring(1);
            compile.Add(remains);
            return compile;
        }

        /// <summary>
        /// Gets a string at a specified index.
        /// </summary>
        /// <param name="Text">The text which to split.</param>
        /// <param name="Interval">The interval at which to split the text.</param>
        /// <param name="Index">The index of the string array to get.</param>
        /// <returns>Returns a specified segment of text.</returns>
        public static string Split(string Text, int Interval, int Index)
        {
            if (Text == "" || Text == null)
                return "";
            if (Interval == 0 || Interval > Text.Length)
                Interval = Text.Length;
            int max = (int)Math.Ceiling(((Text.Length / (double)Interval)));
            if (Index >= max)
                Index = max - 1;
            return Text.Substring(Interval * Index, Interval);
        }

        /// <summary>
        /// Splits a string every interval.
        /// </summary>
        /// <param name="Text">The original string.</param>
        /// <param name="Interval">The interval at which to split.</param>
        /// <returns>A list of strings that are split every interval.</returns>
        public static List<string> Split(string Text, int Interval)
        {
            if (Interval == 0 || Interval > Text.Length)
                Interval = Text.Length;
            List<string> bits = new List<string>();
            while (Text.Length > Interval)
            {
                string temp = Text.Substring(0, Interval);
                Text = Text.Substring(Interval);
                bits.Add(temp);
            }
            bits.Add(Text);
            return bits;
        }

        /// <summary>
        /// Combines a split text every interval of Seperator.  
        /// </summary>
        /// <param name="Text">The presplit array.</param>
        /// <param name="Seperator">The seperator.</param>
        /// <param name="Interval">The interval of how many to combine.</param>
        /// <param name="OutText">The aftermath array of what ever is left oafter combining once.</param>
        /// <returns>One gruop of split texts worth to combine.</returns>
        private static string GetInterval(List<string> Text, char Seperator, int Interval, out List<string> OutText)
        {
            string compile = Text[0];
            Text.RemoveAt(0);
            for (int i = 0; i != Interval && Text.Count != 0; i++)
            {
                compile = compile + Seperator + Text[0];
                Text.RemoveAt(0);
            }
            OutText = Text;
            return compile;
        }

        /// <summary>
        /// Splits a specified string with by a specified seperator.
        /// </summary>
        /// <param name="Text">The string to split.</param>
        /// <param name="Seperator">The seperator to split by.</param>
        /// <returns>The text split into an array.</returns>
        public static List<string> Split(string Text, char Seperator)
        {
            List<string> bits = new List<string>();
            string temp = "";
            foreach (char c in Text)
            {
                if (c == Seperator)
                {
                    string place_holder = temp;
                    bits.Add(place_holder);
                    temp = "";
                }
                else
                    temp = temp + c.ToString();
            }
            if (temp != "")
                bits.Add(temp);
            return bits;
        }

        /// <summary>
        /// Splits a specified string with by a specified string seperator.
        /// </summary>
        /// <param name="Text">The string to split.</param>
        /// <param name="Seperator">The string seperator to split by.</param>
        /// <returns>The text split into an array.</returns>
        public static List<string> Split(string Text, string Seperator)
        {
            List<string> temp = new List<string>();
            string To_Add = "";
            for (int i = 0; i < Text.Count(); i++)
            {
                if (Text[i] == Seperator[0])
                {
                    string Ahead = Look_Ahead(Text, i, (i + Seperator.Count()));
                    if (Ahead == Seperator)
                    {
                        string temp2 = To_Add;
                        temp.Add(temp2);
                        To_Add = "";
                        i = i + Seperator.Count();
                    }
                    else
                        To_Add = To_Add + Text[i];
                }
                else
                    To_Add = To_Add + Text[i];
            }
            if (To_Add != "")
                temp.Add(To_Add);
            return temp;
        }

        /// <summary>
        /// Gets the following string that lies ahead of where the splitter is currently working at.
        /// </summary>
        /// <param name="Text">The text that is being split.</param>
        /// <param name="start_index">The start index to look at.</param>
        /// <param name="end_index">The laSt index at which to look at.</param>
        /// <returns>The text ahead by so many indexes.</returns>
        public static string Look_Ahead(string Text, int start_index, int end_index)
        {
            if (Text.Length < end_index + 1)
                return "";
            string variable = "";
            int temp = start_index;
            if (start_index > end_index)
            {
                temp = end_index;
                end_index = start_index;
                start_index = temp;
            }
            while (temp < end_index)
            {
                variable = variable + Text[temp];
                temp++;
            }
            return variable;
        }

        /// <summary>
        /// Crops an image.
        /// </summary>
        /// <param name="img">The original image to use.</param>
        /// <param name="Position">The position to start at to crop.</param>
        /// <param name="Size">The size of the crop.</param>
        /// <returns>The cropped image.</returns>
        public static Image cropImage(Image img, Point Position, Size Size)
        {
            Rectangle cropArea = new Rectangle(Position, Size);
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }
    }
}
