namespace TestApp
{
    partial class wMain
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
            this.ocmStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.otbAppPath = new System.Windows.Forms.TextBox();
            this.otbParameter = new System.Windows.Forms.TextBox();
            this.otbKey = new System.Windows.Forms.TextBox();
            this.otbValueInput = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.otbValueOutput = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ocmEncrypt = new System.Windows.Forms.Button();
            this.ocmDecrypt = new System.Windows.Forms.Button();
            this.ocbBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ocmStart
            // 
            this.ocmStart.Location = new System.Drawing.Point(291, 146);
            this.ocmStart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ocmStart.Name = "ocmStart";
            this.ocmStart.Size = new System.Drawing.Size(82, 28);
            this.ocmStart.TabIndex = 0;
            this.ocmStart.Text = "Start";
            this.ocmStart.UseVisualStyleBackColor = true;
            this.ocmStart.Click += new System.EventHandler(this.ocmStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Path : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 83);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Parameter : ";
            // 
            // otbAppPath
            // 
            this.otbAppPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbAppPath.Location = new System.Drawing.Point(26, 35);
            this.otbAppPath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.otbAppPath.Name = "otbAppPath";
            this.otbAppPath.Size = new System.Drawing.Size(349, 20);
            this.otbAppPath.TabIndex = 3;
            this.otbAppPath.Tag = "";
            this.otbAppPath.Text = "E:\\02.Project\\11.AdaTask\\AdaTask\\AdaTAsk\\bin\\Debug\\AdaTAsk.exe";
            // 
            // otbParameter
            // 
            this.otbParameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbParameter.Location = new System.Drawing.Point(26, 107);
            this.otbParameter.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.otbParameter.Name = "otbParameter";
            this.otbParameter.Size = new System.Drawing.Size(349, 20);
            this.otbParameter.TabIndex = 4;
            this.otbParameter.Text = "/1 /*";
            // 
            // otbKey
            // 
            this.otbKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbKey.Location = new System.Drawing.Point(26, 240);
            this.otbKey.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.otbKey.Name = "otbKey";
            this.otbKey.Size = new System.Drawing.Size(349, 20);
            this.otbKey.TabIndex = 8;
            // 
            // otbValueInput
            // 
            this.otbValueInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbValueInput.Location = new System.Drawing.Point(26, 191);
            this.otbValueInput.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.otbValueInput.Name = "otbValueInput";
            this.otbValueInput.Size = new System.Drawing.Size(349, 20);
            this.otbValueInput.TabIndex = 7;
            this.otbValueInput.Tag = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 217);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Key :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 172);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Value input";
            // 
            // otbValueOutput
            // 
            this.otbValueOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otbValueOutput.Location = new System.Drawing.Point(26, 292);
            this.otbValueOutput.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.otbValueOutput.Name = "otbValueOutput";
            this.otbValueOutput.Size = new System.Drawing.Size(349, 20);
            this.otbValueOutput.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 269);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Value output";
            // 
            // ocmEncrypt
            // 
            this.ocmEncrypt.Location = new System.Drawing.Point(291, 325);
            this.ocmEncrypt.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ocmEncrypt.Name = "ocmEncrypt";
            this.ocmEncrypt.Size = new System.Drawing.Size(82, 28);
            this.ocmEncrypt.TabIndex = 11;
            this.ocmEncrypt.Text = "Encrypt";
            this.ocmEncrypt.UseVisualStyleBackColor = true;
            this.ocmEncrypt.Click += new System.EventHandler(this.ocmEncrypt_Click);
            // 
            // ocmDecrypt
            // 
            this.ocmDecrypt.Location = new System.Drawing.Point(204, 325);
            this.ocmDecrypt.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ocmDecrypt.Name = "ocmDecrypt";
            this.ocmDecrypt.Size = new System.Drawing.Size(82, 28);
            this.ocmDecrypt.TabIndex = 12;
            this.ocmDecrypt.Text = "Decrypt";
            this.ocmDecrypt.UseVisualStyleBackColor = true;
            this.ocmDecrypt.Click += new System.EventHandler(this.ocmDecrypt_Click);
            // 
            // ocbBrowse
            // 
            this.ocbBrowse.Location = new System.Drawing.Point(291, 65);
            this.ocbBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.ocbBrowse.Name = "ocbBrowse";
            this.ocbBrowse.Size = new System.Drawing.Size(82, 28);
            this.ocbBrowse.TabIndex = 13;
            this.ocbBrowse.Text = "Browse";
            this.ocbBrowse.UseVisualStyleBackColor = true;
            this.ocbBrowse.Click += new System.EventHandler(this.ocbBrowse_Click);
            // 
            // wMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 372);
            this.Controls.Add(this.ocbBrowse);
            this.Controls.Add(this.ocmDecrypt);
            this.Controls.Add(this.ocmEncrypt);
            this.Controls.Add(this.otbValueOutput);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.otbKey);
            this.Controls.Add(this.otbValueInput);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.otbParameter);
            this.Controls.Add(this.otbAppPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ocmStart);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "wMain";
            this.Text = "E:\\02.Project\\11.AdaTask\\AdaTask\\AdaTAsk\\bin\\Debug\\AdaTAsk.exe";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ocmStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox otbAppPath;
        private System.Windows.Forms.TextBox otbParameter;
        private System.Windows.Forms.TextBox otbKey;
        private System.Windows.Forms.TextBox otbValueInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox otbValueOutput;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ocmEncrypt;
        private System.Windows.Forms.Button ocmDecrypt;
        private System.Windows.Forms.Button ocbBrowse;
    }
}

