namespace AdaPos.Control
{
    partial class uMsgQueue
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
            this.opnPage = new System.Windows.Forms.Panel();
            this.opbMsgType = new System.Windows.Forms.PictureBox();
            this.olaMsg = new System.Windows.Forms.Label();
            this.olaDate = new System.Windows.Forms.Label();
            this.olaQData = new System.Windows.Forms.Label();
            this.opnPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opbMsgType)).BeginInit();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.Controls.Add(this.opbMsgType);
            this.opnPage.Controls.Add(this.olaMsg);
            this.opnPage.Controls.Add(this.olaDate);
            this.opnPage.Controls.Add(this.olaQData);
            this.opnPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnPage.Location = new System.Drawing.Point(0, 0);
            this.opnPage.Name = "opnPage";
            this.opnPage.Size = new System.Drawing.Size(352, 80);
            this.opnPage.TabIndex = 4;
            this.opnPage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uMsgQueue_MouseDown);
            this.opnPage.MouseLeave += new System.EventHandler(this.uMsgQueue_MouseLeave);
            this.opnPage.MouseHover += new System.EventHandler(this.uMsgQueue_MouseHover);
            this.opnPage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.uMsgQueue_MouseUp);
            // 
            // opbMsgType
            // 
            this.opbMsgType.Location = new System.Drawing.Point(4, 10);
            this.opbMsgType.Name = "opbMsgType";
            this.opbMsgType.Size = new System.Drawing.Size(60, 60);
            this.opbMsgType.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.opbMsgType.TabIndex = 0;
            this.opbMsgType.TabStop = false;
            this.opbMsgType.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uMsgQueue_MouseDown);
            this.opbMsgType.MouseLeave += new System.EventHandler(this.uMsgQueue_MouseLeave);
            this.opbMsgType.MouseHover += new System.EventHandler(this.uMsgQueue_MouseHover);
            this.opbMsgType.MouseUp += new System.Windows.Forms.MouseEventHandler(this.uMsgQueue_MouseUp);
            // 
            // olaMsg
            // 
            this.olaMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaMsg.AutoSize = true;
            this.olaMsg.Font = new System.Drawing.Font("Segoe UI Historic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaMsg.Location = new System.Drawing.Point(75, 10);
            this.olaMsg.Name = "olaMsg";
            this.olaMsg.Size = new System.Drawing.Size(0, 19);
            this.olaMsg.TabIndex = 1;
            this.olaMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.olaMsg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uMsgQueue_MouseDown);
            this.olaMsg.MouseLeave += new System.EventHandler(this.uMsgQueue_MouseLeave);
            this.olaMsg.MouseHover += new System.EventHandler(this.uMsgQueue_MouseHover);
            this.olaMsg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.uMsgQueue_MouseUp);
            // 
            // olaDate
            // 
            this.olaDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaDate.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaDate.ForeColor = System.Drawing.Color.Gray;
            this.olaDate.Location = new System.Drawing.Point(75, 50);
            this.olaDate.Name = "olaDate";
            this.olaDate.Size = new System.Drawing.Size(270, 21);
            this.olaDate.TabIndex = 2;
            this.olaDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.olaDate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uMsgQueue_MouseDown);
            this.olaDate.MouseLeave += new System.EventHandler(this.uMsgQueue_MouseLeave);
            this.olaDate.MouseHover += new System.EventHandler(this.uMsgQueue_MouseHover);
            this.olaDate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.uMsgQueue_MouseUp);
            // 
            // olaQData
            // 
            this.olaQData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaQData.AutoSize = true;
            this.olaQData.Font = new System.Drawing.Font("Segoe UI Historic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaQData.Location = new System.Drawing.Point(176, 31);
            this.olaQData.Name = "olaQData";
            this.olaQData.Size = new System.Drawing.Size(0, 19);
            this.olaQData.TabIndex = 3;
            this.olaQData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.olaQData.Visible = false;
            this.olaQData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uMsgQueue_MouseDown);
            this.olaQData.MouseLeave += new System.EventHandler(this.uMsgQueue_MouseLeave);
            this.olaQData.MouseHover += new System.EventHandler(this.uMsgQueue_MouseHover);
            this.olaQData.MouseUp += new System.Windows.Forms.MouseEventHandler(this.uMsgQueue_MouseUp);
            // 
            // uMsgQueue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.opnPage);
            this.Name = "uMsgQueue";
            this.Size = new System.Drawing.Size(352, 80);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uMsgQueue_MouseDown);
            this.MouseLeave += new System.EventHandler(this.uMsgQueue_MouseLeave);
            this.MouseHover += new System.EventHandler(this.uMsgQueue_MouseHover);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.uMsgQueue_MouseUp);
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opbMsgType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.PictureBox opbMsgType;
        private System.Windows.Forms.Label olaMsg;
        private System.Windows.Forms.Label olaDate;
        private System.Windows.Forms.Label olaQData;
    }
}
