using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdaPos.Class
{
    public class cCustomer
    {
        public cCustomer()
        {

        }

        /// <summary>
        /// Search Customer Local
        /// </summary>
        /// <param name="pnRows">แถวที่ต้องการค้นหา</param>
        /// <param name="pnSchBy">ประเภทที่ต้องการค้นหา</param>
        /// <param name="ptSch">ข้อความที่ต้องการค้นหา</param>
        /// <param name="pnModeSch">บางส่วน หรือ ทั้งหมด</param>
        /// <returns></returns>
        public List<cmlTCNMCst> C_SCHaCstLocal(int pnRows, int pnSchBy, string ptSch, int pnModeSch)
        {
            List<cmlTCNMCst> aoCst = new List<cmlTCNMCst>();
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();

                oSql.AppendLine("SELECT CST.FTCstCode, ISNULL(CSTL.FTCstName,(SELECT TOP 1 FTCstName FROM TCNMCst_L with(nolock) WHERE FTCstCode = CST.FTCstCode)) AS FTCstName, CST.FTCstTel, CST.FTCstEmail, ISNULL(CGL.FTCgpName,'') AS FTCgpName,CST.FTCgpCode,CST.FTPplCodeRet");
                oSql.AppendLine(",CLL.FTClvName, IMG.FTImgObj AS FTCstImage, ADR.FTAddrDesc, CST.FTCstStaAlwPosCalSo, PNT.FCTxnPntQty AS cCstPoint"); //*Arm 63-03-03 เพิ่ม FTCstStaAlwPosCalSo,PNT.FCTxnPntQty
                oSql.AppendLine("FROM TCNMCst CST with(nolock)");
                oSql.AppendLine("LEFT JOIN TCNMCst_L CSTL with(nolock) ON CSTL.FTCstCode = CST.FTCstCode");
                oSql.AppendLine("   AND CSTL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("LEFT JOIN TCNMCstGrp_L CGL with(nolock) ON CGL.FTCgpCode = CST.FTCgpCode");
                oSql.AppendLine("	AND CGL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("LEFT JOIN TCNMCstLev_L CLL with(nolock) ON CLL.FTClvCode = CST.FTClvCode");
                oSql.AppendLine("	AND CLL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("LEFT JOIN TCNMImgPerson IMG with(nolock) ON IMG.FTImgRefID = CST.FTCstCode AND IMG.FTImgTable = 'TCNMCst'");
                oSql.AppendLine("LEFT JOIN (SELECT  ADDR.FTAddRefNo ,");
                oSql.AppendLine("           (CASE WHEN ISNULL(ADDR.FTAddVersion,'') = '1'");
                oSql.AppendLine("           	THEN ISNULL(ADDR.FTAddV1No,'') + ' ' + ISNULL(ADDR.FTAddV1Soi,'') + ' ' + ISNULL(ADDR.FTAddV1Village,'') + ' ' + ISNULL(ADDR.FTAddV1Road,'') + ' ' +");
                oSql.AppendLine("           		ISNULL(SUD.FTSudName,'') + ' ' + ISNULL(DST.FTDstName,'') + ' ' + ISNULL(PVN.FTPvnName,'') + ' ' + ISNULL(ADDR.FTAddV1PostCode,'')");
                oSql.AppendLine("           	ELSE");
                oSql.AppendLine("           		ISNULL(ADDR.FTAddV2Desc1,'') + ' ' + ISNULL(ADDR.FTAddV2Desc2,'')");
                oSql.AppendLine("           	END) AS FTAddrDesc");
                oSql.AppendLine("           FROM TCNMCstAddress_L ADDR with(nolock)");
                oSql.AppendLine("           LEFT JOIN TCNMSubDistrict_L SUD with(nolock) ON ADDR.FTAddV1DstCode = SUD.FTSudCode AND SUD.FNLngID = 1");
                oSql.AppendLine("           LEFT JOIN TCNMDistrict_L DST with(nolock) ON ADDR.FTAddV1DstCode = DST.FTDstCode AND DST.FNLngID = 1");
                oSql.AppendLine("           LEFT JOIN TCNMProvince_L PVN with(nolock) ON ADDR.FTAddV1PvnCode = PVN.FTPvnCode AND PVN.FNLngID = 1");
                oSql.AppendLine("           WHERE ADDR.FTAddGrpType = '1' AND ADDR.FNLngID = 1) ADR ON CST.FTCstCode = ADR.FTAddRefNo");
                oSql.AppendLine("LEFT JOIN TCNTMemPntActive PNT with(nolock) ON CST.FTCstCode = PNT.FTMemCode");    //*Arm 63-03-13
                if (!string.IsNullOrEmpty(ptSch))
                {
                    oSql.AppendLine("WHERE 1 = 1");

                    // Type Search
                    switch (pnSchBy)
                    {
                        case 0:     // Code
                            oSql.Append("AND CST.FTCstCode ");
                            break;

                        case 1:     // Name
                            //oSql.Append("AND CSTL.FTCstName ");
                            oSql.Append("AND ISNULL(CSTL.FTCstName,(SELECT TOP 1 FTCstName FROM TCNMCst_L with(nolock) WHERE FTCstCode = CST.FTCstCode)) ");  //*Em 62-08-04
                            break;

                        case 2:     // Tel
                            oSql.Append("AND CST.FTCstTel ");
                            break;

                        case 3:     // Email
                            oSql.Append("AND CST.FTCstEmail ");
                            break;

                        case 4:     // Group
                            oSql.Append("AND CGL.FTCgpName ");
                            break;
                    }

                    if (pnModeSch == 0)  // บางส่วนของข้อความ
                        oSql.AppendLine("LIKE '%" + ptSch + "%'");
                    else        // ทั้งหมดของข้อความ
                        oSql.AppendLine("= '" + ptSch + "'");
                }

                oSql.AppendLine("ORDER BY CST.FTCstCode");
                oSql.AppendLine("OFFSET " + pnRows + " ROWS FETCH NEXT 50 ROWS ONLY;");

                aoCst = new cDatabase().C_GETaDataQuery<cmlTCNMCst>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cCustomer", "C_SCHaCstLocal " + oEx.Message); }
            finally
            {
                oSql = null;
                new cSP().SP_CLExMemory();
            }

            return aoCst;
        }

        /// <summary>
        /// Search Customer Server
        /// </summary>
        /// <returns></returns>
        public List<cmlTCNMCst> C_SCHaCstServer()
        {
            List<cmlTCNMCst> aoCst = new List<cmlTCNMCst>();

            try
            {

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cCustomer", "C_SCHaCstServer : " + oEx.Message); }

            return aoCst;
        }
    }
}
