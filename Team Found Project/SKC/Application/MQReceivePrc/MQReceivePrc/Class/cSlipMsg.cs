using MQReceivePrc.Models.SlipMsg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    public class cSlipMsg
    {
        public cSlipMsg()
        {

        }

        /// <summary>
        /// Get Message Header
        /// </summary>
        public int C_GETnSlipMsg(string ptConnStr,string ptSmgCode,string ptSmgType, Graphics poGraphic, int pnWidth, int pnY,Font poFont)
        {
            List<cmlTCNMSlipMsgDT_L> aoMsg = null;
            StringBuilder oSql;
            Graphics oGraphic = poGraphic;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSmgName");
                oSql.AppendLine("FROM TCNMSlipMsgHD_L MSH");
                oSql.AppendLine("INNER JOIN TCNMSlipMsgDT_L MSD ON MSD.FTSmgCode = MSH.FTSmgCode");
                oSql.AppendLine("   AND MSD.FNLngID = MSH.FNLngID");
                oSql.AppendLine("   AND MSD.FNLngID = 1");
                oSql.AppendLine("WHERE MSD.FTSmgType = '" + ptSmgType + "'");
                oSql.AppendLine("   AND MSH.FTSmgCode = '" + ptSmgCode + "'");
                oSql.AppendLine("ORDER BY FNSmgSeq");

                aoMsg = new cDatabase().C_GETaDataQuery<cmlTCNMSlipMsgDT_L>(ptConnStr, oSql.ToString(), 60).ToList();

                foreach (cmlTCNMSlipMsgDT_L oMsg in aoMsg)
                {
                    oGraphic.DrawString(oMsg.FTSmgName, poFont, Brushes.Black, new RectangleF(0, pnY, pnWidth, 18),
                                          new StringFormat
                                          {
                                              FormatFlags = StringFormatFlags.NoWrap,
                                              Trimming = StringTrimming.EllipsisCharacter,
                                              Alignment = StringAlignment.Center
                                          });
                    pnY += 18;
                }
            }
            catch (Exception oEx)
            { }
            finally
            {
                ptSmgType = null;
                oSql = null;
                aoMsg = null;
                poGraphic = null;
            }

            return pnY;
        }
    }
}
