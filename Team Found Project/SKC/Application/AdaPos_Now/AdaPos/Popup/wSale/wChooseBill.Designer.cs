namespace AdaPos.Popup.wSale
{
    partial class wChooseBill
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.opnPage = new System.Windows.Forms.Panel();
            this.ogdPerson = new System.Windows.Forms.DataGridView();
            this.ockTitleChoose = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.otbTitleDocNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleDatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitlePos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otbTitleAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.opnHD = new System.Windows.Forms.Panel();
            this.olaTitleAbout = new System.Windows.Forms.Label();
            this.ocmAccept = new System.Windows.Forms.Button();
            this.ocmBack = new System.Windows.Forms.Button();
            this.opnPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ogdPerson)).BeginInit();
            this.opnHD.SuspendLayout();
            this.SuspendLayout();
            // 
            // opnPage
            // 
            this.opnPage.BackColor = System.Drawing.Color.White;
            this.opnPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opnPage.Controls.Add(this.ogdPerson);
            this.opnPage.Controls.Add(this.opnHD);
            this.opnPage.Location = new System.Drawing.Point(164, 149);
            this.opnPage.Name = "opnPage";
            this.opnPage.Padding = new System.Windows.Forms.Padding(1);
            this.opnPage.Size = new System.Drawing.Size(700, 450);
            this.opnPage.TabIndex = 0;
            // 
            // ogdPerson
            // 
            this.ogdPerson.AllowUserToAddRows = false;
            this.ogdPerson.AllowUserToDeleteRows = false;
            this.ogdPerson.AllowUserToResizeColumns = false;
            this.ogdPerson.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ogdPerson.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ogdPerson.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ogdPerson.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ogdPerson.BackgroundColor = System.Drawing.Color.White;
            this.ogdPerson.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdPerson.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ogdPerson.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ogdPerson.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ockTitleChoose,
            this.otbTitleDocNo,
            this.otbTitleDatetime,
            this.otbTitlePos,
            this.otbTitleAmount});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ogdPerson.DefaultCellStyle = dataGridViewCellStyle5;
            this.ogdPerson.EnableHeadersVisualStyles = false;
            this.ogdPerson.GridColor = System.Drawing.Color.Gainsboro;
            this.ogdPerson.Location = new System.Drawing.Point(21, 71);
            this.ogdPerson.MultiSelect = false;
            this.ogdPerson.Name = "ogdPerson";
            this.ogdPerson.RowHeadersVisible = false;
            this.ogdPerson.RowTemplate.Height = 40;
            this.ogdPerson.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ogdPerson.ShowCellErrors = false;
            this.ogdPerson.ShowCellToolTips = false;
            this.ogdPerson.ShowEditingIcon = false;
            this.ogdPerson.ShowRowErrors = false;
            this.ogdPerson.Size = new System.Drawing.Size(656, 356);
            this.ogdPerson.TabIndex = 24;
            // 
            // ockTitleChoose
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.NullValue = false;
            this.ockTitleChoose.DefaultCellStyle = dataGridViewCellStyle3;
            this.ockTitleChoose.FillWeight = 40F;
            this.ockTitleChoose.HeaderText = "";
            this.ockTitleChoose.Name = "ockTitleChoose";
            this.ockTitleChoose.ReadOnly = true;
            this.ockTitleChoose.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // otbTitleDocNo
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.otbTitleDocNo.DefaultCellStyle = dataGridViewCellStyle4;
            this.otbTitleDocNo.HeaderText = "Document No.";
            this.otbTitleDocNo.Name = "otbTitleDocNo";
            this.otbTitleDocNo.ReadOnly = true;
            this.otbTitleDocNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitleDatetime
            // 
            this.otbTitleDatetime.FillWeight = 80F;
            this.otbTitleDatetime.HeaderText = "Date and time";
            this.otbTitleDatetime.Name = "otbTitleDatetime";
            this.otbTitleDatetime.ReadOnly = true;
            this.otbTitleDatetime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitlePos
            // 
            this.otbTitlePos.FillWeight = 60F;
            this.otbTitlePos.HeaderText = "POS";
            this.otbTitlePos.Name = "otbTitlePos";
            this.otbTitlePos.ReadOnly = true;
            this.otbTitlePos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otbTitleAmount
            // 
            this.otbTitleAmount.FillWeight = 60F;
            this.otbTitleAmount.HeaderText = "Amount";
            this.otbTitleAmount.Name = "otbTitleAmount";
            this.otbTitleAmount.ReadOnly = true;
            this.otbTitleAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // opnHD
            // 
            this.opnHD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.opnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.opnHD.Controls.Add(this.olaTitleAbout);
            this.opnHD.Controls.Add(this.ocmAccept);
            this.opnHD.Controls.Add(this.ocmBack);
            this.opnHD.Location = new System.Drawing.Point(1, 1);
            this.opnHD.Name = "opnHD";
            this.opnHD.Size = new System.Drawing.Size(696, 50);
            this.opnHD.TabIndex = 1;
            // 
            // olaTitleAbout
            // 
            this.olaTitleAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olaTitleAbout.BackColor = System.Drawing.Color.Transparent;
            this.olaTitleAbout.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Bold);
            this.olaTitleAbout.ForeColor = System.Drawing.Color.White;
            this.olaTitleAbout.Location = new System.Drawing.Point(53, 0);
            this.olaTitleAbout.Name = "olaTitleAbout";
            this.olaTitleAbout.Size = new System.Drawing.Size(293, 50);
            this.olaTitleAbout.TabIndex = 9;
            this.olaTitleAbout.Text = "Refer to bill by wristband";
            this.olaTitleAbout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ocmAccept
            // 
            this.ocmAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmAccept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmAccept.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmAccept.FlatAppearance.BorderSize = 0;
            this.ocmAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmAccept.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmAccept.ForeColor = System.Drawing.Color.White;
            this.ocmAccept.Image = global::AdaPos.Properties.Resources.Accept_32;
            this.ocmAccept.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmAccept.Location = new System.Drawing.Point(650, 0);
            this.ocmAccept.Margin = new System.Windows.Forms.Padding(0);
            this.ocmAccept.Name = "ocmAccept";
            this.ocmAccept.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ocmAccept.Size = new System.Drawing.Size(50, 50);
            this.ocmAccept.TabIndex = 8;
            this.ocmAccept.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmAccept.UseVisualStyleBackColor = false;
            this.ocmAccept.Click += new System.EventHandler(this.ocmAccept_Click);
            // 
            // ocmBack
            // 
            this.ocmBack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ocmBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.ocmBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ocmBack.FlatAppearance.BorderSize = 0;
            this.ocmBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ocmBack.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.ocmBack.ForeColor = System.Drawing.Color.White;
            this.ocmBack.Image = global::AdaPos.Properties.Resources.BackW_32;
            this.ocmBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmBack.Location = new System.Drawing.Point(0, 0);
            this.ocmBack.Margin = new System.Windows.Forms.Padding(0);
            this.ocmBack.Name = "ocmBack";
            this.ocmBack.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ocmBack.Size = new System.Drawing.Size(50, 50);
            this.ocmBack.TabIndex = 7;
            this.ocmBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ocmBack.UseVisualStyleBackColor = false;
            this.ocmBack.Click += new System.EventHandler(this.ocmBack_Click);
            // 
            // wChooseBill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.opnPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "wChooseBill";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.SystemColors.ActiveBorder;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.opnPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ogdPerson)).EndInit();
            this.opnHD.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel opnPage;
        private System.Windows.Forms.Panel opnHD;
        private System.Windows.Forms.Button ocmAccept;
        private System.Windows.Forms.Button ocmBack;
        private System.Windows.Forms.Label olaTitleAbout;
        private System.Windows.Forms.DataGridView ogdPerson;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ockTitleChoose;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleDocNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleDatetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitlePos;
        private System.Windows.Forms.DataGridViewTextBoxColumn otbTitleAmount;
    }
}