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

        public void C_CLRxPmtPara()
        {
            if (cVB.oVB_GetPmt != null)
            {
                cVB.oVB_GetPmt.Clear();
                cVB.oVB_GetPmt.Dispose();
                cVB.oVB_GetPmt = null;
            }
            if (cVB.oVB_PmtSug != null)
            {
                cVB.oVB_PmtSug.Clear();
                cVB.oVB_PmtSug.Dispose();
                cVB.oVB_PmtSug = null;
            }

            cVB.oVB_GetPmt = new DataTable();
            cVB.oVB_PmtSug = new DataTable(); 
        }
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
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return tName;
        }

        public void C_PRCxPrepareDT(string ptTblName, string ptDocNo)
        {
            new cLog().C_WRTxLog("cPdtPmt", "C_PRCxPrepareDT : Start STP_PRCxPrepareDTPmt", cVB.bVB_AlwPrnLog);
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
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            new cLog().C_WRTxLog("cPdtPmt", "C_PRCxPrepareDT : End STP_PRCxPrepareDTPmt", cVB.bVB_AlwPrnLog);
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
            new cLog().C_WRTxLog("cPdtPmt", "C_PRCoCalPmt : Start STP_PRCoCalPmt", cVB.bVB_AlwPrnLog);
            //StringBuilder oSql;
            SqlParameter[] aoSqlParam;
            //DataTable oDbTblPdt;
            DataSet aoDSPmt;    //*Em 63-04-16
            bool bStaPrc = false;
            //cVB.oVB_GetPmt = new DataTable();
            //cVB.oVB_PmtSug = new DataTable();   //*Em 63-04-16
            C_CLRxPmtPara(); //*Net 63-06-02
            try
            {
                aoSqlParam = new SqlParameter[] {
                    new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 25){ Value = cVB.tVB_BchCode},
                    new SqlParameter ("@ptDocNo", SqlDbType.VarChar, 25){ Value = cVB.tVB_DocNo},
                    //*Net 63-07-30 ปรับตาม Moshi
                    //new SqlParameter ("@ptCstCode", SqlDbType.VarChar, 25){ Value = cVB.tVB_CstCode},
                    //new SqlParameter ("@ptPplCodeCst", SqlDbType.VarChar, 25){ Value = cVB.tVB_PriceGroup},
                    //new SqlParameter ("@ptCstCode", SqlDbType.VarChar, 25){ Value = String.IsNullOrEmpty(cVB.tVB_CstCode)?"":cVB.tVB_CstCode}, //*Net 63-07-07 ป้องกันค่าเป็น null
                    new SqlParameter ("@ptCstCode", SqlDbType.VarChar, 25){ Value = String.IsNullOrEmpty(cVB.tVB_MemCode)?"":cVB.tVB_MemCode},      //*Em 63-08-11
                    new SqlParameter ("@ptPplCodeCst", SqlDbType.VarChar, 25){ Value = String.IsNullOrEmpty(cVB.tVB_PriceGroup)?"":cVB.tVB_PriceGroup}, //*Net 63-07-07 ป้องกันค่าเป็น null
                    //+++++++++++++++++++++++++++++++++++++
                    new SqlParameter ("@ptStaPmtPrice", SqlDbType.VarChar, 1){ Value = ptStaPmtPrice},     //1:เฉพาะโปรกลุ่มราคา 2:ไม่ใช่โปรกลุ่มราคา
                    new SqlParameter ("@ptSysFmt",SqlDbType.VarChar,5){ Value = string.IsNullOrEmpty(cVB.tVB_ApiCstSch_Fmt)?"":cVB.tVB_ApiCstSch_Fmt},  //*Em 63-08-20
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
                new cLog().C_WRTxLog("cPdtPmt", "C_PRCoCalPmt : End STP_PRCoCalPmt", cVB.bVB_AlwPrnLog);
                //++++++++++++++++
                return bStaPrc;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdtPmt", "C_PRCoCalPmt : " + oEx.Message); return bStaPrc; }
            finally
            {
                aoSqlParam = null;
                aoDSPmt = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public bool C_PRCbCalPmtSuggest()
        {
            new cLog().C_WRTxLog("cPdtPmt", "C_PRCbCalPmtSuggest : Start STP_PRCoCalPmtSuggest", cVB.bVB_AlwPrnLog);
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
                new cLog().C_WRTxLog("cPdtPmt", "C_PRCbCalPmtSuggest : End STP_PRCoCalPmtSuggest", cVB.bVB_AlwPrnLog);
                return bStaPrc;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdtPmt", "C_PRCbCalPmtSuggest : " + oEx.Message); return bStaPrc; }
        }

        public void C_PRCbINSPmtPD(DataTable poDbTbl)
        {
            new cLog().C_WRTxLog("cPdtPmt", "C_PRCbINSPmtPD : Start Insert Data", cVB.bVB_AlwPrnLog);
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
                        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalPD + " (");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
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
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            new cLog().C_WRTxLog("cPdtPmt", "C_PRCbINSPmtPD : End Insert Data", cVB.bVB_AlwPrnLog);
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
            new cLog().C_WRTxLog("cPdtPmt", "C_PRCxPmtDisProratePD : Start Insert Data", cVB.bVB_AlwPrnLog);
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
                        //*Em 63-08-20
                        string tDisCode = "";
                        bool bChg = false;  //*Em 63-08-26
                        DataRow oRow =  cVB.oVB_GetPmt.Select("FTPmhCode = '"+ oDbTblPD.Rows[nCount]["FTPmhDocNo"] + "' AND FNSdtSeqNo=" + oDbTblPD.Rows[nCount]["FNXsdSeqNo"]).FirstOrDefault();
                        if (oRow != null)
                        {
                            tDisCode = oRow.Field<string>("FTDisCode");
                        }
                        //+++++++++++++

                        oSql.Clear();
                        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + " ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                        oSql.AppendLine("(");
                        oSql.AppendLine("  FTBchCode,FTXshDocNo,FNXsdSeqNo");
                        oSql.AppendLine(" ,FDXddDateIns,FNXddStaDis,FTXddDisChgTxt");
                        oSql.AppendLine(" ,FTXddDisChgType,FCXddNet,FCXddValue");
                        oSql.AppendLine(" ,FTXddRefCode"); //*Arm 63-03-12 เลขที่อ้างอิง Redeem
                        oSql.AppendLine(" ,FTRsnCode,FTDisCode");    //*Em 63-06-21
                        oSql.AppendLine(")");
                        //oSql.AppendLine("SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,GETDATE(),'1',");
                        //oSql.AppendLine("SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,GETDATE(),'2',");  //*Em 63-04-14  ให้ลงเป็นท้ายบิล
                        oSql.AppendLine("SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,GETDATE(),'0',");  //*Em 63-06-21
                        //oSql.AppendLine("CASE WHEN '" + oDbTblPD.Rows[nCount]["FTXpdGetType"] + "' = '2' THEN '" + oDbTblPD.Rows[nCount]["FCXpdGetValue"] + "'+'%' ELSE '" + oDbTblPD.Rows[nCount]["FCXpdDisAvg"] + "' END,");
                        //oSql.AppendLine("'1', FCXsdNetAfHD,'" + oDbTblPD.Rows[nCount]["FCXpdDisAvg"] + "','" + oDbTblPD.Rows[nCount]["FTPmhDocNo"] + "',");

                        //*Em 63-08-26
                        switch (oDbTblPD.Rows[nCount]["FTXpdGetType"].ToString())
                        {
                            case "1":
                                if (Convert.ToDecimal(oDbTblPD.Rows[nCount]["FCXpdDisAvg"]) < Convert.ToDecimal(0))
                                {
                                    oSql.AppendLine(" '" + oW_SP.SP_SETtDecShwSve(1, Math.Abs(Convert.ToDecimal(oDbTblPD.Rows[nCount]["FCXpdDisAvg"])), cVB.nVB_DecShow) + "',");
                                    oSql.AppendLine("'3', FCXsdNetAfHD,'" + Math.Abs(Convert.ToDecimal(oDbTblPD.Rows[nCount]["FCXpdDisAvg"])) + "','" + oDbTblPD.Rows[nCount]["FTPmhDocNo"] + "'");
                                    bChg = true;
                                }
                                else
                                {
                                    oSql.AppendLine(" '" + oW_SP.SP_SETtDecShwSve(1, Math.Abs(Convert.ToDecimal(oDbTblPD.Rows[nCount]["FCXpdDisAvg"])), cVB.nVB_DecShow) + "',");
                                    oSql.AppendLine("'1', FCXsdNetAfHD,'" + Math.Abs(Convert.ToDecimal(oDbTblPD.Rows[nCount]["FCXpdDisAvg"])) + "','" + oDbTblPD.Rows[nCount]["FTPmhDocNo"] + "'");
                                }
                                break;

                            case "2":
                                oSql.AppendLine(" '" + oW_SP.SP_SETtDecShwSve(1, Math.Abs(Convert.ToDecimal(oDbTblPD.Rows[nCount]["FCXpdGetValue"])), cVB.nVB_DecShow) + "'+'%',");
                                oSql.AppendLine("'2', FCXsdNetAfHD,'" + Math.Abs(Convert.ToDecimal(oDbTblPD.Rows[nCount]["FCXpdDisAvg"])) + "','" + oDbTblPD.Rows[nCount]["FTPmhDocNo"] + "'");
                                break;

                            case "3":
                                if (Convert.ToDecimal(oDbTblPD.Rows[nCount]["FCXpdDisAvg"]) < Convert.ToDecimal(0))
                                {
                                    oSql.AppendLine(" '" + oW_SP.SP_SETtDecShwSve(1, Math.Abs(Convert.ToDecimal(oDbTblPD.Rows[nCount]["FCXpdDisAvg"])), cVB.nVB_DecShow) + "',");
                                    oSql.AppendLine("'3', FCXsdNetAfHD,'" + Math.Abs(Convert.ToDecimal(oDbTblPD.Rows[nCount]["FCXpdDisAvg"])) + "','" + oDbTblPD.Rows[nCount]["FTPmhDocNo"] + "'");
                                    bChg = true;
                                }
                                else
                                {
                                    oSql.AppendLine(" '" + oW_SP.SP_SETtDecShwSve(1, Math.Abs(Convert.ToDecimal(oDbTblPD.Rows[nCount]["FCXpdDisAvg"])), cVB.nVB_DecShow) + "',");
                                    oSql.AppendLine("'1', FCXsdNetAfHD,'" + Math.Abs(Convert.ToDecimal(oDbTblPD.Rows[nCount]["FCXpdDisAvg"])) + "','" + oDbTblPD.Rows[nCount]["FTPmhDocNo"] + "'");
                                }
                                break;

                            default:
                                oSql.AppendLine(" '" + oW_SP.SP_SETtDecShwSve(1, Math.Abs(Convert.ToDecimal(oDbTblPD.Rows[nCount]["FCXpdDisAvg"])), cVB.nVB_DecShow) + "',");
                                oSql.AppendLine("'1', FCXsdNetAfHD,'" + Math.Abs(Convert.ToDecimal(oDbTblPD.Rows[nCount]["FCXpdDisAvg"])) + "','" + oDbTblPD.Rows[nCount]["FTPmhDocNo"] + "'");
                                break;
                        }
                        //++++++++++++++++

                        //oSql.AppendLine("'' AS FTRsnCode,'' AS FTDisCode"); //*Em 63-06-21
                        oSql.AppendLine(",'' AS FTRsnCode,'" + tDisCode + "' AS FTDisCode"); //*Em 63-08-20
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
                        //oSql.AppendLine("FCXsdNetAfHD=FCXsdNetAfHD - " + oDbTblPD.Rows[nCount]["FCXpdDisAvg"]);

                        //*Em 63-08-26
                        if (bChg)
                        {
                            oSql.AppendLine("FCXsdNetAfHD=FCXsdNetAfHD + (" + Math.Abs(Convert.ToDecimal(oDbTblPD.Rows[nCount]["FCXpdDisAvg"])) + ")");
                        }
                        else
                        {
                            oSql.AppendLine("FCXsdNetAfHD=FCXsdNetAfHD - (" + oDbTblPD.Rows[nCount]["FCXpdDisAvg"] + ")");
                        }
                        //++++++++++++++++++

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
                    //cSale.C_DATxUpdVat(); //*Net 63-07-30 ปรับตาม Moshi
                    new cLog().C_WRTxLog("cPdtPmt", "C_PRCxPmtDisProratePD : End Insert Data", cVB.bVB_AlwPrnLog);
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
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

        }

        public void C_PRCxPdtPmt()
        {
            new cLog().C_WRTxLog("cPdtPmt", "C_PRCxPdtPmt : Start STP_PRCxPreparePmt", cVB.bVB_AlwPrnLog);
            cDatabase oDatabase;
            int nResult = -1;

            StringBuilder oSql; //*Arm 63-05-27

            try
            {
                oDatabase = new cDatabase();

                using (SqlConnection conn = oDatabase.C_CONoDatabase(cVB.oVB_Config))
                using (SqlCommand cmd = new SqlCommand("STP_PRCxPreparePmt", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 90;    //*Em 63-06-02

                    // set up the parameters
                    cmd.Parameters.Add("@ptBchCode", SqlDbType.VarChar, 30);
                    cmd.Parameters.Add("@ptAlwPmtDisAvg", SqlDbType.VarChar, 1);    //*Em 63-08-27
                    cmd.Parameters.Add("@FNResult", SqlDbType.Int).Direction = ParameterDirection.Output;

                    // set parameter values
                    cmd.Parameters["@ptBchCode"].Value = cVB.tVB_BchCode;
                    cmd.Parameters["@ptAlwPmtDisAvg"].Value = cVB.bVB_AlwPmtDisAvg ? "1" : "2"; //*Em 63-08-27

                    // open connection and execute stored procedure

                    cmd.ExecuteNonQuery();

                    // read output value from @FCResult

                    nResult = Convert.ToInt32(cmd.Parameters["@FNResult"].Value);
                    conn.Close();
                }
                if (nResult == -1)
                {
                    new cLog().C_WRTxLog("cPdtPmt", "C_PRCxPdtPmt : STP_PRCxPreparePmt Result = " + nResult, cVB.bVB_AlwPrnLog);
                }
                else if (nResult == 0)
                {
                    new cLog().C_WRTxLog("cPdtPmt", "C_PRCxPdtPmt : STP_PRCxPreparePmt Result = " + nResult, cVB.bVB_AlwPrnLog);
                }

                //*Arm 63-05-27 
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT COUNT(FTPgtStaGetType) FROM TPMTPmt with(nolock) WHERE FTPgtStaGetType = '4'");
                if (oDatabase.C_GEToDataQuery<int>(oSql.ToString()) > 0)
                {
                    cVB.bVB_PmtPriGrp = true;
                }
                else
                {
                    cVB.bVB_PmtPriGrp = false;
                }

                oSql.Clear();
                oSql.AppendLine("SELECT COUNT(FTPgtStaGetType) FROM TPMTPmt with(nolock) WHERE FTPgtStaGetType != '4'");
                if (oDatabase.C_GEToDataQuery<int>(oSql.ToString()) > 0)
                {
                    cVB.bVB_PmtDis = true;
                }
                else
                {
                    cVB.bVB_PmtDis = false;
                }
                //+++++++++++++

                new cLog().C_WRTxLog("cPdtPmt", "C_PRCxPdtPmt : End STP_PRCxPreparePmt", cVB.bVB_AlwPrnLog);
                //+++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdtPmt", "W_PRCxPdtPmt : " + oEx.Message); }
            finally
            {
                oSql = null;
                oDatabase = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public void C_PRCxPdtPrice()
        {
            new cLog().C_WRTxLog("cPdtPmt", "C_PRCxPdtPrice : Start STP_PRCxPdtPrice", cVB.bVB_AlwPrnLog);
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
                    cmd.Parameters.Add("@pdSaleDate", SqlDbType.Date);
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
                    new cLog().C_WRTxLog("cPdtPmt", "C_PRCxPdtPrice : STP_PRCxPdtPrice Result = " + nResult, cVB.bVB_AlwPrnLog);
                }
                else if (nResult == 0)
                {
                    new cLog().C_WRTxLog("cPdtPmt", "C_PRCxPdtPrice : STP_PRCxPdtPrice Result = " + nResult, cVB.bVB_AlwPrnLog);
                }
                new cLog().C_WRTxLog("cPdtPmt", "C_PRCxPdtPrice : End STP_PRCxPdtPrice", cVB.bVB_AlwPrnLog);
                //+++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_PRCxPdtPmt : " + oEx.Message); }
            finally
            {
                oDatabase = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public void C_PRCxDelPmtExpired()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("DELETE HD");
                oSql.AppendLine("FROM TCNTPdtPmtHD_L HD");
                oSql.AppendLine("WHERE EXISTS(SELECT FTPmhDocNo FROM TCNTPdtPmtHD WITH(NOLOCK) ");
                oSql.AppendLine("	WHERE CONVERT(VARCHAR(10),FDPmhDStop,121) < CONVERT(VARCHAR(10),GETDATE(),121)");
                oSql.AppendLine("	AND FTBchCode = HD.FTBchCode AND FTPmhDocNo = HD.FTPmhDocNo)");
                oSql.AppendLine("");
                oSql.AppendLine("DELETE HD");
                oSql.AppendLine("FROM TCNTPdtPmtHDCst HD");
                oSql.AppendLine("WHERE EXISTS(SELECT FTPmhDocNo FROM TCNTPdtPmtHD WITH(NOLOCK) ");
                oSql.AppendLine("	WHERE CONVERT(VARCHAR(10),FDPmhDStop,121) < CONVERT(VARCHAR(10),GETDATE(),121)");
                oSql.AppendLine("	AND FTBchCode = HD.FTBchCode AND FTPmhDocNo = HD.FTPmhDocNo)");
                oSql.AppendLine("");
                oSql.AppendLine("DELETE HD");
                oSql.AppendLine("FROM TCNTPdtPmtHDBch HD");
                oSql.AppendLine("WHERE EXISTS(SELECT FTPmhDocNo FROM TCNTPdtPmtHD WITH(NOLOCK) ");
                oSql.AppendLine("	WHERE CONVERT(VARCHAR(10),FDPmhDStop,121) < CONVERT(VARCHAR(10),GETDATE(),121)");
                oSql.AppendLine("	AND FTBchCode = HD.FTBchCode AND FTPmhDocNo = HD.FTPmhDocNo)");
                oSql.AppendLine("");
                oSql.AppendLine("DELETE DT");
                oSql.AppendLine("FROM TCNTPdtPmtDT DT");
                oSql.AppendLine("WHERE EXISTS(SELECT FTPmhDocNo FROM TCNTPdtPmtHD WITH(NOLOCK) ");
                oSql.AppendLine("	WHERE CONVERT(VARCHAR(10),FDPmhDStop,121) < CONVERT(VARCHAR(10),GETDATE(),121)");
                oSql.AppendLine("	AND FTBchCode = DT.FTBchCode AND FTPmhDocNo = DT.FTPmhDocNo)");
                oSql.AppendLine("");
                oSql.AppendLine("DELETE CB");
                oSql.AppendLine("FROM TCNTPdtPmtCB CB");
                oSql.AppendLine("WHERE EXISTS(SELECT FTPmhDocNo FROM TCNTPdtPmtHD WITH(NOLOCK) ");
                oSql.AppendLine("	WHERE CONVERT(VARCHAR(10),FDPmhDStop,121) < CONVERT(VARCHAR(10),GETDATE(),121)");
                oSql.AppendLine("	AND FTBchCode = CB.FTBchCode AND FTPmhDocNo = CB.FTPmhDocNo)");
                oSql.AppendLine("");
                oSql.AppendLine("DELETE CG");
                oSql.AppendLine("FROM TCNTPdtPmtCG CG");
                oSql.AppendLine("WHERE EXISTS(SELECT FTPmhDocNo FROM TCNTPdtPmtHD WITH(NOLOCK) ");
                oSql.AppendLine("	WHERE CONVERT(VARCHAR(10),FDPmhDStop,121) < CONVERT(VARCHAR(10),GETDATE(),121)");
                oSql.AppendLine("	AND FTBchCode = CG.FTBchCode AND FTPmhDocNo = CG.FTPmhDocNo)");
                oSql.AppendLine("");
                oSql.AppendLine("DELETE HD");
                oSql.AppendLine("FROM TCNTPdtPmtHD HD");
                oSql.AppendLine("WHERE EXISTS(SELECT FTPmhDocNo FROM TCNTPdtPmtHD WITH(NOLOCK) ");
                oSql.AppendLine("	WHERE CONVERT(VARCHAR(10),FDPmhDStop,121) < CONVERT(VARCHAR(10),GETDATE(),121)");
                oSql.AppendLine("	AND FTBchCode = HD.FTBchCode AND FTPmhDocNo = HD.FTPmhDocNo)");
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "C_PRCxDelPmtExpired : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// '*Em 63-07-08
        /// update quota SKC
        /// </summary>
        public void C_DATxUpdateQuotaPmt()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataRow[] aoData;
            try
            {
                //*Em 63-08-10
                oSql.Clear();
                oSql.AppendLine("UPDATE TPMTPmtTmp WITH(ROWLOCK)");
                oSql.AppendLine("SET FCPbyMaxValue = -1 ");
                oSql.AppendLine("WHERE FTPmhStaChkQuota = '1'");
                oDB.C_SETxDataQuery(oSql.ToString());
                //+++++++++++++

                aoData = cVB.oVB_CstPrivilege.Select("FNQuotas > 0");   //*Em 63-08-12
                //foreach (DataRow oRow in cVB.oVB_CstPrivilege.Rows)
                foreach (DataRow oRow in aoData)    //*Em 63-08-12
                {
                    oSql.Clear();
                    oSql.AppendLine("UPDATE TPMTPmtTmp WITH(ROWLOCK)");
                    oSql.AppendLine("SET FCPbyMaxValue = FCPbyMinValue * " + Convert.ToInt32(oRow.Field<decimal>("FNQuotas")));
                    oSql.AppendLine(",FTPmhStaLimitCst = '1'"); //*Em 63-08-11  ถ้ามี Qouta ไม่สนใจ Member
                    oSql.AppendLine("WHERE FTPmhStaChkQuota = '1'");
                    oSql.AppendLine("AND FTPmdRefCode = '" + oRow.Field<string>("FTPdtCode") + "'");
                    oDB.C_SETxDataQuery(oSql.ToString());
                }

                //*Em 63-08-10
                oSql.Clear();
                oSql.AppendLine("DELETE FROM TPMTPmtTmp WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTPmhStaChkQuota = '1'");
                oSql.AppendLine("AND FCPbyMaxValue = -1 ");
                oDB.C_SETxDataQuery(oSql.ToString());
                //+++++++++++++
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "C_DATxUpdateQuotaPmt : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory();
            }
        }

        public decimal C_GETcLimitQuotaByPmt(string ptPmhDocNo)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            decimal cQtyLimit = 0;
            string tPdtCode = "";
            try
            {
                tPdtCode = oDB.C_GETtFunction("TOP 1", "FTPmdRefCode", "TPMTPmtTmp", "WHERE FTPmhDocNo = '" + ptPmhDocNo + "'");
                DataRow oRow = cVB.oVB_CstPrivilege.Select("FTPdtCode = '" + tPdtCode +"'").FirstOrDefault();
                if (oRow != null) cQtyLimit = Convert.ToInt32(oRow.Field<decimal>("FNQuotas"));
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "C_GETcLimitQuotaByPmt : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory();
            }
            return cQtyLimit;
        }
    }
}
