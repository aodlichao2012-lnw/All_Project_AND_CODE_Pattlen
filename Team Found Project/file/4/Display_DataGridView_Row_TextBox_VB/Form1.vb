Public Class Form1

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn() {New DataColumn("Id", GetType(Integer)), _
                                               New DataColumn("Name", GetType(String)), _
                                               New DataColumn("Country", GetType(String))})
        dt.Rows.Add(1, "John Hammond", "United States")
        dt.Rows.Add(2, "Mudassar Khan", "India")
        dt.Rows.Add(3, "Suzanne Mathews", "France")
        dt.Rows.Add(4, "Robert Schidner", "Russia")
        Me.dataGridView1.DataSource = dt
        Me.dataGridView1.AllowUserToAddRows = False
    End Sub

    Private Sub dataGridView1_CellMouseClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dataGridView1.CellMouseClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dataGridView1.Rows(e.RowIndex)
            txtID.Text = row.Cells(0).Value.ToString
            txtName.Text = row.Cells(1).Value.ToString
            txtCountry.Text = row.Cells(2).Value.ToString
        End If
    End Sub
End Class
