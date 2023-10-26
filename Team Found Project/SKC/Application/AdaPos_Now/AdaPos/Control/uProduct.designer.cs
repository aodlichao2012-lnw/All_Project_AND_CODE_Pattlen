namespace AdaPos.Control
{
    partial class uProduct
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
            this.olaPdtPrice = new System.Windows.Forms.Label();
            this.opnPage = new System.Windows.Forms.Panel();
            this.olaSaleType = new System.Windows.Forms.Label();
            this.olaFactor = new System.Windows.Forms.Label();
            this.olaBarcode = new System.Windows.Forms.Label();
            this.olaUnit = new System.Windows.Forms.Label();
            this.olaStaAlwDis = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.opbPdt)).BeginInit();
            this.opnPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // opbPdt
            // 
            this.opbPdt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opbPdt.BackColor = System.Drawing.Color.White;
            this.opbPdt.Location = new System.Drawing.Point(17, 0);
            this.opbPdt.Name = "opbPdt";
            this.opbPdt.Size = new System.Drawing.Size(80, 80);
            this.opbPdt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.opbPdt.TabIndex = 0;
            this.opbPdt.TabStop = false;
            this.opbPdt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uProduct_MouseDown);
            this.opbPdt.MouseLeave += new System.EventHandler(this.uProduct_MouseLeave);
            this.opbPdt.MouseHover += new System.EventHandler(this.uProduct_MouseHover);
            this.opbPdt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.uProduct_MouseUp);
            // 
            // olaPdtName
            // 
            this.olaPdtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaPdtName.AutoEllipsis = true;
            this.olaPdtName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaPdtName.Location = new System.Drawing.Point(0, 108);
            this.olaPdtName.Name = "olaPdtName";
            this.olaPdtName.Size = new System.Drawing.Size(112, 20);
            this.olaPdtName.TabIndex = 1;
            this.olaPdtName.Text = "NameNameNameNameNameName";
            this.olaPdtName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.olaPdtName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uProduct_MouseDown);
            this.olaPdtName.MouseLeave += new System.EventHandler(this.uProduct_MouseLeave);
            this.olaPdtName.MouseHover += new System.EventHandler(this.uProduct_MouseHover);
            this.olaPdtName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.uProduct_MouseUp);
            // 
            // olaPdtPrice
            // 
            this.olaPdtPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.olaPdtPrice.BackColor = System.Drawing.Color.Transparent;
            this.olaPdtPrice.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaPdtPrice.ForeColor = System.Drawing.Color.White;
            this.olaPdtPrice.Location = new System.Drawing.Point(0, 79);
            this.olaPdtPrice.Name = "olaPdtPrice";
            this.olaPdtPrice.Size = new System.Drawing.Size(112, 25);
            this.olaPdtPrice.TabIndex = 2;
            this.olaPdtPrice.Text = "฿100.00";
            this.olaPdtPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.olaPdtPrice.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uProduct_MouseDown);
            this.olaPdtPrice.MouseLeave += new System.EventHandler(this.uProduct_MouseLeave);
            this.olaPdtPrice.MouseHover += new System.EventHandler(this.uProduct_MouseHover);
            this.olaPdtPrice.MouseUp += new System.Windows.Forms.MouseEventHandler(this.uProduct_MouseUp);
            // 
            // opnPage
            // 
            this.opnPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnPage.BackColor = System.Drawing.Color.Transparent;
            this.opnPage.Controls.Add(this.olaSaleType);
            this.opnPage.Controls.Add(this.olaFactor);
            this.opnPage.Controls.Add(this.olaBarcode);
            this.opnPage.Controls.Add(this.olaUnit);
            this.opnPage.Controls.Add(this.olaPdtPrice);
            this.opnPage.Controls.Add(this.opbPdt);
            this.opnPage.Controls.Add(this.olaPdtName);
            this.opnPage.Controls.Add(this.olaStaAlwDis);
            this.opnPage.Location = new System.Drawing.Point(1, 1);
            this.opnPage.Name = "opnPage";
            this.opnPage.Size = new System.Drawing.Size(112, 128);
            this.opnPage.TabIndex = 3;
            // 
            // olaSaleType
            // 
            this.olaSaleType.AutoSize = true;
            this.olaSaleType.Location = new System.Drawing.Point(0, 0);
            this.olaSaleType.Name = "olaSaleType";
            this.olaSaleType.Size = new System.Drawing.Size(0, 13);
            this.olaSaleType.TabIndex = 7;
            this.olaSaleType.Visible = false;
            // 
            // olaFactor
            // 
            this.olaFactor.AutoSize = true;
            this.olaFactor.Location = new System.Drawing.Point(78, 40);
            this.olaFactor.Name = "olaFactor";
            this.olaFactor.Size = new System.Drawing.Size(0, 13);
            this.olaFactor.TabIndex = 6;
            this.olaFactor.Visible = false;
            // 
            // olaBarcode
            // 
            this.olaBarcode.AutoSize = true;
            this.olaBarcode.Location = new System.Drawing.Point(70, 40);
            this.olaBarcode.Name = "olaBarcode";
            this.olaBarcode.Size = new System.Drawing.Size(0, 13);
            this.olaBarcode.TabIndex = 5;
            this.olaBarcode.Visible = false;
            // 
            // olaUnit
            // 
            this.olaUnit.AutoSize = true;
            this.olaUnit.Location = new System.Drawing.Point(0, 0);
            this.olaUnit.Name = "olaUnit";
            this.olaUnit.Size = new System.Drawing.Size(0, 13);
            this.olaUnit.TabIndex = 4;
            this.olaUnit.Visible = false;
            // 
            // olaStaAlwDis
            // 
            this.olaStaAlwDis.AutoSize = true;
            this.olaStaAlwDis.Location = new System.Drawing.Point(37, 40);
            this.olaStaAlwDis.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.olaStaAlwDis.Name = "olaStaAlwDis";
            this.olaStaAlwDis.Size = new System.Drawing.Size(0, 13);
            this.olaStaAlwDis.TabIndex = 8;
            // 
            // uProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.opnPage);
            this.Margin = new System.Windows.Forms.Padding(8);
            this.Name = "uProduct";
            this.Size = new System.Drawing.Size(114, 130);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uProduct_MouseDown);
            this.MouseLeave += new System.EventHandler(this.uProduct_MouseLeave);
            this.MouseHover += new System.EventHandler(this.uProduct_MouseHover);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.uProduct_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.opbPdt)).EndInit();
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label olaPdtName;
        private System.Windows.Forms.Label olaPdtPrice;
        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Label olaUnit;
        private System.Windows.Forms.Label olaBarcode;
        private System.Windows.Forms.Label olaFactor;
        private System.Windows.Forms.Label olaSaleType;
        private System.Windows.Forms.Label olaStaAlwDis;
        private System.Windows.Forms.PictureBox opbPdt;
    }
}
