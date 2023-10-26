namespace AdaPos.Popup.wPayment
{
    partial class wPromptPay
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
            this.olaCntDwn = new System.Windows.Forms.Label();
            this.olaScanQR = new System.Windows.Forms.Label();
            this.opnKey = new System.Windows.Forms.TableLayoutPanel();
            this.opnAmt = new System.Windows.Forms.Panel();
            this.olaAmt = new System.Windows.Forms.Label();
            this.olaTitleAmt = new System.Windows.Forms.Label();
            this.opnManual = new System.Windows.Forms.Panel();
            this.olaDescription = new System.Windows.Forms.Label();
            this.ocbSelectBank = new System.Windows.Forms.ComboBox();
            this.olaTitleBank = new System.Windows.Forms.Label();
            this.otbTransID = new System.Windows.Forms.TextBox();
            this.olaTitleNumber = new System.Windows.Forms.Label();
            this.opbQR = new System.Windows.Forms.PictureBox();
            this.opbForm = new System.Windows.Forms.PictureBox();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitlePromptPay = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmShwKb = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.otmQry = new System.Windows.Forms.Timer(this.components);
            this.opnPage.SuspendLayout();
            this.opnKey.SuspendLayout();
            this.opnAmt.SuspendLayout();
            this.opnManual.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opbQR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.opbForm)).BeginInit();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.olaCntDwn);
            this.opnPage.Controls.Add(this.olaScanQR);
            this.opnPage.Controls.Add(this.opnKey);
            this.opnPage.Controls.Add(this.opbQR);
            this.opnPage.Controls.Add(this.opbForm);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(202, 190);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(619, 433);
            this.opnPage.TabIndex = 2;
            // 
            // olaCntDwn
            // 
            this.olaCntDwn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.olaCntDwn.ForeColor = System.Drawing.Color.Red;
            this.olaCntDwn.Location = new System.Drawing.Point(10, 388);
            this.olaCntDwn.Name = "olaCntDwn";
            this.olaCntDwn.Size = new System.Drawing.Size(53, 28);
            this.olaCntDwn.TabIndex = 13;
            this.olaCntDwn.Text = "0";
            this.olaCntDwn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // olaScanQR
            // 
            this.olaScanQR.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.olaScanQR.Location = new System.Drawing.Point(10, 388);
            this.olaScanQR.Name = "olaScanQR";
            this.olaScanQR.Size = new System.Drawing.Size(606, 28);
            this.olaScanQR.TabIndex = 12;
            this.olaScanQR.Text = "Please scan QR Code";
            this.olaScanQR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // opnKey
            // 
            this.opnKey.ColumnCount = 1;
            this.opnKey.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opnKey.Controls.Add(this.opnAmt, 0, 0);
            this.opnKey.Controls.Add(this.opnManual, 0, 1);
            this.opnKey.Location = new System.Drawing.Point(252, 56);
            this.opnKey.Name = "opnKey";
            this.opnKey.RowCount = 2;
            this.opnKey.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.opnKey.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.opnKey.Size = new System.Drawing.Size(364, 321);
            this.opnKey.TabIndex = 11;
            // 
            // opnAmt
            // 
            this.opnAmt.Controls.Add(this.olaAmt);
            this.opnAmt.Controls.Add(this.olaTitleAmt);
            this.opnAmt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnAmt.Location = new System.Drawing.Point(3, 3);
            this.opnAmt.Name = "opnAmt";
            this.opnAmt.Size = new System.Drawing.Size(358, 74);
            this.opnAmt.TabIndex = 0;
            // 
            // olaAmt
            // 
            this.olaAmt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaAmt.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.olaAmt.Location = new System.Drawing.Point(144, 5);
            this.olaAmt.Name = "olaAmt";
            this.olaAmt.Size = new System.Drawing.Size(209, 65);
            this.olaAmt.TabIndex = 1;
            this.olaAmt.Text = "0.00";
            this.olaAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaTitleAmt
            // 
            this.olaTitleAmt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.olaTitleAmt.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.olaTitleAmt.Location = new System.Drawing.Point(5, 5);
            this.olaTitleAmt.Name = "olaTitleAmt";
            this.olaTitleAmt.Size = new System.Drawing.Size(133, 65);
            this.olaTitleAmt.TabIndex = 0;
            this.olaTitleAmt.Text = "Amount";
            this.olaTitleAmt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opnManual
            // 
            this.opnManual.Controls.Add(this.olaDescription);
            this.opnManual.Controls.Add(this.ocbSelectBank);
            this.opnManual.Controls.Add(this.olaTitleBank);
            this.opnManual.Controls.Add(this.otbTransID);
            this.opnManual.Controls.Add(this.olaTitleNumber);
            this.opnManual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnManual.Location = new System.Drawing.Point(3, 83);
            this.opnManual.Name = "opnManual";
            this.opnManual.Size = new System.Drawing.Size(358, 235);
            this.opnManual.TabIndex = 1;
            // 
            // olaDescription
            // 
            this.olaDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaDescription.BackColor = System.Drawing.Color.LightGray;
            this.olaDescription.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaDescription.ForeColor = System.Drawing.Color.Black;
            this.olaDescription.Location = new System.Drawing.Point(3, 2);
            this.olaDescription.Name = "olaDescription";
            this.olaDescription.Size = new System.Drawing.Size(352, 229);
            this.olaDescription.TabIndex = 25;
            // 
            // ocbSelectBank
            // 
            this.ocbSelectBank.Font = new System.Drawing.Font("Segoe UI Light", 15F);
            this.ocbSelectBank.FormattingEnabled = true;
            this.ocbSelectBank.Location = new System.Drawing.Point(10, 142);
            this.ocbSelectBank.Name = "ocbSelectBank";
            this.ocbSelectBank.Size = new System.Drawing.Size(332, 36);
            this.ocbSelectBank.TabIndex = 24;
            // 
            // olaTitleBank
            // 
            this.olaTitleBank.AutoSize = true;
            this.olaTitleBank.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaTitleBank.Location = new System.Drawing.Point(10, 117);
            this.olaTitleBank.Name = "olaTitleBank";
            this.olaTitleBank.Size = new System.Drawing.Size(39, 19);
            this.olaTitleBank.TabIndex = 23;
            this.olaTitleBank.Text = "Bank";
            // 
            // otbTransID
            // 
            this.otbTransID.Font = new System.Drawing.Font("Segoe UI Light", 15F);
            this.otbTransID.Location = new System.Drawing.Point(10, 56);
            this.otbTransID.MaxLength = 16;
            this.otbTransID.Name = "otbTransID";
            this.otbTransID.Size = new System.Drawing.Size(332, 34);
            this.otbTransID.TabIndex = 22;
            // 
            // olaTitleNumber
            // 
            this.olaTitleNumber.AutoSize = true;
            this.olaTitleNumber.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaTitleNumber.Location = new System.Drawing.Point(10, 31);
            this.olaTitleNumber.Name = "olaTitleNumber";
            this.olaTitleNumber.Size = new System.Drawing.Size(31, 19);
            this.olaTitleNumber.TabIndex = 21;
            this.olaTitleNumber.Text = "No.";
            // 
            // opbQR
            // 
            this.opbQR.Location = new System.Drawing.Point(10, 141);
            this.opbQR.Name = "opbQR";
            this.opbQR.Size = new System.Drawing.Size(236, 236);
            this.opbQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.opbQR.TabIndex = 10;
            this.opbQR.TabStop = false;
            // 
            // opbForm
            // 
            this.opbForm.Image = global::AdaPos.Properties.Resources.promptpay_logo;
            this.opbForm.Location = new System.Drawing.Point(10, 56);
            this.opbForm.Name = "opbForm";
            this.opbForm.Size = new System.Drawing.Size(236, 79);
            this.opbForm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.opbForm.TabIndex = 9;
            this.opbForm.TabStop = false;
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitlePromptPay);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.ocmShwKb);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(615, 50);
            this.opnHD.TabIndex = 1;
            // 
            // olaTitlePromptPay
            // 
            this.olaTitlePromptPay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitlePromptPay.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitlePromptPay.ForeColor = System.Drawing.Color.White;
            this.olaTitlePromptPay.Location = new System.Drawing.Point(56, 0);
            this.olaTitlePromptPay.Name = "olaTitlePromptPay";
            this.olaTitlePromptPay.Size = new System.Drawing.Size(399, 50);
            this.olaTitlePromptPay.TabIndex = 7;
            this.olaTitlePromptPay.Text = "PromptPay";
            this.olaTitlePromptPay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.ocmShwKb.Location = new System.Drawing.Point(509, 0);
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
            this.ocmAccept.Location = new System.Drawing.Point(565, 0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 5;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // otmQry
            // 
            this.otmQry.Interval = 1000;
            this.otmQry.Tick += new System.EventHandler(this.otmQry_Tick);
            // 
            // wPromptPay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wPromptPay";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wPromptPay";
            this.TransparencyKey = System.Drawing.SystemColors.ActiveBorder;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wPromptPay_FormClosing);
            this.opnPage.ResumeLayout(false);
            this.opnKey.ResumeLayout(false);
            this.opnAmt.ResumeLayout(false);
            this.opnManual.ResumeLayout(false);
            this.opnManual.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opbQR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.opbForm)).EndInit();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitlePromptPay;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmShwKb;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.PictureBox opbQR;
        private System.Windows.Forms.PictureBox opbForm;
        private System.Windows.Forms.TableLayoutPanel opnKey;
        private System.Windows.Forms.Panel opnAmt;
        private System.Windows.Forms.Label olaTitleAmt;
        private System.Windows.Forms.Label olaAmt;
        private System.Windows.Forms.Panel opnManual;
        private System.Windows.Forms.ComboBox ocbSelectBank;
        private System.Windows.Forms.Label olaTitleBank;
        private System.Windows.Forms.TextBox otbTransID;
        private System.Windows.Forms.Label olaTitleNumber;
        private System.Windows.Forms.Label olaDescription;
        private System.Windows.Forms.Timer otmQry;
        private System.Windows.Forms.Label olaScanQR;
        private System.Windows.Forms.Label olaCntDwn;
    }
}