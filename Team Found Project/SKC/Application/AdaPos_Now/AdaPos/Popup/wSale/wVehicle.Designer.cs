namespace AdaPos.Popup.wSale
{
    partial class wVehicle
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(wVehicle));
            this.opnPage = new System.Windows.Forms.Panel();
            this.ogdVeh = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.opnBottom = new System.Windows.Forms.Panel();
            this.ocmSchNextPage = new System.Windows.Forms.Button();
            this.ocmSchPrevious = new System.Windows.Forms.Button();
            this.olaPage = new System.Windows.Forms.Label();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitleVehicle = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.opnPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdVeh)).BeginInit();
            this.opnBottom.SuspendLayout();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.Controls.Add(this.ogdVeh);
            this.opnPage.Controls.Add(this.opnBottom);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(1, 1);
            this.opnPage.Name = "opnPage";
            this.opnPage.Size = new System.Drawing.Size(897, 598);
            this.opnPage.TabIndex = 0;
            // 
            // ogdVeh
            // 
            this.ogdVeh.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None;
            this.ogdVeh.AllowEditing = false;
            this.ogdVeh.AllowNodeCellCheck = false;
            this.ogdVeh.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.None;
            this.ogdVeh.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.ogdVeh.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ogdVeh.ColumnInfo = resources.GetString("ogdVeh.ColumnInfo");
            this.ogdVeh.ExtendLastCol = true;
            this.ogdVeh.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None;
            this.ogdVeh.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ogdVeh.Location = new System.Drawing.Point(11, 71);
            this.ogdVeh.Margin = new System.Windows.Forms.Padding(4);
            this.ogdVeh.Name = "ogdVeh";
            this.ogdVeh.Rows.Count = 1;
            this.ogdVeh.Rows.MaxSize = 40;
            this.ogdVeh.Rows.MinSize = 40;
            this.ogdVeh.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ogdVeh.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.ogdVeh.Size = new System.Drawing.Size(874, 455);
            this.ogdVeh.TabIndex = 40;
            this.ogdVeh.UseCompatibleTextRendering = true;
            this.ogdVeh.Click += new System.EventHandler(this.ogdVeh_Click);
            this.ogdVeh.DoubleClick += new System.EventHandler(this.ogdVeh_DoubleClick);
            // 
            // opnBottom
            // 
            this.opnBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.opnBottom.Controls.Add(this.ocmSchNextPage);
            this.opnBottom.Controls.Add(this.ocmSchPrevious);
            this.opnBottom.Controls.Add(this.olaPage);
            this.opnBottom.Location = new System.Drawing.Point(1, 536);
            this.opnBottom.Margin = new System.Windows.Forms.Padding(4);
            this.opnBottom.Name = "opnBottom";
            this.opnBottom.Size = new System.Drawing.Size(895, 59);
            this.opnBottom.TabIndex = 33;
            // 
            // ocmSchNextPage
            // 
            this.ocmSchNextPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmSchNextPage.Enabled = false;
            this.ocmSchNextPage.FlatAppearance.BorderSize = 0;
            this.ocmSchNextPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmSchNextPage.Image = global::AdaPos.Properties.Resources.NextB_48;
            this.ocmSchNextPage.Location = new System.Drawing.Point(826, -2);
            this.ocmSchNextPage.Margin = new System.Windows.Forms.Padding(4);
            this.ocmSchNextPage.Name = "ocmSchNextPage";
            this.ocmSchNextPage.Size = new System.Drawing.Size(67, 62);
            this.ocmSchNextPage.TabIndex = 33;
            this.ocmSchNextPage.UseVisualStyleBackColor = true;
            this.ocmSchNextPage.Click += new System.EventHandler(this.ocmSchNextPage_Click);
            // 
            // ocmSchPrevious
            // 
            this.ocmSchPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmSchPrevious.Enabled = false;
            this.ocmSchPrevious.FlatAppearance.BorderSize = 0;
            this.ocmSchPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmSchPrevious.Image = global::AdaPos.Properties.Resources.PreviousB_48;
            this.ocmSchPrevious.Location = new System.Drawing.Point(0, -2);
            this.ocmSchPrevious.Margin = new System.Windows.Forms.Padding(4);
            this.ocmSchPrevious.Name = "ocmSchPrevious";
            this.ocmSchPrevious.Size = new System.Drawing.Size(67, 62);
            this.ocmSchPrevious.TabIndex = 33;
            this.ocmSchPrevious.UseVisualStyleBackColor = true;
            this.ocmSchPrevious.Click += new System.EventHandler(this.ocmSchPrevious_Click);
            // 
            // olaPage
            // 
            this.olaPage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaPage.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaPage.Location = new System.Drawing.Point(311, -2);
            this.olaPage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaPage.Name = "olaPage";
            this.olaPage.Size = new System.Drawing.Size(274, 62);
            this.olaPage.TabIndex = 33;
            this.olaPage.Text = "Found 35 records. Page 3/5";
            this.olaPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitleVehicle);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.ForeColor = System.Drawing.Color.White;
            this.opnHD.Location = new System.Drawing.Point(2, 2);
            this.opnHD.Margin = new System.Windows.Forms.Padding(4);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(892, 62);
            this.opnHD.TabIndex = 5;
            // 
            // olaTitleVehicle
            // 
            this.olaTitleVehicle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleVehicle.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleVehicle.ForeColor = System.Drawing.Color.White;
            this.olaTitleVehicle.Location = new System.Drawing.Point(75, 0);
            this.olaTitleVehicle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaTitleVehicle.Name = "olaTitleVehicle";
            this.olaTitleVehicle.Size = new System.Drawing.Size(604, 62);
            this.olaTitleVehicle.TabIndex = 7;
            this.olaTitleVehicle.Text = "Vehicle";
            this.olaTitleVehicle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ocmBack
            // 
            this.ocmBack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmBack.FlatAppearance.BorderSize = 0;
            this.ocmBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmBack.Image = global::AdaPos.Properties.Resources.BackW_32;
            this.ocmBack.Location = new System.Drawing.Point(0, 0);
            this.ocmBack.Margin = new System.Windows.Forms.Padding(4);
            this.ocmBack.Name = "ocmBack";
            this.ocmBack.Size = new System.Drawing.Size(67, 62);
            this.ocmBack.TabIndex = 4;
            this.ocmBack.UseVisualStyleBackColor = true;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.Location = new System.Drawing.Point(825, 0);
            this.ocmAccept.Margin = new System.Windows.Forms.Padding(4);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(67, 62);
            this.ocmAccept.TabIndex = 5;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // wVehicle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wVehicle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wVehicle";
            this.opnPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ogdVeh)).EndInit();
            this.opnBottom.ResumeLayout(false);
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitleVehicle;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Panel opnBottom;
        private System.Windows.Forms.Button ocmSchNextPage;
        private System.Windows.Forms.Button ocmSchPrevious;
        private System.Windows.Forms.Label olaPage;
        private C1.Win.C1FlexGrid.C1FlexGrid ogdVeh;
    }
}