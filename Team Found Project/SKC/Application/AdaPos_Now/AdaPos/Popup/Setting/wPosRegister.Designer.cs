namespace AdaPos.Popup.Setting
{
    partial class wPosRegister
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.opnPage = new System.Windows.Forms.Panel();
            this.olaMacAddr = new System.Windows.Forms.Label();
            this.otbMacAddr = new System.Windows.Forms.TextBox();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitlePosRegister = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.olaComName = new System.Windows.Forms.Label();
            this.otbComName = new System.Windows.Forms.TextBox();
            this.opnPage.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.Controls.Add(this.olaComName);
            this.opnPage.Controls.Add(this.otbComName);
            this.opnPage.Controls.Add(this.olaMacAddr);
            this.opnPage.Controls.Add(this.otbMacAddr);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(1, 1);
            this.opnPage.Name = "opnPage";
            this.opnPage.Size = new System.Drawing.Size(648, 318);
            this.opnPage.TabIndex = 0;
            // 
            // olaMacAddr
            // 
            this.olaMacAddr.AutoSize = true;
            this.olaMacAddr.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaMacAddr.Location = new System.Drawing.Point(43, 195);
            this.olaMacAddr.Name = "olaMacAddr";
            this.olaMacAddr.Size = new System.Drawing.Size(177, 28);
            this.olaMacAddr.TabIndex = 7;
            this.olaMacAddr.Text = "Mac. Address No :";
            // 
            // otbMacAddr
            // 
            this.otbMacAddr.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otbMacAddr.Location = new System.Drawing.Point(48, 226);
            this.otbMacAddr.Name = "otbMacAddr";
            this.otbMacAddr.ReadOnly = true;
            this.otbMacAddr.Size = new System.Drawing.Size(550, 38);
            this.otbMacAddr.TabIndex = 6;
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitlePosRegister);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.ForeColor = System.Drawing.Color.White;
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Margin = new System.Windows.Forms.Padding(4);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(646, 62);
            this.opnHD.TabIndex = 5;
            // 
            // olaTitlePosRegister
            // 
            this.olaTitlePosRegister.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitlePosRegister.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitlePosRegister.ForeColor = System.Drawing.Color.White;
            this.olaTitlePosRegister.Location = new System.Drawing.Point(75, 0);
            this.olaTitlePosRegister.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaTitlePosRegister.Name = "olaTitlePosRegister";
            this.olaTitlePosRegister.Size = new System.Drawing.Size(358, 62);
            this.olaTitlePosRegister.TabIndex = 7;
            this.olaTitlePosRegister.Text = "Pos Register";
            this.olaTitlePosRegister.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ocmBack
            // 
            this.ocmBack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmBack.FlatAppearance.BorderSize = 0;
            this.ocmBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmBack.Image = global::AdaPos.Properties.Resources.BackW_32;
            this.ocmBack.Location = new System.Drawing.Point(0, 0);
            this.ocmBack.Margin = new System.Windows.Forms.Padding(4);
            this.ocmBack.Name = "ocmBack";
            this.ocmBack.Size = new System.Drawing.Size(67, 62);
            this.ocmBack.TabIndex = 4;
            this.ocmBack.UseVisualStyleBackColor = true;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.Location = new System.Drawing.Point(579, 0);
            this.ocmAccept.Margin = new System.Windows.Forms.Padding(4);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(67, 62);
            this.ocmAccept.TabIndex = 5;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // olaComName
            // 
            this.olaComName.AutoSize = true;
            this.olaComName.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaComName.Location = new System.Drawing.Point(43, 97);
            this.olaComName.Name = "olaComName";
            this.olaComName.Size = new System.Drawing.Size(174, 28);
            this.olaComName.TabIndex = 9;
            this.olaComName.Text = "Computer Name :";
            // 
            // otbComName
            // 
            this.otbComName.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otbComName.Location = new System.Drawing.Point(48, 128);
            this.otbComName.Name = "otbComName";
            this.otbComName.Size = new System.Drawing.Size(550, 38);
            this.otbComName.TabIndex = 8;
            // 
            // wPosRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(650, 320);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wPosRegister";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wPosRegister";
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitlePosRegister;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Label olaMacAddr;
        private System.Windows.Forms.TextBox otbMacAddr;
        private System.Windows.Forms.Label olaComName;
        private System.Windows.Forms.TextBox otbComName;
    }
}