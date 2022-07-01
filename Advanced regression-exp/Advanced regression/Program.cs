using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Advanced_regression
{
    static class Program
    {
        [STAThread]
        [Obsolete]

        static void Main()
        {
            Application.ThreadException += delegate
            {
                MessageBox.Show("Unexpected error happened", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            };
            //double q = 0.00000000001;
            //MessageBox.Show((decimal)q + "");
            subname = "Subscribtion license.sub";
            trialname = "Subscribtion license.sub";
            //  File.Delete(subname);
            msgg = "";


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // firsstcheck();


            try
            {
                f = new Form1();

                Application.Run(f);
            }
            catch
            {
                MessageBox.Show("Unexpected error happened", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public static  Form1 f;
        public static  string fdoc;
        public static  string fdoc2;
        [Obsolete]
        [STAThread]
        public static void firsstcheck()
        {
            try
            {
                if (File.Exists(subname))
                {
                    subs = subscribtion.load(subname);

                    //   Thread.Sleep(2000);

                    subs.rotinecheck(ms: 0, qn2: true);
                    subs.saveme(subname);

                }
                else
                {
                try
                {
                    subs = new subscribtion();
                //    f = new Form1();
                //    Thread.Sleep(2000);
                    //fdoc = Program.f.webBrowser1.Document.Body.InnerHtml;
                    subs.rotinecheck(ms: 0, qn2: true);
                    subs.saveme(subname);
                }
                catch (Exception ex)
                {



                        MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // return;
                    }
            }
            }
            catch (Exception ex) { if (subs.firsttime) { MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error); Program.f.allowclose = true; Application.Exit(); } else { subs.saveme(subname); } };
            //ThreadStart thr = new ThreadStart(check);
            //Thread nth = new Thread(thr);
            //nth.Start();

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 20000;
            t.Tick += delegate
            {
                itime();
            };
            t.Start();
            

            //try
            //{


            //    fdoc = Program.f.webBrowser1.Document.Body.InnerHtml;
            //}
            //catch
            //{
            //    MessageBox.Show("Unexpected error happened", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

          //  t.Stop();
          //  nth.Abort();
        }
        static string msgg;
        public static bool err;
        static void itime()
        {
            if (err) { return; }
            try
            {
                fdoc = Program.f.webBrowser1.Document.Body.InnerHtml;
            }
            catch { };
            Program.f.webBrowser1.Refresh();
            Program.f.webBrowser2.Refresh();
            ThreadStart thr = new ThreadStart(check);
            Thread th = new Thread(thr);
            th.Start();
            if (msgg != "")
            {
                err = true;
                MessageBox.Show(msgg, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                f.allowclose = true;
                Application.Exit();
            }
        }
        static void check()
        {
            int slep = 37000;
            try
            {
                subs.rotinecheck(slep);
                if (File.Exists(subname))
                {
                    subs.saveme(subname);
                }
                else
                {
                    subs = new subscribtion();
                    subs.rotinecheck(slep);
                    subs.saveme(subname);
                }
                
            }
            catch(Exception ex)
            {
                //   check();
                msgg = ex.Message;
            }
            Thread.CurrentThread.Abort();
         //   Thread.Sleep(slep);
           // check();
        }
        public  enum modetype{trial,subscribtion,ended,suspendedfull, suspendedtrial };
        public static modetype mode;
        public static subscribtion subs;
        //public static trial tri;
        public static string subname;
        public static string trialname;
    }
}
