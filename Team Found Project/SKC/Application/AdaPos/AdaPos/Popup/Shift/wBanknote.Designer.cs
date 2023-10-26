namespace AdaPos.Popup.wSale
{
    partial class wBanknote
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
            this.ogdBankNote = new System.Windows.Forms.DataGridView();
            this.otbType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbFCBntRateAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbQuantityValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.opnPage = new System.Windows.Forms.Panel();
            this.opnBody = new System.Windows.Forms.Panel();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitle = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.opnFood = new System.Windows.Forms.Panel();
            this.olaResult = new System.Windows.Forms.Label();
            this.olaRemark = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ogdBankNote)).BeginInit();
            this.opnPage.SuspendLayout();
            this.opnBody.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.opnFood.SuspendLayout();
            this.SuspendLayout();
            // 
            // ogdBankNote
            // 
            this.ogdBankNote.AllowUserToAddRows = false;
            this.ogdBankNote.AllowUserToDeleteRows = false;
            this.ogdBankNote.AllowUserToResizeColumns = false;
            this.ogdBankNote.AllowUserToResizeRows = false;
            this.ogdBankNote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ogdBankNote.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ogdBankNote.BackgroundColor = System.Drawing.Color.White;
            this.ogdBankNote.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdBankNote.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ogdBankNote.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ogdBankNote.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.otbType,
            this.otbQuantity,
            this.otbAmt,
            this.otbFCBntRateAmt,
            this.otbQuantityValue});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdBankNote.DefaultCellStyle = dataGridViewCellStyle4;
            this.ogdBankNote.EnableHeadersVisualStyles = false;
            this.ogdBankNote.GridColor = System.Drawing.Color.Gainsboro;
            this.ogdBankNote.Location = new System.Drawing.Point(4, 4);
            this.ogdBankNote.MultiSelect = false;
            this.ogdBankNote.Name = "ogdBankNote";
            this.ogdBankNote.RowHeadersVisible = false;
            this.ogdBankNote.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.ogdBankNote.RowTemplate.Height = 40;
            this.ogdBankNote.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ogdBankNote.ShowCellErrors = false;
            this.ogdBankNote.ShowCellToolTips = false;
            this.ogdBankNote.ShowEditingIcon = false;
            this.ogdBankNote.ShowRowErrors = false;
            this.ogdBankNote.Size = new System.Drawing.Size(458, 401);
            this.ogdBankNote.TabIndex = 16;
            this.ogdBankNote.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.ogdBankNote_CellPainting);
            this.ogdBankNote.SelectionChanged += new System.EventHandler(this.ogdBankNote_SelectionChanged);
            this.ogdBankNote.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ogdBankNote_KeyDown);
            // 
            // otbType
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.otbType.DefaultCellStyle = dataGridViewCellStyle2;
            this.otbType.FillWeight = 120F;
            this.otbType.HeaderText = "ประเภท";
            this.otbType.Name = "otbType";
            this.otbType.ReadOnly = true;
            // 
            // otbQuantity
            // 
            this.otbQuantity.FillWeight = 110F;
            this.otbQuantity.HeaderText = "จำนวน";
            this.otbQuantity.Name = "otbQuantity";
            this.otbQuantity.ReadOnly = true;
            this.otbQuantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbAmt
            // 
            this.otbAmt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.otbAmt.DefaultCellStyle = dataGridViewCellStyle3;
            this.otbAmt.FillWeight = 110F;
            this.otbAmt.HeaderText = "จำนวนเงิน";
            this.otbAmt.Name = "otbAmt";
            this.otbAmt.ReadOnly = true;
            this.otbAmt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbFCBntRateAmt
            // 
            this.otbFCBntRateAmt.HeaderText = "FCBntRateAmt";
            this.otbFCBntRateAmt.Name = "otbFCBntRateAmt";
            this.otbFCBntRateAmt.Visible = false;
            // 
            // otbQuantityValue
            // 
            this.otbQuantityValue.HeaderText = "QuantityValue";
            this.otbQuantityValue.Name = "otbQuantityValue";
            this.otbQuantityValue.Visible = false;
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.opnBody);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Controls.Add(this.opnFood);
            this.opnPage.Location = new System.Drawing.Point(254, 91);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(469, 517);
            this.opnPage.TabIndex = 26;
            // 
            // opnBody
            // 
            this.opnBody.Controls.Add(this.ogdBankNote);
            this.opnBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnBody.Location = new System.Drawing.Point(1, 51);
            this.opnBody.Margin = new System.Windows.Forms.Padding(2);
            this.opnBody.Name = "opnBody";
            this.opnBody.Size = new System.Drawing.Size(465, 406);
            this.opnBody.TabIndex = 18;
            // 
            // opnHD
            // 
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitle);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Dock = System.Windows.Forms.DockStyle.Top;
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(465, 50);
            this.opnHD.TabIndex = 1;
            // 
            // olaTitle
            // 
            this.olaTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitle.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitle.ForeColor = System.Drawing.Color.White;
            this.olaTitle.Location = new System.Drawing.Point(56, 0);
            this.olaTitle.Name = "olaTitle";
            this.olaTitle.Size = new System.Drawing.Size(352, 50);
            this.olaTitle.TabIndex = 7;
            this.olaTitle.Text = "Banknote";
            this.olaTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.ocmBack.TabIndex = 4;
            this.ocmBack.UseVisualStyleBackColor = true;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.Location = new System.Drawing.Point(414, 0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 5;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // opnFood
            // 
            this.opnFood.Controls.Add(this.olaResult);
            this.opnFood.Controls.Add(this.olaRemark);
            this.opnFood.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.opnFood.Location = new System.Drawing.Point(1, 457);
            this.opnFood.Margin = new System.Windows.Forms.Padding(2);
            this.opnFood.Name = "opnFood";
            this.opnFood.Size = new System.Drawing.Size(465, 57);
            this.opnFood.TabIndex = 17;
            // 
            // olaResult
            // 
            this.olaResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.olaResult.Font = new System.Drawing.Font("Segoe UI Semibold", 15F, System.Drawing.FontStyle.Bold);
            this.olaResult.ForeColor = System.Drawing.Color.Red;
            this.olaResult.Location = new System.Drawing.Point(266, 12);
            this.olaResult.Name = "olaResult";
            this.olaResult.Size = new System.Drawing.Size(184, 26);
            this.olaResult.TabIndex = 10;
            this.olaResult.Text = "0.00";
            this.olaResult.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaRemark
            // 
            this.olaRemark.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaRemark.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaRemark.ForeColor = System.Drawing.Color.Black;
            this.olaRemark.Location = new System.Drawing.Point(11, 15);
            this.olaRemark.Name = "olaRemark";
            this.olaRemark.Size = new System.Drawing.Size(249, 26);
            this.olaRemark.TabIndex = 9;
            this.olaRemark.Text = "สรุปยอดเงินสดทั้งหมด";
            this.olaRemark.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // wBanknote
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(976, 730);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "wBanknote";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1365, 945";
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.wBanknote_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.ogdBankNote)).EndInit();
            this.opnPage.ResumeLayout(false);
            this.opnBody.ResumeLayout(false);
            this.opnHD.ResumeLayout(false);
            this.opnFood.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitle;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Panel opnBody;
        private System.Windows.Forms.Panel opnFood;
        private System.Windows.Forms.Label olaRemark;
        public System.Windows.Forms.Label olaResult;
        public System.Windows.Forms.DataGridView ogdBankNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbType;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbFCBntRateAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbQuantityValue;
    }
}