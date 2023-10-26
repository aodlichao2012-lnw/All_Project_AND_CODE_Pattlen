namespace AdaPos.Control
{
    partial class uMarqueeTxt
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
            this.components = new System.ComponentModel.Container();
            this.olaText = new System.Windows.Forms.Label();
            this.otmMoveText = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // olaText
            // 
            this.olaText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.olaText.AutoSize = true;
            this.olaText.Location = new System.Drawing.Point(0, 0);
            this.olaText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaText.Name = "olaText";
            this.olaText.Size = new System.Drawing.Size(88, 21);
            this.olaText.TabIndex = 0;
            this.olaText.Text = "Sample Text";
            // 
            // otmMoveText
            // 
            this.otmMoveText.Tick += new System.EventHandler(this.otmMoveText_Tick);
            // 
            // uMarqueeTxt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.olaText);
            this.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "uMarqueeTxt";
            this.Size = new System.Drawing.Size(200, 242);
            this.Load += new System.EventHandler(this.uMarqueeTxt_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label olaText;
        private System.Windows.Forms.Timer otmMoveText;
    }
}
