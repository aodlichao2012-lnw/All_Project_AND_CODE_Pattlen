namespace AdaPos.Popup.wLogin
{
    partial class wLanguage
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.opnEmail = new System.Windows.Forms.Panel();
            this.ogdLanguage = new System.Windows.Forms.DataGridView();
            this.otbLngID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbLanShort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbLangName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.opnHD = new System.Windows.Forms.Panel();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmOK = new System.Windows.Forms.Button();
            this.olaTitleLanguage = new System.Windows.Forms.Label();
            this.opnEmail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdLanguage)).BeginInit();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnEmail
            // 
            this.opnEmail.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnEmail.BackColor = System.Drawing.Color.White;
            this.opnEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnEmail.Controls.Add(this.ogdLanguage);
            this.opnEmail.Controls.Add(this.opnHD);
            this.opnEmail.Location = new System.Drawing.Point(312, 158);
            this.opnEmail.Name = "opnEmail";
            this.opnEmail.Padding = new System.Windows.Forms.Padding(1);
            this.opnEmail.Size = new System.Drawing.Size(400, 450);
            this.opnEmail.TabIndex = 1;
            // 
            // ogdLanguage
            // 
            this.ogdLanguage.AllowUserToAddRows = false;
            this.ogdLanguage.AllowUserToDeleteRows = false;
            this.ogdLanguage.AllowUserToResizeColumns = false;
            this.ogdLanguage.AllowUserToResizeRows = false;
            this.ogdLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ogdLanguage.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ogdLanguage.BackgroundColor = System.Drawing.Color.White;
            this.ogdLanguage.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.ogdLanguage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ogdLanguage.ColumnHeadersVisible = false;
            this.ogdLanguage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.otbLngID,
            this.otbLanShort,
            this.otbLangName});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdLanguage.DefaultCellStyle = dataGridViewCellStyle3;
            this.ogdLanguage.EnableHeadersVisualStyles = false;
            this.ogdLanguage.GridColor = System.Drawing.Color.Gainsboro;
            this.ogdLanguage.Location = new System.Drawing.Point(11, 61);
            this.ogdLanguage.MultiSelect = false;
            this.ogdLanguage.Name = "ogdLanguage";
            this.ogdLanguage.ReadOnly = true;
            this.ogdLanguage.RowHeadersVisible = false;
            this.ogdLanguage.RowTemplate.Height = 40;
            this.ogdLanguage.RowTemplate.ReadOnly = true;
            this.ogdLanguage.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ogdLanguage.ShowCellErrors = false;
            this.ogdLanguage.ShowCellToolTips = false;
            this.ogdLanguage.ShowEditingIcon = false;
            this.ogdLanguage.ShowRowErrors = false;
            this.ogdLanguage.Size = new System.Drawing.Size(376, 376);
            this.ogdLanguage.TabIndex = 0;
            this.ogdLanguage.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ogdLanguage_CellDoubleClick);
            this.ogdLanguage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ogdLanguage_KeyDown);
            // 
            // otbLngID
            // 
            this.otbLngID.HeaderText = "ID";
            this.otbLngID.Name = "otbLngID";
            this.otbLngID.ReadOnly = true;
            this.otbLngID.Visible = false;
            // 
            // otbLanShort
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.otbLanShort.DefaultCellStyle = dataGridViewCellStyle1;
            this.otbLanShort.FillWeight = 50F;
            this.otbLanShort.HeaderText = "Langauge short name";
            this.otbLanShort.Name = "otbLanShort";
            this.otbLanShort.ReadOnly = true;
            // 
            // otbLangName
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.otbLangName.DefaultCellStyle = dataGridViewCellStyle2;
            this.otbLangName.HeaderText = "Langauge Name";
            this.otbLangName.Name = "otbLangName";
            this.otbLangName.ReadOnly = true;
            // 
            // opnHD
            // 
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.ocmOK);
            this.opnHD.Controls.Add(this.olaTitleLanguage);
            this.opnHD.Dock = System.Windows.Forms.DockStyle.Top;
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(396, 50);
            this.opnHD.TabIndex = 11;
            // 
            // ocmBack
            // 
            this.ocmBack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmBack.FlatAppearance.BorderSize = 0;
            this.ocmBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmBack.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmBack.ForeColor = System.Drawing.Color.White;
            this.ocmBack.Image = global::AdaPos.Properties.Resources.BackW_32;
            this.ocmBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmBack.Location = new System.Drawing.Point(0, 0);
            this.ocmBack.Margin = new System.Windows.Forms.Padding(0);
            this.ocmBack.Name = "ocmBack";
            this.ocmBack.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ocmBack.Size = new System.Drawing.Size(50, 50);
            this.ocmBack.TabIndex = 3;
            this.ocmBack.Tag = "2";
            this.ocmBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmBack.UseVisualStyleBackColor = false;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // ocmOK
            // 
            this.ocmOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmOK.FlatAppearance.BorderSize = 0;
            this.ocmOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmOK.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmOK.ForeColor = System.Drawing.Color.White;
            this.ocmOK.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmOK.Location = new System.Drawing.Point(346, 0);
            this.ocmOK.Margin = new System.Windows.Forms.Padding(0);
            this.ocmOK.MaximumSize = new System.Drawing.Size(250, 50);
            this.ocmOK.Name = "ocmOK";
            this.ocmOK.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ocmOK.Size = new System.Drawing.Size(50, 50);
            this.ocmOK.TabIndex = 2;
            this.ocmOK.Tag = "1";
            this.ocmOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmOK.UseVisualStyleBackColor = false;
            this.ocmOK.Click += new System.EventHandler(this.ocmOK_Click);
            // 
            // olaTitleLanguage
            // 
            this.olaTitleLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleLanguage.BackColor = System.Drawing.Color.Transparent;
            this.olaTitleLanguage.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleLanguage.ForeColor = System.Drawing.Color.White;
            this.olaTitleLanguage.Location = new System.Drawing.Point(50, 0);
            this.olaTitleLanguage.Name = "olaTitleLanguage";
            this.olaTitleLanguage.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.olaTitleLanguage.Size = new System.Drawing.Size(293, 50);
            this.olaTitleLanguage.TabIndex = 3;
            this.olaTitleLanguage.Text = "Language";
            this.olaTitleLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // wLanguage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnEmail);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "wLanguage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wLanguage";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wLanguage_FormClosing);
            this.Shown += new System.EventHandler(this.wLanguage_Shown);
            this.opnEmail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ogdLanguage)).EndInit();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnEmail;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmOK;
        private System.Windows.Forms.Label olaTitleLanguage;
        private System.Windows.Forms.DataGridView ogdLanguage;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbLngID;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbLanShort;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbLangName;
    }
}