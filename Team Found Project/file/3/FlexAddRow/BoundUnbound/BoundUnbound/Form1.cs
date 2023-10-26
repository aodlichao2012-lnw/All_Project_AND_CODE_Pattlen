using C1.Win.C1FlexGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoundUnbound
{
    public partial class Form1 : Form
    {

        string connectionString = String.Format(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString + "", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
        OleDbCommand command = new OleDbCommand();
        DataTable d = new DataTable();
        string query = ConfigurationManager.ConnectionStrings["selectQuery"].ConnectionString;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                //Connection and Data Binding
                connection.Open();
                command = new OleDbCommand(query, connection);
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(d);
                c1FlexGrid1.DataSource = d;
                connection.Close();
            }

            d.Columns.Add("Value1", typeof(decimal), "HP*2");
            d.Columns.Add("Value2", typeof(decimal), "HP*3");

            Column col = c1FlexGrid1.Cols.Add();
            col.Name = col.Caption = "Extra One";

            col = c1FlexGrid1.Cols.Add();
            col.Name = col.Caption = "Extra Two";

            c1FlexGrid1.GetUnboundValue += C1FlexGrid1_GetUnboundValue;

        }

        private void C1FlexGrid1_GetUnboundValue(object sender, UnboundValueEventArgs e)
        {
            if (e.Col == 18)
            e.Value = Convert.ToInt32(c1FlexGrid1[e.Row, 4]) * 2;
            if(e.Col == 19)
            e.Value = Convert.ToInt32(c1FlexGrid1[e.Row, 4]) * 3;
        }



    }
}
