namespace AdaPos.Control
{
    partial class uCoupon
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
            this.olaTitleStore = new System.Windows.Forms.Label();
            this.opbLogo = new System.Windows.Forms.PictureBox();
            this.olaStore = new System.Windows.Forms.Label();
            this.olaPdtPrice = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.opbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // olaTitleStore
            // 
            this.olaTitleStore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleStore.BackColor = System.Drawing.Color.DimGray;
            this.olaTitleStore.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaTitleStore.ForeColor = System.Drawing.Color.White;
            this.olaTitleStore.Location = new System.Drawing.Point(0, 120);
            this.olaTitleStore.Name = "olaTitleStore";
            this.olaTitleStore.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.olaTitleStore.Size = new System.Drawing.Size(201, 30);
            this.olaTitleStore.TabIndex = 1;
            this.olaTitleStore.Text = "คูปองร้านค้า";
            this.olaTitleStore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opbLogo
            // 
            this.opbLogo.BackColor = System.Drawing.Color.White;
            this.opbLogo.Location = new System.Drawing.Point(0, -1);
            this.opbLogo.Name = "opbLogo";
            this.opbLogo.Size = new System.Drawing.Size(200, 121);
            this.opbLogo.TabIndex = 0;
            this.opbLogo.TabStop = false;
            // 
            // olaStore
            // 
            this.olaStore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaStore.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.olaStore.Location = new System.Drawing.Point(0, -1);
            this.olaStore.Name = "olaStore";
            this.olaStore.Size = new System.Drawing.Size(200, 121);
            this.olaStore.TabIndex = 4;
            this.olaStore.Text = "Adasoft";
            this.olaStore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // olaPdtPrice
            // 
            this.olaPdtPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.olaPdtPrice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.olaPdtPrice.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.olaPdtPrice.ForeColor = System.Drawing.Color.White;
            this.olaPdtPrice.Location = new System.Drawing.Point(80, 5);
            this.olaPdtPrice.Name = "olaPdtPrice";
            this.olaPdtPrice.Size = new System.Drawing.Size(120, 25);
            this.olaPdtPrice.TabIndex = 5;
            this.olaPdtPrice.Text = "฿100.00";
            this.olaPdtPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // uCoupon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.olaPdtPrice);
            this.Controls.Add(this.olaTitleStore);
            this.Controls.Add(this.olaStore);
            this.Controls.Add(this.opbLogo);
            this.Margin = new System.Windows.Forms.Padding(20, 10, 30, 10);
            this.Name = "uCoupon";
            this.Size = new System.Drawing.Size(200, 150);
            ((System.ComponentModel.ISupportInitialize)(this.opbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox opbLogo;
        private System.Windows.Forms.Label olaTitleStore;
        private System.Windows.Forms.Label olaStore;
        private System.Windows.Forms.Label olaPdtPrice;
    }
}
