namespace AdaPos.Control
{
    partial class uPdtPrice
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.opbPdt = new System.Windows.Forms.PictureBox();
            this.olaPdtName = new System.Windows.Forms.Label();
            this.olaTItleAmount = new System.Windows.Forms.Label();
            this.olaTitleQty = new System.Windows.Forms.Label();
            this.oudNum = new System.Windows.Forms.NumericUpDown();
            this.olaAmount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ocmAccept = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.opbPdt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oudNum)).BeginInit();
            this.SuspendLayout();
            // 
            // opbPdt
            // 
            this.opbPdt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.opbPdt.Location = new System.Drawing.Point(0, 0);
            this.opbPdt.Name = "opbPdt";
            this.opbPdt.Size = new System.Drawing.Size(130, 150);
            this.opbPdt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.opbPdt.TabIndex = 0;
            this.opbPdt.TabStop = false;
            // 
            // olaPdtName
            // 
            this.olaPdtName.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.olaPdtName.Location = new System.Drawing.Point(140, 10);
            this.olaPdtName.Name = "olaPdtName";
            this.olaPdtName.Size = new System.Drawing.Size(200, 23);
            this.olaPdtName.TabIndex = 1;
            this.olaPdtName.Text = "Name";
            // 
            // olaTItleAmount
            // 
            this.olaTItleAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.olaTItleAmount.Location = new System.Drawing.Point(142, 49);
            this.olaTItleAmount.Name = "olaTItleAmount";
            this.olaTItleAmount.Size = new System.Drawing.Size(56, 23);
            this.olaTItleAmount.TabIndex = 2;
            this.olaTItleAmount.Text = "Amount";
            this.olaTItleAmount.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // olaTitleQty
            // 
            this.olaTitleQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.olaTitleQty.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.olaTitleQty.Location = new System.Drawing.Point(140, 118);
            this.olaTitleQty.Name = "olaTitleQty";
            this.olaTitleQty.Size = new System.Drawing.Size(47, 23);
            this.olaTitleQty.TabIndex = 3;
            this.olaTitleQty.Text = "Qty :";
            this.olaTitleQty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // oudNum
            // 
            this.oudNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.oudNum.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.oudNum.Location = new System.Drawing.Point(194, 118);
            this.oudNum.Name = "oudNum";
            this.oudNum.ReadOnly = true;
            this.oudNum.Size = new System.Drawing.Size(61, 25);
            this.oudNum.TabIndex = 5;
            this.oudNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // olaAmount
            // 
            this.olaAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.olaAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.olaAmount.Location = new System.Drawing.Point(204, 43);
            this.olaAmount.Name = "olaAmount";
            this.olaAmount.Size = new System.Drawing.Size(138, 34);
            this.olaAmount.TabIndex = 6;
            this.olaAmount.Text = "฿1,234.00";
            this.olaAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(143, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "1 X 1,234.00";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmAccept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(171)))), ((int)(((byte)(132)))));
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.Location = new System.Drawing.Point(281, 114);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(59, 32);
            this.ocmAccept.TabIndex = 8;
            this.ocmAccept.UseVisualStyleBackColor = false;
            // 
            // uPdtPrice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ocmAccept);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.olaAmount);
            this.Controls.Add(this.oudNum);
            this.Controls.Add(this.olaTitleQty);
            this.Controls.Add(this.olaTItleAmount);
            this.Controls.Add(this.olaPdtName);
            this.Controls.Add(this.opbPdt);
            this.Name = "uPdtPrice";
            this.Size = new System.Drawing.Size(345, 150);
            ((System.ComponentModel.ISupportInitialize)(this.opbPdt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oudNum)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox opbPdt;
        private System.Windows.Forms.Label olaPdtName;
        private System.Windows.Forms.Label olaTItleAmount;
        private System.Windows.Forms.Label olaTitleQty;
        private System.Windows.Forms.NumericUpDown oudNum;
        private System.Windows.Forms.Label olaAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ocmAccept;
    }
}
