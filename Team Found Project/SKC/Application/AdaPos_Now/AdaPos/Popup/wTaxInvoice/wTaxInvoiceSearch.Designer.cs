namespace AdaPos
{
    partial class wTaxInvoiceSearch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.opnPage = new System.Windows.Forms.Panel();
            this.ocmSearch = new System.Windows.Forms.Button();
            this.otbTicketNo = new System.Windows.Forms.TextBox();
            this.olaTitleSearch = new System.Windows.Forms.Label();
            this.ogdSelectABB = new System.Windows.Forms.DataGridView();
            this.opnHD = new System.Windows.Forms.Panel();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.olaTitleSearchDoc = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.otbTitleDocNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleDatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitlePos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.opnPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdSelectABB)).BeginInit();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.ocmSearch);
            this.opnPage.Controls.Add(this.otbTicketNo);
            this.opnPage.Controls.Add(this.olaTitleSearch);
            this.opnPage.Controls.Add(this.ogdSelectABB);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(162, 109);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(700, 550);
            this.opnPage.TabIndex = 2;
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
            this.ocmSearch.Location = new System.Drawing.Point(637, 86);
            this.ocmSearch.Name = "ocmSearch";
            this.ocmSearch.Size = new System.Drawing.Size(50, 32);
            this.ocmSearch.TabIndex = 6;
            this.ocmSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ocmSearch.UseVisualStyleBackColor = false;
            this.ocmSearch.Click += new System.EventHandler(this.ocmSearch_Click);
            // 
            // otbTicketNo
            // 
            this.otbTicketNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbTicketNo.Font = new System.Drawing.Font("Segoe UI Light", 16F);
            this.otbTicketNo.Location = new System.Drawing.Point(11, 86);
            this.otbTicketNo.MaxLength = 50;
            this.otbTicketNo.Name = "otbTicketNo";
            this.otbTicketNo.Size = new System.Drawing.Size(626, 36);
            this.otbTicketNo.TabIndex = 8;
            this.otbTicketNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbTicketNo_KeyDown);
            // 
            // olaTitleSearch
            // 
            this.olaTitleSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaTitleSearch.Location = new System.Drawing.Point(11, 61);
            this.olaTitleSearch.Name = "olaTitleSearch";
            this.olaTitleSearch.Size = new System.Drawing.Size(226, 22);
            this.olaTitleSearch.TabIndex = 7;
            this.olaTitleSearch.Text = "Search";
            this.olaTitleSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ogdSelectABB
            // 
            this.ogdSelectABB.AllowUserToAddRows = false;
            this.ogdSelectABB.AllowUserToDeleteRows = false;
            this.ogdSelectABB.AllowUserToResizeColumns = false;
            this.ogdSelectABB.AllowUserToResizeRows = false;
            this.ogdSelectABB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ogdSelectABB.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ogdSelectABB.BackgroundColor = System.Drawing.Color.White;
            this.ogdSelectABB.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdSelectABB.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ogdSelectABB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ogdSelectABB.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.otbTitleDocNo,
            this.otbTitleDatetime,
            this.otbTitlePos,
            this.otbTitleAmt});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdSelectABB.DefaultCellStyle = dataGridViewCellStyle3;
            this.ogdSelectABB.EnableHeadersVisualStyles = false;
            this.ogdSelectABB.GridColor = System.Drawing.Color.Gainsboro;
            this.ogdSelectABB.Location = new System.Drawing.Point(11, 132);
            this.ogdSelectABB.MultiSelect = false;
            this.ogdSelectABB.Name = "ogdSelectABB";
            this.ogdSelectABB.ReadOnly = true;
            this.ogdSelectABB.RowHeadersVisible = false;
            this.ogdSelectABB.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.ogdSelectABB.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.ogdSelectABB.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.ogdSelectABB.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.ogdSelectABB.RowTemplate.Height = 40;
            this.ogdSelectABB.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ogdSelectABB.ShowCellErrors = false;
            this.ogdSelectABB.ShowCellToolTips = false;
            this.ogdSelectABB.ShowEditingIcon = false;
            this.ogdSelectABB.ShowRowErrors = false;
            this.ogdSelectABB.Size = new System.Drawing.Size(676, 405);
            this.ogdSelectABB.TabIndex = 2;
            this.ogdSelectABB.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ogdSelectABB_CellClick);
            this.ogdSelectABB.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ogdSelectABB_CellDoubleClick);
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Controls.Add(this.olaTitleSearchDoc);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(696, 50);
            this.opnHD.TabIndex = 1;
            // 
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.Location = new System.Drawing.Point(650, 0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 2;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // olaTitleSearchDoc
            // 
            this.olaTitleSearchDoc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleSearchDoc.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleSearchDoc.ForeColor = System.Drawing.Color.White;
            this.olaTitleSearchDoc.Location = new System.Drawing.Point(56, 0);
            this.olaTitleSearchDoc.Name = "olaTitleSearchDoc";
            this.olaTitleSearchDoc.Size = new System.Drawing.Size(510, 50);
            this.olaTitleSearchDoc.TabIndex = 1;
            this.olaTitleSearchDoc.Text = "Search Document";
            this.olaTitleSearchDoc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // otbTitleDocNo
            // 
            this.otbTitleDocNo.HeaderText = "Document No.";
            this.otbTitleDocNo.Name = "otbTitleDocNo";
            this.otbTitleDocNo.ReadOnly = true;
            this.otbTitleDocNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitleDatetime
            // 
            this.otbTitleDatetime.FillWeight = 80F;
            this.otbTitleDatetime.HeaderText = "Date and time";
            this.otbTitleDatetime.Name = "otbTitleDatetime";
            this.otbTitleDatetime.ReadOnly = true;
            this.otbTitleDatetime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitlePos
            // 
            this.otbTitlePos.FillWeight = 50F;
            this.otbTitlePos.HeaderText = "POS";
            this.otbTitlePos.Name = "otbTitlePos";
            this.otbTitlePos.ReadOnly = true;
            this.otbTitlePos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitleAmt
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.otbTitleAmt.DefaultCellStyle = dataGridViewCellStyle2;
            this.otbTitleAmt.FillWeight = 60F;
            this.otbTitleAmt.HeaderText = "Amount";
            this.otbTitleAmt.Name = "otbTitleAmt";
            this.otbTitleAmt.ReadOnly = true;
            this.otbTitleAmt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // wTaxInvoiceSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "wTaxInvoiceSearch";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wTaxInvoiceSearch_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.wTaxInvoiceSearch_KeyDown);
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdSelectABB)).EndInit();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.DataGridView ogdSelectABB;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Label olaTitleSearchDoc;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmSearch;
        private System.Windows.Forms.TextBox otbTicketNo;
        private System.Windows.Forms.Label olaTitleSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleDocNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleDatetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitlePos;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleAmt;
    }
}