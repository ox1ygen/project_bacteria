using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;


namespace WindowsFormsApplication3
{

  public partial class Form1 : Form
  {
      
      int min_por = 30;
      int max_por = 70;
      DataSet ds_test = new DataSet();
      DataSet ds_data = new DataSet();

      DataSet ds_division = new DataSet();
      DataSet ds_cluster = new DataSet();


      
      public Form1()
      {
        InitializeComponent();
      }

      private void button1_Click(object sender, EventArgs e)
      {
        if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
          //DataSet ds = new DataSet();
          ds_test.Clear();
          ds_test.ReadXml(openFileDialog1.FileName);
          dataGridView1.DataSource = ds_test.Tables[0];
        }
      }

      private void button2_Click(object sender, EventArgs e)
      {

        if (dataGridView2.AllowUserToAddRows) {
          dataGridView3.RowCount = dataGridView2.Rows.Count-1;
        } else {
          dataGridView3.RowCount = dataGridView2.Rows.Count;
        }
        dataGridView3.ColumnCount = 3;
        for (int j1 = 0; j1 < dataGridView3.RowCount; j1++)
        {
            int bad_test = 0;
            int sum = 0;
            for (int i1 = 1; i1 < dataGridView2.Columns.Count; i1++)
            {
                if (Convert.ToString(dataGridView1.Rows[0].Cells[i1 - 1].Value) == Convert.ToString(1))
                {
                    sum = sum + Convert.ToInt32(dataGridView2.Rows[j1].Cells[i1].Value);
                    if (Convert.ToInt32(dataGridView2.Rows[j1].Cells[i1].Value) < max_por)
                        bad_test++;
                }
                if (Convert.ToString(dataGridView1.Rows[0].Cells[i1 - 1].Value) == Convert.ToString(0))
                {
                    sum = sum + (100 - Convert.ToInt32(dataGridView2.Rows[j1].Cells[i1].Value));
                    if (Convert.ToInt32(dataGridView2.Rows[j1].Cells[i1].Value) > min_por)
                        bad_test++;
                }
            }
            dataGridView3.Rows[j1].Cells[0].Value = dataGridView2.Rows[j1].Cells[0].Value;
            dataGridView3.Rows[j1].Cells[1].Value = Convert.ToInt32(sum);
            dataGridView3.Rows[j1].Cells[2].Value = Convert.ToInt32(bad_test);
        }
        sort(dataGridView3, 2, 1);
      }
      /*вспомогательная функция для сортировки в диапазоне от up до down в столбике col*/
      void ssort(DataGridView gri, int up, int down, int col)
      {
          for (int i = up; i < down; i++)
          {
              for (int j = up; j < down; j++)
              {
                  if (Convert.ToInt32(gri.Rows[j + 1].Cells[col].Value) > Convert.ToInt32(gri.Rows[j].Cells[col].Value))
                  {
                      int a = Convert.ToInt32(gri.Rows[j + 1].Cells[col].Value);
                      gri.Rows[j + 1].Cells[col].Value = gri.Rows[j].Cells[col].Value;
                      gri.Rows[j].Cells[col].Value = a;
                      String b = Convert.ToString(gri.Rows[j + 1].Cells[col-1].Value);
                      gri.Rows[j + 1].Cells[col-1].Value = gri.Rows[j].Cells[col-1].Value;
                      gri.Rows[j].Cells[col-1].Value = b;
                  }
              }
          }
      }
      /* глафная функция сортирует в датагриде gri сначала главный столбец a,  потом  другой столбец b*/
      void sort(DataGridView gri, int a, int b)
      {
          ListSortDirection direction = ListSortDirection.Ascending;
          gri.Sort(gri.Columns[a], direction);
          int up = 0;
          int down = 0;
          for (int i = 0; i < gri.RowCount - 1; i++)
          {
              if (Convert.ToInt32(gri.Rows[i].Cells[a].Value) != Convert.ToInt32(gri.Rows[i + 1].Cells[a].Value))
              {
                  down = i;
                  ssort(gri, up, down, b);
                  up = i + 1;
              }
          }
      }
      private void button3_Click(object sender, EventArgs e)
      {

        if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
          ds_data.Clear();
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

      private void Form1_Load(object sender, EventArgs e)
      {
        var sw = new Stopwatch();
        sw.Start();
        openFileDialog1.FileName = "xml_test_file.xml";
        ds_test.ReadXml(openFileDialog1.FileName);
        dataGridView1.DoubleBuffered(true);
        dataGridView1.DataSource = ds_test.Tables[0];

        openFileDialog2.FileName = "xml_data_file.xml";
        ds_data.ReadXml(openFileDialog2.FileName);
        dataGridView2.DoubleBuffered(true);
        dataGridView2.DataSource = ds_data.Tables[0];
        dataGridView2.Columns[0].Width = 160;

        ds_cluster.ReadXml("xml_data_cluster.xml");
        dataGridView4.DoubleBuffered(true);
        dataGridView5.DoubleBuffered(true);
        dataGridView6.DoubleBuffered(true);
        dataGridView4.DataSource = ds_cluster.Tables[0];
        ds_division.ReadXml("xml_division_cluster.xml");
        dataGridView5.DataSource = ds_division.Tables[0];
        sw.Stop();
      }


      private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
      {
        //int rowOfInterest = dataGridView2.CurrentCell.RowIndex;
        if (e.Control && e.KeyCode == Keys.C) {
          DataObject d = dataGridView2.GetClipboardContent();
          Clipboard.SetDataObject(d);
          e.Handled = true;
        } else if (e.Control && e.KeyCode == Keys.V) {
          string s = Clipboard.GetText();
          string[] lines = s.Split('\n');
          int row = dataGridView2.CurrentCell.RowIndex;
          int col = dataGridView2.CurrentCell.ColumnIndex;
          foreach (string line in lines) {
            if (row < dataGridView2.RowCount && line.Length >
                0) {
              string[] cells = line.Split('\t');
              for (int i = 0; i < cells.GetLength(0); ++i) {
                if (col + i <
                    this.dataGridView2.ColumnCount) {
                  dataGridView2[col + i, row].Value =
                    Convert.ChangeType(cells[i], dataGridView2[col + i, row].ValueType);
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


      private void средниеЗначенияToolStripMenuItem_Click(object sender, EventArgs e)
      {
        cluster_data_form f = new cluster_data_form();
        f.Show();
      }

      private void разбиениеНаКластерыToolStripMenuItem_Click(object sender, EventArgs e)
      {
        cluster_division_form f = new cluster_division_form();
        f.Show();
      }

      private void button4_Click(object sender, EventArgs e)
      {
        if (dataGridView4.AllowUserToAddRows) {
          dataGridView6.RowCount = dataGridView4.Columns.Count-1;
        } else {
          dataGridView6.RowCount = dataGridView4.Columns.Count;
        }
        dataGridView6.ColumnCount = 2;

        for (int j1 = 1; j1 < dataGridView4.Columns.Count; j1++) {
          double sum = 0;
          for (int i1 = 1; i1 < dataGridView4.Rows.Count; i1++) {
            if (Convert.ToString(dataGridView1.Rows[0].Cells[i1 - 1].Value) == Convert.ToString(1)) {
              sum = sum + Convert.ToDouble(dataGridView4.Rows[i1].Cells[j1].Value);
            }
            if (Convert.ToString(dataGridView1.Rows[0].Cells[i1 - 1].Value) == Convert.ToString(0)) {
              sum = sum + (100 - Convert.ToDouble(dataGridView4.Rows[i1].Cells[j1].Value));
            }
          }
          dataGridView6.Rows[j1-1].Cells[0].Value = j1;
          dataGridView6.Rows[j1-1].Cells[1].Value = Convert.ToInt32(sum);
        }
      }

      private void button5_Click(object sender, EventArgs e)
      {
          if (dataGridView6.RowCount == 0)
              return;
        dataGridView7.Rows.Clear();
        //  dataGridView7.RowCount = 6;
        dataGridView7.ColumnCount = 3;
        int counter = 0;
        int mode_cluster = -1;
        string _index ="";
        for (int i = 0; i < dataGridView6.RowCount; i++)
        {
            if (Convert.ToInt32(dataGridView6[1, i].Value) > mode_cluster)
            {
                mode_cluster = Convert.ToInt32(dataGridView6[1, i].Value);
                _index = Convert.ToString(dataGridView6[0, i].Value);
            }
        }
        for (int i = 0; i < dataGridView2.Rows.Count; i++) {
          double sum = 0;
          int not = 0;
              if (_index == Convert.ToString(dataGridView5.Rows[i].Cells["Cluster"].Value))
              {
                  for (int i1 = 1; i1 < dataGridView2.Columns.Count; i1++)
                  {
                      if (Convert.ToString(dataGridView1.Rows[0].Cells[i1 - 1].Value) == Convert.ToString(1))
                      {
                          if (Convert.ToDouble(dataGridView2.Rows[i].Cells[i1].Value) < max_por)
                          {
                              not++;
                          }
                          sum = sum + Convert.ToDouble(dataGridView2.Rows[i].Cells[i1].Value);
                      }
                      if (Convert.ToString(dataGridView1.Rows[0].Cells[i1 - 1].Value) == Convert.ToString(0))
                      {
                          if (Convert.ToDouble(dataGridView2.Rows[i].Cells[i1].Value) > min_por)
                          {
                              not++;
                          }
                          sum = sum + (100 - Convert.ToDouble(dataGridView2.Rows[i].Cells[i1].Value));
                      }
                  }
                  this.dataGridView7.Rows.Add(dataGridView2.Rows[i].Cells[0].Value, Convert.ToInt32(sum), Convert.ToInt32(not));
                  counter++;
              }

        }
      }

      private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
      {
        dataGridView1.ClearSelection();
        int a1 = dataGridView2.CurrentCell.RowIndex;
        int b1 = dataGridView2.CurrentCell.ColumnIndex;
  /*      for (int i = 0; i < a1; i++) {
          dataGridView2.Rows[i].Cells[b1].Selected = true;
        }
        for (int i = 0; i < b1; i++) {
          dataGridView2.Rows[a1].Cells[i].Selected = true;
        }*/
        if (b1 != 0)
        {
            dataGridView1.Rows[0].Cells[b1 - 1].Selected = true;
            dataGridView2.Rows[a1].Cells[0].Selected = true;
        }
      }

      private void dataGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
      {
          if (e.KeyCode == Keys.Delete)
          {
              dataGridView1.CurrentCell.Value = "";
          }
      }

  }
}
