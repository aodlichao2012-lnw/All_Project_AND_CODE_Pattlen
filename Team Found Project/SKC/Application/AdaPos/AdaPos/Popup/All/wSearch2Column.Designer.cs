namespace AdaPos.Popup.All
{
    partial class wSearch2Column
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.opnPage = new System.Windows.Forms.Panel();
            this.ocbSearchBy = new System.Windows.Forms.ComboBox();
            this.ocmSearch = new System.Windows.Forms.Button();
            this.otbSearchPdt = new System.Windows.Forms.TextBox();
            this.ogdSearch = new System.Windows.Forms.DataGridView();
            this.otbCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.opnHD = new System.Windows.Forms.Panel();
            this.ocmShwKb = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.ocmBack = new System.Windows.Forms.Button();
            this.olaTitle = new System.Windows.Forms.Label();
            this.ocbSchMatch = new System.Windows.Forms.ComboBox();
            this.opnPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdSearch)).BeginInit();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.ocbSearchBy);
            this.opnPage.Controls.Add(this.ocmSearch);
            this.opnPage.Controls.Add(this.otbSearchPdt);
            this.opnPage.Controls.Add(this.ogdSearch);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Controls.Add(this.ocbSchMatch);
            this.opnPage.Location = new System.Drawing.Point(187, 134);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(650, 500);
            this.opnPage.TabIndex = 1;
            // 
            // ocbSearchBy
            // 
            this.ocbSearchBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ocbSearchBy.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocbSearchBy.FormattingEnabled = true;
            this.ocbSearchBy.Location = new System.Drawing.Point(10, 60);
            this.ocbSearchBy.Name = "ocbSearchBy";
            this.ocbSearchBy.Size = new System.Drawing.Size(110, 29);
            this.ocbSearchBy.TabIndex = 35;
            // 
            // ocmSearch
            // 
            this.ocmSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(171)))), ((int)(((byte)(132)))));
            this.ocmSearch.FlatAppearance.BorderSize = 0;
            this.ocmSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ocmSearch.ForeColor = System.Drawing.Color.White;
            this.ocmSearch.Image = global::AdaPos.Properties.Resources.SearchW_32;
            this.ocmSearch.Location = new System.Drawing.Point(577, 61);
            this.ocmSearch.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.ocmSearch.Name = "ocmSearch";
            this.ocmSearch.Size = new System.Drawing.Size(60, 29);
            this.ocmSearch.TabIndex = 33;
            this.ocmSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ocmSearch.UseVisualStyleBackColor = false;
            this.ocmSearch.Click += new System.EventHandler(this.ocmSearch_Click);
            // 
            // otbSearchPdt
            // 
            this.otbSearchPdt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbSearchPdt.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.otbSearchPdt.Location = new System.Drawing.Point(120, 61);
            this.otbSearchPdt.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            this.otbSearchPdt.Name = "otbSearchPdt";
            this.otbSearchPdt.Size = new System.Drawing.Size(455, 29);
            this.otbSearchPdt.TabIndex = 34;
            this.otbSearchPdt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbSearchPdt_KeyDown);
            // 
            // ogdSearch
            // 
            this.ogdSearch.AllowUserToAddRows = false;
            this.ogdSearch.AllowUserToDeleteRows = false;
            this.ogdSearch.AllowUserToResizeColumns = false;
            this.ogdSearch.AllowUserToResizeRows = false;
            this.ogdSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ogdSearch.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ogdSearch.BackgroundColor = System.Drawing.Color.White;
            this.ogdSearch.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdSearch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ogdSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ogdSearch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.otbCode,
            this.otbName});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdSearch.DefaultCellStyle = dataGridViewCellStyle4;
            this.ogdSearch.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.ogdSearch.EnableHeadersVisualStyles = false;
            this.ogdSearch.GridColor = System.Drawing.Color.Gainsboro;
            this.ogdSearch.Location = new System.Drawing.Point(11, 100);
            this.ogdSearch.Margin = new System.Windows.Forms.Padding(0);
            this.ogdSearch.MultiSelect = false;
            this.ogdSearch.Name = "ogdSearch";
            this.ogdSearch.ReadOnly = true;
            this.ogdSearch.RowHeadersVisible = false;
            this.ogdSearch.RowTemplate.Height = 40;
            this.ogdSearch.RowTemplate.ReadOnly = true;
            this.ogdSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ogdSearch.ShowCellErrors = false;
            this.ogdSearch.ShowCellToolTips = false;
            this.ogdSearch.ShowEditingIcon = false;
            this.ogdSearch.ShowRowErrors = false;
            this.ogdSearch.Size = new System.Drawing.Size(626, 387);
            this.ogdSearch.TabIndex = 25;
            this.ogdSearch.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ogdSearch_CellMouseDoubleClick);
            this.ogdSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ogdSearch_KeyDown);
            // 
            // otbCode
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.otbCode.DefaultCellStyle = dataGridViewCellStyle2;
            this.otbCode.FillWeight = 60F;
            this.otbCode.HeaderText = "Code";
            this.otbCode.Name = "otbCode";
            this.otbCode.ReadOnly = true;
            this.otbCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbName
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.otbName.DefaultCellStyle = dataGridViewCellStyle3;
            this.otbName.HeaderText = "Name";
            this.otbName.Name = "otbName";
            this.otbName.ReadOnly = true;
            this.otbName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.ocmShwKb);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.olaTitle);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(646, 50);
            this.opnHD.TabIndex = 1;
            // 
            // ocmShwKb
            // 
            this.ocmShwKb.Dock = System.Windows.Forms.DockStyle.Right;
            this.ocmShwKb.FlatAppearance.BorderSize = 0;
            this.ocmShwKb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmShwKb.Image = global::AdaPos.Properties.Resources.KBB_32;
            this.ocmShwKb.Location = new System.Drawing.Point(546, 0);
            this.ocmShwKb.Name = "ocmShwKb";
            this.ocmShwKb.Size = new System.Drawing.Size(50, 50);
            this.ocmShwKb.TabIndex = 9;
            this.ocmShwKb.UseVisualStyleBackColor = true;
            this.ocmShwKb.Click += new System.EventHandler(this.ocmShwKb_Click);
            // 
            // ocmAccept
            // 
            this.ocmAccept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmAccept.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmAccept.Dock = System.Windows.Forms.DockStyle.Right;
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmAccept.ForeColor = System.Drawing.Color.White;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmAccept.Location = new System.Drawing.Point(596, 0);
            this.ocmAccept.Margin = new System.Windows.Forms.Padding(0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 7;
            this.ocmAccept.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmAccept.UseVisualStyleBackColor = false;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
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
            this.ocmBack.TabIndex = 5;
            this.ocmBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmBack.UseVisualStyleBackColor = false;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // olaTitle
            // 
            this.olaTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitle.BackColor = System.Drawing.Color.Transparent;
            this.olaTitle.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitle.ForeColor = System.Drawing.Color.White;
            this.olaTitle.Location = new System.Drawing.Point(53, 0);
            this.olaTitle.Name = "olaTitle";
            this.olaTitle.Size = new System.Drawing.Size(487, 50);
            this.olaTitle.TabIndex = 6;
            this.olaTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ocbSchMatch
            // 
            this.ocbSchMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ocbSchMatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ocbSchMatch.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocbSchMatch.FormattingEnabled = true;
            this.ocbSchMatch.Location = new System.Drawing.Point(487, 61);
            this.ocbSchMatch.Name = "ocbSchMatch";
            this.ocbSchMatch.Size = new System.Drawing.Size(150, 29);
            this.ocbSchMatch.TabIndex = 36;
            this.ocbSchMatch.Visible = false;
            // 
            // wSearch2Column
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1024, 640);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "wSearch2Column";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.SystemColors.ActiveBorder;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.wSearch2Column_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.wSearch2Column_KeyDown);
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdSearch)).EndInit();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Label olaTitle;
        private System.Windows.Forms.DataGridView ogdSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbName;
        private System.Windows.Forms.ComboBox ocbSchMatch;
        private System.Windows.Forms.ComboBox ocbSearchBy;
        private System.Windows.Forms.Button ocmSearch;
        private System.Windows.Forms.TextBox otbSearchPdt;
        private System.Windows.Forms.Button ocmShwKb;
    }
}