namespace AdaPos.Control
{
    partial class uQty
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
            this.otbQty = new System.Windows.Forms.TextBox();
            this.ocmDel = new System.Windows.Forms.Button();
            this.ocmAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // otbQty
            // 
            this.otbQty.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.otbQty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.otbQty.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.otbQty.ForeColor = System.Drawing.Color.Black;
            this.otbQty.Location = new System.Drawing.Point(42, 2);
            this.otbQty.Margin = new System.Windows.Forms.Padding(2);
            this.otbQty.MaxLength = 5;
            this.otbQty.Multiline = true;
            this.otbQty.Name = "otbQty";
            this.otbQty.Size = new System.Drawing.Size(58, 29);
            this.otbQty.TabIndex = 8;
            this.otbQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.otbQty.WordWrap = false;
            this.otbQty.Click += new System.EventHandler(this.otbQty_Click);
            this.otbQty.TextChanged += new System.EventHandler(this.otbQty_TextChanged);
            this.otbQty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otbQty_KeyDown);
            this.otbQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.otbQty_KeyPress);
            // 
            // ocmDel
            // 
            this.ocmDel.BackColor = System.Drawing.Color.Transparent;
            this.ocmDel.Dock = System.Windows.Forms.DockStyle.Left;
            this.ocmDel.FlatAppearance.BorderSize = 0;
            this.ocmDel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmDel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ocmDel.ForeColor = System.Drawing.Color.Black;
            this.ocmDel.Image = global::AdaPos.Properties.Resources.ClearW_32_V2;
            this.ocmDel.Location = new System.Drawing.Point(2, 2);
            this.ocmDel.Margin = new System.Windows.Forms.Padding(2);
            this.ocmDel.Name = "ocmDel";
            this.ocmDel.Size = new System.Drawing.Size(40, 29);
            this.ocmDel.TabIndex = 6;
            this.ocmDel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ocmDel.UseVisualStyleBackColor = false;
            this.ocmDel.Click += new System.EventHandler(this.ocmDel_Click);
            // 
            // ocmAdd
            // 
            this.ocmAdd.BackColor = System.Drawing.Color.Transparent;
            this.ocmAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.ocmAdd.FlatAppearance.BorderSize = 0;
            this.ocmAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAdd.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ocmAdd.ForeColor = System.Drawing.Color.Black;
            this.ocmAdd.Image = global::AdaPos.Properties.Resources.AddW_32_V2;
            this.ocmAdd.Location = new System.Drawing.Point(100, 2);
            this.ocmAdd.Margin = new System.Windows.Forms.Padding(2);
            this.ocmAdd.Name = "ocmAdd";
            this.ocmAdd.Size = new System.Drawing.Size(40, 29);
            this.ocmAdd.TabIndex = 7;
            this.ocmAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ocmAdd.UseVisualStyleBackColor = false;
            this.ocmAdd.Click += new System.EventHandler(this.ocmAdd_Click);
            // 
            // uQty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.otbQty);
            this.Controls.Add(this.ocmAdd);
            this.Controls.Add(this.ocmDel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "uQty";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(142, 33);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.TextBox otbQty;
        public System.Windows.Forms.Button ocmDel;
        public System.Windows.Forms.Button ocmAdd;
    }
}
