namespace AdaPos.Popup.All
{
    partial class wHelp
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
            this.opnHeader = new System.Windows.Forms.Panel();
            this.olaTel = new System.Windows.Forms.Label();
            this.olaEmail = new System.Windows.Forms.Label();
            this.opnHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnHeader
            // 
            this.opnHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHeader.BackColor = System.Drawing.Color.White;
            this.opnHeader.Controls.Add(this.olaTel);
            this.opnHeader.Controls.Add(this.olaEmail);
            this.opnHeader.Location = new System.Drawing.Point(0, 0);
            this.opnHeader.Name = "opnHeader";
            this.opnHeader.Size = new System.Drawing.Size(1024, 50);
            this.opnHeader.TabIndex = 0;
            // 
            // olaTel
            // 
            this.olaTel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTel.AutoSize = true;
            this.olaTel.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaTel.ForeColor = System.Drawing.Color.DimGray;
            this.olaTel.Location = new System.Drawing.Point(683, 16);
            this.olaTel.Name = "olaTel";
            this.olaTel.Size = new System.Drawing.Size(136, 19);
            this.olaTel.TabIndex = 1;
            this.olaTel.Text = "+662-530-1681 (Auto)";
            this.olaTel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaEmail
            // 
            this.olaEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.olaEmail.AutoSize = true;
            this.olaEmail.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaEmail.ForeColor = System.Drawing.Color.DimGray;
            this.olaEmail.Location = new System.Drawing.Point(862, 16);
            this.olaEmail.Name = "olaEmail";
            this.olaEmail.Size = new System.Drawing.Size(145, 19);
            this.olaEmail.TabIndex = 0;
            this.olaEmail.Text = "Adapos@ada-soft.com";
            this.olaEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // wHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "wHelp";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Click += new System.EventHandler(this.wHelp_Click);
            this.opnHeader.ResumeLayout(false);
            this.opnHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnHeader;
        private System.Windows.Forms.Label olaTel;
        private System.Windows.Forms.Label olaEmail;
    }
}