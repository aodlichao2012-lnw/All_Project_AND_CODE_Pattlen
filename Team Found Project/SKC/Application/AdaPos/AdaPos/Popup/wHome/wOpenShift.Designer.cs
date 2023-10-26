namespace AdaPos.Popup.wHome
{
    partial class wOpenShift
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
            this.opnBG = new System.Windows.Forms.Panel();
            this.onpNumpad = new AdaPos.Control.uNumpad();
            this.otbSaleDate = new System.Windows.Forms.MaskedTextBox();
            this.otbPos = new System.Windows.Forms.TextBox();
            this.olaTitlePos = new System.Windows.Forms.Label();
            this.ocmChgSaleDate = new System.Windows.Forms.Button();
            this.olaTitleSaleDate = new System.Windows.Forms.Label();
            this.opnHD = new System.Windows.Forms.Panel();
            this.ocmShwKb = new System.Windows.Forms.Button();
            this.olaTitleOpenShift = new System.Windows.Forms.Label();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.ocmBack = new System.Windows.Forms.Button();
            this.opnBG.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnBG
            // 
            this.opnBG.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnBG.BackColor = System.Drawing.Color.White;
            this.opnBG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnBG.Controls.Add(this.onpNumpad);
            this.opnBG.Controls.Add(this.otbSaleDate);
            this.opnBG.Controls.Add(this.otbPos);
            this.opnBG.Controls.Add(this.olaTitlePos);
            this.opnBG.Controls.Add(this.ocmChgSaleDate);
            this.opnBG.Controls.Add(this.olaTitleSaleDate);
            this.opnBG.Controls.Add(this.opnHD);
            this.opnBG.Location = new System.Drawing.Point(212, 227);
            this.opnBG.Name = "opnBG";
            this.opnBG.Padding = new System.Windows.Forms.Padding(1);
            this.opnBG.Size = new System.Drawing.Size(600, 315);
            this.opnBG.TabIndex = 0;
            // 
            // onpNumpad
            // 
            this.onpNumpad.Enabled = false;
            this.onpNumpad.Location = new System.Drawing.Point(327, 63);
            this.onpNumpad.Name = "onpNumpad";
            this.onpNumpad.Size = new System.Drawing.Size(260, 240);
            this.onpNumpad.TabIndex = 7;
            this.onpNumpad.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbSaleDate_KeyDown);
            // 
            // otbSaleDate
            // 
            this.otbSaleDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbSaleDate.Enabled = false;
            this.otbSaleDate.Font = new System.Drawing.Font("Segoe UI Light", 14F);
            this.otbSaleDate.Location = new System.Drawing.Point(15, 86);
            this.otbSaleDate.Mask = "00/00/0000";
            this.otbSaleDate.Name = "otbSaleDate";
            this.otbSaleDate.Size = new System.Drawing.Size(252, 32);
            this.otbSaleDate.TabIndex = 1;
            this.otbSaleDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.otbSaleDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbSaleDate_KeyDown);
            // 
            // otbPos
            // 
            this.otbPos.Enabled = false;
            this.otbPos.Font = new System.Drawing.Font("Segoe UI Light", 14F);
            this.otbPos.Location = new System.Drawing.Point(14, 158);
            this.otbPos.MaxLength = 10;
            this.otbPos.Name = "otbPos";
            this.otbPos.ReadOnly = true;
            this.otbPos.Size = new System.Drawing.Size(300, 32);
            this.otbPos.TabIndex = 6;
            // 
            // olaTitlePos
            // 
            this.olaTitlePos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitlePos.AutoSize = true;
            this.olaTitlePos.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaTitlePos.Location = new System.Drawing.Point(11, 137);
            this.olaTitlePos.Name = "olaTitlePos";
            this.olaTitlePos.Size = new System.Drawing.Size(36, 19);
            this.olaTitlePos.TabIndex = 5;
            this.olaTitlePos.Text = "POS";
            this.olaTitlePos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ocmChgSaleDate
            // 
            this.ocmChgSaleDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmChgSaleDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(171)))), ((int)(((byte)(132)))));
            this.ocmChgSaleDate.FlatAppearance.BorderSize = 0;
            this.ocmChgSaleDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmChgSaleDate.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ocmChgSaleDate.ForeColor = System.Drawing.Color.White;
            this.ocmChgSaleDate.Image = global::AdaPos.Properties.Resources.Edit_32;
            this.ocmChgSaleDate.Location = new System.Drawing.Point(273, 86);
            this.ocmChgSaleDate.Name = "ocmChgSaleDate";
            this.ocmChgSaleDate.Size = new System.Drawing.Size(38, 32);
            this.ocmChgSaleDate.TabIndex = 4;
            this.ocmChgSaleDate.Tag = "";
            this.ocmChgSaleDate.UseVisualStyleBackColor = false;
            this.ocmChgSaleDate.Click += new System.EventHandler(this.ocmChgSaleDate_Click);
            this.ocmChgSaleDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbSaleDate_KeyDown);
            // 
            // olaTitleSaleDate
            // 
            this.olaTitleSaleDate.AutoSize = true;
            this.olaTitleSaleDate.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaTitleSaleDate.Location = new System.Drawing.Point(11, 64);
            this.olaTitleSaleDate.Name = "olaTitleSaleDate";
            this.olaTitleSaleDate.Size = new System.Drawing.Size(66, 19);
            this.olaTitleSaleDate.TabIndex = 2;
            this.olaTitleSaleDate.Text = "Sale date";
            this.olaTitleSaleDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.ocmShwKb);
            this.opnHD.Controls.Add(this.olaTitleOpenShift);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(596, 50);
            this.opnHD.TabIndex = 1;
            // 
            // ocmShwKb
            // 
            this.ocmShwKb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmShwKb.FlatAppearance.BorderSize = 0;
            this.ocmShwKb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmShwKb.Image = global::AdaPos.Properties.Resources.Fn;
            this.ocmShwKb.Location = new System.Drawing.Point(490, 0);
            this.ocmShwKb.Name = "ocmShwKb";
            this.ocmShwKb.Size = new System.Drawing.Size(50, 50);
            this.ocmShwKb.TabIndex = 6;
            this.ocmShwKb.UseVisualStyleBackColor = true;
            this.ocmShwKb.Click += new System.EventHandler(this.ocmShwKb_Click);
            this.ocmShwKb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbSaleDate_KeyDown);
            // 
            // olaTitleOpenShift
            // 
            this.olaTitleOpenShift.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.olaTitleOpenShift.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaTitleOpenShift.ForeColor = System.Drawing.Color.White;
            this.olaTitleOpenShift.Location = new System.Drawing.Point(56, 0);
            this.olaTitleOpenShift.Name = "olaTitleOpenShift";
            this.olaTitleOpenShift.Size = new System.Drawing.Size(228, 50);
            this.olaTitleOpenShift.TabIndex = 2;
            this.olaTitleOpenShift.Text = "Open shift";
            this.olaTitleOpenShift.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.Location = new System.Drawing.Point(546, 0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 1;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            this.ocmAccept.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbSaleDate_KeyDown);
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
            this.ocmBack.TabIndex = 0;
            this.ocmBack.UseVisualStyleBackColor = true;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            this.ocmBack.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbSaleDate_KeyDown);
            // 
            // wOpenShift
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnBG);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wOpenShift";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wOpenShift_FormClosing);
            this.Shown += new System.EventHandler(this.wOpenShift_Shown);
            this.opnBG.ResumeLayout(false);
            this.opnBG.PerformLayout();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnBG;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitleOpenShift;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Label olaTitleSaleDate;
        private System.Windows.Forms.Button ocmChgSaleDate;
        private System.Windows.Forms.Label olaTitlePos;
        private System.Windows.Forms.TextBox otbPos;
        private System.Windows.Forms.MaskedTextBox otbSaleDate;
        private Control.uNumpad onpNumpad;
        private System.Windows.Forms.Button ocmShwKb;
    }
}