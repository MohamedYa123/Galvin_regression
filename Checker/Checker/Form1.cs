using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using DeviceId;
using DeviceId.Windows;
using DeviceId.Windows.Wmi;

namespace Checker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cpuID = string.Empty;
            var g = new DeviceIdBuilder();
            cpuID = g.OnWindows(windows => windows.AddMotherboardSerialNumber()).ToString() + g.OnWindows(windows => windows.AddProcessorId()).ToString() + g.AddMachineName().ToString();
            // Cpuid = cpuID;
            textBox1.Text = cpuID;
        }
    }
}
