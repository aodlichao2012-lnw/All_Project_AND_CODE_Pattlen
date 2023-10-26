namespace AdaPos.Popup.All
{
    partial class wCountDown
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
            this.components = new System.ComponentModel.Container();
            this.opnPage = new System.Windows.Forms.Panel();
            this.opnContent = new System.Windows.Forms.Panel();
            this.olaCount = new System.Windows.Forms.Label();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitle = new System.Windows.Forms.Label();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.otmCountDown = new System.Windows.Forms.Timer(this.components);
            this.opnPage.SuspendLayout();
            this.opnContent.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.opnContent);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnPage.Location = new System.Drawing.Point(0, 0);
            this.opnPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(332, 205);
            this.opnPage.TabIndex = 10;
            // 
            // opnContent
            // 
            this.opnContent.BackColor = System.Drawing.Color.White;
            this.opnContent.Controls.Add(this.olaCount);
            this.opnContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnContent.Location = new System.Drawing.Point(1, 51);
            this.opnContent.Margin = new System.Windows.Forms.Padding(0);
            this.opnContent.Name = "opnContent";
            this.opnContent.Padding = new System.Windows.Forms.Padding(12, 13, 12, 13);
            this.opnContent.Size = new System.Drawing.Size(328, 151);
            this.opnContent.TabIndex = 10;
            // 
            // olaCount
            // 
            this.olaCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olaCount.Font = new System.Drawing.Font("Segoe UI Semibold", 60F);
            this.olaCount.Location = new System.Drawing.Point(12, 13);
            this.olaCount.Name = "olaCount";
            this.olaCount.Size = new System.Drawing.Size(304, 125);
            this.olaCount.TabIndex = 0;
            this.olaCount.Text = "0";
            this.olaCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // opnHD
            // 
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitle);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Dock = System.Windows.Forms.DockStyle.Top;
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.opnHD.Name = "opnHD";
            this.opnHD.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.opnHD.Size = new System.Drawing.Size(328, 50);
            this.opnHD.TabIndex = 9;
            // 
            // olaTitle
            // 
            this.olaTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olaTitle.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Bold);
            this.olaTitle.ForeColor = System.Drawing.Color.White;
            this.olaTitle.Location = new System.Drawing.Point(12, 0);
            this.olaTitle.Name = "olaTitle";
            this.olaTitle.Size = new System.Drawing.Size(266, 50);
            this.olaTitle.TabIndex = 7;
            this.olaTitle.Text = "Stop Count Down ?";
            this.olaTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ocmAccept
            // 
            this.ocmAccept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmAccept.Dock = System.Windows.Forms.DockStyle.Right;
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.Location = new System.Drawing.Point(278, 0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 38;
            this.ocmAccept.UseVisualStyleBackColor = false;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // otmCountDown
            // 
            this.otmCountDown.Interval = 1000;
            this.otmCountDown.Tick += new System.EventHandler(this.otmCountDown_Tick);
            // 
            // wCountDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(332, 205);
            this.Controls.Add(this.opnPage);
            this.Font = new System.Drawing.Font("Segoe UI Light", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "wCountDown";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wCountDown";
            this.Shown += new System.EventHandler(this.wCountDown_Shown);
            this.opnPage.ResumeLayout(false);
            this.opnContent.ResumeLayout(false);
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnContent;
        private System.Windows.Forms.Label olaCount;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitle;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Timer otmCountDown;
    }
}