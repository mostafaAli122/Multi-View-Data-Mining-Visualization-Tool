using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Multi_View_Data_Visualization_Tool
{
    class PreProcess
    {
        bool res_;
        double no_;
        double MAX;
        double MIN;
        double min;
        double max;
        double[] arr = new double[10000];

        public double calculate_mean(int column_index, DataTable dt)
        {
            double sum = 0;
            double count = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string s = dt.Rows[i][column_index].ToString();
                if (s != "")
                {
                    sum += double.Parse(s);
                    count++;
                }
            }
            return Math.Round((sum / count), 2);
        }
        public DataGridView PreProcess_(DataTable Dt, ComboBox Com, DataGridView DGV, TextBox txtnewmin, TextBox txtnewmax)
        {
            bool res;
            double no;
            if (DGV.Rows.Count != 0)
            {
                if (Com.SelectedItem == "Data Cleaning(Remove Noise).")
                {
                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < Dt.Columns.Count; j++)
                        {
                            if (Dt.Rows[i][j].ToString().Equals(""))
                            {
                                string s = Dt.Rows[i - 1][j].ToString();
                                res = double.TryParse(s, out no);
                                if (res)
                                {
                                    double mean = calculate_mean(j,Dt);
                                    Dt.Rows[i][j] = mean;
                                }
                                else
                                    Dt.Rows[i][j] = "NULL";
                            }

                        }
                    }
                    DGV.DataSource = Dt;
                }

                else if (Com.SelectedItem == "Data Normalization.")
                {
                    if (txtnewmax.Text.ToString().Equals("") || txtnewmin.Text.ToString().Equals(""))
                    {
                        MessageBox.Show("Enter NewMax AND NewMin");
                    }
                    else
                    {
                        min = double.Parse(txtnewmin.Text.ToString());
                        max = double.Parse(txtnewmax.Text.ToString());

                        for (int i = 0; i < Dt.Rows.Count; i++)
                        {
                            for (int j = 1; j < Dt.Columns.Count; j++)
                            {
                                string s = Dt.Rows[i][j].ToString();
                                res_ = double.TryParse(s, out no_);
                                if (res_)
                                {
                                    for (int k = 0; k < Dt.Rows.Count; k++)
                                    {
                                        arr[k] = double.Parse(Dt.Rows[k][j].ToString());
                                    }
                                    Array.Sort<double>(arr);
                                    MAX = arr[arr.Length - 1];
                                    MIN = arr[0];
                                    Dt.Rows[i][j] = ((double.Parse(Dt.Rows[i][j].ToString()) - MIN) / (MAX - MIN)) * (max - min) + (min);
                                }
                            }
                        }
                        DGV.DataSource = Dt;
                    }
                }

                else
                    MessageBox.Show("Please ,Select One of the Pre-Processing Techniques");
            }

            else
            {
                MessageBox.Show("Please ,Load Data Set ...");
            }
            return DGV;
        }
    }
}

