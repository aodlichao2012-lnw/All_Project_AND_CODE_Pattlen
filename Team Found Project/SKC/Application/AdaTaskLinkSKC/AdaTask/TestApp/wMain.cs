using AdaHash.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestApp.Class;

namespace TestApp
{
    public partial class wMain : Form
    {
        public wMain()
        {
            InitializeComponent();
        }

        private void ocmStart_Click(object sender, EventArgs e)
        {
            using (Process oProcess = new Process())
            {
                oProcess.StartInfo.FileName = otbAppPath.Text;
                oProcess.StartInfo.Arguments = otbParameter.Text;
                //oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                oProcess.Start();
            }
        }

        private void ocmDecrypt_Click(object sender, EventArgs e)
        {
            otbValueOutput.Text = cSecurity.C_Decrypt(otbValueInput.Text, otbKey.Text);
        }

        private void ocmEncrypt_Click(object sender, EventArgs e)
        {
            otbValueOutput.Text = cSecurity.C_Encrypt(otbValueInput.Text, otbKey.Text);
        }

        private void ocbBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFile = new OpenFileDialog();
            if(oFile.ShowDialog() == DialogResult.OK)
            {
                otbAppPath.Text = oFile.FileName;
            }
        }
    }
}
