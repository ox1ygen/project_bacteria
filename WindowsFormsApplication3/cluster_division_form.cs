using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
  public partial class cluster_division_form : Form
  {
      public cluster_division_form()
      {
        InitializeComponent();
      }
      DataSet ds = new DataSet();
      private void cluster_division_Load(object sender, EventArgs e)
      {

        dataGridView1.DoubleBuffered(true);
        ds.ReadXml("xml_division_cluster.xml");
        dataGridView1.DataSource = ds.Tables[0];
        dataGridView1.Columns[0].Width = 160;
      }

      private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
      {
        //int rowOfInterest = dataGridView2.CurrentCell.RowIndex;
        if (e.Control && e.KeyCode == Keys.C) {
          DataObject d = dataGridView1.GetClipboardContent();
          Clipboard.SetDataObject(d);
          e.Handled = true;
        } else if (e.Control && e.KeyCode == Keys.V) {
          string s = Clipboard.GetText();
          string[] lines = s.Split('\n');
          int row = dataGridView1.CurrentCell.RowIndex;
          int col = dataGridView1.CurrentCell.ColumnIndex;
          foreach (string line in lines) {
            if (row < dataGridView1.RowCount && line.Length >
                0) {
              string[] cells = line.Split('\t');
              for (int i = 0; i < cells.GetLength(0); ++i) {
                if (col + i <
                    this.dataGridView1.ColumnCount) {
                  dataGridView1[col + i, row].Value =
                    Convert.ChangeType(cells[i], dataGridView1[col + i, row].ValueType);
                } else {
                  break;
                }
              }
              row++;
            } else {
              break;
            }
          }
        }
      }

      private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
      {
        ds.AcceptChanges();
        ds.WriteXml("xml_division_cluster.xml");
      }
  }
}
