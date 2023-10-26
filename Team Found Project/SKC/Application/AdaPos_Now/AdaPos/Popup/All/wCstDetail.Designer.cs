namespace AdaPos.Popup.All
{
    partial class wCstDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(wCstDetail));
            this.opnPage = new System.Windows.Forms.Panel();
            this.opnTop = new System.Windows.Forms.Panel();
            this.olaCstCode = new System.Windows.Forms.Label();
            this.olaTableTitlt = new System.Windows.Forms.Label();
            this.olaName = new System.Windows.Forms.Label();
            this.olaPoint = new System.Windows.Forms.Label();
            this.olaPntAval = new System.Windows.Forms.Label();
            this.opnBottom = new System.Windows.Forms.Panel();
            this.ogdCstDetail = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitleCstSearch = new System.Windows.Forms.Label();
            this.ocmBack = new System.Windows.Forms.Button();
            this.ocmShwKb = new System.Windows.Forms.Button();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.opnPage.SuspendLayout();
            this.opnTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdCstDetail)).BeginInit();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.opnTop);
            this.opnPage.Controls.Add(this.opnBottom);
            this.opnPage.Controls.Add(this.ogdCstDetail);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(0, 0);
            this.opnPage.Margin = new System.Windows.Forms.Padding(4);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(999, 600);
            this.opnPage.TabIndex = 1;
            // 
            // opnTop
            // 
            this.opnTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnTop.BackColor = System.Drawing.Color.White;
            this.opnTop.Controls.Add(this.olaCstCode);
            this.opnTop.Controls.Add(this.olaTableTitlt);
            this.opnTop.Controls.Add(this.olaName);
            this.opnTop.Controls.Add(this.olaPoint);
            this.opnTop.Controls.Add(this.olaPntAval);
            this.opnTop.Location = new System.Drawing.Point(2, 66);
            this.opnTop.Name = "opnTop";
            this.opnTop.Size = new System.Drawing.Size(992, 125);
            this.opnTop.TabIndex = 42;
            // 
            // olaCstCode
            // 
            this.olaCstCode.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaCstCode.Location = new System.Drawing.Point(25, 8);
            this.olaCstCode.Name = "olaCstCode";
            this.olaCstCode.Size = new System.Drawing.Size(406, 23);
            this.olaCstCode.TabIndex = 37;
            this.olaCstCode.Text = "CstCode";
            // 
            // olaTableTitlt
            // 
            this.olaTableTitlt.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaTableTitlt.Location = new System.Drawing.Point(25, 90);
            this.olaTableTitlt.Name = "olaTableTitlt";
            this.olaTableTitlt.Size = new System.Drawing.Size(500, 23);
            this.olaTableTitlt.TabIndex = 41;
            this.olaTableTitlt.Text = "Benefit List";
            this.olaTableTitlt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // olaName
            // 
            this.olaName.Font = new System.Drawing.Font("Segoe UI Semibold", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaName.Location = new System.Drawing.Point(22, 35);
            this.olaName.Name = "olaName";
            this.olaName.Size = new System.Drawing.Size(641, 45);
            this.olaName.TabIndex = 34;
            this.olaName.Text = "Customer Name";
            // 
            // olaPoint
            // 
            this.olaPoint.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaPoint.Location = new System.Drawing.Point(742, 38);
            this.olaPoint.Name = "olaPoint";
            this.olaPoint.Size = new System.Drawing.Size(250, 43);
            this.olaPoint.TabIndex = 36;
            this.olaPoint.Text = "10000";
            this.olaPoint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // olaPntAval
            // 
            this.olaPntAval.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olaPntAval.Location = new System.Drawing.Point(742, 9);
            this.olaPntAval.Name = "olaPntAval";
            this.olaPntAval.Size = new System.Drawing.Size(250, 23);
            this.olaPntAval.TabIndex = 38;
            this.olaPntAval.Text = "Points Avaliable";
            this.olaPntAval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // opnBottom
            // 
            this.opnBottom.BackColor = System.Drawing.Color.LightGray;
            this.opnBottom.Location = new System.Drawing.Point(2, 549);
            this.opnBottom.Name = "opnBottom";
            this.opnBottom.Size = new System.Drawing.Size(992, 47);
            this.opnBottom.TabIndex = 40;
            // 
            // ogdCstDetail
            // 
            this.ogdCstDetail.AllowEditing = false;
            this.ogdCstDetail.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.None;
            this.ogdCstDetail.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.ogdCstDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ogdCstDetail.AutoResize = true;
            this.ogdCstDetail.ColumnInfo = resources.GetString("ogdCstDetail.ColumnInfo");
            this.ogdCstDetail.ExtendLastCol = true;
            this.ogdCstDetail.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None;
            this.ogdCstDetail.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ogdCstDetail.Location = new System.Drawing.Point(2, 191);
            this.ogdCstDetail.Margin = new System.Windows.Forms.Padding(4);
            this.ogdCstDetail.Name = "ogdCstDetail";
            this.ogdCstDetail.Rows.Count = 1;
            this.ogdCstDetail.Rows.MaxSize = 40;
            this.ogdCstDetail.Rows.MinSize = 40;
            this.ogdCstDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ogdCstDetail.Size = new System.Drawing.Size(992, 357);
            this.ogdCstDetail.StyleInfo = resources.GetString("ogdCstDetail.StyleInfo");
            this.ogdCstDetail.TabIndex = 39;
            this.ogdCstDetail.UseCompatibleTextRendering = true;
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitleCstSearch);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Controls.Add(this.ocmShwKb);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.ForeColor = System.Drawing.Color.White;
            this.opnHD.Location = new System.Drawing.Point(2, 2);
            this.opnHD.Margin = new System.Windows.Forms.Padding(4);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(992, 62);
            this.opnHD.TabIndex = 4;
            // 
            // olaTitleCstSearch
            // 
            this.olaTitleCstSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleCstSearch.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleCstSearch.ForeColor = System.Drawing.Color.White;
            this.olaTitleCstSearch.Location = new System.Drawing.Point(75, 0);
            this.olaTitleCstSearch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.olaTitleCstSearch.Name = "olaTitleCstSearch";
            this.olaTitleCstSearch.Size = new System.Drawing.Size(704, 62);
            this.olaTitleCstSearch.TabIndex = 7;
            this.olaTitleCstSearch.Text = "Search Customer List";
            this.olaTitleCstSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // ocmShwKb
            // 
            this.ocmShwKb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmShwKb.FlatAppearance.BorderSize = 0;
            this.ocmShwKb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmShwKb.Image = global::AdaPos.Properties.Resources.KBB_32;
            this.ocmShwKb.Location = new System.Drawing.Point(850, 0);
            this.ocmShwKb.Margin = new System.Windows.Forms.Padding(4);
            this.ocmShwKb.Name = "ocmShwKb";
            this.ocmShwKb.Size = new System.Drawing.Size(67, 62);
            this.ocmShwKb.TabIndex = 6;
            this.ocmShwKb.UseVisualStyleBackColor = true;
            this.ocmShwKb.Visible = false;
            // 
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.Location = new System.Drawing.Point(925, 0);
            this.ocmAccept.Margin = new System.Windows.Forms.Padding(4);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Size = new System.Drawing.Size(67, 62);
            this.ocmAccept.TabIndex = 5;
            this.ocmAccept.UseVisualStyleBackColor = true;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // wCstDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1000, 601);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "wCstDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wCstDetail";
            this.opnPage.ResumeLayout(false);
            this.opnTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ogdCstDetail)).EndInit();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Label olaPoint;
        private System.Windows.Forms.Label olaName;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Label olaTitleCstSearch;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Button ocmShwKb;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Label olaPntAval;
        private System.Windows.Forms.Label olaCstCode;
        private C1.Win.C1FlexGrid.C1FlexGrid ogdCstDetail;
        private System.Windows.Forms.Label olaTableTitlt;
        private System.Windows.Forms.Panel opnBottom;
        private System.Windows.Forms.Panel opnTop;
    }
}