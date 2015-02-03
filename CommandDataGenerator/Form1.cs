using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandMonitoring.Models;
using Newtonsoft.Json;

namespace CommandDataGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            GenerateRandomValue();
        }

        private void GenerateRandomValue()
        {
            Random random = new Random();

            numericUpDown1.Value = (decimal)random.NextDouble() * random.Next(1, 10);
            numericUpDown2.Value = (decimal)random.NextDouble() * random.Next(1, 10);
            numericUpDown3.Value = (decimal)random.NextDouble() * random.Next(1, 10);
            numericUpDown4.Value = (decimal)random.NextDouble() * random.Next(1, 10);
            numericUpDown5.Value = (decimal)random.NextDouble() * random.Next(1, 10);
            numericUpDown6.Value = (decimal)random.NextDouble() * random.Next(1, 10);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PostData();
            GenerateRandomValue();
        }

        private void PostData()
        {
            // Post to our Web API
            const string uri = "http://localhost:62477/api/drillholes";
            var hole = new DrillHole();
            hole.ProjectId = 1;
            hole.TimeStamp = new DateTimeOffset(DateTime.Now);
            hole.DFPressure = (double)numericUpDown1.Value;
            hole.DFFlow = (double)numericUpDown2.Value;
            hole.Torque = (double)numericUpDown3.Value;
            hole.WOB = (double)numericUpDown4.Value;
            hole.RPM = (double)numericUpDown5.Value;
            hole.ROP = (double)numericUpDown6.Value;

            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";

            var deptSerialized = JsonConvert.SerializeObject(hole);
            using (StreamWriter sw = new StreamWriter(webRequest.GetRequestStream()))
            {
                sw.Write(deptSerialized);
            }

            HttpWebResponse httpWebResponse = webRequest.GetResponse() as HttpWebResponse;
            using (StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                Debug.WriteLine(String.Format("StatusCode == {0}", httpWebResponse.StatusCode));
                Debug.WriteLine(sr.ReadToEnd());
            }
        }
    }
}
