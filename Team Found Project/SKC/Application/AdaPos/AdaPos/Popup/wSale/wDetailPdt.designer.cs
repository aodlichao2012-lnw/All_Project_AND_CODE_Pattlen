namespace AdaPos.Popup.wSale
{
    partial class wDetailPdt
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.opnDetail = new System.Windows.Forms.Panel();
            this.opnHD = new System.Windows.Forms.Panel();
            this.ocmShwKb = new System.Windows.Forms.Button();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.olaTitlePdtDesc = new System.Windows.Forms.Label();
            this.opnContent = new System.Windows.Forms.Panel();
            this.ogdPdtBarcode = new System.Windows.Forms.DataGridView();
            this.ocmTitleFavorite = new System.Windows.Forms.DataGridViewButtonColumn();
            this.otbTitleBarcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleFactor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitlePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.opnDescription = new System.Windows.Forms.TableLayoutPanel();
            this.olaPdtName = new System.Windows.Forms.Label();
            this.olaPdtCode = new System.Windows.Forms.Label();
            this.olaTitlePdtCode = new System.Windows.Forms.Label();
            this.olaTitlePdtName = new System.Windows.Forms.Label();
            this.opbPdt = new System.Windows.Forms.PictureBox();
            this.opnDetail.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.opnContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdPdtBarcode)).BeginInit();
            this.opnDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opbPdt)).BeginInit();
            this.SuspendLayout();
            // 
            // opnDetail
            // 
            this.opnDetail.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.opnDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnDetail.Controls.Add(this.opnHD);
            this.opnDetail.Controls.Add(this.opnContent);
            this.opnDetail.Location = new System.Drawing.Point(112, 84);
            this.opnDetail.Name = "opnDetail";
            this.opnDetail.Padding = new System.Windows.Forms.Padding(1);
            this.opnDetail.Size = new System.Drawing.Size(800, 600);
            this.opnDetail.TabIndex = 1;
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.ocmShwKb);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Controls.Add(this.olaTitlePdtDesc);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(796, 50);
            this.opnHD.TabIndex = 10;
            // 
            // ocmShwKb
            // 
            this.ocmShwKb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmShwKb.FlatAppearance.BorderSize = 0;
            this.ocmShwKb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmShwKb.Image = global::AdaPos.Properties.Resources.Fn;
            this.ocmShwKb.Location = new System.Drawing.Point(693, 0);
            this.ocmShwKb.Name = "ocmShwKb";
            this.ocmShwKb.Size = new System.Drawing.Size(50, 50);
            this.ocmShwKb.TabIndex = 10;
            this.ocmShwKb.UseVisualStyleBackColor = true;
            this.ocmShwKb.Click += new System.EventHandler(this.ocmShwKb_Click);
            // 
            // ocmBack
            // 
            this.ocmBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
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
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmAccept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmAccept.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmAccept.ForeColor = System.Drawing.Color.White;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmAccept.Location = new System.Drawing.Point(746, 0);
            this.ocmAccept.Margin = new System.Windows.Forms.Padding(0);
            this.ocmAccept.MaximumSize = new System.Drawing.Size(250, 50);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 9;
            this.ocmAccept.Tag = "0";
            this.ocmAccept.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmAccept.UseVisualStyleBackColor = false;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // olaTitlePdtDesc
            // 
            this.olaTitlePdtDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitlePdtDesc.BackColor = System.Drawing.Color.Transparent;
            this.olaTitlePdtDesc.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitlePdtDesc.ForeColor = System.Drawing.Color.White;
            this.olaTitlePdtDesc.Location = new System.Drawing.Point(53, 0);
            this.olaTitlePdtDesc.Name = "olaTitlePdtDesc";
            this.olaTitlePdtDesc.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.olaTitlePdtDesc.Size = new System.Drawing.Size(398, 50);
            this.olaTitlePdtDesc.TabIndex = 5;
            this.olaTitlePdtDesc.Text = "Product Description";
            this.olaTitlePdtDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opnContent
            // 
            this.opnContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnContent.BackColor = System.Drawing.Color.White;
            this.opnContent.Controls.Add(this.ogdPdtBarcode);
            this.opnContent.Controls.Add(this.opnDescription);
            this.opnContent.Controls.Add(this.opbPdt);
            this.opnContent.Location = new System.Drawing.Point(4, 53);
            this.opnContent.Name = "opnContent";
            this.opnContent.Size = new System.Drawing.Size(790, 541);
            this.opnContent.TabIndex = 8;
            // 
            // ogdPdtBarcode
            // 
            this.ogdPdtBarcode.AllowUserToAddRows = false;
            this.ogdPdtBarcode.AllowUserToDeleteRows = false;
            this.ogdPdtBarcode.AllowUserToResizeColumns = false;
            this.ogdPdtBarcode.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ogdPdtBarcode.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ogdPdtBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ogdPdtBarcode.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ogdPdtBarcode.BackgroundColor = System.Drawing.Color.White;
            this.ogdPdtBarcode.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdPdtBarcode.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ogdPdtBarcode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ogdPdtBarcode.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ocmTitleFavorite,
            this.otbTitleBarcode,
            this.otbTitleUnit,
            this.otbTitleFactor,
            this.otbTitlePrice});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdPdtBarcode.DefaultCellStyle = dataGridViewCellStyle6;
            this.ogdPdtBarcode.EnableHeadersVisualStyles = false;
            this.ogdPdtBarcode.GridColor = System.Drawing.Color.Gainsboro;
            this.ogdPdtBarcode.Location = new System.Drawing.Point(20, 180);
            this.ogdPdtBarcode.MultiSelect = false;
            this.ogdPdtBarcode.Name = "ogdPdtBarcode";
            this.ogdPdtBarcode.RowHeadersVisible = false;
            this.ogdPdtBarcode.RowTemplate.Height = 40;
            this.ogdPdtBarcode.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ogdPdtBarcode.ShowCellErrors = false;
            this.ogdPdtBarcode.ShowCellToolTips = false;
            this.ogdPdtBarcode.ShowEditingIcon = false;
            this.ogdPdtBarcode.ShowRowErrors = false;
            this.ogdPdtBarcode.Size = new System.Drawing.Size(750, 351);
            this.ogdPdtBarcode.TabIndex = 25;
            // 
            // ocmTitleFavorite
            // 
            this.ocmTitleFavorite.FillWeight = 50F;
            this.ocmTitleFavorite.HeaderText = "Favorite";
            this.ocmTitleFavorite.Name = "ocmTitleFavorite";
            this.ocmTitleFavorite.ReadOnly = true;
            this.ocmTitleFavorite.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // otbTitleBarcode
            // 
            this.otbTitleBarcode.HeaderText = "Barcode";
            this.otbTitleBarcode.Name = "otbTitleBarcode";
            this.otbTitleBarcode.ReadOnly = true;
            this.otbTitleBarcode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitleUnit
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.otbTitleUnit.DefaultCellStyle = dataGridViewCellStyle3;
            this.otbTitleUnit.FillWeight = 80F;
            this.otbTitleUnit.HeaderText = "Unit";
            this.otbTitleUnit.Name = "otbTitleUnit";
            this.otbTitleUnit.ReadOnly = true;
            this.otbTitleUnit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitleFactor
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.otbTitleFactor.DefaultCellStyle = dataGridViewCellStyle4;
            this.otbTitleFactor.FillWeight = 60F;
            this.otbTitleFactor.HeaderText = "Factor";
            this.otbTitleFactor.Name = "otbTitleFactor";
            this.otbTitleFactor.ReadOnly = true;
            this.otbTitleFactor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitlePrice
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.otbTitlePrice.DefaultCellStyle = dataGridViewCellStyle5;
            this.otbTitlePrice.FillWeight = 60F;
            this.otbTitlePrice.HeaderText = "Price";
            this.otbTitlePrice.Name = "otbTitlePrice";
            this.otbTitlePrice.ReadOnly = true;
            this.otbTitlePrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // opnDescription
            // 
            this.opnDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnDescription.ColumnCount = 2;
            this.opnDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.opnDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.opnDescription.Controls.Add(this.olaPdtName, 1, 1);
            this.opnDescription.Controls.Add(this.olaPdtCode, 1, 0);
            this.opnDescription.Controls.Add(this.olaTitlePdtCode, 0, 0);
            this.opnDescription.Controls.Add(this.olaTitlePdtName, 0, 1);
            this.opnDescription.Location = new System.Drawing.Point(190, 6);
            this.opnDescription.Name = "opnDescription";
            this.opnDescription.RowCount = 2;
            this.opnDescription.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opnDescription.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opnDescription.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opnDescription.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opnDescription.Size = new System.Drawing.Size(580, 81);
            this.opnDescription.TabIndex = 2;
            // 
            // olaPdtName
            // 
            this.olaPdtName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaPdtName.AutoSize = true;
            this.olaPdtName.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaPdtName.Location = new System.Drawing.Point(235, 40);
            this.olaPdtName.Name = "olaPdtName";
            this.olaPdtName.Size = new System.Drawing.Size(342, 41);
            this.olaPdtName.TabIndex = 5;
            this.olaPdtName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // olaPdtCode
            // 
            this.olaPdtCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaPdtCode.AutoSize = true;
            this.olaPdtCode.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaPdtCode.Location = new System.Drawing.Point(235, 0);
            this.olaPdtCode.Name = "olaPdtCode";
            this.olaPdtCode.Size = new System.Drawing.Size(342, 40);
            this.olaPdtCode.TabIndex = 4;
            this.olaPdtCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // olaTitlePdtCode
            // 
            this.olaTitlePdtCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitlePdtCode.AutoSize = true;
            this.olaTitlePdtCode.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaTitlePdtCode.Location = new System.Drawing.Point(3, 0);
            this.olaTitlePdtCode.Name = "olaTitlePdtCode";
            this.olaTitlePdtCode.Size = new System.Drawing.Size(226, 40);
            this.olaTitlePdtCode.TabIndex = 0;
            this.olaTitlePdtCode.Text = "Product Code : ";
            this.olaTitlePdtCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // olaTitlePdtName
            // 
            this.olaTitlePdtName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitlePdtName.AutoSize = true;
            this.olaTitlePdtName.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaTitlePdtName.Location = new System.Drawing.Point(3, 40);
            this.olaTitlePdtName.Name = "olaTitlePdtName";
            this.olaTitlePdtName.Size = new System.Drawing.Size(226, 41);
            this.olaTitlePdtName.TabIndex = 1;
            this.olaTitlePdtName.Text = "Product Name :";
            this.olaTitlePdtName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opbPdt
            // 
            this.opbPdt.Image = global::AdaPos.Properties.Resources.C_KBDxProduct;
            this.opbPdt.Location = new System.Drawing.Point(20, 10);
            this.opbPdt.Name = "opbPdt";
            this.opbPdt.Size = new System.Drawing.Size(150, 150);
            this.opbPdt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.opbPdt.TabIndex = 0;
            this.opbPdt.TabStop = false;
            // 
            // wDetailPdt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnDetail);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wDetailPdt";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wDetailPdt";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.opnDetail.ResumeLayout(false);
            this.opnHD.ResumeLayout(false);
            this.opnContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ogdPdtBarcode)).EndInit();
            this.opnDescription.ResumeLayout(false);
            this.opnDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opbPdt)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnDetail;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Label olaTitlePdtDesc;
        private System.Windows.Forms.Panel opnContent;
        private System.Windows.Forms.PictureBox opbPdt;
        private System.Windows.Forms.TableLayoutPanel opnDescription;
        private System.Windows.Forms.Label olaTitlePdtCode;
        private System.Windows.Forms.Label olaPdtCode;
        private System.Windows.Forms.Label olaTitlePdtName;
        private System.Windows.Forms.Label olaPdtName;
        private System.Windows.Forms.DataGridView ogdPdtBarcode;
        private System.Windows.Forms.Button ocmShwKb;
        private System.Windows.Forms.DataGridViewButtonColumn ocmTitleFavorite;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleBarcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleFactor;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitlePrice;
    }
}