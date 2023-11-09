
using Microsoft.Office.Interop.Excel;
using Microsoft.SqlServer.Server;
using OfficeOpenXml;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;

namespace Model_Helper_famework
{
    public class Extention
    {
        #region สร้างภาพ jigsaw
        public string Jigsaw(int divide)
        {
            Random random = new Random();
            string path = string.Empty;
            int number_from_random = random.Next(0, 10);
            if (!Directory.Exists("D:\\Api\\Image\\"))
            {
                Directory.CreateDirectory("D:\\Api\\Image\\");
            }
            using (Bitmap image = new Bitmap($@"D:\Api\Image\{number_from_random}.jpg"))
            {
                int piecesWight = image.Width / divide;
                int piecesHeight = image.Width / divide;
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        System.Drawing.Rectangle piecseRetangle = new System.Drawing.Rectangle(i * piecesWight, j * piecesHeight, piecesWight, piecesHeight);
                        using (Bitmap picese = new Bitmap(piecesWight, piecesWight))
                        {
                            using (Graphics g = Graphics.FromImage(picese))
                            {
                                g.DrawImage(image, new System.Drawing.Rectangle(0, 0, piecesWight, piecesWight), piecseRetangle, GraphicsUnit.Pixel);
                            }
                            path += "http://172.21.140.104:8084/imageOutput/piece_" + number_from_random.ToString() + "_i" + i + "_j" + j + ".jpg" + ";";
                            picese.Save("D:\\Api\\imageOutput\\piece_" + number_from_random.ToString() + "_i" + i + "_j" + j + ".jpg");
                        }
                    }
                }
            }
            return path;
        }
        #endregion
        #region Log 
        public void Log(string message)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~\\bin") + "\\InformationLog_Sql_And_Event\\";
                if (!Directory.Exists(path))

                {
                    Directory.CreateDirectory(path);
                }
                using (StreamWriter steam = new StreamWriter(path + DateTime.Now.ToString("yyyyMMdd") + ".txt", true))
                {

                    steam.WriteLine(": " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ": = " + message);
                }
            }
            catch
            {

            }


        }

        public void LogSql(string message)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~\\bin") + "\\Information_sql\\";
                if (!Directory.Exists(path))

                {
                    Directory.CreateDirectory(path);
                }
                using (StreamWriter steam = new StreamWriter(path + DateTime.Now.ToString("yyyyMMdd") + ".txt", true))
                {

                    steam.WriteLine(": " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ": = " + message);
                }
            }
            catch
            {

            }


        }

        public void Log_Save_information(string Cookie_AgenId, string Datetimes)
        {
            try
            {

                string path = HttpContext.Current.Server.MapPath("~\\bin") + "\\Log_Save_AgenID\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
                if (!Directory.Exists(path))

                {
                    Directory.CreateDirectory(path);
                }
                using (StreamWriter steam = new StreamWriter(path + "_" + Cookie_AgenId + "_" + Datetimes + ".txt", true))
                {

                    steam.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "=" + Cookie_AgenId + ",");
                }
            }
            catch
            {

            }


        }

        public string Log_Get_information(string Cookie_AgenId, string Datetimes)
        {
            string messages = string.Empty;
            try
            {

                string path = HttpContext.Current.Server.MapPath("~\\bin") + "\\Log_Save_AgenID\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";

                using (StreamReader steam = new StreamReader(path + "_" + Cookie_AgenId + "_" + Datetimes + ".txt", true))
                {

                    messages = steam.ReadToEnd();
                }
                return messages;
            }
            catch (Exception ex)
            {
                LogSql(ex.Message.ToString());
                return string.Empty;
            }
        }

        public string Log_Get_information_SaveData_And_Edit(string result = "", string type = "", string Cookie_AgenId = "", string Datetimes = "", ViewModel Model = null)

        {
            string messages = string.Empty;
            try
            {

                string path = HttpContext.Current.Server.MapPath("~\\bin") + "\\SaveData_And_Edit\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
                if (!Directory.Exists(path))

                {
                    Directory.CreateDirectory(path);
                }
                using (StreamWriter steam = new StreamWriter(path + "_" + Cookie_AgenId + "_" + Datetimes + ".txt", true))
                {
                    steam.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " , Result = " + result + " , Type = " + type + " ," + "=" + $@" txtTel_No :{Model.txtTel_No} , txtName : {Model.txtName} , txtSName : {Model.txtSName} , cboDate : {Model.cboDate} , cboMouth : {Model.cboMouth} , txtYear : {Model.txtYear} , cboSex : {Model.cboSex} , cboStatus : {Model.cboStatus.ToString().Replace(" ", "")} , cbocity :{Model.cbocity}");

                }


                return messages;
            }
            catch (Exception ex)
            {
                LogSql(ex.Message.ToString());
                return string.Empty;
            }


        }

        public int Log_Get_information_lenght(string Cookie_AgenId, string Datetimes)
        {
            int count = 0;
            try
            {

                string path = HttpContext.Current.Server.MapPath("~\\bin") + "\\Log_Save_AgenID\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";

                using (StreamReader steam = new StreamReader(path + "_" + Cookie_AgenId + "_" + Datetimes + ".txt", true))
                {

                    count = steam.ReadToEnd().Split(',').Length - 1;
                }
                return count;

            }
            catch (Exception ex)
            {
                LogSql(ex.Message.ToString());
                return count = 0;
            }


        }

        #endregion
        #region ส่งออก Excel
        public void ExportExcel(System.Data.DataTable dt, string sheetname = "Sheet1", string path = "D:\\Excel\\")
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage(path + DateTime.Now.ToString("yyyy_MM-dd_mm_ss") + ".xlsx"))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetname);
                    ws.Cells["A1"].LoadFromDataTable(dt, true);
                    pck.Save();
                }
            }
            catch (Exception ex)
            {
                Error_providers.Instances.CustomsExceptions(ex);
            }

        }
        #endregion
        #region นำเข้า Excel
        public System.Data.DataTable ImportExcel(string pathfile, string sheetName = "Sheet1", bool hasHeader = true)
        {
            var dt = new System.Data.DataTable();
            try
            {

                var fi = new FileInfo(pathfile);
                // Check if the file exists
                if (!fi.Exists)
                    throw new Exception("File " + pathfile + " Does Not Exists");

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                var xlPackage = new ExcelPackage(fi);
                // get the first worksheet in the workbook
                var worksheet = xlPackage.Workbook.Worksheets[sheetName];

                dt = worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column].ToDataTable(c =>
                {
                    c.FirstRowIsColumnNames = true;
                });

                return dt;
            }
            catch (Exception ex)
            {
                Error_providers.Instances.CustomsExceptions(ex);
                return dt;
            }

        }
        #endregion
        #region เลบอภาพ
        public void ImageBlur(string imagePath)
        {
            // โหลดภาพ
            Bitmap originalImage = new Bitmap(imagePath);

            // สร้าง Bitmap สำหรับภาพที่เบลอ
            Bitmap blurredImage = new Bitmap(originalImage.Width, originalImage.Height);

            int blurRadius = 5; // ปรับค่านี้เพื่อควบคุมความเบลอ

            // ใช้ฟังก์ชันเบลอภาพ
            ApplyBoxBlur(originalImage, blurredImage, blurRadius);

            // บันทึกภาพที่เบลอลงไฟล์
            blurredImage.Save("blurred_image.jpg");

            // ปิด Bitmap
            originalImage.Dispose();
            blurredImage.Dispose();
        }

        static void ApplyBoxBlur(Bitmap source, Bitmap destination, int radius)
        {
            for (int x = 0; x < source.Width; x++)
            {
                for (int y = 0; y < source.Height; y++)
                {
                    int red = 0, green = 0, blue = 0, count = 0;

                    for (int i = -radius; i <= radius; i++)
                    {
                        for (int j = -radius; j <= radius; j++)
                        {
                            int newX = x + i;
                            int newY = y + j;

                            if (newX >= 0 && newX < source.Width && newY >= 0 && newY < source.Height)
                            {
                                Color pixel = source.GetPixel(newX, newY);
                                red += pixel.R;
                                green += pixel.G;
                                blue += pixel.B;
                                count++;
                            }
                        }
                    }

                    red /= count;
                    green /= count;
                    blue /= count;

                    Color blurredColor = Color.FromArgb(red, green, blue);
                    destination.SetPixel(x, y, blurredColor);
                }
            }


        }
        #endregion
        #region ฟังก์ชั้น คัดลอกและวางรูปภาพลงในสเต็ปโดยตรง

        private PictureBox pictureBox;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.Button pasteButton;

        public void ImageCopyPasteApp()
        {

            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D
            };

            copyButton = new System.Windows.Forms.Button
            {
                Text = "Copy Image",
                Dock = DockStyle.Top
            };

            pasteButton = new System.Windows.Forms.Button
            {
                Text = "Paste Image",
                Dock = DockStyle.Top
            };

        }
        private void CopyButton_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image != null)
            {
                Clipboard.SetImage(pictureBox.Image);
            }
        }

        private void PasteButton_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                pictureBox.Image = Clipboard.GetImage();
            }
        }
        #endregion
        #region เปลี่ยนจาก Datatable เป็น object
        public List<T> DataTableTooject<T>(System.Data.DataTable dt) where T : new()
        {
            List<T> modelList = new List<T>();
            int i = 0;
            foreach (var dt2 in dt.Rows)
            {
                T model = new T();
                foreach (var property in model.GetType().GetProperties())
                {
                    foreach (var dt3 in dt.Columns)
                        if ((property.Name.Contains(dt.Columns[i].ColumnName)))
                        {
                            property.SetValue(model, dt.Rows[i][dt.Columns[i].ColumnName], null);
                        }
                }
                modelList.Add(model);
                i++;
            }
            return modelList;
        }
        #endregion
        #region เปลี่ยนจาก object  เป็น Datatable
        public System.Data.DataTable objectToDataTable<T>(List<T> list) where T : new()
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            foreach (var property in list.GetType().GetProperties())
            {
                dt.Columns.Add(property.Name);
            }
            foreach (var item in list)
            {
                dt.Rows.Add(item);
            }
            return dt;
        }
        #endregion
        #region Socket
        private HttpListener _httpListener;
        private async Task AcceptWebSocketAsync(HttpListenerContext context)
        {
            if (context.Request.IsWebSocketRequest)
            {
                WebSocketContext webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);

                using (WebSocket webSocket = webSocketContext.WebSocket)
                {
                    byte[] receiveBuffer = new byte[1024];
                    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

                    while (!result.CloseStatus.HasValue)
                    {
                        // Handle received data
                        string receivedData = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
                        Console.WriteLine($"Received data: {receivedData}");

                        // Send a response
                        string responseMessage = "Server received: " + receivedData;
                        byte[] responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                        await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);

                        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                    }
                }
            }
        }
        #endregion
        #region เริ่ม Socket
        public async Task StartAsync()
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add("http://localhost:8082/");
            _httpListener.Start();

            while (true)
            {
                HttpListenerContext context = await _httpListener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    await AcceptWebSocketAsync(context);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }
        #endregion
    }
}