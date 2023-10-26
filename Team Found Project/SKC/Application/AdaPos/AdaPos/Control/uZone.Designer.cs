namespace AdaPos.Control
{
    partial class uZone
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
            this.olaZone = new System.Windows.Forms.Label();
            this.olaPrice = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // olaZone
            // 
            this.olaZone.AutoSize = true;
            this.olaZone.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.olaZone.Location = new System.Drawing.Point(10, 8);
            this.olaZone.Name = "olaZone";
            this.olaZone.Size = new System.Drawing.Size(57, 21);
            this.olaZone.TabIndex = 0;
            this.olaZone.Text = "Zone 1";
            this.olaZone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // olaPrice
            // 
            this.olaPrice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaPrice.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.olaPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.olaPrice.Location = new System.Drawing.Point(0, 56);
            this.olaPrice.Name = "olaPrice";
            this.olaPrice.Size = new System.Drawing.Size(225, 35);
            this.olaPrice.TabIndex = 1;
            this.olaPrice.Text = "฿ 2.00";
            this.olaPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 27);
            this.label1.TabIndex = 2;
            this.label1.Text = "Zone 1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Visible = false;
            // 
            // uZone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.olaPrice);
            this.Controls.Add(this.olaZone);
            this.Name = "uZone";
            this.Size = new System.Drawing.Size(228, 91);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label olaZone;
        private System.Windows.Forms.Label olaPrice;
        private System.Windows.Forms.Label label1;
    }
}
