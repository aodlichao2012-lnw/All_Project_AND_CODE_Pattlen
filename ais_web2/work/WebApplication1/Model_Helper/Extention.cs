using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_Helper
{
    public class Extention
    {
        Guid Guid { get; set; }
        //Capcha jigsaw
        public string Jigsaw()
        {
            Random random = new Random(10);
            random.Next();
            using (Bitmap image = new Bitmap(Directory.GetCurrentDirectory() +"//Image//"+ random + ".jpeg"))
            {
                int piecesWight = image.Width;
                int piecesHeight = image.Height;
                for (int i = 0; i < piecesWight; i++)
                {
                    for (int j = 0; j < piecesHeight; j++)
                    {
                        Rectangle piecseRetangle = new Rectangle(i, j, piecesWight, piecesHeight);
                        using (Bitmap picese = new Bitmap(piecesWight, piecesHeight))
                        {
                            using (Graphics g = Graphics.FromImage(picese))
                            {
                                g.DrawImage(image, new Rectangle(0, 0, piecesWight, piecesWight), piecseRetangle, GraphicsUnit.Pixel);
                            }
                            picese.Save("D:\\image\\piece_"+ random.ToString()+"_" + DateTime.Now.ToString("dd-MM-yyyy-mm-HH-ss") + ".jpg");
                        }
                    }
                }
            }
            return "";
        }
    }
}
