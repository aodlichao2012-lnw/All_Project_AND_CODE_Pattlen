namespace AdaPos.Popup.wSpotCheck
{
    partial class wSpotCheckHistory
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.opnPage = new System.Windows.Forms.Panel();
            this.opnContent = new System.Windows.Forms.TableLayoutPanel();
            this.ogdHistory = new System.Windows.Forms.DataGridView();
            this.otbTitleSeq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleDocNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleDatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitlePos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.opnDate = new System.Windows.Forms.Panel();
            this.ocmSchHistory = new System.Windows.Forms.Button();
            this.olaTitleDateSC = new System.Windows.Forms.Label();
            this.otbSaleDate = new System.Windows.Forms.MaskedTextBox();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitleHistory = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.opnPage.SuspendLayout();
            this.opnContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdHistory)).BeginInit();
            this.opnDate.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.opnContent);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(112, 84);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(800, 600);
            this.opnPage.TabIndex = 0;
            // 
            // opnContent
            // 
            this.opnContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnContent.ColumnCount = 2;
            this.opnContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opnContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opnContent.Controls.Add(this.ogdHistory, 0, 1);
            this.opnContent.Controls.Add(this.opnDate, 0, 0);
            this.opnContent.Location = new System.Drawing.Point(21, 71);
            this.opnContent.Name = "opnContent";
            this.opnContent.RowCount = 2;
            this.opnContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.opnContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opnContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opnContent.Size = new System.Drawing.Size(756, 506);
            this.opnContent.TabIndex = 2;
            // 
            // ogdHistory
            // 
            this.ogdHistory.AllowUserToAddRows = false;
            this.ogdHistory.AllowUserToDeleteRows = false;
            this.ogdHistory.AllowUserToResizeColumns = false;
            this.ogdHistory.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ogdHistory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ogdHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ogdHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ogdHistory.BackgroundColor = System.Drawing.Color.White;
            this.ogdHistory.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ogdHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ogdHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.otbTitleSeq,
            this.otbTitleDocNo,
            this.otbTitleDatetime,
            this.otbTitleType,
            this.otbTitlePos,
            this.otbTitleAmt});
            this.opnContent.SetColumnSpan(this.ogdHistory, 2);
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdHistory.DefaultCellStyle = dataGridViewCellStyle3;
            this.ogdHistory.EnableHeadersVisualStyles = false;
            this.ogdHistory.GridColor = System.Drawing.Color.Gainsboro;
            this.ogdHistory.Location = new System.Drawing.Point(0, 80);
            this.ogdHistory.Margin = new System.Windows.Forms.Padding(0);
            this.ogdHistory.MultiSelect = false;
            this.ogdHistory.Name = "ogdHistory";
            this.ogdHistory.ReadOnly = true;
            this.ogdHistory.RowHeadersVisible = false;
            this.ogdHistory.RowTemplate.Height = 40;
            this.ogdHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ogdHistory.ShowCellErrors = false;
            this.ogdHistory.ShowCellToolTips = false;
            this.ogdHistory.ShowEditingIcon = false;
            this.ogdHistory.ShowRowErrors = false;
            this.ogdHistory.Size = new System.Drawing.Size(756, 426);
            this.ogdHistory.TabIndex = 2;
            // 
            // otbTitleSeq
            // 
            this.otbTitleSeq.FillWeight = 40F;
            this.otbTitleSeq.HeaderText = "Seq";
            this.otbTitleSeq.Name = "otbTitleSeq";
            this.otbTitleSeq.ReadOnly = true;
            this.otbTitleSeq.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            // otbTitleType
            // 
            this.otbTitleType.FillWeight = 60F;
            this.otbTitleType.HeaderText = "Type";
            this.otbTitleType.Name = "otbTitleType";
            this.otbTitleType.ReadOnly = true;
            this.otbTitleType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            this.otbTitleAmt.FillWeight = 60F;
            this.otbTitleAmt.HeaderText = "Amount";
            this.otbTitleAmt.Name = "otbTitleAmt";
            this.otbTitleAmt.ReadOnly = true;
            this.otbTitleAmt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // opnDate
            // 
            this.opnDate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnContent.SetColumnSpan(this.opnDate, 2);
            this.opnDate.Controls.Add(this.ocmSchHistory);
            this.opnDate.Controls.Add(this.olaTitleDateSC);
            this.opnDate.Controls.Add(this.otbSaleDate);
            this.opnDate.Location = new System.Drawing.Point(0, 0);
            this.opnDate.Margin = new System.Windows.Forms.Padding(0);
            this.opnDate.Name = "opnDate";
            this.opnDate.Size = new System.Drawing.Size(756, 80);
            this.opnDate.TabIndex = 3;
            // 
            // ocmSchHistory
            // 
            this.ocmSchHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmSchHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(171)))), ((int)(((byte)(132)))));
            this.ocmSchHistory.FlatAppearance.BorderSize = 0;
            this.ocmSchHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmSchHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ocmSchHistory.ForeColor = System.Drawing.Color.White;
            this.ocmSchHistory.Image = global::AdaPos.Properties.Resources.SearchW_32;
            this.ocmSchHistory.Location = new System.Drawing.Point(466, 34);
            this.ocmSchHistory.Name = "ocmSchHistory";
            this.ocmSchHistory.Size = new System.Drawing.Size(38, 32);
            this.ocmSchHistory.TabIndex = 6;
            this.ocmSchHistory.Tag = "";
            this.ocmSchHistory.UseVisualStyleBackColor = false;
            // 
            // olaTitleDateSC
            // 
            this.olaTitleDateSC.AutoSize = true;
            this.olaTitleDateSC.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaTitleDateSC.Location = new System.Drawing.Point(292, 8);
            this.olaTitleDateSC.Name = "olaTitleDateSC";
            this.olaTitleDateSC.Size = new System.Drawing.Size(135, 19);
            this.olaTitleDateSC.TabIndex = 5;
            this.olaTitleDateSC.Text = "Date for Spot Check";
            this.olaTitleDateSC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // otbSaleDate
            // 
            this.otbSaleDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbSaleDate.Enabled = false;
            this.otbSaleDate.Font = new System.Drawing.Font("Segoe UI Light", 14F);
            this.otbSaleDate.Location = new System.Drawing.Point(296, 34);
            this.otbSaleDate.Mask = "00/00/0000";
            this.otbSaleDate.Name = "otbSaleDate";
            this.otbSaleDate.Size = new System.Drawing.Size(164, 32);
            this.otbSaleDate.TabIndex = 2;
            this.otbSaleDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitleHistory);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(796, 50);
            this.opnHD.TabIndex = 1;
            // 
            // olaTitleHistory
            // 
            this.olaTitleHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleHistory.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleHistory.ForeColor = System.Drawing.Color.White;
            this.olaTitleHistory.Location = new System.Drawing.Point(56, 0);
            this.olaTitleHistory.Name = "olaTitleHistory";
            this.olaTitleHistory.Size = new System.Drawing.Size(451, 50);
            this.olaTitleHistory.TabIndex = 1;
            this.olaTitleHistory.Text = "History";
            this.olaTitleHistory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // wSpotCheckHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wSpotCheckHistory";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wSpotCheckHistory_FormClosing);
            this.Shown += new System.EventHandler(this.wSpotCheckHistory_Shown);
            this.opnPage.ResumeLayout(false);
            this.opnContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ogdHistory)).EndInit();
            this.opnDate.ResumeLayout(false);
            this.opnDate.PerformLayout();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Label olaTitleHistory;
        private System.Windows.Forms.DataGridView ogdHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleSeq;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleDocNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleDatetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleType;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitlePos;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleAmt;
        private System.Windows.Forms.TableLayoutPanel opnContent;
        private System.Windows.Forms.Panel opnDate;
        private System.Windows.Forms.MaskedTextBox otbSaleDate;
        private System.Windows.Forms.Button ocmSchHistory;
        private System.Windows.Forms.Label olaTitleDateSC;
    }
}