using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using AdaPos.Class;

namespace AdaPos.Control
{
    [Guid("426C2922-D174-498C-B8B4-96181A9B2D65")]
    public partial class uVideo2ndScreen : UserControl
    {
        string tPathVideo = Environment.CurrentDirectory + "\\Videos\\";
        //string tFullPath = tPathVideo + @"\Videos\";
        public const string Separator = ",";
        public static ArrayList aSearchVideo = new ArrayList();
        public static StreamWriter sw;
        
        public uVideo2ndScreen()
        {
            InitializeComponent();
        }

        private void uVideo2ndScreen_Load(object sender, EventArgs e)
        {
            cW_VideoRead();
            cW_CreatePlayListVideo();
           
            owm2ndScreen.settings.setMode("loop", true);
            //*Net เริ่มต้น play วิดีโอเมื่อ Form2ndScreen แสดง
            if (cVB.nVB_Check2nd == 1) owm2ndScreen.Ctlcontrols.play();
            else owm2ndScreen.Ctlcontrols.stop();
        }

        private void cW_VideoRead()
        {
            string tFormatIni = "formats.ini";
            string tTemp;
            int index;
            bool bCheckFile = File.Exists(tPathVideo + tFormatIni);
            if (bCheckFile)
            {
                FileStream fs = new FileStream(tPathVideo + tFormatIni, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                using (fs)
                {
                    string fileLine = string.Empty;
                    try
                    {
                        while ((fileLine = sr.ReadLine()) != null)
                        {
                            if (fileLine == string.Empty)
                                continue;

                            if (fileLine[0] == '#')     // Process only uncommented lines
                                continue;

                            index = 0;
                            tTemp = cW_Parse(fileLine, ref index);
                            switch (tTemp.ToUpper())
                            {
                                /*case "AF":
                                    sTemp = Parse(fileLine, ref index);
                                    while (string.IsNullOrEmpty(sTemp) == false)
                                    {
                                        searchAudio.Add(sTemp);

                                        sTemp = Parse(fileLine, ref index);
                                    }
                                    break;*/
                                case "VF":
                                    tTemp = cW_Parse(fileLine, ref index);
                                    while (string.IsNullOrEmpty(tTemp) == false)
                                    {
                                        aSearchVideo.Add(tTemp);

                                        tTemp = cW_Parse(fileLine, ref index);
                                    }
                                    break;
                                default:
                                    break;

                            }   // End switch
                        }       // End while
                    }           // End Try
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                    finally
                    {
                        // Close File handles
                        sr.Close();
                        fs.Close();
                    }
                }           // End Using
            }
            else
            {
                Directory.CreateDirectory("Videos");
                FileStream fs = File.Create(tPathVideo + tFormatIni);
                sw = new StreamWriter(fs);

                try
                {
                    sw.WriteLine("#AudioFormats");    // File Header
                    sw.WriteLine("AF,.wma,.wax,.cda,.wav,.mp3,.m3u,.mid,.midi,.rmi,.aif");
                    sw.WriteLine("AF,.aifc,.aiff,.au,.snd,.ra,.rm,.ram,.rpm,.rmm,.rnx");
                    sw.WriteLine("");
                    sw.WriteLine("#VideoFormats");
                    sw.WriteLine("VF,.asf,.asx,.wm,.wmx,.wmd,.wmz,.wmv,.wvx,.avi");
                    sw.WriteLine("VF,.mpeg,.mpg,.mpe,.m1v,.mp2,.mpv2,.mp2v,.mpa,.mp4");
                    sw.WriteLine("VF,.dvr-ms,.rm,.ram,.rpm,.rmm,.rnx");


                    //MessageBox.Show(sFileName, "Create Playlis");

                }
                catch (Exception ex)
                {
                   
                }
                finally
                {
                    sw.Close();
                    fs.Close();
                }
            }
            
        }


        private void cW_CreatePlayListVideo()
        {
            // Open a file to write
            string sFileName = "My_Videos.wpl";
            FileStream fs = File.Create(tPathVideo + sFileName);
            sw = new StreamWriter(fs);

            try
            {
                sw.WriteLine("<?wpl version=\"1.0\"?>");    // File Header
                sw.WriteLine("<smil>");                     // Start of File Tag

                sw.WriteLine("\t<head>");                     // Playlist File Header Information Start Tag
                sw.WriteLine("\t\t<meta name=\"Generator\" content=\"Microsoft Windows Media Player -- 10.0.0.4036\"/>");
                sw.WriteLine("\t\t<author> Bersama Jaya Teknik </author>");
                sw.WriteLine("\t\t<title> Playlist Video </title>");
                sw.WriteLine("\t</head>");                    // Playlist File Header Information End Tag

                sw.WriteLine("\t<body>");                     // Start of body Tag
                sw.WriteLine("\t\t<seq>");                      // Start of filelist Tag


                // Get Directory's File list and Add files
                DirectoryListing(tPathVideo);

                sw.WriteLine("\t\t</seq>");                      // End of filelist Tag
                sw.WriteLine("\t</body>");                    // End of body Tag
                sw.WriteLine("</smil>");                    // End of File Tag

                sFileName = sFileName + " Successfully created.";

                //MessageBox.Show(sFileName, "Create Playlis");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Create Playlist: Error");

                sFileName = sFileName + " Unsuccessful.";

                MessageBox.Show(sFileName, "Create Playlis");
            }
            finally
            {
                sw.Close();
                fs.Close();
            }

            owm2ndScreen.URL = tPathVideo + "My_Videos.wpl";
            owm2ndScreen.Ctlcontrols.play();
        }
        private string cW_Parse(string line, ref int index)
        {
            if (index == -1)
            {
                return "";
            }

            //Bypass the very first separator and start the string at the start of the data
            //if (index == 0)
            //{
            //    index = line.IndexOf(Separator, 0);
            //    index += Separator.Length;
            //}

            //Get the next separator position
            int tempNo = line.IndexOf(Separator, index);

            string sTemp = string.Empty;
            //Get the data in between the separators
            if (tempNo == -1)
            {
                sTemp = line.Substring(index);
                index = tempNo;
            }
            else
            {
                sTemp = line.Substring(index, tempNo - index);
                //Set the index to the start of the next set of data
                index = tempNo + Separator.Length;
            }

            // Remove Double quotes in a string.
            sTemp = sTemp.Trim('"', ' ');

            return sTemp.Trim();
        }


        private int DirectoryListing(string sPath)
        {
            int iFileCount = 0;

            if (string.IsNullOrEmpty(sPath) == true)
            {
                MessageBox.Show("Directory not specified. Please select Valid directory.");
                return iFileCount;
            }

            //            if (Directory.Exists(sPath) == false)
            //            {
            //                MessageBox.Show("Directory not exist. Please select Valid directory.");
            //                return iFileCount;
            //            }

            ArrayList searchList = new ArrayList();

            /*switch (cmbType.SelectedIndex)
            {
                case 0:                                         // Audio files
                    searchList = searchAudio;
                    break;
                case 1:                                         // Video files
                */
            searchList = aSearchVideo;
            /*   break;
           case 2:                                         // All Audio Video Files
               searchList = searchAudio;
               searchList.AddRange(searchVideo);
               break;
           case 3:                                         // All files
               //searchList = null;
               break;
           default:
               //searchList = null;
               break;
       }*/

            if (File.Exists(sPath))
            {
                // This path is a file
                iFileCount = ProcessFile(sPath, searchList);
            }
            else if (Directory.Exists(sPath))
            {
                // This path is a directory
                iFileCount = ProcessDirectory(sPath, searchList);
            }
            else
            {
                MessageBox.Show(sPath + " is not a valid file or directory.");
            }

            return iFileCount;

        }
        public static int ProcessFile(string fileName, ArrayList searchList)
        {
            string fileLine;
            string sFileExt;
            int iFileCount = 0;

            if (string.IsNullOrEmpty(fileName) == true)
                return iFileCount;

            if (searchList.Count != 0)                     // If it's not All files
            {
                sFileExt = fileName.Substring(fileName.IndexOf('.'));
                if (searchList.IndexOf(sFileExt) == -1)
                    return iFileCount;
            }

            fileLine = "\t\t\t<media src=\"";
            fileLine = fileLine + fileName + "\"/>";
            sw.WriteLine(fileLine);

            return (++iFileCount);
        }

        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static int ProcessDirectory(string targetDirectory, ArrayList searchList)
        {
            int iFileCount = 0;

            if (string.IsNullOrEmpty(targetDirectory) == true)
                return iFileCount;

            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            if (fileEntries.Length > 0)
            {
                foreach (string fileInfo in fileEntries)
                    iFileCount += ProcessFile(fileInfo, searchList);
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            if (subdirectoryEntries.Length > 0)
            {
                foreach (string subdirectory in subdirectoryEntries)
                    ProcessDirectory(subdirectory, searchList);
            }
            return iFileCount;
        }

    }
}
