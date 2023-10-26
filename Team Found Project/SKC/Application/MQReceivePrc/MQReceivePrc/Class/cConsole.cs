using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Standard
{
    public static class cConsole
    {

        #region Console
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private static void SetConsoleWindowVisibility(bool visible)
        {
            IntPtr hWnd = FindWindow(null, Console.Title);
            if (hWnd != IntPtr.Zero)
            {
                if (visible) ShowWindow(hWnd, 1); //1 = SW_SHOWNORMAL           
                else ShowWindow(hWnd, 0); //0 = SW_HIDE               
            }
        }

        private const int STD_INPUT_HANDLE = -10;

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleMode(
            IntPtr hConsoleHandle,
            out int lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(
            IntPtr hConsoleHandle,
            int ioMode);

        /// <summary>
        /// This flag enables the user to use the mouse to select and edit text. To enable
        /// this option, you must also set the ExtendedFlags flag.
        /// </summary>
        const int QuickEditMode = 64;

        // ExtendedFlags must be combined with
        // InsertMode and QuickEditMode when setting
        /// <summary>
        /// ExtendedFlags must be enabled in order to enable InsertMode or QuickEditMode.
        /// </summary>
        const int ExtendedFlags = 128;

        public static void DisableQuickEdit()
        {
            IntPtr conHandle = GetStdHandle(STD_INPUT_HANDLE);
            int mode;

            if (!GetConsoleMode(conHandle, out mode))
            {
                // error getting the console mode. Exit.
                return;
            }

            mode = mode & ~(QuickEditMode | ExtendedFlags);

            if (!SetConsoleMode(conHandle, mode))
            {
                // error setting console mode.
            }
        }
        #endregion
        #region SystemTray
        private static NotifyIcon notifyIcon = new NotifyIcon();
        static bool Visible = false;
        public static void SetupSystemTry(bool pbStartShow,bool pbChkMinimize)
        {
            notifyIcon.DoubleClick += (s, e) =>
            {
                Visible = !Visible;
                SetConsoleWindowVisibility(Visible);
            };
            notifyIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            notifyIcon.Visible = true;
            notifyIcon.Text = Application.ProductName;

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Exit", null, (s, e) => { Environment.Exit(0); });
            notifyIcon.ContextMenuStrip = contextMenu;

            SetConsoleWindowVisibility(pbStartShow);
            if(pbChkMinimize) new Thread(() => C_CHKxMinimize()).Start();
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);
        private static void C_CHKxMinimize()
        {
            while(true)
            {
                if(IsIconic(Process.GetCurrentProcess().MainWindowHandle))
                {
                    SetConsoleWindowVisibility(false);
                }
                Thread.Sleep(500);
            }
        }
        #endregion
    }
}
