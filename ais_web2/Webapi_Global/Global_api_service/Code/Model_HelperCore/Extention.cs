using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_HelperCore
{
    public class Extention
    {
        public string Jigsaw()
        {
            Random random = new Random();
            string path = string.Empty;
            int number_from_random = random.Next(0, 10);
            using (Bitmap image = new Bitmap($@"D:\Api\Image\{number_from_random}.jpg"))
            {
                int piecesWight = image.Width / 2;
                int piecesHeight = image.Width / 2;
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        Rectangle piecseRetangle = new Rectangle(i * piecesWight, j * piecesHeight, piecesWight, piecesHeight);
                        using (Bitmap picese = new Bitmap(piecesWight, piecesWight))
                        {
                            using (Graphics g = Graphics.FromImage(picese))
                            {
                                g.DrawImage(image, new Rectangle(0, 0, piecesWight, piecesWight), piecseRetangle, GraphicsUnit.Pixel);
                            }
                            path += "http://172.21.140.104:8084/imageOutput/piece_" + number_from_random.ToString() + "_i" + i + "_j" + j + ".jpg" + ";";
                            picese.Save("D:\\Api\\imageOutput\\piece_" + number_from_random.ToString() + "_i" + i + "_j" + j + ".jpg");
                        }
                    }
                }
            }
            return path;
        }

    }
}