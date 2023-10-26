namespace AdaPos.Popup.wPayment
{
    partial class wCreditCard
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
            this.onpNumpad = new AdaPos.Control.uNumpad();
            this.olaCreditBank = new System.Windows.Forms.Label();
            this.otbCreditNo = new System.Windows.Forms.TextBox();
            this.olaCreditNumber = new System.Windows.Forms.Label();
            this.opnHD = new System.Windows.Forms.Panel();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.ocmBack = new System.Windows.Forms.Button();
            this.olaTitleEdc = new System.Windows.Forms.Label();
            this.ocmSearch = new System.Windows.Forms.Button();
            this.otbSelectBank = new System.Windows.Forms.TextBox();
            this.otbCardName = new System.Windows.Forms.TextBox();
            this.olaTitleCardName = new System.Windows.Forms.Label();
            this.opnPage.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.ocmSearch);
            this.opnPage.Controls.Add(this.otbSelectBank);
            this.opnPage.Controls.Add(this.otbCardName);
            this.opnPage.Controls.Add(this.olaTitleCardName);
            this.opnPage.Controls.Add(this.onpNumpad);
            this.opnPage.Controls.Add(this.olaCreditBank);
            this.opnPage.Controls.Add(this.otbCreditNo);
            this.opnPage.Controls.Add(this.olaCreditNumber);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(222, 219);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(580, 330);
            this.opnPage.TabIndex = 1;
            // 
            // onpNumpad
            // 
            this.onpNumpad.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.onpNumpad.Location = new System.Drawing.Point(297, 71);
            this.onpNumpad.Name = "onpNumpad";
            this.onpNumpad.Size = new System.Drawing.Size(260, 236);
            this.onpNumpad.TabIndex = 27;
            // 
            // olaCreditBank
            // 
            this.olaCreditBank.AutoSize = true;
            this.olaCreditBank.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaCreditBank.Location = new System.Drawing.Point(21, 157);
            this.olaCreditBank.Name = "olaCreditBank";
            this.olaCreditBank.Size = new System.Drawing.Size(39, 19);
            this.olaCreditBank.TabIndex = 19;
            this.olaCreditBank.Text = "Bank";
            // 
            // otbCreditNo
            // 
            this.otbCreditNo.Font = new System.Drawing.Font("Segoe UI Light", 15F);
            this.otbCreditNo.Location = new System.Drawing.Point(20, 95);
            this.otbCreditNo.MaxLength = 16;
            this.otbCreditNo.Name = "otbCreditNo";
            this.otbCreditNo.Size = new System.Drawing.Size(260, 34);
            this.otbCreditNo.TabIndex = 18;
            this.otbCreditNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbCreditNo_KeyDown);
            // 
            // olaCreditNumber
            // 
            this.olaCreditNumber.AutoSize = true;
            this.olaCreditNumber.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaCreditNumber.Location = new System.Drawing.Point(21, 71);
            this.olaCreditNumber.Name = "olaCreditNumber";
            this.olaCreditNumber.Size = new System.Drawing.Size(31, 19);
            this.olaCreditNumber.TabIndex = 17;
            this.olaCreditNumber.Text = "No.";
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.olaTitleEdc);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(576, 50);
            this.opnHD.TabIndex = 1;
            // 
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmAccept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmAccept.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmAccept.ForeColor = System.Drawing.Color.White;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmAccept.Location = new System.Drawing.Point(526, 0);
            this.ocmAccept.Margin = new System.Windows.Forms.Padding(0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 7;
            this.ocmAccept.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmAccept.UseVisualStyleBackColor = false;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
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
            // olaTitleEdc
            // 
            this.olaTitleEdc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleEdc.BackColor = System.Drawing.Color.Transparent;
            this.olaTitleEdc.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleEdc.ForeColor = System.Drawing.Color.White;
            this.olaTitleEdc.Location = new System.Drawing.Point(53, 0);
            this.olaTitleEdc.Name = "olaTitleEdc";
            this.olaTitleEdc.Size = new System.Drawing.Size(643, 50);
            this.olaTitleEdc.TabIndex = 6;
            this.olaTitleEdc.Text = "Credit Card";
            this.olaTitleEdc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ocmSearch
            // 
            this.ocmSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(171)))), ((int)(((byte)(132)))));
            this.ocmSearch.FlatAppearance.BorderSize = 0;
            this.ocmSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ocmSearch.ForeColor = System.Drawing.Color.White;
            this.ocmSearch.Image = global::AdaPos.Properties.Resources.SearchW_32;
            this.ocmSearch.Location = new System.Drawing.Point(232, 179);
            this.ocmSearch.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.ocmSearch.Name = "ocmSearch";
            this.ocmSearch.Size = new System.Drawing.Size(48, 34);
            this.ocmSearch.TabIndex = 38;
            this.ocmSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ocmSearch.UseVisualStyleBackColor = false;
            this.ocmSearch.Click += new System.EventHandler(this.ocmSearch_Click);
            // 
            // otbSelectBank
            // 
            this.otbSelectBank.Font = new System.Drawing.Font("Segoe UI Light", 15F);
            this.otbSelectBank.Location = new System.Drawing.Point(20, 179);
            this.otbSelectBank.MaxLength = 16;
            this.otbSelectBank.Name = "otbSelectBank";
            this.otbSelectBank.Size = new System.Drawing.Size(218, 34);
            this.otbSelectBank.TabIndex = 37;
            // 
            // otbCardName
            // 
            this.otbCardName.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.otbCardName.Location = new System.Drawing.Point(20, 264);
            this.otbCardName.MaxLength = 16;
            this.otbCardName.Name = "otbCardName";
            this.otbCardName.Size = new System.Drawing.Size(260, 29);
            this.otbCardName.TabIndex = 36;
            // 
            // olaTitleCardName
            // 
            this.olaTitleCardName.AutoSize = true;
            this.olaTitleCardName.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaTitleCardName.Location = new System.Drawing.Point(20, 242);
            this.olaTitleCardName.Name = "olaTitleCardName";
            this.olaTitleCardName.Size = new System.Drawing.Size(79, 19);
            this.olaTitleCardName.TabIndex = 35;
            this.olaTitleCardName.Text = "Card Name";
            // 
            // wCreditCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wCreditCard";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.SystemColors.ActiveBorder;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wCreditCard_FormClosing);
            this.Shown += new System.EventHandler(this.wCreditCard_Shown);
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private Control.uNumpad onpNumpad;
        private System.Windows.Forms.Label olaCreditBank;
        private System.Windows.Forms.TextBox otbCreditNo;
        private System.Windows.Forms.Label olaCreditNumber;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Label olaTitleEdc;
        private System.Windows.Forms.Button ocmSearch;
        private System.Windows.Forms.TextBox otbSelectBank;
        private System.Windows.Forms.TextBox otbCardName;
        private System.Windows.Forms.Label olaTitleCardName;
    }
}