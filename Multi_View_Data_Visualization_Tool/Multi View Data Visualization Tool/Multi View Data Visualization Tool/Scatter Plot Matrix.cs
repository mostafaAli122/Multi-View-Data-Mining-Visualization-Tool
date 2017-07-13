using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
namespace Multi_View_Data_Visualization_Tool
{
    class Scatter_Plot_Matrix
    {
        DataTable dt;
        Chart[,] charts;
        public DataTable data;
        public Color[] clusters_color;
        int [] clusters;
        public Scatter_Plot_Matrix(DataTable t)
        {
            dt = t;
            charts = new Chart[dt.Columns.Count, dt.Columns.Count];
        }
        private void generate_colors()
        {
            int j = 1;
            int z = 0;
            for (int i = 0; i < clusters_color.Length; i++)
            {
                bool found = false;
                Color t = Color.FromArgb((i * j) % 256, (j * 30) % 256, (i * 50) % 256);
                
                for (int k = 0; k < z; k++)
                {
                    if (clusters_color[k] == t||t==Color.Red)
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    clusters_color[i] = t;
                    z++;
                }
                else
                    i--;
                j += 100;
            }
        }
        public void Generate_martix(Panel panel, int[] cluster_index, int num_clusters)
        {
            /////////////////
            
            /////////////////
            clusters = cluster_index;
            clusters_color = new Color[num_clusters];
            generate_colors();
            // panel.Invalidate();
            panel.Controls.Clear();
            int value = dt.Columns.Count;
            int width = panel.Width / value;
            int height = panel.Height / value;
            panel.Width = width * value;
            panel.Height = height * value;
            for (int i = 0; i < value; i++)
            {

                for (int j = 0; j < value; j++)
                {
                    if (i == j)
                    {
                        charts[i, j] = null;
                        Label l = new Label();
                        l.Width = width - 2;
                        l.Height = height - 2;
                        l.Location = new Point((j * width), (i * height));
                        panel.Controls.Add(l);
                        l.Text = dt.Columns[i].ColumnName;
                        l.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        l.Font = new Font("Tahoma", 10, FontStyle.Bold);
                        l.BackColor = Color.White;
                        continue;
                    }
                    ChartArea ca = new ChartArea();
                    ca.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
                    ca.AxisX.LabelStyle.Enabled = false;
                    ca.AxisX.MajorGrid.Enabled = false;
                    ca.AxisX.MajorTickMark.Enabled = false;
                    ca.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
                    ca.AxisY.LabelStyle.Enabled = false;
                    ca.AxisY.MajorGrid.Enabled = false;
                    ca.AxisY.MajorTickMark.Enabled = false;
                    ca.Position.Auto = false;
                    ca.Position.Height = 95F;
                    ca.Position.Width = 95F;
                    ca.Position.X = 0F;
                    ca.Position.Y = 0F;
                    Chart z = new Chart();
                    z.Width = width - 2;
                    z.Height = height - 2;
                    //z.BorderlineColor = System.Drawing.Color.Black;
                    //z.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
                    z.ChartAreas.Add(ca);
                    z.MouseMove += chart_MouseMove;
                    z.MouseLeave += chart_MouseLeave;
                    z.Series.Add(1.ToString());
                    z.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    z.Series[0].MarkerSize = 5;
                    z.Series[0].YValuesPerPoint = 20;
                    z.Series[0].IsXValueIndexed = false;
                    // z.Series[0].Color = clusters_color[k];


                    //z.Series[0].Points.AddXY(2, 3);
                    charts[i, j] = z;
                    panel.Controls.Add(z);
                    z.Location = new Point((j * width), (i * height));
                    DataRow d;
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        d = dt.Rows[k];
                        int cluster_ind = cluster_index[k];

                        charts[i, j].Series[0].Points.AddXY(double.Parse(d[j].ToString()), d[i]);
                        charts[i, j].Series[0].Points[k].Color = clusters_color[cluster_ind];
                    }
                }
            }
            // draw_points(cluster_index);

        }
        private void chart_MouseLeave(object sender, EventArgs e)
        {
            change_colors();
        }
        private void change_colors()
        {
            for (int i = 0; i < dt.Columns.Count; i++)
                for (int j = 0; j < dt.Columns.Count; j++)
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (charts[i, j] != null)
                        {
                            int cluster_ind = clusters[k];
                            charts[i, j].Series[0].Points[k].Color = clusters_color[cluster_ind];
                        }
                    }
        }
        private void chart_MouseMove(object sender, MouseEventArgs e)
        {
            change_colors();
            Chart f = (Chart)sender;
            if (e.X > f.ChartAreas[0].Position.X && e.X < (f.Width - 5) && e.Y > f.ChartAreas[0].Position.Y && e.Y < (f.Height - 5))
            {
                int index = f.HitTest(e.X, e.Y).PointIndex;
                if (index >= 0)
                {

                    object[] r = data.Rows[index].ItemArray;
                    string ss = "";
                    for (int m = 0; m < r.Length; m++)
                        ss += dt.Columns[m].ColumnName.ToString() + " = " + r[m].ToString() + ".";
                    f.Series[0].Points[index].ToolTip = ss;
                    for (int k = 0; k < dt.Columns.Count; k++)
                        for (int t = 0; t < dt.Columns.Count; t++)
                        {
                            if (charts[k, t] != null)
                                charts[k, t].Series[0].Points[index].Color = Color.Red;
                        }
                }
            }
        }
    }
}
