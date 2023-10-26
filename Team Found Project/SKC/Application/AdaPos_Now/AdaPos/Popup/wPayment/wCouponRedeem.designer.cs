namespace AdaPos.Popup.wPayment
{
    partial class wCouponRedeem
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
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitle = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmShwKb = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.opnHeader = new System.Windows.Forms.Panel();
            this.olaCpnMinVal = new System.Windows.Forms.Label();
            this.olaTitleCpnMinVal = new System.Windows.Forms.Label();
            this.olaTitleCpnLimitPerBill = new System.Windows.Forms.Label();
            this.olaCpnLimitPerBill = new System.Windows.Forms.Label();
            this.olaCpnTime = new System.Windows.Forms.Label();
            this.olaCpnDate = new System.Windows.Forms.Label();
            this.olaCpnName = new System.Windows.Forms.Label();
            this.opnSearch = new System.Windows.Forms.Panel();
            this.ocmSearch = new System.Windows.Forms.Button();
            this.otbCouponNo = new System.Windows.Forms.TextBox();
            this.opnDetail = new System.Windows.Forms.Panel();
            this.ogdRedeemPdt = new System.Windows.Forms.DataGridView();
            this.FTBarCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FTPdtName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FTPunName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FNQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FCB4Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FCAfPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.opnPage = new System.Windows.Forms.Panel();
            this.ogbTop = new System.Windows.Forms.GroupBox();
            this.opnHD.SuspendLayout();
            this.opnDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdRedeemPdt)).BeginInit();
            this.opnPage.SuspendLayout();
            this.ogbTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnHD
            // 
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitle);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.ocmShwKb);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Dock = System.Windows.Forms.DockStyle.Top;
            this.opnHD.ForeColor = System.Drawing.Color.White;
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(785, 50);
            this.opnHD.TabIndex = 2;
            // 
            // olaTitle
            // 
            this.olaTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.olaTitle.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitle.ForeColor = System.Drawing.Color.White;
            this.olaTitle.Location = new System.Drawing.Point(50, 0);
            this.olaTitle.Name = "olaTitle";
            this.olaTitle.Size = new System.Drawing.Size(535, 50);
            this.olaTitle.TabIndex = 7;
            this.olaTitle.Text = "Redeem Coupon";
            this.olaTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ocmBack
            // 
            this.ocmBack.Dock = System.Windows.Forms.DockStyle.Left;
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
            this.ocmShwKb.Dock = System.Windows.Forms.DockStyle.Right;
            this.ocmShwKb.FlatAppearance.BorderSize = 0;
            this.ocmShwKb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmShwKb.Image = global::AdaPos.Properties.Resources.KBB_32;
            this.ocmShwKb.Location = new System.Drawing.Point(685, 0);
            this.ocmShwKb.Name = "ocmShwKb";
            this.ocmShwKb.Size = new System.Drawing.Size(50, 50);
            this.ocmShwKb.TabIndex = 6;
            this.ocmShwKb.UseVisualStyleBackColor = true;
            this.ocmShwKb.Click += new System.EventHandler(this.ocmShwKb_Click);
            // 
            // ocmAccept
            // 
            this.ocmAccept.Dock = System.Windows.Forms.DockStyle.Right;
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.Location = new System.Drawing.Point(735, 0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 5;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // opnHeader
            // 
            this.opnHeader.BackColor = System.Drawing.Color.White;
            this.opnHeader.ForeColor = System.Drawing.Color.White;
            this.opnHeader.Location = new System.Drawing.Point(777, 51);
            this.opnHeader.Margin = new System.Windows.Forms.Padding(0);
            this.opnHeader.Name = "opnHeader";
            this.opnHeader.Padding = new System.Windows.Forms.Padding(20);
            this.opnHeader.Size = new System.Drawing.Size(9, 118);
            this.opnHeader.TabIndex = 3;
            // 
            // olaCpnMinVal
            // 
            this.olaCpnMinVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaCpnMinVal.BackColor = System.Drawing.Color.Transparent;
            this.olaCpnMinVal.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaCpnMinVal.ForeColor = System.Drawing.Color.Black;
            this.olaCpnMinVal.Location = new System.Drawing.Point(626, 58);
            this.olaCpnMinVal.Name = "olaCpnMinVal";
            this.olaCpnMinVal.Size = new System.Drawing.Size(108, 24);
            this.olaCpnMinVal.TabIndex = 29;
            this.olaCpnMinVal.Text = "12345";
            this.olaCpnMinVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaTitleCpnMinVal
            // 
            this.olaTitleCpnMinVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleCpnMinVal.AutoSize = true;
            this.olaTitleCpnMinVal.BackColor = System.Drawing.Color.Transparent;
            this.olaTitleCpnMinVal.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaTitleCpnMinVal.ForeColor = System.Drawing.Color.Black;
            this.olaTitleCpnMinVal.Location = new System.Drawing.Point(472, 58);
            this.olaTitleCpnMinVal.Name = "olaTitleCpnMinVal";
            this.olaTitleCpnMinVal.Size = new System.Drawing.Size(122, 21);
            this.olaTitleCpnMinVal.TabIndex = 28;
            this.olaTitleCpnMinVal.Text = "Minimum Value";
            this.olaTitleCpnMinVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaTitleCpnLimitPerBill
            // 
            this.olaTitleCpnLimitPerBill.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleCpnLimitPerBill.AutoSize = true;
            this.olaTitleCpnLimitPerBill.BackColor = System.Drawing.Color.Transparent;
            this.olaTitleCpnLimitPerBill.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaTitleCpnLimitPerBill.ForeColor = System.Drawing.Color.Black;
            this.olaTitleCpnLimitPerBill.Location = new System.Drawing.Point(472, 25);
            this.olaTitleCpnLimitPerBill.Name = "olaTitleCpnLimitPerBill";
            this.olaTitleCpnLimitPerBill.Size = new System.Drawing.Size(101, 21);
            this.olaTitleCpnLimitPerBill.TabIndex = 27;
            this.olaTitleCpnLimitPerBill.Text = "Limit per Bill";
            this.olaTitleCpnLimitPerBill.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaCpnLimitPerBill
            // 
            this.olaCpnLimitPerBill.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaCpnLimitPerBill.BackColor = System.Drawing.Color.Transparent;
            this.olaCpnLimitPerBill.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaCpnLimitPerBill.ForeColor = System.Drawing.Color.Black;
            this.olaCpnLimitPerBill.Location = new System.Drawing.Point(626, 25);
            this.olaCpnLimitPerBill.Name = "olaCpnLimitPerBill";
            this.olaCpnLimitPerBill.Size = new System.Drawing.Size(108, 24);
            this.olaCpnLimitPerBill.TabIndex = 26;
            this.olaCpnLimitPerBill.Text = "12345";
            this.olaCpnLimitPerBill.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaCpnTime
            // 
            this.olaCpnTime.AutoSize = true;
            this.olaCpnTime.BackColor = System.Drawing.Color.Transparent;
            this.olaCpnTime.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaCpnTime.ForeColor = System.Drawing.Color.Black;
            this.olaCpnTime.Location = new System.Drawing.Point(201, 58);
            this.olaCpnTime.Name = "olaCpnTime";
            this.olaCpnTime.Size = new System.Drawing.Size(144, 21);
            this.olaCpnTime.TabIndex = 25;
            this.olaCpnTime.Text = "18:30:00 - 21:00:00";
            this.olaCpnTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // olaCpnDate
            // 
            this.olaCpnDate.AutoSize = true;
            this.olaCpnDate.BackColor = System.Drawing.Color.Transparent;
            this.olaCpnDate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaCpnDate.ForeColor = System.Drawing.Color.Black;
            this.olaCpnDate.Location = new System.Drawing.Point(14, 58);
            this.olaCpnDate.Name = "olaCpnDate";
            this.olaCpnDate.Size = new System.Drawing.Size(192, 21);
            this.olaCpnDate.TabIndex = 24;
            this.olaCpnDate.Text = "31/12/2020 - 31/12/2021";
            this.olaCpnDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // olaCpnName
            // 
            this.olaCpnName.AutoSize = true;
            this.olaCpnName.BackColor = System.Drawing.Color.Transparent;
            this.olaCpnName.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.olaCpnName.ForeColor = System.Drawing.Color.Black;
            this.olaCpnName.Location = new System.Drawing.Point(14, 21);
            this.olaCpnName.Name = "olaCpnName";
            this.olaCpnName.Size = new System.Drawing.Size(136, 25);
            this.olaCpnName.TabIndex = 23;
            this.olaCpnName.Text = "Coupon Name";
            this.olaCpnName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opnSearch
            // 
            this.opnSearch.Location = new System.Drawing.Point(655, 119);
            this.opnSearch.Name = "opnSearch";
            this.opnSearch.Padding = new System.Windows.Forms.Padding(20, 5, 20, 5);
            this.opnSearch.Size = new System.Drawing.Size(80, 32);
            this.opnSearch.TabIndex = 4;
            // 
            // ocmSearch
            // 
            this.ocmSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(171)))), ((int)(((byte)(132)))));
            this.ocmSearch.FlatAppearance.BorderSize = 0;
            this.ocmSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmSearch.Image = global::AdaPos.Properties.Resources.SearchW_32;
            this.ocmSearch.Location = new System.Drawing.Point(718, 164);
            this.ocmSearch.Name = "ocmSearch";
            this.ocmSearch.Size = new System.Drawing.Size(48, 30);
            this.ocmSearch.TabIndex = 26;
            this.ocmSearch.UseVisualStyleBackColor = false;
            this.ocmSearch.Click += new System.EventHandler(this.ocmSearch_Click);
            // 
            // otbCouponNo
            // 
            this.otbCouponNo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbCouponNo.Font = new System.Drawing.Font("Segoe UI Light", 12.5F);
            this.otbCouponNo.Location = new System.Drawing.Point(21, 164);
            this.otbCouponNo.MaxLength = 30;
            this.otbCouponNo.Name = "otbCouponNo";
            this.otbCouponNo.Size = new System.Drawing.Size(697, 30);
            this.otbCouponNo.TabIndex = 25;
            this.otbCouponNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbCouponNo_KeyDown);
            this.otbCouponNo.Leave += new System.EventHandler(this.otbCouponNo_Leave);
            // 
            // opnDetail
            // 
            this.opnDetail.Controls.Add(this.ogdRedeemPdt);
            this.opnDetail.Controls.Add(this.opnSearch);
            this.opnDetail.Location = new System.Drawing.Point(1, 199);
            this.opnDetail.Name = "opnDetail";
            this.opnDetail.Padding = new System.Windows.Forms.Padding(20, 5, 20, 5);
            this.opnDetail.Size = new System.Drawing.Size(785, 276);
            this.opnDetail.TabIndex = 5;
            // 
            // ogdRedeemPdt
            // 
            this.ogdRedeemPdt.AllowUserToAddRows = false;
            this.ogdRedeemPdt.AllowUserToDeleteRows = false;
            this.ogdRedeemPdt.AllowUserToResizeColumns = false;
            this.ogdRedeemPdt.AllowUserToResizeRows = false;
            this.ogdRedeemPdt.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ogdRedeemPdt.BackgroundColor = System.Drawing.Color.White;
            this.ogdRedeemPdt.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.ogdRedeemPdt.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdRedeemPdt.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ogdRedeemPdt.ColumnHeadersHeight = 40;
            this.ogdRedeemPdt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.ogdRedeemPdt.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FTBarCode,
            this.FTPdtName,
            this.FTPunName,
            this.FNQty,
            this.FCB4Price,
            this.FCAfPrice});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI Light", 10F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdRedeemPdt.DefaultCellStyle = dataGridViewCellStyle8;
            this.ogdRedeemPdt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ogdRedeemPdt.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.ogdRedeemPdt.EnableHeadersVisualStyles = false;
            this.ogdRedeemPdt.GridColor = System.Drawing.Color.Gainsboro;
            this.ogdRedeemPdt.Location = new System.Drawing.Point(20, 5);
            this.ogdRedeemPdt.MultiSelect = false;
            this.ogdRedeemPdt.Name = "ogdRedeemPdt";
            this.ogdRedeemPdt.ReadOnly = true;
            this.ogdRedeemPdt.RowHeadersVisible = false;
            this.ogdRedeemPdt.RowTemplate.Height = 40;
            this.ogdRedeemPdt.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ogdRedeemPdt.ShowCellErrors = false;
            this.ogdRedeemPdt.ShowCellToolTips = false;
            this.ogdRedeemPdt.ShowEditingIcon = false;
            this.ogdRedeemPdt.ShowRowErrors = false;
            this.ogdRedeemPdt.Size = new System.Drawing.Size(745, 266);
            this.ogdRedeemPdt.TabIndex = 0;
            this.ogdRedeemPdt.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ogdRedeemPdt_CellDoubleClick);
            // 
            // FTBarCode
            // 
            this.FTBarCode.DataPropertyName = "FTBarCode";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.FTBarCode.DefaultCellStyle = dataGridViewCellStyle2;
            this.FTBarCode.FillWeight = 45.61382F;
            this.FTBarCode.HeaderText = "Barcode";
            this.FTBarCode.Name = "FTBarCode";
            this.FTBarCode.ReadOnly = true;
            this.FTBarCode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FTBarCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FTPdtName
            // 
            this.FTPdtName.DataPropertyName = "FTPdtName";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.FTPdtName.DefaultCellStyle = dataGridViewCellStyle3;
            this.FTPdtName.FillWeight = 60.32465F;
            this.FTPdtName.HeaderText = "Product Name";
            this.FTPdtName.Name = "FTPdtName";
            this.FTPdtName.ReadOnly = true;
            this.FTPdtName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FTPdtName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FTPunName
            // 
            this.FTPunName.DataPropertyName = "FTPunName";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.FTPunName.DefaultCellStyle = dataGridViewCellStyle4;
            this.FTPunName.FillWeight = 32.33729F;
            this.FTPunName.HeaderText = "Unit";
            this.FTPunName.Name = "FTPunName";
            this.FTPunName.ReadOnly = true;
            this.FTPunName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FTPunName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FNQty
            // 
            this.FNQty.DataPropertyName = "FNQty";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.FNQty.DefaultCellStyle = dataGridViewCellStyle5;
            this.FNQty.FillWeight = 19.27831F;
            this.FNQty.HeaderText = "Qty";
            this.FNQty.Name = "FNQty";
            this.FNQty.ReadOnly = true;
            this.FNQty.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FNQty.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FCB4Price
            // 
            this.FCB4Price.DataPropertyName = "FCB4Price";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.FCB4Price.DefaultCellStyle = dataGridViewCellStyle6;
            this.FCB4Price.FillWeight = 32.33729F;
            this.FCB4Price.HeaderText = "B4Price";
            this.FCB4Price.Name = "FCB4Price";
            this.FCB4Price.ReadOnly = true;
            this.FCB4Price.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FCB4Price.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FCAfPrice
            // 
            this.FCAfPrice.DataPropertyName = "FCAfPrice";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.FCAfPrice.DefaultCellStyle = dataGridViewCellStyle7;
            this.FCAfPrice.FillWeight = 32.33729F;
            this.FCAfPrice.HeaderText = "AfPrice";
            this.FCAfPrice.Name = "FCAfPrice";
            this.FCAfPrice.ReadOnly = true;
            this.FCAfPrice.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FCAfPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.ocmSearch);
            this.opnPage.Controls.Add(this.ogbTop);
            this.opnPage.Controls.Add(this.opnDetail);
            this.opnPage.Controls.Add(this.otbCouponNo);
            this.opnPage.Controls.Add(this.opnHeader);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(118, 145);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(789, 478);
            this.opnPage.TabIndex = 8;
            // 
            // ogbTop
            // 
            this.ogbTop.Controls.Add(this.olaCpnMinVal);
            this.ogbTop.Controls.Add(this.olaCpnName);
            this.ogbTop.Controls.Add(this.olaTitleCpnMinVal);
            this.ogbTop.Controls.Add(this.olaCpnDate);
            this.ogbTop.Controls.Add(this.olaTitleCpnLimitPerBill);
            this.ogbTop.Controls.Add(this.olaCpnLimitPerBill);
            this.ogbTop.Controls.Add(this.olaCpnTime);
            this.ogbTop.Location = new System.Drawing.Point(21, 51);
            this.ogbTop.Margin = new System.Windows.Forms.Padding(2);
            this.ogbTop.Name = "ogbTop";
            this.ogbTop.Padding = new System.Windows.Forms.Padding(2);
            this.ogbTop.Size = new System.Drawing.Size(745, 107);
            this.ogbTop.TabIndex = 1;
            this.ogbTop.TabStop = false;
            // 
            // wCouponRedeem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wCouponRedeem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wRedeemCoupon";
            this.TransparencyKey = System.Drawing.SystemColors.ActiveBorder;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.wCouponRedeem_Shown);
            this.opnHD.ResumeLayout(false);
            this.opnDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ogdRedeemPdt)).EndInit();
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            this.ogbTop.ResumeLayout(false);
            this.ogbTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitle;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmShwKb;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Panel opnHeader;
        private System.Windows.Forms.Label olaCpnName;
        private System.Windows.Forms.Label olaCpnDate;
        private System.Windows.Forms.Label olaTitleCpnLimitPerBill;
        private System.Windows.Forms.Label olaCpnLimitPerBill;
        private System.Windows.Forms.Label olaCpnTime;
        private System.Windows.Forms.Label olaCpnMinVal;
        private System.Windows.Forms.Label olaTitleCpnMinVal;
        private System.Windows.Forms.Panel opnSearch;
        private System.Windows.Forms.TextBox otbCouponNo;
        private System.Windows.Forms.Panel opnDetail;
        private System.Windows.Forms.DataGridView ogdRedeemPdt;
        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Button ocmSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn FTBarCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn FTPdtName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FTPunName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FNQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn FCB4Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn FCAfPrice;
        private System.Windows.Forms.GroupBox ogbTop;
    }
}