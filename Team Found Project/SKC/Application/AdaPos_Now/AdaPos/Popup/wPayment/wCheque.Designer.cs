namespace AdaPos.Popup.wPayment
{
    partial class wCheque
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
            this.otpDate = new System.Windows.Forms.DateTimePicker();
            this.olaDate = new System.Windows.Forms.Label();
            this.otbChequeNo = new System.Windows.Forms.TextBox();
            this.olaChequeNo = new System.Windows.Forms.Label();
            this.olaBank = new System.Windows.Forms.Label();
            this.ocbSelectBank = new System.Windows.Forms.ComboBox();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitleCheck = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmShwKb = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.opnPage.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.otpDate);
            this.opnPage.Controls.Add(this.olaDate);
            this.opnPage.Controls.Add(this.otbChequeNo);
            this.opnPage.Controls.Add(this.olaChequeNo);
            this.opnPage.Controls.Add(this.olaBank);
            this.opnPage.Controls.Add(this.ocbSelectBank);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(277, 269);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(470, 230);
            this.opnPage.TabIndex = 0;
            // 
            // otpDate
            // 
            this.otpDate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.otpDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.otpDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.otpDate.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.otpDate.Location = new System.Drawing.Point(291, 158);
            this.otpDate.Margin = new System.Windows.Forms.Padding(0);
            this.otpDate.Name = "otpDate";
            this.otpDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.otpDate.Size = new System.Drawing.Size(148, 35);
            this.otpDate.TabIndex = 10;
            // 
            // olaDate
            // 
            this.olaDate.AutoSize = true;
            this.olaDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaDate.Location = new System.Drawing.Point(290, 140);
            this.olaDate.Name = "olaDate";
            this.olaDate.Size = new System.Drawing.Size(36, 17);
            this.olaDate.TabIndex = 8;
            this.olaDate.Text = "Date";
            // 
            // otbChequeNo
            // 
            this.otbChequeNo.Font = new System.Drawing.Font("Segoe UI Light", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otbChequeNo.Location = new System.Drawing.Point(32, 86);
            this.otbChequeNo.Name = "otbChequeNo";
            this.otbChequeNo.Size = new System.Drawing.Size(408, 34);
            this.otbChequeNo.TabIndex = 6;
            // 
            // olaChequeNo
            // 
            this.olaChequeNo.AutoSize = true;
            this.olaChequeNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaChequeNo.Location = new System.Drawing.Point(30, 67);
            this.olaChequeNo.Name = "olaChequeNo";
            this.olaChequeNo.Size = new System.Drawing.Size(66, 17);
            this.olaChequeNo.TabIndex = 5;
            this.olaChequeNo.Text = "Check No";
            // 
            // olaBank
            // 
            this.olaBank.AutoSize = true;
            this.olaBank.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaBank.Location = new System.Drawing.Point(30, 140);
            this.olaBank.Name = "olaBank";
            this.olaBank.Size = new System.Drawing.Size(38, 17);
            this.olaBank.TabIndex = 4;
            this.olaBank.Text = "Bank";
            // 
            // ocbSelectBank
            // 
            this.ocbSelectBank.Font = new System.Drawing.Font("Segoe UI Light", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ocbSelectBank.FormattingEnabled = true;
            this.ocbSelectBank.Location = new System.Drawing.Point(32, 159);
            this.ocbSelectBank.Name = "ocbSelectBank";
            this.ocbSelectBank.Size = new System.Drawing.Size(241, 36);
            this.ocbSelectBank.TabIndex = 3;
            // 
            // opnHD
            // 
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitleCheck);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.ocmShwKb);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Dock = System.Windows.Forms.DockStyle.Top;
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(466, 50);
            this.opnHD.TabIndex = 2;
            // 
            // olaTitleCheck
            // 
            this.olaTitleCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleCheck.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleCheck.ForeColor = System.Drawing.Color.White;
            this.olaTitleCheck.Location = new System.Drawing.Point(56, 0);
            this.olaTitleCheck.Name = "olaTitleCheck";
            this.olaTitleCheck.Size = new System.Drawing.Size(250, 50);
            this.olaTitleCheck.TabIndex = 7;
            this.olaTitleCheck.Text = "Cheque";
            this.olaTitleCheck.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ocmBack
            // 
            this.ocmBack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmBack.FlatAppearance.BorderSize = 0;
            this.ocmBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmBack.Image = global::AdaPos.Properties.Resources.BackW_32;
            this.ocmBack.Location = new System.Drawing.Point(0, 0);
            this.ocmBack.Name = "ocmBack";
            this.ocmBack.Size = new System.Drawing.Size(50, 50);
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
            this.ocmShwKb.Location = new System.Drawing.Point(360, 0);
            this.ocmShwKb.Name = "ocmShwKb";
            this.ocmShwKb.Size = new System.Drawing.Size(50, 50);
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
            this.ocmAccept.Location = new System.Drawing.Point(416, 0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 5;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // wCheque
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wCheque";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wPayByCheck";
            this.TransparencyKey = System.Drawing.SystemColors.ActiveBorder;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitleCheck;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmShwKb;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.ComboBox ocbSelectBank;
        private System.Windows.Forms.Label olaDate;
        private System.Windows.Forms.TextBox otbChequeNo;
        private System.Windows.Forms.Label olaChequeNo;
        private System.Windows.Forms.Label olaBank;
        private System.Windows.Forms.DateTimePicker otpDate;
    }
}