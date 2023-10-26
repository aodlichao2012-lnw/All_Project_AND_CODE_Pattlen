using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Class
{
    public class cCompany
    {
        public cCompany()
        {

        }

        /// <summary>
        /// Get company
        /// </summary>
        public void C_GETxCompany()
        {
            StringBuilder oSql;
            cmlTCNMCompany oCmp;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP(1) CMP.FTCmpCode, CMP.FTBchcode, CMP.FTRteCode, CMP.FTVatCode, ");
                //oSql.AppendLine("       CMPL.FTCmpName, BCH.FTWahCode, BCHL.FTBchName,CMP.FTCmpRetInOrEx,");
                //oSql.AppendLine("       CMPL.FTCmpName, BCHL.FTBchName,CMP.FTCmpRetInOrEx,");   //*Em 62-06-10
                oSql.AppendLine("       ISNULL(CMPL.FTCmpName,(SELECT TOP 1 FTCmpName FROM TCNMComp_L WITH(NOLOCK) WHERE FTCmpCode = CMP.FTCmpCode)) AS FTCmpName,");   //*Em 62-09-06
                oSql.AppendLine("       ISNULL(BCHL.FTBchName,(SELECT TOP 1 FTBchName FROM TCNMBranch_L WITH(NOLOCK) WHERE FTBchcode = CMP.FTBchcode)) AS FTBchName,"); //*Em 62-09-06
                oSql.AppendLine("       ISNULL((SELECT TOP 1 FTPplCode FROM TCNMBranch WITH(NOLOCK) WHERE FTBchcode = CMP.FTBchcode),'') AS FTPplCode,"); //*Net 63-03-24
                oSql.AppendLine("       CMP.FTCmpRetInOrEx,CMP.FTVatCode");
                oSql.AppendLine("FROM TCNMComp CMP WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TCNMComp_L CMPL WITH(NOLOCK) ON CMPL.FTCmpCode = CMP.FTCmpCode");
                oSql.AppendLine("   AND CMPL.FNLngID = " + cVB.nVB_Language);
                //oSql.AppendLine("INNER JOIN TCNMBranch BCH WITH(NOLOCK) ON BCH.FTBchCode = CMP.FTBchcode ");
                //oSql.AppendLine("   AND BCH.FTBchStaActive = '1'");
                oSql.AppendLine("LEFT JOIN TCNMBranch_L BCHL WITH(NOLOCK) ON BCHL.FTBchCode = CMP.FTBchcode ");
                oSql.AppendLine("   AND BCHL.FNLngID = " + cVB.nVB_Language);

                oCmp = new cDatabase().C_GEToDataQuery<cmlTCNMCompany>(oSql.ToString());

                if (oCmp != null)
                {
                    //*Net 63-04-07 ถ้า Bchcode ดึงมาจาก TSysSetting แล้ว ไม่่ต้องเอามาจาก Comp
                    if (string.IsNullOrEmpty(cVB.tVB_BchCode))
                    {
                        cVB.tVB_BchCode = oCmp.FTBchcode;
                    }
                    //+++++++++++++++++++++++++++++++++++++++++++
                    cVB.tVB_RteCode = oCmp.FTRteCode;
                    cVB.tVB_CmpCode = oCmp.FTCmpCode;
                    cVB.tVB_CmpName = oCmp.FTCmpName;
                    //cVB.tVB_WahCode = oCmp.FTWahCode;
                    cVB.tVB_WahCode = "001";    //*Em 62-06-10
                    //cVB.tVB_BchName = oCmp.FTBchName;
                    cVB.tVB_VATInOrEx = oCmp.FTCmpRetInOrEx;
                    cVB.tVB_VatCode = oCmp.FTVatCode;
                    C_GEToBchProperty(); //*Net 63-04-07

                    //*Em 62-09-06
                    try
                    {

                        oSql.Clear();
                        oSql.AppendLine("SELECT TOP 1 FTAddTaxNo FROM TCNMAddress_L WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTAddRefCode = '" + cVB.tVB_BchCode + "' AND FTAddGrpType = '1' AND FTAddRefNo = '1'");
                        cVB.tVB_TaxID = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());

                    }
                    catch (Exception oEx) { new cLog().C_WRTxLog("cCompany", "C_GETxCompany : " + oEx.Message); }
                    //+++++++++++++++++
                    C_GETxVAtCompany();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cCompany", "C_GETxCompany : " + oEx.Message); }
            finally
            {
                oSql = null;
                oCmp = null;
            }
        }

        public void C_SETxBchCompany(string ptBchcode)
        {
            StringBuilder oSql;
            try
            {
                oSql = new StringBuilder();
                if (String.IsNullOrEmpty(ptBchcode)) return;
                oSql.AppendLine($"UPDATE TCNMComp with(rowlock) SET FTBchcode = '{ptBchcode}'");
                new cDatabase().C_GEToDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cCompany", "C_SETxBchCompany : " + oEx.Message); }
            finally
            {
                oSql = null;
            }
        }
        /// <summary>
        /// Get Shop
        /// </summary>
        public void C_GETxShpName()
        {
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT SHPL.FTShpName");
                oSql.AppendLine("FROM TCNMShop SHP WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TCNMShop_L SHPL WITH(NOLOCK) ON SHPL.FTShpCode = SHP.FTShpCode");
                oSql.AppendLine("   AND SHPL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("   AND SHPL.FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("   AND SHPL.FTBchCode = SHP.FTBchCode");
                oSql.AppendLine("WHERE SHP.FTShpCode = '" + cVB.tVB_ShpCode + "'");

                cVB.tVB_ShpName = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cCompany", "C_GETxShpName : " + oEx.Message); }
            finally
            {
                oSql = null;
            }
        }

        public void C_GEToBchProperty()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable oTblBch;
            try
            {
                oSql.AppendLine("SELECT TOP 1 BCHL.FTBchName,BCH.FTPplCode,BCH.FTBchRefID");
                oSql.AppendLine("FROM TCNMBranch BCH WITH(NOLOCK)");
                oSql.AppendLine($"INNER JOIN TCNMBranch_L BCHL ON BCH.FTBchCode=BCHL.FTBchCode AND BCHL.FNLngID={cVB.nVB_Language}");
                oSql.AppendLine($"WHERE BCH.FTBchCode = '{cVB.tVB_BchCode}'");
                oTblBch = oDB.C_GEToDataQuery(oSql.ToString());

                cVB.tVB_BchName = oTblBch.Rows[0]["FTBchName"].ToString();
                cVB.tVB_BchPriceGroup = oTblBch.Rows[0]["FTPplCode"].ToString();
                cVB.tVB_BchRefID = oTblBch.Rows[0]["FTBchRefID"].ToString(); //*Net 63-05-22
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cCompany", "C_GETxCompany : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
            }
        }

        /// <summary>
        /// Get Image Company / Branch / Shop
        /// </summary>
        /// <param name="ptCmpCode"></param>
        /// <param name="ptBchCode"></param>
        /// <param name="ptShpCode"></param>
        /// <returns></returns>
        public Image C_GEToImageLogo()
        {
            Bitmap oBitmap = null;
            string tPath = "";
            StringBuilder oSql;
            try
            {
                /*if (string.Equals(cVB.tVB_PosType, "1"))     // Store
                    tPath = Application.StartupPath + @"\AdaImage\Shop\" + cVB.tVB_ShpCode + ".png";

                if (File.Exists(tPath))
                    oBitmap = new Bitmap(tPath);
                else
                {
                    tPath = Application.StartupPath + @"\AdaImage\Branch\" + cVB.tVB_BchCode + ".png";

                    if (File.Exists(tPath))
                        oBitmap = new Bitmap(tPath);
                    else
                    {
                        tPath = Application.StartupPath + @"\AdaImage\Company\" + cVB.tVB_CmpCode + ".png";

                        if (File.Exists(tPath))
                            oBitmap = new Bitmap(tPath);
                    }
                }*/

                //*Arm 63-04-17 Comment Code
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT TOP 1 FTImgObj");
                //oSql.AppendLine("FROM TCNMImgObj IMG WITH(NOLOCK)");
                //oSql.AppendLine($"WHERE IMG.FTImgRefID = '{cVB.tVB_CmpCode}' AND FTImgTable='TCNMComp' ");
                //oSql.AppendLine($"ORDER BY FDLastUpdOn DESC");
                //tPath = new cDatabase().C_GETaDataQuery<string>(oSql.ToString()).FirstOrDefault();
                ////cVB.tVB_PathLogo = tPath;
                //if (File.Exists(tPath))
                //    oBitmap = new Bitmap(tPath);
                //+++++++++++++++
                
                oBitmap = Properties.Resources.Adasoft; //*Arm 63-04-17
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cUser", "C_GEToImageLogo : " + oEx.Message); }
            finally
            {
                tPath = null;
            }

            return oBitmap;
        }

        /// <summary>
        /// Get vat rate for company.
        /// </summary>
        public void C_GETxVAtCompany()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("SELECT TOP 1 FCVatRate");
                oSql.AppendLine("FROM TCNMVatRate WITH(NOLOCK)");
                oSql.AppendLine("WHERE FDVatStart <= GETDATE()");
                oSql.AppendLine("AND FTVatCode = '" + cVB.tVB_VatCode + "'");
                oSql.AppendLine("ORDER BY FDVatStart DESC");

                cVB.cVB_VatRate = oDB.C_GEToDataQuery<decimal>(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cCompany", "C_GETxCompany : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
            }
        }

        /// <summary>
        /// Get merchant name.
        /// </summary>
        public void C_GETxMerInfo()
        {
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 ISNULL(MERL.FTMerName ,(SELECT TOP 1 FTMerName FROM TCNMMerchant_L WITH(NOLOCK) WHERE FTMerCode = MER.FTMerCode)) AS FTMerName");
                oSql.AppendLine("FROM TCNMMerchant MER WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TCNMMerchant_L MERL WITH(NOLOCK) ON MERL.FTMerCode = MER.FTMerCode AND MERL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("WHERE MER.FTMerCode = '" + cVB.tVB_Merchart + "'");

                cVB.tVB_MerName = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());

                cVB.tVB_TaxID = "";
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 FTAddTaxNo FROM TCNMAddress_L WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTAddRefCode = '" + cVB.tVB_Merchart + "' AND FTAddGrpType = '7' AND FTAddRefNo = '1'");
                cVB.tVB_TaxID = new cDatabase().C_GEToDataQuery<string>(oSql.ToString()).ToString();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cCompany", "C_GETxMerName : " + oEx.Message); }
            finally
            {
                oSql = null;
            }
        }
    }
}
