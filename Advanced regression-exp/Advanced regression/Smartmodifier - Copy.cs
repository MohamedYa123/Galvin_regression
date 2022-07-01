using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Guna.UI2.WinForms;
//تم عمل المشروع بواسطة محمد ياسر
//رابط السلسلة أشرح فيها بالكامل كيف عملت المشروع
//https://www.youtube.com/watch?v=tNf97hymK_w&list=PLsXQEsz9IQ-q-l05rrZwjRnX3SCzGKDJZ
//رابط القناة
//https://www.youtube.com/channel/UCyK_zVDWCutbm8Ji_m3QDWQ
namespace smartmodify
{

    class smartmodifier
    {
        public List<string> parts;
        public List<int[]> parts2;
        public int selection;
        public Form f2;
        public Dictionary<string, int> ids;
        public smartmodifier(Form f)
        {
            parts = new List<string>();
            parts2 = new List<int[]>();
            selection = 0;
            ids = new Dictionary<string, int>();
            f2 = f;
            //Guna2Button g = new Guna2Button();
            foreach (Control c in f2.Controls)
            {
                if (c.GetType() == (new Guna2Button()).GetType() && !(c.Text == "c" || c.Text == "del" || c.Text == "="))
                {
                    parts.Add(c.Text);
                    ids.Add(c.Text, Convert.ToInt32( c.Name.Substring(1)));
                }
            }
            prepare();
        }
        private void prepare()
        {
            int i = 0;
            foreach (string s in SortByLength(parts))
            {
                parts[i] = s;
                i++;
            }
        }
        public string insert(int y, int y2, string ins, string area)
        {
            //string area = f2.area.Text;
            selection = y;
            area = remove(y, y2, area, with: 0);

            string p = "";
            bool g = true;
            for (int rj = 0; rj < parts.Count; rj++)
            {
                p = parts[rj];
                if (!g)
                {
                    break;
                }
                for (int i = 0; i < area.Length - p.Length + 1; i++)
                {
                    if (area.Substring(i, p.Length) == p)
                    {
                        //int[,] minipart = new int[1,2];
                        //minipart[0, 0] = i;
                        //minipart[0, 1] = i+p.Length;
                        if (y > i && y < i + p.Length)
                        {
                            g = false;
                            break;
                        }
                    }
                }
            }
            if (g)
            {
                area = area.Substring(0, selection) + ins + area.Substring(selection);
                selection += ins.Length;
            }
            return area;
        }
        public string remove(int y, int y2, string area, int with = 1)
        {
            // string area = f2.area.Text;

            bool g = true;
            string p = "";
            int ro = 1;
            if (y2 > 0)
            {
                ro = 0;
            }
            for (int rj = 0; rj < parts.Count; rj++)
            {
                p = parts[rj];
                if (!g)
                {
                    break;
                }
                for (int i = 0; i < area.Length - p.Length + 1; i++)
                {
                    if (area.Substring(i, p.Length) == p)
                    {
                        //int[,] minipart = new int[1,2];
                        //minipart[0, 0] = i;
                        //minipart[0, 1] = i+p.Length;

                        if (y > i - 1 + ro && y < i + p.Length + 1 * ro && !(y2 == 0 && with == 0))
                        {
                            area = area.Substring(0, i) + area.Substring(i + p.Length);
                            y2 = y2 - p.Length + y - i;
                            y -= y - i;
                            //i -= p.Length;
                            rj = -1;
                            selection = i;
                            i -= 1;
                            if (y2 <= 0)
                            {

                                g = false;
                            }
                            break;
                        }
                    }
                }
            }


            return area;
        }
        public string fillparts2( string area)
        {
            // string area = f2.area.Text;
            parts2 = new List<int[]>();
            string p = "";
            for (int rj = 0; rj < parts.Count; rj++)
            {
                p = parts[rj];

                for (int i = 0; i < area.Length - p.Length + 1; i++)
                {
                    var g = true;
                    // 4tan(
                    foreach(var p2 in parts2)
                    {
                        if(i>=p2[0] && i + p.Length <= p2[0] + p2[1])
                        {
                            g = false;
                            break;
                        }
                    }
                    if (!g)
                    {
                        continue;
                    }
                    if (area.Substring(i, p.Length) == p)
                    {
                        int[] m = { i, p.Length };
                        parts2.Add(m);

                    }
                }
            }
            var i2 = 0;
            foreach(var c in SortbyIndex(parts2))
            {
                parts2[i2] = c;
                i2 += 1;
            }

            return area;
        }
        static IEnumerable<int[]> SortbyIndex(IEnumerable<int[]> e)
        {
            // Use LINQ to sort the array received and return a copy.
            var sorted = from s in e
                         orderby s[0] ascending
                         select s;
            return sorted;
        }

        static IEnumerable<string> SortByLength(IEnumerable<string> e)
        {
            // Use LINQ to sort the array received and return a copy.
            var sorted = from s in e
                         orderby s.Length descending
                         select s;
            return sorted;
        }
    }
}
