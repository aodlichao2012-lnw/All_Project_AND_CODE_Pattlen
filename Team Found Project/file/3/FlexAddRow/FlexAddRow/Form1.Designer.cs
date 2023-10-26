namespace FlexAddRow
{
  partial class Form1
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
      this.c1FlexGrid1 = new C1.Win.C1FlexGrid.C1FlexGrid();
      this.c1FlexGrid2 = new C1.Win.C1FlexGrid.C1FlexGrid();
      this.buttonAddRow = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.c1FlexGrid1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.c1FlexGrid2)).BeginInit();
      this.SuspendLayout();
      // 
      // c1FlexGrid1
      // 
      this.c1FlexGrid1.ColumnInfo = "10,1,0,0,0,95,Columns:";
      this.c1FlexGrid1.Location = new System.Drawing.Point(15, 56);
      this.c1FlexGrid1.Name = "c1FlexGrid1";
      this.c1FlexGrid1.Rows.Count = 10;
      this.c1FlexGrid1.Rows.DefaultSize = 19;
      this.c1FlexGrid1.Size = new System.Drawing.Size(288, 391);
      this.c1FlexGrid1.TabIndex = 0;
      // 
      // c1FlexGrid2
      // 
      this.c1FlexGrid2.AllowAddNew = true;
      this.c1FlexGrid2.ColumnInfo = "10,1,0,0,0,95,Columns:";
      this.c1FlexGrid2.Location = new System.Drawing.Point(328, 56);
      this.c1FlexGrid2.Name = "c1FlexGrid2";
      this.c1FlexGrid2.Rows.Count = 10;
      this.c1FlexGrid2.Rows.DefaultSize = 19;
      this.c1FlexGrid2.Size = new System.Drawing.Size(268, 390);
      this.c1FlexGrid2.TabIndex = 1;
      // 
      // buttonAddRow
      // 
      this.buttonAddRow.Location = new System.Drawing.Point(228, 453);
      this.buttonAddRow.Name = "buttonAddRow";
      this.buttonAddRow.Size = new System.Drawing.Size(75, 23);
      this.buttonAddRow.TabIndex = 2;
      this.buttonAddRow.Text = "AddRow";
      this.buttonAddRow.UseVisualStyleBackColor = true;
      this.buttonAddRow.Click += new System.EventHandler(this.buttonAddRow_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(325, 31);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(113, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Add row automatically:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 31);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(93, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Add row manually:";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(635, 486);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.buttonAddRow);
      this.Controls.Add(this.c1FlexGrid2);
      this.Controls.Add(this.c1FlexGrid1);
      this.Name = "Form1";
      this.Text = "Form1";
      ((System.ComponentModel.ISupportInitialize)(this.c1FlexGrid1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.c1FlexGrid2)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private C1.Win.C1FlexGrid.C1FlexGrid c1FlexGrid1;
    private C1.Win.C1FlexGrid.C1FlexGrid c1FlexGrid2;
    private System.Windows.Forms.Button buttonAddRow;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
  }
}

