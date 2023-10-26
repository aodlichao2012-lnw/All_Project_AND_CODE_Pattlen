namespace AdaPos.Popup.wPayment
{
    partial class wEDC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(wEDC));
            this.opnPage = new System.Windows.Forms.Panel();
            this.olaTitleCardName = new System.Windows.Forms.Label();
            this.ocmSearch = new System.Windows.Forms.Button();
            this.otbSelectBank = new System.Windows.Forms.TextBox();
            this.otbCardName = new System.Windows.Forms.TextBox();
            this.oucNumpad = new AdaPos.Control.uNumpad();
            this.olaStatusEDC = new System.Windows.Forms.Label();
            this.olaApproveCode = new System.Windows.Forms.Label();
            this.olaTraceCode = new System.Windows.Forms.Label();
            this.otbTraceCode = new System.Windows.Forms.TextBox();
            this.otbApproveCode = new System.Windows.Forms.TextBox();
            this.olaBank = new System.Windows.Forms.Label();
            this.otbCreditNo = new System.Windows.Forms.TextBox();
            this.olaCreditNumber = new System.Windows.Forms.Label();
            this.oimTypeBank = new System.Windows.Forms.PictureBox();
            this.oimEDC = new System.Windows.Forms.PictureBox();
            this.opnHD = new System.Windows.Forms.Panel();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.ocmBack = new System.Windows.Forms.Button();
            this.olaTitleEdc = new System.Windows.Forms.Label();
            this.opnPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.oimTypeBank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oimEDC)).BeginInit();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.olaTitleCardName);
            this.opnPage.Controls.Add(this.ocmSearch);
            this.opnPage.Controls.Add(this.otbSelectBank);
            this.opnPage.Controls.Add(this.otbCardName);
            this.opnPage.Controls.Add(this.oucNumpad);
            this.opnPage.Controls.Add(this.olaStatusEDC);
            this.opnPage.Controls.Add(this.olaApproveCode);
            this.opnPage.Controls.Add(this.olaTraceCode);
            this.opnPage.Controls.Add(this.otbTraceCode);
            this.opnPage.Controls.Add(this.otbApproveCode);
            this.opnPage.Controls.Add(this.olaBank);
            this.opnPage.Controls.Add(this.otbCreditNo);
            this.opnPage.Controls.Add(this.olaCreditNumber);
            this.opnPage.Controls.Add(this.oimTypeBank);
            this.opnPage.Controls.Add(this.oimEDC);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(162, 219);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(700, 330);
            this.opnPage.TabIndex = 0;
            // 
            // olaTitleCardName
            // 
            this.olaTitleCardName.AutoSize = true;
            this.olaTitleCardName.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaTitleCardName.Location = new System.Drawing.Point(141, 196);
            this.olaTitleCardName.Name = "olaTitleCardName";
            this.olaTitleCardName.Size = new System.Drawing.Size(79, 19);
            this.olaTitleCardName.TabIndex = 42;
            this.olaTitleCardName.Text = "Card Name";
            // 
            // ocmSearch
            // 
            this.ocmSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(171)))), ((int)(((byte)(132)))));
            this.ocmSearch.FlatAppearance.BorderSize = 0;
            this.ocmSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ocmSearch.ForeColor = System.Drawing.Color.White;
            this.ocmSearch.Image = ((System.Drawing.Image)(resources.GetObject("ocmSearch.Image")));
            this.ocmSearch.Location = new System.Drawing.Point(363, 155);
            this.ocmSearch.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.ocmSearch.Name = "ocmSearch";
            this.ocmSearch.Size = new System.Drawing.Size(48, 34);
            this.ocmSearch.TabIndex = 41;
            this.ocmSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ocmSearch.UseVisualStyleBackColor = false;
            this.ocmSearch.Click += new System.EventHandler(this.ocmSearch_Click);
            // 
            // otbSelectBank
            // 
            this.otbSelectBank.Font = new System.Drawing.Font("Segoe UI Light", 15F);
            this.otbSelectBank.Location = new System.Drawing.Point(140, 154);
            this.otbSelectBank.MaxLength = 16;
            this.otbSelectBank.Name = "otbSelectBank";
            this.otbSelectBank.Size = new System.Drawing.Size(230, 34);
            this.otbSelectBank.TabIndex = 40;
            // 
            // otbCardName
            // 
            this.otbCardName.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.otbCardName.Location = new System.Drawing.Point(140, 217);
            this.otbCardName.MaxLength = 16;
            this.otbCardName.Name = "otbCardName";
            this.otbCardName.ReadOnly = true;
            this.otbCardName.Size = new System.Drawing.Size(274, 29);
            this.otbCardName.TabIndex = 39;
            // 
            // oucNumpad
            // 
            this.oucNumpad.Location = new System.Drawing.Point(420, 70);
            this.oucNumpad.Name = "oucNumpad";
            this.oucNumpad.Size = new System.Drawing.Size(260, 240);
            this.oucNumpad.TabIndex = 27;
            // 
            // olaStatusEDC
            // 
            this.olaStatusEDC.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaStatusEDC.Location = new System.Drawing.Point(20, 70);
            this.olaStatusEDC.Name = "olaStatusEDC";
            this.olaStatusEDC.Size = new System.Drawing.Size(100, 23);
            this.olaStatusEDC.TabIndex = 26;
            // 
            // olaApproveCode
            // 
            this.olaApproveCode.AutoSize = true;
            this.olaApproveCode.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaApproveCode.Location = new System.Drawing.Point(141, 252);
            this.olaApproveCode.Name = "olaApproveCode";
            this.olaApproveCode.Size = new System.Drawing.Size(97, 19);
            this.olaApproveCode.TabIndex = 25;
            this.olaApproveCode.Text = "Approve Code";
            // 
            // olaTraceCode
            // 
            this.olaTraceCode.AutoSize = true;
            this.olaTraceCode.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaTraceCode.Location = new System.Drawing.Point(258, 252);
            this.olaTraceCode.Name = "olaTraceCode";
            this.olaTraceCode.Size = new System.Drawing.Size(78, 19);
            this.olaTraceCode.TabIndex = 24;
            this.olaTraceCode.Text = "Trace Code";
            // 
            // otbTraceCode
            // 
            this.otbTraceCode.Font = new System.Drawing.Font("Segoe UI Light", 15F);
            this.otbTraceCode.Location = new System.Drawing.Point(257, 276);
            this.otbTraceCode.Name = "otbTraceCode";
            this.otbTraceCode.Size = new System.Drawing.Size(157, 34);
            this.otbTraceCode.TabIndex = 23;
            this.otbTraceCode.Enter += new System.EventHandler(this.otbTraceCode_Enter);
            // 
            // otbApproveCode
            // 
            this.otbApproveCode.Font = new System.Drawing.Font("Segoe UI Light", 15F);
            this.otbApproveCode.Location = new System.Drawing.Point(140, 276);
            this.otbApproveCode.Name = "otbApproveCode";
            this.otbApproveCode.Size = new System.Drawing.Size(103, 34);
            this.otbApproveCode.TabIndex = 21;
            this.otbApproveCode.Enter += new System.EventHandler(this.otbApproveCode_Enter);
            // 
            // olaBank
            // 
            this.olaBank.AutoSize = true;
            this.olaBank.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaBank.Location = new System.Drawing.Point(141, 133);
            this.olaBank.Name = "olaBank";
            this.olaBank.Size = new System.Drawing.Size(39, 19);
            this.olaBank.TabIndex = 19;
            this.olaBank.Text = "Bank";
            // 
            // otbCreditNo
            // 
            this.otbCreditNo.Font = new System.Drawing.Font("Segoe UI Light", 15F);
            this.otbCreditNo.Location = new System.Drawing.Point(140, 95);
            this.otbCreditNo.MaxLength = 20;
            this.otbCreditNo.Name = "otbCreditNo";
            this.otbCreditNo.Size = new System.Drawing.Size(274, 34);
            this.otbCreditNo.TabIndex = 18;
            this.otbCreditNo.Enter += new System.EventHandler(this.otbCreditNo_Enter);
            this.otbCreditNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbCreditNo_KeyDown);
            // 
            // olaCreditNumber
            // 
            this.olaCreditNumber.AutoSize = true;
            this.olaCreditNumber.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaCreditNumber.Location = new System.Drawing.Point(141, 71);
            this.olaCreditNumber.Name = "olaCreditNumber";
            this.olaCreditNumber.Size = new System.Drawing.Size(31, 19);
            this.olaCreditNumber.TabIndex = 17;
            this.olaCreditNumber.Text = "No.";
            // 
            // oimTypeBank
            // 
            this.oimTypeBank.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.oimTypeBank.Image = ((System.Drawing.Image)(resources.GetObject("oimTypeBank.Image")));
            this.oimTypeBank.Location = new System.Drawing.Point(20, 210);
            this.oimTypeBank.Name = "oimTypeBank";
            this.oimTypeBank.Size = new System.Drawing.Size(100, 100);
            this.oimTypeBank.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.oimTypeBank.TabIndex = 16;
            this.oimTypeBank.TabStop = false;
            this.oimTypeBank.Visible = false;
            // 
            // oimEDC
            // 
            this.oimEDC.Image = ((System.Drawing.Image)(resources.GetObject("oimEDC.Image")));
            this.oimEDC.Location = new System.Drawing.Point(20, 94);
            this.oimEDC.Name = "oimEDC";
            this.oimEDC.Size = new System.Drawing.Size(100, 110);
            this.oimEDC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.oimEDC.TabIndex = 15;
            this.oimEDC.TabStop = false;
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
            this.opnHD.Size = new System.Drawing.Size(696, 50);
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
            this.ocmAccept.Location = new System.Drawing.Point(646, 0);
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
            this.olaTitleEdc.Size = new System.Drawing.Size(373, 50);
            this.olaTitleEdc.TabIndex = 6;
            this.olaTitleEdc.Text = "Credit Card";
            this.olaTitleEdc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // wEDC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wEDC";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.SystemColors.ActiveBorder;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wEDC_FormClosing);
            this.Shown += new System.EventHandler(this.wEDC_Shown);
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.oimTypeBank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oimEDC)).EndInit();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Label olaTitleEdc;
        private System.Windows.Forms.Button ocmAccept;
        private Control.uNumpad oucNumpad;
        private System.Windows.Forms.Label olaStatusEDC;
        private System.Windows.Forms.Label olaApproveCode;
        private System.Windows.Forms.Label olaTraceCode;
        private System.Windows.Forms.TextBox otbTraceCode;
        private System.Windows.Forms.TextBox otbApproveCode;
        private System.Windows.Forms.Label olaBank;
        private System.Windows.Forms.TextBox otbCreditNo;
        private System.Windows.Forms.Label olaCreditNumber;
        private System.Windows.Forms.PictureBox oimTypeBank;
        private System.Windows.Forms.PictureBox oimEDC;
        private System.Windows.Forms.Label olaTitleCardName;
        private System.Windows.Forms.Button ocmSearch;
        private System.Windows.Forms.TextBox otbSelectBank;
        private System.Windows.Forms.TextBox otbCardName;
    }
}