namespace AdaPos.Forms
{
    partial class wProgress
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
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitle = new System.Windows.Forms.Label();
            this.opnContent = new System.Windows.Forms.Panel();
            this.opaStatus = new System.Windows.Forms.Panel();
            this.olaStaTblName = new System.Windows.Forms.Label();
            this.olaStatus = new System.Windows.Forms.Label();
            this.olaStatusTable = new System.Windows.Forms.Label();
            this.opgStatus = new System.Windows.Forms.ProgressBar();
            this.opnPage = new System.Windows.Forms.Panel();
            this.opnHD.SuspendLayout();
            this.opnContent.SuspendLayout();
            this.opaStatus.SuspendLayout();
            this.opnPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnHD
            // 
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitle);
            this.opnHD.Dock = System.Windows.Forms.DockStyle.Top;
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.opnHD.Name = "opnHD";
            this.opnHD.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.opnHD.Size = new System.Drawing.Size(891, 62);
            this.opnHD.TabIndex = 3;
            // 
            // olaTitle
            // 
            this.olaTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.olaTitle.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitle.ForeColor = System.Drawing.Color.White;
            this.olaTitle.Location = new System.Drawing.Point(13, 0);
            this.olaTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaTitle.Name = "olaTitle";
            this.olaTitle.Size = new System.Drawing.Size(504, 62);
            this.olaTitle.TabIndex = 7;
            this.olaTitle.Text = "Update Data";
            this.olaTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opnContent
            // 
            this.opnContent.BackColor = System.Drawing.Color.White;
            this.opnContent.Controls.Add(this.opaStatus);
            this.opnContent.Controls.Add(this.olaStatusTable);
            this.opnContent.Controls.Add(this.opgStatus);
            this.opnContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnContent.Location = new System.Drawing.Point(1, 63);
            this.opnContent.Margin = new System.Windows.Forms.Padding(0);
            this.opnContent.Name = "opnContent";
            this.opnContent.Padding = new System.Windows.Forms.Padding(27, 12, 27, 25);
            this.opnContent.Size = new System.Drawing.Size(891, 100);
            this.opnContent.TabIndex = 4;
            // 
            // opaStatus
            // 
            this.opaStatus.Controls.Add(this.olaStaTblName);
            this.opaStatus.Controls.Add(this.olaStatus);
            this.opaStatus.Location = new System.Drawing.Point(31, 16);
            this.opaStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.opaStatus.Name = "opaStatus";
            this.opaStatus.Size = new System.Drawing.Size(669, 22);
            this.opaStatus.TabIndex = 8;
            // 
            // olaStaTblName
            // 
            this.olaStaTblName.AutoSize = true;
            this.olaStaTblName.Dock = System.Windows.Forms.DockStyle.Left;
            this.olaStaTblName.Font = new System.Drawing.Font("Segoe UI Light", 10F);
            this.olaStaTblName.Location = new System.Drawing.Point(61, 0);
            this.olaStaTblName.Name = "olaStaTblName";
            this.olaStaTblName.Size = new System.Drawing.Size(0, 23);
            this.olaStaTblName.TabIndex = 8;
            // 
            // olaStatus
            // 
            this.olaStatus.AutoSize = true;
            this.olaStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.olaStatus.Font = new System.Drawing.Font("Segoe UI Light", 10F);
            this.olaStatus.Location = new System.Drawing.Point(0, 0);
            this.olaStatus.Name = "olaStatus";
            this.olaStatus.Size = new System.Drawing.Size(61, 23);
            this.olaStatus.TabIndex = 5;
            this.olaStatus.Text = "Status :";
            // 
            // olaStatusTable
            // 
            this.olaStatusTable.Font = new System.Drawing.Font("Segoe UI Light", 10F);
            this.olaStatusTable.Location = new System.Drawing.Point(777, 12);
            this.olaStatusTable.Name = "olaStatusTable";
            this.olaStatusTable.Size = new System.Drawing.Size(83, 23);
            this.olaStatusTable.TabIndex = 6;
            this.olaStatusTable.Text = "(0/0)";
            this.olaStatusTable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // opgStatus
            // 
            this.opgStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opgStatus.Location = new System.Drawing.Point(29, 38);
            this.opgStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.opgStatus.Name = "opgStatus";
            this.opgStatus.Size = new System.Drawing.Size(834, 34);
            this.opgStatus.TabIndex = 4;
            // 
            // opnPage
            // 
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.opnContent);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnPage.Location = new System.Drawing.Point(0, 0);
            this.opnPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(895, 166);
            this.opnPage.TabIndex = 5;
            // 
            // wProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(895, 166);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "wProgress";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wAutoSync";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.wProgress_FormClosed);
            this.Load += new System.EventHandler(this.wAutoSync_Load);
            this.Shown += new System.EventHandler(this.wProgress_Shown);
            this.opnHD.ResumeLayout(false);
            this.opnContent.ResumeLayout(false);
            this.opaStatus.ResumeLayout(false);
            this.opaStatus.PerformLayout();
            this.opnPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitle;
        private System.Windows.Forms.Panel opnContent;
        private System.Windows.Forms.Label olaStatus;
        private System.Windows.Forms.Panel opnPage;
        public System.Windows.Forms.Label olaStatusTable;
        public System.Windows.Forms.ProgressBar opgStatus;
        private System.Windows.Forms.Panel opaStatus;
        private System.Windows.Forms.Label olaStaTblName;
    }
}