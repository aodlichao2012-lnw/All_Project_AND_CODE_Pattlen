namespace AdaPos.Control
{
    partial class uVideo2ndScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uVideo2ndScreen));
            this.owm2ndScreen = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.owm2ndScreen)).BeginInit();
            this.SuspendLayout();
            // 
            // owm2ndScreen
            // 
            this.owm2ndScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.owm2ndScreen.Enabled = true;
            this.owm2ndScreen.Location = new System.Drawing.Point(0, 0);
            this.owm2ndScreen.Name = "owm2ndScreen";
            this.owm2ndScreen.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("owm2ndScreen.OcxState")));
            this.owm2ndScreen.Size = new System.Drawing.Size(675, 599);
            this.owm2ndScreen.TabIndex = 0;
            // 
            // uVideo2ndScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.owm2ndScreen);
            this.Name = "uVideo2ndScreen";
            this.Size = new System.Drawing.Size(675, 509);
            this.Load += new System.EventHandler(this.uVideo2ndScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.owm2ndScreen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public AxWMPLib.AxWindowsMediaPlayer owm2ndScreen;
    }
}
