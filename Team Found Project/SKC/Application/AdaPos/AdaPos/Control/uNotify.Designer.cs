namespace AdaPos.Control
{
    partial class uNotify
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
            this.opbMsgType = new System.Windows.Forms.PictureBox();
            this.olaMsg = new System.Windows.Forms.Label();
            this.olaDate = new System.Windows.Forms.Label();
            this.opnPage = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.opbMsgType)).BeginInit();
            this.opnPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // opbMsgType
            // 
            this.opbMsgType.Location = new System.Drawing.Point(4, 4);
            this.opbMsgType.Name = "opbMsgType";
            this.opbMsgType.Size = new System.Drawing.Size(50, 50);
            this.opbMsgType.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.opbMsgType.TabIndex = 0;
            this.opbMsgType.TabStop = false;
            // 
            // olaMsg
            // 
            this.olaMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaMsg.AutoSize = true;
            this.olaMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaMsg.Location = new System.Drawing.Point(60, 8);
            this.olaMsg.Name = "olaMsg";
            this.olaMsg.Size = new System.Drawing.Size(0, 17);
            this.olaMsg.TabIndex = 1;
            this.olaMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.olaMsg.Resize += new System.EventHandler(this.olaMsg_Resize);
            // 
            // olaDate
            // 
            this.olaDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaDate.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaDate.ForeColor = System.Drawing.Color.Gray;
            this.olaDate.Location = new System.Drawing.Point(60, 31);
            this.olaDate.Name = "olaDate";
            this.olaDate.Size = new System.Drawing.Size(288, 21);
            this.olaDate.TabIndex = 2;
            this.olaDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opnPage
            // 
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.Controls.Add(this.opbMsgType);
            this.opnPage.Controls.Add(this.olaMsg);
            this.opnPage.Controls.Add(this.olaDate);
            this.opnPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnPage.Location = new System.Drawing.Point(0, 0);
            this.opnPage.Name = "opnPage";
            this.opnPage.Size = new System.Drawing.Size(352, 62);
            this.opnPage.TabIndex = 3;
            // 
            // uNotify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.opnPage);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "uNotify";
            this.Size = new System.Drawing.Size(352, 62);
            ((System.ComponentModel.ISupportInitialize)(this.opbMsgType)).EndInit();
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox opbMsgType;
        private System.Windows.Forms.Label olaMsg;
        private System.Windows.Forms.Label olaDate;
        private System.Windows.Forms.Panel opnPage;
    }
}
