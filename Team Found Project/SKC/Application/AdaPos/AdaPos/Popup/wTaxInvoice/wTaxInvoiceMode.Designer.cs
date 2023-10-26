namespace AdaPos
{
    partial class wTaxInvoiceMode
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.olaSep = new System.Windows.Forms.Label();
            this.olaWristband = new System.Windows.Forms.Label();
            this.olaDocument = new System.Windows.Forms.Label();
            this.opnDocument = new System.Windows.Forms.Panel();
            this.ocmSchDoc = new System.Windows.Forms.Button();
            this.olaTitleDocNo = new System.Windows.Forms.Label();
            this.otbDocument = new System.Windows.Forms.TextBox();
            this.opnWristband = new System.Windows.Forms.Panel();
            this.olaTitleWBNo = new System.Windows.Forms.Label();
            this.otbWristband = new System.Windows.Forms.TextBox();
            this.opnHD = new System.Windows.Forms.Panel();
            this.ocmKB = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.olaTitleTax = new System.Windows.Forms.Label();
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
            this.opnPage.Controls.Add(this.tableLayoutPanel1);
            this.opnPage.Controls.Add(this.opnDocument);
            this.opnPage.Controls.Add(this.opnWristband);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(312, 284);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(400, 200);
            this.opnPage.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 90.95238F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.047619F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 194F));
            this.tableLayoutPanel1.Controls.Add(this.olaSep, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.olaWristband, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.olaDocument, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 155);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(394, 29);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // olaSep
            // 
            this.olaSep.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.olaSep.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaSep.Location = new System.Drawing.Point(184, 0);
            this.olaSep.Name = "olaSep";
            this.olaSep.Size = new System.Drawing.Size(12, 28);
            this.olaSep.TabIndex = 12;
            this.olaSep.Text = "|";
            this.olaSep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // olaWristband
            // 
            this.olaWristband.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.olaWristband.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaWristband.Location = new System.Drawing.Point(3, 0);
            this.olaWristband.Name = "olaWristband";
            this.olaWristband.Size = new System.Drawing.Size(175, 28);
            this.olaWristband.TabIndex = 0;
            this.olaWristband.Text = "Wristband";
            this.olaWristband.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.olaWristband.Click += new System.EventHandler(this.olaWristband_Click);
            // 
            // olaDocument
            // 
            this.olaDocument.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.olaDocument.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaDocument.Location = new System.Drawing.Point(207, 0);
            this.olaDocument.Name = "olaDocument";
            this.olaDocument.Size = new System.Drawing.Size(184, 28);
            this.olaDocument.TabIndex = 11;
            this.olaDocument.Text = "Document";
            this.olaDocument.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.olaDocument.Click += new System.EventHandler(this.olaDocument_Click);
            // 
            // opnDocument
            // 
            this.opnDocument.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnDocument.Controls.Add(this.ocmSchDoc);
            this.opnDocument.Controls.Add(this.olaTitleDocNo);
            this.opnDocument.Controls.Add(this.otbDocument);
            this.opnDocument.Location = new System.Drawing.Point(10, 57);
            this.opnDocument.Name = "opnDocument";
            this.opnDocument.Size = new System.Drawing.Size(376, 93);
            this.opnDocument.TabIndex = 5;
            this.opnDocument.Visible = false;
            // 
            // ocmSchDoc
            // 
            this.ocmSchDoc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(171)))), ((int)(((byte)(132)))));
            this.ocmSchDoc.FlatAppearance.BorderSize = 0;
            this.ocmSchDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmSchDoc.Image = global::AdaPos.Properties.Resources.SearchW_32;
            this.ocmSchDoc.Location = new System.Drawing.Point(320, 50);
            this.ocmSchDoc.Name = "ocmSchDoc";
            this.ocmSchDoc.Size = new System.Drawing.Size(50, 29);
            this.ocmSchDoc.TabIndex = 1;
            this.ocmSchDoc.UseVisualStyleBackColor = false;
            this.ocmSchDoc.Click += new System.EventHandler(this.ocmSchDoc_Click);
            // 
            // olaTitleDocNo
            // 
            this.olaTitleDocNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleDocNo.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaTitleDocNo.Location = new System.Drawing.Point(6, 16);
            this.olaTitleDocNo.Name = "olaTitleDocNo";
            this.olaTitleDocNo.Size = new System.Drawing.Size(226, 22);
            this.olaTitleDocNo.TabIndex = 3;
            this.olaTitleDocNo.Text = "Wristband No. / Card No.";
            this.olaTitleDocNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // otbDocument
            // 
            this.otbDocument.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otbDocument.Location = new System.Drawing.Point(10, 50);
            this.otbDocument.MaxLength = 20;
            this.otbDocument.Name = "otbDocument";
            this.otbDocument.Size = new System.Drawing.Size(310, 29);
            this.otbDocument.TabIndex = 0;
            // 
            // opnWristband
            // 
            this.opnWristband.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnWristband.Controls.Add(this.olaTitleWBNo);
            this.opnWristband.Controls.Add(this.otbWristband);
            this.opnWristband.Location = new System.Drawing.Point(9, 56);
            this.opnWristband.Name = "opnWristband";
            this.opnWristband.Size = new System.Drawing.Size(380, 90);
            this.opnWristband.TabIndex = 4;
            // 
            // olaTitleWBNo
            // 
            this.olaTitleWBNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleWBNo.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaTitleWBNo.Location = new System.Drawing.Point(6, 16);
            this.olaTitleWBNo.Name = "olaTitleWBNo";
            this.olaTitleWBNo.Size = new System.Drawing.Size(230, 22);
            this.olaTitleWBNo.TabIndex = 3;
            this.olaTitleWBNo.Text = "Wristband No. / Card No.";
            this.olaTitleWBNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // otbWristband
            // 
            this.otbWristband.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otbWristband.Location = new System.Drawing.Point(10, 50);
            this.otbWristband.MaxLength = 20;
            this.otbWristband.Name = "otbWristband";
            this.otbWristband.Size = new System.Drawing.Size(360, 29);
            this.otbWristband.TabIndex = 0;
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.ocmKB);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Controls.Add(this.olaTitleTax);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(396, 50);
            this.opnHD.TabIndex = 1;
            // 
            // ocmKB
            // 
            this.ocmKB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmKB.FlatAppearance.BorderSize = 0;
            this.ocmKB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmKB.Image = global::AdaPos.Properties.Resources.KBB_32;
            this.ocmKB.Location = new System.Drawing.Point(294, 0);
            this.ocmKB.Name = "ocmKB";
            this.ocmKB.Size = new System.Drawing.Size(50, 50);
            this.ocmKB.TabIndex = 3;
            this.ocmKB.UseVisualStyleBackColor = true;
            this.ocmKB.Click += new System.EventHandler(this.ocmKB_Click);
            // 
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.Location = new System.Drawing.Point(350, 0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 2;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // olaTitleTax
            // 
            this.olaTitleTax.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleTax.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaTitleTax.ForeColor = System.Drawing.Color.White;
            this.olaTitleTax.Location = new System.Drawing.Point(56, 0);
            this.olaTitleTax.Name = "olaTitleTax";
            this.olaTitleTax.Size = new System.Drawing.Size(228, 50);
            this.olaTitleTax.TabIndex = 1;
            this.olaTitleTax.Text = "Tax Invoice";
            this.olaTitleTax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // wTaxInvoiceMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wTaxInvoiceMode";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wTaxInvoiceMode_FormClosing);
            this.opnPage.ResumeLayout(false);
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
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Label olaTitleTax;
        private System.Windows.Forms.Button ocmKB;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Panel opnWristband;
        private System.Windows.Forms.TextBox otbWristband;
        private System.Windows.Forms.Label olaTitleWBNo;
        private System.Windows.Forms.Panel opnDocument;
        private System.Windows.Forms.Button ocmSchDoc;
        private System.Windows.Forms.Label olaTitleDocNo;
        private System.Windows.Forms.TextBox otbDocument;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label olaSep;
        private System.Windows.Forms.Label olaWristband;
        private System.Windows.Forms.Label olaDocument;
    }
}