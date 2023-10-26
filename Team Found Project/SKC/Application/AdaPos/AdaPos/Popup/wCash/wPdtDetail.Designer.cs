namespace AdaPos.Popup.wCash
{
    partial class wPdtDetail
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.opnPage = new System.Windows.Forms.Panel();
            this.opnSummarize = new System.Windows.Forms.TableLayoutPanel();
            this.olaTitleSum = new System.Windows.Forms.Label();
            this.olaSumQty = new System.Windows.Forms.Label();
            this.olaSumAmt = new System.Windows.Forms.Label();
            this.ogdSelectABB = new System.Windows.Forms.DataGridView();
            this.otbTitleSeq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitlePdtCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitlePdtName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitlePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitlePdtList = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.opnPage.SuspendLayout();
            this.opnSummarize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdSelectABB)).BeginInit();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.opnSummarize);
            this.opnPage.Controls.Add(this.ogdSelectABB);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(162, 109);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(700, 550);
            this.opnPage.TabIndex = 2;
            // 
            // opnSummarize
            // 
            this.opnSummarize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnSummarize.ColumnCount = 6;
            this.opnSummarize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.692307F));
            this.opnSummarize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.51282F));
            this.opnSummarize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.64103F));
            this.opnSummarize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.38461F));
            this.opnSummarize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.38461F));
            this.opnSummarize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.38461F));
            this.opnSummarize.Controls.Add(this.olaTitleSum, 2, 0);
            this.opnSummarize.Controls.Add(this.olaSumQty, 4, 0);
            this.opnSummarize.Controls.Add(this.olaSumAmt, 5, 0);
            this.opnSummarize.Location = new System.Drawing.Point(11, 502);
            this.opnSummarize.Name = "opnSummarize";
            this.opnSummarize.RowCount = 1;
            this.opnSummarize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opnSummarize.Size = new System.Drawing.Size(676, 35);
            this.opnSummarize.TabIndex = 10;
            // 
            // olaTitleSum
            // 
            this.olaTitleSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnSummarize.SetColumnSpan(this.olaTitleSum, 2);
            this.olaTitleSum.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaTitleSum.Location = new System.Drawing.Point(189, 0);
            this.olaTitleSum.Margin = new System.Windows.Forms.Padding(0);
            this.olaTitleSum.Name = "olaTitleSum";
            this.olaTitleSum.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.olaTitleSum.Size = new System.Drawing.Size(276, 35);
            this.olaTitleSum.TabIndex = 10;
            this.olaTitleSum.Text = "Summary";
            this.olaTitleSum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaSumQty
            // 
            this.olaSumQty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaSumQty.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaSumQty.Location = new System.Drawing.Point(465, 0);
            this.olaSumQty.Margin = new System.Windows.Forms.Padding(0);
            this.olaSumQty.Name = "olaSumQty";
            this.olaSumQty.Size = new System.Drawing.Size(103, 35);
            this.olaSumQty.TabIndex = 11;
            this.olaSumQty.Text = "0";
            this.olaSumQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaSumAmt
            // 
            this.olaSumAmt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaSumAmt.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaSumAmt.Location = new System.Drawing.Point(568, 0);
            this.olaSumAmt.Margin = new System.Windows.Forms.Padding(0);
            this.olaSumAmt.Name = "olaSumAmt";
            this.olaSumAmt.Size = new System.Drawing.Size(108, 35);
            this.olaSumAmt.TabIndex = 12;
            this.olaSumAmt.Text = "0.00";
            this.olaSumAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.otbTitleSeq,
            this.otbTitlePdtCode,
            this.otbTitlePdtName,
            this.otbTitlePrice,
            this.otbTitleQty,
            this.otbTitleAmt});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdSelectABB.DefaultCellStyle = dataGridViewCellStyle8;
            this.ogdSelectABB.EnableHeadersVisualStyles = false;
            this.ogdSelectABB.GridColor = System.Drawing.Color.Gainsboro;
            this.ogdSelectABB.Location = new System.Drawing.Point(11, 61);
            this.ogdSelectABB.MultiSelect = false;
            this.ogdSelectABB.Name = "ogdSelectABB";
            this.ogdSelectABB.RowHeadersVisible = false;
            this.ogdSelectABB.RowTemplate.Height = 40;
            this.ogdSelectABB.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ogdSelectABB.ShowCellErrors = false;
            this.ogdSelectABB.ShowCellToolTips = false;
            this.ogdSelectABB.ShowEditingIcon = false;
            this.ogdSelectABB.ShowRowErrors = false;
            this.ogdSelectABB.Size = new System.Drawing.Size(676, 438);
            this.ogdSelectABB.TabIndex = 2;
            // 
            // otbTitleSeq
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.otbTitleSeq.DefaultCellStyle = dataGridViewCellStyle2;
            this.otbTitleSeq.FillWeight = 30F;
            this.otbTitleSeq.HeaderText = "Seq";
            this.otbTitleSeq.Name = "otbTitleSeq";
            this.otbTitleSeq.ReadOnly = true;
            this.otbTitleSeq.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.otbTitleSeq.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitlePdtCode
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.otbTitlePdtCode.DefaultCellStyle = dataGridViewCellStyle3;
            this.otbTitlePdtCode.FillWeight = 80F;
            this.otbTitlePdtCode.HeaderText = "Product Code";
            this.otbTitlePdtCode.Name = "otbTitlePdtCode";
            this.otbTitlePdtCode.ReadOnly = true;
            this.otbTitlePdtCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitlePdtName
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.otbTitlePdtName.DefaultCellStyle = dataGridViewCellStyle4;
            this.otbTitlePdtName.HeaderText = "Product Name";
            this.otbTitlePdtName.Name = "otbTitlePdtName";
            this.otbTitlePdtName.ReadOnly = true;
            this.otbTitlePdtName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitlePrice
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.otbTitlePrice.DefaultCellStyle = dataGridViewCellStyle5;
            this.otbTitlePrice.FillWeight = 60F;
            this.otbTitlePrice.HeaderText = "POS";
            this.otbTitlePrice.Name = "otbTitlePrice";
            this.otbTitlePrice.ReadOnly = true;
            this.otbTitlePrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitleQty
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.otbTitleQty.DefaultCellStyle = dataGridViewCellStyle6;
            this.otbTitleQty.FillWeight = 60F;
            this.otbTitleQty.HeaderText = "Qty";
            this.otbTitleQty.Name = "otbTitleQty";
            this.otbTitleQty.ReadOnly = true;
            this.otbTitleQty.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitleAmt
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.otbTitleAmt.DefaultCellStyle = dataGridViewCellStyle7;
            this.otbTitleAmt.FillWeight = 60F;
            this.otbTitleAmt.HeaderText = "Amount";
            this.otbTitleAmt.Name = "otbTitleAmt";
            this.otbTitleAmt.ReadOnly = true;
            this.otbTitleAmt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitlePdtList);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(696, 50);
            this.opnHD.TabIndex = 1;
            // 
            // olaTitlePdtList
            // 
            this.olaTitlePdtList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitlePdtList.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitlePdtList.ForeColor = System.Drawing.Color.White;
            this.olaTitlePdtList.Location = new System.Drawing.Point(56, 0);
            this.olaTitlePdtList.Name = "olaTitlePdtList";
            this.olaTitlePdtList.Size = new System.Drawing.Size(351, 50);
            this.olaTitlePdtList.TabIndex = 1;
            this.olaTitlePdtList.Text = "Product List";
            this.olaTitlePdtList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // wPdtDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wPdtDetail";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.opnPage.ResumeLayout(false);
            this.opnSummarize.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ogdSelectABB)).EndInit();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.DataGridView ogdSelectABB;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitlePdtList;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.TableLayoutPanel opnSummarize;
        private System.Windows.Forms.Label olaTitleSum;
        private System.Windows.Forms.Label olaSumQty;
        private System.Windows.Forms.Label olaSumAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleSeq;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitlePdtCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitlePdtName;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitlePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleAmt;
    }
}