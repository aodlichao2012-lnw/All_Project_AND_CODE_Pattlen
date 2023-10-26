namespace AdaPos.Popup.wPayment
{
    partial class wCouponDis
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
            this.olaAvailable = new System.Windows.Forms.Label();
            this.otbCouponNo = new System.Windows.Forms.TextBox();
            this.ocmSearch = new System.Windows.Forms.Button();
            this.olaTitleAval = new System.Windows.Forms.Label();
            this.olaTitle = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmShwKb = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.opnSearch = new System.Windows.Forms.Panel();
            this.opnHeader = new System.Windows.Forms.Panel();
            this.opnHD = new System.Windows.Forms.Panel();
            this.opnDetail = new System.Windows.Forms.TableLayoutPanel();
            this.opnCpnValue = new System.Windows.Forms.Panel();
            this.olaCpnVal = new System.Windows.Forms.Label();
            this.olaTitleValue = new System.Windows.Forms.Label();
            this.opnCpnDetail = new System.Windows.Forms.Panel();
            this.olaTitleCondition = new System.Windows.Forms.Label();
            this.olaTitleTime = new System.Windows.Forms.Label();
            this.olaTitleDate = new System.Windows.Forms.Label();
            this.olaCpnCondition = new System.Windows.Forms.Label();
            this.olaCpnTime = new System.Windows.Forms.Label();
            this.olaCpnDate = new System.Windows.Forms.Label();
            this.olaCpnName = new System.Windows.Forms.Label();
            this.opnPage = new System.Windows.Forms.Panel();
            this.opnSearch.SuspendLayout();
            this.opnHeader.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.opnDetail.SuspendLayout();
            this.opnCpnValue.SuspendLayout();
            this.opnCpnDetail.SuspendLayout();
            this.opnPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // olaAvailable
            // 
            this.olaAvailable.Dock = System.Windows.Forms.DockStyle.Right;
            this.olaAvailable.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.olaAvailable.Location = new System.Drawing.Point(545, 20);
            this.olaAvailable.Name = "olaAvailable";
            this.olaAvailable.Size = new System.Drawing.Size(231, 58);
            this.olaAvailable.TabIndex = 26;
            this.olaAvailable.Text = "12345.67";
            this.olaAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // otbCouponNo
            // 
            this.otbCouponNo.AccessibleDescription = "";
            this.otbCouponNo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbCouponNo.Font = new System.Drawing.Font("Segoe UI Light", 12.5F);
            this.otbCouponNo.Location = new System.Drawing.Point(20, 5);
            this.otbCouponNo.MaxLength = 30;
            this.otbCouponNo.Name = "otbCouponNo";
            this.otbCouponNo.Size = new System.Drawing.Size(720, 30);
            this.otbCouponNo.TabIndex = 0;
            this.otbCouponNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbCouponNo_KeyDown);
            this.otbCouponNo.Leave += new System.EventHandler(this.otbCouponNo_Leave);
            // 
            // ocmSearch
            // 
            this.ocmSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(171)))), ((int)(((byte)(132)))));
            this.ocmSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.ocmSearch.FlatAppearance.BorderSize = 0;
            this.ocmSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmSearch.Image = global::AdaPos.Properties.Resources.SearchW_32;
            this.ocmSearch.Location = new System.Drawing.Point(746, 5);
            this.ocmSearch.Name = "ocmSearch";
            this.ocmSearch.Size = new System.Drawing.Size(30, 30);
            this.ocmSearch.TabIndex = 1;
            this.ocmSearch.UseVisualStyleBackColor = false;
            this.ocmSearch.Click += new System.EventHandler(this.ocmSearch_Click);
            // 
            // olaTitleAval
            // 
            this.olaTitleAval.Dock = System.Windows.Forms.DockStyle.Left;
            this.olaTitleAval.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleAval.Location = new System.Drawing.Point(20, 20);
            this.olaTitleAval.Name = "olaTitleAval";
            this.olaTitleAval.Size = new System.Drawing.Size(374, 58);
            this.olaTitleAval.TabIndex = 23;
            this.olaTitleAval.Text = "Available Discount";
            this.olaTitleAval.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.olaTitle.Text = "Coupon Discount";
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
            this.ocmBack.TabIndex = 2;
            this.ocmBack.UseVisualStyleBackColor = true;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // ocmShwKb
            // 
            this.ocmShwKb.Dock = System.Windows.Forms.DockStyle.Right;
            this.ocmShwKb.FlatAppearance.BorderSize = 0;
            this.ocmShwKb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmShwKb.Image = global::AdaPos.Properties.Resources.KBB_32;
            this.ocmShwKb.Location = new System.Drawing.Point(696, 0);
            this.ocmShwKb.Name = "ocmShwKb";
            this.ocmShwKb.Size = new System.Drawing.Size(50, 50);
            this.ocmShwKb.TabIndex = 1;
            this.ocmShwKb.UseVisualStyleBackColor = true;
            this.ocmShwKb.Click += new System.EventHandler(this.ocmShwKb_Click);
            // 
            // ocmAccept
            // 
            this.ocmAccept.Dock = System.Windows.Forms.DockStyle.Right;
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.Location = new System.Drawing.Point(746, 0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 0;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // opnSearch
            // 
            this.opnSearch.Controls.Add(this.otbCouponNo);
            this.opnSearch.Controls.Add(this.ocmSearch);
            this.opnSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.opnSearch.Location = new System.Drawing.Point(1, 149);
            this.opnSearch.Name = "opnSearch";
            this.opnSearch.Padding = new System.Windows.Forms.Padding(20, 5, 20, 5);
            this.opnSearch.Size = new System.Drawing.Size(796, 40);
            this.opnSearch.TabIndex = 8;
            // 
            // opnHeader
            // 
            this.opnHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(171)))), ((int)(((byte)(132)))));
            this.opnHeader.Controls.Add(this.olaTitleAval);
            this.opnHeader.Controls.Add(this.olaAvailable);
            this.opnHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.opnHeader.ForeColor = System.Drawing.Color.White;
            this.opnHeader.Location = new System.Drawing.Point(1, 51);
            this.opnHeader.Margin = new System.Windows.Forms.Padding(0);
            this.opnHeader.Name = "opnHeader";
            this.opnHeader.Padding = new System.Windows.Forms.Padding(20);
            this.opnHeader.Size = new System.Drawing.Size(796, 98);
            this.opnHeader.TabIndex = 7;
            // 
            // opnHD
            // 
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.ocmShwKb);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Controls.Add(this.olaTitle);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Dock = System.Windows.Forms.DockStyle.Top;
            this.opnHD.ForeColor = System.Drawing.Color.White;
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(796, 50);
            this.opnHD.TabIndex = 6;
            // 
            // opnDetail
            // 
            this.opnDetail.ColumnCount = 2;
            this.opnDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opnDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opnDetail.Controls.Add(this.opnCpnValue, 1, 0);
            this.opnDetail.Controls.Add(this.opnCpnDetail, 0, 0);
            this.opnDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnDetail.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.opnDetail.Location = new System.Drawing.Point(1, 189);
            this.opnDetail.Name = "opnDetail";
            this.opnDetail.Padding = new System.Windows.Forms.Padding(20, 0, 20, 20);
            this.opnDetail.RowCount = 1;
            this.opnDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opnDetail.Size = new System.Drawing.Size(796, 258);
            this.opnDetail.TabIndex = 9;
            // 
            // opnCpnValue
            // 
            this.opnCpnValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnCpnValue.Controls.Add(this.olaCpnVal);
            this.opnCpnValue.Controls.Add(this.olaTitleValue);
            this.opnCpnValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnCpnValue.Location = new System.Drawing.Point(403, 0);
            this.opnCpnValue.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.opnCpnValue.Name = "opnCpnValue";
            this.opnCpnValue.Padding = new System.Windows.Forms.Padding(5);
            this.opnCpnValue.Size = new System.Drawing.Size(373, 238);
            this.opnCpnValue.TabIndex = 1;
            // 
            // olaCpnVal
            // 
            this.olaCpnVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaCpnVal.Font = new System.Drawing.Font("Segoe UI Semibold", 36F, System.Drawing.FontStyle.Bold);
            this.olaCpnVal.Location = new System.Drawing.Point(-1, 101);
            this.olaCpnVal.Margin = new System.Windows.Forms.Padding(5);
            this.olaCpnVal.Name = "olaCpnVal";
            this.olaCpnVal.Size = new System.Drawing.Size(361, 83);
            this.olaCpnVal.TabIndex = 28;
            this.olaCpnVal.Text = "50.00";
            this.olaCpnVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaTitleValue
            // 
            this.olaTitleValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.olaTitleValue.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.olaTitleValue.Location = new System.Drawing.Point(5, 5);
            this.olaTitleValue.Margin = new System.Windows.Forms.Padding(5);
            this.olaTitleValue.Name = "olaTitleValue";
            this.olaTitleValue.Size = new System.Drawing.Size(361, 73);
            this.olaTitleValue.TabIndex = 27;
            this.olaTitleValue.Text = "Coupon Value";
            this.olaTitleValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opnCpnDetail
            // 
            this.opnCpnDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnCpnDetail.Controls.Add(this.olaTitleCondition);
            this.opnCpnDetail.Controls.Add(this.olaTitleTime);
            this.opnCpnDetail.Controls.Add(this.olaTitleDate);
            this.opnCpnDetail.Controls.Add(this.olaCpnCondition);
            this.opnCpnDetail.Controls.Add(this.olaCpnTime);
            this.opnCpnDetail.Controls.Add(this.olaCpnDate);
            this.opnCpnDetail.Controls.Add(this.olaCpnName);
            this.opnCpnDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnCpnDetail.Location = new System.Drawing.Point(20, 0);
            this.opnCpnDetail.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.opnCpnDetail.Name = "opnCpnDetail";
            this.opnCpnDetail.Padding = new System.Windows.Forms.Padding(5);
            this.opnCpnDetail.Size = new System.Drawing.Size(373, 238);
            this.opnCpnDetail.TabIndex = 0;
            // 
            // olaTitleCondition
            // 
            this.olaTitleCondition.AutoSize = true;
            this.olaTitleCondition.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.olaTitleCondition.Location = new System.Drawing.Point(10, 145);
            this.olaTitleCondition.Margin = new System.Windows.Forms.Padding(5);
            this.olaTitleCondition.Name = "olaTitleCondition";
            this.olaTitleCondition.Size = new System.Drawing.Size(82, 21);
            this.olaTitleCondition.TabIndex = 33;
            this.olaTitleCondition.Text = "Condition";
            this.olaTitleCondition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaTitleTime
            // 
            this.olaTitleTime.AutoSize = true;
            this.olaTitleTime.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.olaTitleTime.Location = new System.Drawing.Point(10, 114);
            this.olaTitleTime.Margin = new System.Windows.Forms.Padding(5);
            this.olaTitleTime.Name = "olaTitleTime";
            this.olaTitleTime.Size = new System.Drawing.Size(116, 21);
            this.olaTitleTime.TabIndex = 32;
            this.olaTitleTime.Text = "Time Avaliable";
            this.olaTitleTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaTitleDate
            // 
            this.olaTitleDate.AutoSize = true;
            this.olaTitleDate.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.olaTitleDate.Location = new System.Drawing.Point(10, 83);
            this.olaTitleDate.Margin = new System.Windows.Forms.Padding(5);
            this.olaTitleDate.Name = "olaTitleDate";
            this.olaTitleDate.Size = new System.Drawing.Size(114, 21);
            this.olaTitleDate.TabIndex = 31;
            this.olaTitleDate.Text = "Date Avaliable";
            this.olaTitleDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaCpnCondition
            // 
            this.olaCpnCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.olaCpnCondition.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.olaCpnCondition.Location = new System.Drawing.Point(133, 145);
            this.olaCpnCondition.Margin = new System.Windows.Forms.Padding(5);
            this.olaCpnCondition.Name = "olaCpnCondition";
            this.olaCpnCondition.Size = new System.Drawing.Size(228, 21);
            this.olaCpnCondition.TabIndex = 30;
            this.olaCpnCondition.Text = "Discount Amt  (50.00)";
            this.olaCpnCondition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaCpnTime
            // 
            this.olaCpnTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.olaCpnTime.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.olaCpnTime.Location = new System.Drawing.Point(133, 114);
            this.olaCpnTime.Margin = new System.Windows.Forms.Padding(5);
            this.olaCpnTime.Name = "olaCpnTime";
            this.olaCpnTime.Size = new System.Drawing.Size(228, 21);
            this.olaCpnTime.TabIndex = 28;
            this.olaCpnTime.Text = "18:30:00 - 21:00:00";
            this.olaCpnTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaCpnDate
            // 
            this.olaCpnDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.olaCpnDate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.olaCpnDate.Location = new System.Drawing.Point(137, 83);
            this.olaCpnDate.Margin = new System.Windows.Forms.Padding(5);
            this.olaCpnDate.Name = "olaCpnDate";
            this.olaCpnDate.Size = new System.Drawing.Size(224, 21);
            this.olaCpnDate.TabIndex = 27;
            this.olaCpnDate.Text = "31/12/2020 - 31/12/2021";
            this.olaCpnDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaCpnName
            // 
            this.olaCpnName.Dock = System.Windows.Forms.DockStyle.Top;
            this.olaCpnName.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.olaCpnName.Location = new System.Drawing.Point(5, 5);
            this.olaCpnName.Name = "olaCpnName";
            this.olaCpnName.Size = new System.Drawing.Size(361, 73);
            this.olaCpnName.TabIndex = 26;
            this.olaCpnName.Text = "Coupon Name";
            this.olaCpnName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // opnPage
            // 
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.opnDetail);
            this.opnPage.Controls.Add(this.opnSearch);
            this.opnPage.Controls.Add(this.opnHeader);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnPage.Location = new System.Drawing.Point(0, 0);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(800, 450);
            this.opnPage.TabIndex = 10;
            // 
            // wCouponDis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wCouponDis";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wCouponDis";
            this.Shown += new System.EventHandler(this.wCouponDis_Shown);
            this.opnSearch.ResumeLayout(false);
            this.opnSearch.PerformLayout();
            this.opnHeader.ResumeLayout(false);
            this.opnHD.ResumeLayout(false);
            this.opnDetail.ResumeLayout(false);
            this.opnCpnValue.ResumeLayout(false);
            this.opnCpnDetail.ResumeLayout(false);
            this.opnCpnDetail.PerformLayout();
            this.opnPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label olaAvailable;
        private System.Windows.Forms.TextBox otbCouponNo;
        private System.Windows.Forms.Button ocmSearch;
        private System.Windows.Forms.Label olaTitleAval;
        private System.Windows.Forms.Label olaTitle;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmShwKb;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Panel opnSearch;
        private System.Windows.Forms.Panel opnHeader;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.TableLayoutPanel opnDetail;
        private System.Windows.Forms.Panel opnCpnValue;
        private System.Windows.Forms.Panel opnCpnDetail;
        private System.Windows.Forms.Label olaCpnVal;
        private System.Windows.Forms.Label olaTitleValue;
        private System.Windows.Forms.Label olaTitleCondition;
        private System.Windows.Forms.Label olaTitleTime;
        private System.Windows.Forms.Label olaTitleDate;
        private System.Windows.Forms.Label olaCpnCondition;
        private System.Windows.Forms.Label olaCpnTime;
        private System.Windows.Forms.Label olaCpnDate;
        private System.Windows.Forms.Label olaCpnName;
        private System.Windows.Forms.Panel opnPage;
    }
}