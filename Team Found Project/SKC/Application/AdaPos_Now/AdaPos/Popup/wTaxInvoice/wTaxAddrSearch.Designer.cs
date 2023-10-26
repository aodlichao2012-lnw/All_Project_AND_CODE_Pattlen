namespace AdaPos.Popup.wTaxInvoice
{
    partial class wTaxAddrSearch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.opnPage = new System.Windows.Forms.Panel();
            this.ocmSearch = new System.Windows.Forms.Button();
            this.otbSearch = new System.Windows.Forms.TextBox();
            this.olaTitleSearchTxt = new System.Windows.Forms.Label();
            this.ogdSearch = new System.Windows.Forms.DataGridView();
            this.opnHD = new System.Windows.Forms.Panel();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.olaTitleSearch = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.otbTitleCardID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleTaxName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleTel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleFax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleAddr1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.opnPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdSearch)).BeginInit();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.ocmSearch);
            this.opnPage.Controls.Add(this.otbSearch);
            this.opnPage.Controls.Add(this.olaTitleSearchTxt);
            this.opnPage.Controls.Add(this.ogdSearch);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnPage.Location = new System.Drawing.Point(0, 0);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(900, 450);
            this.opnPage.TabIndex = 0;
            // 
            // ocmSearch
            // 
            this.ocmSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(171)))), ((int)(((byte)(132)))));
            this.ocmSearch.FlatAppearance.BorderSize = 0;
            this.ocmSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ocmSearch.ForeColor = System.Drawing.Color.White;
            this.ocmSearch.Image = global::AdaPos.Properties.Resources.SearchW_32;
            this.ocmSearch.Location = new System.Drawing.Point(836, 81);
            this.ocmSearch.Name = "ocmSearch";
            this.ocmSearch.Size = new System.Drawing.Size(49, 38);
            this.ocmSearch.TabIndex = 1;
            this.ocmSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ocmSearch.UseVisualStyleBackColor = false;
            this.ocmSearch.Click += new System.EventHandler(this.ocmSearch_Click);
            // 
            // otbSearch
            // 
            this.otbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbSearch.Font = new System.Drawing.Font("Segoe UI Light", 16F);
            this.otbSearch.Location = new System.Drawing.Point(11, 81);
            this.otbSearch.MaxLength = 50;
            this.otbSearch.Name = "otbSearch";
            this.otbSearch.Size = new System.Drawing.Size(823, 36);
            this.otbSearch.TabIndex = 0;
            this.otbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbSearch_KeyDown);
            // 
            // olaTitleSearchTxt
            // 
            this.olaTitleSearchTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleSearchTxt.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaTitleSearchTxt.Location = new System.Drawing.Point(11, 56);
            this.olaTitleSearchTxt.Name = "olaTitleSearchTxt";
            this.olaTitleSearchTxt.Size = new System.Drawing.Size(325, 22);
            this.olaTitleSearchTxt.TabIndex = 11;
            this.olaTitleSearchTxt.Text = "Search Text";
            this.olaTitleSearchTxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.otbTitleCardID,
            this.otbTitleTaxName,
            this.otbTitleTel,
            this.otbTitleFax,
            this.otbTitleAddr1});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdSearch.DefaultCellStyle = dataGridViewCellStyle5;
            this.ogdSearch.EnableHeadersVisualStyles = false;
            this.ogdSearch.GridColor = System.Drawing.Color.Gainsboro;
            this.ogdSearch.Location = new System.Drawing.Point(11, 127);
            this.ogdSearch.MultiSelect = false;
            this.ogdSearch.Name = "ogdSearch";
            this.ogdSearch.RowHeadersVisible = false;
            this.ogdSearch.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.ogdSearch.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.ogdSearch.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.ogdSearch.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.ogdSearch.RowTemplate.Height = 40;
            this.ogdSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ogdSearch.ShowCellErrors = false;
            this.ogdSearch.ShowCellToolTips = false;
            this.ogdSearch.ShowEditingIcon = false;
            this.ogdSearch.ShowRowErrors = false;
            this.ogdSearch.Size = new System.Drawing.Size(874, 308);
            this.ogdSearch.TabIndex = 2;
            this.ogdSearch.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ogdSearch_CellContentDoubleClick);
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Controls.Add(this.olaTitleSearch);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(896, 50);
            this.opnHD.TabIndex = 2;
            // 
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.Location = new System.Drawing.Point(843, 0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 2;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // olaTitleSearch
            // 
            this.olaTitleSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleSearch.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleSearch.ForeColor = System.Drawing.Color.White;
            this.olaTitleSearch.Location = new System.Drawing.Point(56, 0);
            this.olaTitleSearch.Name = "olaTitleSearch";
            this.olaTitleSearch.Size = new System.Drawing.Size(710, 50);
            this.olaTitleSearch.TabIndex = 1;
            this.olaTitleSearch.Text = "Search Address";
            this.olaTitleSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // 
            // otbTitleCardID
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.otbTitleCardID.DefaultCellStyle = dataGridViewCellStyle2;
            this.otbTitleCardID.HeaderText = "Card ID.";
            this.otbTitleCardID.MinimumWidth = 100;
            this.otbTitleCardID.Name = "otbTitleCardID";
            this.otbTitleCardID.ReadOnly = true;
            this.otbTitleCardID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitleTaxName
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.otbTitleTaxName.DefaultCellStyle = dataGridViewCellStyle3;
            this.otbTitleTaxName.FillWeight = 150F;
            this.otbTitleTaxName.HeaderText = "Tax Name";
            this.otbTitleTaxName.MinimumWidth = 100;
            this.otbTitleTaxName.Name = "otbTitleTaxName";
            this.otbTitleTaxName.ReadOnly = true;
            this.otbTitleTaxName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitleTel
            // 
            this.otbTitleTel.HeaderText = "Tel";
            this.otbTitleTel.Name = "otbTitleTel";
            this.otbTitleTel.ReadOnly = true;
            this.otbTitleTel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitleFax
            // 
            this.otbTitleFax.HeaderText = "Fax";
            this.otbTitleFax.Name = "otbTitleFax";
            this.otbTitleFax.ReadOnly = true;
            this.otbTitleFax.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitleAddr1
            // 
            this.otbTitleAddr1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.otbTitleAddr1.DefaultCellStyle = dataGridViewCellStyle4;
            this.otbTitleAddr1.FillWeight = 200F;
            this.otbTitleAddr1.HeaderText = "Address1";
            this.otbTitleAddr1.MinimumWidth = 200;
            this.otbTitleAddr1.Name = "otbTitleAddr1";
            this.otbTitleAddr1.ReadOnly = true;
            this.otbTitleAddr1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // wTaxAddrSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(900, 450);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "wTaxAddrSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wTaxAddrSearch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wTaxAddrSearch_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.wTaxAddrSearch_KeyDown);
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
        private System.Windows.Forms.Label olaTitleSearch;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmSearch;
        private System.Windows.Forms.TextBox otbSearch;
        private System.Windows.Forms.Label olaTitleSearchTxt;
        private System.Windows.Forms.DataGridView ogdSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleCardID;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleTaxName;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleTel;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleFax;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleAddr1;
    }
}