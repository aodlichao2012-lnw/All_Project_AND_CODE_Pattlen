using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace AdaPos.Class
{
    public class cUser
    {
        public cUser()
        {

        }

        /// <summary>
        /// Get user cashier
        /// </summary>
        public List<cmlTCNMUser> C_GETaUser()
        {
            StringBuilder oSql;
            List<cmlTCNMUser> aoUser = new List<cmlTCNMUser>();

            try
            {
                oSql = new StringBuilder();
                //oSql.AppendLine("SELECT USR.FTUsrCode, ISNULL(USRL.FTUsrName,(SELECT TOP 1 FTUsrName FROM TCNMUser_L WITH(NOLOCK) WHERE FTUsrCode = USR.FTUsrCode)) AS FTUsrName, USL.FTUsrLoginPwd AS FTUsrPwd, USR.FTRolCode, UGP.FTShpCode, UGP.FTUsrStaShop, ROL.FNRolLevel ");
                oSql.AppendLine("SELECT USR.FTUsrCode, USL.FTUsrLogin AS FTUsrName, USL.FTUsrLoginPwd AS FTUsrPwd, USR.FTRolCode, UGP.FTShpCode, UGP.FTUsrStaShop, ROL.FNRolLevel ");   //*Em 62-09-10
                oSql.AppendLine("FROM TCNMUser USR WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TCNMUser_L USRL WITH(NOLOCK) ON USRL.FTUsrCode = USR.FTUsrCode");
                oSql.AppendLine("   AND USRL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("INNER JOIN TCNTUsrGroup UGP WITH(NOLOCK) ON UGP.FTUsrCode = USR.FTUsrCode");
                oSql.AppendLine("INNER JOIN TCNMUsrRole ROL WITH(NOLOCK) ON ROL.FTRolCode = USR.FTRolCode");    //*Em 62-09-03
                oSql.AppendLine("INNER JOIN TCNMUsrLogin USL WITH(NOLOCK) ON USR.FTUsrCode = USL.FTUsrCode");   //*Em 62-09-10

                //switch (cVB.tVB_PosType)
                //{
                //    case "1": // Store
                //        oSql.AppendLine("WHERE UGP.FTBchCode = '" + cVB.tVB_BchCode + "'");
                //        oSql.AppendLine("AND  FTUsrStaShop = '3'");
                //        break;

                //    case "2": // Cashier
                //        oSql.AppendLine("WHERE FTUsrStaShop= '1' ");
                //        oSql.AppendLine("OR (FTUsrStaShop = '2' AND UGP.FTBchCode = '" + cVB.tVB_BchCode + "')");
                //        break;
                //}
                oSql.AppendLine("   WHERE (FTUsrStaShop = '1' OR (FTUsrStaShop = '2' AND UGP.FTBchCode = '" + cVB.tVB_BchCode + "') OR (FTUsrStaShop = '3' AND UGP.FTShpCode = '" + cVB.tVB_ShpCode + "'))");   //*Em 62-06-19
                oSql.AppendLine("   AND (CONVERT(VARCHAR(19),GETDATE(),121) BETWEEN CONVERT(VARCHAR(19),USL.FDUsrPwdStart,121) AND CONVERT(VARCHAR(19),USL.FDUsrPwdExpired,121)) "); //*Em 62-09-10
                oSql.AppendLine("ORDER BY USR.FTUsrCode");

                aoUser = new cDatabase().C_GETaDataQuery<cmlTCNMUser>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cUser", "C_GETaUser : " + oEx.Message); }
            finally
            {
                oSql = null;
                new cSP().SP_CLExMemory();
            }

            return aoUser;
        }

        /// <summary>
        /// Get Last 3 user
        /// </summary>
        /// <returns></returns>
        public List<cmlTCNMUser> C_GETaLastUser(string ptTypePwd)
        {
            StringBuilder oSql;
            List<cmlTCNMUser> aoUser = new List<cmlTCNMUser>();

            try
            {
                oSql = new StringBuilder();
                //oSql.AppendLine("SELECT TOP(3) FTUsrCode, FTUsrName, FDSdtDSignIn, FTSdtTSignIn");
                oSql.AppendLine("SELECT TOP(3) FTUsrCode, FTUsrName, FDSdtSignIn"); //*Em 62-06-10
                oSql.AppendLine("FROM");
                oSql.AppendLine("(");
                //oSql.AppendLine("   SELECT DISTINCT USR.FTUsrCode, USR.FTUsrName, MAX(SDT.FDSdtDSignIn) AS FDSdtDSignIn,");
                //oSql.AppendLine("       MAX(SDT.FTSdtTSignIn) AS FTSdtTSignIn");
                oSql.AppendLine("   SELECT DISTINCT USRL.FTUsrCode, USRL.FTUsrLogin AS FTUsrName, MAX(SDT.FDSdtSignIn) AS FDSdtSignIn");   //*Em 62-06-10
                oSql.AppendLine("   FROM TPSTShiftDT SDT WITH(NOLOCK)");
                //oSql.AppendLine("   INNER JOIN TCNMUser_L USR WITH(NOLOCK) ON USR.FTUsrCode = SDT.FTUsrCode");
                //oSql.AppendLine("       AND USR.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("   INNER JOIN TCNTUsrGroup UGP WITH(NOLOCK) ON SDT.FTUsrCode = UGP.FTUsrCode");

                //*Em 62-09-12
                oSql.AppendLine("   INNER JOIN TCNMUsrLogin USRL WITH(NOLOCK) ON USRL.FTUsrCode = SDT.FTUsrCode AND USRL.FTUsrLogType = '"+ ptTypePwd +"'");
                //++++++++++++++

                //switch (cVB.tVB_PosType)
                //{
                //    case "1": // Store
                //        oSql.AppendLine("   WHERE UGP.FTBchCode = '" + cVB.tVB_BchCode + "'");
                //        oSql.AppendLine("   AND FTUsrStaShop = '3'");
                //        break;

                //    case "2": // Cashier
                //        oSql.AppendLine("   WHERE FTUsrStaShop= '1' ");
                //        oSql.AppendLine("   OR (FTUsrStaShop = '2' AND UGP.FTBchCode = '" + cVB.tVB_BchCode + "')");
                //        break;
                //}
                //*Em 62-06-10
                oSql.AppendLine("   WHERE (FTUsrStaShop = '1' OR (FTUsrStaShop = '2' AND UGP.FTBchCode = '"+ cVB.tVB_BchCode +"') OR (FTUsrStaShop = '3' AND UGP.FTShpCode = '"+ cVB.tVB_ShpCode +"'))");
                //++++++++++++

                oSql.AppendLine("   GROUP BY USRL.FTUsrCode, USRL.FTUsrLogin");
                oSql.AppendLine(") LastUsr");
                //oSql.AppendLine("ORDER BY FDSdtDSignIn DESC, FTSdtTSignIn DESC");
                oSql.AppendLine("ORDER BY FDSdtSignIn DESC");   //*Em 62-06-10

                aoUser = new cDatabase().C_GETaDataQuery<cmlTCNMUser>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cUser", "C_GETaLastUser : " + oEx.Message); }
            finally
            {
                oSql = null;
                new cSP().SP_CLExMemory();
            }

            return aoUser;
        }

        /// <summary>
        /// Check User Role
        /// </summary>
        /// <param name="ptRolCode"></param>
        /// <returns></returns>
        public string C_CHKtUserRole(string ptRolCode, string ptScreen)
        {
            StringBuilder oSql;
            string tUfrRef = "", tFuncCode;

            try
            {
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(ptScreen);

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT UFR.FTUfrRef");
                oSql.AppendLine("FROM TCNTUsrFuncRpt UFR WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TPSMFuncHD FHD WITH(NOLOCK) ON FHD.FTGhdCode = FTUfrGrpRef ");

                switch (cVB.tVB_PosType)
                {
                    case "1": // Store
                        oSql.AppendLine("   AND FHD.FTGhdApp = 'PS'");
                        break;

                    case "2": // Cashier
                        oSql.AppendLine("   AND FHD.FTGhdApp = 'FC'");
                        break;
                }

                oSql.AppendLine("   AND FHD.FTKbdScreen = '" + ptScreen + "'");
                oSql.AppendLine("INNER JOIN TPSMFuncDT FDT WITH(NOLOCK) ON FDT.FTGhdCode = FHD.FTGhdCode ");
                oSql.AppendLine("   AND FDT.FTSysCode = UFR.FTUfrRef");
                oSql.AppendLine("WHERE UFR.FTRolCode = '" + ptRolCode + "'");
                oSql.AppendLine("AND UFR.FTUfrType = '1'");
                oSql.AppendLine("AND FDT.FTSysCode = '" + tFuncCode + "'");

                tUfrRef = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cUser", "C_CHKtUserRole : " + oEx.Message); }
            finally
            {
                oSql = null;
                tFuncCode = null;
                ptRolCode = null;
                ptScreen = null;
                new cSP().SP_CLExMemory();
            }

            return tUfrRef;
        }

        /// <summary>
        /// Check user role with role code screen name and function code.
        /// </summary>
        /// 
        /// <param name="ptRolCode">Role code.</param>
        /// <param name="ptScreen">Screen name.</param>
        /// <param name="ptFuncCode">Function code.</param>
        /// 
        /// <returns>
        /// Code number.
        /// <para> 0: not permission.</para>
        /// <para> 1: allowed.</para>
        /// <para> 800: data not found or role not set.</para>
        /// <para> 900: function process error.</para>
        /// </returns>
        /// 
        /// <remarks>*[AnUBiS][][2019-01-08] - create function.</remarks>
        //public string C_CHKtUserRole(string ptRolCode, string ptScreen, string ptFuncCode)
        public string C_CHKtUserRole(int pnRolLevel, string ptScreen, string ptFuncCode)    //*Em62-09-03
        {
            StringBuilder oSql;
            string tUfrStaAlw, tResCode;

            try
            {
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT ISNULL(UFR.FTUfrStaAlw, '') AS FTUfrStaAlw");
                //oSql.AppendLine("FROM TCNTUsrFuncRpt UFR WITH(NOLOCK)");
                //oSql.AppendLine("INNER JOIN TPSMFuncHD FHD WITH(NOLOCK) ON FHD.FTGhdCode = FTUfrGrpRef ");

                //switch (cVB.tVB_PosType)
                //{
                //    case "1": // Store
                //        oSql.AppendLine("   AND FHD.FTGhdApp = 'POS'");
                //        break;

                //    case "2": // Cashier
                //        oSql.AppendLine("   AND FHD.FTGhdApp = 'FC'");
                //        break;
                //    default:
                //        oSql.AppendLine("   AND FHD.FTGhdApp = 'POS'");
                //        break;
                //}

                //oSql.AppendLine("   AND FHD.FTKbdScreen = '" + ptScreen + "'");
                //oSql.AppendLine("INNER JOIN TPSMFuncDT FDT WITH(NOLOCK) ON FDT.FTGhdCode = FHD.FTGhdCode ");
                //oSql.AppendLine("   AND FDT.FTSysCode = UFR.FTUfrRef");
                //oSql.AppendLine("WHERE UFR.FTRolCode = '" + ptRolCode + "'");
                //oSql.AppendLine("AND UFR.FTUfrType = '1'");
                //oSql.AppendLine("AND FDT.FTSysCode = '" + ptFuncCode + "'");

                //*Em 62-09-03
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT CASE WHEN "+ pnRolLevel +" >= DT.FNGdtFuncLevel THEN '1' ELSE '0' END AS FTGdtAlwUse");
                oSql.AppendLine("FROM TPSMFuncDT DT WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TPSMFuncHD HD WITH(NOLOCK) ON HD.FTGhdCode = DT.FTGhdCode");
                switch (cVB.tVB_PosType)
                {
                    case "1": // Store
                        oSql.AppendLine("WHERE HD.FTGhdApp = 'PS'");
                        break;

                    case "2": // Cashier
                        oSql.AppendLine("WHERE HD.FTGhdApp = 'FC'");
                        break;
                    default:
                        oSql.AppendLine("WHERE HD.FTGhdApp = 'PS'");
                        break;
                }
                oSql.AppendLine("AND HD.FTKbdScreen = '" + ptScreen + "'");
                oSql.AppendLine("AND DT.FTSysCode = '" + ptFuncCode + "'");
                //+++++++++++++++

                tUfrStaAlw = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());

                switch (tUfrStaAlw)
                {
                    case null:
                        // data not found.
                        tResCode = "800";
                        break;
                    case "1":
                        // allowed.
                        tResCode = "1";
                        break;
                    default:
                        // not permission.
                        tResCode = "0";
                        break;
                }

                return tResCode;
            }
            catch (Exception oExn)
            {
                tResCode = "900";
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);

                return tResCode;
            }
            finally
            {
                oSql = null;
                tUfrStaAlw = null;
                tResCode = null;
                ptScreen = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get username
        /// </summary>
        /// <returns></returns>
        public string C_GETtUsername()
        {
            StringBuilder oSql;
            string tUsername = "";

            try
            {
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTUsrName ");
                //oSql.AppendLine("FROM TCNMUser_L");
                //oSql.AppendLine("WHERE FTUsrCode = '" + cVB.tVB_UsrCode + "'");
                //oSql.AppendLine("AND FNLngID = " + cVB.nVB_Language);

                //tUsername = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
                tUsername = cVB.tVB_UsrName;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cUser", "C_GETtUsername : " + oEx.Message); }
            finally
            {
                oSql = null;
                new cSP().SP_CLExMemory();
            }

            return tUsername;
        }

        /// <summary>
        /// Get Image User
        /// </summary>
        /// <param name="ptUsrCode"></param>
        /// <returns></returns>
        public Image C_GEToImageUsr(string ptUsrCode, string ptTable)
        {
            Bitmap oBitmap = Properties.Resources.DefaultUser_256;
            string tPath;
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();

            try
            {
                //tPath = Directory.GetParent(Application.StartupPath) + @"\AdaImage\Person\" + ptUsrCode + ".png";
                oSql.AppendLine("SELECT FTImgObj FROM TCNMImgPerson WITH(NOLOCK) WHERE FTImgTable = '"+ ptTable + "'  AND FTImgRefID = '"+ ptUsrCode +"'"); //*Em 62-09-03
                tPath = oDB.C_GEToDataQuery<String>(oSql.ToString());   //*Em 62-09-03

                if (File.Exists(tPath))
                    oBitmap = new Bitmap(tPath);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cUser", "C_GEToImageUsr : " + oEx.Message); }
            finally
            {
                ptUsrCode = null;
                tPath = null;
            }

            return oBitmap;
        }

    }
}
