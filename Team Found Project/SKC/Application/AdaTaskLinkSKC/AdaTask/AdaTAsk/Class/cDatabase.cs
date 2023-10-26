using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using AdaTask.Class;
using AdaTask.Event;

namespace AdaTask.Class
{
    public class cDatabase
    {
        public int C_SQL_Execute(string ptSql)
        {
            try
            {
                SqlConnection oCon = new SqlConnection(cVB.tVB_SQLCon);
                oCon.Open();
                SqlCommand cmd = new SqlCommand(ptSql, oCon);
                int nGet = cmd.ExecuteNonQuery();
                oCon.Close();
                return nGet;
            }
            catch (Exception oEx)
            {
                Console.WriteLine("cDatabase > C_SQL_Execute : " + oEx.ToString());
                new cLog().C_WRTxLog("cDatabase", "C_SQL_Execute: " + oEx.ToString());
                return 0;
            }
        }

        public DataTable C_SQL_Query(string ptSQL)
        {
            DataTable oDt = new DataTable();
            try
            {
                using (SqlConnection oConnection = new SqlConnection(cVB.tVB_SQLCon))
                {
                    SqlCommand oCommand = new SqlCommand(ptSQL, oConnection);
                    oConnection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(oCommand);
                    da.Fill(oDt);
                    oConnection.Close();
                    da.Dispose();
                }
                return oDt;
            }
            catch (Exception oEx)
            {
                Console.WriteLine("cDatabase > C_SQL_Query : " + oEx.Message);
                return null;
            }
        }
    }
}
