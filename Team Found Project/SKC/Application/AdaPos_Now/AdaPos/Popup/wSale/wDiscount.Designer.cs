namespace AdaPos.Popup.wSale
{
    partial class wDiscount
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
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.otbB4Dis = new System.Windows.Forms.TextBox();
            this.olaB4Dis = new System.Windows.Forms.Label();
            this.otbDisAmount = new System.Windows.Forms.TextBox();
            this.olaDisAmount = new System.Windows.Forms.Label();
            this.otbAfDis = new System.Windows.Forms.TextBox();
            this.olaAfDis = new System.Windows.Forms.Label();
            this.oucNumpad = new AdaPos.Control.uNumpad();
            this.opnPage = new System.Windows.Forms.Panel();
            this.ocmReason = new System.Windows.Forms.Button();
            this.olaReason = new System.Windows.Forms.Label();
            this.otbReason = new System.Windows.Forms.TextBox();
            this.opnHD.SuspendLayout();
            this.opnPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnHD
            // 
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitle);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Dock = System.Windows.Forms.DockStyle.Top;
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(594, 50);
            this.opnHD.TabIndex = 2;
            // 
            // olaTitle
            // 
            this.olaTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitle.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitle.ForeColor = System.Drawing.Color.White;
            this.olaTitle.Location = new System.Drawing.Point(56, 0);
            this.olaTitle.Name = "olaTitle";
            this.olaTitle.Size = new System.Drawing.Size(378, 50);
            this.olaTitle.TabIndex = 7;
            this.olaTitle.Text = "Discount Amount";
            this.olaTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ocmBack
            // 
            this.ocmBack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmBack.FlatAppearance.BorderSize = 0;
            this.ocmBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmBack.Image = global::AdaPos.Properties.Resources.BackW_32;
            this.ocmBack.Location = new System.Drawing.Point(0, 0);
            this.ocmBack.Name = "ocmBack";
            this.ocmBack.Size = new System.Drawing.Size(50, 50);
            this.ocmBack.TabIndex = 4;
            this.ocmBack.UseVisualStyleBackColor = true;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.Location = new System.Drawing.Point(543, 0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 5;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // otbB4Dis
            // 
            this.otbB4Dis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbB4Dis.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.otbB4Dis.Enabled = false;
            this.otbB4Dis.Font = new System.Drawing.Font("Segoe UI Light", 16F);
            this.otbB4Dis.Location = new System.Drawing.Point(23, 93);
            this.otbB4Dis.Name = "otbB4Dis";
            this.otbB4Dis.Size = new System.Drawing.Size(276, 36);
            this.otbB4Dis.TabIndex = 29;
            this.otbB4Dis.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // olaB4Dis
            // 
            this.olaB4Dis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaB4Dis.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaB4Dis.Location = new System.Drawing.Point(20, 68);
            this.olaB4Dis.Name = "olaB4Dis";
            this.olaB4Dis.Size = new System.Drawing.Size(278, 22);
            this.olaB4Dis.TabIndex = 28;
            this.olaB4Dis.Text = "Old Price";
            this.olaB4Dis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // otbDisAmount
            // 
            this.otbDisAmount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbDisAmount.Font = new System.Drawing.Font("Segoe UI Light", 16F);
            this.otbDisAmount.Location = new System.Drawing.Point(23, 170);
            this.otbDisAmount.Name = "otbDisAmount";
            this.otbDisAmount.Size = new System.Drawing.Size(276, 36);
            this.otbDisAmount.TabIndex = 31;
            this.otbDisAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.otbDisAmount.TextChanged += new System.EventHandler(this.otbDisAmount_TextChanged);
            // 
            // olaDisAmount
            // 
            this.olaDisAmount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaDisAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaDisAmount.Location = new System.Drawing.Point(20, 145);
            this.olaDisAmount.Name = "olaDisAmount";
            this.olaDisAmount.Size = new System.Drawing.Size(278, 22);
            this.olaDisAmount.TabIndex = 30;
            this.olaDisAmount.Text = "Amount";
            this.olaDisAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // otbAfDis
            // 
            this.otbAfDis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbAfDis.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.otbAfDis.Enabled = false;
            this.otbAfDis.Font = new System.Drawing.Font("Segoe UI Light", 16F);
            this.otbAfDis.Location = new System.Drawing.Point(23, 248);
            this.otbAfDis.Name = "otbAfDis";
            this.otbAfDis.Size = new System.Drawing.Size(276, 36);
            this.otbAfDis.TabIndex = 33;
            this.otbAfDis.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // olaAfDis
            // 
            this.olaAfDis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaAfDis.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaAfDis.Location = new System.Drawing.Point(20, 223);
            this.olaAfDis.Name = "olaAfDis";
            this.olaAfDis.Size = new System.Drawing.Size(278, 22);
            this.olaAfDis.TabIndex = 32;
            this.olaAfDis.Text = "New Price";
            this.olaAfDis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // oucNumpad
            // 
            this.oucNumpad.Location = new System.Drawing.Point(320, 67);
            this.oucNumpad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.oucNumpad.Name = "oucNumpad";
            this.oucNumpad.Size = new System.Drawing.Size(256, 226);
            this.oucNumpad.TabIndex = 34;
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.ocmReason);
            this.opnPage.Controls.Add(this.olaReason);
            this.opnPage.Controls.Add(this.otbReason);
            this.opnPage.Controls.Add(this.oucNumpad);
            this.opnPage.Controls.Add(this.otbAfDis);
            this.opnPage.Controls.Add(this.olaAfDis);
            this.opnPage.Controls.Add(this.otbDisAmount);
            this.opnPage.Controls.Add(this.olaDisAmount);
            this.opnPage.Controls.Add(this.otbB4Dis);
            this.opnPage.Controls.Add(this.olaB4Dis);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(213, 186);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(598, 396);
            this.opnPage.TabIndex = 35;
            // 
            // ocmReason
            // 
            this.ocmReason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmReason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmReason.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmReason.FlatAppearance.BorderSize = 0;
            this.ocmReason.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmReason.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmReason.ForeColor = System.Drawing.Color.White;
            this.ocmReason.Image = global::AdaPos.Properties.Resources.SearchW_32;
            this.ocmReason.Location = new System.Drawing.Point(254, 329);
            this.ocmReason.Margin = new System.Windows.Forms.Padding(0);
            this.ocmReason.Name = "ocmReason";
            this.ocmReason.Size = new System.Drawing.Size(45, 36);
            this.ocmReason.TabIndex = 9;
            this.ocmReason.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmReason.UseVisualStyleBackColor = false;
            this.ocmReason.Click += new System.EventHandler(this.ocmReason_Click);
            // 
            // olaReason
            // 
            this.olaReason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaReason.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaReason.Location = new System.Drawing.Point(20, 305);
            this.olaReason.Name = "olaReason";
            this.olaReason.Size = new System.Drawing.Size(278, 22);
            this.olaReason.TabIndex = 36;
            this.olaReason.Text = "Reason";
            this.olaReason.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // otbReason
            // 
            this.otbReason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbReason.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.otbReason.Enabled = false;
            this.otbReason.Font = new System.Drawing.Font("Segoe UI Light", 16F);
            this.otbReason.Location = new System.Drawing.Point(23, 329);
            this.otbReason.Name = "otbReason";
            this.otbReason.Size = new System.Drawing.Size(232, 36);
            this.otbReason.TabIndex = 35;
            // 
            // wDiscount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "wDiscount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wDiscount";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wDiscount_FormClosing);
            this.Shown += new System.EventHandler(this.wDiscount_Shown);
            this.opnHD.ResumeLayout(false);
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitle;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.TextBox otbB4Dis;
        private System.Windows.Forms.Label olaB4Dis;
        private System.Windows.Forms.TextBox otbDisAmount;
        private System.Windows.Forms.Label olaDisAmount;
        private System.Windows.Forms.TextBox otbAfDis;
        private System.Windows.Forms.Label olaAfDis;
        private Control.uNumpad oucNumpad;
        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Label olaReason;
        private System.Windows.Forms.TextBox otbReason;
        private System.Windows.Forms.Button ocmReason;
    }
}