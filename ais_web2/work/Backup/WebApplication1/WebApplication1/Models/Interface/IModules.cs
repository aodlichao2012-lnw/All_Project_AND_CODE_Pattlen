using ais_web3.Models;
using Oracle.ManagedDataAccess.Client;
//using Oracle.DataAccess.Client;
//using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication1.Models.Interface
{
   public   interface IModules
    {
        object Caching_set(string key, object o);
        object Caching_Get(string key);


        [Obsolete]
        void Connectdb();


        //[Obsolete]
        // static void Connectdb2()
        //{

        //    // 'Dim strConnect As String = "Data Source=ORAIEC2;User ID=PREGOV;Password=RIUD6D;" 'base จริง
        //    WriteLog.instance.Log("Connectdb");
        //    // '  Dim strConnect As String = "Data Source=DTACIVR;User ID=YUMT;Password=PASSTB;"   '' Base สำรอง
        //    if (strDB == "Production")
        //    {

        //        strConn = " Data Source=ORAIEC2;User ID=PREGOV;Password=RIUD6D;Connection Timeout=500000;";
        //    }
        //    else if (strDB == "Backup")
        //    {
        //        strConn = " Data Source=SHINIVR3;User ID=PREGOV;Password=PASSTB;Connection Timeout=500000;";
        //    }
        //    else
        //    {
        //        strConn = " Data Source=ORAIEC2;User ID=PREGOV;Password=RIUD6D;Connection Timeout=500000;";
        //    }


        //    // Dim strConnect As String = "Data Source=ORAIEC2;User ID=PREGOW;Password=RIUD7D;"

        //    try
        //    {
        //        Conn2 = new OracleConnection(strConn);
        //        WriteLog.instance.Log("    Connect = new OracleConnection(strConn);");
        //        {

        //            if (Conn2.State == ConnectionState.Open)
        //                Conn2.Close();

        //            Conn2.Open();
        //            WriteLog.instance.Log("  Connect.Open();");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.instance.Log("ไม่สามารถบันทึกข้อมูลได้เนื่องจากปัญหาติดต่อฐานข้อมูล " + ex.Message + "ผลการตรวจสอบ");
        //    }
        //}

        [Obsolete]
        DataTable Comman(string sQL);

        [Obsolete]
        DataTable Comman_Static(string sQL, string[] input = null, string[] parameter = null);

        [Obsolete]
        DataTable Comman_Static_All(string sQL);
        [Obsolete]
          Task<DataTable> Comman_Static2(string sQL, string[] input = null, string[] parameter = null);

        [Obsolete]
        DataTable Commandt(string sQL);

        [Obsolete]
         Task<OracleDataReader> CommanDataread(string sQL, string[] input = null, string[] parameter = null);

        [Obsolete]
        OracleDataAdapter CommanDataApt(string sQL);

        [Obsolete]
        int CommanEx(string sQL, string[] input = null, string[] parameter = null);

        [Obsolete]
        DataSet CommandSet(string sQL, string table, string[] input = null, string[] parameter = null);

        List<string> GetFromToken(string jwt);

        [Obsolete]
         DataTable Comman_Static3(string sQL);
    }
}
