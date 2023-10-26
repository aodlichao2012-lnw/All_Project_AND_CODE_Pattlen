namespace AdaPos.Popup.wSale
{
    partial class wEditSalePrice
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
            this.opnPage = new System.Windows.Forms.Panel();
            this.onpNumpad = new AdaPos.Control.uNumpad();
            this.otbNewPrice = new System.Windows.Forms.TextBox();
            this.otbOldPrice = new System.Windows.Forms.TextBox();
            this.olaPriceNew = new System.Windows.Forms.Label();
            this.olaPriceOld = new System.Windows.Forms.Label();
            this.otbPdtName = new System.Windows.Forms.TextBox();
            this.otbPdtCode = new System.Windows.Forms.TextBox();
            this.olaPdtName = new System.Windows.Forms.Label();
            this.olaPdtCode = new System.Windows.Forms.Label();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitle = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.opnPage.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.onpNumpad);
            this.opnPage.Controls.Add(this.otbNewPrice);
            this.opnPage.Controls.Add(this.otbOldPrice);
            this.opnPage.Controls.Add(this.olaPriceNew);
            this.opnPage.Controls.Add(this.olaPriceOld);
            this.opnPage.Controls.Add(this.otbPdtName);
            this.opnPage.Controls.Add(this.otbPdtCode);
            this.opnPage.Controls.Add(this.olaPdtName);
            this.opnPage.Controls.Add(this.olaPdtCode);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(1, 0);
            this.opnPage.Margin = new System.Windows.Forms.Padding(0);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(599, 349);
            this.opnPage.TabIndex = 0;
            // 
            // onpNumpad
            // 
            this.onpNumpad.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.onpNumpad.Location = new System.Drawing.Point(335, 94);
            this.onpNumpad.Margin = new System.Windows.Forms.Padding(0);
            this.onpNumpad.Name = "onpNumpad";
            this.onpNumpad.Size = new System.Drawing.Size(239, 210);
            this.onpNumpad.TabIndex = 12;
            // 
            // otbNewPrice
            // 
            this.otbNewPrice.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otbNewPrice.Location = new System.Drawing.Point(20, 279);
            this.otbNewPrice.Name = "otbNewPrice";
            this.otbNewPrice.Size = new System.Drawing.Size(280, 29);
            this.otbNewPrice.TabIndex = 11;
            this.otbNewPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // otbOldPrice
            // 
            this.otbOldPrice.Enabled = false;
            this.otbOldPrice.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otbOldPrice.Location = new System.Drawing.Point(21, 213);
            this.otbOldPrice.Name = "otbOldPrice";
            this.otbOldPrice.ReadOnly = true;
            this.otbOldPrice.Size = new System.Drawing.Size(279, 29);
            this.otbOldPrice.TabIndex = 10;
            this.otbOldPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // olaPriceNew
            // 
            this.olaPriceNew.AutoSize = true;
            this.olaPriceNew.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaPriceNew.Location = new System.Drawing.Point(18, 256);
            this.olaPriceNew.Name = "olaPriceNew";
            this.olaPriceNew.Size = new System.Drawing.Size(104, 21);
            this.olaPriceNew.TabIndex = 9;
            this.olaPriceNew.Text = "ราคาขายใหม่";
            // 
            // olaPriceOld
            // 
            this.olaPriceOld.AutoSize = true;
            this.olaPriceOld.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaPriceOld.Location = new System.Drawing.Point(18, 190);
            this.olaPriceOld.Name = "olaPriceOld";
            this.olaPriceOld.Size = new System.Drawing.Size(103, 21);
            this.olaPriceOld.TabIndex = 8;
            this.olaPriceOld.Text = "ราคาขายเดิม";
            // 
            // otbPdtName
            // 
            this.otbPdtName.Enabled = false;
            this.otbPdtName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otbPdtName.Location = new System.Drawing.Point(21, 153);
            this.otbPdtName.Name = "otbPdtName";
            this.otbPdtName.ReadOnly = true;
            this.otbPdtName.Size = new System.Drawing.Size(279, 29);
            this.otbPdtName.TabIndex = 7;
            // 
            // otbPdtCode
            // 
            this.otbPdtCode.Enabled = false;
            this.otbPdtCode.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otbPdtCode.Location = new System.Drawing.Point(20, 93);
            this.otbPdtCode.Name = "otbPdtCode";
            this.otbPdtCode.ReadOnly = true;
            this.otbPdtCode.Size = new System.Drawing.Size(280, 29);
            this.otbPdtCode.TabIndex = 6;
            // 
            // olaPdtName
            // 
            this.olaPdtName.AutoSize = true;
            this.olaPdtName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaPdtName.Location = new System.Drawing.Point(18, 130);
            this.olaPdtName.Name = "olaPdtName";
            this.olaPdtName.Size = new System.Drawing.Size(71, 21);
            this.olaPdtName.TabIndex = 5;
            this.olaPdtName.Text = "ชื่อสินค้า";
            // 
            // olaPdtCode
            // 
            this.olaPdtCode.AutoSize = true;
            this.olaPdtCode.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaPdtCode.Location = new System.Drawing.Point(17, 70);
            this.olaPdtCode.Name = "olaPdtCode";
            this.olaPdtCode.Size = new System.Drawing.Size(79, 21);
            this.olaPdtCode.TabIndex = 4;
            this.olaPdtCode.Text = "รหัสสินค้า";
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
            this.opnHD.Size = new System.Drawing.Size(595, 50);
            this.opnHD.TabIndex = 2;
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
            this.olaTitle.Size = new System.Drawing.Size(379, 50);
            this.olaTitle.TabIndex = 7;
            this.olaTitle.Text = "Edit Sale Price";
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
            this.ocmAccept.Location = new System.Drawing.Point(545, 0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 5;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // wEditSalePrice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 351);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wEditSalePrice";
            this.Padding = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wEditSalePrice";
            this.Shown += new System.EventHandler(this.wEditSalePrice_Shown);
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.TextBox otbNewPrice;
        private System.Windows.Forms.TextBox otbOldPrice;
        private System.Windows.Forms.Label olaPriceNew;
        private System.Windows.Forms.Label olaPriceOld;
        private System.Windows.Forms.TextBox otbPdtName;
        private System.Windows.Forms.TextBox otbPdtCode;
        private System.Windows.Forms.Label olaPdtName;
        private System.Windows.Forms.Label olaPdtCode;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitle;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmAccept;
        private Control.uNumpad onpNumpad;
    }
}