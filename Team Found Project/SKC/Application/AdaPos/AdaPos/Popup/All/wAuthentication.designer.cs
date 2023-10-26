namespace AdaPos.Popup.All
{
    partial class wAuthentication
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
            this.components = new System.ComponentModel.Container();
            this.opnPage = new System.Windows.Forms.Panel();
            this.otbUsername = new System.Windows.Forms.TextBox();
            this.opnSignTypePwd = new System.Windows.Forms.Panel();
            this.uNumpadPwd = new AdaPos.Control.uNumpadLogin();
            this.otbPassword = new System.Windows.Forms.TextBox();
            this.olaPassword = new System.Windows.Forms.Label();
            this.olaUsername = new System.Windows.Forms.Label();
            this.ocmSearch = new System.Windows.Forms.Button();
            this.opnHD = new System.Windows.Forms.Panel();
            this.ocmKB = new System.Windows.Forms.Button();
            this.ocmBack = new System.Windows.Forms.Button();
            this.olaTitleForm = new System.Windows.Forms.Label();
            this.ocmNextPwd = new System.Windows.Forms.Button();
            this.otmClose = new System.Windows.Forms.Timer(this.components);
            this.opnPage.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.otbUsername);
            this.opnPage.Controls.Add(this.opnSignTypePwd);
            this.opnPage.Controls.Add(this.uNumpadPwd);
            this.opnPage.Controls.Add(this.otbPassword);
            this.opnPage.Controls.Add(this.olaPassword);
            this.opnPage.Controls.Add(this.olaUsername);
            this.opnPage.Controls.Add(this.ocmSearch);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(312, 20);
            this.opnPage.Name = "opnPage";
            this.opnPage.Size = new System.Drawing.Size(400, 560);
            this.opnPage.TabIndex = 2;
            // 
            // otbUsername
            // 
            this.otbUsername.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.otbUsername.BackColor = System.Drawing.SystemColors.Control;
            this.otbUsername.Enabled = false;
            this.otbUsername.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otbUsername.Location = new System.Drawing.Point(23, 94);
            this.otbUsername.MaxLength = 6;
            this.otbUsername.Name = "otbUsername";
            this.otbUsername.Size = new System.Drawing.Size(310, 36);
            this.otbUsername.TabIndex = 40;
            // 
            // opnSignTypePwd
            // 
            this.opnSignTypePwd.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.opnSignTypePwd.AutoSize = true;
            this.opnSignTypePwd.BackColor = System.Drawing.Color.Transparent;
            this.opnSignTypePwd.Location = new System.Drawing.Point(49, 477);
            this.opnSignTypePwd.Name = "opnSignTypePwd";
            this.opnSignTypePwd.Size = new System.Drawing.Size(300, 41);
            this.opnSignTypePwd.TabIndex = 39;
            // 
            // uNumpadPwd
            // 
            this.uNumpadPwd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.uNumpadPwd.BackColor = System.Drawing.Color.Transparent;
            this.uNumpadPwd.Location = new System.Drawing.Point(70, 242);
            this.uNumpadPwd.Margin = new System.Windows.Forms.Padding(2);
            this.uNumpadPwd.Name = "uNumpadPwd";
            this.uNumpadPwd.Size = new System.Drawing.Size(258, 209);
            this.uNumpadPwd.TabIndex = 38;
            // 
            // otbPassword
            // 
            this.otbPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.otbPassword.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otbPassword.Location = new System.Drawing.Point(23, 173);
            this.otbPassword.MaxLength = 0;
            this.otbPassword.Name = "otbPassword";
            this.otbPassword.Size = new System.Drawing.Size(351, 36);
            this.otbPassword.TabIndex = 36;
            this.otbPassword.UseSystemPasswordChar = true;
            this.otbPassword.TextChanged += new System.EventHandler(this.otbPassword_TextChanged);
            this.otbPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbPassword_KeyDown);
            this.otbPassword.Leave += new System.EventHandler(this.otbPassword_Leave);
            // 
            // olaPassword
            // 
            this.olaPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.olaPassword.BackColor = System.Drawing.Color.Transparent;
            this.olaPassword.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.olaPassword.ForeColor = System.Drawing.Color.Black;
            this.olaPassword.Location = new System.Drawing.Point(25, 143);
            this.olaPassword.Name = "olaPassword";
            this.olaPassword.Size = new System.Drawing.Size(111, 27);
            this.olaPassword.TabIndex = 35;
            this.olaPassword.Text = "Password";
            this.olaPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // olaUsername
            // 
            this.olaUsername.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.olaUsername.BackColor = System.Drawing.Color.Transparent;
            this.olaUsername.Cursor = System.Windows.Forms.Cursors.Default;
            this.olaUsername.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.olaUsername.ForeColor = System.Drawing.Color.Black;
            this.olaUsername.Location = new System.Drawing.Point(25, 64);
            this.olaUsername.Name = "olaUsername";
            this.olaUsername.Size = new System.Drawing.Size(111, 27);
            this.olaUsername.TabIndex = 34;
            this.olaUsername.Text = "Username";
            this.olaUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ocmSearch
            // 
            this.ocmSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ocmSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(171)))), ((int)(((byte)(132)))));
            this.ocmSearch.FlatAppearance.BorderSize = 0;
            this.ocmSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ocmSearch.ForeColor = System.Drawing.Color.White;
            this.ocmSearch.Image = global::AdaPos.Properties.Resources.SearchW_32;
            this.ocmSearch.Location = new System.Drawing.Point(334, 94);
            this.ocmSearch.Name = "ocmSearch";
            this.ocmSearch.Size = new System.Drawing.Size(40, 36);
            this.ocmSearch.TabIndex = 33;
            this.ocmSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ocmSearch.UseVisualStyleBackColor = false;
            this.ocmSearch.Click += new System.EventHandler(this.ocmSearch_Click);
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.ocmKB);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.olaTitleForm);
            this.opnHD.Controls.Add(this.ocmNextPwd);
            this.opnHD.Location = new System.Drawing.Point(0, 0);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(398, 50);
            this.opnHD.TabIndex = 1;
            // 
            // ocmKB
            // 
            this.ocmKB.Dock = System.Windows.Forms.DockStyle.Right;
            this.ocmKB.FlatAppearance.BorderSize = 0;
            this.ocmKB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmKB.Image = global::AdaPos.Properties.Resources.KBB_32;
            this.ocmKB.Location = new System.Drawing.Point(298, 0);
            this.ocmKB.Name = "ocmKB";
            this.ocmKB.Size = new System.Drawing.Size(50, 50);
            this.ocmKB.TabIndex = 7;
            this.ocmKB.UseVisualStyleBackColor = true;
            this.ocmKB.Click += new System.EventHandler(this.ocmKB_Click);
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
            this.ocmBack.TabIndex = 5;
            this.ocmBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmBack.UseVisualStyleBackColor = false;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // olaTitleForm
            // 
            this.olaTitleForm.BackColor = System.Drawing.Color.Transparent;
            this.olaTitleForm.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleForm.ForeColor = System.Drawing.Color.White;
            this.olaTitleForm.Location = new System.Drawing.Point(53, 0);
            this.olaTitleForm.Name = "olaTitleForm";
            this.olaTitleForm.Size = new System.Drawing.Size(241, 50);
            this.olaTitleForm.TabIndex = 6;
            this.olaTitleForm.Text = "Authentication";
            this.olaTitleForm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ocmNextPwd
            // 
            this.ocmNextPwd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmNextPwd.Dock = System.Windows.Forms.DockStyle.Right;
            this.ocmNextPwd.FlatAppearance.BorderSize = 0;
            this.ocmNextPwd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmNextPwd.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmNextPwd.Location = new System.Drawing.Point(348, 0);
            this.ocmNextPwd.Name = "ocmNextPwd";
            this.ocmNextPwd.Size = new System.Drawing.Size(50, 50);
            this.ocmNextPwd.TabIndex = 37;
            this.ocmNextPwd.UseVisualStyleBackColor = false;
            this.ocmNextPwd.Click += new System.EventHandler(this.ocmNextPwd_Click);
            // 
            // otmClose
            // 
            this.otmClose.Interval = 500;
            this.otmClose.Tick += new System.EventHandler(this.otmClose_Tick);
            // 
            // wAuthentication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1024, 640);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "wAuthentication";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.SystemColors.ActiveBorder;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.wAuthentication_Shown);
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Button ocmSearch;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Label olaTitleForm;
        private System.Windows.Forms.TextBox otbUsername;
        private System.Windows.Forms.Panel opnSignTypePwd;
        private Control.uNumpadLogin uNumpadPwd;
        private System.Windows.Forms.Button ocmNextPwd;
        private System.Windows.Forms.TextBox otbPassword;
        private System.Windows.Forms.Label olaPassword;
        private System.Windows.Forms.Label olaUsername;
        private System.Windows.Forms.Timer otmClose;
        private System.Windows.Forms.Button ocmKB;
    }
}