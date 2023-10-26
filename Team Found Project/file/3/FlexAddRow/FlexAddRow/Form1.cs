using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlexAddRow
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();


      for (int r = 0; r < this.c1FlexGrid1.Rows.Count; r++)
      {
        for (int c =0; c< this.c1FlexGrid1.Cols.Count; c++)
        {
          this.c1FlexGrid1[r, c] = "cell " + r + "/" + c;
          this.c1FlexGrid2[r, c] = "cell " + r + "/" + c;
        }
      }
    }

    private void buttonAddRow_Click(object sender, EventArgs e)
    {
      this.c1FlexGrid1.Rows.Insert(this.c1FlexGrid1.Row);
    }
  }
}
