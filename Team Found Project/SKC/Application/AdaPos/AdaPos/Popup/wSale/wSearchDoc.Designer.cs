namespace AdaPos.Popup.wSale
{
    partial class wSearchDoc
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.opnPage = new System.Windows.Forms.Panel();
            this.ocmSearch = new System.Windows.Forms.Button();
            this.otbTicketNo = new System.Windows.Forms.TextBox();
            this.olaTitleSearch = new System.Windows.Forms.Label();
            this.ogdDoc = new System.Windows.Forms.DataGridView();
            this.otbTitleDocNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleDatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitlePos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitleAbout = new System.Windows.Forms.Label();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.ocmBack = new System.Windows.Forms.Button();
            this.opnPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdDoc)).BeginInit();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.ocmSearch);
            this.opnPage.Controls.Add(this.otbTicketNo);
            this.opnPage.Controls.Add(this.olaTitleSearch);
            this.opnPage.Controls.Add(this.ogdDoc);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(162, 159);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(700, 450);
            this.opnPage.TabIndex = 1;
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
            this.ocmSearch.Location = new System.Drawing.Point(617, 88);
            this.ocmSearch.Name = "ocmSearch";
            this.ocmSearch.Size = new System.Drawing.Size(60, 36);
            this.ocmSearch.TabIndex = 25;
            this.ocmSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ocmSearch.UseVisualStyleBackColor = false;
            this.ocmSearch.Click += new System.EventHandler(this.ocmSearch_Click);
            // 
            // otbTicketNo
            // 
            this.otbTicketNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbTicketNo.Font = new System.Drawing.Font("Segoe UI Light", 16F);
            this.otbTicketNo.Location = new System.Drawing.Point(21, 88);
            this.otbTicketNo.Name = "otbTicketNo";
            this.otbTicketNo.Size = new System.Drawing.Size(596, 36);
            this.otbTicketNo.TabIndex = 27;
            // 
            // olaTitleSearch
            // 
            this.olaTitleSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaTitleSearch.Location = new System.Drawing.Point(17, 63);
            this.olaTitleSearch.Name = "olaTitleSearch";
            this.olaTitleSearch.Size = new System.Drawing.Size(226, 22);
            this.olaTitleSearch.TabIndex = 26;
            this.olaTitleSearch.Text = "Search";
            this.olaTitleSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ogdDoc
            // 
            this.ogdDoc.AllowUserToAddRows = false;
            this.ogdDoc.AllowUserToDeleteRows = false;
            this.ogdDoc.AllowUserToResizeColumns = false;
            this.ogdDoc.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ogdDoc.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ogdDoc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ogdDoc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ogdDoc.BackgroundColor = System.Drawing.Color.White;
            this.ogdDoc.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdDoc.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ogdDoc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ogdDoc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.otbTitleDocNo,
            this.otbTitleDatetime,
            this.otbTitlePos,
            this.otbTitleAmount});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdDoc.DefaultCellStyle = dataGridViewCellStyle4;
            this.ogdDoc.EnableHeadersVisualStyles = false;
            this.ogdDoc.GridColor = System.Drawing.Color.Gainsboro;
            this.ogdDoc.Location = new System.Drawing.Point(21, 140);
            this.ogdDoc.MultiSelect = false;
            this.ogdDoc.Name = "ogdDoc";
            this.ogdDoc.RowHeadersVisible = false;
            this.ogdDoc.RowTemplate.Height = 40;
            this.ogdDoc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ogdDoc.ShowCellErrors = false;
            this.ogdDoc.ShowCellToolTips = false;
            this.ogdDoc.ShowEditingIcon = false;
            this.ogdDoc.ShowRowErrors = false;
            this.ogdDoc.Size = new System.Drawing.Size(656, 287);
            this.ogdDoc.TabIndex = 24;
            this.ogdDoc.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ogdDoc_CellClick);
            this.ogdDoc.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ogdDoc_CellDoubleClick);
            // 
            // otbTitleDocNo
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.otbTitleDocNo.DefaultCellStyle = dataGridViewCellStyle3;
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
            this.otbTitlePos.FillWeight = 60F;
            this.otbTitlePos.HeaderText = "POS";
            this.otbTitlePos.Name = "otbTitlePos";
            this.otbTitlePos.ReadOnly = true;
            this.otbTitlePos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitleAmount
            // 
            this.otbTitleAmount.FillWeight = 60F;
            this.otbTitleAmount.HeaderText = "Amount";
            this.otbTitleAmount.Name = "otbTitleAmount";
            this.otbTitleAmount.ReadOnly = true;
            this.otbTitleAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitleAbout);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(696, 50);
            this.opnHD.TabIndex = 1;
            // 
            // olaTitleAbout
            // 
            this.olaTitleAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleAbout.BackColor = System.Drawing.Color.Transparent;
            this.olaTitleAbout.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleAbout.ForeColor = System.Drawing.Color.White;
            this.olaTitleAbout.Location = new System.Drawing.Point(53, 0);
            this.olaTitleAbout.Name = "olaTitleAbout";
            this.olaTitleAbout.Size = new System.Drawing.Size(293, 50);
            this.olaTitleAbout.TabIndex = 9;
            this.olaTitleAbout.Text = "Refer to bill by wristband";
            this.olaTitleAbout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.ocmAccept.Location = new System.Drawing.Point(650, 0);
            this.ocmAccept.Margin = new System.Windows.Forms.Padding(0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 8;
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
            this.ocmBack.TabIndex = 7;
            this.ocmBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmBack.UseVisualStyleBackColor = false;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // wSearchDoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnPage);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wSearchDoc";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wSearchDoc_FormClosing);
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdDoc)).EndInit();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.DataGridView ogdDoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleDocNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleDatetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitlePos;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleAmount;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitleAbout;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmSearch;
        private System.Windows.Forms.TextBox otbTicketNo;
        private System.Windows.Forms.Label olaTitleSearch;
    }
}