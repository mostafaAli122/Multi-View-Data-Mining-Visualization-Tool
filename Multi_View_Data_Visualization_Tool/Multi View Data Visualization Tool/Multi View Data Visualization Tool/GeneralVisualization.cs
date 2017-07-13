using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
namespace Multi_View_Data_Visualization_Tool
{
    public partial class GeneralVisualization : Form
    {
        Histogram h;
        PieChart p;
        Chart pie;
        Chart column;
        Chart hist;
        Color[] colors;
        DataTable datatable = null;
        string[] col_names;

        string filename;
        public GeneralVisualization()
        {
            InitializeComponent();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            string file1;
            string Line;
            listView1.Columns.Clear();
            listView1.Items.Clear();
            StreamReader tr;
            DataTable dt;
            DialogResult Result = openFileDialog1.ShowDialog();
            if (Result == DialogResult.OK)
            {
                groupBox3.Visible = false;
                groupBox1.Visible = false;
                Visualize_btn.Enabled = false;
                groupBox2.Visible = false;
                charts_comboBox.SelectedIndex = -1;
                Choose_btn.Enabled = false;

                string[] items;
                file1 = openFileDialog1.FileName;
                try
                {
                    dt = new DataTable();
                    tr = File.OpenText(file1);
                    Line = tr.ReadLine();
                    col_names = Line.Split('\t');
                    while ((Line = tr.ReadLine()) != null)
                    {
                        items = Line.Split('\t');
                        if (dt.Columns.Count == 0)
                        {
                            for (int i = 0; i < items.Length; i++)
                            {
                                if (items[i].GetType() != typeof(string))
                                    dt.Columns.Add(col_names[i], typeof(double));
                                else
                                    dt.Columns.Add(col_names[i], typeof(string));
                                listView1.Columns.Add(col_names[i]);
                            }
                        }
                        ListViewItem l = new ListViewItem(items);
                        listView1.Items.Add(l);
                        dt.Rows.Add(items);
                    }
                    filename = file1;
                    datatable = dt;
                    groupBox2.Visible = true;
                    groupBox3.Visible = true;
                    //p = new PieChart();
                    //p.fill_table();
                    //comboBox1.Items.AddRange(p.get_column_names());
                    comboBox1.Items.Clear();
                }
                catch { }
              
            }

        }

        private void GeneralVisualization_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text File (*.txt)|*.txt";
            groupBox3.Visible = false;
            groupBox1.Visible = false;
            Choose_btn.Enabled = false;
            Visualize_btn.Enabled = false;
            groupBox2.Visible = false;
        }
        private void generate_random_colors()
        {
            int j = 1;
            int z = 0;
            for (int i = 0; i < colors.Length; i++)
            {
                bool found = false;
                Color t = Color.FromArgb((i * j) % 256, (j * 30) % 256, (i * 50) % 256);

                for (int k = 0; k < z; k++)
                {
                    if (colors[k] == t)
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    colors[i] = t;
                    z++;
                }
                else
                    i--;
                j += 100;
            }
        }
        private void Choose_btn_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            int choosen_tech = charts_comboBox.SelectedIndex;
            groupBox1.Visible= true;
            comboBox1.Items.Clear();
            comboBox1.Text = "";
            comboBox1.Items.AddRange(col_names);
            Visualize_btn.Enabled = false;
            if (choosen_tech == 0)
            {
                label1.Text = "Choose Column";
            }
            else if (choosen_tech == 1)
            {
                label1.Text = "Choose Column";
                //generate_Piechart();
            }
            else if (choosen_tech == 2)
            {
                label1.Text = "Choose (X) axis";
                comboBox1.Items.Insert(0, "Default");
               // generate_columnchart();
            }

            else
                groupBox1.Visible = false;

        }
        private void charts_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Choose_btn.Enabled = true;
            groupBox1.Visible = false;
            panel1.Controls.Clear();
            comboBox1.Text = "";
            label2.Text = "";
           
        }
      
        private void Visualize_btn_Click(object sender, EventArgs e)
        {
            // Histogram
            if (charts_comboBox.SelectedIndex == 0)
            {
                generate_histogram();
                label2.Text = "Histogram For " + comboBox1.SelectedItem.ToString();
                h = new Histogram(datatable);
                hist.ChartAreas[0].AxisX.Title = comboBox1.SelectedItem.ToString();
                hist.Series[0].Points.Clear();
               
                Bin_data[] bin = h.get_Bins(comboBox1.SelectedItem.ToString());
                if (bin != null)
                {
                    colors = new Color[bin.Length];
                    generate_random_colors();
                    int i = 0;
                    foreach (Bin_data g in bin)
                    {
                        hist.Series[0].Points.AddXY(g.minimum.ToString() + " : " + g.maximum.ToString(), g.frequency);
                        hist.Series[0].Points[i].Color = colors[i];
                        i++;
                    }
                }
                else {
                    panel1.Controls.Clear();
                    label2.Text = "";
                    MessageBox.Show("Data Must be Numeric");
                }
            }
            // pie chart
            if (charts_comboBox.SelectedIndex == 1)
            {
                generate_Piechart();
                label2.Text = "Pie Chart For " + comboBox1.SelectedItem.ToString();
                if (comboBox1.SelectedIndex != -1)
                {
                     p = new PieChart(datatable);

                    pie.Series[0].Points.Clear();
                    PieChart_Data[] pc = p.count_data(comboBox1.SelectedItem.ToString());
                    colors = new Color[pc.Length];
                    generate_random_colors();
                    int i = 0;
                    foreach (PieChart_Data t in pc)
                    {
                        pie.Series[0].Points.AddXY(t.bin_name, t.frequency);
                        pie.Series[0].Points[i].Color = colors[i];
                        i++;
                    }
                }
            }
                // column chart
            else if (charts_comboBox.SelectedIndex == 2)
            {
                generate_columnchart();
                int selected_index = comboBox1.SelectedIndex;
               column.Legends.Clear();
                column.Series.Clear();

                Legend l = new Legend();
                column.Legends.Add(l);
                if (selected_index == 0)
                {
                    string n = filename.Substring(filename.LastIndexOf("\\")+1);
                    string[] n1 = n.Split('.');
                    label2.Text = "Column Chart For " +n1[0];
                    for (int i = 0; i < datatable.Columns.Count; i++)
                    {
                        Series s = new Series(col_names[i]);
                       column.Series.Add(s);
                        for (int j = 0; j < datatable.Rows.Count; j++)
                        {
                            DataRow d = datatable.Rows[j];
                            column.Series[i].Points.AddXY(j, d[i]);
                        }
                    }
                }
                else if (selected_index > 0)
                {
                    label2.Text = "Column Chart For " + comboBox1.SelectedItem.ToString();
                    selected_index--;
                    for (int i = 0; i < datatable.Columns.Count; i++)
                    {
                        int t = i;
                        if (i == selected_index)
                            continue;
                        if (i > selected_index)
                            t--;
                        Series s = new Series(col_names[i]);
                       //s.IsXValueIndexed = true;
                        column.Series.Add(s);
                        for (int j = 0; j < datatable.Rows.Count; j++)
                        {
                            DataRow d = datatable.Rows[j];
                            column.Series[t].Points.AddXY(d[selected_index], d[i]);
                        }
                    }
                }
            }
        }
       // Pie Chart functions
        private void generate_Piechart()
        {
            panel1.Controls.Clear();
            pie = new Chart();
            ChartArea chartArea1 = new ChartArea();  
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Angle = 90;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 100F;
            chartArea1.Position.Width = 100F;
           pie.ChartAreas.Add(chartArea1);
           pie.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
           Series series1 = new Series();
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.CustomProperties = "PieLineColor=Red";
            series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            series1.IsVisibleInLegend = false;
            series1.Label = "(#VALY) #VALX";
            series1.LabelToolTip = "#PERCENT";
            series1.Name = "Series1";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
           pie.Series.Add(series1);
           pie.Size = new System.Drawing.Size(334, 322);
            pie.TabIndex = 1;
            pie.MouseMove += chart_MouseMove;
            pie.MouseLeave += chart_MouseLeave;

            panel1.Controls.Add(pie);
            panel1.Controls[0].Location = new Point(100, 0);
        }
        private void change_color( Chart t)
        {
            try
            {
                for (int i = 0; i < t.Series[0].Points.Count; i++)
                {
                    t.Series[0].Points[i].Color = colors[i];
                }
            }
            catch(Exception ex)
            {}
        }
        private void chart_MouseMove(object sender, MouseEventArgs e)
        {
            change_color(pie);
            int index = pie.HitTest(e.X, e.Y).PointIndex;
            if (index >= 0)
            {
                pie.Series[0].Points[index].Color = Color.Red;
            }
        }
        private void chart_MouseLeave(object sender, EventArgs e)
        {
            change_color(pie);
        }
        ////////////////////////////////////////////////////////////////////////////////
        // column chart functions 
        private void generate_columnchart()
        {
            panel1.Controls.Clear();
            column = new Chart();
            ChartArea chartArea3 = new ChartArea();
            chartArea3.AxisX.IsLabelAutoFit = true ;
            chartArea3.AxisX.LabelStyle.Angle = 90;
            chartArea3.AxisX.MajorGrid.Enabled = false;
            column.ChartAreas.Add(chartArea3);
            column.Size = new System.Drawing.Size(540, 333);
           column.TabIndex = 0;
           column.MouseMove += chart1_MouseMove;
           panel1.Controls.Add(column);
        }
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            // not implemented  yet 
           
        }
        ////////////////////////////////////////////////////////
        // histogram functions 
        private void generate_histogram()
        {
            panel1.Controls.Clear();
            hist = new Chart();
            ChartArea chartArea2 = new ChartArea();
            chartArea2.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea2.AxisX.IsLabelAutoFit = false;
            chartArea2.AxisX.IsMarginVisible = false;
            chartArea2.AxisX.LabelStyle.Angle = 90;
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisY.MajorGrid.Enabled = false;
            chartArea2.Position.Auto = false;
            chartArea2.Position.Height = 100F;
            chartArea2.Position.Width = 100F;
            chartArea2.AxisY.Title = "Frequency";
            hist.ChartAreas.Add(chartArea2);
            Series series2 = new Series();
            series2.Color = System.Drawing.Color.Gray;
            series2.CustomProperties = "DrawSideBySide=True, DrawingStyle=Cylinder, PointWidth=1";
            series2.IsXValueIndexed = true;
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
           hist.Series.Add(series2);
           hist.Size = new System.Drawing.Size(450, 350);
           hist.MouseLeave += histogram_MouseLeave;
           hist.MouseMove += histogram_MouseMove;
           panel1.Controls.Add(hist);
           panel1.Controls[0].Location = new Point(50, 0);
        }
        private void histogram_MouseMove(object sender, MouseEventArgs e)
        {
            change_color(hist);
            int index = hist.HitTest(e.X, e.Y).PointIndex;
            if (e.X >= 50 && e.X < 450 && e.Y >= 10 && e.Y <= 300)
                if (index >= 0)
                {
                    hist.Series[0].Points[index].Color = Color.Red;

                }
        }
        private void histogram_MouseLeave(object sender, EventArgs e)
        {
            change_color(hist);

        }
        ///////////////////////////////////////////////////////////////////////////////
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Visualize_btn.Enabled = true;
            panel1.Controls.Clear();
            label2.Text = "";
            
        }

        private void Exit_File_btn_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void menuItem2_Click_1(object sender, EventArgs e)
        {
            string s = "General Visualization";
            Help h = new Help(s.ToString());
            h.Show();
        }
    }
}
