namespace AdaPos.Popup.wTaxInvoice
{
    partial class wTaxSelectPrint
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
            this.ocmPrnSelect = new System.Windows.Forms.Button();
            this.otbPrnDriver = new System.Windows.Forms.TextBox();
            this.orbPrnA4 = new System.Windows.Forms.RadioButton();
            this.orbPrnThermal = new System.Windows.Forms.RadioButton();
            this.opnHD = new System.Windows.Forms.Panel();
            this.ocmPreview = new System.Windows.Forms.Button();
            this.ocmPrint = new System.Windows.Forms.Button();
            this.olaTitleSelectPnt = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocdPrn = new System.Windows.Forms.PrintDialog();
            this.opnPage.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.ocmPrnSelect);
            this.opnPage.Controls.Add(this.otbPrnDriver);
            this.opnPage.Controls.Add(this.orbPrnA4);
            this.opnPage.Controls.Add(this.orbPrnThermal);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnPage.Location = new System.Drawing.Point(0, 0);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(400, 228);
            this.opnPage.TabIndex = 0;
            // 
            // ocmPrnSelect
            // 
            this.ocmPrnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmPrnSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmPrnSelect.FlatAppearance.BorderSize = 0;
            this.ocmPrnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmPrnSelect.Image = global::AdaPos.Properties.Resources.SearchW_32;
            this.ocmPrnSelect.Location = new System.Drawing.Point(323, 174);
            this.ocmPrnSelect.Name = "ocmPrnSelect";
            this.ocmPrnSelect.Size = new System.Drawing.Size(50, 29);
            this.ocmPrnSelect.TabIndex = 6;
            this.ocmPrnSelect.UseVisualStyleBackColor = false;
            this.ocmPrnSelect.Click += new System.EventHandler(this.ocmPrnSelect_Click);
            // 
            // otbPrnDriver
            // 
            this.otbPrnDriver.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbPrnDriver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.otbPrnDriver.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otbPrnDriver.Location = new System.Drawing.Point(62, 174);
            this.otbPrnDriver.MaxLength = 20;
            this.otbPrnDriver.Name = "otbPrnDriver";
            this.otbPrnDriver.Size = new System.Drawing.Size(255, 29);
            this.otbPrnDriver.TabIndex = 5;
            // 
            // orbPrnA4
            // 
            this.orbPrnA4.AutoSize = true;
            this.orbPrnA4.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.orbPrnA4.Location = new System.Drawing.Point(48, 130);
            this.orbPrnA4.Name = "orbPrnA4";
            this.orbPrnA4.Size = new System.Drawing.Size(83, 23);
            this.orbPrnA4.TabIndex = 4;
            this.orbPrnA4.TabStop = true;
            this.orbPrnA4.Text = "Paper A4";
            this.orbPrnA4.UseVisualStyleBackColor = true;
            this.orbPrnA4.CheckedChanged += new System.EventHandler(this.orbPrnA4_CheckedChanged);
            // 
            // orbPrnThermal
            // 
            this.orbPrnThermal.AutoSize = true;
            this.orbPrnThermal.Checked = true;
            this.orbPrnThermal.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.orbPrnThermal.Location = new System.Drawing.Point(48, 84);
            this.orbPrnThermal.Name = "orbPrnThermal";
            this.orbPrnThermal.Size = new System.Drawing.Size(117, 23);
            this.orbPrnThermal.TabIndex = 3;
            this.orbPrnThermal.TabStop = true;
            this.orbPrnThermal.Text = "Paper Thermal";
            this.orbPrnThermal.UseVisualStyleBackColor = true;
            this.orbPrnThermal.CheckedChanged += new System.EventHandler(this.orbPrnThermal_CheckedChanged);
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.ocmPreview);
            this.opnHD.Controls.Add(this.ocmPrint);
            this.opnHD.Controls.Add(this.olaTitleSelectPnt);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(396, 50);
            this.opnHD.TabIndex = 2;
            // 
            // ocmPreview
            // 
            this.ocmPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmPreview.FlatAppearance.BorderSize = 0;
            this.ocmPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmPreview.Image = global::AdaPos.Properties.Resources.Tax_32;
            this.ocmPreview.Location = new System.Drawing.Point(298, 0);
            this.ocmPreview.Name = "ocmPreview";
            this.ocmPreview.Size = new System.Drawing.Size(50, 50);
            this.ocmPreview.TabIndex = 3;
            this.ocmPreview.UseVisualStyleBackColor = true;
            this.ocmPreview.Click += new System.EventHandler(this.ocmPreview_Click);
            // 
            // ocmPrint
            // 
            this.ocmPrint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmPrint.FlatAppearance.BorderSize = 0;
            this.ocmPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmPrint.Image = global::AdaPos.Properties.Resources.Print_32;
            this.ocmPrint.Location = new System.Drawing.Point(347, 0);
            this.ocmPrint.Name = "ocmPrint";
            this.ocmPrint.Size = new System.Drawing.Size(50, 50);
            this.ocmPrint.TabIndex = 2;
            this.ocmPrint.UseVisualStyleBackColor = true;
            this.ocmPrint.Click += new System.EventHandler(this.ocmPrint_Click);
            // 
            // olaTitleSelectPnt
            // 
            this.olaTitleSelectPnt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleSelectPnt.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleSelectPnt.ForeColor = System.Drawing.Color.White;
            this.olaTitleSelectPnt.Location = new System.Drawing.Point(56, 0);
            this.olaTitleSelectPnt.Name = "olaTitleSelectPnt";
            this.olaTitleSelectPnt.Size = new System.Drawing.Size(232, 50);
            this.olaTitleSelectPnt.TabIndex = 1;
            this.olaTitleSelectPnt.Text = "Select format to Print";
            this.olaTitleSelectPnt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.ocmBack.TabIndex = 0;
            this.ocmBack.UseVisualStyleBackColor = true;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // ocdPrn
            // 
            this.ocdPrn.UseEXDialog = true;
            // 
            // wTaxSelectPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(400, 228);
            this.ControlBox = false;
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wTaxSelectPrint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wTaxSelectPrint";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wTaxSelectPrint_FormClosing);
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitleSelectPnt;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.RadioButton orbPrnA4;
        private System.Windows.Forms.RadioButton orbPrnThermal;
        private System.Windows.Forms.TextBox otbPrnDriver;
        private System.Windows.Forms.Button ocmPrnSelect;
        private System.Windows.Forms.PrintDialog ocdPrn;
        private System.Windows.Forms.Button ocmPreview;
        private System.Windows.Forms.Button ocmPrint;
    }
}