using Newtonsoft.Json;
using Oanda;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static Trade_forex.Tradeing_Forex;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Trade_forex
{
    public partial class Tradeing_Forex : Form
    {
        static int count = 60;
        public Tradeing_Forex()
        {
            InitializeComponent();

    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.BackColor == Color.Green)
            {
                button1.BackColor = Color.Transparent;
                timer1.Stop();
                timer1.Enabled = false;
            }
            else
            {
                count = 60;
                label1.Text = "นับถอยหลังในอีก  : " + count;
                timer1.Tick += Timer1_Tick;
                timer1.Enabled = true;
                timer1.Start();
            }
        
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() => {
                if (count > 0)
                {
                    count--;
                    label1.Text = "นับถอยหลังในอีก  : " + count;
                }
                else if (count <= 0)
                {
                    label1.Text = "เริ่มกระบวนการซื้อขาย";
                    button1.BackColor = Color.Green;
                    Thread.Sleep(5000);
                    count = 60;
                    timer1.Stop();
                    timer1.Enabled = false;
                    Load_by_click();
                    timer1.Enabled = true;
                    label1.Text = "นับถอยหลังในอีก  : " + count;
                }
            }));
        }

        public void Load_by_click()
        {
            using (HttpClient client = new HttpClient())
            {
                string Apikey = textBox1.Text;
                string symbol = textBox2.Text;
                string apiUrl = $"https://v6.exchangerate-api.com/v6/f5e5f8c38f2e7d14e87c3b38/latest/USD";
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    Root forexData = JsonConvert.DeserializeObject<Root>(json);



                    // วิเคราะห์ข้อมูล
                    double movingAverage = Values_Exchange.Excange(forexData, "EUR");

                    // กำหนดทิศทางการซื้อขาย
                    if (movingAverage > 2)
                    {
                        Treading.sale(Convert.ToDouble( textBox4.Text));
                    }
                    else
                    {
                        Treading.buy(Convert.ToDouble(textBox3.Text));
                    }

                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }

            }
        }
    }
    // คลาสที่ใช้เก็บข้อมูล Forex ที่ได้จาก API

    public class ConversionRates
        {
            public double USD { get; set; }
            public double AED { get; set; }
            public double AFN { get; set; }
            public double ALL { get; set; }
            public double AMD { get; set; }
            public double ANG { get; set; }
            public double AOA { get; set; }
            public double ARS { get; set; }
            public double AUD { get; set; }
            public double AWG { get; set; }
            public double AZN { get; set; }
            public double BAM { get; set; }
            public double BBD { get; set; }
            public double BDT { get; set; }
            public double BGN { get; set; }
            public double BHD { get; set; }
            public double BIF { get; set; }
            public double BMD { get; set; }
            public double BND { get; set; }
            public double BOB { get; set; }
            public double BRL { get; set; }
            public double BSD { get; set; }
            public double BTN { get; set; }
            public double BWP { get; set; }
            public double BYN { get; set; }
            public double BZD { get; set; }
            public double CAD { get; set; }
            public double CDF { get; set; }
            public double CHF { get; set; }
            public double CLP { get; set; }
            public double CNY { get; set; }
            public double COP { get; set; }
            public double CRC { get; set; }
            public double CUP { get; set; }
            public double CVE { get; set; }
            public double CZK { get; set; }
            public double DJF { get; set; }
            public double DKK { get; set; }
            public double DOP { get; set; }
            public double DZD { get; set; }
            public double EGP { get; set; }
            public double ERN { get; set; }
            public double ETB { get; set; }
            public double EUR { get; set; }
            public double FJD { get; set; }
            public double FKP { get; set; }
            public double FOK { get; set; }
            public double GBP { get; set; }
            public double GEL { get; set; }
            public double GGP { get; set; }
            public double GHS { get; set; }
            public double GIP { get; set; }
            public double GMD { get; set; }
            public double GNF { get; set; }
            public double GTQ { get; set; }
            public double GYD { get; set; }
            public double HKD { get; set; }
            public double HNL { get; set; }
            public double HRK { get; set; }
            public double HTG { get; set; }
            public double HUF { get; set; }
            public double IDR { get; set; }
            public double ILS { get; set; }
            public double IMP { get; set; }
            public double INR { get; set; }
            public double IQD { get; set; }
            public double IRR { get; set; }
            public double ISK { get; set; }
            public double JEP { get; set; }
            public double JMD { get; set; }
            public double JOD { get; set; }
            public double JPY { get; set; }
            public double KES { get; set; }
            public double KGS { get; set; }
            public double KHR { get; set; }
            public double KID { get; set; }
            public double KMF { get; set; }
            public double KRW { get; set; }
            public double KWD { get; set; }
            public double KYD { get; set; }
            public double KZT { get; set; }
            public double LAK { get; set; }
            public double LBP { get; set; }
            public double LKR { get; set; }
            public double LRD { get; set; }
            public double LSL { get; set; }
            public double LYD { get; set; }
            public double MAD { get; set; }
            public double MDL { get; set; }
            public double MGA { get; set; }
            public double MKD { get; set; }
            public double MMK { get; set; }
            public double MNT { get; set; }
            public double MOP { get; set; }
            public double MRU { get; set; }
            public double MUR { get; set; }
            public double MVR { get; set; }
            public double MWK { get; set; }
            public double MXN { get; set; }
            public double MYR { get; set; }
            public double MZN { get; set; }
            public double NAD { get; set; }
            public double NGN { get; set; }
            public double NIO { get; set; }
            public double NOK { get; set; }
            public double NPR { get; set; }
            public double NZD { get; set; }
            public double OMR { get; set; }
            public double PAB { get; set; }
            public double PEN { get; set; }
            public double PGK { get; set; }
            public double PHP { get; set; }
            public double PKR { get; set; }
            public double PLN { get; set; }
            public double PYG { get; set; }
            public double QAR { get; set; }
            public double RON { get; set; }
            public double RSD { get; set; }
            public double RUB { get; set; }
            public double RWF { get; set; }
            public double SAR { get; set; }
            public double SBD { get; set; }
            public double SCR { get; set; }
            public double SDG { get; set; }
            public double SEK { get; set; }
            public double SGD { get; set; }
            public double SHP { get; set; }
            public double SLE { get; set; }
            public double SLL { get; set; }
            public double SOS { get; set; }
            public double SRD { get; set; }
            public double SSP { get; set; }
            public double STN { get; set; }
            public double SYP { get; set; }
            public double SZL { get; set; }
            public double THB { get; set; }
            public double TJS { get; set; }
            public double TMT { get; set; }
            public double TND { get; set; }
            public double TOP { get; set; }
            public double TRY { get; set; }
            public double TTD { get; set; }
            public double TVD { get; set; }
            public double TWD { get; set; }
            public double TZS { get; set; }
            public double UAH { get; set; }
            public double UGX { get; set; }
            public double UYU { get; set; }
            public double UZS { get; set; }
            public double VES { get; set; }
            public double VND { get; set; }
            public double VUV { get; set; }
            public double WST { get; set; }
            public double XAF { get; set; }
            public double XCD { get; set; }
            public double XDR { get; set; }
            public double XOF { get; set; }
            public double XPF { get; set; }
            public double YER { get; set; }
            public double ZAR { get; set; }
            public double ZMW { get; set; }
            public double ZWL { get; set; }
        }

      public class Root
        {
            public string result { get; set; }
            public string documentation { get; set; }
            public string terms_of_use { get; set; }
            public double time_last_update_unix { get; set; }
            public string time_last_update_utc { get; set; }
            public double time_next_update_unix { get; set; }
            public string time_next_update_utc { get; set; }
            public string base_code { get; set; }
            public ConversionRates conversion_rates { get; set; }
        }


    public class Values_Exchange
    {
        public static double Excange(Root forexData , string currency)
        {

            for(int i =0; i < forexData.conversion_rates.GetType().GetProperties().Length; i++)
            {
                if(forexData.conversion_rates.GetType().GetProperties().GetValue(i).ToString().Replace("Double ", "") == currency)
                {
                    double result = Convert.ToDouble(forexData.conversion_rates.GetType().GetProperty(forexData.conversion_rates.GetType().GetProperties().GetValue(i).ToString().Replace("Double ", "")).GetValue(forexData.conversion_rates));
                    return result;
                }
            }
            return 0.00;

        }
    }


    public class Treading
    {
        public static void sale(double price)
        {

        }
        public static void buy(double price)
        {

        }
    }
}
