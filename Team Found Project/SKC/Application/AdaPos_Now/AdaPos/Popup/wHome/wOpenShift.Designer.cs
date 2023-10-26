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
            this.odtSaleDate = new System.Windows.Forms.DateTimePicker();
            this.otbPos = new System.Windows.Forms.TextBox();
            this.olaTitlePos = new System.Windows.Forms.Label();
            this.ocmChgSaleDate = new System.Windows.Forms.Button();
            this.olaTitleSaleDate = new System.Windows.Forms.Label();
            this.opnHD = new System.Windows.Forms.Panel();
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
            this.opnBG.Controls.Add(this.odtSaleDate);
            this.opnBG.Controls.Add(this.otbPos);
            this.opnBG.Controls.Add(this.olaTitlePos);
            this.opnBG.Controls.Add(this.ocmChgSaleDate);
            this.opnBG.Controls.Add(this.olaTitleSaleDate);
            this.opnBG.Controls.Add(this.opnHD);
            this.opnBG.Location = new System.Drawing.Point(343, 267);
            this.opnBG.Name = "opnBG";
            this.opnBG.Padding = new System.Windows.Forms.Padding(1);
            this.opnBG.Size = new System.Drawing.Size(338, 233);
            this.opnBG.TabIndex = 0;
            // 
            // odtSaleDate
            // 
            this.odtSaleDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.odtSaleDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.odtSaleDate.CustomFormat = "dd/MM/yyyy";
            this.odtSaleDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.odtSaleDate.Enabled = false;
            this.odtSaleDate.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.odtSaleDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.odtSaleDate.Location = new System.Drawing.Point(15, 87);
            this.odtSaleDate.Margin = new System.Windows.Forms.Padding(0);
            this.odtSaleDate.Name = "odtSaleDate";
            this.odtSaleDate.Size = new System.Drawing.Size(258, 30);
            this.odtSaleDate.TabIndex = 8;
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
            this.ocmChgSaleDate.Location = new System.Drawing.Point(276, 86);
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
            this.opnHD.Controls.Add(this.olaTitleOpenShift);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(334, 50);
            this.opnHD.TabIndex = 1;
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
            this.ocmAccept.Location = new System.Drawing.Point(284, 0);
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
        private System.Windows.Forms.DateTimePicker odtSaleDate;
    }
}