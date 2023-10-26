using AdaLinkSKC.Model;
using AdaLinkSKC.Model.Config;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaLinkSKC.Class
{
    class cConfig
    {
        public cmlRabbitMQ oC_RabbitMQ; 
        NameValueCollection oCfgDabtabase;

        /// <summary>
        /// *Arm 63-07-02
        /// </summary>
        public bool C_CFGbLoadConfig(out string ptMsgErr)
        {
            cMS oMsg;

            StringBuilder oSql;
            cDatabase oDB;
            DataTable odtTmp;
            cSP oSP;
            try
            {
                new cLog().C_PRCxLog("C_CFGbLoadConfig", "Load Config .... ");

                oCfgDabtabase = (NameValueCollection)ConfigurationManager.GetSection("ConnectionSQL");
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oC_RabbitMQ = new cmlRabbitMQ();
                cVB.oVB_RabbitMQ = new cmlRabbitMQ();
                oMsg = new cMS();
                oSP = new cSP();

                if (oCfgDabtabase == null)
                {
                    ptMsgErr = string.Format(oMsg.tMS_CfgNotFound, "rabbit MQ");
                    return false;
                }

                cVB.oVB_Config = new cmlConfigDB();
                cVB.oVB_Config.tServerDB = oCfgDabtabase["ServerDB"].Trim();
                cVB.oVB_Config.tUser = oCfgDabtabase["UserDB"].Trim();
                cVB.oVB_Config.tPassword = oCfgDabtabase["PassDB"].Trim();
                cVB.oVB_Config.tNameDB = oCfgDabtabase["DBName"].Trim();
                cVB.oVB_Config.tAuthenDB = oCfgDabtabase["AuthenDB"].Trim();

                // Test Connect Database
                oSql.Clear();
                oSql.AppendLine("SELECT DB_NAME() AS DatabaseName");
                string tTestConnection = oDB.C_DAToExecuteQuery<string>(oSql.ToString());
                if (string.IsNullOrEmpty(tTestConnection))
                {
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Connect Database false ...");
                }
                else
                {
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Connect Database name " + tTestConnection + " Success.");


                    // Load Config MQ
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Load Config RabbitMQ ...");
                    if (string.IsNullOrEmpty(oCfgDabtabase["tMQVHost"].Trim()))
                    {
                        new cLog().C_PRCxLog("C_CFGbLoadConfig", "No configured tMQVHost collected at App.Config");
                    }
                    else
                    {
                        if (oCfgDabtabase["tMQVHost"].Trim() == "1" || oCfgDabtabase["tMQVHost"].Trim() == "2" || oCfgDabtabase["tMQVHost"].Trim() == "3")
                        {
                            oSql.Clear();
                            oSql.AppendLine("SELECT FTCfgCode,ISNULL(FTCfgStaUsrValue,ISNULL(FTCfgStaDefValue,'')) AS FTCfgStaUsrValue");
                            oSql.AppendLine("FROM TLKMConfig WITH(NOLOCK) ");
                            switch (oCfgDabtabase["tMQVHost"].Trim())
                            {
                                case "1":
                                    oSql.AppendLine("WHERE FTCfgCode IN ('tLK_NotiHost','tLK_NotiUsr','tLK_NotiPwd','tLK_NotiVHost','tLK_NotiQueue') ");
                                    break;
                                case "2":
                                    oSql.AppendLine("WHERE FTCfgCode IN ('tLK_NotiHost','tLK_NotiUsr','tLK_NotiPwd','tLK_NotiVHost','tLK_NotiQueue','tLK_SleepSnd','tLK_ExportDec','tLK_TotSnd','tLK_MailCC','tLK_MailReceive','tLK_MailSender','tLK_MailSubject') ");
                                    break;
                                case "3":
                                    break;

                            }
                            oSql.AppendLine("AND FTCfgSeq = '" + oCfgDabtabase["tMQVHost"].Trim() + "'");

                            odtTmp = new DataTable();
                            odtTmp = oDB.C_GEToSQLToDatatable(oSql.ToString());
                            if (odtTmp != null && odtTmp.Rows.Count > 0)
                            {
                                
                                foreach (DataRow oRow in odtTmp.Rows)
                                {
                                    switch (oRow.Field<string>("FTCfgCode"))
                                    {
                                        case "tLK_NotiHost":
                                            oC_RabbitMQ.tMQHostName = oRow.Field<string>("FTCfgStaUsrValue");
                                            cVB.oVB_RabbitMQ.tMQHostName = oC_RabbitMQ.tMQHostName;
                                            break;
                                        case "tLK_NotiUsr":
                                            oC_RabbitMQ.tMQUserName = oRow.Field<string>("FTCfgStaUsrValue");
                                            cVB.oVB_RabbitMQ.tMQUserName = oC_RabbitMQ.tMQUserName;
                                            break;
                                        case "tLK_NotiPwd":
                                            oC_RabbitMQ.tMQPassword = oRow.Field<string>("FTCfgStaUsrValue");
                                            cVB.oVB_RabbitMQ.tMQPassword = oC_RabbitMQ.tMQPassword;
                                            break;
                                        case "tLK_NotiVHost":
                                            oC_RabbitMQ.tMQVirtualHost = oRow.Field<string>("FTCfgStaUsrValue");
                                            cVB.oVB_RabbitMQ.tMQVirtualHost = oC_RabbitMQ.tMQVirtualHost;
                                            break;
                                        case "tLK_NotiQueue":
                                            oC_RabbitMQ.tMQListQueue = oRow.Field<string>("FTCfgStaUsrValue");
                                            cVB.oVB_RabbitMQ.tMQListQueue = oC_RabbitMQ.tMQListQueue;
                                            break;
                                        
                                        
                                    }
                                }

                                // Check Parameter
                                int nCntFlase = 0; 
                                if (string.IsNullOrEmpty(oC_RabbitMQ.tMQHostName))
                                {
                                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Check that the Config MQ host name is not set.");
                                    nCntFlase++;
                                }
                                if (string.IsNullOrEmpty(oC_RabbitMQ.tMQUserName))
                                {
                                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Check that the Config MQ Username is not set.");
                                    nCntFlase++;
                                }
                                if (string.IsNullOrEmpty(oC_RabbitMQ.tMQPassword))
                                {
                                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Check that the Config MQ Password is not set.");
                                    nCntFlase++;
                                }
                                if (string.IsNullOrEmpty(oC_RabbitMQ.tMQVirtualHost))
                                {
                                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Check that the Config MQ Visualhost is not set.");
                                    nCntFlase++;
                                }
                                if (string.IsNullOrEmpty(oC_RabbitMQ.tMQListQueue))
                                {
                                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Check that the Config MQ QueueName is not set.");
                                    nCntFlase++;
                                }

                                if (nCntFlase == 0)
                                {
                                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Verify Parameter MQ : true");
                                }
                                else
                                {
                                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Verify Parameter MQ : false");

                                }


                            }
                        }
                        else
                        {
                            new cLog().C_PRCxLog("C_CFGbLoadConfig", "This tMQVHost value is set incorrect.");
                        }
                        
                    }
                    
                    //GET CmpCpde
                    cVB.tVB_CmpCode = oSP.C_GETtCompany();

                    //GET BchCode
                    cVB.tVB_BchCode = oSP.C_GETtBranchCode();

                    //GET Properties
                    oSP.C_GETtBchProperties();

                    //GET Setting 
                    oSql.Clear();
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Load Link Config...");
                    oSql.AppendLine("SELECT FTCfgCode,ISNULL(FTCfgStaUsrValue,ISNULL(FTCfgStaDefValue,'')) AS FTCfgStaUsrValue");
                    oSql.AppendLine("FROM TLKMConfig WITH(NOLOCK) ");
                    oSql.AppendLine("WHERE FTCfgCode IN ('tLK_BackupPath','tLK_InPath','nLK_NameABB','tLK_OutPath','tLK_SleepSnd','tLK_ExportDec','tLK_TotSnd','tLK_MailCC','tLK_MailReceive','tLK_MailSender','tLK_MailSubject') ");
                    odtTmp = null;
                    odtTmp = new DataTable();
                    odtTmp = oDB.C_GEToSQLToDatatable(oSql.ToString());
                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        foreach (DataRow oRow in odtTmp.Rows)
                        {
                            switch (oRow.Field<string>("FTCfgCode"))
                            {
                                case "tLK_BackupPath":
                                    cVB.tVB_PathBackUP = oRow.Field<string>("FTCfgStaUsrValue");
                                    break;
                                case "tLK_InPath":
                                    DataTable oDbTbl = new DataTable();
                                    string tSplit = @"\";
                                    oSql.Clear();
                                    oSql.AppendLine(" SELECT COUNT(*) AS SplitURL FROM STRING_SPLIT((SELECT FTCfgStaUsrValue FROM TLKMConfig ");
                                    oSql.AppendLine(" WHERE FTCfgCode = 'tLK_InPath'),'" + tSplit + "')  ");
                                    oDbTbl = new cDatabase().C_GEToSQLToDatatable(oSql.ToString());
                                    cVB.nVB_SplitURL = Convert.ToInt32(oDbTbl.Rows[0]["SplitURL"].ToString());
                                    cVB.tVB_PathIN = oRow.Field<string>("FTCfgStaUsrValue");
                                    break;
                                case "tLK_OutPath":
                                    cVB.tVB_PathOUT = oRow.Field<string>("FTCfgStaUsrValue");
                                    break;
                                case "nLK_NameABB":
                                    cVB.nVB_SubStr = Convert.ToInt32(oRow.Field<string>("FTCfgStaUsrValue"));
                                    break;

                                case "tLK_ExportDec":
                                    if (string.IsNullOrEmpty(oRow.Field<string>("FTCfgStaUsrValue")))
                                    {
                                        cVB.nVB_Desc = 0;
                                    }
                                    else
                                    {
                                        cVB.nVB_Desc = Convert.ToInt32(oRow.Field<string>("FTCfgStaUsrValue"));
                                    }
                                    break;
                                case "tLK_TotSnd":
                                    if (string.IsNullOrEmpty(oRow.Field<string>("FTCfgStaUsrValue")))
                                    {
                                        cVB.nVB_ToSnd = 0;
                                    }
                                    else
                                    {
                                        cVB.nVB_ToSnd = Convert.ToInt32(oRow.Field<string>("FTCfgStaUsrValue"));
                                    }
                                    break;
                                case "tLK_SleepSnd":
                                    if (string.IsNullOrEmpty(oRow.Field<string>("FTCfgStaUsrValue")))
                                    {
                                        cVB.nVB_Pending = 0;
                                    }
                                    else
                                    {
                                        cVB.nVB_Pending = Convert.ToInt32(oRow.Field<string>("FTCfgStaUsrValue"));
                                    }
                                    break;
                                case "tLK_MailSender":
                                    cVB.tVB_MailSnd = oRow.Field<string>("FTCfgStaUsrValue");
                                    break;
                                case "tLK_MailReceive":
                                    cVB.tVB_MailRcv = oRow.Field<string>("FTCfgStaUsrValue");
                                    break;
                                case "tLK_MailCC":
                                    cVB.tVB_MailCC = oRow.Field<string>("FTCfgStaUsrValue");
                                    break;
                                case "tLK_MailSubject":
                                    cVB.tVB_MailSubj = oRow.Field<string>("FTCfgStaUsrValue");
                                    break;
                                case "XXXX":
                                    cVB.tVB_MailPwd = oRow.Field<string>("FTCfgStaUsrValue");
                                    break;
                                case "Smtp":
                                    cVB.tVB_MailSMTP = oRow.Field<string>("FTCfgStaUsrValue");
                                    break;
                                case "Port":
                                    cVB.nVB_MailPORT = Convert.ToInt32(oRow.Field<string>("FTCfgStaUsrValue"));
                                    break;
                            }
                        }
                    }

                    //Load Config
                    cVB.bVB_OpenLogMonitor = false;
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Load System Config...");
                    oSql.Clear();
                    oSql.AppendLine("SELECT FTSysCode, FTSysSeq, FTSysKey, FTSysStaUsrValue, FTSysStaUsrRef");
                    oSql.AppendLine("FROM TSysConfig WITH(NOLOCK)");
                    oSql.Append("WHERE FTSysApp IN ('ADALINK')");
                    oSql.Append("AND FTSysCode IN ('bAL_LogMonitor')");
                    odtTmp = new DataTable();
                    odtTmp = oDB.C_GEToSQLToDatatable(oSql.ToString());
                    if(odtTmp != null && odtTmp.Rows.Count >0)
                    {
                        foreach(DataRow oRow in odtTmp.Rows)
                        {
                            switch(oRow.Field<string>("FTSysCode"))
                            {
                                case "bAL_LogMonitor":
                                    if (oRow.Field<string>("FTSysStaUsrValue") == "1")
                                    {
                                        cVB.bVB_OpenLogMonitor = true;
                                    }
                                    break;
                            }
                        }
                    }
                    

                    //Load Config API
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Load API Config...");
                    string tAPI = new cSP().C_GETxCfgApiKADS("00004", cVB.tVB_BchCode, cVB.tVB_CmpCode, cVB.tVB_AgnCode, "", "", "");
                    if (!string.IsNullOrEmpty(tAPI))
                    {
                        string[] aAPI = tAPI.Split(',');
                        cVB.tVB_ApiAuth = aAPI[0].ToString();
                        cVB.tVB_ApiExport = aAPI[1].ToString();
                        cVB.tVB_ApiName = aAPI[2].ToString();
                        
                    }
                    
                    //Test
                    //cVB.tVB_MailSnd = "Attaphon@ada-soft.com";
                    //cVB.tVB_MailRcv = "armover123@gmail.com";
                    //cVB.tVB_MailCC = "armover123@gmail.com";
                    //cVB.tVB_MailBCC = "armover123@gmail.com";
                    //cVB.tVB_MailPwd = "Arm123456";
                    //cVB.tVB_MailSubj = "Test Send Mail !!";
                    //cVB.tVB_MailSMTP = "smtp.gmail.com";
                    //cVB.nVB_MailPORT = 587;

                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "======================= Config System =========================");
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Company Code = " + cVB.tVB_CmpCode);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Branch Code = " + cVB.tVB_BchCode);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "BchRefID/PantCode = " + cVB.tVB_BchRefID);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Agency Code = " + cVB.tVB_AgnCode);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "SaleOrg = " + cVB.tVB_SaleOrg);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Wahouse Code = " + cVB.tVB_WahCode);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Channel = " + cVB.tVB_Channel);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "DescShow = " + cVB.nVB_Desc);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Time Re-Send = " + cVB.nVB_ToSnd);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Time Pending Send = " + cVB.nVB_Pending + "s");
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Status Open log Monitor = " + cVB.bVB_OpenLogMonitor);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "======================== Config E-mail ========================");
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "smtp = " + cVB.tVB_MailSMTP);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Port = " + cVB.nVB_MailPORT);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Sender = " + cVB.tVB_MailSnd);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Receive = " + cVB.tVB_MailRcv);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "CC = " + cVB.tVB_MailCC);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "BCC = " + cVB.tVB_MailBCC);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Subject = " + cVB.tVB_MailSubj);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "========================== Config API =========================");
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Api Url = " + cVB.tVB_ApiExport);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Api Authen = " + cVB.tVB_ApiAuth);
                    new cLog().C_PRCxLog("C_CFGbLoadConfig", "Api Name = " + cVB.tVB_ApiName);
                    
                } // Connect Data Base

                ptMsgErr = "";
                return true;
            }
            catch(Exception oEx)
            {
                ptMsgErr = oEx.Message;
                new cLog().C_PRCxLog("C_CFGbLoadConfig", ptMsgErr);
            }
            finally
            {
                odtTmp = null;
                oSql = null;
                oDB = null;
                new cSP().SP_CLExMemory();
            }
            return false;
        }


        public void C_GETxConfig()
        {
            List<cmlTLKMConfig> aoTLKMConfig;
            try
            {
                oCfgDabtabase = (NameValueCollection)ConfigurationManager.GetSection("ConnectionSQL");
                
                cVB.oVB_Config = new cmlConfigDB();
                cVB.oVB_Config.tServerDB = oCfgDabtabase["ServerDB"].Trim();
                cVB.oVB_Config.tUser = oCfgDabtabase["UserDB"].Trim();
                cVB.oVB_Config.tPassword = oCfgDabtabase["PassDB"].Trim();
                cVB.oVB_Config.tNameDB = oCfgDabtabase["DBName"].Trim();
                cVB.oVB_Config.tAuthenDB = oCfgDabtabase["AuthenDB"].Trim();


                
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine(" SELECT FTCfgCode ,FTCfgApp ,FTCfgKey ,FTCfgSeq ,FTGmnCode ,FTCfgStaAlwEdit ,FTCfgStaDataType ");
                oSql.AppendLine("       ,FNCfgMaxLength ,FTCfgStaDefValue ,FTCfgStaDefRef ,FTCfgStaUsrValue ,FTCfgStaUsrRef ");
                oSql.AppendLine(" FROM TLKMConfig ");
                oSql.AppendLine(" WHERE FTCfgCode IN ( ");
                oSql.AppendLine(" 'tLK_BackupPath','tLK_InPath','tLK_OutPath','tLK_MQHost','tLK_MQUser', ");
                oSql.AppendLine(" 'tLK_MQPass','tLK_MQVHost','nLK_MQPort','tLK_MQQueue','nLK_NameABB' ");
                oSql.AppendLine(" ) ");

                aoTLKMConfig = new cDatabase().C_GETaDataQuery<cmlTLKMConfig>(oSql.ToString());

                foreach (cmlTLKMConfig oConfig in aoTLKMConfig)
                {
                    switch (oConfig.FTCfgCode)
                    {
                        case "tLK_BackupPath":
                            cVB.tVB_PathBackUP = oConfig.FTCfgStaUsrValue;
                            break;
                        case "tLK_InPath":
                            DataTable oDbTbl = new DataTable();
                            string tSplit = @"\";
                            oSql.Clear();
                            oSql.AppendLine(" SELECT COUNT(*) AS SplitURL FROM STRING_SPLIT((SELECT FTCfgStaUsrValue FROM TLKMConfig ");
                            oSql.AppendLine(" WHERE FTCfgCode = 'tLK_InPath'),'"+ tSplit + "')  ");
                            oDbTbl = new cDatabase().C_GEToSQLToDatatable(oSql.ToString());
                            cVB.nVB_SplitURL = Convert.ToInt32(oDbTbl.Rows[0]["SplitURL"].ToString());
                            cVB.tVB_PathIN = oConfig.FTCfgStaUsrValue;
                            break;
                        case "tLK_OutPath":
                            cVB.tVB_PathOUT = oConfig.FTCfgStaUsrValue;
                            break;                      
                        case "tLK_MQHost":
                            cVB.tVB_MQHost = oConfig.FTCfgStaUsrValue;
                            break;
                        case "tLK_MQUser":
                            cVB.tVB_MQUser = oConfig.FTCfgStaUsrValue;
                            break;
                        case "tLK_MQPass":
                            cVB.tVB_MQPass = oConfig.FTCfgStaUsrValue;
                            break;
                        case "tLK_MQVHost":
                            cVB.tVB_MQVB = oConfig.FTCfgStaUsrValue;
                            break;
                        case "nLK_MQPort":
                            cVB.nVB_MQPort = Convert.ToInt32(oConfig.FTCfgStaUsrValue);
                            break;
                        case "tLK_MQQueue":
                            cVB.tVB_MQQueue = oConfig.FTCfgStaUsrValue;
                            break;
                        case "nLK_NameABB":
                            cVB.nVB_SubStr = Convert.ToInt32(oConfig.FTCfgStaUsrValue);
                            break;
                    }
                }
                

            }
            catch(Exception oEx)
            {
                new cLog().C_PRCxLog("C_GETxConfig", oEx.Message.Trim());
            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }
        
    }
}
