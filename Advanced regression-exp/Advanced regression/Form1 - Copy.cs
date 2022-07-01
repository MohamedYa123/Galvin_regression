using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using smartmodify;
//تم عمل المشروع بواسطة محمد ياسر
//رابط السلسلة أشرح فيها بالكامل كيف عملت المشروع
//https://www.youtube.com/watch?v=tNf97hymK_w&list=PLsXQEsz9IQ-q-l05rrZwjRnX3SCzGKDJZ
//رابط القناة
//https://www.youtube.com/channel/UCyK_zVDWCutbm8Ji_m3QDWQ
namespace Calculator
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            sm = new smartmodifier(this);
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == o4.GetType() && !(c.Text=="c" || c.Text=="del" || c.Text=="=") )
                {
                    c.Click += delegate
                    {
                        int y = area.SelectionStart;
                        area.Text =sm.insert(area.SelectionStart, area.SelectionLength, c.Text, area.Text);
                        area.SelectionStart = sm.selection;
                        area.Focus();
                    };
                }
            }
            variablvalues = new Dictionary<string, decimal>();
            variablvalues.Add("π", Convert.ToDecimal(3.14159265358979323846));
            variablvalues.Add("e", Convert.ToDecimal(2.71828182846));
            variablvalues.Add("ans", Convert.ToDecimal(0));
            variablvalues.Add("X", Convert.ToDecimal(0));
            variablvalues.Add("Y", Convert.ToDecimal(0));
            variablvalues.Add("Z", Convert.ToDecimal(0));
            first = false;
        }
        Dictionary<string, decimal> variablvalues;
        smartmodifier sm;
        double[,] rates1;
        double[,] rates2;
        bool first;
        private void Form1_Load(object sender, EventArgs e)
        {
            rates1 = new double[Controls.Count, 2];
            rates2 = new double[Controls.Count, 2];
            try{
                int i = 0;
                foreach(Control c in Controls)
                {
                    rates1[i, 0] = Convert.ToDouble(c.Width) / Convert.ToDouble(Width);
                    rates1[i, 1] = Convert.ToDouble(c.Height) / Convert.ToDouble(Height);
                    rates2[i, 0] = Convert.ToDouble(c.Top) / Convert.ToDouble(Height);
                    rates2[i, 1] = Convert.ToDouble(c.Left) / Convert.ToDouble(Width);
                    i += 1;
                }
                first = true;
            }
            catch
            {

            }

        }

        private void guna2Button25_Click(object sender, EventArgs e)
        {
            area.Text = "";
            area.Focus();
        }

        private void guna2Button40_Click(object sender, EventArgs e)
        {
            area.Text = sm.remove(area.SelectionStart, area.SelectionLength, area.Text);
            area.SelectionStart = sm.selection;
            area.Focus();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            try
            {
                if (first)
                {
                    int i = 0;
                    foreach (Control c in Controls)
                    {
                        c.Width = Convert.ToInt32(rates1[i, 0] * Convert.ToDouble(Width));
                        c.Height = Convert.ToInt32(rates1[i, 1] * Convert.ToDouble(Height));
                        c.Top = Convert.ToInt32(rates2[i, 0] * Convert.ToDouble(Height));
                        c.Left = Convert.ToInt32(rates2[i, 1] * Convert.ToDouble(Width));
                        i += 1;
                    }
                }
            }
            catch
            {

            }
        }


        private void area_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            foreach(Control c in Controls)
            {
                if (c.GetType() == o4.GetType() && !(c.Text == "c" || c.Text == "del" || c.Text == "="))
                {
                    if (e.KeyChar.ToString().ToLower() == c.Text || e.KeyChar.ToString().ToUpper() == c.Text)
                    {
                        Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)c;
                        b.PerformClick();
                    }
                }
            }
            e.Handled = true;
        }

        private void area_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData.ToString()=="Back" || e.KeyData.ToString() == "Delete")
            {
                gunadel.PerformClick();
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            operation op = new operation(area.Text.Replace("%", "×0.01"), sm, variablvalues);
            try
            {
                var ans = op.calc();
                variablvalues["ans"] = ans;
                result.Text = ans + "";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            variablvalues["X"]=Convert.ToDecimal(result.Text);
        }

        private void yToolStripMenuItem_Click(object sender, EventArgs e)
        {
            variablvalues["Y"] = Convert.ToDecimal(result.Text);
        }

        private void zToolStripMenuItem_Click(object sender, EventArgs e)
        {
            variablvalues["Z"] = Convert.ToDecimal(result.Text);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form2 f = new Form2();
            //f.ShowDialog();
        }
    }
}
