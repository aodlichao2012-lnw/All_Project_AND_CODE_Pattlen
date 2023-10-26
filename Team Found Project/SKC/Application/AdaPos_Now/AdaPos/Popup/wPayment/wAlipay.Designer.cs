namespace AdaPos.Popup.wPayment
{
    partial class wAlipay
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
            this.olaDescription = new System.Windows.Forms.Label();
            this.opbForm = new System.Windows.Forms.PictureBox();
            this.olaKeyScan = new System.Windows.Forms.Label();
            this.otbPaymentCode = new System.Windows.Forms.TextBox();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitleAlipay = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmShwKb = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.otmQry = new System.Windows.Forms.Timer(this.components);
            this.otmReverse = new System.Windows.Forms.Timer(this.components);
            this.opnPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opbForm)).BeginInit();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.olaDescription);
            this.opnPage.Controls.Add(this.opbForm);
            this.opnPage.Controls.Add(this.olaKeyScan);
            this.opnPage.Controls.Add(this.otbPaymentCode);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(187, 284);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(650, 199);
            this.opnPage.TabIndex = 1;
            // 
            // olaDescription
            // 
            this.olaDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaDescription.BackColor = System.Drawing.Color.LightGray;
            this.olaDescription.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaDescription.ForeColor = System.Drawing.Color.Black;
            this.olaDescription.Location = new System.Drawing.Point(317, 122);
            this.olaDescription.Name = "olaDescription";
            this.olaDescription.Size = new System.Drawing.Size(319, 55);
            this.olaDescription.TabIndex = 10;
            this.olaDescription.Text = "Key/Scan";
            // 
            // opbForm
            // 
            this.opbForm.Image = global::AdaPos.Properties.Resources.Alipay_logo;
            this.opbForm.Location = new System.Drawing.Point(13, 70);
            this.opbForm.Name = "opbForm";
            this.opbForm.Size = new System.Drawing.Size(293, 106);
            this.opbForm.TabIndex = 9;
            this.opbForm.TabStop = false;
            // 
            // olaKeyScan
            // 
            this.olaKeyScan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaKeyScan.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaKeyScan.ForeColor = System.Drawing.Color.Black;
            this.olaKeyScan.Location = new System.Drawing.Point(317, 61);
            this.olaKeyScan.Name = "olaKeyScan";
            this.olaKeyScan.Size = new System.Drawing.Size(123, 29);
            this.olaKeyScan.TabIndex = 8;
            this.olaKeyScan.Text = "Key/Scan";
            this.olaKeyScan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // otbPaymentCode
            // 
            this.otbPaymentCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbPaymentCode.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.otbPaymentCode.Location = new System.Drawing.Point(317, 90);
            this.otbPaymentCode.MaxLength = 200;
            this.otbPaymentCode.Name = "otbPaymentCode";
            this.otbPaymentCode.Size = new System.Drawing.Size(319, 29);
            this.otbPaymentCode.TabIndex = 2;
            this.otbPaymentCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbPaymentCode_KeyDown);
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitleAlipay);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.ocmShwKb);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(646, 50);
            this.opnHD.TabIndex = 1;
            // 
            // olaTitleAlipay
            // 
            this.olaTitleAlipay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleAlipay.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleAlipay.ForeColor = System.Drawing.Color.White;
            this.olaTitleAlipay.Location = new System.Drawing.Point(56, 0);
            this.olaTitleAlipay.Name = "olaTitleAlipay";
            this.olaTitleAlipay.Size = new System.Drawing.Size(430, 50);
            this.olaTitleAlipay.TabIndex = 7;
            this.olaTitleAlipay.Text = "Alipay";
            this.olaTitleAlipay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.ocmShwKb.Location = new System.Drawing.Point(540, 0);
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
            this.ocmAccept.Location = new System.Drawing.Point(596, 0);
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
            // otmReverse
            // 
            this.otmReverse.Interval = 1000;
            this.otmReverse.Tick += new System.EventHandler(this.otmReverse_Tick);
            // 
            // wAlipay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wAlipay";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wAlipay";
            this.TransparencyKey = System.Drawing.SystemColors.ActiveBorder;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.wAlipay_Shown);
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opbForm)).EndInit();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Label olaKeyScan;
        private System.Windows.Forms.TextBox otbPaymentCode;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitleAlipay;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmShwKb;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.PictureBox opbForm;
        private System.Windows.Forms.Label olaDescription;
        private System.Windows.Forms.Timer otmQry;
        private System.Windows.Forms.Timer otmReverse;
    }
}