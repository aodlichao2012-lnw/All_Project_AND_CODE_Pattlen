namespace AdaPos.Popup.wTaxInvoice
{
    partial class wTaxInvoicePreview
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
            this.opnFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.opnContent = new System.Windows.Forms.Panel();
            this.opnContentX = new System.Windows.Forms.Panel();
            this.opnHD = new System.Windows.Forms.Panel();
            this.ocmPrint = new System.Windows.Forms.Button();
            this.olaTitlePreview = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.opnPage.SuspendLayout();
            this.opnFlowLayoutPanel.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.opnFlowLayoutPanel);
            this.opnPage.Controls.Add(this.opnContentX);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(469, 11);
            this.opnPage.Margin = new System.Windows.Forms.Padding(4);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(426, 923);
            this.opnPage.TabIndex = 2;
            // 
            // opnFlowLayoutPanel
            // 
            this.opnFlowLayoutPanel.AutoScroll = true;
            this.opnFlowLayoutPanel.Controls.Add(this.opnContent);
            this.opnFlowLayoutPanel.Location = new System.Drawing.Point(18, 70);
            this.opnFlowLayoutPanel.Name = "opnFlowLayoutPanel";
            this.opnFlowLayoutPanel.Size = new System.Drawing.Size(402, 847);
            this.opnFlowLayoutPanel.TabIndex = 3;
            this.opnFlowLayoutPanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.opnFlowLayoutPanel_Scroll);
            // 
            // opnContent
            // 
            this.opnContent.BackColor = System.Drawing.Color.White;
            this.opnContent.Location = new System.Drawing.Point(3, 3);
            this.opnContent.Name = "opnContent";
            this.opnContent.Size = new System.Drawing.Size(368, 830);
            this.opnContent.TabIndex = 0;
            // 
            // opnContentX
            // 
            this.opnContentX.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnContentX.BackColor = System.Drawing.Color.White;
            this.opnContentX.Location = new System.Drawing.Point(28, 75);
            this.opnContentX.Margin = new System.Windows.Forms.Padding(4);
            this.opnContentX.Name = "opnContentX";
            this.opnContentX.Size = new System.Drawing.Size(368, 832);
            this.opnContentX.TabIndex = 2;
            this.opnContentX.Paint += new System.Windows.Forms.PaintEventHandler(this.opnContent_Paint);
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.ocmPrint);
            this.opnHD.Controls.Add(this.olaTitlePreview);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Margin = new System.Windows.Forms.Padding(4);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(421, 62);
            this.opnHD.TabIndex = 1;
            // 
            // ocmPrint
            // 
            this.ocmPrint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmPrint.FlatAppearance.BorderSize = 0;
            this.ocmPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmPrint.Image = global::AdaPos.Properties.Resources.Print_32;
            this.ocmPrint.Location = new System.Drawing.Point(355, 0);
            this.ocmPrint.Margin = new System.Windows.Forms.Padding(4);
            this.ocmPrint.Name = "ocmPrint";
            this.ocmPrint.Size = new System.Drawing.Size(67, 62);
            this.ocmPrint.TabIndex = 2;
            this.ocmPrint.UseVisualStyleBackColor = true;
            this.ocmPrint.Click += new System.EventHandler(this.ocmPrint_Click);
            // 
            // olaTitlePreview
            // 
            this.olaTitlePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitlePreview.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitlePreview.ForeColor = System.Drawing.Color.White;
            this.olaTitlePreview.Location = new System.Drawing.Point(75, 0);
            this.olaTitlePreview.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaTitlePreview.Name = "olaTitlePreview";
            this.olaTitlePreview.Size = new System.Drawing.Size(253, 62);
            this.olaTitlePreview.TabIndex = 1;
            this.olaTitlePreview.Text = "Print Preview";
            this.olaTitlePreview.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ocmBack
            // 
            this.ocmBack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmBack.FlatAppearance.BorderSize = 0;
            this.ocmBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmBack.Image = global::AdaPos.Properties.Resources.BackW_32;
            this.ocmBack.Location = new System.Drawing.Point(0, 0);
            this.ocmBack.Margin = new System.Windows.Forms.Padding(4);
            this.ocmBack.Name = "ocmBack";
            this.ocmBack.Size = new System.Drawing.Size(67, 62);
            this.ocmBack.TabIndex = 0;
            this.ocmBack.UseVisualStyleBackColor = true;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // wTaxInvoicePreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1365, 945);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "wTaxInvoicePreview";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.opnPage.ResumeLayout(false);
            this.opnFlowLayoutPanel.ResumeLayout(false);
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Button ocmPrint;
        private System.Windows.Forms.Label olaTitlePreview;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Panel opnContentX;
        private System.Windows.Forms.FlowLayoutPanel opnFlowLayoutPanel;
        private System.Windows.Forms.Panel opnContent;
    }
}