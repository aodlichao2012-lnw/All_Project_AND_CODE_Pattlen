namespace AdaPos.Forms
{
    partial class wVideo
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.opnPage = new System.Windows.Forms.Panel();
            this.opnHDMenu = new System.Windows.Forms.Panel();
            this.opnHDMenuBar = new System.Windows.Forms.FlowLayoutPanel();
            this.opbLogo = new System.Windows.Forms.PictureBox();
            this.olaBranch = new System.Windows.Forms.Label();
            this.opbHome = new System.Windows.Forms.PictureBox();
            this.olaHome = new System.Windows.Forms.Label();
            this.opnMenu = new System.Windows.Forms.Panel();
            this.opnMenuBT = new System.Windows.Forms.FlowLayoutPanel();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmAbout = new System.Windows.Forms.Button();
            this.ocmHelp = new System.Windows.Forms.Button();
            this.ocmMenu = new System.Windows.Forms.Button();
            this.opnContent = new System.Windows.Forms.TableLayoutPanel();
            this.ogdYoutube = new System.Windows.Forms.DataGridView();
            this.otbYoutubeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbYoutubeDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.owbYoutube = new Gecko.GeckoWebBrowser();
            this.otmClose = new System.Windows.Forms.Timer(this.components);
            this.opnPage.SuspendLayout();
            this.opnHDMenu.SuspendLayout();
            this.opnHDMenuBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.opbHome)).BeginInit();
            this.opnMenu.SuspendLayout();
            this.opnMenuBT.SuspendLayout();
            this.opnContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdYoutube)).BeginInit();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.opnPage.Controls.Add(this.opnHDMenu);
            this.opnPage.Controls.Add(this.opnMenu);
            this.opnPage.Controls.Add(this.opnContent);
            this.opnPage.Location = new System.Drawing.Point(0, 0);
            this.opnPage.Name = "opnPage";
            this.opnPage.Size = new System.Drawing.Size(1024, 768);
            this.opnPage.TabIndex = 0;
            // 
            // opnHDMenu
            // 
            this.opnHDMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHDMenu.BackColor = System.Drawing.Color.Transparent;
            this.opnHDMenu.Controls.Add(this.opnHDMenuBar);
            this.opnHDMenu.Location = new System.Drawing.Point(50, 0);
            this.opnHDMenu.Margin = new System.Windows.Forms.Padding(0);
            this.opnHDMenu.Name = "opnHDMenu";
            this.opnHDMenu.Size = new System.Drawing.Size(974, 50);
            this.opnHDMenu.TabIndex = 4;
            // 
            // opnHDMenuBar
            // 
            this.opnHDMenuBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHDMenuBar.BackColor = System.Drawing.Color.White;
            this.opnHDMenuBar.Controls.Add(this.opbLogo);
            this.opnHDMenuBar.Controls.Add(this.olaBranch);
            this.opnHDMenuBar.Controls.Add(this.opbHome);
            this.opnHDMenuBar.Controls.Add(this.olaHome);
            this.opnHDMenuBar.Location = new System.Drawing.Point(0, 0);
            this.opnHDMenuBar.Margin = new System.Windows.Forms.Padding(0);
            this.opnHDMenuBar.Name = "opnHDMenuBar";
            this.opnHDMenuBar.Size = new System.Drawing.Size(974, 50);
            this.opnHDMenuBar.TabIndex = 3;
            // 
            // opbLogo
            // 
            this.opbLogo.Image = global::AdaPos.Properties.Resources.Adasoft;
            this.opbLogo.Location = new System.Drawing.Point(0, 0);
            this.opbLogo.Margin = new System.Windows.Forms.Padding(0);
            this.opbLogo.Name = "opbLogo";
            this.opbLogo.Size = new System.Drawing.Size(100, 50);
            this.opbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.opbLogo.TabIndex = 4;
            this.opbLogo.TabStop = false;
            // 
            // olaBranch
            // 
            this.olaBranch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.olaBranch.AutoSize = true;
            this.olaBranch.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaBranch.Location = new System.Drawing.Point(103, 0);
            this.olaBranch.Name = "olaBranch";
            this.olaBranch.Size = new System.Drawing.Size(117, 50);
            this.olaBranch.TabIndex = 7;
            this.olaBranch.Text = "The Mall Bangkapi";
            this.olaBranch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opbHome
            // 
            this.opbHome.Image = global::AdaPos.Properties.Resources.VideoB_32;
            this.opbHome.Location = new System.Drawing.Point(223, 0);
            this.opbHome.Margin = new System.Windows.Forms.Padding(0);
            this.opbHome.Name = "opbHome";
            this.opbHome.Size = new System.Drawing.Size(35, 50);
            this.opbHome.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.opbHome.TabIndex = 5;
            this.opbHome.TabStop = false;
            // 
            // olaHome
            // 
            this.olaHome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.olaHome.AutoSize = true;
            this.olaHome.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaHome.Location = new System.Drawing.Point(258, 0);
            this.olaHome.Margin = new System.Windows.Forms.Padding(0);
            this.olaHome.Name = "olaHome";
            this.olaHome.Size = new System.Drawing.Size(43, 50);
            this.olaHome.TabIndex = 6;
            this.olaHome.Text = "Video";
            this.olaHome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opnMenu
            // 
            this.opnMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.opnMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnMenu.Controls.Add(this.opnMenuBT);
            this.opnMenu.Controls.Add(this.ocmMenu);
            this.opnMenu.Location = new System.Drawing.Point(0, 0);
            this.opnMenu.Name = "opnMenu";
            this.opnMenu.Size = new System.Drawing.Size(50, 768);
            this.opnMenu.TabIndex = 1;
            // 
            // opnMenuBT
            // 
            this.opnMenuBT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnMenuBT.Controls.Add(this.ocmBack);
            this.opnMenuBT.Controls.Add(this.ocmAbout);
            this.opnMenuBT.Controls.Add(this.ocmHelp);
            this.opnMenuBT.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            this.opnMenuBT.Location = new System.Drawing.Point(0, 608);
            this.opnMenuBT.Margin = new System.Windows.Forms.Padding(0);
            this.opnMenuBT.Name = "opnMenuBT";
            this.opnMenuBT.Size = new System.Drawing.Size(250, 160);
            this.opnMenuBT.TabIndex = 15;
            // 
            // ocmBack
            // 
            this.ocmBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmBack.FlatAppearance.BorderSize = 0;
            this.ocmBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmBack.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmBack.ForeColor = System.Drawing.Color.White;
            this.ocmBack.Image = global::AdaPos.Properties.Resources.BackW_32;
            this.ocmBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmBack.Location = new System.Drawing.Point(0, 110);
            this.ocmBack.Margin = new System.Windows.Forms.Padding(0);
            this.ocmBack.Name = "ocmBack";
            this.ocmBack.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ocmBack.Size = new System.Drawing.Size(250, 50);
            this.ocmBack.TabIndex = 5;
            this.ocmBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmBack.UseVisualStyleBackColor = false;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // ocmAbout
            // 
            this.ocmAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmAbout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmAbout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmAbout.FlatAppearance.BorderSize = 0;
            this.ocmAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAbout.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmAbout.ForeColor = System.Drawing.Color.White;
            this.ocmAbout.Image = global::AdaPos.Properties.Resources.About_32;
            this.ocmAbout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmAbout.Location = new System.Drawing.Point(0, 60);
            this.ocmAbout.Margin = new System.Windows.Forms.Padding(0);
            this.ocmAbout.MaximumSize = new System.Drawing.Size(250, 50);
            this.ocmAbout.Name = "ocmAbout";
            this.ocmAbout.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ocmAbout.Size = new System.Drawing.Size(250, 50);
            this.ocmAbout.TabIndex = 7;
            this.ocmAbout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmAbout.UseVisualStyleBackColor = false;
            this.ocmAbout.Click += new System.EventHandler(this.ocmAbout_Click);
            // 
            // ocmHelp
            // 
            this.ocmHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmHelp.FlatAppearance.BorderSize = 0;
            this.ocmHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmHelp.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmHelp.ForeColor = System.Drawing.Color.White;
            this.ocmHelp.Image = global::AdaPos.Properties.Resources.Help_32;
            this.ocmHelp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmHelp.Location = new System.Drawing.Point(0, 10);
            this.ocmHelp.Margin = new System.Windows.Forms.Padding(0);
            this.ocmHelp.MaximumSize = new System.Drawing.Size(250, 50);
            this.ocmHelp.Name = "ocmHelp";
            this.ocmHelp.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ocmHelp.Size = new System.Drawing.Size(250, 50);
            this.ocmHelp.TabIndex = 6;
            this.ocmHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmHelp.UseVisualStyleBackColor = false;
            this.ocmHelp.Click += new System.EventHandler(this.ocmHelp_Click);
            // 
            // ocmMenu
            // 
            this.ocmMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmMenu.BackgroundImage = global::AdaPos.Properties.Resources.Menu_64;
            this.ocmMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ocmMenu.FlatAppearance.BorderSize = 0;
            this.ocmMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmMenu.Location = new System.Drawing.Point(0, 0);
            this.ocmMenu.Name = "ocmMenu";
            this.ocmMenu.Size = new System.Drawing.Size(50, 50);
            this.ocmMenu.TabIndex = 1;
            this.ocmMenu.UseVisualStyleBackColor = false;
            this.ocmMenu.Click += new System.EventHandler(this.ocmMenu_Click);
            // 
            // opnContent
            // 
            this.opnContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnContent.ColumnCount = 2;
            this.opnContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.opnContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.opnContent.Controls.Add(this.ogdYoutube, 0, 0);
            this.opnContent.Controls.Add(this.owbYoutube, 1, 0);
            this.opnContent.Location = new System.Drawing.Point(52, 52);
            this.opnContent.Name = "opnContent";
            this.opnContent.RowCount = 1;
            this.opnContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opnContent.Size = new System.Drawing.Size(970, 714);
            this.opnContent.TabIndex = 3;
            // 
            // ogdYoutube
            // 
            this.ogdYoutube.AllowUserToAddRows = false;
            this.ogdYoutube.AllowUserToDeleteRows = false;
            this.ogdYoutube.AllowUserToResizeColumns = false;
            this.ogdYoutube.AllowUserToResizeRows = false;
            this.ogdYoutube.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ogdYoutube.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ogdYoutube.BackgroundColor = System.Drawing.Color.White;
            this.ogdYoutube.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ogdYoutube.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ogdYoutube.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ogdYoutube.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ogdYoutube.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.otbYoutubeName,
            this.otbYoutubeDate});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdYoutube.DefaultCellStyle = dataGridViewCellStyle4;
            this.ogdYoutube.EnableHeadersVisualStyles = false;
            this.ogdYoutube.GridColor = System.Drawing.Color.LightGray;
            this.ogdYoutube.Location = new System.Drawing.Point(0, 0);
            this.ogdYoutube.Margin = new System.Windows.Forms.Padding(0);
            this.ogdYoutube.MultiSelect = false;
            this.ogdYoutube.Name = "ogdYoutube";
            this.ogdYoutube.ReadOnly = true;
            this.ogdYoutube.RowHeadersVisible = false;
            this.ogdYoutube.RowTemplate.Height = 40;
            this.ogdYoutube.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ogdYoutube.ShowCellErrors = false;
            this.ogdYoutube.ShowCellToolTips = false;
            this.ogdYoutube.ShowEditingIcon = false;
            this.ogdYoutube.ShowRowErrors = false;
            this.ogdYoutube.Size = new System.Drawing.Size(339, 714);
            this.ogdYoutube.TabIndex = 1;
            // 
            // otbYoutubeName
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.otbYoutubeName.DefaultCellStyle = dataGridViewCellStyle2;
            this.otbYoutubeName.FillWeight = 60F;
            this.otbYoutubeName.HeaderText = "Name";
            this.otbYoutubeName.Name = "otbYoutubeName";
            this.otbYoutubeName.ReadOnly = true;
            this.otbYoutubeName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbYoutubeDate
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.otbYoutubeDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.otbYoutubeDate.FillWeight = 40F;
            this.otbYoutubeDate.HeaderText = "Date and time";
            this.otbYoutubeDate.Name = "otbYoutubeDate";
            this.otbYoutubeDate.ReadOnly = true;
            this.otbYoutubeDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // owbYoutube
            // 
            this.owbYoutube.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.owbYoutube.Location = new System.Drawing.Point(341, 0);
            this.owbYoutube.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.owbYoutube.Name = "owbYoutube";
            this.owbYoutube.Size = new System.Drawing.Size(629, 714);
            this.owbYoutube.TabIndex = 2;
            this.owbYoutube.UseHttpActivityObserver = false;
            // 
            // otmClose
            // 
            this.otmClose.Tick += new System.EventHandler(this.otmClose_Tick);
            // 
            // wVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(1024, 766);
            this.Name = "wVideo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wYoutube";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wVideo_FormClosing);
            this.opnPage.ResumeLayout(false);
            this.opnHDMenu.ResumeLayout(false);
            this.opnHDMenuBar.ResumeLayout(false);
            this.opnHDMenuBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.opbHome)).EndInit();
            this.opnMenu.ResumeLayout(false);
            this.opnMenuBT.ResumeLayout(false);
            this.opnContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ogdYoutube)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnMenu;
        private System.Windows.Forms.Button ocmMenu;
        private System.Windows.Forms.TableLayoutPanel opnContent;
        private System.Windows.Forms.DataGridView ogdYoutube;
        private System.Windows.Forms.FlowLayoutPanel opnMenuBT;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmAbout;
        private System.Windows.Forms.Button ocmHelp;
        private Gecko.GeckoWebBrowser owbYoutube;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbYoutubeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbYoutubeDate;
        private System.Windows.Forms.Panel opnHDMenu;
        private System.Windows.Forms.FlowLayoutPanel opnHDMenuBar;
        private System.Windows.Forms.PictureBox opbLogo;
        private System.Windows.Forms.Label olaBranch;
        private System.Windows.Forms.PictureBox opbHome;
        private System.Windows.Forms.Label olaHome;
        private System.Windows.Forms.Timer otmClose;
    }
}