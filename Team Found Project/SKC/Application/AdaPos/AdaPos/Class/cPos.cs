using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cPos
    {
        public cPos()
        {
            try
            {
                C_GETxPosData();
                C_GETxPosHW();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPos", "cPos : " + oEx.Message); }
        }

        /// <summary>
        /// Get TCNMPos
        /// </summary>
        private void C_GETxPosData()
        {
            cmlTCNMPos oPos;
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTPosType, FTPosStaShift, FTPosRegNo, FTSmgCode, FTPosStaSumScan, FTPosStaSumPrn");
                oSql.AppendLine("FROM TCNMPos");
                oSql.AppendLine("WHERE FTPosCode = '" + cVB.tVB_PosCode + "' ");
                oSql.AppendLine("AND FTBchcode = '" + cVB.tVB_BchCode + "'");

                oPos = new cDatabase().C_GEToDataQuery<cmlTCNMPos>(oSql.ToString());

                if(oPos != null)
                {
                    cVB.tVB_PosType = oPos.FTPosType;
                    cVB.tVB_PosStaShift = oPos.FTPosStaShift;
                    cVB.tVB_SmgCode = oPos.FTSmgCode;
                    cVB.tVB_PosRegNo = oPos.FTPosRegNo;
                    
                    //*Arm 63-05-05
                    //สถานะรวมรายการสินค้าตอนสแกน 1:อนุญาต 2: ไม่อนุญาต
                    if(string.IsNullOrEmpty(oPos.FTPosStaSumScan))
                    {
                        cVB.nVB_StaSumScan = 2;
                    }
                    else
                    {
                        cVB.nVB_StaSumScan = Convert.ToInt32(oPos.FTPosStaSumScan);
                    }

                    //สถานะรวมรายการสินค้าตอนพิมพ์ 1:อนุญาต 2: ไม่อนุญาต
                    if (string.IsNullOrEmpty(oPos.FTPosStaSumPrn))
                    {
                        cVB.nVB_StaSumPrn = 2;
                    }
                    else
                    {
                        cVB.nVB_StaSumPrn = Convert.ToInt32(oPos.FTPosStaSumPrn);
                    }
                    //+++++++++++++
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPos", "C_GETxPosData : " + oEx.Message); }
        }

        /// <summary>
        /// Get TCNMPosHW
        /// </summary>
        private void C_GETxPosHW()
        {
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTPhwCode, FTShwCode, FTPhwName, FTPhwConnType, ");
                oSql.AppendLine("		FTPhwConnRef, FTPhwCustom, FNPhwTimeOut");
                oSql.AppendLine("FROM TCNMPosHW");
                oSql.AppendLine("WHERE FTPosCode = '" + cVB.tVB_PosCode + "'");

                cVB.aoVB_PosHW = new cDatabase().C_GETaDataQuery<cmlTCNMPosHW>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPos", "C_GETxPosHW : " + oEx.Message); }
        }
    }
}
