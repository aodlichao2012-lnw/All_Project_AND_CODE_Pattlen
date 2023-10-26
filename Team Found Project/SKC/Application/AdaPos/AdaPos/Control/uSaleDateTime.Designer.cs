namespace AdaPos.Control
{
    partial class uSaleDateTime
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
            this.olaSaleDate = new System.Windows.Forms.Label();
            this.otmSaleDate = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // olaSaleDate
            // 
            this.olaSaleDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.olaSaleDate.AutoSize = true;
            this.olaSaleDate.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaSaleDate.Location = new System.Drawing.Point(11, 12);
            this.olaSaleDate.Margin = new System.Windows.Forms.Padding(0);
            this.olaSaleDate.Name = "olaSaleDate";
            this.olaSaleDate.Size = new System.Drawing.Size(102, 19);
            this.olaSaleDate.TabIndex = 4;
            this.olaSaleDate.Text = "10/05/2018 17:31";
            this.olaSaleDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // otmSaleDate
            // 
            this.otmSaleDate.Interval = 10000;
            this.otmSaleDate.Tick += new System.EventHandler(this.otmSaleDate_Tick);
            // 
            // uSaleDateTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.olaSaleDate);
            this.Name = "uSaleDateTime";
            this.Size = new System.Drawing.Size(129, 40);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label olaSaleDate;
        private System.Windows.Forms.Timer otmSaleDate;
    }
}
