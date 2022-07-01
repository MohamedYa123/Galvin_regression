using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
namespace Advanced_regression
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        subscribtion s;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                s = Program.subs;
                Program.f.allow = false;
              //  Program.fdoc = Program.f.webBrowser1.Document.Body.InnerHtml;
                Program.f.webBrowser1.Refresh();
                Program.f.webBrowser2.Refresh();
                max2 = 0;
                timer1.Start();
                return;
                //   if (Program.fdoc!="")
                {
                    //     throw new Exception("Please provide an internet connection to assure your subscribtion !");
                }
                Program.fdoc = Program.f.webBrowser1.Document.Body.InnerHtml;
                s.serialkey = data;
                s.check();
                s.rotinecheck(ms: 0, qn2: true);

                // s.rotinecheck(1);
                s.saveme(Program.subname);
                update();
                //    Program.f.webBrowser1.Refresh();
                MessageBox.Show("Congratulation , your subscribtion has been aproved ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Program.subs.saveme(Program.subname);
                update();
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        string data;
        int max = 30;
        int max2 = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Text file|*.txt";
            of.ShowDialog();
            if (of.FileName != "")
            {
                data = File.ReadAllText(of.FileName);
                textBox1.Text = of.FileName;
                button1.Enabled = true;
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            update();
        }
        void update()
        {
            label1.Text = Program.subs.gettext();
            label3.Text = Math.Round(Convert.ToDouble(Program.subs.DAYS), 4) + " Days";
            label6.Text = Program.subs.Dateofsub.ToString(" dd/MM/yyyy");
            label4.Text = Math.Round(Convert.ToDouble(Program.subs.OFFLINE_MINUTES) / 60, 4) + " hr ";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button1.Enabled = false;
            max2++;
            if (!Program.f.allow && max2 < max)
            {
              //  max2++;
            }
            else if (max2 < max-2)
            {
                max2 = max - 2;
            }
            else if (max2 > max)
            {
                button1.Enabled = true;
                try
                {
                    Program.fdoc = Program.f.webBrowser1.Document.Body.InnerHtml;
                    s.serialkey = data;
                    s.check();
                    s.rotinecheck(ms: 0, qn2: true);

                    // s.rotinecheck(1);
                    s.saveme(Program.subname);
                    update();
                    timer1.Stop();
                    //    Program.f.webBrowser1.Refresh();
                    MessageBox.Show("Congratulation , your subscribtion has been aproved ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Program.subs.saveme(Program.subname);
                    update();
                    timer1.Stop();
                    MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }
    }
}
