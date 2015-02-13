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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandMonitoring.Models;
using Newtonsoft.Json;

namespace CommandDataGenerator
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource cancellationToken;

        public Form1()
        {
            InitializeComponent();

            GenerateRandomValue();
        }

        private void GenerateRandomValue()
        {
            Random random = new Random();

            numericUpDown1.Value = (decimal)random.Next(100, 300);
            numericUpDown2.Value = (decimal)random.Next(200, 500);
            numericUpDown3.Value = (decimal)random.Next(1000, 5000);
            numericUpDown4.Value = (decimal)random.Next(0, 1000);
            numericUpDown5.Value = (decimal)random.Next(200, 500);
            numericUpDown6.Value = (decimal)random.Next(0, 1000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PostData();
            GenerateRandomValue();
        }

        private void PostData()
        {
            // Post to our Web API
            //const string uri = "http://commandhub.azurewebsites.net/api/drillholes";
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

        private async Task DoPeriodicWorkAsync(TimeSpan startTimeSpan, TimeSpan interval, CancellationToken token)
        {
            // Initial wait time before we begin the periodic loop.
            if (startTimeSpan > TimeSpan.Zero)
            {
                await Task.Delay(startTimeSpan, token);
            }

            // Repeat this loop until cancelled.
            while (!token.IsCancellationRequested)
            {
                PostData();
                GenerateRandomValue();

                // Wait to repeat again.
                if (interval > TimeSpan.Zero)
                    await Task.Delay(interval, token);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // Start after 1 second
                var dueTime = TimeSpan.FromSeconds(1);
                // Send data every second
                var interval = TimeSpan.FromSeconds(1);

                cancellationToken = new CancellationTokenSource();

                DoPeriodicWorkAsync(dueTime, interval, cancellationToken.Token);
            }
            else
            {
                cancellationToken.Cancel();
            }
        }
    }
}
