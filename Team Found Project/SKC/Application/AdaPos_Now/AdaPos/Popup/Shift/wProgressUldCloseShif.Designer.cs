namespace AdaPos.Popup.Shift
{
    partial class wProgressUldCloseShif
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
            this.components = new System.ComponentModel.Container();
            this.opnPage = new System.Windows.Forms.Panel();
            this.opnContent = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.opgStaVod = new System.Windows.Forms.ProgressBar();
            this.olaVodTbl = new System.Windows.Forms.Label();
            this.olaStaVod = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.opgStaSal = new System.Windows.Forms.ProgressBar();
            this.olaSalTbl = new System.Windows.Forms.Label();
            this.olaStaSal = new System.Windows.Forms.Label();
            this.opnShf = new System.Windows.Forms.Panel();
            this.opgStaShf = new System.Windows.Forms.ProgressBar();
            this.olaShfTbl = new System.Windows.Forms.Label();
            this.olaStaShf = new System.Windows.Forms.Label();
            this.olaErrNotConn = new System.Windows.Forms.Label();
            this.ocmReSync = new System.Windows.Forms.Button();
            this.opnHD = new System.Windows.Forms.Panel();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.olaTitle = new System.Windows.Forms.Label();
            this.otmTimer = new System.Windows.Forms.Timer(this.components);
            this.otmClose = new System.Windows.Forms.Timer(this.components);
            this.opnPage.SuspendLayout();
            this.opnContent.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.opnShf.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.opnContent);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnPage.Location = new System.Drawing.Point(0, 0);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(671, 366);
            this.opnPage.TabIndex = 7;
            // 
            // opnContent
            // 
            this.opnContent.BackColor = System.Drawing.Color.White;
            this.opnContent.Controls.Add(this.panel2);
            this.opnContent.Controls.Add(this.panel1);
            this.opnContent.Controls.Add(this.opnShf);
            this.opnContent.Controls.Add(this.olaErrNotConn);
            this.opnContent.Controls.Add(this.ocmReSync);
            this.opnContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnContent.Location = new System.Drawing.Point(1, 51);
            this.opnContent.Margin = new System.Windows.Forms.Padding(0);
            this.opnContent.Name = "opnContent";
            this.opnContent.Padding = new System.Windows.Forms.Padding(20, 10, 20, 20);
            this.opnContent.Size = new System.Drawing.Size(667, 312);
            this.opnContent.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.opgStaVod);
            this.panel2.Controls.Add(this.olaVodTbl);
            this.panel2.Controls.Add(this.olaStaVod);
            this.panel2.Location = new System.Drawing.Point(24, 189);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(625, 65);
            this.panel2.TabIndex = 23;
            // 
            // opgStaVod
            // 
            this.opgStaVod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opgStaVod.Location = new System.Drawing.Point(2, 39);
            this.opgStaVod.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.opgStaVod.Name = "opgStaVod";
            this.opgStaVod.Size = new System.Drawing.Size(620, 19);
            this.opgStaVod.TabIndex = 8;
            // 
            // olaVodTbl
            // 
            this.olaVodTbl.AutoSize = true;
            this.olaVodTbl.Font = new System.Drawing.Font("Segoe UI Light", 10F);
            this.olaVodTbl.Location = new System.Drawing.Point(2, 11);
            this.olaVodTbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.olaVodTbl.Name = "olaVodTbl";
            this.olaVodTbl.Size = new System.Drawing.Size(50, 19);
            this.olaVodTbl.TabIndex = 7;
            this.olaVodTbl.Text = "Status :";
            // 
            // olaStaVod
            // 
            this.olaStaVod.Font = new System.Drawing.Font("Segoe UI Light", 10F);
            this.olaStaVod.Location = new System.Drawing.Point(512, 11);
            this.olaStaVod.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.olaStaVod.Name = "olaStaVod";
            this.olaStaVod.Size = new System.Drawing.Size(111, 19);
            this.olaStaVod.TabIndex = 6;
            this.olaStaVod.Text = "(0/0)";
            this.olaStaVod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.opgStaSal);
            this.panel1.Controls.Add(this.olaSalTbl);
            this.panel1.Controls.Add(this.olaStaSal);
            this.panel1.Location = new System.Drawing.Point(24, 110);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(625, 65);
            this.panel1.TabIndex = 22;
            // 
            // opgStaSal
            // 
            this.opgStaSal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opgStaSal.Location = new System.Drawing.Point(2, 39);
            this.opgStaSal.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.opgStaSal.Name = "opgStaSal";
            this.opgStaSal.Size = new System.Drawing.Size(620, 19);
            this.opgStaSal.TabIndex = 8;
            // 
            // olaSalTbl
            // 
            this.olaSalTbl.AutoSize = true;
            this.olaSalTbl.Font = new System.Drawing.Font("Segoe UI Light", 10F);
            this.olaSalTbl.Location = new System.Drawing.Point(2, 11);
            this.olaSalTbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.olaSalTbl.Name = "olaSalTbl";
            this.olaSalTbl.Size = new System.Drawing.Size(50, 19);
            this.olaSalTbl.TabIndex = 7;
            this.olaSalTbl.Text = "Status :";
            // 
            // olaStaSal
            // 
            this.olaStaSal.Font = new System.Drawing.Font("Segoe UI Light", 10F);
            this.olaStaSal.Location = new System.Drawing.Point(512, 11);
            this.olaStaSal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.olaStaSal.Name = "olaStaSal";
            this.olaStaSal.Size = new System.Drawing.Size(111, 19);
            this.olaStaSal.TabIndex = 6;
            this.olaStaSal.Text = "(0/0)";
            this.olaStaSal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // opnShf
            // 
            this.opnShf.Controls.Add(this.opgStaShf);
            this.opnShf.Controls.Add(this.olaShfTbl);
            this.opnShf.Controls.Add(this.olaStaShf);
            this.opnShf.Location = new System.Drawing.Point(24, 31);
            this.opnShf.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.opnShf.Name = "opnShf";
            this.opnShf.Size = new System.Drawing.Size(625, 65);
            this.opnShf.TabIndex = 21;
            // 
            // opgStaShf
            // 
            this.opgStaShf.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opgStaShf.Location = new System.Drawing.Point(2, 39);
            this.opgStaShf.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.opgStaShf.Name = "opgStaShf";
            this.opgStaShf.Size = new System.Drawing.Size(620, 19);
            this.opgStaShf.TabIndex = 8;
            // 
            // olaShfTbl
            // 
            this.olaShfTbl.AutoSize = true;
            this.olaShfTbl.Font = new System.Drawing.Font("Segoe UI Light", 10F);
            this.olaShfTbl.Location = new System.Drawing.Point(2, 11);
            this.olaShfTbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.olaShfTbl.Name = "olaShfTbl";
            this.olaShfTbl.Size = new System.Drawing.Size(50, 19);
            this.olaShfTbl.TabIndex = 7;
            this.olaShfTbl.Text = "Status :";
            // 
            // olaStaShf
            // 
            this.olaStaShf.Font = new System.Drawing.Font("Segoe UI Light", 10F);
            this.olaStaShf.Location = new System.Drawing.Point(512, 11);
            this.olaStaShf.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.olaStaShf.Name = "olaStaShf";
            this.olaStaShf.Size = new System.Drawing.Size(111, 19);
            this.olaStaShf.TabIndex = 6;
            this.olaStaShf.Text = "(0/0)";
            this.olaStaShf.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaErrNotConn
            // 
            this.olaErrNotConn.AutoSize = true;
            this.olaErrNotConn.Font = new System.Drawing.Font("Segoe UI Light", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaErrNotConn.Location = new System.Drawing.Point(23, 10);
            this.olaErrNotConn.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.olaErrNotConn.Name = "olaErrNotConn";
            this.olaErrNotConn.Size = new System.Drawing.Size(42, 19);
            this.olaErrNotConn.TabIndex = 19;
            this.olaErrNotConn.Text = "label1";
            // 
            // ocmReSync
            // 
            this.ocmReSync.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmReSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmReSync.ForeColor = System.Drawing.Color.White;
            this.ocmReSync.Location = new System.Drawing.Point(568, 271);
            this.ocmReSync.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ocmReSync.Name = "ocmReSync";
            this.ocmReSync.Size = new System.Drawing.Size(77, 33);
            this.ocmReSync.TabIndex = 18;
            this.ocmReSync.Text = "ReSync";
            this.ocmReSync.UseVisualStyleBackColor = false;
            this.ocmReSync.Visible = false;
            // 
            // opnHD
            // 
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Controls.Add(this.olaTitle);
            this.opnHD.Dock = System.Windows.Forms.DockStyle.Top;
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.opnHD.Size = new System.Drawing.Size(667, 50);
            this.opnHD.TabIndex = 3;
            // 
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmAccept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmAccept.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmAccept.ForeColor = System.Drawing.Color.White;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmAccept.Location = new System.Drawing.Point(618, 0);
            this.ocmAccept.Margin = new System.Windows.Forms.Padding(0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 9;
            this.ocmAccept.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmAccept.UseVisualStyleBackColor = false;
            this.ocmAccept.Visible = false;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // olaTitle
            // 
            this.olaTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.olaTitle.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitle.ForeColor = System.Drawing.Color.White;
            this.olaTitle.Location = new System.Drawing.Point(10, 0);
            this.olaTitle.Name = "olaTitle";
            this.olaTitle.Size = new System.Drawing.Size(378, 50);
            this.olaTitle.TabIndex = 7;
            this.olaTitle.Text = "Update Data";
            this.olaTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // otmTimer
            // 
            this.otmTimer.Tick += new System.EventHandler(this.otmTimer_Tick);
            // 
            // otmClose
            // 
            this.otmClose.Interval = 3000;
            this.otmClose.Tick += new System.EventHandler(this.otmClose_Tick);
            // 
            // wProgressUldCloseShif
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 366);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "wProgressUldCloseShif";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wProgressUldCloseShif";
            this.Shown += new System.EventHandler(this.wProgressUldCloseShif_Shown);
            this.opnPage.ResumeLayout(false);
            this.opnContent.ResumeLayout(false);
            this.opnContent.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.opnShf.ResumeLayout(false);
            this.opnShf.PerformLayout();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnContent;
        public System.Windows.Forms.Label olaStaShf;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitle;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Button ocmReSync;
        private System.Windows.Forms.Panel opnShf;
        private System.Windows.Forms.Label olaShfTbl;
        private System.Windows.Forms.Label olaErrNotConn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ProgressBar opgStaVod;
        private System.Windows.Forms.Label olaVodTbl;
        public System.Windows.Forms.Label olaStaVod;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar opgStaSal;
        private System.Windows.Forms.Label olaSalTbl;
        public System.Windows.Forms.Label olaStaSal;
        private System.Windows.Forms.ProgressBar opgStaShf;
        private System.Windows.Forms.Timer otmTimer;
        private System.Windows.Forms.Timer otmClose;
    }
}