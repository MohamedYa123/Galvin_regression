using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using regressor2;
namespace Advanced_regression
{
    public partial class Form3 : Form
    {
        public Form3(regressor g)
        {
            InitializeComponent();
            myreg = g;
        }
        public regressor myreg;
        public List<double[]> inputs;
        public List<double> outputs;
        private void Form3_Load(object sender, EventArgs e)
        {

            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            var ch2 = new ColumnHeader();
            ch2.Text = "index";
            listView1.Columns.Add(ch2);
            for (int i = 0; i < inputs[0].Length; i++)
            {
                var ch = new ColumnHeader();
                ch.Text = "                  X" + (i+1)+ "                  ";
                listView1.Columns.Add(ch);
            }
             ch2 = new ColumnHeader();
            ch2.Text = "Real output";
            listView1.Columns.Add(ch2);
            ch2 = new ColumnHeader();
            ch2.Text = "Equation predicts";
            listView1.Columns.Add(ch2);
            ch2 = new ColumnHeader();
            ch2.Text = "Accuracy (%)";
            listView1.Columns.Add(ch2);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            for (int i = 0; i < inputs.Count; i++)
            {
                ListViewItem li = new ListViewItem((i+1) + "");
                for (int i2 = 0; i2 < inputs[0].Length; i2++)
                {
                    li.SubItems.Add(inputs[i][i2] + "");
                }
                var y = outputs[i];
                li.SubItems.Add(y + "");
                var y2 = myreg.calc(myreg.equation, inputs[i]);
                li.SubItems.Add(y2 + "");
                var er = 1 - Math.Abs((y - y2) / y);
                li.SubItems.Add(er*100 + "");
                listView1.Items.Add(li);
            }
            timer1.Start();
            WindowState = FormWindowState.Maximized;
            int top1 = 20;
            int left1 = 80;
            int add = 2;
            int height = 20;
 
            for (int i = 0; i < myreg.equation.Count; i++)
            {
                left1 = 20;
                for (int i2 = 0; i2 < myreg.equation[0].Length; i2++)
                {
                    Guna.UI2.WinForms.Guna2NumericUpDown n = new Guna.UI2.WinForms.Guna2NumericUpDown();
                    n.Maximum = 1000000;
                    n.Width = 100;
                    n.Height = 30;
                    n.Minimum = -1000000;
                    n.DecimalPlaces = 4;

                    height = n.Height;
                    Label l = new Label();
                    n.Top = top1;
                    n.Left = left1;
                    l.Font = new Font("tahoma", 8, FontStyle.Bold);
                    n.Name = i + " " + i2;
                    if (i2 != 0 && !myreg.expon )
                    {
                        l.Text = "     ×         X" + i2 + "^";
                        if (i2 != myreg.equation[0].Length - 1)
                        {
                            l.Text += "        ";
                        }
                        n.Value = Convert.ToDecimal(myreg.equation[i][i2]);
                        l.Left = left1 - l.Width - add;
                    }
                    else if (i2 == 0)
                    {
                        l.Text = "               +";
                        n.ForeColor = Color.Blue;
                        n.Value = Convert.ToDecimal(myreg.equation[i][i2]);
                        l.Left = left1 - l.Width - add;
                    }
                    else if (myreg.expon&&i2%2==0)
                    {
                        l.Text = "          ×         ";
                        if (i2 != myreg.equation[0].Length - 1)
                        {
                            l.Text += "";
                        }
                        Label l2 = new Label();
                        n.Value = Convert.ToDecimal(Math.Pow(myreg.powf,myreg.equation[i][i2]));
                        l2.Text= "^X" + ((i2)/2);
                        l2.Top = top1 + 5;
                        l2.Left =left1+ n.Width + add;
                        l.Left = left1 - l.Width - add;
                        left1 += l2.Width + add;
                        home.Controls.Add(l2);
                    }
                    else if (myreg.expon)
                    {
                        l.Text = "     ×         X" + ((i2-1)/2+1) + "^";
                        if (i2 != myreg.equation[0].Length - 1)
                        {
                            l.Text += "        ";
                        }
                        n.Value = Convert.ToDecimal(myreg.equation[i][i2]);
                        l.Left = left1 - l.Width - add;
                    }

                    l.Top = top1 + 5;

                    left1 += l.Width + n.Width + add;
                    if (left1 >= home.Width-n.Width-l.Width-vScrollBar1.Width)
                    {
                        left1 = 90;
                        top1 += height + 10;
                    }
                    //n.ValueChanged += delegate(object sender,EventArgs e)
                    //{
                    //    Guna.UI2.WinForms.Guna2NumericUpDown gg=new Guna.UI2.WinForms.Guna2NumericUpDown();
                    //    foreach (var c in home.Controls)
                    //    {
                    //        if (c.GetType() == n.GetType())
                    //        {
                    //            gg = (Guna.UI2.WinForms.Guna2NumericUpDown)c;
                    //        }
                    //    }
                    //    myreg.equation[i][i2] = Convert.ToDouble(gg.Value);
                    //};
                    n.ValueChanged += delegate
                    {
                        timer1.Start();
                    };
                    home.Controls.Add(n);
                    home.Controls.Add(l);
                }
                top1 += height + 30;
            }
            val = (home.Height - listView1.Height - guna2Panel1.Height);
            vScrollBar1.Maximum = Convert.ToInt32( 2000/ Convert.ToDouble(top1-val))+5;
            double inc = 1;
            if (myreg.expon)
            {
                inc = 3;
            }
            vScrollBar1.Maximum =Convert.ToInt32(Convert.ToDouble( vScrollBar1.Maximum*inc)/ 0.7);

        }
        int val;
        private void timer1_Tick(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2NumericUpDown p = new Guna.UI2.WinForms.Guna2NumericUpDown();
            foreach (var c in home.Controls)
            {
              if (c.GetType() == p.GetType())
                {
                    var k = (Guna.UI2.WinForms.Guna2NumericUpDown)c;
                    var indics = k.Name.Split();
                    var i1 = Convert.ToInt32(indics[0]);
                    var i2 = Convert.ToInt32(indics[1]);
                    if (i2!=0 && i2 % 2 == 0 && myreg.expon)
                    {
                        myreg.equation[i1][i2] = Convert.ToDouble(Math.Log(Convert.ToDouble(k.Value),myreg.powf));
                    }
                    else
                    {
                        myreg.equation[i1][i2] = Convert.ToDouble(k.Value);
                    }
                }

            }
            var l = inputs[0].Length+1;
            for (int i = 0; i < inputs.Count; i++)
            {
                var li = listView1.Items[i];
                var y = outputs[i];
                li.SubItems[l].Text = y+"";
                var y2 = myreg.calc(myreg.equation, inputs[i]);
                li.SubItems[l+1].Text = y2+"";
                var er = 1 - Math.Abs((y - y2) / y);
                li.SubItems[l+2].Text = er*100+"";
            }
            label2.Text= (myreg.accCalc(myreg.equation, inputs, outputs, par: 1)*100)+"%";
            timer1.Stop();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.data = myreg.write(8) + "\r\n\r\nAccuracy : " + label2.Text;
            f.Show();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            
            Form2 f = new Form2();
            f.data = myreg.excell(8);
            f.Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string f = "";
            int i2 = 0;
            foreach(ColumnHeader c in listView1.Columns)
            {
                if (i2 != 0)
                {
                    f += c.Text + ",";
                }
                i2++;
            }
            f += "\r\n";
            foreach(ListViewItem u in listView1.Items)
            {
                int i = 0;
                foreach(ListViewItem.ListViewSubItem g in u.SubItems)
                {
                    if (i != 0)
                    {
                        f += g.Text + ",";
                    }
                    i++;
                }
                f += "\r\n";
            }
            SaveFileDialog of = new SaveFileDialog();
            of.Filter = "Comma delicated Files|*.csv";
            of.ShowDialog();
            if (of.FileName != "")
            {
                 File.WriteAllText(of.FileName, f);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.Filter = "equation file Files|*.eqad";
            sv.ShowDialog();
            if (sv.FileName != "")
            {
                myreg.saveme(sv.FileName);
            }
        }
        int lastv;
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            int mainv = -(vScrollBar1.Value - lastv) * 10;
            if (vScrollBar1.Value > lastv)
            {
                mainv =-( vScrollBar1.Value - lastv)*10;
            }
            if (vScrollBar1.Value == lastv)
            {
                mainv = 0;
            }
            lastv = vScrollBar1.Value;
           foreach (Control f in home.Controls)
            {
                if (f.GetType() != vScrollBar1.GetType())
                {
                    f.Top += mainv; 
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }
    }
}
