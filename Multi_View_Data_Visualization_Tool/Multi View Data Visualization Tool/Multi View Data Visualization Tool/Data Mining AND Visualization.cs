using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
 
namespace Multi_View_Data_Visualization_Tool
{
    public partial class Form2 : MetroFramework.Forms.MetroForm
    {
        Attribute targ;
        List<Attribute> attlist;
        List<Attribute> temp;
        Scatter_Plot_Matrix sc;
        PreProcess pre = new PreProcess();
        Apriori apriori;
        Classifier classbulit;
        K_Means obj = new K_Means();
        int[] clu;
        DataTable dt;
        DataTable DT;
        DataTable DT_Copy;
        DataTable DT_Cluster;
        TextReader tr;
        String Line;
        String file;
        string[] col_names;
        Arff a;
        int[] clustering;

        List<string> dataarff = new List<string>();
        List<List<string>> transactions = new List<List<string>>();
        List<string> trans;
        int num = 0;

        Random x = new Random();
        Color[] colors;

        List<double> saveliftvlues = new List<double>();
        List<int> supportvlues = new List<int>();

        public Form2()
        {
            InitializeComponent();
        }
        struct attval
        {
            public int index;
            public string val;
        }

        // Defination of Funcation 

        public List<List<string>> Convert_from_DataTable_To_ListOfList(DataTable dt)
        {
            transactions = new List<List<string>>();
            dt = DT_Copy.Copy();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string dataar = null;
                trans = new List<string>();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    trans.Add(dt.Rows[i][j].ToString());
                    if (j == dt.Columns.Count - 1)
                    {
                        dataar += dt.Rows[i][j].ToString();
                    }
                    else
                    {
                        dataar += dt.Rows[i][j].ToString() + ",";
                    }

                }
                transactions.Add(trans);
                dataarff.Add(dataar);
            }
            return transactions;
        }
        public void ShowClustered(double[][] data, int[] clustering, int numClusters, int decimals, int columnas)
        {
            int contar = 0;
            string cadena = "";
            string[] row = new string[columnas];

            dataShowCluster.ColumnCount = columnas;
            for (int k = 0; k < numClusters; ++k)
            {
                dataShowCluster.Rows.Add();
                dataShowCluster.Rows.Add("Cluster: " + k);
                for (int i = 0; i < data.Length; i++)
                {
                    int clusterID = clustering[i];
                    if (clusterID != k) continue;

                    for (int j = 1; j < data[i].Length; j++)
                    {
                        if (data[i][j] >= 0.0) Console.Write(" ");
                        Console.Write(data[i][j].ToString("F" + decimals) + " ");
                        row[0] = (i + 1).ToString();
                        row[j] = data[i][j].ToString();
                        // chartClustering.Series[k].Points.AddXY(data[i][Convert.ToInt32(cboX.Text)], data[i][Convert.ToInt32(cboY.Text)]).ToString();
                    }
                    dataShowCluster.Rows.Add(row);
                    contar++;
                }
                cadena += "Cluster: " + k + "   Total: " + contar + "   Items \n";
                contar = 0;
            }
            lblresult.Text = cadena;
        }
        public void ShowData(double[][] data, int decimals, bool indices, bool newLine, int numClusters, int columnas)
        {
            label1.Text = "";
            num = numClusters;
            clustering = obj.Cluster(data, numClusters);
            clu = clustering;
            sc = new Scatter_Plot_Matrix(DT_Copy);
            sc.data = (DataTable)dataGridView1.DataSource;
            sc.Generate_martix(panel1, clustering, numClusters);
            string n = file.Substring(file.LastIndexOf("\\") + 1);
            string[] n1 = n.Split('.');
            label1.Text = "Scatter Plot for " + n1[0];
            //panel1.Invalidate();
            panel2.Invalidate();
            obj.ShowVector(clustering, true);
            ShowClustered(data, clustering, numClusters, 1, columnas);

        }
        public void draw_clusters_panel2(Color[] colors)
        {
            Graphics g = panel2.CreateGraphics();
            for (int i = 0; i < colors.Length; i++)
            {
                g.FillRectangle(new SolidBrush(colors[i]), 2, i * 30, 20, 20);
                g.DrawString("Cluster " + (i + 1).ToString(), new Font(System.Drawing.FontFamily.Families[0], 15, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 30, i * 30);

            }
        }
        public DataTable ConvertFromNominalToDecimal(DataTable Dt)
        {
            double no;
            string str;
            List<string> L = new List<string>();
            List<string> l = new List<string>();
            for (int i = 1; i < Dt.Columns.Count; i++)
            {
                L.Clear();
                l.Clear();
                str = (Dt.Rows[0][i]).ToString();
                if (!(double.TryParse(str, out no)))
                {
                    for (int j = 0; j < Dt.Rows.Count; j++)
                    {
                        l.Add(Dt.Rows[j][i].ToString());
                    }
                    L = l.Distinct().ToList();
                    for (int k = 0; k < L.Count(); k++)
                    {
                        L[k] = L[k] + "," + k.ToString();
                    }

                    for (int m = 0; m < Dt.Rows.Count; m++)
                    {
                        for (int n = 0; n < L.Count(); n++)
                        {
                            string[] s = L[n].Split(',');
                            if (Dt.Rows[m][i].Equals(s[0]))
                            {
                                Dt.Rows[m][i] = s[1];
                            }

                        }
                    }
                }
            }
            return Dt;
        }
        public void Convertertoarff()
        {
            dt = (DataTable)dataGridView1.DataSource;
             targ = null;
             attlist = new List<Attribute>();
             temp = new List<Attribute>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                List<Value> attvalue = new List<Value>();
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    Value v = new Value(dt.Rows[j][i].ToString());
                    if (!attvalue.Contains(v))
                    {
                        attvalue.Add(v);
                    }
                }

                Attribute newatt = new Attribute(i, col_names[i], attvalue);
                if (i == dt.Columns.Count - 1)
                {
                    targ = newatt;
                }
                else
                {
                    attlist.Add(newatt);
                }
                temp.Add(newatt);
            }
            List<Data> datavalue = new List<Data>();
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                Data d = new Data(attlist, dataarff[j]);
                datavalue.Add(d);
            }

             a = new Arff();
            a.Attributes = attlist;
            a.Data = datavalue;
            a.Target = targ;
            classbulit = new Classifier(a);
        }
        private void generate_random_colors()
        {
            int j = 1;
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.FromArgb((i + j) % 255, (j * (10 * 50)) % 255, (i * (100 * 10)) % 255);
                j += 100;
            }
        }

        // End Of Defination of Funcation 


        // Defination of Control Events 

        private void btnFile_Click(object sender, EventArgs e)
        {
            DialogResult Result = openFileDialog1.ShowDialog();
            if (Result == DialogResult.OK)
            {
                string[] items;
                file = openFileDialog1.FileName;
                try
                {
                    dt = new DataTable();
                    tr = File.OpenText(file);
                    Line = tr.ReadLine();
                    col_names = Line.Split('\t');
                    while ((Line = tr.ReadLine()) != null)
                    {
                        items = Line.Split('\t');

                        if (dt.Columns.Count == 0)
                        {
                            for (int i = 0; i < col_names.Length; i++)
                            {
                                dt.Columns.Add(col_names[i]);
                            }
                        }

                        dt.Rows.Add(items);
                    }
                    this.dataGridView1.DataSource = dt;
                }
                catch (IOException)
                {
                }
            }

        }
        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count != 0)
            {
                if (comboBox6.SelectedItem == "Data Normalization.")
                {
                    groupBox10.Visible = true;
                }
            }
           else
            {
                MessageBox.Show("Please ,Load Data Set ...");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            TextBox Min = textBox3;
            TextBox Max = textBox2;
            dataGridView1 = pre.PreProcess_(dt, comboBox6, dataGridView1, Min, Max);

        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();

            if (dataGridView1.Rows.Count != 0)
            {
                if (comboBox3.SelectedItem == "Apriori")
                {
                    groupBox4.Visible = true;
                    listBox1.Items.Clear();
                    listBox2.Items.Clear();
                    listBox3.Items.Clear();
                    listBox4.Items.Clear();
                    List<List<string>> transactions_= new List<List<string>>();
                    transactions_ = Convert_from_DataTable_To_ListOfList(dt);
                    apriori = new Apriori(transactions_);

                    foreach (var transacion in apriori.transactions)
                    {
                        StringBuilder tempString = new StringBuilder();

                        foreach (var item in transacion)
                        {
                            tempString.Append(item + ", ");
                        }
                        listBox1.Items.Add(tempString.ToString());
                    }

                    trackBar1.Maximum = apriori.FirstFrequent.Values.ToList().Max();
                    findfrequent.Enabled = true;
                    trackBar1.Enabled = true;
                    trackBar2.Enabled = true;
                    btncheck.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Please ,Load Data Set ...");
            }
        }
        private void findfrequent_Click_1(object sender, EventArgs e)
        {
                listBox2.Items.Clear();
                listBox3.Items.Clear();

                apriori.SetUp(trackBar1.Value);
                apriori.FindFrequent();

                foreach (var item in apriori.FirstFrequent)
                {
                    listBox2.Items.Add(item.Key + " - " + item.Value);
                }

                foreach (var item in apriori.FrequentItemSets)
                {
                    listBox3.Items.Add(item.Key + " - " + item.Value);
                    supportvlues.Add(item.Value);
                }
                btncheck.Enabled = true;
           
            
        }
        private void btncheck_Click_1(object sender, EventArgs e)
        {
            listBox4.Items.Clear();
            apriori.GetConfidence(trackBar2.Value);

            foreach (var item in apriori.ConfidenceItemSets)
            {
                listBox4.Items.Add(item.Key + " - " + Math.Round(item.Value, 2));
                saveliftvlues.Add(Math.Round(item.Value, 2));
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (txtnumClusters.Text.ToString() != "")
            {
                if (Convert.ToInt32(txtnumClusters.Text) <= Convert.ToInt32(txtrows.Text) && Convert.ToInt32(txtnumClusters.Text) != 0)
                {
                    dataShowCluster.Rows.Clear();
                    int row = Convert.ToInt32(txtrows.Text);
                    int column = Convert.ToInt32(txtcols.Text);
                    int numClusters = Convert.ToInt32(txtnumClusters.Text);
                    double[][] rawData = new double[row][];
                    for (int i = 0; i <= dataInicial.Rows.Count - 1; i++)
                    {
                        rawData[i] = new double[column];
                        for (int j = 0; j <= dataInicial.Columns.Count - 1; j++)
                        {
                            rawData[i][j] = double.Parse(dataInicial.Rows[i].Cells[j].Value.ToString());
                        }
                    }
                    ShowData(rawData, 1, true, true, numClusters, column);
                }
                else 
                {
                    MessageBox.Show("Enter Number of Cluster Less than Number of Rows !!");
                }

                //parallel 
                chart1.Visible = true;
                chart1.Series.Clear();
                DataTable parallel = dt;
                for (int i = 0; i < parallel.Rows.Count; i++)
                {
                    chart1.Series.Add("r" + i);
                    chart1.Series["r" + i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                }
                for (int i = 0; i < parallel.Rows.Count; i++)
                {
                    for (int j = 0; j < parallel.Columns.Count; j++)
                    {
                        string s = parallel.Rows[i][j].ToString();
                        double no_;
                        bool res_ = double.TryParse(s, out no_);
                        if (res_)
                        {
                            double[] arr = new double[10000];
                            double MAX;
                            double MIN;
                            double min = 0;
                            double max = 1;
                            for (int k = 0; k < parallel.Rows.Count; k++)
                            {
                                arr[k] = double.Parse(parallel.Rows[k][j].ToString());
                            }
                            Array.Sort<double>(arr);
                            MAX = arr[arr.Length - 1];
                            MIN = arr[0];
                            parallel.Rows[i][j] = ((double.Parse(parallel.Rows[i][j].ToString()) - MIN) / (MAX - MIN)) * (max - min) + (min);
                        }
                    }
                }
                for (int i = 0; i < parallel.Rows.Count; i++)
                {
                    for (int j = 0; j < parallel.Columns.Count; j++)
                    {
                        chart1.Series["r" + i].Points.AddXY(j, parallel.Rows[i][j]);
                    }
                }
                for (int i = 0; i < parallel.Columns.Count; i++)
                {
                    chart1.Series["r1"].Points[i].AxisLabel = col_names[i];

                }
            }
            else
            {
                MessageBox.Show("Please , Enter Number of Cluster ... !!");
            }

        }
        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            DT = DT_Copy.Copy();
            dataGridView1.DataSource = DT_Cluster;
            
            if (dataGridView1.Rows.Count != 0)
            {
                if (comboBox8.SelectedIndex == 0)
                {
                    try
                    {
                        groupBox7.Visible = true;
                        groupBox11.Visible = true;
                        
                        dataInicial.DataSource = ConvertFromNominalToDecimal(DT_Copy);
                        txtrows.Text = dataInicial.Rows.Count.ToString();
                        txtcols.Text = dataInicial.Columns.Count.ToString();
                        cboX.Items.Clear();
                        cboY.Items.Clear();

                        for (int i = 0; i < Convert.ToInt32(txtcols.Text); i++)
                        {
                            cboX.Items.Add(i);
                            cboY.Items.Add(i);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Select Data Set !!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please ,Load Data Set ...");
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                transactions.Clear();
                dataarff.Clear();
                    dt.Rows.Clear();
                dt.Columns.Clear();
                DT.Rows.Clear();
                DT.Columns.Clear();

                DT_Cluster.Rows.Clear();
                DT_Cluster.Columns.Clear();

                DT_Copy.Rows.Clear();
                DT_Copy.Columns.Clear();
            }
            catch (Exception ex) { }
            DialogResult Result = openFileDialog1.ShowDialog();
            if (Result == DialogResult.OK)
            {
                string[] items;
                file = openFileDialog1.FileName;
                try
                {
                    dt = new DataTable();
                    tr = File.OpenText(file);
                    Line = tr.ReadLine();
                    col_names = Line.Split('\t');
                    while ((Line = tr.ReadLine()) != null)
                    {
                        items = Line.Split('\t');

                        if (dt.Columns.Count == 0)
                        {
                            for (int i = 0; i < col_names.Length; i++)
                            {
                                dt.Columns.Add(col_names[i]);
                            }
                        }

                        dt.Rows.Add(items);
                    }
                    DT_Copy = dt.Copy();
                    DT_Cluster = dt.Copy();
                    this.dataGridView1.DataSource = dt;
                }
                catch (IOException)
                {
                }
            }
            TabControl.Visible = true;
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                draw_clusters_panel2(sc.clusters_color);
            }
            catch
            { }
        }
        private void panel2_Validated(object sender, EventArgs e)
        {
            draw_clusters_panel2(sc.clusters_color);
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBoxSupportValue.Text = trackBar1.Value.ToString();
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            textBoxConfidenceValue.Text = trackBar2.Value.ToString() + "%";
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (dataGridView1.Rows.Count != 0)
            {
                groupBox6.Visible = true;
            }
            else
            {
                MessageBox.Show("Please ,Load Data Set ...");
            }
        }
        private void btnclassify_Click(object sender, EventArgs e)
        {
           
            DT = (DataTable)dataGridView1.DataSource;
            if (comboBox4.SelectedItem == "ID3")
            {
                Convert_from_DataTable_To_ListOfList(DT);
                Convertertoarff();

                chart1.Visible = true;
                chart1.Series.Clear();
                DataTable parallel = DT.Copy();

                for (int i = 0; i < parallel.Rows.Count; i++)
                {
                    chart1.Series.Add("r" + i);
                    chart1.Series["r" + i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                }

                int j = 1;
                int z = 0;
                Color[] class_color = new Color[a.Target.Values.Count];
                for (int i = 0; i < a.Target.Values.Count; i++)
                {
                    bool found = false;
                    Color t = Color.FromArgb((i + j) % 255, (j * (10 * 50)) % 255, (i * (100 * 10)) % 255);
                    for (int k = 0; k < z; k++)
                    {
                        if (class_color[k] == t)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                    {
                        class_color[i] = t;
                        z++;
                    }
                    else
                        i--;
                    j += 100;
                }
                Legend l = new Legend();
                for (int i = 0; i < a.Target.Values.Count; i++)
                {
                    LegendItem item = new LegendItem();
                    item.Color = class_color[i];
                    item.Name = "Class " + (i + 1) + " : " + a.Target.Values[i].ToString();
                    l.CustomItems.Add(item);
                }

                chart1.Legends.Add(l);
                Dictionary<attval, int> intval = new Dictionary<attval, int>();
                for (int i = 0; i < temp.Count; i++)
                {
                    for (int jj = 1; jj <= temp[i].Values.Count; jj++)
                    {
                        attval newval;
                        newval.index = i;
                        newval.val = temp[i].Values[jj - 1].item;
                        intval.Add(newval, jj);
                    }
                }
                for (int i = 0; i < parallel.Rows.Count; i++)
                {
                    for (int jj = 0; jj < parallel.Columns.Count; jj++)
                    {
                        attval newval;
                        newval.index = jj;
                        newval.val = parallel.Rows[i][jj].ToString();
                        parallel.Rows[i][jj] = intval[newval];
                    }
                }
                for (int i = 0; i < parallel.Rows.Count; i++)
                {
                    for (int jj = 0; jj < parallel.Columns.Count; jj++)
                    {
                        string t = parallel.Rows[i][jj].ToString();
                        double no_;
                        bool res_ = double.TryParse(t, out no_);
                        if (res_)
                        {
                            double[] arr = new double[10000];
                            double MAX;
                            double MIN;
                            double min = 0;
                            double max = 1;
                            for (int k = 0; k < parallel.Rows.Count; k++)
                            {
                                arr[k] = double.Parse(parallel.Rows[k][jj].ToString());
                            }
                            Array.Sort<double>(arr);
                            MAX = arr[arr.Length - 1];
                            MIN = arr[0];
                            parallel.Rows[i][jj] = ((double.Parse(parallel.Rows[i][jj].ToString()) - MIN) / (MAX - MIN)) * (max - min) + (min);
                        }
                    }
                }

                for (int i = 0; i < parallel.Rows.Count; i++)
                {
                    for (int jj = 0; jj < parallel.Columns.Count; jj++)
                    {
                        chart1.Series["r" + i].Points.AddXY(jj, parallel.Rows[i][jj]);
                    }
                }
                dt = (DataTable)dataGridView1.DataSource;
                for (int i = 0; i < chart1.Series.Count; i++)
                {
                    for (j = 0; j < a.Target.Values.Count; j++)
                    {
                        string temp=dt.Rows[i][dt.Columns.Count-1].ToString();
                        if (temp.Equals( a.Target.Values[j].item.ToString()))
                        {
                            break;
                        }
                    }

                    chart1.Series["r" + i].Color = class_color[j];
                }
               for (int i = 0; i < parallel.Columns.Count; i++)
               {
                   chart1.Series["r1"].Points[i].AxisLabel = col_names[i];
               }
                string s = classbulit.DrawTree();
                label19.Text = classbulit.DrawTree();
            }
            else
            {
                MessageBox.Show("Choose One of The Techniques !!");
            }
            dataGridView1.DataSource = DT_Copy;
        }
        private void howToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = "DataMining And Visualization";
            Help h = new Help(s.ToString());
            h.Show();
        }
        //ENd of Defination of Control Events 

        // Visualization

        private void Visualization_rule_graph_Click(object sender, EventArgs e)
        {
            Scatter_plot.Parent = null;
            Rule_graph.Parent = tabControl1;
            Parallel_coordinates.Parent = null;
        
            TabControl.SelectedTab = tabPage7;
            tabControl1.SelectedTab = Rule_graph;
               panel3.Invalidate();
        }
        private void Visualization_scatter_plot_Click(object sender, EventArgs e)
        {
            Scatter_plot.Parent = tabControl1;
            Rule_graph.Parent = null;
            Parallel_coordinates.Parent = null;
            TabControl.SelectedTab = tabPage7;
            tabControl1.SelectedTab = Scatter_plot;
          
            
        }
        private void Visualization_parrallel_coordinates_Click(object sender, EventArgs e)
        {
            Scatter_plot.Parent = null;
            Rule_graph.Parent = null;
            Parallel_coordinates.Parent = tabControl1;
            TabControl.SelectedTab = tabPage7;
            tabControl1.SelectedTab = Parallel_coordinates;
            
        }

        // Clear Tab Item
        private void tabPage1_Enter(object sender, EventArgs e)
        {
            comboBox6.SelectedIndex = -1;
            comboBox7.SelectedIndex = -1;
            textBox2.Text = "";
            textBox3.Text = "";
            groupBox10.Visible = false;
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            comboBox3.SelectedIndex = -1;
            groupBox4.Visible = false;
            listBox1.Items.Clear();
            textBoxSupportValue.Text = "";
            textBoxConfidenceValue.Text = "";
        }
        private void tabPage3_Enter(object sender, EventArgs e)
        {
            comboBox8.SelectedIndex = -1;
            groupBox7.Visible = false;
            groupBox11.Visible = false;
            txtrows.Text = "";
            txtcols.Text = "";
            txtnumClusters.Text = "";
            cboX.Items.Clear();
            cboY.Items.Clear();
            if (dataInicial.Rows.Count != 0 && dataShowCluster.Rows.Count != 0)
            {
                dataInicial.DataSource = null;
                dataShowCluster.DataSource = null;
                dataInicial.Rows.Clear();
                dataShowCluster.Rows.Clear();
                dataInicial.Refresh();
                dataShowCluster.Refresh();
            }
            lblresult.Text = "";
            dataGridView1.DataSource = DT_Cluster;
        }

        private void tabPage4_Enter(object sender, EventArgs e)
        {
            comboBox4.SelectedIndex = -1;
            groupBox6.Visible = false;
            label19.Text = "";
        }

        private void groupBox10_Enter(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Scatter_plot.Parent = null;
            Rule_graph.Parent = null;
            Parallel_coordinates.Parent = null;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            try {
                showRulegraph_Click();
            }
            catch { }
        }

        private void showRulegraph_Click()
        {
            int intisize = 10;
            int counter = 0;
            int[] AllsupportInlist4 = new int[listBox4.Items.Count];
            colors = new Color[saveliftvlues.Distinct().Count()];
            generate_random_colors();
            List<double> distinctliftVAlue = saveliftvlues.Distinct().ToList();
            List<int> distinctSupportVAlue = supportvlues.Distinct().ToList();

            Dictionary<double, Color> Dic_color = new Dictionary<double, Color>();
            Dictionary<int, int> Dic_size = new Dictionary<int, int>();
            //put for each distinict values in confidence acolor 
            for (int i = 0; i < distinctliftVAlue.Count; i++)
            {
                Dic_color.Add(distinctliftVAlue[i], colors[i]);
            }
            //put for each distinct values (sorted)ascending asize 
            for (int i = 0; i < supportvlues.Distinct().Count(); i++)
            {
                distinctSupportVAlue.Sort();
                Dic_size.Add(distinctSupportVAlue[i], intisize);
                intisize += 10;
            }
            //compare each row in list(freq item set) with list confedence to put value of support of each row in it 
            for (int i = 0; i < listBox3.Items.Count; i++)
            {
                string rowinList3 = listBox3.Items[i].ToString();
                string[] splitedRowList3 = rowinList3.Split(new char[] { ',', '-', '>' });

                for (int j = i; j < listBox4.Items.Count; j++)
                {
                    string rowinList4 = listBox4.Items[j].ToString();
                    string[] splitedRowList4 = rowinList4.Split(new char[] { ',', '-', '>' });

                    List<string> splist = splitedRowList4.ToList();
                    splist.Remove("");
                    splist.Remove(" ");
                    int exist_counter = 0;
                    for (int k = 0; k < splitedRowList3.Length; k++)
                    {

                        for (int l = 0; l < splist.Count; l++)
                        {
                            if (splitedRowList3[k].Trim() == splist[l].Trim())
                                exist_counter++;
                            if (exist_counter == splitedRowList3.Length - 1)
                                AllsupportInlist4[j] = Convert.ToInt32(splitedRowList3[splitedRowList3.Length - 1]);
                        }

                    }
                }
            }
            int cter = 0;
            string afterarrow = "";
            for (int i = 0; i < listBox4.Items.Count; i++)
            {
                string rowinList4 = listBox4.Items[i].ToString();
                string[] splitedRowList4 = rowinList4.Split(new char[] { ',', '-' });
                string lift = "";
                List<string> splist = splitedRowList4.ToList();
                for (int j = 0; j < splist.Count; j++)
                {
                    if (splist[j].Contains(">"))
                    {
                        afterarrow = splist[j];
                        afterarrow = afterarrow.Substring(1, afterarrow.Length - 1);
                        //afterarrow=afterarrow.Split(new char[] { ',', '-' })
                        splist.RemoveAt(j);
                    }
                }
                lift = splist[splist.Count - 1];
                splist.Remove("");
                splist.Remove(" ");
                splist.RemoveAt(splist.Count - 1);
                string[] aferarrowArr = afterarrow.Split(',');

                SolidBrush sb = new SolidBrush(Dic_color[Convert.ToDouble(lift)]);
                Pen blackpen = new Pen(Color.Black, 2);

                Graphics g = panel3.CreateGraphics();
                Pen p = new Pen(Color.Black, 5);
                p.StartCap = LineCap.Round;
                p.EndCap = LineCap.ArrowAnchor;
                FontFamily ff = new FontFamily("Arial");
                // int tmp = x.Next(0, 2);

                for (int k = 0; k < splist.Count; k++)
                {
                    g.DrawString(splist[k], new Font(ff, 10), sb, new Point(0 + cter, 5));
                    g.DrawLine(p, 10 + cter, 20, 10 + cter, 40);

                    if (Dic_size[AllsupportInlist4[i]] != 0)
                        g.FillEllipse(sb, 5 + cter, 40, Dic_size[AllsupportInlist4[i]], Dic_size[AllsupportInlist4[i]]);
                    g.DrawLine(p, 10 + cter, 52, 10 + cter, 90);
                    g.DrawString(aferarrowArr[k], new Font(ff, 10), sb, new Point(0 + cter, 90));
                    cter += 50;
                }

            }


        }

        private void panel3_Validated(object sender, EventArgs e)
        {
            showRulegraph_Click();
        }


        string prefsearies = null;
        Color prefcolor = new Color();
        Color prefborder = new Color();

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                chart1.Series[prefsearies].Color = prefcolor;
                Details.Text = "";
                chart1.Series[prefsearies].BorderColor = prefborder;
            }
            catch (Exception ex)
            {

            }
            try
            {
                HitTestResult result = chart1.HitTest(e.X, e.Y);

                //if (result.ChartElementType == ChartElementType.DataPoint && result.ChartArea == _chartarea && result.Series == _data)
                //{
                //    // A point is selected.
                //}
                string seariesName = result.Series.Name.ToString();
                prefcolor = chart1.Series[seariesName].Color;
                chart1.Series[seariesName].Color = Color.Gold;
                prefborder = chart1.Series[seariesName].BorderColor;
                chart1.Series[seariesName].BorderColor = Color.GreenYellow;

                prefsearies = seariesName;
                string[] temp = seariesName.Split('r');
                string row = null;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i == dt.Columns.Count - 1)
                    {
                        row += a.Target.Name + ":" + dt.Rows[int.Parse(temp[1])][i] + "       ";
                        break;
                    }
                    row += a.Attributes[i].Name + ":" + dt.Rows[int.Parse(temp[1])][i] + "       ";
                }
                Details.Text = "Row" + int.Parse(temp[1]) + 1 + "    " + row;
            }
            catch (Exception ex)
            {
                MessageBox.Show("please select a point have a spcific coulmn");

            }
        }
   




    }
}