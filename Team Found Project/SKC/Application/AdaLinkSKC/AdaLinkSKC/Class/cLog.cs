using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaLinkSKC.Class
{
    class cLog
    {
        public void C_PRCxLog(string ptForm, string ptFunction)
        {
            string tPath;
            string tFileName;
            try
            {

                #region Check Directory Log

                tPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                if (!System.IO.Directory.Exists(tPath + @"\xLog"))
                    System.IO.Directory.CreateDirectory(tPath + @"\xLog");

                #endregion

                #region Create File Name Log

                tFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                tPath += @"\xLog\" + tFileName;

                #endregion

                #region Check File in Log

                if (!System.IO.File.Exists(tPath))
                    System.IO.File.Create(tPath).Dispose();

                #endregion

                #region Write Data To File Log

                using (System.IO.StreamWriter oOutputFile = new System.IO.StreamWriter(tPath, true))
                {
                    oOutputFile.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : Form (" + ptForm + ") > " + ptFunction);
                    oOutputFile.Dispose();
                }

                #endregion

            }
            catch (Exception oEx) { System.Diagnostics.Debug.WriteLine(oEx.ToString()); }
            finally
            {
                tPath = null;
            }
        }

        public void C_PRCxLogError(string ptFileName, string ptDetail)
        {
            string tPath;
            string tFileName;
            try
            {

                #region Check Directory Log

                //tPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                if (!System.IO.Directory.Exists(cVB.tVB_PathIN + @"\" + cVB.tVB_MasBackUP + @"\Error"))
                    System.IO.Directory.CreateDirectory(cVB.tVB_PathIN + @"\" + cVB.tVB_MasBackUP + @"\Error");

                #endregion

                #region Create File Name Log
                tPath = cVB.tVB_PathIN + @"\" + cVB.tVB_MasBackUP + @"\Error\";
                tFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                tPath +=  tFileName;

                #endregion

                #region Check File in Log

                if (!System.IO.File.Exists(tPath))
                    System.IO.File.Create(tPath).Dispose();

                #endregion

                #region Write Data To File Log

                using (System.IO.StreamWriter oOutputFile = new System.IO.StreamWriter(tPath, true))
                {
                    oOutputFile.WriteLine("File Name : " + ptFileName + " > " + ptDetail);
                    oOutputFile.Dispose();
                }

                #endregion

            }
            catch (Exception oEx) { System.Diagnostics.Debug.WriteLine(oEx.ToString()); }
            finally
            {
                tPath = null;
            }
        }

        public void C_PRCxLogMonitor(string ptFunction, string ptDetail)
        {
            string tPath;
            string tFileName;
            try
            {
                if (cVB.bVB_OpenLogMonitor == true)
                {

                    #region Check Directory Log

                    tPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    if (!System.IO.Directory.Exists(tPath + @"\xLog"))
                        System.IO.Directory.CreateDirectory(tPath + @"\xLog");

                    #endregion

                    #region Create File Name Log

                    tFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    tPath += @"\xLog\" + tFileName;

                    #endregion

                    #region Check File in Log

                    if (!System.IO.File.Exists(tPath))
                        System.IO.File.Create(tPath).Dispose();

                    #endregion

                    #region Write Data To File Log

                    using (System.IO.StreamWriter oOutputFile = new System.IO.StreamWriter(tPath, true))
                    {
                        oOutputFile.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : Form (" + ptFunction + ") > " + ptDetail);
                        oOutputFile.Dispose();
                    }

                    #endregion
                }
                else
                {
                    return;
                }
            }
            catch (Exception oEx) { System.Diagnostics.Debug.WriteLine(oEx.ToString()); }
            finally
            {
                tPath = null;
            }
        }
        //public void C_PRCxLogError()
        //{

        //}
    }
}
