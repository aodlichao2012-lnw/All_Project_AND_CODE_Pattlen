namespace AdaPos
{
    partial class wSplashScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(wSplashScreen));
            this.ockShowForm = new System.Windows.Forms.CheckBox();
            this.otmClose = new System.Windows.Forms.Timer(this.components);
            this.opnMenu = new System.Windows.Forms.TableLayoutPanel();
            this.ocmMenu = new System.Windows.Forms.Button();
            this.opnMenuT = new System.Windows.Forms.TableLayoutPanel();
            this.opnMenuB = new System.Windows.Forms.TableLayoutPanel();
            this.opnMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ockShowForm
            // 
            this.ockShowForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ockShowForm.AutoSize = true;
            this.ockShowForm.BackColor = System.Drawing.Color.Transparent;
            this.ockShowForm.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ockShowForm.Location = new System.Drawing.Point(828, 861);
            this.ockShowForm.Name = "ockShowForm";
            this.ockShowForm.Size = new System.Drawing.Size(184, 23);
            this.ockShowForm.TabIndex = 10;
            this.ockShowForm.Tag = "";
            this.ockShowForm.Text = "Don\'t show this page again";
            this.ockShowForm.UseVisualStyleBackColor = false;
            this.ockShowForm.CheckedChanged += new System.EventHandler(this.ockShowForm_CheckedChanged);
            // 
            // otmClose
            // 
            this.otmClose.Tick += new System.EventHandler(this.otmClose_Tick);
            // 
            // opnMenu
            // 
            this.opnMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.opnMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnMenu.ColumnCount = 1;
            this.opnMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opnMenu.Controls.Add(this.ocmMenu, 0, 0);
            this.opnMenu.Controls.Add(this.opnMenuT, 0, 1);
            this.opnMenu.Controls.Add(this.opnMenuB, 0, 2);
            this.opnMenu.Location = new System.Drawing.Point(0, 0);
            this.opnMenu.Name = "opnMenu";
            this.opnMenu.RowCount = 3;
            this.opnMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.opnMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opnMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opnMenu.Size = new System.Drawing.Size(55, 768);
            this.opnMenu.TabIndex = 11;
            this.opnMenu.MouseLeave += new System.EventHandler(this.opnMenu_MouseLeave);
            // 
            // ocmMenu
            // 
            this.ocmMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmMenu.BackgroundImage = global::AdaPos.Properties.Resources.Menu_64;
            this.ocmMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ocmMenu.FlatAppearance.BorderSize = 0;
            this.ocmMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmMenu.Location = new System.Drawing.Point(3, 3);
            this.ocmMenu.Name = "ocmMenu";
            this.ocmMenu.Size = new System.Drawing.Size(50, 49);
            this.ocmMenu.TabIndex = 12;
            this.ocmMenu.UseVisualStyleBackColor = false;
            this.ocmMenu.Click += new System.EventHandler(this.ocmMenu_Click);
            // 
            // opnMenuT
            // 
            this.opnMenuT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnMenuT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnMenuT.ColumnCount = 1;
            this.opnMenuT.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 247F));
            this.opnMenuT.Location = new System.Drawing.Point(3, 58);
            this.opnMenuT.Name = "opnMenuT";
            this.opnMenuT.RowCount = 12;
            this.opnMenuT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.opnMenuT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.opnMenuT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.opnMenuT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.opnMenuT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.opnMenuT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.opnMenuT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.opnMenuT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.opnMenuT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.opnMenuT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.opnMenuT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.opnMenuT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.opnMenuT.Size = new System.Drawing.Size(247, 350);
            this.opnMenuT.TabIndex = 0;
            // 
            // opnMenuB
            // 
            this.opnMenuB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnMenuB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnMenuB.ColumnCount = 1;
            this.opnMenuB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 247F));
            this.opnMenuB.Location = new System.Drawing.Point(3, 414);
            this.opnMenuB.Name = "opnMenuB";
            this.opnMenuB.RowCount = 10;
            this.opnMenuB.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opnMenuB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.opnMenuB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.opnMenuB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.opnMenuB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.opnMenuB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.opnMenuB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.opnMenuB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.opnMenuB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.opnMenuB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.opnMenuB.Size = new System.Drawing.Size(247, 351);
            this.opnMenuB.TabIndex = 13;
            // 
            // wSplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.BackgroundImage = global::AdaPos.Properties.Resources.BG;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.ControlBox = false;
            this.Controls.Add(this.opnMenu);
            this.Controls.Add(this.ockShowForm);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "wSplashScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wSplashScreen_FormClosing);
            this.Shown += new System.EventHandler(this.wSplashScreen_Shown);
            this.Click += new System.EventHandler(this.wSplashScreen_Click);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ocmMenu_KeyDown);
            this.opnMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox ockShowForm;
        private System.Windows.Forms.Timer otmClose;
        private System.Windows.Forms.TableLayoutPanel opnMenu;
        private System.Windows.Forms.Button ocmMenu;
        private System.Windows.Forms.TableLayoutPanel opnMenuT;
        private System.Windows.Forms.TableLayoutPanel opnMenuB;
    }
}