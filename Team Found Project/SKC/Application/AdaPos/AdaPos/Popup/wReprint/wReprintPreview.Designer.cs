namespace AdaPos.Popup.wReprint
{
    partial class wReprintPreview
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
            this.opnPreview = new System.Windows.Forms.Panel();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitleDocNo = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmPrint = new System.Windows.Forms.Button();
            this.opnFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.opnPreview1 = new System.Windows.Forms.Panel();
            this.opnPage.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.opnFlowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.opnPreview);
            this.opnPage.Controls.Add(this.opnFlowLayoutPanel);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(416, 11);
            this.opnPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(533, 923);
            this.opnPage.TabIndex = 0;
            // 
            // opnPreview
            // 
            this.opnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnPreview.BackColor = System.Drawing.Color.White;
            this.opnPreview.Location = new System.Drawing.Point(22, 717);
            this.opnPreview.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.opnPreview.Name = "opnPreview";
            this.opnPreview.Size = new System.Drawing.Size(368, 10);
            this.opnPreview.TabIndex = 2;
            this.opnPreview.Visible = false;
            this.opnPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.opnPreview_Paint);
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitleDocNo);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.ocmPrint);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(528, 62);
            this.opnHD.TabIndex = 1;
            // 
            // olaTitleDocNo
            // 
            this.olaTitleDocNo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleDocNo.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleDocNo.ForeColor = System.Drawing.Color.White;
            this.olaTitleDocNo.Location = new System.Drawing.Point(75, 0);
            this.olaTitleDocNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaTitleDocNo.Name = "olaTitleDocNo";
            this.olaTitleDocNo.Size = new System.Drawing.Size(379, 62);
            this.olaTitleDocNo.TabIndex = 5;
            this.olaTitleDocNo.Text = "S1500001001-0000001";
            this.olaTitleDocNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ocmBack
            // 
            this.ocmBack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmBack.FlatAppearance.BorderSize = 0;
            this.ocmBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmBack.Image = global::AdaPos.Properties.Resources.BackW_32;
            this.ocmBack.Location = new System.Drawing.Point(0, 0);
            this.ocmBack.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ocmBack.Name = "ocmBack";
            this.ocmBack.Size = new System.Drawing.Size(67, 62);
            this.ocmBack.TabIndex = 3;
            this.ocmBack.UseVisualStyleBackColor = true;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // ocmPrint
            // 
            this.ocmPrint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmPrint.FlatAppearance.BorderSize = 0;
            this.ocmPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmPrint.Image = global::AdaPos.Properties.Resources.Print_32;
            this.ocmPrint.Location = new System.Drawing.Point(461, 0);
            this.ocmPrint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ocmPrint.Name = "ocmPrint";
            this.ocmPrint.Size = new System.Drawing.Size(67, 62);
            this.ocmPrint.TabIndex = 4;
            this.ocmPrint.UseVisualStyleBackColor = true;
            this.ocmPrint.Click += new System.EventHandler(this.ocmPrint_Click);
            // 
            // opnFlowLayoutPanel
            // 
            this.opnFlowLayoutPanel.AutoScroll = true;
            this.opnFlowLayoutPanel.BackColor = System.Drawing.Color.White;
            this.opnFlowLayoutPanel.Controls.Add(this.opnPreview1);
            this.opnFlowLayoutPanel.Location = new System.Drawing.Point(83, 70);
            this.opnFlowLayoutPanel.Name = "opnFlowLayoutPanel";
            this.opnFlowLayoutPanel.Size = new System.Drawing.Size(444, 847);
            this.opnFlowLayoutPanel.TabIndex = 4;
            // 
            // opnPreview1
            // 
            this.opnPreview1.BackColor = System.Drawing.Color.White;
            this.opnPreview1.Location = new System.Drawing.Point(3, 3);
            this.opnPreview1.Name = "opnPreview1";
            this.opnPreview1.Size = new System.Drawing.Size(368, 830);
            this.opnPreview1.TabIndex = 5;
            // 
            // wReprintPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1365, 945);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "wReprintPreview";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.wReprintPreview_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.wReprintPreview_KeyDown);
            this.opnPage.ResumeLayout(false);
            this.opnHD.ResumeLayout(false);
            this.opnFlowLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmPrint;
        private System.Windows.Forms.Label olaTitleDocNo;
        private System.Windows.Forms.Panel opnPreview;
        private System.Windows.Forms.FlowLayoutPanel opnFlowLayoutPanel;
        private System.Windows.Forms.Panel opnPreview1;
    }
}