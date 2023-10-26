namespace AdaPos.Forms
{
    partial class wBlank
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
            this.opnMenu = new System.Windows.Forms.TableLayoutPanel();
            this.ocmMenu = new System.Windows.Forms.Button();
            this.opnContent = new System.Windows.Forms.Panel();
            this.opnBG = new System.Windows.Forms.Panel();
            this.opnHDMenu = new System.Windows.Forms.Panel();
            this.opnHDMenuBar = new System.Windows.Forms.FlowLayoutPanel();
            this.opbLogo = new System.Windows.Forms.PictureBox();
            this.olaBranch = new System.Windows.Forms.Label();
            this.opnHDUser = new System.Windows.Forms.Panel();
            this.opnPage.SuspendLayout();
            this.opnMenu.SuspendLayout();
            this.opnContent.SuspendLayout();
            this.opnHDMenu.SuspendLayout();
            this.opnHDMenuBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Controls.Add(this.opnMenu);
            this.opnPage.Controls.Add(this.opnContent);
            this.opnPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnPage.Location = new System.Drawing.Point(0, 0);
            this.opnPage.Name = "opnPage";
            this.opnPage.Size = new System.Drawing.Size(1024, 768);
            this.opnPage.TabIndex = 25;
            // 
            // opnMenu
            // 
            this.opnMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnMenu.ColumnCount = 1;
            this.opnMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opnMenu.Controls.Add(this.ocmMenu, 0, 0);
            this.opnMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.opnMenu.Location = new System.Drawing.Point(0, 0);
            this.opnMenu.Name = "opnMenu";
            this.opnMenu.RowCount = 3;
            this.opnMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.opnMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opnMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opnMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.opnMenu.Size = new System.Drawing.Size(55, 768);
            this.opnMenu.TabIndex = 22;
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
            // opnContent
            // 
            this.opnContent.Controls.Add(this.opnBG);
            this.opnContent.Controls.Add(this.opnHDMenu);
            this.opnContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnContent.Location = new System.Drawing.Point(0, 0);
            this.opnContent.Name = "opnContent";
            this.opnContent.Padding = new System.Windows.Forms.Padding(55, 0, 0, 0);
            this.opnContent.Size = new System.Drawing.Size(1024, 768);
            this.opnContent.TabIndex = 13;
            // 
            // opnBG
            // 
            this.opnBG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opnBG.Location = new System.Drawing.Point(55, 50);
            this.opnBG.Name = "opnBG";
            this.opnBG.Size = new System.Drawing.Size(969, 718);
            this.opnBG.TabIndex = 24;
            // 
            // opnHDMenu
            // 
            this.opnHDMenu.BackColor = System.Drawing.Color.Transparent;
            this.opnHDMenu.Controls.Add(this.opnHDMenuBar);
            this.opnHDMenu.Controls.Add(this.opnHDUser);
            this.opnHDMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.opnHDMenu.Location = new System.Drawing.Point(55, 0);
            this.opnHDMenu.Margin = new System.Windows.Forms.Padding(0);
            this.opnHDMenu.Name = "opnHDMenu";
            this.opnHDMenu.Size = new System.Drawing.Size(969, 50);
            this.opnHDMenu.TabIndex = 23;
            // 
            // opnHDMenuBar
            // 
            this.opnHDMenuBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHDMenuBar.BackColor = System.Drawing.Color.White;
            this.opnHDMenuBar.Controls.Add(this.opbLogo);
            this.opnHDMenuBar.Controls.Add(this.olaBranch);
            this.opnHDMenuBar.Location = new System.Drawing.Point(0, 0);
            this.opnHDMenuBar.Margin = new System.Windows.Forms.Padding(0);
            this.opnHDMenuBar.Name = "opnHDMenuBar";
            this.opnHDMenuBar.Size = new System.Drawing.Size(689, 50);
            this.opnHDMenuBar.TabIndex = 3;
            // 
            // opbLogo
            // 
            this.opbLogo.Location = new System.Drawing.Point(0, 0);
            this.opbLogo.Margin = new System.Windows.Forms.Padding(0);
            this.opbLogo.Name = "opbLogo";
            this.opbLogo.Size = new System.Drawing.Size(100, 50);
            this.opbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.opbLogo.TabIndex = 4;
            this.opbLogo.TabStop = false;
            this.opbLogo.Visible = false;
            // 
            // olaBranch
            // 
            this.olaBranch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.olaBranch.AutoSize = true;
            this.olaBranch.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaBranch.Location = new System.Drawing.Point(110, 0);
            this.olaBranch.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.olaBranch.Name = "olaBranch";
            this.olaBranch.Size = new System.Drawing.Size(117, 50);
            this.olaBranch.TabIndex = 7;
            this.olaBranch.Text = "The Mall Bangkapi";
            this.olaBranch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opnHDUser
            // 
            this.opnHDUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHDUser.BackColor = System.Drawing.Color.White;
            this.opnHDUser.Location = new System.Drawing.Point(689, 0);
            this.opnHDUser.Name = "opnHDUser";
            this.opnHDUser.Size = new System.Drawing.Size(280, 50);
            this.opnHDUser.TabIndex = 1;
            // 
            // wBlank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wBlank";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wBlank";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wBlank_FormClosing);
            this.Shown += new System.EventHandler(this.wBlank_Shown);
            this.opnPage.ResumeLayout(false);
            this.opnMenu.ResumeLayout(false);
            this.opnContent.ResumeLayout(false);
            this.opnHDMenu.ResumeLayout(false);
            this.opnHDMenuBar.ResumeLayout(false);
            this.opnHDMenuBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.TableLayoutPanel opnMenu;
        private System.Windows.Forms.Button ocmMenu;
        private System.Windows.Forms.Panel opnContent;
        private System.Windows.Forms.Panel opnHDMenu;
        private System.Windows.Forms.FlowLayoutPanel opnHDMenuBar;
        private System.Windows.Forms.PictureBox opbLogo;
        private System.Windows.Forms.Label olaBranch;
        private System.Windows.Forms.Panel opnHDUser;
        private System.Windows.Forms.Panel opnBG;
    }
}