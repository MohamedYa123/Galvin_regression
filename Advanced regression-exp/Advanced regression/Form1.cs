using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using regressor2;
using Microsoft.VisualBasic;
//using WindowsFormsApp4;
namespace Advanced_regression
{
    public partial class Form1 : Form
    {
        public double stop;
        public int numofthreads;
        public int maxsteps;
        public List<regressor> regressors;
        public List<regressor> liveregs;
        public List<regressor> bests;
        public List<Thread> allthreads;
        public List<double[]> threadsdata;
        public List<double> sentdata;
        project mproject;
        subscribtion mysubscribtion;
        [Obsolete]
        public Form1()
        {
            InitializeComponent();
            try
            {
                mproject2 = project.load("temp.proj");
            }
            catch
            {

            }
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser2.ScriptErrorsSuppressed = true;
            allthreads = new List<Thread>();
            
            stop =0.98;
            stopupdown.Value = Convert.ToDecimal( stop * 100);
            numofthreads = 10;
            maxsteps = 100000;
            numsteps.Value = Convert.ToDecimal(maxsteps);
            regressors = new List<regressor>();
            liveregs = new List<regressor>();
            bests = new List<regressor>();
            allthreads = new List<Thread>();
            threadsdata = new List<double[]>();
            sentdata = new List<double>();
            sentdata.Add(6);//number of terms
            sentdata.Add(-4);// minpow
            sentdata.Add(4);//maxpow
            sentdata.Add(0.001);//neglection limit
            sentdata.Add(60);//shufflelen
            sentdata.Add(0.01);//initial expandlimit
            sentdata.Add(0.05);//bracke limit
            sentdata.Add(0.7);//arrange acc
            sentdata.Add(0.75);//neglect acc
            sentdata.Add(0.75);//bracke off acc
            sentdata.Add(-1);//reset bracke and neglection
            sentdata.Add(100);//term limit
            sentdata.Add(0.05);//general growth
            sentdata.Add(0.0001);//>0.99
            sentdata.Add(1);//>errpow
            sentdata.Add(0.001);//>0.25
            sentdata.Add(1.01);//powf
            sentdata.Add(0.01);//powerneglect
         //   sentdata.Add(0);//>exponential
            ColumnHeader ch = new ColumnHeader();
            numdg2 = 6;
            ch.Text = "Thread id";
            listView1.Columns.Add(ch);
            ch = new ColumnHeader();
            ch.Text = "Accuracy (%)";
            listView1.Columns.Add(ch);
            ch = new ColumnHeader();
            ch.Text = "Steps";
            listView1.Columns.Add(ch);
            ch = new ColumnHeader();
            ch.Text = "Average speed (steps/sec)";
            listView1.Columns.Add(ch);
            ch = new ColumnHeader();
            ch.Text = "Live Accuracy (%)";
            listView1.Columns.Add(ch);
            ch = new ColumnHeader();
            ch.Text = "Expand limit";
            listView1.Columns.Add(ch);
            ch = new ColumnHeader();
            ch.Text = "Thread Case";
            listView1.Columns.Add(ch);
            mainanme = "Galvin-regression";
            pname = "Untitled project";
            //if (Program.subs.MODE == Program.modetype.trial)
            //{
               // mainanme += " <pending internet connection> ";
                //pname = pname;
            //}
        }
        string mainanme;
        double[,] rates;
        double[,] rates2;
        bool first2;
        string pname2;
        public string pname { get {
                return pname2;
            } set {

                pname2 = value;
                Text = mainanme + " - " + pname2 + " - ";
                if (projname.Text != pname2)
                {
                    projname.Text = pname2;
                }
                if (value == "")
                {
                    Text = mainanme;
                }

            } }
        bool dochange;
        void checktrialsubscribtion()
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
           
            dochange = true;
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //this.ContextMenuStrip = contextMenuStrip1;
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            watch = new System.Diagnostics.Stopwatch();
            listView1.FullRowSelect = true;
            //1942, 974
            Height = 800;
            Width = 1250;
            listView1.ContextMenuStrip = contextMenuStrip1;
          //  listView1.ContextMenu =(ContextMenu)contextMenuStrip1;
            foreach (Control c in setpanel.Controls)
            {
                if (c.GetType() == n0.GetType() && c.Name!= "digitsview")
                {
                    var q = (Guna.UI2.WinForms.Guna2NumericUpDown)c;
                    q.Value=Convert.ToDecimal( sentdata[Convert.ToInt32(q.Name.Substring(1))]) ;
                    q.ValueChanged += delegate
                    {
                        if (dochange)
                        {
                            guna2Button7_Click(sender, e);
                        }
                    };
                }
            }
            rates = new double[Controls.Count, 2];
            rates2 = new double[Controls.Count, 2];
            int i = 0;
            foreach (Control c in Controls)

            {

                rates[i, 0] = Convert.ToDouble(c.Top) / Convert.ToDouble(Height);
                rates[i, 1] = Convert.ToDouble(c.Left) / Convert.ToDouble(Width);
                rates2[i, 0] = Convert.ToDouble(c.Height) / Convert.ToDouble(Height);
                rates2[i, 1] = Convert.ToDouble(c.Width) / Convert.ToDouble(Width);
                i++;
            }
            first2 = true;
            WindowState = FormWindowState.Maximized;
        }
        void licenseachieve(int id)
        {
            if (Program.subs.MODE == Program.modetype.trial)
            {
                switch (id)
                {
                    case 0://Run
                        if (inputs[0].Length > 3   || inputs.Count>400)
                        {
                            throw new Exception("You are in free trial you can't work on more than 3 variables and 400 rows");
                        }
                        break;
                    case 1  ://analysis
                        throw new Exception("This feature is not available in free trial");
                    case 2: //save , import ,restore
                        throw new Exception("This feature is not available in free trial");
                }
            }
            else if(Program.subs.MODE == Program.modetype.subscribtion)
            {

            }
            else if (Program.subs.MODE == Program.modetype.suspendedfull || Program.subs.MODE == Program.modetype.suspendedtrial)
            {
                throw new Exception("Sorry, your subscribtion is suspended you can't use this feature");
            }
            else if (Program.subs.MODE == Program.modetype.ended)
            {
                throw new Exception("Sorry, your subscribtion has ended you can't use this feature");
            }
        }
        void restorevalues()
        {
            stopupdown.Value = Convert.ToDecimal(stop*100);
            threadsnum.Value = Convert.ToDecimal(numofthreads);
            numsteps.Value = Convert.ToDecimal(maxsteps);
            projname.Text = pname;
            foreach (Control c in setpanel.Controls)
            {
                if (c.GetType() == n0.GetType() && c.Name!="digitsview")
                {
                    var q = (Guna.UI2.WinForms.Guna2NumericUpDown)c;
                    q.Value =Convert.ToDecimal( sentdata[Convert.ToInt32(q.Name.Substring(1))]);
                }
            }
            change = true;
        }

        [Obsolete]
        void loadproject(project proj)
        {
            dochange = false;
            pname = proj.name;
            stop = proj.stop;
            numofthreads = proj.numofthreads;
            maxsteps = proj.maxsteps;
            regressors = proj.regressors;
            liveregs = proj.liveregs;
            bests = proj.bests;
           // allthreads = proj.allthreads;
            threadsdata = proj.threadsdata;
            sentdata = proj.sentdata;
            inputs = proj.inputs;
            outputs = proj.outputs;
            columns = proj.columns;
            guna2Button10.PerformClick();
            try
            {
                for (int i = 0; i < allthreads.Count; i++)
                {
                    try { allthreads[i].Suspend(); }
                    catch { }
                    try { allthreads[i].Abort(); }
                    catch { }
                }
            }
            catch { }
            allthreads.Clear();
            fill();
            restorevalues();
            for (int i = 0; i < numofthreads; i++)
            {

                // start(i);
                //start();
                threaddi = i;
                //start();

                Thread.Sleep(10);
                ThreadStart thr = new ThreadStart(start);
                Thread.Sleep(10);
                Thread th = new Thread(thr);
                Thread.Sleep(10);
                th.Start();
                Thread.Sleep(10);
                allthreads.Add(th);
                Thread.Sleep(10);
                // if (i == numofthreads - 1)
                //{
                //  break;
                // }
            }
            Thread.Sleep(500);
            foreach (var th in allthreads)
            {

            }

            watch.Start();
            first = true;
            dochange = true;
           // saveproject();
        }
        void fill()
        {
            
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            for (int i = 0; i < columns.Length; i++)
            {
                DataGridViewColumn dg = new DataGridViewColumn();
                dg.HeaderText = columns[i];
                dataGridView1.Columns.Add(dg);
            }
            for (int i = 0; i < inputs.Count; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                var cells = inputs[i];
                //   if (true)
                DataGridViewCell dc = new DataGridViewTextBoxCell();
                for (int i2 = 0; i2 < cells.Length; i2++)
                    {
                         dc = new DataGridViewTextBoxCell();
                        dc.Value = cells[i2];
                        dr.Cells.Add(dc);
                        
                    }
                dc = new DataGridViewTextBoxCell();
                dc.Value = outputs[i]+"";
                dr.Cells.Add(dc);
                dataGridView1.Rows.Add(dr);

            }
        }
        List<double[]> readcsv(string read)
        {
            List<double[]> listA = new List<double[]>();

            using (var reader = new StreamReader(read))
            {
                int ii = 0;
                while (!reader.EndOfStream)
                {

                    var line = reader.ReadLine();
                    line = line.Replace(",", ";");
                    var values = line.Split(';');
                    if (ii == 0)
                    {
                        columns = values;
                    }
                    if (ii != 0)
                    {
                        double[] rg = new double[values.Length];
                        for (int i = 0; i < values.Length; i++)
                        {
                            var s= Convert.ToDouble(values[i]);
                            if (s < 0)
                            {
                                //MessageBox.Show("Negative values in input are forbiden ! ");
                                throw new Exception("Negative values in input are forbiden ! ");
                            }
                            else if (s == 0)
                            {
                                s = 0.00001;
                            }
                            rg[i] = s;
                        }
                        listA.Add(rg);
                    }
                    ii++;
                }
            }
            return listA;
        }
        string[] columns;
        List<double> readocsv(string read)
        {
            List<double> listA = new List<double>();
            int ii = 0;
            using (var reader = new StreamReader(read))
            {

                while (!reader.EndOfStream)
                {

                    var line = reader.ReadLine();
                    line=line.Replace(",", ";");
                    var values = line.Split(';');
                    if (ii == 0)
                    {
                        var ncolumns = new string[columns.Length + 1];
                        for (int ih = 0; ih < columns.Length; ih++)
                        {
                            ncolumns[ih] = columns[ih];
                        }
                        ncolumns[columns.Length] = values[0];
                        columns = ncolumns;
                    }
                    if (ii != 0)
                    {
                        double[] rg = new double[values.Length];
                        for (int i = 0; i < values.Length; i++)
                        {
                            rg[i] = Convert.ToDouble(values[i]);
                        }
                        var s = rg[0];
                        if (s == 0)
                        {
                            s = 0.00001;
                        }
                        listA.Add(s);
                    }
                    ii++;
                }
            }
            return listA;
        }
        List<double[]> inputs;
        List<double> outputs;
        private void guna2Button1_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            OpenFileDialog of = new OpenFileDialog();
            of.Filter= "Comma delicated Files|*.csv;*.txt";
            of.ShowDialog();
            if (of.FileName != "")
            {
                guna2TextBox1.Text = of.FileName;
                try
                {
                    inputs = readcsv(of.FileName);
                }
                catch(Exception er)
                {
                    MessageBox.Show(er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                tr.Text = "Try " + (inputs[0].Length - 2);
                refreshrecommended();
                if (outputs!=null && inputs.Count == outputs.Count)
                {
                    fill();
                    
                }
            }
        }
        double calcrec(double rj)
        {
            if (rj < 0)
            {
                rj = 1;
            }
            return rj * inputs[0].Length;
        }
        void refreshrecommended()
        {
            if (inputs !=null && inputs.Count != 0)
            {
                double rj = Convert.ToDouble(n0.Value);
                rfrsh.Text="Recommended : "+ Convert.ToInt32(calcrec(rj-0.5))+"~"+Convert.ToInt32(calcrec(rj + 2.5));
            }
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Cross validation Files|*.csv;*.txt";
            of.ShowDialog();
            if (of.FileName != "")
            {
                guna2TextBox2.Text = of.FileName;
                outputs = readocsv(of.FileName);
                if (outputs != null && inputs.Count == outputs.Count)
                {
                    fill();
                    Refresh();
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < allthreads.Count; i++)
            //{
            //    allthreads[i].Abort();
            //}
            //threaddi += 1;
            int id = 0;
            if (inputs==null || outputs==null || outputs.Count == 0 || inputs.Count == 0 || allthreads.Count!=0)
            {
                return;
            }
            try
            {
                licenseachieve(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            numofthreads = Convert.ToInt32(threadsnum.Value);
            int terms = Convert.ToInt32(sentdata[0]);
            var minpow = sentdata[1];
            var maxpow = sentdata[2];
            var neglection = sentdata[3];
            var shufflelen = Convert.ToInt32(sentdata[4]);
            var expandlimit = sentdata[5];
            var brackelimit = sentdata[6];
            var termlimit = sentdata[11];
            msg = new List<string>();
            for (int i = 0; i < numofthreads; i++)
            {
                var regg = new regressor(inputs, outputs, terms, checkBox1.Checked, point:(minpow+maxpow)/2+0.01);
                regg.copy();
                regg.minpow = minpow;
                regg.maxpow = maxpow;
                regg.neglectionlimit = neglection;
                regg.shufflelen = shufflelen;
                regg.expandlimit = expandlimit;
                regg.brackelimit = brackelimit;
                regg.termlimit = termlimit;
            //    if (checkBox1.Checked)
                {
                    regg.expon = checkBox1.Checked;
                }
                regressors.Add(regg);
                double[] dat = { 0, 0, 0, 0,0};
                msg.Add("");
                threadsdata.Add(dat);
                bests.Add(regg);
                liveregs.Add(regg);
            }
            //start();
            //start();

            for (int i = 0; i < numofthreads; i++)
            {

                // start(i);
                //start();
                threaddi = i;
                 
                
                Thread.Sleep(10);
                ThreadStart thr = new ThreadStart(start);
                Thread.Sleep(10);
                Thread th = new Thread(thr);
                Thread.Sleep(10);
                th.Start();
                Thread.Sleep(10);
                allthreads.Add(th);
                Thread.Sleep(10);
                // if (i == numofthreads - 1)
                //{
                //  break;
                // }
            }
            Thread.Sleep(500);
            foreach (var th in allthreads)
            {
                
            }

            watch.Start();
            first = true;
            saveproject();
         //   guna2Button8.PerformClick();
        }
        void saveproject()
        {
            mproject = new project(pname, stop, numofthreads, maxsteps, regressors, liveregs, bests, threadsdata, sentdata, inputs, outputs, columns);
        }
        List<string> msg;
        bool change;
        void start2(int i)
        {
            try
            {
                //start(i);
            }
            catch(Exception e)
            {
              //  msg[i] = e.Message;
                start2(i);
            }
        }
        public int threaddi;

        [Obsolete]
        void start()
        {
            var threadid = threaddi + 0;
            double er2 = 0;
            var reg = liveregs[threadid];// regressors[threadid];
            var rt = reg.copier();
            var sd = reg.sidecopy();
            int u2 = maxsteps;
            // evolutionpool ev = new evolutionpool(reg, reg.binputs, reg.boutputs);
            // ev.runsteps(100);
            var er = -100000.0;
            if (threadsdata[threadid][0] != 0)
            {
                er = threadsdata[threadid][0];
            }
            bool lm = true;
            int cb = 2;
            for (int u = 0; u < maxsteps*100; u++)
            {
                if (er2 >= stop)
                {
                   // u2 = u;
                  //  break;
                }
                if (u >= maxsteps)
                {
                  //  Thread.CurrentThread.Suspend();
                }
                reg.Train();
                //    reg.shufflelen = Convert.ToInt32(sentdata[4]);
                //new
                if (change)
                {
                    reg.minpow = sentdata[1];
                    reg.maxpow = sentdata[2];
                    reg.neglectionlimit = sentdata[3];
                    reg.shufflelen = Convert.ToInt32(sentdata[4]);
                    //reg.expandlimit = sentdata[5];
                    reg.brackelimit = sentdata[6];
                    reg.termlimit = sentdata[11];
                    reg.errpow = sentdata[14];
                    //change = false;
                }
                //end new
                var oldr = er2;
                er2 = reg.accCalc(reg.equation, reg.binputs, reg.boutputs, par: 1);
                threadsdata[threadid][1] = u;
                threadsdata[threadid][2] = er2;
                //  reg.mainacc = er2;
                liveregs[threadid] = reg;
                reg.sadd = 0;
                reg.bracks = true;
                reg.powf = sentdata[16];
                cb -= 1;
                if ((er2 - oldr) > sentdata[12] * Math.Abs(oldr) )
                {
                    reg.sadd = 1;
                    lm = true;
                    cb = 2;
                }
                else if (er2 > 0.25)
                {
                    if ((er2 - oldr) > sentdata[15] * oldr)
                    { reg.sadd = 1; lm = true; cb = 2; }
                }
                else if (er2 > 0.95)
                {
                    if ((er2 - oldr) > sentdata[13] * oldr)
                    { reg.sadd = 1;lm = true;cb = 2;  }

                }
                else {
                    //if (cb < 1 && er > 0)
                    //{
                    //    lm = false;
                    //    cb = 2;
                    //    reg.equation = best1.copier();
                    //    reg.sides = best1.sidecopy();
                    //}
                    //else if (er < 0) { cb = 2; }
                    //if (er < bacc && er < 0.9&&bacc>0.9)
                    //{
                    //    reg.equation = best1.copier();
                    //    reg.sides = best1.sidecopy();
                    //    er2 = bacc;
                    //    er2 = reg.accCalc(reg.equation, reg.binputs, reg.boutputs, par: 1);
                    //}
                }
                if (er2 > sentdata[8])
                {
                   // reg.bracks = false;
                    reg.neglect = true;
                }
                if (er2 > sentdata[9])
                {
                    reg.bracks = false;
                   // reg.neglect = true;
                }
                if (er2 < sentdata[10])
                {
                  //  reg.bracks = true;
                    reg.neglect = false;
                }
                threadsdata[threadid][4] = reg.expandlimit;

                if (er2 > er || er == 0)
                {
                    regressor reg2 = reg.copy();
                  //  reg2.mainacc = er2;
                    bests[threadid] = reg2;
                    rt = reg.copier();
                    sd = reg.sidecopy();
                    threadsdata[threadid][0] = er2;
                    reg.sadd = 1;

                    er = er2;
                    if (er > sentdata[7])
                    {
                        //reg.sadd = 0;
                      //  reg.sadd = 0;
                        reg.arnge = 1;
                    }

                }
                else
                {

                }
                if (er2 > 0)
                {
                    reg.expandlimit = (1 - er) * sentdata[5];
                    if (reg.expandlimit < 0)
                    {
                        reg.expandlimit = 0.000001;
                    }
                }
                // reg.diff = ( er2 - oldr)/Math.Abs(er2);
            }
           // Thread.CurrentThread.Abort();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        System.Diagnostics.Stopwatch watch;
        regressor best1;
        bool first;
        double bacc;
        int bestid;
        double bestshuffle;
        [Obsolete]
        private void timer1_Tick(object sender, EventArgs e)
        
        {
            //if (!first)
            //{
            //    Random rd = new Random();
            //    var xx = Convert.ToDouble(rd.Next(80, 120)) / 100;
            //    sentdata[4] = Convert.ToInt32(bestshuffle * xx);
            //    Text = sentdata[4] + "";
            //}
           
            double bestacc = -10000000;
            if (first)
            {
                //bestshuffle = sentdata[4];
                listView1.Items.Clear();
                first = false;
                for (int i = 0; i < allthreads.Count; i++)
                {
                    ListViewItem li = new ListViewItem(i + "");
                    // li.SubItems.Add(i + "");
                    var acc = threadsdata[i][0];
                    if (acc > bestacc)
                    {
                        bestacc = acc;
                        best1 = bests[i];
                        bestid = i;
                    }
                    li.SubItems.Add(threadsdata[i][0] * 100 + "");
                    var g = threadsdata[i][1];
                    var g2 = watch.ElapsedMilliseconds / 1000;
                    li.SubItems.Add(g + "");
                    li.SubItems.Add(g / g2 + "");
                    li.SubItems.Add(threadsdata[i][2] * 100 + "");
                    li.SubItems.Add(threadsdata[i][4] + "");
                    li.SubItems.Add("Working");
                    //  string[] row = { "", "", "", "" };
                    listView1.Items.Add(li);
                }
                guna2Button8.PerformClick();
            }
            else
            {
                for (int i = 0; i < allthreads.Count; i++)
                {

                    var li = listView1.Items[i];
                    // li.SubItems.Add(i + "");
                    var acc = threadsdata[i][0];
                    if (acc > bestacc)
                    {
                        bestacc = acc;
                        best1 = bests[i];
                        bestid = i;
                    }
                    li.SubItems[1].Text = (threadsdata[i][0] * 100 + "");
                    var g = threadsdata[i][1];
                    var g2 = watch.ElapsedMilliseconds / 1000;
                    if (g >= maxsteps)
                    {
                        allthreads[i].Suspend();
                    }
                    li.SubItems[2].Text=(g + "");
                    li.SubItems[3].Text=(g / g2 + "");
                    li.SubItems[4].Text=(threadsdata[i][2] * 100 + "");
                    li.SubItems[5].Text=((decimal)threadsdata[i][4] + "");
                    if (allthreads[i].ThreadState == ThreadState.Running)
                    {
                        li.SubItems[6].Text = ("Working");
                    }
                    else if (allthreads[i].ThreadState == ThreadState.Suspended)
                    {
                        li.SubItems[6].Text = ("Suspended");
                    }
                    else if (allthreads[i].ThreadState == ThreadState.Aborted)
                    {
                        li.SubItems[6].Text = ("Aborted");
                    }
                    //  string[] row = { "", "", "", "" };
                    // listView1.Items.Add(li);
                }
            }
            label5.Text = Math.Round( Convert.ToDouble(watch.ElapsedMilliseconds) / (1000*60),2) + " min";
            guna2TextBox3.Text = bestacc * 100 + "%";
            bacc = bestacc;
            try
            {
                if (bestacc > stop || Program.subs.MODE == Program.modetype.ended)
                {
                    guna2Button10_Click(sender, e);
                }
            }
            catch { }
        }
        public bool allowclose;
        [Obsolete]
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Application.Exit();
            if (!allowclose)
            {
                DialogResult dg = MessageBox.Show("Are you Sure? , you want to close the app -any pending threads will be terminated- ","", MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                if (dg == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }

            }
            try
            {
                for (int i = 0; i < allthreads.Count; i++)
                {
                    try { allthreads[i].Suspend(); }
                    catch { }
                    try { allthreads[i].Resume(); }
                    catch { }
                    try { allthreads[i].Abort(); }
                    catch { }
                }
            }
            catch { }

        //    Thread.CurrentThread.Abort();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            numdg2 = Convert.ToInt32(digitsview.Value);
            f.data = best1.write(numdg2)+ "\r\n\r\nAccuracy : " + bacc*100+"%";
            f.Show();
        }
        int numdg2 ;
        //public static string write(regressor reg,int numdg22)
        //{
        //    string mywrite = "y=";

        //    for (int i = 0; i < reg.equation.Count; i++)
        //    {
        //        for (int i2 = 0; i2 < reg.equation[0].Length; i2++)
        //        {
        //            if (Math.Abs(reg.equation[i][0]) > 0.000001)
        //            {
        //                var r = ((decimal)Math.Round(reg.equation[i][i2], numdg22)).ToString();
        //                if (i2 > 0)
        //                {
        //                    if (Math.Abs(Convert.ToDouble( r)) > 0.000001)
        //                        mywrite += " * x" + i2 + "^" + r;

        //                }
        //                else 
        //                {
        //                    if (Math.Abs(Convert.ToDouble(r)) < 0.000001)
        //                    {
        //                        break;
        //                    }
        //                    if (Convert.ToDouble(r) >= 0)
        //                    {
        //                        mywrite += "\r\n+" + r;
        //                    }
        //                    else
        //                    {
        //                        mywrite += "\r\n" + r;
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    return mywrite+"\r\n"+"Equation created by Galvin-regression app";
        //}

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            licenseachieve(1);
            savepredict(predictme(best1)); 
        }
        List<double> predictme(regressor rgs)
        {
            List<Double> ls = new List<double>();
            foreach (var g in testinput)
            {
                ls.Add(rgs.calc(rgs.equation, g));
            }
            return ls;
        }
        private void guna2Button6_Click(object sender, EventArgs e)
        {
            
            Form2 f = new Form2();
            f.data = best1.excell(numdg2);
            f.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        void fillpanel(int threadid)
        {
            idtxt.Text = threadid + "";
            acctxt.Text = threadsdata[threadid][0]*100 + " %";
            besteq.Text = bests[threadid].write(numdg2);
            liveeq.Text = liveregs[threadid].write(numdg2);
            liveacc.Text = Math.Round( threadsdata[threadid][2] * 100, 2) + "%";

            
        }
        
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                fillpanel(listView1.SelectedIndices[0]);
            }
            catch
            {

            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            foreach (Control c in setpanel.Controls)
            {
                if (c.GetType() == n0.GetType() &&c.Name != "digitsview")
                {
                    var q = (Guna.UI2.WinForms.Guna2NumericUpDown)c;
                    sentdata[Convert.ToInt32(q.Name.Substring(1))] =Convert.ToDouble( q.Value);
                }
            }
            change = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (first2)
            {
                int i = 0;
                foreach (Control c in Controls)

                {
                    c.Top = Convert.ToInt32(rates[i, 0] * Height);
                    c.Left = Convert.ToInt32(rates[i, 1] * Width);
                    if (c.Name[0] != 'b' || true)
                    {

                        c.Height = Convert.ToInt32(rates2[i, 0] * Height);
                        c.Width = Convert.ToInt32(rates2[i, 1] * Width);
                    }
                    i++;
                }
            }
        }

        private void n0_ValueChanged(object sender, EventArgs e)
        {
            refreshrecommended();
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items[listView1.SelectedIndices[0]].Selected = false;
            }
            catch { }
            listView1.Items[bestid].Selected = true;
        }

        [Obsolete]
        private void suspendThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int threadiid = listView1.SelectedIndices[0];
                allthreads[threadiid].Suspend();
            }
            catch
            {

            }
        }

        [Obsolete]
        private void resumeThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int threadiid = listView1.SelectedIndices[0];
                allthreads[threadiid].Resume();
            }
            catch { }
        }

        private void showEquationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                numdg2 = Convert.ToInt32(digitsview.Value);
                int threadiid = listView1.SelectedIndices[0];
                Form2 f = new Form2();
                f.data = bests[threadiid].write(numdg2)+ "\r\n\r\nAccuracy :" + threadsdata[threadiid][0]*100+"%";
                f.Show();
            }
            catch { }
        }

        private void showInExcellFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int threadiid = listView1.SelectedIndices[0];
                var reg = bests[threadiid];
           
                Form2 f = new Form2();
                f.data = reg.excell(numdg2);
                f.Show();
            }
            catch { }
        }

        [Obsolete]
        private void guna2Button10_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < allthreads.Count; i++)
            {
                try
                {
                    allthreads[i].Suspend();
                }
                catch { }
            }
            try
            {
                watch.Stop();
            }
            catch { }
        }

        [Obsolete]
        private void guna2Button9_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < allthreads.Count; i++)
            {
                try
                {
                    allthreads[i].Resume();
                }
                catch { }
            }
            try
            {
                watch.Start();
            }
            catch { }
        }

        private void predictToolStripMenuItem_Click(object sender, EventArgs e)
        {
            licenseachieve(1);
            int threadiid = listView1.SelectedIndices[0];
            savepredict( predictme(bests[threadiid]));
        }
        public void savepredict(List<double> predso)
        {
            var f = "";
            int i = 0;
            foreach(var ih in testinput)
            {
                if (i == 0)
                {
                    for(int q = 0; q < testinput[0].Length;q++)
                    {
                        f += "X" + (q + 1);
                        if (q != testinput[0].Length - 1)
                        {
                            f += ",";
                        }
                    }
                    f += ",Predicted";
                    f += "\r\n";
                }
                
                {
                    for (int q = 0; q < testinput[0].Length; q++)
                    {
                        f += ih[q] + "";
                        if (q != testinput[0].Length - 1)
                        {
                            f += ",";
                        }
                    }
                    f +=","+ predso[i];
                    f += "\r\n";
                }
                i++;
            }
            SaveFileDialog of = new SaveFileDialog();
            of.Filter = "Comma delicated Files|*.csv";
            of.ShowDialog();
            if (of.FileName != "")
            {
                File.WriteAllText(of.FileName, f);
            }
        }
        private void stopupdown_ValueChanged(object sender, EventArgs e)
        {
            stop =Convert.ToDouble( stopupdown.Value / 100);

        }

        private void numsteps_ValueChanged(object sender, EventArgs e)
        {
            maxsteps = Convert.ToInt32(numsteps.Value);
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            // var h = (Guna.UI2.WinForms.Guna2Button)sender;
            // h.Text="";
            int id = 1;
            try
            {
                licenseachieve(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Form3 f = new Form3(best1.copy());
            f.inputs = inputs;
            f.outputs = outputs;
        //    f.myreg = best1;
            f.Show();
        }

        private void analysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int threadiid = listView1.SelectedIndices[0];
                Form3 f = new Form3(bests[threadiid].copy());
                f.inputs = inputs;
                f.outputs = outputs;
               // f.myreg = best1;
                f.Show();
            }
            catch { }
        }

        private void calculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void calculatorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Calculator.Form5 f = new Calculator.Form5();
            f.Show();
            //       WindowsFormsApp4.Form1 f = new WindowsFormsApp4.Form1();
            //      f.Show();
        }

        private void digitsview_ValueChanged(object sender, EventArgs e)
        {
            numdg2 = Convert.ToInt32(digitsview.Value);
        }

        private void n4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            Random rd = new Random();
            if (rd.Next(0, 2) == 0)
            {
                change = false;
            }
        }

        private void saveEquationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = 2;
            try
            {
                licenseachieve(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (best1 == null)
            {
                MessageBox.Show("there's no equation to save");
                return;
            }

            SaveFileDialog sv = new SaveFileDialog();
            sv.Filter = "equation file Files|*.eqad";
            sv.ShowDialog();
            if (sv.FileName != "")
            {
                best1.saveme(sv.FileName);

            }
        }
        void fillcols()
        {
            var ncolumns = new string[inputs[0].Length + 1];
            for (int ih = 0; ih < ncolumns.Length; ih++)
            {
                ncolumns[ih] = "X" + (ih + 1);
            }
            ncolumns[ncolumns.Length-1] = "output";
            columns = ncolumns;
        }
        private void importEquationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = 2;
            try
            {
                licenseachieve(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            OpenFileDialog sv = new OpenFileDialog();
            sv.Filter = "equation file Files|*.eqad";
            sv.ShowDialog();
            if (sv.FileName != "")
            {
                best1 = regressor.load(sv.FileName);
                inputs = best1.binputs;
                outputs = best1.boutputs;
                columns = new string[0];
                fillcols();
                refreshrecommended();
                fill();
                Refresh();
                anyls.PerformClick();
            }
        }

        private void saveEquationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                int threadiid = listView1.SelectedIndices[0];
                SaveFileDialog sv = new SaveFileDialog();
                sv.Filter = "equation file Files|*.eqad";
                sv.ShowDialog();
                if (sv.FileName != "")
                {
                    bests[threadiid].saveme(sv.FileName);

                }
            }
            catch { }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutMeAndTheProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 f = new Form4();
            f.Icon = Icon;
            f.Show();
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            pname = projname.Text;
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = 2;
            try
            {
                licenseachieve(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (mproject == null)
            {
                MessageBox.Show("there's no project to save");
                return;
            }

            SaveFileDialog sv = new SaveFileDialog();
            sv.Filter = "project file Files|*.proj";
            sv.ShowDialog();
            if (sv.FileName != "")
            {
                saveproject();
                mproject.saveme(sv.FileName);

            }
        }

        [Obsolete]
        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = 2;
            try
            {
                licenseachieve(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            OpenFileDialog sv = new OpenFileDialog();
            sv.Filter = "Project file Files|*.proj";
            sv.ShowDialog();
            if (sv.FileName != "")
            {
                mproject = project.load(sv.FileName);
                loadproject(mproject);
                guna2Button10.PerformClick();
                refreshrecommended();
                Refresh();
              //  anyls.PerformClick();
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (allthreads.Count != 0)
            {
             //   Text = "done"+sentdata[0];
                saveproject();
                mproject.saveme("temp.proj");
            }
        }
        project mproject2;

        [Obsolete]
        private void restorePreviousProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = 2;
            try
            {
                licenseachieve(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult dg = MessageBox.Show("Are you Sure? The cureent project will be cleared ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dg == DialogResult.No)
            {
                return;
            }
            mproject = mproject2;
            loadproject(mproject);
            guna2Button10.PerformClick();
            refreshrecommended();
            Refresh();
            //  anyls.Perfor
        }

        private void subscribtionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Form5 f = new Form5();
            f.Icon = Icon;
            f.ShowDialog();
        }
        List<double[]> testinput;
        private void guna2Button11_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Comma delicated Files|*.csv;*.txt";
            of.ShowDialog();
            if (of.FileName != "")
            {
                guna2TextBox4.Text = of.FileName;
                testinput = readcsv(of.FileName); 
            }
        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < bests.Count; i++)
            {
                liveregs[i].equation = bests[i].copier();
                liveregs[i].sides = bests[i].sidecopy();
            }
        }

        private void fillAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < liveregs.Count; i++)
            {
                liveregs[i].equation = best1.copier();
                liveregs[i].sides = best1.sidecopy();
            }
           // start();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int threadiid = listView1.SelectedIndices[0];
                var i = threadiid;
                liveregs[i].equation = bests[i].copier();
                liveregs[i].sides = bests[i].sidecopy();
            }
            catch { }
        }

        private void fillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int threadiid = listView1.SelectedIndices[0];
                var i = threadiid;
                liveregs[i].equation = best1.copier();
                liveregs[i].sides = best1.sidecopy();
            }
            catch { }
        }

        private void n5_ValueChanged(object sender, EventArgs e)
        {

        }

        private void setpanel_Paint(object sender, PaintEventArgs e)
        {

        }
        int pg = 2;
        public bool allow;
        [Obsolete]
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                Program.fdoc = Program.f.webBrowser1.Document.Body.InnerHtml;
                xz++;
                if (xz2 >= 1 && xz >= 1 && !setf)
                {
                    setf = true;
                    Program.firsstcheck();
                    mainanme = "Galvin-regression";
                    pname = pname;
                }
            }
            catch { }
             pname = pname;
            
            //allow = true;
            //if (pg>0)
            //{
            //    Program.firsstcheck();
            //    pg --;
            //}
        }
        int xz = 0;
        int xz2 = 0;
        bool setf;
        [Obsolete]
        private void webBrowser2_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                Program.fdoc2 = Program.f.webBrowser2.Document.Body.InnerHtml;
                xz2++;
                if (xz2 >= 1 && xz >= 1 && !setf)
                {
                    setf = true;
                    Program.firsstcheck();
                    mainanme = "Galvin-regression";
                    pname = pname;
                }
            }
            catch { }
        
        }
    }
}
