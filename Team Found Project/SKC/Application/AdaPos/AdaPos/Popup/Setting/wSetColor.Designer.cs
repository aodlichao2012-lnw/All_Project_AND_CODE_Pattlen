namespace AdaPos.Popup.Setting
{
    partial class wSetColor
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
            this.opnExample = new System.Windows.Forms.Panel();
            this.ocmClrDialog = new System.Windows.Forms.Button();
            this.olaCode = new System.Windows.Forms.Label();
            this.opnHD = new System.Windows.Forms.Panel();
            this.ocmShwKb = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.ocmBack = new System.Windows.Forms.Button();
            this.olaTitle = new System.Windows.Forms.Label();
            this.otbCode = new System.Windows.Forms.TextBox();
            this.odgClrDialog = new System.Windows.Forms.ColorDialog();
            this.opnPage.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.Controls.Add(this.opnExample);
            this.opnPage.Controls.Add(this.ocmClrDialog);
            this.opnPage.Controls.Add(this.olaCode);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Controls.Add(this.otbCode);
            this.opnPage.Location = new System.Drawing.Point(1, 1);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(357, 180);
            this.opnPage.TabIndex = 0;
            // 
            // opnExample
            // 
            this.opnExample.BackColor = System.Drawing.Color.Gainsboro;
            this.opnExample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnExample.Location = new System.Drawing.Point(211, 63);
            this.opnExample.Name = "opnExample";
            this.opnExample.Size = new System.Drawing.Size(119, 103);
            this.opnExample.TabIndex = 7;
            // 
            // ocmClrDialog
            // 
            this.ocmClrDialog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmClrDialog.BackColor = System.Drawing.Color.Silver;
            this.ocmClrDialog.FlatAppearance.BorderSize = 0;
            this.ocmClrDialog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmClrDialog.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.ocmClrDialog.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ocmClrDialog.Location = new System.Drawing.Point(27, 132);
            this.ocmClrDialog.Name = "ocmClrDialog";
            this.ocmClrDialog.Size = new System.Drawing.Size(169, 34);
            this.ocmClrDialog.TabIndex = 6;
            this.ocmClrDialog.Text = "Color...";
            this.ocmClrDialog.UseVisualStyleBackColor = false;
            this.ocmClrDialog.Click += new System.EventHandler(this.ocmClrDialog_Click);
            // 
            // olaCode
            // 
            this.olaCode.AutoSize = true;
            this.olaCode.Font = new System.Drawing.Font("Segoe UI Historic", 12F, System.Drawing.FontStyle.Bold);
            this.olaCode.Location = new System.Drawing.Point(13, 63);
            this.olaCode.Name = "olaCode";
            this.olaCode.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.olaCode.Size = new System.Drawing.Size(117, 21);
            this.olaCode.TabIndex = 5;
            this.olaCode.Text = "Color Code :";
            this.olaCode.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.ocmShwKb);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.olaTitle);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(355, 50);
            this.opnHD.TabIndex = 4;
            // 
            // ocmShwKb
            // 
            this.ocmShwKb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmShwKb.FlatAppearance.BorderSize = 0;
            this.ocmShwKb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmShwKb.Image = global::AdaPos.Properties.Resources.KBB_32;
            this.ocmShwKb.Location = new System.Drawing.Point(249, 0);
            this.ocmShwKb.Name = "ocmShwKb";
            this.ocmShwKb.Size = new System.Drawing.Size(50, 50);
            this.ocmShwKb.TabIndex = 7;
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
            this.ocmAccept.Location = new System.Drawing.Point(305, 0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 6;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
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
            this.ocmBack.TabIndex = 3;
            this.ocmBack.UseVisualStyleBackColor = true;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // olaTitle
            // 
            this.olaTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitle.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitle.ForeColor = System.Drawing.Color.White;
            this.olaTitle.Location = new System.Drawing.Point(59, 0);
            this.olaTitle.Name = "olaTitle";
            this.olaTitle.Size = new System.Drawing.Size(240, 50);
            this.olaTitle.TabIndex = 2;
            this.olaTitle.Text = "Set Corlor";
            this.olaTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // otbCode
            // 
            this.otbCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.otbCode.Location = new System.Drawing.Point(27, 87);
            this.otbCode.MaxLength = 7;
            this.otbCode.Name = "otbCode";
            this.otbCode.Size = new System.Drawing.Size(169, 29);
            this.otbCode.TabIndex = 0;
            this.otbCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbCode_KeyDown);
            // 
            // wSetColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(360, 183);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wSetColor";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wSetColor";
            this.opnPage.ResumeLayout(false);
            this.opnPage.PerformLayout();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.TextBox otbCode;
        private System.Windows.Forms.ColorDialog odgClrDialog;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitle;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Label olaCode;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Button ocmClrDialog;
        private System.Windows.Forms.Button ocmShwKb;
        private System.Windows.Forms.Panel opnExample;
    }
}