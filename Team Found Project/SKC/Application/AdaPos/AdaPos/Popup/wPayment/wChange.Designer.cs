namespace AdaPos.Popup.wPayment
{
    partial class wChange
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
            this.opnChange = new System.Windows.Forms.Panel();
            this.olaNumChange = new System.Windows.Forms.Label();
            this.olaChange = new System.Windows.Forms.Label();
            this.otmClose = new System.Windows.Forms.Timer(this.components);
            this.opnChange.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnChange
            // 
            this.opnChange.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.opnChange.BackColor = System.Drawing.Color.White;
            this.opnChange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnChange.Controls.Add(this.olaNumChange);
            this.opnChange.Controls.Add(this.olaChange);
            this.opnChange.Location = new System.Drawing.Point(112, 234);
            this.opnChange.Name = "opnChange";
            this.opnChange.Padding = new System.Windows.Forms.Padding(1);
            this.opnChange.Size = new System.Drawing.Size(800, 300);
            this.opnChange.TabIndex = 0;
            this.opnChange.Click += new System.EventHandler(this.opnChange_Click);
            // 
            // olaNumChange
            // 
            this.olaNumChange.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaNumChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 100F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.olaNumChange.Location = new System.Drawing.Point(1, 151);
            this.olaNumChange.Name = "olaNumChange";
            this.olaNumChange.Size = new System.Drawing.Size(796, 146);
            this.olaNumChange.TabIndex = 1;
            this.olaNumChange.Text = "0,000,000.00";
            this.olaNumChange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.olaNumChange.Click += new System.EventHandler(this.opnChange_Click);
            // 
            // olaChange
            // 
            this.olaChange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaChange.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.olaChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.olaChange.ForeColor = System.Drawing.Color.White;
            this.olaChange.Location = new System.Drawing.Point(1, 1);
            this.olaChange.Name = "olaChange";
            this.olaChange.Size = new System.Drawing.Size(796, 150);
            this.olaChange.TabIndex = 0;
            this.olaChange.Text = "Change";
            this.olaChange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.olaChange.Click += new System.EventHandler(this.opnChange_Click);
            // 
            // otmClose
            // 
            this.otmClose.Interval = 10000;
            this.otmClose.Tick += new System.EventHandler(this.otmClose_Tick);
            // 
            // wChange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnChange);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wChange";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wChange";
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Click += new System.EventHandler(this.opnChange_Click);
            this.opnChange.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnChange;
        private System.Windows.Forms.Label olaNumChange;
        private System.Windows.Forms.Label olaChange;
        private System.Windows.Forms.Timer otmClose;
    }
}