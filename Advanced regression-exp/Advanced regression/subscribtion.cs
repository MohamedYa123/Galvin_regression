using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using DeviceId;
using DeviceId.Windows;
using DeviceId.Windows.Wmi;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Advanced_regression
{
    [Serializable]
    class subscribtion
    {
        private bool activated;
        public string serialkey;
        private  string pass;
        private double offline_minutes;
        private double offline_minutes2;
        private string Cpuid;
        DateTime firstdate;
        string sbdetails;
        string path;
        public bool imoffline;
        public bool firsttime;
        private double period;
        private double days;
        private double cdays;
        private  Program.modetype mode;
        private string keylocation;
        private string pass2;
        private string licenseid;
        public string url1 = @"https://today-date.com/";
        public string url2 = @"https://galvin2022.blogspot.com/2022/05/var-now-new-datedocument.html";
        public string seperate1 = "";
        public string seperate2 = "";
        public string regexfilter = "";
        
        public string URL1 { get { return url1; } set {
                if (url1 != value)
                {
                    Program.f.webBrowser1.Url = new Uri(value);
                    Program.f.webBrowser1.Navigate(value);
                }
                url1 = value;
            } }
        public string URL2 { get { return url2; } set {
                if (url2 != value)
                {
                    Program.f.webBrowser2.Url = new Uri(value);
                    Program.f.webBrowser2.Navigate(value);
                }
                url2 = value;
            } }
        public int DAYS {
            get
            {
                return Convert.ToInt32(days);
            }
        }
        public DateTime Dateofsub
        {
            get
            {
                return firstdate;
            }
        }
        public int OFFLINE_MINUTES
        {
            get
            {
                return Convert.ToInt32(offline_minutes);
            }
        }
        public Program.modetype MODE
        {
            get
            {
                return mode;
            }
        }
        private Program.modetype MODE2
        {
            set
            {
                mode = value;
                switch (mode )
                {
                    case Program.modetype.ended:
                        // offline_minutes = 0;
                        pass = "iuewjlk";
                       // pass2 = "bgrweup";
                        days = 0;
                        break;
                    case Program.modetype.subscribtion:
                        offline_minutes = 600;
                        break;
                    case Program.modetype.trial:
                        offline_minutes = 30;
                        cdays = 7;
                        days = 7;
                        try
                        {
                           // firstdate = GetNistTime();
                        }
                        catch
                        {
                            throw new Exception("Please provide an internet connection to assure your subscribtion !");
                        }
                        break;
                }

            }
           
        }
        private void refreshoffline()
        {
            switch (MODE)
            {
                case Program.modetype.ended:
                    offline_minutes = 0;
                    break;
                case Program.modetype.subscribtion:
                    offline_minutes = 600;
                    break;
                case Program.modetype.trial:
                    offline_minutes = 30;
                    break;
            }
        }
        public string gettext()
        {
            string msg = "";
            switch (MODE)
            {
                case Program.modetype.ended:
                    msg = "Your subscribtion has ended (×⁔×) .";
                    break;
                case Program.modetype.trial:
                    msg = "You have a freetrial license , some features are limited .";
                    break;
                case Program.modetype.subscribtion:
                    msg = "You have subscribed to the program (•‿•) , you have full access to all features of the app .";
                    break;
            }
            return msg;
        }
        public bool Ended { set { activated = !value;  if (value) { MODE2 = Program.modetype.ended;
                    throw new Exception("Invalid subscribtion License !"); } } }
        public subscribtion()
        {
            pass = "Some random key of me bla 2bla2";
            pass2 = "Some random key of me bla 2bla2";
            activated = true;
            sbdetails = @"Subdetails";
            path = @"C:\Windows\system.xml";
            firsttime = true;
            offline_minutes = 30;
            MODE2 = Program.modetype.trial;
            keylocation= @"Galvin\Details";
            activated = true;
        }
        public bool case_of_subscribtion
        {
            get { return activated; }
        }
        public void decrease(double ms)
        {
            if (imoffline)
            {
                offline_minutes -= ms / (60000);
                if (offline_minutes <= 0)
                {
                    throw new Exception("you have run out offline time please provide internet connection to refresh offline time ");
                }
            }
            else
            {
                refreshoffline();
            }
        }
        public void check()
        {
            try {
                check2();
            
            } catch(Exception ex)
            {
                activated = false;
                MODE2 = Program.modetype.ended;
                if (ex.Message != "Please provide an internet connection to assure your subscribtion !")
                { throw new Exception("Invalid subscribtion License !"); }
                else
                {
                    throw ex;
                }
            }
        }
        public void check2()
        {
           // string vs = pass;
            pass = pass2;
            var g = serialkey.Split(Convert.ToChar( "\n"));
           var keyy = Registry.CurrentUser.OpenSubKey(keylocation, true);
            if (keyy == null)
            {
                 keyy = Registry.CurrentUser.CreateSubKey(keylocation);
            }
            for (int i = 0; i < g.Length; i++)
            {
                g[i] = g[i].Replace("\r", "");
            }
            var s1 = g[g.Length-4];
            var ss1 = ComputeMd5Hash(getcpu());
            if (s1 != ss1)
            {
                activated = false;
                MODE2 = Program.modetype.ended;
                throw new Exception("Invalid subscribtion License !");
            }
            var s2 = g[g.Length - 3];
            var s3 = g[g.Length - 2];
            var s4 = g[g.Length - 1];
            licenseid = s4;
            DateTime dt = Convert.ToDateTime(s3);
            firstdate = dt;
            DateTime dt2;
            try
            {
                dt2 = GetNistTime(qn:true,z:true);
            }
            catch(Exception ex)
            {
                if (ex.Message== "Invalid subscribtion License !")
                {
                    throw ex;
                }
                throw new Exception("Please provide an internet connection to assure your subscribtion !");
            }
            double masdays = Convert.ToDouble(s2);
            double daysleft = masdays-(dt2 - dt).TotalDays;
            days = daysleft;
            cdays = masdays;
            if (daysleft<=0 || daysleft > masdays)
            {
                Ended = true;
                return;
            }

            var q = g[0];
            var q2 = serial(s1,s2,s3,s4);
            if (q == q2)
            {
                activated = true;
                MODE2 = Program.modetype.subscribtion;
            }
            else
            {
                activated = false;
                MODE2 = Program.modetype.ended;
                throw new Exception("Invalid subscribtion License !");
            }
            regressave(regresassumption());
            filesave(fileassumption());
            //pass = vs;
        }
        string serial(string s1,string s2,string s3,string s4)
        {
            var part1 = ComputeMd5Hash(s1);
            var part2 = ComputeMd5Hash(s2);
            var part3 = ComputeMd5Hash(s3);
            var part5 = ComputeMd5Hash(s4);
            var ss = s1 + s2 + s3;
            var part4 = ComputeMd5Hash(ss);
            var q2 = part1 + "" + part2 + "" + part3 + "" + part4+part5;
            return q2;
        }
        string regresassumption()
        {
            string s2 = secretdecode("107" + firstdate + "102");
            string checks2 = secretdecode("This is sb") + " " + s2;
            return checks2;
        }
        private string secretdecode(string msg)
        {

            string msg2 = msg + Cpuid;
            var ans = ComputeMd5Hash(msg2);
            return ans;
        }
        string fileassumption()
        {
            string mywrite = secretdecode("12This is random sb2");
            string s2 = secretdecode(firstdate + "2");
            string checks = mywrite + " " + s2;
            return checks;
        }
        void regressave(string msg)
        {
            var keyy = Registry.CurrentUser.OpenSubKey(keylocation, true);
            keyy.SetValue(sbdetails, msg);
        }
        void filesave(string msg)
        {
            try
            {
                File.Delete(path);
                File.Create(path).Close();
                File.WriteAllText(path, msg);
            }
            catch
            {
                throw new Exception("Access denied");
            }
        }
        string readregress()
        {
            var keyy = Registry.CurrentUser.OpenSubKey(keylocation, true);
            var s= (string)keyy.GetValue(sbdetails);
            return s;
        }
        string readfile()
        {
           if (!File.Exists(path))
            {
                MODE2 = Program.modetype.ended;
                return "";
            }
            string msg = "";
            try
            {
                msg= File.ReadAllText(path);
            }
            catch
            {
                Random rd = new Random();
                msg = rd.Next(0, 1000000) + "";
            }
            return msg;
        }
        public void rotinecheck(double ms,bool  qn2=false ,bool zz=false)
        {
            getcpu();
            if (MODE == Program.modetype.ended)
            {
                string g1 = " ABCDEFGHIJKLMNOPQRSTUVWXY123456789*-+/&^%$#@}[{abcdefghijklmnopqrstuvwxyz";
                Random rd1 = new Random();
                int keylength1 = 25;
                pass = "";
                for (int i = 0; i < keylength1; i++)
                {
                    int z = rd1.Next(0, g1.Length);
                    pass += g1.Substring(z, 1);
                }
                var r11 = regresassumption();
                var r21 = fileassumption();
                filesave(r21);
                regressave(r11);
                pass = pass2;
                return;
            }

            var r1 = regresassumption();
            var r2 = fileassumption();
            if (!File.Exists(path) && Registry.CurrentUser.OpenSubKey(keylocation, true) == null && firsttime )
            {

                try
                {
                   
                    firstdate = GetNistTime(qn: qn2, z: zz);
                    Registry.CurrentUser.CreateSubKey(keylocation);

                }
                catch
                {
                    if (firsttime)
                    {
                        throw new Exception("Please provide an internet connection to assure your subscribtion !");
                    }
                    Registry.CurrentUser.CreateSubKey(keylocation);
                    filesave(r2);
                    regressave(r1);

                    throw new Exception("Please provide an internet connection to assure your subscribtion !");
                }
                filesave(r2);
                regressave(r1);
                MODE2 = Program.modetype.trial;
            }
            try
            {
                if (readfile() == r2 && readregress() == r1 && mode != Program.modetype.ended)
                {

                }
                else
                {
                    Ended = true;
                    MODE2 = Program.modetype.ended;
                    //    throw new Exception("");
                }
            }
            catch
            {
                Ended = true;
                MODE2 = Program.modetype.ended;
            }
            try
            {
                DateTime dt2 = GetNistTime(qn:qn2,z:zz);
                double daysleft =cdays- (dt2 - firstdate).TotalDays;
                if (daysleft <= 0 || daysleft > cdays)
                {
                    Ended = true;
                    return;
                }
                days = daysleft;
                refreshoffline();
            }
            catch
            {
                imoffline = true;
                decrease(ms);
            }
            string g = " ABCDEFGHIJKLMNOPQRSTUVWXY123456789*-+/&^%$#@}[{abcdefghijklmnopqrstuvwxyz";
            Random rd = new Random();
            int keylength = 25;
            pass = "";
            for (int i = 0; i < keylength; i++)
            {
                int z = rd.Next(0, g.Length);
                pass += g.Substring(z, 1);
            }
            filesave(fileassumption());
            regressave(regresassumption());

         //   rotinecheck(1);
        }
        private void update()
        {
            imoffline = false;
            if (firsttime)
            {
                try
                {
                    firstdate = GetNistTime();
                }
                catch
                {
                    throw new Exception("Please provide an internet connection to assure your subscribtion !");
                }
                firsttime = false;
            }
            DateTime lastdate;
            try
            {
                lastdate = GetNistTime();
                var temp_period = (lastdate - firstdate).TotalDays;
                if (temp_period > 0 && temp_period < days)
                {
                    period = temp_period;
                }
                else
                {
                    period = 0;
                    Ended = true;
                }
            }
            catch
            {
                imoffline = true;
            }
            if (MODE==Program.modetype.ended)
            {
                throw new Exception("Your Subscribtion has ended ! ");
            }
        }
        private string ComputeMd5Hash(string message)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] txt = UTF8Encoding.UTF8.GetBytes(message);
            var md5key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(pass));
            TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider();
            trip.Key = md5key;
            trip.Mode = CipherMode.ECB;
            trip.Padding = PaddingMode.ANSIX923;
            ICryptoTransform trans = trip.CreateEncryptor();
            byte[] result = trans.TransformFinalBlock(txt, 0, txt.Length);
            string rs = Convert.ToBase64String(result);
            return rs;
        }
        private DateTime GetNistTime(bool qn=false,bool z=false)
        {
            if (z)
            {
                hat();
               // var w = new DateTime();
                if (tempdt.Year==( (new DateTime()).Year))
                {
                    throw new Exception("");
                }
                return tempdt;
            }
            tempdt = new DateTime();
            DateTime dt = DateTime.Now;
            ThreadStart thr = new ThreadStart(hat);
            Thread th = new Thread(thr);
            th.Start();
            hat();
            if (qn)
            {
                Thread.Sleep(2500);
            }
            else
            {
                Thread.Sleep(10000);
            }
            if (tempdt.Year==1 || MODE==Program.modetype.ended)
            {
                try
                {
                    th.Abort();
                }
                catch { }
                throw new Exception("");
            }
            dt = tempdt;
            return dt;
        }
        private DateTime tempdt;
        private  void hat()
        {
            if (Cpuid == "0CHE34VMTC1Q4NYF09EHB1B7QANJ4SQY6363BTNS2AV1E64WBTQG0AC1NBS2HJ8QNYZB9MKSS4R02DKPDYBXZR12J495D2FK6X61TRYGAR0F55GRM4SFKZZ7CEKRED0QFZFYP7JSG4ZF9S3SAGYN638QAJ3G" && false)// here here"0CHE34VMTC1Q4NYF09EHB1B7QANJ4SQY6363BTNS2AV1E64WBTQG0AC1NBS2HJ8QNYZB9MKSS4R02DKPDYBXZR12J495D2FK6X61TRYGAR0F55GRM4SFKZZ7CEKRED0QFZFYP7JSG4ZF9S3SAGYN638QAJ3G"
            {
                tempdt = DateTime.Now;
                return;
            }
        //    try
        //    {
                //var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");
                //var response = myHttpWebRequest.GetResponse();
                //string todaysDates = response.Headers["date"];
                //tempdt = DateTime.ParseExact(todaysDates,
                //                           "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                //                           CultureInfo.InvariantCulture.DateTimeFormat,
                //                           DateTimeStyles.AssumeUniversal);
                using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
                {
                    // client.DownloadFile("http://yoursite.com/page.html", @"C:\localfile.html");

                    // Or you can get the file content without saving it
                    //string htmlCode = client.DownloadString("https://galvin2022.blogspot.com/2022/05/var-now-new-datedocument.html");
                    //    Program.f.webBrowser1.Url = new Uri("https://galvin2022.blogspot.com/2022/05/var-now-new-datedocument.html");
                    //  Program.f.webBrowser1.
               //     var d = Program.f.webBrowser1.Document;
                    //string documentdt = Program.f.webBrowser1.Document.Body.InnerHtml;
                    //Program.f.webBrowser1.Refresh();
                    Regex reg = new Regex(@"###.+###");
                    Regex reg2 = new Regex(@"##[\w+,]+##");
                    Regex reg3 = new Regex(@"######.+######");
                    string fg = reg.Match(Program.fdoc).Value;
                    if (fg == "")
                {
                    tempdt = new DateTime(); return;
                }
                    fg= fg.Replace("#", "");
                    URL2 = fg;
                    string fg2 = reg2.Match(Program.fdoc).Value;
                    string fg3 = reg3.Match(Program.fdoc).Value;
                    regexfilter = fg3.Replace("#","");
                    Regex regdate = new Regex(regexfilter);
                    fg= regdate.Match(Program.fdoc2).Value;
                    Regex regfirst = new Regex(@"####.+####");
                    string firstpart = regfirst.Match(Program.fdoc).Value;
                    seperate1 = firstpart.Replace("#", ""); 
                    Regex reglast = new Regex(@"#####.+#####");
                    string lastpart = regfirst.Match(Program.fdoc).Value;
                    seperate2 = lastpart.Replace("#", ""); 
                    fg = fg.Replace(seperate1, "");
                    fg = fg.Replace(seperate2, "");
                    tempdt = new DateTime();
                    if (fg == "")
                    {
                        throw new Exception("");
                    }
                //    fg = fg.Replace("#", "");
                    
                    fg2 = fg2.Replace("#", "");
                    var rf = fg.Split('/');
                    var rf2 = fg2.Split(',');
                    tempdt = new DateTime( Convert.ToInt32(rf[2]), Convert.ToInt32(rf[0]), Convert.ToInt32(rf[1]));
                    bool destroy = true;
                    if (licenseid == ""|| licenseid == null)
                {
                    licenseid = "1";
                }
                    foreach (var gs in rf2)
                    {
                        if (gs == licenseid)
                        {
                            destroy = false;
                            break;
                        }
                    }
                    if (destroy)
                    {
                        Ended = true;
                    }
                  //  tempdt = new DateTime(2022,5,10);
                    

                    
                    
                }
      //  }
          //  catch (Exception ex) { if (ex.Message=="Invalid subscribtion license !") throw ex; }
        }
       
        private void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser wb = sender as WebBrowser;
            // wb.Document is not null at this point
        }
        bool checkvm()
        {
            using (var searcher = new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
            {
                using (var items = searcher.Get())
                {
                    foreach (var item in items)
                    {
                        string manufacturer = item["Manufacturer"].ToString().ToLower();
                        if ((manufacturer == "microsoft corporation" && item["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL"))
                            || manufacturer.Contains("vmware")
                            || item["Model"].ToString() == "VirtualBox")
                        {

                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private string getcpu()
        {
            if (checkvm())
            {
                MODE2 = Program.modetype.ended;
                // throw new Exception("Invalid license");
            }
            string cpuID = string.Empty;
            var g = new DeviceIdBuilder();
            cpuID = g.OnWindows(windows => windows.AddMotherboardSerialNumber()).ToString() + g.OnWindows(windows => windows.AddProcessorId()).ToString() + g.AddMachineName().ToString();
            Cpuid = cpuID;
            return cpuID;
        }
        public void saveme(string datapath)
        {
            firsttime = false;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream writerFileStream =
                 new FileStream(datapath, FileMode.Create, FileAccess.Write);
            // Save our dictionary of friends to file
            formatter.Serialize(writerFileStream, this);
            // Close the writerFileStream when we are done.
            writerFileStream.Close();

        }

        public static subscribtion load(string datapath)
        {
            subscribtion tc;
            FileStream readerFileStream = new FileStream(datapath,
    FileMode.Open, FileAccess.Read);
            BinaryFormatter formatter = new BinaryFormatter();
            // Reconstruct information of our friends from file.
            tc = (subscribtion)formatter.Deserialize(readerFileStream);
            // Close the readerFileStream when we are done
            readerFileStream.Close();
            return tc;
        }
    }
    //class trial
    //{
    //    private string key;
    //    private double offline_minutes;
    //    private DateTime firstdate;
    //    private double days;
    //    private double period;
    //    bool firsttime;
    //    private bool ended;
    //    bool imoffline;
    //    string cpuid;
    //    string path;
    //    string trialdetails;
    //    private string Cpuid { set { if (cpuid != "" || cpuid != null || cpuid!=value) { Ended = true; } cpuid = value; } get { return cpuid; } }
    //    public double Offline_Minutes { get { return offline_minutes; } }
    //    public bool Ended { set { ended = value;if (ended) { updatefilefail();updateregesfail(); throw new Exception("Your trial has ended"); } } }
    //    public void decrease(double ms)
    //    {
    //        if (imoffline)
    //        {
    //            offline_minutes -= ms / 6000;
    //            if (offline_minutes <= 0)
    //            {
    //                Ended = true;
    //                throw new Exception("Your trial has ended");
    //            }
    //        }
    //    }
    //    public trial(bool g=false)
    //    {
    //        firsttime = true;
    //        offline_minutes = 120;
    //        days = 15;
    //        RegistryKey keyy = Registry.CurrentUser.OpenSubKey(@"Galvin\Details",true);
    //        path = @"C:\Windows\system.xml";
    //        trialdetails = "Trial details";
    //       // string mywrite = secretdecode("This is free trial");
    //       // string s2 = secretdecode(firstdate + "");
    //       // string checks = mywrite + " " + s2;
    //       //string checks2 = secretdecode("This7is6free5trial2") + " " + s2;
    //       //   RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Galvin\Details");
    //        if (keyy == null && g)
    //        {
    //            keyy = Registry.CurrentUser.CreateSubKey(@"Galvin\Details");
    //            updatereges();
    //        }
    //        else if (!g)
    //        {
    //            Ended = true;
    //        }
    //        if (g)
    //        {

    //            if (File.Exists(path))
    //            {
    //                Ended = true;
    //            }

    //            File.Create(path).Close();
    //            updatefile();
    //        }
    //        keyy .Close();
    //    }
    //    private string secretdecode(string msg)
    //    {
    //        getcpu();
    //        string msg2 = msg + Cpuid;
    //        var ans = ComputeMd5Hash(msg2);
    //        return ans;
    //    }
    //    private string ComputeMd5Hash(string message)
    //    {
    //        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
    //        byte[] txt = UTF8Encoding.UTF8.GetBytes(message);
    //        var md5key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
    //        TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider();
    //        trip.Key = md5key;
    //        trip.Mode = CipherMode.ECB;
    //        trip.Padding = PaddingMode.ANSIX923;
    //        ICryptoTransform trans = trip.CreateEncryptor();
    //        byte[] result = trans.TransformFinalBlock(txt, 0, txt.Length);
    //        string rs = Convert.ToBase64String(result);
    //        return rs;
    //    }
    //    private void getcpu()
    //    {
    //        string cpuID = string.Empty;
    //        ManagementClass mc = new ManagementClass("win32_processor");
    //        ManagementObjectCollection moc = mc.GetInstances();

    //        foreach (ManagementObject mo in moc)
    //        {
    //            if (cpuID == "")
    //            {
    //                //Remark gets only the first CPU ID
    //                cpuID = mo.Properties["processorID"].Value.ToString();

    //            }
    //        }
    //        Cpuid = cpuID;
    //    }
    //    public void check()
    //    {
    //        string mywrite = secretdecode("This is free trial");
    //        string s2 = secretdecode(firstdate + "");
    //        string checks = mywrite + " " + s2;
    //        string othercheck = File.ReadAllText(path);
    //        if (othercheck != checks)
    //        {
    //            Ended = true;
    //        }
    //        var keyy = Registry.CurrentUser.OpenSubKey(@"Galvin\Details", true);
    //        s2 = secretdecode("17" + firstdate + "12");
    //        string checks2 = secretdecode("This7is6free5trial2") + " " + s2;
    //        if ((string)keyy.GetValue(trialdetails) != checks2)
    //        {
    //            Ended = true;
    //        }
    //        string g = " ABCDEFGHIJKLMNOPQRSTUVWXY123456789*-+/&^%$#@}[{abcdefghijklmnopqrstuvwxyz";
    //        Random rd = new Random();
    //        int keylength = 25;
    //        key = "";
    //        for (int i=0; i < keylength; i++)
    //        {
    //            int z = rd.Next(0, g.Length);
    //            key += g.Substring(z, 1); 
    //        }
    //        updatefile();
    //        updatereges();
    //    }
    //    private string regresassumption()
    //    {
    //        return "";
    //    }
    //    private string fileassumption()
    //    {
    //        return "";
    //    }
    //    private string readfile()
    //    {
    //        return "";
    //    }
    //    private string readregress()
    //    {
    //        return "";
    //    }
    //    public void filesave( string msg)
    //    {

    //    }
    //    public void regressave(string msg)
    //    {

    //    }
    //    public void rotinecheck(double ms)
    //    {
    //        var r1 = regresassumption();
    //        var r2 = fileassumption();
    //        if (readfile() == r2 && readregress() == r1)
    //        {

    //        }
    //        else
    //        {
    //            Ended = true;
    //        }
    //        DateTime dt2 = GetNistTime();
    //        double daysleft = (dt2 - firstdate).TotalDays;
    //        if (daysleft < 0 || daysleft > days)
    //        {
    //            Ended = true;
    //            return;
    //        }
    //        days = daysleft;
    //        string g = " ABCDEFGHIJKLMNOPQRSTUVWXY123456789*-+/&^%$#@}[{abcdefghijklmnopqrstuvwxyz";
    //        Random rd = new Random();
    //        int keylength = 25;
    //        key = "";
    //        for (int i = 0; i < keylength; i++)
    //        {
    //            int z = rd.Next(0, g.Length);
    //            key += g.Substring(z, 1);
    //        }
    //        filesave(fileassumption());
    //        regressave(regresassumption());
    //        decrease(ms);
    //        //   rotinecheck(1);
    //    }
    //    void updatefile()
    //    {
    //        string mywrite = secretdecode("This is free trial");
    //        string s2 = secretdecode(firstdate + "");
    //        string checks = mywrite + " " + s2;
    //        File.Delete(path);
    //        File.Create(path).Close();
    //        File.WriteAllText(path, checks);


    //    }
    //    void updatereges()
    //    {
    //        var keyy= Registry.CurrentUser.OpenSubKey(@"Galvin\Details",true);
    //        string s2 = secretdecode("17"+firstdate + "12");
    //        string checks2 = secretdecode("This7is6free5trial2") + " " + s2;
    //        keyy.SetValue(trialdetails, checks2);
    //    }
    //    void updatefilefail()
    //    {
    //        string mywrite = secretdecode("This is free trirgtuejowpal");
    //        string s2 = secretdecode(firstdate + "");
    //        string checks = mywrite + " " + s2;
    //        File.Delete(path);
    //        File.Create(path).Close();
    //        File.WriteAllText(path, checks);


    //    }
    //    void updateregesfail()
    //    {
    //        var keyy = Registry.CurrentUser.OpenSubKey(@"Galvin\Details", true);
    //        string s2 = secretdecode("17" + firstdate + "12");
    //        string checks2 = secretdecode("This7is6frerycsebhidmkl,ee5trial2") + " " + s2;
    //        keyy.SetValue(trialdetails, checks2);
    //    }
    //    private DateTime GetNistTime()
    //    {
    //        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");
    //        var response = myHttpWebRequest.GetResponse();
    //        string todaysDates = response.Headers["date"];
    //        return DateTime.ParseExact(todaysDates,
    //                                   "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
    //                                   CultureInfo.InvariantCulture.DateTimeFormat,
    //                                   DateTimeStyles.AssumeUniversal);
    //    }
    //    private void update()
    //    {
    //        imoffline = false;

    //            if (firsttime)
    //            {
    //            try
    //            {
    //                firstdate = GetNistTime();
    //            }
    //            catch {
    //                throw new Exception("Please provide an internet connection to start your free trial !");
    //            }
    //                firsttime = false;
    //            }
    //        DateTime lastdate;
    //        try
    //        {
    //            lastdate = GetNistTime();
    //            var temp_period = (lastdate - firstdate).TotalDays;
    //            if (temp_period >0 && temp_period < days)
    //            {
    //                period = temp_period;
    //            }
    //            else
    //            {
    //                period = 0;
    //                Ended = true;
    //            }
    //        }
    //        catch
    //        {
    //            imoffline = true;
    //        }
    //        if (ended)
    //        {
    //            throw new Exception("Your free trial has ended ! ");
    //        }
    //    }
    //    public void saveme(string datapath)
    //    {
    //        BinaryFormatter formatter = new BinaryFormatter();
    //        FileStream writerFileStream =
    //             new FileStream(datapath, FileMode.Create, FileAccess.Write);
    //        // Save our dictionary of friends to file
    //        formatter.Serialize(writerFileStream, this);
    //        // Close the writerFileStream when we are done.
    //        writerFileStream.Close();

    //    }

    //    public static trial load(string datapath)
    //    {
    //        trial tc;
    //        FileStream readerFileStream = new FileStream(datapath,
    //FileMode.Open, FileAccess.Read);
    //        BinaryFormatter formatter = new BinaryFormatter();
    //        // Reconstruct information of our friends from file.
    //        tc = (trial)formatter.Deserialize(readerFileStream);
    //        // Close the readerFileStream when we are done
    //        readerFileStream.Close();
    //        return tc;
    //    }
    //}
}
