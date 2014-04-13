using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication3
{
  public partial class Form1 : Form
  {
      DataSet ds_test = new DataSet();
      DataSet ds_data = new DataSet();
      public Form1()
      {
        InitializeComponent();
      }
      private void button1_Click(object sender, EventArgs e)
      {
        if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
          //DataSet ds = new DataSet();
          ds_test.ReadXml(openFileDialog1.FileName);
          dataGridView1.DataSource = ds_test.Tables[0];
        }
      }

      private void button2_Click(object sender, EventArgs e)
      {
        dataGridView3.RowCount = dataGridView2.Rows.Count;
        dataGridView3.ColumnCount = 2;
        for (int j1 = 0; j1 < dataGridView3.RowCount; j1++) {
          int sum = 0;
          for (int i1 = 1; i1 < dataGridView2.Columns.Count; i1++) {
            if (Convert.ToInt32(dataGridView1.Rows[0].Cells[i1-1].Value) == 1) {
              sum = sum + Convert.ToInt32(dataGridView2.Rows[j1].Cells[i1].Value);
            } else {
              sum = sum + (100 - Convert.ToInt32(dataGridView2.Rows[j1].Cells[i1].Value));
            }
          }
          sum = sum / dataGridView1.Columns.Count;
          dataGridView3.Rows[j1].Cells[0].Value = dataGridView2.Rows[j1].Cells[0].Value;
          dataGridView3.Rows[j1].Cells[1].Value = Convert.ToInt32(sum);
        }
      }

      private void button3_Click(object sender, EventArgs e)
      {
        if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
          ds_data.ReadXml(openFileDialog2.FileName);
          dataGridView2.DataSource = ds_data.Tables[0];
          dataGridView2.Columns[0].Width = 160;
        }
      }

      private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
      {
        ds_test.AcceptChanges();
        ds_test.WriteXml(openFileDialog1.FileName);
      }

      private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
      {
        ds_data.AcceptChanges();
        ds_data.WriteXml(openFileDialog2.FileName);
      }
  }
}
