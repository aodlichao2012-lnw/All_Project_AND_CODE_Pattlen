namespace AdaPos.Popup.wSale
{
    partial class wReferBill
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
            this.ockRefer = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.olaSep = new System.Windows.Forms.Label();
            this.olaWristband = new System.Windows.Forms.Label();
            this.olaDocument = new System.Windows.Forms.Label();
            this.opnDocument = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.otpSaleDate = new System.Windows.Forms.DateTimePicker();
            this.ocmSchDoc = new System.Windows.Forms.Button();
            this.olaTitleDocNo = new System.Windows.Forms.Label();
            this.otbDocument = new System.Windows.Forms.TextBox();
            this.opnWristband = new System.Windows.Forms.Panel();
            this.otbWristband = new System.Windows.Forms.TextBox();
            this.olaTitleWB = new System.Windows.Forms.Label();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitleReferBill = new System.Windows.Forms.Label();
            this.ocmKB = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.ocmBack = new System.Windows.Forms.Button();
            this.opnPage.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.opnDocument.SuspendLayout();
            this.opnWristband.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.ockRefer);
            this.opnPage.Controls.Add(this.tableLayoutPanel1);
            this.opnPage.Controls.Add(this.opnDocument);
            this.opnPage.Controls.Add(this.opnWristband);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(416, 331);
            this.opnPage.Margin = new System.Windows.Forms.Padding(4);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(533, 310);
            this.opnPage.TabIndex = 0;
            // 
            // ockRefer
            // 
            this.ockRefer.AutoSize = true;
            this.ockRefer.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ockRefer.ForeColor = System.Drawing.Color.Black;
            this.ockRefer.Location = new System.Drawing.Point(29, 66);
            this.ockRefer.Margin = new System.Windows.Forms.Padding(4);
            this.ockRefer.Name = "ockRefer";
            this.ockRefer.Size = new System.Drawing.Size(128, 32);
            this.ockRefer.TabIndex = 8;
            this.ockRefer.Text = "Refer to bill";
            this.ockRefer.UseVisualStyleBackColor = true;
            this.ockRefer.CheckedChanged += new System.EventHandler(this.ockRefer_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 90.95238F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.047619F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 264F));
            this.tableLayoutPanel1.Controls.Add(this.olaSep, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.olaWristband, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.olaDocument, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 270);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(525, 36);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // olaSep
            // 
            this.olaSep.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.olaSep.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaSep.Location = new System.Drawing.Point(241, 1);
            this.olaSep.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaSep.Name = "olaSep";
            this.olaSep.Size = new System.Drawing.Size(15, 34);
            this.olaSep.TabIndex = 12;
            this.olaSep.Text = "|";
            this.olaSep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // olaWristband
            // 
            this.olaWristband.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.olaWristband.Enabled = false;
            this.olaWristband.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaWristband.Location = new System.Drawing.Point(4, 1);
            this.olaWristband.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaWristband.Name = "olaWristband";
            this.olaWristband.Size = new System.Drawing.Size(229, 34);
            this.olaWristband.TabIndex = 0;
            this.olaWristband.Text = "Wristband";
            this.olaWristband.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.olaWristband.Click += new System.EventHandler(this.ocmWristband_Click);
            // 
            // olaDocument
            // 
            this.olaDocument.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.olaDocument.Enabled = false;
            this.olaDocument.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaDocument.Location = new System.Drawing.Point(276, 1);
            this.olaDocument.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaDocument.Name = "olaDocument";
            this.olaDocument.Size = new System.Drawing.Size(245, 34);
            this.olaDocument.TabIndex = 11;
            this.olaDocument.Text = "Document";
            this.olaDocument.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.olaDocument.Click += new System.EventHandler(this.ocmDocument_Click);
            // 
            // opnDocument
            // 
            this.opnDocument.BackColor = System.Drawing.Color.DimGray;
            this.opnDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnDocument.Controls.Add(this.label1);
            this.opnDocument.Controls.Add(this.otpSaleDate);
            this.opnDocument.Controls.Add(this.ocmSchDoc);
            this.opnDocument.Controls.Add(this.olaTitleDocNo);
            this.opnDocument.Controls.Add(this.otbDocument);
            this.opnDocument.Location = new System.Drawing.Point(28, 105);
            this.opnDocument.Margin = new System.Windows.Forms.Padding(4);
            this.opnDocument.Name = "opnDocument";
            this.opnDocument.Size = new System.Drawing.Size(477, 157);
            this.opnDocument.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(427, 28);
            this.label1.TabIndex = 10;
            this.label1.Text = "Sale Date";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // otpSaleDate
            // 
            this.otpSaleDate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.otpSaleDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.otpSaleDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.otpSaleDate.Enabled = false;
            this.otpSaleDate.Font = new System.Drawing.Font("Segoe UI Light", 13F);
            this.otpSaleDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.otpSaleDate.Location = new System.Drawing.Point(31, 39);
            this.otpSaleDate.Margin = new System.Windows.Forms.Padding(0);
            this.otpSaleDate.Name = "otpSaleDate";
            this.otpSaleDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.otpSaleDate.Size = new System.Drawing.Size(359, 36);
            this.otpSaleDate.TabIndex = 9;
            this.otpSaleDate.Value = new System.DateTime(2019, 8, 28, 13, 9, 0, 0);
            // 
            // ocmSchDoc
            // 
            this.ocmSchDoc.BackColor = System.Drawing.Color.Silver;
            this.ocmSchDoc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmSchDoc.Enabled = false;
            this.ocmSchDoc.FlatAppearance.BorderSize = 0;
            this.ocmSchDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmSchDoc.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmSchDoc.ForeColor = System.Drawing.Color.White;
            this.ocmSchDoc.Image = global::AdaPos.Properties.Resources.SearchW_32;
            this.ocmSchDoc.Location = new System.Drawing.Point(387, 109);
            this.ocmSchDoc.Margin = new System.Windows.Forms.Padding(0);
            this.ocmSchDoc.Name = "ocmSchDoc";
            this.ocmSchDoc.Size = new System.Drawing.Size(67, 36);
            this.ocmSchDoc.TabIndex = 8;
            this.ocmSchDoc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmSchDoc.UseVisualStyleBackColor = false;
            this.ocmSchDoc.Click += new System.EventHandler(this.ocmSchDoc_Click);
            // 
            // olaTitleDocNo
            // 
            this.olaTitleDocNo.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaTitleDocNo.Location = new System.Drawing.Point(27, 81);
            this.olaTitleDocNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaTitleDocNo.Name = "olaTitleDocNo";
            this.olaTitleDocNo.Size = new System.Drawing.Size(427, 28);
            this.olaTitleDocNo.TabIndex = 0;
            this.olaTitleDocNo.Text = "Document No.";
            this.olaTitleDocNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // otbDocument
            // 
            this.otbDocument.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbDocument.Enabled = false;
            this.otbDocument.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.otbDocument.Location = new System.Drawing.Point(31, 109);
            this.otbDocument.Margin = new System.Windows.Forms.Padding(4);
            this.otbDocument.Name = "otbDocument";
            this.otbDocument.ReadOnly = true;
            this.otbDocument.Size = new System.Drawing.Size(359, 34);
            this.otbDocument.TabIndex = 1;
            this.otbDocument.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbDocument_KeyDown);
            // 
            // opnWristband
            // 
            this.opnWristband.BackColor = System.Drawing.Color.DimGray;
            this.opnWristband.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnWristband.Controls.Add(this.otbWristband);
            this.opnWristband.Controls.Add(this.olaTitleWB);
            this.opnWristband.Location = new System.Drawing.Point(28, 96);
            this.opnWristband.Margin = new System.Windows.Forms.Padding(4);
            this.opnWristband.Name = "opnWristband";
            this.opnWristband.Size = new System.Drawing.Size(477, 137);
            this.opnWristband.TabIndex = 9;
            // 
            // otbWristband
            // 
            this.otbWristband.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbWristband.Enabled = false;
            this.otbWristband.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.otbWristband.Location = new System.Drawing.Point(27, 74);
            this.otbWristband.Margin = new System.Windows.Forms.Padding(4);
            this.otbWristband.Name = "otbWristband";
            this.otbWristband.ReadOnly = true;
            this.otbWristband.Size = new System.Drawing.Size(423, 34);
            this.otbWristband.TabIndex = 1;
            // 
            // olaTitleWB
            // 
            this.olaTitleWB.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaTitleWB.Location = new System.Drawing.Point(27, 25);
            this.olaTitleWB.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaTitleWB.Name = "olaTitleWB";
            this.olaTitleWB.Size = new System.Drawing.Size(427, 28);
            this.olaTitleWB.TabIndex = 0;
            this.olaTitleWB.Text = "Wristband No. / Card No.";
            this.olaTitleWB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opnHD
            // 
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitleReferBill);
            this.opnHD.Controls.Add(this.ocmKB);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Dock = System.Windows.Forms.DockStyle.Top;
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Margin = new System.Windows.Forms.Padding(4);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(529, 62);
            this.opnHD.TabIndex = 1;
            // 
            // olaTitleReferBill
            // 
            this.olaTitleReferBill.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleReferBill.BackColor = System.Drawing.Color.Transparent;
            this.olaTitleReferBill.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleReferBill.ForeColor = System.Drawing.Color.White;
            this.olaTitleReferBill.Location = new System.Drawing.Point(67, 0);
            this.olaTitleReferBill.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaTitleReferBill.Name = "olaTitleReferBill";
            this.olaTitleReferBill.Size = new System.Drawing.Size(329, 62);
            this.olaTitleReferBill.TabIndex = 10;
            this.olaTitleReferBill.Text = "ReferBill";
            this.olaTitleReferBill.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ocmKB
            // 
            this.ocmKB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmKB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmKB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmKB.FlatAppearance.BorderSize = 0;
            this.ocmKB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmKB.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmKB.ForeColor = System.Drawing.Color.White;
            this.ocmKB.Image = global::AdaPos.Properties.Resources.KBB_32;
            this.ocmKB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmKB.Location = new System.Drawing.Point(400, 0);
            this.ocmKB.Margin = new System.Windows.Forms.Padding(0);
            this.ocmKB.Name = "ocmKB";
            this.ocmKB.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.ocmKB.Size = new System.Drawing.Size(67, 62);
            this.ocmKB.TabIndex = 7;
            this.ocmKB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmKB.UseVisualStyleBackColor = false;
            this.ocmKB.Click += new System.EventHandler(this.ocmKB_Click);
            // 
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmAccept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmAccept.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmAccept.ForeColor = System.Drawing.Color.White;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmAccept.Location = new System.Drawing.Point(467, 0);
            this.ocmAccept.Margin = new System.Windows.Forms.Padding(0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.ocmAccept.Size = new System.Drawing.Size(67, 62);
            this.ocmAccept.TabIndex = 6;
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
            this.ocmBack.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.ocmBack.Size = new System.Drawing.Size(67, 62);
            this.ocmBack.TabIndex = 5;
            this.ocmBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmBack.UseVisualStyleBackColor = false;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // wReferBill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1365, 945);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "wReferBill";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wReferBill_FormClosing);
            this.Shown += new System.EventHandler(this.wReferBill_Shown);
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.opnDocument.ResumeLayout(false);
            this.opnDocument.PerformLayout();
            this.opnWristband.ResumeLayout(false);
            this.opnWristband.PerformLayout();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Button ocmKB;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.CheckBox ockRefer;
        private System.Windows.Forms.Label olaTitleWB;
        private System.Windows.Forms.TextBox otbWristband;
        private System.Windows.Forms.TextBox otbDocument;
        private System.Windows.Forms.Label olaTitleDocNo;
        private System.Windows.Forms.Button ocmSchDoc;
        private System.Windows.Forms.Panel opnDocument;
        private System.Windows.Forms.Panel opnWristband;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label olaWristband;
        private System.Windows.Forms.Label olaSep;
        private System.Windows.Forms.Label olaDocument;
        private System.Windows.Forms.Label olaTitleReferBill;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker otpSaleDate;
    }
}