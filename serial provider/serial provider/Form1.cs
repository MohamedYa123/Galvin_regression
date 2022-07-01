using DeviceId;
using DeviceId.Windows;
using DeviceId.Windows.Wmi;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace serial_provider
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string pass;
        string password;
        private void button1_Click(object sender, EventArgs e)
        {
            // bool g = (ComputeMd5Hash(Convert.ToString(numericUpDown1.Value)) == ComputeMd5Hash(Convert.ToString(180)));
            //MessageBox.Show(g +"");
            if (passw.Text != password)
            {
                throw new Exception("Invalid password !");
            }
            var s1 = ComputeMd5Hash(textBox1.Text);
            var s4 = licid.Text;
            var s2 = Convert.ToString(numericUpDown1.Value);
            var s3 = DateTime.Now.ToString("dd MMM yyyy");
            var part1 = ComputeMd5Hash(s1);
            var part2 = ComputeMd5Hash(s2);
            var part3 = ComputeMd5Hash(s3);
            var part5 = ComputeMd5Hash(s4);
            var ss = s1 + s2 + s3;
            var part4 = ComputeMd5Hash(ss);
            textBox2.Text = part1 + part2 + part3 +part4+ part5+ "\r\n" +s1 + "\r\n" + s2 + "\r\n" + s3 + "\r\n" + s4;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //pass = "1";
            pass = "Some random key of me bla 2bla2";
            password = "doublequick 0";
           // pass = "1";
        }
        public  string ComputeMd5Hash(string message)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] txt =UTF8Encoding.UTF8.GetBytes( message);
            var md5key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(pass));
            TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider();
            trip.Key = md5key;
            trip.Mode = CipherMode.ECB;
            trip.Padding = PaddingMode.ANSIX923;
            ICryptoTransform trans = trip.CreateEncryptor();
            byte[] result = trans.TransformFinalBlock(txt,0,txt.Length);
            string rs = Convert.ToBase64String(result);
            return rs;
        }
        public string UnlockMd5Hash(string message)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] txt = Convert.FromBase64String(textBox1.Text);
            var md5key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(pass));
            TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider();
            trip.Key = md5key;
            trip.Mode = CipherMode.ECB;
            trip.Padding = PaddingMode.PKCS7;
            ICryptoTransform trans = trip.CreateDecryptor();
            byte[] result = trans.TransformFinalBlock(txt, 0, txt.Length);
            string rs = UTF8Encoding.UTF8.GetString(result);
            return rs;
        }
        public static DateTime GetNistTime()
        {
            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");
            var response = myHttpWebRequest.GetResponse();
            string todaysDates = response.Headers["date"];
            return DateTime.ParseExact(todaysDates,
                                       "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                       CultureInfo.InvariantCulture.DateTimeFormat,
                                       DateTimeStyles.AssumeUniversal);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string cpuID = string.Empty;
            var g = new DeviceIdBuilder();
            cpuID = g.OnWindows(windows => windows.AddMotherboardSerialNumber()).ToString() + g.OnWindows(windows => windows.AddProcessorId()).ToString() + g.AddMachineName().ToString();
           // Cpuid = cpuID;
            textBox1.Text = cpuID;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //textBox2.Text= UnlockMd5Hash(textBox2.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string licid2 = "";
            var g1 = "ABCDEFGHIJKLMNOPQRSTUVWXY0123456789";
            Random rd1 = new Random();
            int keylength1 = 40;
       //     pass = "";
            for (int i = 0; i < keylength1; i++)
            {
                int z = rd1.Next(0, g1.Length);
                licid2 += g1.Substring(z, 1);
            }
            licid.Text = licid2;
        }
    }
}
