using AdaPos.Forms;
using AdaPos.Models.Other;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cPdtPmt
    {
        private cSP oW_SP = new cSP();
        public DataTable oDbTblCoCal;
        public string C_GETtNamePmt(string ptPmtCode)
        {
            StringBuilder oSql;
            string tName = "";
            List<string> aName;
            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTPmhName");
                oSql.AppendLine("FROM TPMTPmtHD_L");
                oSql.AppendLine("WHERE FTBchCode = '"+cVB.tVB_BchCode+"' AND FTPmhDocNo = '"+ ptPmtCode + "' AND FNLngID = " + cVB.nVB_Language);

                aName = new cDatabase().C_GETaDataQuery<string>(oSql.ToString());
                tName = aName[0].Trim();
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("cPdtPmt", "C_GETtNamePmt : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                new cSP().SP_CLExMemory();
            }
            return tName;
        }

        public void C_PRCxPrepareDT(string ptTblName, string ptDocNo)
        {

            cDatabase oDatabase;
            int nResult = -1;
            try
            {
                oDatabase = new cDatabase();

                using (SqlConnection conn = oDatabase.C_CONoDatabase(cVB.oVB_Config))
                using (SqlCommand cmd = new SqlCommand("STP_PRCxPrepareDTPmt", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // set up the parameters
                    cmd.Parameters.Add("@ptTblName", SqlDbType.VarChar, 30);
                    cmd.Parameters.Add("@ptDocNo", SqlDbType.VarChar, 30);
                    cmd.Parameters.Add("@FNResult", SqlDbType.Int).Direction = ParameterDirection.Output;

                    // set parameter values
                    cmd.Parameters["@ptTblName"].Value = ptTblName;
                    cmd.Parameters["@ptDocNo"].Value = ptDocNo;

                    // open connection and execute stored procedure

                    cmd.ExecuteNonQuery();

                    // read output value from @FCResult

                    nResult = Convert.ToInt32(cmd.Parameters["@FNResult"].Value);
                    conn.Close();
                }
                //if (nResult == -1)
                //{
                //    new cLog().C_WRTxLog("cPdtPmt", "C_PRCxPrepareDT : STP_PRCxPrepareDTPmt Result = " + nResult);
                //}
                //else if (nResult == 0)
                //{
                //    new cLog().C_WRTxLog("cPdtPmt", "C_PRCxPrepareDT : STP_PRCxPrepareDTPmt Result = " + nResult);
                //}
                //+++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdtPmt", "C_PRCxPrepareDT : " + oEx.Message); }
            finally
            {
                oDatabase = null;
                new cSP().SP_CLExMemory();
            }

        }

        
        public DataTable C_CONoFill<T>(IEnumerable<T> Ts) where T : class
        {
            //Get Enumerable Type
            Type tT = typeof(T);
            DataTable dt = new DataTable();
            //Get Collection of NoVirtual properties
            var T_props = tT.GetProperties().Where(p => !p.GetGetMethod().IsVirtual).ToArray();

            //Fill Schema
            foreach (PropertyInfo p in T_props)
                dt.Columns.Add(p.Name, p.GetMethod.ReturnParameter.ParameterType.BaseType);

            //Fill Data
            foreach (T t in Ts)
            {
                DataRow row = dt.NewRow();

                foreach (PropertyInfo p in T_props)
                    row[p.Name] = p.GetValue(t);

                dt.Rows.Add(row);
            }
            return dt;

        }
        public bool C_PRCoCalPmt(string ptStaPmtPrice)
        {

            //StringBuilder oSql;
            SqlParameter[] aoSqlParam;
            //DataTable oDbTblPdt;
            DataSet aoDSPmt;    //*Em 63-04-16
            bool bStaPrc = false;
            cVB.oVB_GetPmt = new DataTable();
            cVB.oVB_PmtSug = new DataTable();   //*Em 63-04-16
            try
            {
                aoSqlParam = new SqlParameter[] {
                    new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 25){ Value = cVB.tVB_BchCode},
                    new SqlParameter ("@ptDocNo", SqlDbType.VarChar, 25){ Value = cVB.tVB_DocNo},
                    new SqlParameter ("@ptCstCode", SqlDbType.VarChar, 25){ Value = cVB.tVB_CstCode},
                    new SqlParameter ("@ptPplCodeCst", SqlDbType.VarChar, 25){ Value = cVB.tVB_PriceGroup},
                    new SqlParameter ("@ptStaPmtPrice", SqlDbType.VarChar, 1){ Value = ptStaPmtPrice},     //1:เฉพาะโปรกลุ่มราคา 2:ไม่ใช่โปรกลุ่มราคา
                    new SqlParameter ("@FNResult", SqlDbType.Int) {
                        Direction = ParameterDirection.Output }
                };
                //oDbTblPdt = new DataTable();
                //bStaPrc = new cDatabase().C_DATbExecuteQuerySto/*r*/eProcedure(cVB.oVB_Config, "STP_PRCoCalPmt", ref aoSqlParam, ref oDbTblPdt);

                //if (bStaPrc)
                //{
                //    if (oDbTblPdt.Rows.Count > 0)
                //    {

                //        //oDbTblPdt.DefaultView.ToTable(true, "FTPmhCode", "FCXpdDis", "FCXpdGetQtyDiv", "FCXpdPoint");
                //        cVB.oVB_GetPmt = oDbTblPdt;
                //        bStaPrc = true;
                //    }
                //    else
                //    {
                //        bStaPrc = false;
                //    }

                //    //else new wPayment(3).Show();
                //}

                //*Em 63-04-16
                aoDSPmt = new DataSet();
                bStaPrc = new cDatabase().C_DATbExecuteQueryStoreProcedure(cVB.oVB_Config, "STP_PRCoCalPmt", ref aoSqlParam, ref aoDSPmt);
                if (bStaPrc)
                {
                    cVB.oVB_GetPmt = aoDSPmt.Tables[0];
                    cVB.oVB_PmtSug = aoDSPmt.Tables[1];
                    bStaPrc = true;
                }
                else
                {
                    bStaPrc = false;
                }
                //++++++++++++++++
                return bStaPrc;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdtPmt", "C_PRCoCalPmt : " + oEx.Message); return bStaPrc; }
            finally
            {
                aoSqlParam = null;
                aoDSPmt = null;
                new cSP().SP_CLExMemory();
            }
        }

        public bool C_PRCbCalPmtSuggest()
        {

            StringBuilder oSql;
            SqlParameter[] aoSqlParam;
            DataTable oDbTblPdt;
            bool bStaPrc = false;
            cVB.oVB_PmtSug = new DataTable();
            try
            {
                aoSqlParam = new SqlParameter[] {
                    new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 25){ Value = cVB.tVB_BchCode},
                    new SqlParameter ("@ptDocNo", SqlDbType.VarChar, 25){ Value = cVB.tVB_DocNo},
                    new SqlParameter ("@ptCstCode", SqlDbType.VarChar, 25){ Value = cVB.tVB_CstCode},
                    new SqlParameter ("@ptPplCodeCst", SqlDbType.VarChar, 25){ Value = cVB.tVB_PriceGroup},
                    new SqlParameter ("@FNResult", SqlDbType.Int) {
                        Direction = ParameterDirection.Output }
                };
                oDbTblPdt = new DataTable();

                bStaPrc = new cDatabase().C_DATbExecuteQueryStoreProcedure(cVB.oVB_Config, "STP_PRCoCalPmtSuggest", ref aoSqlParam, ref oDbTblPdt);
                if (bStaPrc)
                {
                    if (oDbTblPdt.Rows.Count > 0)
                    {
                        cVB.oVB_PmtSug = oDbTblPdt;
                        bStaPrc = true;
                    }
                    else
                    {
                        bStaPrc = false;
                    }
                }

                return bStaPrc;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdtPmt", "C_PRCbCalPmtSuggest : " + oEx.Message); return bStaPrc; }
        }

        public void C_PRCbINSPmtPD(DataTable poDbTbl)
        {
            bool bStatus = false;
            StringBuilder oSql = new StringBuilder();
            cDatabase oDatabase = new cDatabase();
            double dPoint = 0;
            try
            {
                if (poDbTbl.Rows.Count > 0)
                {
                    oSql.AppendLine("DELETE FROM " + cSale.tC_TblSalPD + " WITH(ROWLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + poDbTbl.Rows[0]["FTBchCode"] + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + poDbTbl.Rows[0]["FTXshDocNo"] + "'");
                    if (cVB.oVB_Sale.bW_CalPmtPrice ==  false && cVB.bVB_PriceConfirm == true) oSql.AppendLine("AND FTXpdGetType <> '4' "); //*Em 63-05-05

                    foreach (DataRow oRow in poDbTbl.Rows)
                    {
                        if (!string.IsNullOrEmpty(oRow["FCXpdPoint"].ToString()) == true) { dPoint = Convert.ToDouble(oRow["FCXpdPoint"].ToString()); }
                        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalPD + " WITH(ROWLOCK) (");
                        oSql.AppendLine("FTBchCode ,FTXshDocNo ,FTPmhDocNo ,FNXsdSeqNo ");
                        oSql.AppendLine(",FTPmdGrpName,FTPdtCode,FTPunCode,FCXsdQty ");
                        oSql.AppendLine(",FCXsdQtyAll,FCXsdSetPrice ,FCXsdNet ,FCXpdGetQtyDiv ");
                        oSql.AppendLine(",FTXpdGetType,FCXpdGetValue,FCXpdDis,FCXpdPerDisAvg ");
                        oSql.AppendLine(",FCXpdDisAvg,FCXpdPoint,FTXpdStaRcv,FTPplCode,FTXpdCpnText,FTCpdBarCpn)");
                        oSql.AppendLine("VALUES ( ");
                        oSql.AppendLine("'" + oRow["FTBchCode"] + "','" + oRow["FTXshDocNo"] + "','" + oRow["FTPmhCode"] + "','" + oRow["FNSdtSeqNo"] + "' ");
                        oSql.AppendLine(",'" + oRow["FTPmdGrpName"] + "','" + oRow["FTPdtCode"] + "','" + oRow["FTPunCode"] + "'," + oRow["FCXsdQty"] + " ");
                        oSql.AppendLine("," + oRow["FCXsdQtyAll"] + "," + oRow["FCXsdSetPrice"] + "," + oRow["FCXsdNet"] + "," + C_GETnConvertInt(oRow["FCXpdGetQtyDiv"].ToString()) + " ");
                        oSql.AppendLine(",'" + oRow["FTXpdGetType"] + "'," + C_GETnConvertInt(oRow["FCXpdGetValue"].ToString()) + "," + C_GETnConvertInt(oRow["FCXpdDis"].ToString()) + "," + C_GETnConvertInt(oRow["FCXdtPerDisAvg"].ToString()) + " ");
                        oSql.AppendLine("," + C_GETnConvertInt(oRow["FCXdtDisAvg"].ToString()) + "," + dPoint + ",'" + oRow["FTXpdStaRcv"] + "','" + oRow["FTPplCode"] + "' ");
                        oSql.AppendLine(",'" + oRow["FTPgtCpnText"] + "','" + oRow["FTCpdBarCpn"] + "' ");
                        oSql.AppendLine(") ");
                    }
                    oDatabase.C_SETxDataQuery(oSql.ToString());
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdtPmt", "C_PRCbINSPmtPD : " + oEx.Message); }
            finally
            {
                oSql = null;
                oDatabase = null;
                new cSP().SP_CLExMemory();
            }
           // return bStatus;
        }

        public decimal C_GETnConvertInt(string ptString)
        {
            decimal nInt = 0;
            if (!string.IsNullOrEmpty(ptString))
            {
                nInt = Convert.ToDecimal(ptString);
            }
            return nInt;
        }

        public void C_PRCxPmtDisProratePD()
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDatabase = new cDatabase();
            DataTable oDbTblPD = new DataTable();
            DataTable oDbTblDT = new DataTable();
            string tSign = "";
            try
            {

                oSql.AppendLine("SELECT * " + " FROM  " + cSale.tC_TblSalPD + " WITH(NOLOCK) ");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FCXpdDisAvg <> 0");
                oSql.AppendLine("AND FTXpdGetType <> '4'"); //*Em 63-05-06

                oDbTblPD = oDatabase.C_GEToDataQuery(oSql.ToString());



                if (oDbTblPD.Rows.Count > 0)
                {
                    for (int nCount = 0; nCount < oDbTblPD.Rows.Count; nCount++)
                    {

                        oSql.Clear();
                        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + " WITH(ROWLOCK)");
                        oSql.AppendLine("(");
                        oSql.AppendLine("  FTBchCode,FTXshDocNo,FNXsdSeqNo");
                        oSql.AppendLine(" ,FDXddDateIns,FNXddStaDis,FTXddDisChgTxt");
                        oSql.AppendLine(" ,FTXddDisChgType,FCXddNet,FCXddValue");
                        oSql.AppendLine(" ,FTXddRefCode"); //*Arm 63-03-12 เลขที่อ้างอิง Redeem
                        oSql.AppendLine(")");
                        //oSql.AppendLine("SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,GETDATE(),'1',");
                        oSql.AppendLine("SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,GETDATE(),'2',");  //*Em 63-04-14  ให้ลงเป็นท้ายบิล
                        oSql.AppendLine("CASE WHEN '" + oDbTblPD.Rows[nCount]["FTXpdGetType"] + "' = '2' THEN '" + oDbTblPD.Rows[nCount]["FCXpdGetValue"] + "'+'%' ELSE '" + oDbTblPD.Rows[nCount]["FCXpdDisAvg"] + "' END,");
                        oSql.AppendLine("'1', FCXsdNetAfHD,'" + oDbTblPD.Rows[nCount]["FCXpdDisAvg"] + "','" + oDbTblPD.Rows[nCount]["FTPmhDocNo"] + "'");
                        oSql.AppendLine("FROM " + cSale.tC_TblSalDT + "");
                        oSql.AppendLine("WHERE FTXshDocNo='" + cVB.tVB_DocNo + "' AND FNXsdSeqNo=" + oDbTblPD.Rows[nCount]["FNXsdSeqNo"] + " AND FTBchCode='" + cVB.tVB_BchCode + "'");
                        //oSql.AppendLine("VALUES");
                        //oSql.AppendLine("(");
                        //oSql.AppendLine("  '" + cVB.tVB_BchCode + "','" + cVB.tVB_DocNo + "'," + oDbTblPD.Rows[nCount]["FNXsdSeqNo"] + "");
                        //oSql.AppendLine(" ,GETDATE(),2,'" + oDbTblPD.Rows[nCount]["FCXpdGetValue"] + "'");
                        //oSql.AppendLine(" ,'" + oDbTblPD.Rows[nCount]["FTXpdGetType"] + "','" + oDbTblPD.Rows[nCount]["FCXsdNet"] + "','" + oDbTblPD.Rows[nCount]["FCXpdDisAvg"] + "'");
                        //oSql.AppendLine(" ,'" + oDbTblPD.Rows[nCount]["FTPmhDocNo"] + "'");
                        //oSql.AppendLine(")");

                        //*Em 63-04-17
                        // Set NetAfHD เลยกรณีมีแค่ 1 Row
                        oSql.AppendLine("");
                        oSql.AppendLine("UPDATE " + cSale.tC_TblSalDT + " WITH(ROWLOCK) SET ");
                        oSql.AppendLine("FCXsdNetAfHD=FCXsdNetAfHD - " + oDbTblPD.Rows[nCount]["FCXpdDisAvg"]);
                        oSql.AppendLine("WHERE FTXshDocNo='" + cVB.tVB_DocNo + "' AND FNXsdSeqNo=" + oDbTblPD.Rows[nCount]["FNXsdSeqNo"] + " AND FTBchCode='" + cVB.tVB_BchCode + "'");
                        //++++++++++++++++++++++++++++++++
                        oDatabase.C_SETxDataQuery(oSql.ToString());


                        //// Set NetAfHD เลยกรณีมีแค่ 1 Row
                        //oSql.Clear();
                        //oSql.AppendLine("UPDATE " + cSale.tC_TblSalDT + " WITH(ROWLOCK) SET ");
                        //oSql.AppendLine("FCXsdNetAfHD=FCXsdNetAfHD - " + oDbTblPD.Rows[nCount]["FCXpdDisAvg"]);
                        //oSql.AppendLine("WHERE FTXshDocNo='" + cVB.tVB_DocNo + "' AND FNXsdSeqNo=" + oDbTblPD.Rows[nCount]["FNXsdSeqNo"] + " AND FTBchCode='" + cVB.tVB_BchCode + "'");
                        //oDatabase.C_SETxDataQuery(oSql.ToString());
                    }
                    cSale.C_DATxUpdVat();
                    //cSale.C_DATxUpdCost();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdtPmt", "C_PRCxPmtDisProratePD : " + oEx.Message); }
            finally
            {
                oSql = null;
                oDatabase = null;
                oDbTblPD = null;
                oDbTblDT = null;
                new cSP().SP_CLExMemory();
            }
            
        }

        public void C_PRCxPdtPmt()
        {

            cDatabase oDatabase;
            int nResult = -1;
            try
            {
                oDatabase = new cDatabase();

                using (SqlConnection conn = oDatabase.C_CONoDatabase(cVB.oVB_Config))
                using (SqlCommand cmd = new SqlCommand("STP_PRCxPreparePmt", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // set up the parameters
                    cmd.Parameters.Add("@ptBchCode", SqlDbType.VarChar, 30);
                    cmd.Parameters.Add("@FNResult", SqlDbType.Int).Direction = ParameterDirection.Output;

                    // set parameter values
                    cmd.Parameters["@ptBchCode"].Value = cVB.tVB_BchCode;

                    // open connection and execute stored procedure

                    cmd.ExecuteNonQuery();

                    // read output value from @FCResult

                    nResult = Convert.ToInt32(cmd.Parameters["@FNResult"].Value);
                    conn.Close();
                }
                if (nResult == -1)
                {
                    new cLog().C_WRTxLog("wSale", "W_PRCxPdtPmt : STP_PRCxPreparePmt Result = " + nResult);
                }
                else if (nResult == 0)
                {
                    new cLog().C_WRTxLog("wSale", "W_PRCxPdtPmt : STP_PRCxPreparePmt Result = " + nResult);
                }
                //+++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_PRCxPdtPmt : " + oEx.Message); }
            finally
            {
                oDatabase = null;
                new cSP().SP_CLExMemory();
            }
        }

        public void C_PRCxPdtPrice()
        {
            cDatabase oDatabase;
            int nResult = -1;
            try
            {
                oDatabase = new cDatabase();

                using (SqlConnection conn = oDatabase.C_CONoDatabase(cVB.oVB_Config))
                using (SqlCommand cmd = new SqlCommand("STP_PRCxPdtPrice", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // set up the parameters
                    cmd.Parameters.Add("@pdSaleDate", SqlDbType.DateTime);
                    cmd.Parameters.Add("@FNResult", SqlDbType.Int).Direction = ParameterDirection.Output;

                    // set parameter values
                    //cmd.Parameters["@pdSaleDate"].Value = cVB.tVB_SaleDate;
                    //*Net 63-04-03 ส่งเวลาไปเช็คด้วย
                    cmd.Parameters["@pdSaleDate"].Value = cVB.tVB_SaleDate + ' ' + DateTime.Now.ToString("hh:mm:ss");

                    // open connection and execute stored procedure

                    cmd.ExecuteNonQuery();

                    // read output value from @FCResult

                    nResult = Convert.ToInt32(cmd.Parameters["@FNResult"].Value);
                    conn.Close();
                }
                if (nResult == -1)
                {
                    new cLog().C_WRTxLog("wSale", "W_PRCxPdtPrice : STP_PRCxPdtPrice Result = " + nResult);
                }
                else if (nResult == 0)
                {
                    new cLog().C_WRTxLog("wSale", "W_PRCxPdtPrice : STP_PRCxPdtPrice Result = " + nResult);
                }
                //+++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_PRCxPdtPmt : " + oEx.Message); }
            finally
            {
                oDatabase = null;
                new cSP().SP_CLExMemory();
            }
        }

       
    }
}
