namespace AdaPos.Popup.All
{
    partial class wChangePassword
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
            this.uNumpadPwd = new AdaPos.Control.uNumpadLogin();
            this.otbConfirmPwd = new System.Windows.Forms.TextBox();
            this.olaConfirmPwd = new System.Windows.Forms.Label();
            this.otbNewPwd = new System.Windows.Forms.TextBox();
            this.olaNewPwd = new System.Windows.Forms.Label();
            this.otbOldPwd = new System.Windows.Forms.TextBox();
            this.olaOldPwd = new System.Windows.Forms.Label();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitleChangePassword = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmShwKb = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
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
            this.opnPage.Controls.Add(this.uNumpadPwd);
            this.opnPage.Controls.Add(this.otbConfirmPwd);
            this.opnPage.Controls.Add(this.olaConfirmPwd);
            this.opnPage.Controls.Add(this.otbNewPwd);
            this.opnPage.Controls.Add(this.olaNewPwd);
            this.opnPage.Controls.Add(this.otbOldPwd);
            this.opnPage.Controls.Add(this.olaOldPwd);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(1, 1);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(546, 694);
            this.opnPage.TabIndex = 0;
            // 
            // uNumpadPwd
            // 
            this.uNumpadPwd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.uNumpadPwd.BackColor = System.Drawing.Color.Transparent;
            this.uNumpadPwd.Location = new System.Drawing.Point(98, 364);
            this.uNumpadPwd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.uNumpadPwd.Name = "uNumpadPwd";
            this.uNumpadPwd.Size = new System.Drawing.Size(350, 280);
            this.uNumpadPwd.TabIndex = 39;
            // 
            // otbConfirmPwd
            // 
            this.otbConfirmPwd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbConfirmPwd.Font = new System.Drawing.Font("Segoe UI Light", 16F);
            this.otbConfirmPwd.Location = new System.Drawing.Point(67, 285);
            this.otbConfirmPwd.Margin = new System.Windows.Forms.Padding(4);
            this.otbConfirmPwd.Name = "otbConfirmPwd";
            this.otbConfirmPwd.Size = new System.Drawing.Size(413, 43);
            this.otbConfirmPwd.TabIndex = 37;
            this.otbConfirmPwd.UseSystemPasswordChar = true;
            this.otbConfirmPwd.Click += new System.EventHandler(this.otbConfirmPwd_Click);
            // 
            // olaConfirmPwd
            // 
            this.olaConfirmPwd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaConfirmPwd.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaConfirmPwd.Location = new System.Drawing.Point(67, 254);
            this.olaConfirmPwd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaConfirmPwd.Name = "olaConfirmPwd";
            this.olaConfirmPwd.Size = new System.Drawing.Size(145, 27);
            this.olaConfirmPwd.TabIndex = 36;
            this.olaConfirmPwd.Text = "Confirm Password";
            this.olaConfirmPwd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // otbNewPwd
            // 
            this.otbNewPwd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbNewPwd.Font = new System.Drawing.Font("Segoe UI Light", 16F);
            this.otbNewPwd.Location = new System.Drawing.Point(67, 197);
            this.otbNewPwd.Margin = new System.Windows.Forms.Padding(4);
            this.otbNewPwd.Name = "otbNewPwd";
            this.otbNewPwd.Size = new System.Drawing.Size(413, 43);
            this.otbNewPwd.TabIndex = 35;
            this.otbNewPwd.UseSystemPasswordChar = true;
            this.otbNewPwd.Click += new System.EventHandler(this.otbNewPwd_Click);
            // 
            // olaNewPwd
            // 
            this.olaNewPwd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaNewPwd.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaNewPwd.Location = new System.Drawing.Point(67, 166);
            this.olaNewPwd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaNewPwd.Name = "olaNewPwd";
            this.olaNewPwd.Size = new System.Drawing.Size(145, 27);
            this.olaNewPwd.TabIndex = 34;
            this.olaNewPwd.Text = "New Password";
            this.olaNewPwd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // otbOldPwd
            // 
            this.otbOldPwd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbOldPwd.Font = new System.Drawing.Font("Segoe UI Light", 16F);
            this.otbOldPwd.Location = new System.Drawing.Point(67, 110);
            this.otbOldPwd.Margin = new System.Windows.Forms.Padding(4);
            this.otbOldPwd.Name = "otbOldPwd";
            this.otbOldPwd.Size = new System.Drawing.Size(413, 43);
            this.otbOldPwd.TabIndex = 33;
            this.otbOldPwd.UseSystemPasswordChar = true;
            this.otbOldPwd.Click += new System.EventHandler(this.otbOldPwd_Click);
            // 
            // olaOldPwd
            // 
            this.olaOldPwd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaOldPwd.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaOldPwd.Location = new System.Drawing.Point(67, 79);
            this.olaOldPwd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaOldPwd.Name = "olaOldPwd";
            this.olaOldPwd.Size = new System.Drawing.Size(145, 27);
            this.olaOldPwd.TabIndex = 32;
            this.olaOldPwd.Text = "Old Password";
            this.olaOldPwd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitleChangePassword);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.ocmShwKb);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.ForeColor = System.Drawing.Color.White;
            this.opnHD.Location = new System.Drawing.Point(2, 2);
            this.opnHD.Margin = new System.Windows.Forms.Padding(4);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(541, 62);
            this.opnHD.TabIndex = 4;
            // 
            // olaTitleChangePassword
            // 
            this.olaTitleChangePassword.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleChangePassword.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleChangePassword.ForeColor = System.Drawing.Color.White;
            this.olaTitleChangePassword.Location = new System.Drawing.Point(75, 0);
            this.olaTitleChangePassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaTitleChangePassword.Name = "olaTitleChangePassword";
            this.olaTitleChangePassword.Size = new System.Drawing.Size(253, 62);
            this.olaTitleChangePassword.TabIndex = 7;
            this.olaTitleChangePassword.Text = "Change Password";
            this.olaTitleChangePassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // ocmShwKb
            // 
            this.ocmShwKb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmShwKb.FlatAppearance.BorderSize = 0;
            this.ocmShwKb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmShwKb.Image = global::AdaPos.Properties.Resources.KBB_32;
            this.ocmShwKb.Location = new System.Drawing.Point(399, 0);
            this.ocmShwKb.Margin = new System.Windows.Forms.Padding(4);
            this.ocmShwKb.Name = "ocmShwKb";
            this.ocmShwKb.Size = new System.Drawing.Size(67, 62);
            this.ocmShwKb.TabIndex = 6;
            this.ocmShwKb.UseVisualStyleBackColor = true;
            this.ocmShwKb.Click += new System.EventHandler(this.ocmShwKb_Click);
            // 
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.Location = new System.Drawing.Point(474, 0);
            this.ocmAccept.Margin = new System.Windows.Forms.Padding(4);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(67, 62);
            this.ocmAccept.TabIndex = 5;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // wChangePassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(550, 696);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wChangePassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wChangePassword";
            this.Shown += new System.EventHandler(this.wChangePassword_Shown);
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitleChangePassword;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmShwKb;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.TextBox otbConfirmPwd;
        private System.Windows.Forms.Label olaConfirmPwd;
        private System.Windows.Forms.TextBox otbNewPwd;
        private System.Windows.Forms.Label olaNewPwd;
        private System.Windows.Forms.TextBox otbOldPwd;
        private System.Windows.Forms.Label olaOldPwd;
        private Control.uNumpadLogin uNumpadPwd;
    }
}