namespace AdaPos.Popup.wSale
{
    partial class wChangePdtQty
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
            this.otbLimitQty = new System.Windows.Forms.TextBox();
            this.olaLimitQty = new System.Windows.Forms.Label();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitle = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.otbQtyNew = new System.Windows.Forms.TextBox();
            this.otbQtyOld = new System.Windows.Forms.TextBox();
            this.olaQtyNew = new System.Windows.Forms.Label();
            this.olaQtyOld = new System.Windows.Forms.Label();
            this.onpNumpad = new AdaPos.Control.uNumpad();
            this.opnPage.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.otbLimitQty);
            this.opnPage.Controls.Add(this.olaLimitQty);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Controls.Add(this.otbQtyNew);
            this.opnPage.Controls.Add(this.otbQtyOld);
            this.opnPage.Controls.Add(this.olaQtyNew);
            this.opnPage.Controls.Add(this.olaQtyOld);
            this.opnPage.Controls.Add(this.onpNumpad);
            this.opnPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnPage.Location = new System.Drawing.Point(0, 0);
            this.opnPage.Margin = new System.Windows.Forms.Padding(0);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(600, 320);
            this.opnPage.TabIndex = 0;
            // 
            // otbLimitQty
            // 
            this.otbLimitQty.Enabled = false;
            this.otbLimitQty.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otbLimitQty.Location = new System.Drawing.Point(30, 228);
            this.otbLimitQty.Name = "otbLimitQty";
            this.otbLimitQty.ReadOnly = true;
            this.otbLimitQty.Size = new System.Drawing.Size(279, 29);
            this.otbLimitQty.TabIndex = 19;
            this.otbLimitQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // olaLimitQty
            // 
            this.olaLimitQty.AutoSize = true;
            this.olaLimitQty.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaLimitQty.Location = new System.Drawing.Point(27, 204);
            this.olaLimitQty.Name = "olaLimitQty";
            this.olaLimitQty.Size = new System.Drawing.Size(84, 21);
            this.olaLimitQty.TabIndex = 18;
            this.olaLimitQty.Text = "จำนวนเดิม";
            // 
            // opnHD
            // 
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitle);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Dock = System.Windows.Forms.DockStyle.Top;
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Margin = new System.Windows.Forms.Padding(0);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(596, 50);
            this.opnHD.TabIndex = 3;
            // 
            // olaTitle
            // 
            this.olaTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.olaTitle.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitle.ForeColor = System.Drawing.Color.White;
            this.olaTitle.Location = new System.Drawing.Point(50, 0);
            this.olaTitle.Name = "olaTitle";
            this.olaTitle.Size = new System.Drawing.Size(384, 50);
            this.olaTitle.TabIndex = 7;
            this.olaTitle.Text = "Change Pdt Qty";
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
            // ocmAccept
            // 
            this.ocmAccept.Dock = System.Windows.Forms.DockStyle.Right;
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.Location = new System.Drawing.Point(546, 0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 5;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // otbQtyNew
            // 
            this.otbQtyNew.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otbQtyNew.Location = new System.Drawing.Point(28, 161);
            this.otbQtyNew.Name = "otbQtyNew";
            this.otbQtyNew.Size = new System.Drawing.Size(280, 29);
            this.otbQtyNew.TabIndex = 17;
            this.otbQtyNew.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // otbQtyOld
            // 
            this.otbQtyOld.Enabled = false;
            this.otbQtyOld.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otbQtyOld.Location = new System.Drawing.Point(29, 95);
            this.otbQtyOld.Name = "otbQtyOld";
            this.otbQtyOld.ReadOnly = true;
            this.otbQtyOld.Size = new System.Drawing.Size(279, 29);
            this.otbQtyOld.TabIndex = 16;
            this.otbQtyOld.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // olaQtyNew
            // 
            this.olaQtyNew.AutoSize = true;
            this.olaQtyNew.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaQtyNew.Location = new System.Drawing.Point(26, 138);
            this.olaQtyNew.Name = "olaQtyNew";
            this.olaQtyNew.Size = new System.Drawing.Size(85, 21);
            this.olaQtyNew.TabIndex = 15;
            this.olaQtyNew.Text = "จำนวนใหม่";
            // 
            // olaQtyOld
            // 
            this.olaQtyOld.AutoSize = true;
            this.olaQtyOld.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaQtyOld.Location = new System.Drawing.Point(26, 72);
            this.olaQtyOld.Name = "olaQtyOld";
            this.olaQtyOld.Size = new System.Drawing.Size(84, 21);
            this.olaQtyOld.TabIndex = 14;
            this.olaQtyOld.Text = "จำนวนเดิม";
            // 
            // onpNumpad
            // 
            this.onpNumpad.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.onpNumpad.Location = new System.Drawing.Point(331, 72);
            this.onpNumpad.Margin = new System.Windows.Forms.Padding(0);
            this.onpNumpad.Name = "onpNumpad";
            this.onpNumpad.Size = new System.Drawing.Size(240, 227);
            this.onpNumpad.TabIndex = 13;
            // 
            // wChangePdtQty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 320);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wChangePdtQty";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wChangPdtQty";
            this.Shown += new System.EventHandler(this.wChangePdtQty_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.wChangePdtQty_KeyDown);
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitle;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmAccept;
        private Control.uNumpad onpNumpad;
        private System.Windows.Forms.TextBox otbQtyNew;
        private System.Windows.Forms.TextBox otbQtyOld;
        private System.Windows.Forms.Label olaQtyNew;
        private System.Windows.Forms.Label olaQtyOld;
        private System.Windows.Forms.TextBox otbLimitQty;
        private System.Windows.Forms.Label olaLimitQty;
    }
}