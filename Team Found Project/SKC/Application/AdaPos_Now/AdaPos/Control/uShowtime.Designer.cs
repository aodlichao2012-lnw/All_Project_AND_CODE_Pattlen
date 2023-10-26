namespace AdaPos.Control
{
    partial class uShowtime
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
            this.olaShowtime = new System.Windows.Forms.Label();
            this.olaDate = new System.Windows.Forms.Label();
            this.olaTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // olaShowtime
            // 
            this.olaShowtime.AutoSize = true;
            this.olaShowtime.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaShowtime.Location = new System.Drawing.Point(10, 10);
            this.olaShowtime.Name = "olaShowtime";
            this.olaShowtime.Size = new System.Drawing.Size(104, 19);
            this.olaShowtime.TabIndex = 0;
            this.olaShowtime.Text = "รอบ 10.00 - 11.00";
            // 
            // olaDate
            // 
            this.olaDate.AutoSize = true;
            this.olaDate.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaDate.Location = new System.Drawing.Point(10, 50);
            this.olaDate.Name = "olaDate";
            this.olaDate.Size = new System.Drawing.Size(111, 19);
            this.olaDate.TabIndex = 1;
            this.olaDate.Text = "Date : 22/06/2018";
            // 
            // olaTime
            // 
            this.olaTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTime.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.olaTime.Location = new System.Drawing.Point(-2, 81);
            this.olaTime.Name = "olaTime";
            this.olaTime.Size = new System.Drawing.Size(230, 37);
            this.olaTime.TabIndex = 2;
            this.olaTime.Text = "19.00 - 20.00";
            this.olaTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // uShowtime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.olaTime);
            this.Controls.Add(this.olaDate);
            this.Controls.Add(this.olaShowtime);
            this.Name = "uShowtime";
            this.Size = new System.Drawing.Size(228, 118);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label olaShowtime;
        private System.Windows.Forms.Label olaDate;
        private System.Windows.Forms.Label olaTime;
    }
}
