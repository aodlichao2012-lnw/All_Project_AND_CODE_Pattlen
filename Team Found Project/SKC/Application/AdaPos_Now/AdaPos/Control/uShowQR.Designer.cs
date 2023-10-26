namespace AdaPos.Control
{
    partial class uShowQR
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.opbHDPic = new System.Windows.Forms.PictureBox();
            this.opbQR = new System.Windows.Forms.PictureBox();
            this.opnPage = new System.Windows.Forms.TableLayoutPanel();
            this.otbDescript = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.opbHDPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.opbQR)).BeginInit();
            this.opnPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // opbHDPic
            // 
            this.opbHDPic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opbHDPic.Location = new System.Drawing.Point(0, 0);
            this.opbHDPic.Margin = new System.Windows.Forms.Padding(0);
            this.opbHDPic.Name = "opbHDPic";
            this.opbHDPic.Size = new System.Drawing.Size(356, 148);
            this.opbHDPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.opbHDPic.TabIndex = 2;
            this.opbHDPic.TabStop = false;
            // 
            // opbQR
            // 
            this.opbQR.Location = new System.Drawing.Point(0, 148);
            this.opbQR.Margin = new System.Windows.Forms.Padding(0);
            this.opbQR.Name = "opbQR";
            this.opbQR.Size = new System.Drawing.Size(208, 155);
            this.opbQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.opbQR.TabIndex = 3;
            this.opbQR.TabStop = false;
            // 
            // opnPage
            // 
            this.opnPage.ColumnCount = 1;
            this.opnPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opnPage.Controls.Add(this.opbQR, 0, 1);
            this.opnPage.Controls.Add(this.opbHDPic, 0, 0);
            this.opnPage.Controls.Add(this.otbDescript, 0, 2);
            this.opnPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnPage.Location = new System.Drawing.Point(0, 0);
            this.opnPage.Margin = new System.Windows.Forms.Padding(0);
            this.opnPage.Name = "opnPage";
            this.opnPage.RowCount = 3;
            this.opnPage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 76.08695F));
            this.opnPage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 204F));
            this.opnPage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.91304F));
            this.opnPage.Size = new System.Drawing.Size(356, 399);
            this.opnPage.TabIndex = 3;
            // 
            // otbDescript
            // 
            this.otbDescript.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.otbDescript.DetectUrls = false;
            this.otbDescript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.otbDescript.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.otbDescript.Location = new System.Drawing.Point(3, 355);
            this.otbDescript.Multiline = false;
            this.otbDescript.Name = "otbDescript";
            this.otbDescript.ReadOnly = true;
            this.otbDescript.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.otbDescript.Size = new System.Drawing.Size(350, 41);
            this.otbDescript.TabIndex = 4;
            this.otbDescript.Text = "Text Hear";
            // 
            // uShowQR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.opnPage);
            this.Name = "uShowQR";
            this.Size = new System.Drawing.Size(356, 399);
            this.Load += new System.EventHandler(this.uShowQR_Load);
            ((System.ComponentModel.ISupportInitialize)(this.opbHDPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.opbQR)).EndInit();
            this.opnPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox opbHDPic;
        private System.Windows.Forms.PictureBox opbQR;
        private System.Windows.Forms.TableLayoutPanel opnPage;
        private System.Windows.Forms.RichTextBox otbDescript;
    }
}
