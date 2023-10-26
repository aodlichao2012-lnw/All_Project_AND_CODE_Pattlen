using AdaPos.Models.DatabaseTmp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cSlipMsg
    {
        public cSlipMsg()
        {

        }

        /// <summary>
        /// Get Message Header
        /// </summary>
        public int C_GETnSlipMsg(string ptSmgType, Graphics poGraphic, int pnWidth, int pnY)
        {
            List<cmlTCNMSlipMsgDTTmp_L> aoMsg = null;
            StringBuilder oSql;
            Graphics oGraphic = poGraphic;

            try
            {
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTSmgName");
                //oSql.AppendLine("FROM TCNMSlipMsgHD_L MSH WITH(NOLOCK)");
                //oSql.AppendLine("INNER JOIN TCNMSlipMsgDT_L MSD WITH(NOLOCK) ON MSD.FTSmgCode = MSH.FTSmgCode");
                //oSql.AppendLine("   AND MSD.FNLngID = MSH.FNLngID");
                //oSql.AppendLine("   AND MSD.FNLngID = '" + cVB.nVB_Language + "'");
                //oSql.AppendLine("WHERE MSD.FTSmgType = '" + ptSmgType + "'");
                //oSql.AppendLine("   AND MSH.FTSmgCode = '" + cVB.tVB_SmgCode + "'");
                //oSql.AppendLine("ORDER BY FNSmgSeq");

                //aoMsg = new cDatabase().C_GETaDataQuery<cmlTCNMSlipMsgDTTmp_L>(oSql.ToString());

                //*Em 63-05-16
                aoMsg = cVB.oVB_SlipMsg.Where(oMsg => oMsg.FTSmgType == ptSmgType).OrderBy(oSort => oSort.FNSmgSeq).ToList();
                //+++++++++++++++

                foreach(cmlTCNMSlipMsgDTTmp_L oMsg in aoMsg)
                {
                    oGraphic.DrawString(oMsg.FTSmgName, cVB.aoVB_PInvLayout[0], Brushes.Black, new RectangleF(0, pnY, pnWidth, 18),
                                          new StringFormat
                                          {
                                              FormatFlags = StringFormatFlags.NoWrap,
                                              Trimming = StringTrimming.EllipsisCharacter,
                                              Alignment = StringAlignment.Center
                                          });
                    pnY += 18;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSlipMsg", "C_GETnSlipMsg : " + oEx.Message); }
            finally
            {
                ptSmgType = null;
                oSql = null;
                aoMsg = null;
                poGraphic = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return pnY;
        }

        public static void C_GETxSlipMsg()
        {
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT MSD.FTSmgType,MSD.FNSmgSeq,MSD.FTSmgName");
                oSql.AppendLine("FROM TCNMSlipMsgHD_L MSH WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TCNMSlipMsgDT_L MSD WITH(NOLOCK) ON MSD.FTSmgCode = MSH.FTSmgCode");
                oSql.AppendLine("   AND MSD.FNLngID = MSH.FNLngID");
                oSql.AppendLine("   AND MSD.FNLngID = '" + cVB.nVB_Language + "'");
                oSql.AppendLine("WHERE MSH.FTSmgCode = '" + cVB.tVB_SmgCode + "'");
                oSql.AppendLine("ORDER BY MSD.FTSmgType,MSD.FNSmgSeq");
                cVB.oVB_SlipMsg = new cDatabase().C_GETaDataQuery<cmlTCNMSlipMsgDTTmp_L>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSlipMsg", "C_GETxSlipMsg : " + oEx.Message); }
            finally
            {
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }
    }
}
