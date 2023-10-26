using System;
using System.Collections.Generic;
using System.Text;

namespace IVRCall_Dll_Agen
{
    class En_De_Coder
    {
        public string Decode(string hexString) {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hexString.Length - 1; i += 4)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hexString.Substring(i, 4), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }

        public string Encode(string sHex_thai) {

            string message = ""; string hexdata; string hex_Eng;
            int dec;

            if (sHex_thai.CompareTo("") == 0 || sHex_thai.ToString() == "" || sHex_thai.ToString() == "null")
            {
                return "";
            }

            byte[] bStrUTF8 = System.Text.Encoding.UTF8.GetBytes(sHex_thai);
            byte[] bStrBIG5 = System.Text.Encoding.Convert(System.Text.Encoding.UTF8, System.Text.Encoding.GetEncoding("windows-874"), bStrUTF8);

            foreach (byte ste in bStrBIG5)
            {

                dec = Convert.ToInt32(ste.ToString());
                hexdata = String.Format("{0:X}", (dec - 160));
                hex_Eng = String.Format("{0:X}", (dec));
                if ((dec >= 160 && dec <= 250))
                {// Hex Thai Unicode{
                    if (hexdata.Length <= 1)
                    {
                        hexdata = "0" + hexdata;
                    }
                    hexdata = "0E" + hexdata;
                }
                else
                { //Hex eng Unicode

                    if (dec <= 1)
                    {
                        hexdata = "0" + hex_Eng;
                    }
                    hexdata = "00" + hex_Eng;
                }
                message += hexdata;
            }
            return message;
        }
    }
}
