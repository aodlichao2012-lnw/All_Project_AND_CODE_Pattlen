namespace AdaPos.Popup.wLogin
{
    partial class wPasscode
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
            this.opnEmail = new System.Windows.Forms.Panel();
            this.opcPincode = new AdaPos.Control.uPasscode();
            this.otbPasscode = new System.Windows.Forms.TextBox();
            this.opnHD = new System.Windows.Forms.Panel();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmOK = new System.Windows.Forms.Button();
            this.olaTitlePasscode = new System.Windows.Forms.Label();
            this.opnEmail.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnEmail
            // 
            this.opnEmail.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnEmail.BackColor = System.Drawing.Color.White;
            this.opnEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnEmail.Controls.Add(this.opcPincode);
            this.opnEmail.Controls.Add(this.otbPasscode);
            this.opnEmail.Controls.Add(this.opnHD);
            this.opnEmail.Location = new System.Drawing.Point(327, 143);
            this.opnEmail.Name = "opnEmail";
            this.opnEmail.Padding = new System.Windows.Forms.Padding(1);
            this.opnEmail.Size = new System.Drawing.Size(370, 482);
            this.opnEmail.TabIndex = 1;
            // 
            // opcPincode
            // 
            this.opcPincode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opcPincode.Location = new System.Drawing.Point(11, 143);
            this.opcPincode.Name = "opcPincode";
            this.opcPincode.Size = new System.Drawing.Size(346, 316);
            this.opcPincode.TabIndex = 12;
            // 
            // otbPasscode
            // 
            this.otbPasscode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbPasscode.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.otbPasscode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.otbPasscode.Font = new System.Drawing.Font("Segoe UI Light", 25F);
            this.otbPasscode.Location = new System.Drawing.Point(11, 71);
            this.otbPasscode.MaxLength = 6;
            this.otbPasscode.Name = "otbPasscode";
            this.otbPasscode.Size = new System.Drawing.Size(346, 52);
            this.otbPasscode.TabIndex = 0;
            this.otbPasscode.TabStop = false;
            this.otbPasscode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.otbPasscode.UseSystemPasswordChar = true;
            this.otbPasscode.WordWrap = false;
            this.otbPasscode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbPasscode_KeyDown);
            this.otbPasscode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.otbPasscode_KeyPress);
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.ocmOK);
            this.opnHD.Controls.Add(this.olaTitlePasscode);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(366, 50);
            this.opnHD.TabIndex = 11;
            // 
            // ocmBack
            // 
            this.ocmBack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmBack.FlatAppearance.BorderSize = 0;
            this.ocmBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmBack.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmBack.ForeColor = System.Drawing.Color.White;
            this.ocmBack.Image = global::AdaPos.Properties.Resources.BackW_32;
            this.ocmBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmBack.Location = new System.Drawing.Point(0, 0);
            this.ocmBack.Margin = new System.Windows.Forms.Padding(0);
            this.ocmBack.Name = "ocmBack";
            this.ocmBack.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ocmBack.Size = new System.Drawing.Size(50, 50);
            this.ocmBack.TabIndex = 3;
            this.ocmBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmBack.UseVisualStyleBackColor = false;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // ocmOK
            // 
            this.ocmOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmOK.FlatAppearance.BorderSize = 0;
            this.ocmOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmOK.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmOK.ForeColor = System.Drawing.Color.White;
            this.ocmOK.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmOK.Location = new System.Drawing.Point(316, 0);
            this.ocmOK.Margin = new System.Windows.Forms.Padding(0);
            this.ocmOK.MaximumSize = new System.Drawing.Size(250, 50);
            this.ocmOK.Name = "ocmOK";
            this.ocmOK.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ocmOK.Size = new System.Drawing.Size(50, 50);
            this.ocmOK.TabIndex = 2;
            this.ocmOK.Tag = "0";
            this.ocmOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmOK.UseVisualStyleBackColor = false;
            this.ocmOK.Click += new System.EventHandler(this.ocmOK_Click);
            // 
            // olaTitlePasscode
            // 
            this.olaTitlePasscode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitlePasscode.BackColor = System.Drawing.Color.Transparent;
            this.olaTitlePasscode.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitlePasscode.ForeColor = System.Drawing.Color.White;
            this.olaTitlePasscode.Location = new System.Drawing.Point(50, 0);
            this.olaTitlePasscode.Name = "olaTitlePasscode";
            this.olaTitlePasscode.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.olaTitlePasscode.Size = new System.Drawing.Size(263, 50);
            this.olaTitlePasscode.TabIndex = 4;
            this.olaTitlePasscode.Text = "Passcode";
            this.olaTitlePasscode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // wPasscode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnEmail);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "wPasscode";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wPasscode";
            this.TransparencyKey = System.Drawing.SystemColors.ActiveBorder;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.wPasscode_Shown);
            this.opnEmail.ResumeLayout(false);
            this.opnEmail.PerformLayout();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnEmail;
        private System.Windows.Forms.TextBox otbPasscode;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmOK;
        private System.Windows.Forms.Label olaTitlePasscode;
        private Control.uPasscode opcPincode;
    }
}