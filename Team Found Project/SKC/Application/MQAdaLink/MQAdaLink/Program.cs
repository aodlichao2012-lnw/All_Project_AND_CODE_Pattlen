using MQAdaLink.Class;
using System;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace MQAdaLink
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        static void Main(string[] args)
        {
            try
            {
                DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), 0xF060, 0x00000000); //Enable Close Form
                cConsole.DisableQuickEdit();
                cConsole.SetupSystemTry(true);
                new Thread(() =>  new cMQReceiver().C_PRCxMQProcess()).Start();
                Application.Run();
                cConsole.notifyIcon.Visible = true;
            }
            catch (Exception oEx)
            {
                oEx.Message.ToString();
            }
        }
    }
}
