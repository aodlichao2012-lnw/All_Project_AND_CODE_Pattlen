using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cSetting
    {
        public cSetting()
        {

        }

        /// <summary>
        /// Get Config setting
        /// </summary>
        public void C_GETxCfgSetting()
        {
            List<cmlTSysSetting> aoSetting;
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSysCode, FTSysStaUsrValue, FTSysStaUsrRef, FNSysMaxLength, FTSysSeq ");
                oSql.AppendLine("FROM TSysSetting");
                oSql.AppendLine("WHERE FTSysCode IN ( 'tPS_AppColor', 'tPS_AppTheme', 'tPS_LastBillA', ");
                oSql.AppendLine("   'tPS_LastBillR', 'tPS_LastBillS', 'tPS_LastSaleD', 'tPS_PosNo', ");
                oSql.AppendLine("   'nPS_MenuBillPerPage','nPS_PdtGrpPerPage','nPS_PdtPerPage',");    //*Em 62-11-15
                oSql.AppendLine("   'tPS_SplashScreen', 'tPS_SgnRPosSrv','tPS_MQ','tPS_Password','nPS_Shw2ndScreen',");
                oSql.AppendLine("   'tPS_TaxPrint',");   //*Net 63-03-28 ยกมาจาก baseline
                oSql.AppendLine("   'tPS_Branch',");   //*Net 63-03-28 ยกมาจาก baseline
                oSql.AppendLine("   'nPS_SaleMode')"); //*Net 63-03-28 ยกมาจาก baseline

                aoSetting = new cDatabase().C_GETaDataQuery<cmlTSysSetting>(oSql.ToString());

                foreach (cmlTSysSetting oCfg in aoSetting)
                {
                    switch (oCfg.FTSysCode)
                    {
                        case "tPS_AppColor":
                            switch (oCfg.FTSysSeq)
                            {
                                case "1":
                                    cVB.oVB_ColNormal = ColorTranslator.FromHtml("#" + oCfg.FTSysStaUsrValue);
                                    break;

                                case "2":
                                    cVB.oVB_ColDark = ColorTranslator.FromHtml("#" + oCfg.FTSysStaUsrValue);
                                    break;

                                case "3":
                                    cVB.oVB_ColLight = ColorTranslator.FromHtml("#" + oCfg.FTSysStaUsrValue);
                                    break;
                            }
                            break;

                        case "tPS_AppTheme":
                            cVB.nVB_CNTheme = Convert.ToInt32(oCfg.FTSysStaUsrValue);
                            break;

                        case "tPS_LastBillA":
                            break;

                        case "tPS_LastBillR":
                            break;

                        case "tPS_LastBillS":
                            break;

                        case "tPS_LastSaleD":
                            break;

                        case "tPS_PosNo":
                            cVB.tVB_PosCode = oCfg.FTSysStaUsrValue;
                            break;
                            //*Zen 63-03-31
                        case "tPS_Branch":
                            cVB.tVB_BchCode = oCfg.FTSysStaUsrValue;
                            break;

                        case "tPS_SplashScreen":
                            cVB.bVB_SplashScreen = (string.Equals(oCfg.FTSysStaUsrValue, "1")) ? true : false;
                            break;

                        case "tPS_SgnRPosSrv":
                            cVB.tVB_SgnRPosSrv = oCfg.FTSysStaUsrValue;
                            break;
                        case "tPS_Password":
                            if (!string.IsNullOrEmpty(oCfg.FTSysStaUsrValue))
                            {
                                cVB.tVB_Passcode = oCfg.FTSysStaUsrValue;
                            }
                            break;
                        //case "tPS_MQ":
                        //    switch (oCfg.FTSysSeq)
                        //    {
                        //        case "1":
                        //            cVB.oVB_RabbitMQConfig.tHostName = oCfg.FTSysStaUsrValue;
                        //            break;

                        //        case "2":
                        //            cVB.oVB_RabbitMQConfig.tUserName = oCfg.FTSysStaUsrValue;
                        //            break;

                        //        case "3":
                        //            //cVB.oVB_RabbitMQConfig.tPassword = oCfg.FTSysStaUsrValue;
                        //            cVB.oVB_RabbitMQConfig.tPassword = new cEncryptDecrypt("1").C_CALtDecrypt(oCfg.FTSysStaUsrValue,cVB.tVB_MQ_KEY);    //*Em 62-08-20
                        //            break;

                        //        case "4":
                        //            cVB.oVB_RabbitMQConfig.tVirtual = oCfg.FTSysStaUsrValue;
                        //            break;

                        //        case "5":
                        //            cVB.oVB_RabbitMQConfig.tHostPort = oCfg.FTSysStaUsrValue;
                        //            break;

                        //        case "6":
                        //            cVB.oVB_RabbitMQConfig.tQueueName = oCfg.FTSysStaUsrValue;
                        //            break;

                        //        case "7":
                        //            cVB.oVB_RabbitMQConfig.tExchangeName = oCfg.FTSysStaUsrValue;
                        //            break;

                        //        case "8":
                        //            cVB.oVB_RabbitMQConfig.tRoutingKey = oCfg.FTSysStaUsrValue;
                        //            break;
                        //    }
                        //    break;

                        //*Em 62-11-15
                        case "nPS_MenuBillPerPage":
                            cVB.nVB_MenuPerPage = Convert.ToInt32(oCfg.FTSysStaUsrValue);
                            break;
                        case "nPS_PdtGrpPerPage":
                            cVB.nVB_GrpPerPage = Convert.ToInt32(oCfg.FTSysStaUsrValue);
                            break;
                        case "nPS_PdtPerPage":
                            cVB.nVB_PdtPerPage = Convert.ToInt32(oCfg.FTSysStaUsrValue);
                            break;
                        case "nPS_Shw2ndScreen":
                            if (!string.IsNullOrEmpty(oCfg.FTSysStaUsrValue))
                            {
                                cVB.nVB_Check2nd = Convert.ToInt32(oCfg.FTSysStaUsrValue);
                            }
                            break;
                        //++++++++++++++++++

                        //*Net 63-03-28 ยกมาจาก baseline
                        case "tPS_TaxPrint":
                            cVB.tVB_CNStaPrnTax = oCfg.FTSysStaUsrValue;
                            cVB.tVB_CNTaxPrnDriver = oCfg.FTSysStaUsrRef;
                            break;
                        //+++++++++++++

                        //*Net 63-03-28 ยกมาจาก baseline
                        case "nPS_SaleMode":
                            cVB.nVB_SaleModeStd = Convert.ToInt32(oCfg.FTSysStaUsrValue);
                            break;
                    }
                }

                //*Net 63-03-28 ยกมาจาก baseline
                if (string.IsNullOrEmpty(cVB.tVB_CNStaPrnTax))
                {
                    cVB.tVB_CNStaPrnTax = "1";  //1:กระดาษ Thermal  2:กระดาษ A4
                    cVB.tVB_CNTaxPrnDriver = "";
                }
                //+++++++++++++

                //*Em 62-09-14
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 UOJ.FTUrlAddress,UOJ.FTUrlPort,UOL.FTUolVhost,UOL.FTUolUser,UOL.FTUolPassword");
                oSql.AppendLine("FROM TCNTUrlObject UOJ WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TCNTUrlObjectLogin UOL WITH(NOLOCK) ON UOJ.FTUrlRefID = UOL.FTUrlRefID AND UOJ.FTUrlAddress = UOL.FTUrlAddress AND UOJ.FNUrlType = 3");
                oSql.AppendLine("WHERE UOL.FTUolKey = 'MQSale'");
                oSql.AppendLine("AND UOL.FTUolStaActive = '1'");
                oSql.AppendLine("AND UOJ.FTUrlTable = 'TCNMBranch' AND UOJ.FTUrlRefID = '" + cVB.tVB_BchCode +"'"); //*Em 62-11-03
                cVB.oVB_RabbitMQConfig = new Models.Other.cmlRabbitMQConfig();  //*Em 62-08-20
                DataTable odtTmp = new DataTable();
                odtTmp = new cDatabase().C_GEToDataQuery(oSql.ToString());
                if (odtTmp != null)
                {
                    if (odtTmp.Rows.Count > 0)
                    {
                        DataRow oRow = odtTmp.Rows[0];
                        cVB.oVB_RabbitMQConfig.tHostName = oRow.Field<string>("FTUrlAddress");
                        cVB.oVB_RabbitMQConfig.tHostPort = oRow.Field<string>("FTUrlPort");
                        cVB.oVB_RabbitMQConfig.tVirtual = oRow.Field<string>("FTUolVhost");
                        cVB.oVB_RabbitMQConfig.tUserName = oRow.Field<string>("FTUolUser");
                        cVB.oVB_RabbitMQConfig.tPassword = oRow.Field<string>("FTUolPassword");
                        cVB.oVB_RabbitMQConfig.tQueueName = "STOCKVD2POS";

                        //*Em 63-01-09
                        cVB.tVB_BCHMQHost = oRow.Field<string>("FTUrlAddress");
                        cVB.tVB_BCHMQPort = oRow.Field<string>("FTUrlPort");
                        cVB.tVB_BCHMQVirtual = oRow.Field<string>("FTUolVhost");
                        cVB.tVB_BCHMQUsr = oRow.Field<string>("FTUolUser");
                        cVB.tVB_BCHMQPwd = oRow.Field<string>("FTUolPassword");
                        //+++++++++++++++++
                    }
                }
                //++++++++++++++

                //*Em 63-01-09
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 UOJ.FTUrlAddress,UOJ.FTUrlPort,UOL.FTUolVhost,UOL.FTUolUser,UOL.FTUolPassword");
                oSql.AppendLine("FROM TCNTUrlObject UOJ WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TCNTUrlObjectLogin UOL WITH(NOLOCK) ON UOJ.FTUrlRefID = UOL.FTUrlRefID AND UOJ.FTUrlAddress = UOL.FTUrlAddress AND UOJ.FNUrlType = 3");
                oSql.AppendLine("WHERE UOL.FTUolKey = 'MQSale'");
                oSql.AppendLine("AND UOJ.FTUrlTable = 'TCNMBranch' AND UOJ.FTUrlRefID = '" + cVB.tVB_HQBchCode + "'"); //*Em 62-11-03
                //cVB.oVB_RabbitMQConfig = new Models.Other.cmlRabbitMQConfig();  //*Em 62-08-20
                odtTmp = new DataTable();
                odtTmp = new cDatabase().C_GEToDataQuery(oSql.ToString());
                if (odtTmp != null)
                {
                    if (odtTmp.Rows.Count > 0)
                    {
                        DataRow oRow = odtTmp.Rows[0];
                        cVB.tVB_HQMQHost = oRow.Field<string>("FTUrlAddress");
                        cVB.tVB_HQMQPort = oRow.Field<string>("FTUrlPort");
                        cVB.tVB_HQMQVirtual = oRow.Field<string>("FTUolVhost");
                        cVB.tVB_HQMQUsr = oRow.Field<string>("FTUolUser");
                        cVB.tVB_HQMQPwd = oRow.Field<string>("FTUolPassword");
                    }
                }
                //+++++++++++++++++++++

                //*Arm 63-03-17 Connect RabbitMQ AdaMember(Center)
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 UOJ.FTUrlAddress,UOJ.FTUrlPort,UOL.FTUolVhost,UOL.FTUolUser,UOL.FTUolPassword");
                oSql.AppendLine("FROM TCNTUrlObject UOJ WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TCNTUrlObjectLogin UOL WITH(NOLOCK) ON UOJ.FTUrlRefID = UOL.FTUrlRefID AND UOJ.FTUrlAddress = UOL.FTUrlAddress AND UOJ.FNUrlType = 13"); //*Arm 63-03-30 แก้ไข UOJ.FNUrlType = 13
                oSql.AppendLine("WHERE UOL.FTUolKey = 'MQMember'");
                oSql.AppendLine("AND UOJ.FTUrlTable = 'TCNMBranch' AND UOJ.FTUrlRefID = '" + cVB.tVB_BchCode + "'"); 
                
                odtTmp = new DataTable();
                odtTmp = new cDatabase().C_GEToDataQuery(oSql.ToString());
                if (odtTmp != null)
                {
                    if (odtTmp.Rows.Count > 0)
                    {
                        DataRow oRow = odtTmp.Rows[0];
                        cVB.tVB_MemberMQHost = oRow.Field<string>("FTUrlAddress");
                        cVB.tVB_MemberMQPort = oRow.Field<string>("FTUrlPort");
                        cVB.tVB_MemberMQVirtual = oRow.Field<string>("FTUolVhost");
                        cVB.tVB_MemberMQUsr = oRow.Field<string>("FTUolUser");
                        cVB.tVB_MemberMQPwd = oRow.Field<string>("FTUolPassword");
                    }
                }
                //+++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSetting", "C_GETxCfgSetting : " + oEx.Message); }
        }
    }
}
