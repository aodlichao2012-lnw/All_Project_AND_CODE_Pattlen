namespace AdaPos.Popup.wPayment
{
    partial class wListCoupon
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.opnPage = new System.Windows.Forms.Panel();
            this.ogdForm = new System.Windows.Forms.DataGridView();
            this.FTCpnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FTCpnMsg1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FTCpnMsg2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FDCpnDateFrm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FDCpnDateTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FTCpnCond = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FCCpnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbCouponNo = new System.Windows.Forms.TextBox();
            this.olaTitleNumber = new System.Windows.Forms.Label();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitleCoupon = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmShwKb = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.opnPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdForm)).BeginInit();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.ogdForm);
            this.opnPage.Controls.Add(this.otbCouponNo);
            this.opnPage.Controls.Add(this.olaTitleNumber);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(137, 184);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(2);
            this.opnPage.Size = new System.Drawing.Size(750, 400);
            this.opnPage.TabIndex = 0;
            // 
            // ogdForm
            // 
            this.ogdForm.AllowUserToAddRows = false;
            this.ogdForm.AllowUserToDeleteRows = false;
            this.ogdForm.AllowUserToResizeColumns = false;
            this.ogdForm.AllowUserToResizeRows = false;
            this.ogdForm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ogdForm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ogdForm.BackgroundColor = System.Drawing.Color.White;
            this.ogdForm.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdForm.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ogdForm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ogdForm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FTCpnName,
            this.FTCpnMsg1,
            this.FTCpnMsg2,
            this.FDCpnDateFrm,
            this.FDCpnDateTo,
            this.FTCpnCond,
            this.FCCpnValue});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdForm.DefaultCellStyle = dataGridViewCellStyle9;
            this.ogdForm.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.ogdForm.EnableHeadersVisualStyles = false;
            this.ogdForm.GridColor = System.Drawing.Color.Gainsboro;
            this.ogdForm.Location = new System.Drawing.Point(13, 126);
            this.ogdForm.Margin = new System.Windows.Forms.Padding(0);
            this.ogdForm.MultiSelect = false;
            this.ogdForm.Name = "ogdForm";
            this.ogdForm.ReadOnly = true;
            this.ogdForm.RowHeadersVisible = false;
            this.ogdForm.RowTemplate.Height = 40;
            this.ogdForm.RowTemplate.ReadOnly = true;
            this.ogdForm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ogdForm.ShowCellErrors = false;
            this.ogdForm.ShowCellToolTips = false;
            this.ogdForm.ShowEditingIcon = false;
            this.ogdForm.ShowRowErrors = false;
            this.ogdForm.Size = new System.Drawing.Size(724, 261);
            this.ogdForm.TabIndex = 26;
            // 
            // FTCpnName
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.FTCpnName.DefaultCellStyle = dataGridViewCellStyle2;
            this.FTCpnName.HeaderText = "Name";
            this.FTCpnName.Name = "FTCpnName";
            this.FTCpnName.ReadOnly = true;
            this.FTCpnName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FTCpnMsg1
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.FTCpnMsg1.DefaultCellStyle = dataGridViewCellStyle3;
            this.FTCpnMsg1.HeaderText = "Message1";
            this.FTCpnMsg1.Name = "FTCpnMsg1";
            this.FTCpnMsg1.ReadOnly = true;
            this.FTCpnMsg1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FTCpnMsg2
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.FTCpnMsg2.DefaultCellStyle = dataGridViewCellStyle4;
            this.FTCpnMsg2.HeaderText = "Message2";
            this.FTCpnMsg2.Name = "FTCpnMsg2";
            this.FTCpnMsg2.ReadOnly = true;
            this.FTCpnMsg2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FDCpnDateFrm
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.FDCpnDateFrm.DefaultCellStyle = dataGridViewCellStyle5;
            this.FDCpnDateFrm.HeaderText = "From Date";
            this.FDCpnDateFrm.Name = "FDCpnDateFrm";
            this.FDCpnDateFrm.ReadOnly = true;
            this.FDCpnDateFrm.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FDCpnDateTo
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.FDCpnDateTo.DefaultCellStyle = dataGridViewCellStyle6;
            this.FDCpnDateTo.HeaderText = "To Date";
            this.FDCpnDateTo.Name = "FDCpnDateTo";
            this.FDCpnDateTo.ReadOnly = true;
            this.FDCpnDateTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FTCpnCond
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.FTCpnCond.DefaultCellStyle = dataGridViewCellStyle7;
            this.FTCpnCond.HeaderText = "Condition";
            this.FTCpnCond.Name = "FTCpnCond";
            this.FTCpnCond.ReadOnly = true;
            this.FTCpnCond.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FCCpnValue
            // 
            this.FCCpnValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.FCCpnValue.DefaultCellStyle = dataGridViewCellStyle8;
            this.FCCpnValue.HeaderText = "Value";
            this.FCCpnValue.Name = "FCCpnValue";
            this.FCCpnValue.ReadOnly = true;
            this.FCCpnValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbCouponNo
            // 
            this.otbCouponNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbCouponNo.Enabled = false;
            this.otbCouponNo.Font = new System.Drawing.Font("Segoe UI Light", 15F);
            this.otbCouponNo.Location = new System.Drawing.Point(13, 77);
            this.otbCouponNo.MaxLength = 20;
            this.otbCouponNo.Name = "otbCouponNo";
            this.otbCouponNo.Size = new System.Drawing.Size(264, 34);
            this.otbCouponNo.TabIndex = 23;
            this.otbCouponNo.Text = "12345678901234567890";
            // 
            // olaTitleNumber
            // 
            this.olaTitleNumber.AutoSize = true;
            this.olaTitleNumber.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.olaTitleNumber.Location = new System.Drawing.Point(9, 55);
            this.olaTitleNumber.Name = "olaTitleNumber";
            this.olaTitleNumber.Size = new System.Drawing.Size(81, 19);
            this.olaTitleNumber.TabIndex = 24;
            this.olaTitleNumber.Text = "Coupon No";
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitleCoupon);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.ocmShwKb);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Location = new System.Drawing.Point(2, 2);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(744, 50);
            this.opnHD.TabIndex = 2;
            // 
            // olaTitleCoupon
            // 
            this.olaTitleCoupon.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleCoupon.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleCoupon.ForeColor = System.Drawing.Color.White;
            this.olaTitleCoupon.Location = new System.Drawing.Point(56, 0);
            this.olaTitleCoupon.Name = "olaTitleCoupon";
            this.olaTitleCoupon.Size = new System.Drawing.Size(528, 50);
            this.olaTitleCoupon.TabIndex = 7;
            this.olaTitleCoupon.Text = "Select Coupon";
            this.olaTitleCoupon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // ocmShwKb
            // 
            this.ocmShwKb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmShwKb.FlatAppearance.BorderSize = 0;
            this.ocmShwKb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmShwKb.Image = global::AdaPos.Properties.Resources.KBB_32;
            this.ocmShwKb.Location = new System.Drawing.Point(638, 0);
            this.ocmShwKb.Name = "ocmShwKb";
            this.ocmShwKb.Size = new System.Drawing.Size(50, 50);
            this.ocmShwKb.TabIndex = 6;
            this.ocmShwKb.UseVisualStyleBackColor = true;
            this.ocmShwKb.Click += new System.EventHandler(this.ocmShwKb_Click);
            // 
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.Location = new System.Drawing.Point(694, 0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 5;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // wListCoupon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wListCoupon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wListCoupon";
            this.TransparencyKey = System.Drawing.SystemColors.ActiveBorder;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wListCoupon_FormClosing);
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdForm)).EndInit();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitleCoupon;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmShwKb;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.TextBox otbCouponNo;
        private System.Windows.Forms.Label olaTitleNumber;
        private System.Windows.Forms.DataGridView ogdForm;
        private System.Windows.Forms.DataGridViewTextBoxColumn FTCpnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FTCpnMsg1;
        private System.Windows.Forms.DataGridViewTextBoxColumn FTCpnMsg2;
        private System.Windows.Forms.DataGridViewTextBoxColumn FDCpnDateFrm;
        private System.Windows.Forms.DataGridViewTextBoxColumn FDCpnDateTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn FTCpnCond;
        private System.Windows.Forms.DataGridViewTextBoxColumn FCCpnValue;
    }
}