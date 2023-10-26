using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Configuration;
using AdaTask.Class;
using AdaTask.Event;
using System.Diagnostics;

namespace AdaTask
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string tBchCode = "";
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                cSP ocSP = new cSP();
                new cLog().C_WRTxLog("Program", "Main : " + string.Format("Start : adaTask Ver.{0}", Application.ProductVersion.ToString()));
                Console.WriteLine(string.Format("Start : adaTask Ver.{0}", Application.ProductVersion.ToString()));

                ocSP.cSP_GETxSQLConnectionString();
                new cLog().C_WRTxLog("Program", "Main : " + "Load database config.");
                Console.WriteLine("Load database config.");

                ocSP.cSP_GETxRabbitMQConfig();
                new cLog().C_WRTxLog("Program", "Main : " + "Load rabbitMQ config.");
                Console.WriteLine("Load rabbitMQ config.");

                if (args == null)
                {
                    new cLog().C_WRTxLog("Program", "Main : " + "Adatask recive parameter invalid format.");
                    Console.WriteLine(string.Format("Adatask recive parameter invalid format."));
                }
                else
                {
                    if (args.Length > 0)
                    {
                        string[] aoAgrs = args;
                        string tProcess = aoAgrs[0];
                        string tParameter = aoAgrs[1];

                        if (args.Length == 3)
                        {
                            tBchCode = aoAgrs[2].Substring(1);
                        }
                        //string tProcess = "/4";
                        //string tParameter = "/*";
                        Console.WriteLine(string.Format("Adatask recive (Process:{0},Parameter:{1})", tProcess, tParameter));
                        new cLog().C_WRTxLog("Program", "Main : " + string.Format("Adatask recive (Process:{0},Parameter:{1})", tProcess, tParameter));
                        switch (tProcess)
                        {
                            case "/1": //[BOY][63-03-24]
                                if (tParameter == "/*")
                                {
                                    new cEvent1().C_PROxRequestTranferSAPByJSON();
                                }
                                break;
                            case "/2": //[BOY][63-03-24]
                                if (tParameter == "/*")
                                {
                                    new cEvent2().C_PROxRequestTranferSAPByJSON(tBchCode);
                                }
                                break;
                            case "/3": //[BOY][63-03-24]
                                if (tParameter == "/*")
                                {
                                    new cEvent3().C_PROxRequestTranferSAPByJSON(tBchCode);
                                }
                                break;
                            case "/4": //*Arm 63-07-06 Sendmail
                                if (tParameter == "/*")
                                {
                                    new cEvent4().C_PROxReqSendMailErrExpSale();
                                }
                                break;
                            case "/5": //*Zen เส้น 1 2 3
                                new cEvent5().C_IMPxDataBySKC(tParameter,tBchCode);
                                break;
                            default:
                                Console.WriteLine(string.Format("Adatask recive parameter invalid format."));
                                break;
                        }
                    }
                    else
                    {
                        new cLog().C_WRTxLog("Program", "Main : " + "Adatask recive parameter invalid format.");
                        Console.WriteLine(string.Format("Adatask recive parameter invalid format."));
                    }
                    //Console.ReadLine();
                    new cLog().C_WRTxLog("Program", "Main : " + string.Format("End : adaTask Ver.{0}", Application.ProductVersion.ToString()));
                    Console.WriteLine(string.Format("End : adaTask Ver.{0}", Application.ProductVersion.ToString()));
                    Process.GetCurrentProcess().Kill();
                }
            }
            catch (Exception oEx)
            {
                Console.WriteLine("Error: " + oEx.Message);
            }
        }
    }
}
