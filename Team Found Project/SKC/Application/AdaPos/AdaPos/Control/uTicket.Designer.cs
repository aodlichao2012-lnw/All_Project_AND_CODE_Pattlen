namespace AdaPos.Control
{
    partial class uTicket
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
            this.opbTicket = new System.Windows.Forms.PictureBox();
            this.olaTKName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.opbTicket)).BeginInit();
            this.SuspendLayout();
            // 
            // opbTicket
            // 
            this.opbTicket.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.opbTicket.Location = new System.Drawing.Point(0, 0);
            this.opbTicket.Name = "opbTicket";
            this.opbTicket.Size = new System.Drawing.Size(130, 130);
            this.opbTicket.TabIndex = 0;
            this.opbTicket.TabStop = false;
            // 
            // olaTKName
            // 
            this.olaTKName.AutoSize = true;
            this.olaTKName.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaTKName.Location = new System.Drawing.Point(140, 7);
            this.olaTKName.Name = "olaTKName";
            this.olaTKName.Size = new System.Drawing.Size(53, 21);
            this.olaTKName.TabIndex = 1;
            this.olaTKName.Text = "Name";
            this.olaTKName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label5.Location = new System.Drawing.Point(241, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 25);
            this.label5.TabIndex = 6;
            this.label5.Text = "฿ 0.00";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(144, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 70);
            this.label1.TabIndex = 7;
            this.label1.Text = "Desc ";
            // 
            // uTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.olaTKName);
            this.Controls.Add(this.opbTicket);
            this.Name = "uTicket";
            this.Size = new System.Drawing.Size(310, 130);
            ((System.ComponentModel.ISupportInitialize)(this.opbTicket)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox opbTicket;
        private System.Windows.Forms.Label olaTKName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
    }
}
