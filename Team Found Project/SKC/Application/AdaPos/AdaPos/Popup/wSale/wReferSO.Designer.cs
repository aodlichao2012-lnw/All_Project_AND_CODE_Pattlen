namespace AdaPos.Popup.wSale
{
    partial class wReferSO
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
            this.olaCstName = new System.Windows.Forms.Label();
            this.ogdSo = new System.Windows.Forms.DataGridView();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitle = new System.Windows.Forms.Label();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.ocmBack = new System.Windows.Forms.Button();
            this.otbBchCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbDocNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbDocDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbGrand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbUsrKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbUsrApv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbCshOrCrd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.opnPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdSo)).BeginInit();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.olaCstName);
            this.opnPage.Controls.Add(this.ogdSo);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(1, 1);
            this.opnPage.Margin = new System.Windows.Forms.Padding(4);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(997, 551);
            this.opnPage.TabIndex = 0;
            // 
            // olaCstName
            // 
            this.olaCstName.AutoSize = true;
            this.olaCstName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaCstName.Location = new System.Drawing.Point(28, 69);
            this.olaCstName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaCstName.Name = "olaCstName";
            this.olaCstName.Size = new System.Drawing.Size(65, 28);
            this.olaCstName.TabIndex = 3;
            this.olaCstName.Text = "label1";
            // 
            // ogdSo
            // 
            this.ogdSo.AllowUserToAddRows = false;
            this.ogdSo.AllowUserToDeleteRows = false;
            this.ogdSo.AllowUserToResizeColumns = false;
            this.ogdSo.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ogdSo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ogdSo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ogdSo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ogdSo.BackgroundColor = System.Drawing.Color.White;
            this.ogdSo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ogdSo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ogdSo.ColumnHeadersHeight = 43;
            this.ogdSo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.ogdSo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.otbBchCode,
            this.otbDocNo,
            this.otbDocDate,
            this.otbGrand,
            this.otbUsrKey,
            this.otbUsrApv,
            this.otbCshOrCrd});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI Light", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdSo.DefaultCellStyle = dataGridViewCellStyle4;
            this.ogdSo.EnableHeadersVisualStyles = false;
            this.ogdSo.GridColor = System.Drawing.Color.Gainsboro;
            this.ogdSo.Location = new System.Drawing.Point(28, 105);
            this.ogdSo.Margin = new System.Windows.Forms.Padding(4);
            this.ogdSo.MultiSelect = false;
            this.ogdSo.Name = "ogdSo";
            this.ogdSo.RowHeadersVisible = false;
            this.ogdSo.RowTemplate.Height = 40;
            this.ogdSo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ogdSo.ShowCellErrors = false;
            this.ogdSo.ShowCellToolTips = false;
            this.ogdSo.ShowEditingIcon = false;
            this.ogdSo.ShowRowErrors = false;
            this.ogdSo.Size = new System.Drawing.Size(939, 417);
            this.ogdSo.TabIndex = 0;
            this.ogdSo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ogdSo_CellClick);
            this.ogdSo.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ogdSo_CellMouseDoubleClick);
            // 
            // opnHD
            // 
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitle);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Dock = System.Windows.Forms.DockStyle.Top;
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Margin = new System.Windows.Forms.Padding(4);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(993, 59);
            this.opnHD.TabIndex = 2;
            // 
            // olaTitle
            // 
            this.olaTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitle.BackColor = System.Drawing.Color.Transparent;
            this.olaTitle.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitle.ForeColor = System.Drawing.Color.White;
            this.olaTitle.Location = new System.Drawing.Point(71, 0);
            this.olaTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaTitle.Name = "olaTitle";
            this.olaTitle.Size = new System.Drawing.Size(456, 59);
            this.olaTitle.TabIndex = 9;
            this.olaTitle.Text = "Sale Order";
            this.olaTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.ocmAccept.Location = new System.Drawing.Point(925, 0);
            this.ocmAccept.Margin = new System.Windows.Forms.Padding(0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.ocmAccept.Size = new System.Drawing.Size(67, 59);
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
            this.ocmBack.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.ocmBack.Size = new System.Drawing.Size(67, 59);
            this.ocmBack.TabIndex = 7;
            this.ocmBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmBack.UseVisualStyleBackColor = false;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // otbBchCode
            // 
            this.otbBchCode.FillWeight = 80F;
            this.otbBchCode.HeaderText = "Branch Code";
            this.otbBchCode.Name = "otbBchCode";
            this.otbBchCode.ReadOnly = true;
            // 
            // otbDocNo
            // 
            this.otbDocNo.FillWeight = 120F;
            this.otbDocNo.HeaderText = "Document No.";
            this.otbDocNo.Name = "otbDocNo";
            this.otbDocNo.ReadOnly = true;
            // 
            // otbDocDate
            // 
            this.otbDocDate.HeaderText = "Date";
            this.otbDocDate.Name = "otbDocDate";
            this.otbDocDate.ReadOnly = true;
            // 
            // otbGrand
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.otbGrand.DefaultCellStyle = dataGridViewCellStyle3;
            this.otbGrand.FillWeight = 70F;
            this.otbGrand.HeaderText = "Grand";
            this.otbGrand.Name = "otbGrand";
            this.otbGrand.ReadOnly = true;
            // 
            // otbUsrKey
            // 
            this.otbUsrKey.HeaderText = "UserCode";
            this.otbUsrKey.Name = "otbUsrKey";
            this.otbUsrKey.ReadOnly = true;
            // 
            // otbUsrApv
            // 
            this.otbUsrApv.HeaderText = "Approved";
            this.otbUsrApv.Name = "otbUsrApv";
            this.otbUsrApv.ReadOnly = true;
            // 
            // otbCshOrCrd
            // 
            this.otbCshOrCrd.HeaderText = "Pay by CshOrCrd";
            this.otbCshOrCrd.Name = "otbCshOrCrd";
            this.otbCshOrCrd.ReadOnly = true;
            // 
            // wReferSO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1000, 554);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "wReferSO";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wSaleOrder";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.wReferSO_KeyDown);
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdSo)).EndInit();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitle;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.DataGridView ogdSo;
        private System.Windows.Forms.Label olaCstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbBchCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbDocNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbDocDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbGrand;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbUsrKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbUsrApv;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbCshOrCrd;
    }
}