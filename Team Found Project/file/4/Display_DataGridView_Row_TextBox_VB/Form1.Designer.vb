<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtCountry = New System.Windows.Forms.TextBox()
        Me.lblCountry = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.label1 = New System.Windows.Forms.Label()
        Me.dataGridView1 = New System.Windows.Forms.DataGridView()
        CType(Me.dataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtCountry
        '
        Me.txtCountry.Location = New System.Drawing.Point(83, 252)
        Me.txtCountry.Name = "txtCountry"
        Me.txtCountry.Size = New System.Drawing.Size(225, 22)
        Me.txtCountry.TabIndex = 13
        '
        'lblCountry
        '
        Me.lblCountry.AutoSize = True
        Me.lblCountry.Location = New System.Drawing.Point(19, 253)
        Me.lblCountry.Name = "lblCountry"
        Me.lblCountry.Size = New System.Drawing.Size(61, 17)
        Me.lblCountry.TabIndex = 12
        Me.lblCountry.Text = "Country:"
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(84, 217)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(225, 22)
        Me.txtName.TabIndex = 11
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(19, 221)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(49, 17)
        Me.lblName.TabIndex = 10
        Me.lblName.Text = "Name:"
        '
        'txtID
        '
        Me.txtID.Location = New System.Drawing.Point(85, 182)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(225, 22)
        Me.txtID.TabIndex = 9
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(23, 185)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(25, 17)
        Me.label1.TabIndex = 8
        Me.label1.Text = "ID:"
        '
        'dataGridView1
        '
        Me.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridView1.Location = New System.Drawing.Point(19, 15)
        Me.dataGridView1.Name = "dataGridView1"
        Me.dataGridView1.RowTemplate.Height = 24
        Me.dataGridView1.Size = New System.Drawing.Size(457, 146)
        Me.dataGridView1.TabIndex = 7
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(488, 288)
        Me.Controls.Add(Me.txtCountry)
        Me.Controls.Add(Me.lblCountry)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.txtID)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.dataGridView1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.dataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents txtCountry As System.Windows.Forms.TextBox
    Private WithEvents lblCountry As System.Windows.Forms.Label
    Private WithEvents txtName As System.Windows.Forms.TextBox
    Private WithEvents lblName As System.Windows.Forms.Label
    Private WithEvents txtID As System.Windows.Forms.TextBox
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents dataGridView1 As System.Windows.Forms.DataGridView

End Class
