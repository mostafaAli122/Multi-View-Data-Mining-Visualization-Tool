namespace Multi_View_Data_Visualization_Tool
{
    partial class GeneralVisualization
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.His_and_pie_gb = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Visualize_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Choose_btn = new System.Windows.Forms.Button();
            this.charts_comboBox = new System.Windows.Forms.ComboBox();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.Open_File_btn = new System.Windows.Forms.MenuItem();
            this.Exit_File_btn = new System.Windows.Forms.MenuItem();
            this.Help_btn = new System.Windows.Forms.MenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.His_and_pie_gb.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listView1);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(540, 613);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data Set";
            // 
            // listView1
            // 
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(3, 19);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(531, 588);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.Choose_btn);
            this.groupBox3.Controls.Add(this.charts_comboBox);
            this.groupBox3.Location = new System.Drawing.Point(558, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(804, 605);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "General Techniques";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.His_and_pie_gb);
            this.groupBox1.Location = new System.Drawing.Point(6, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(792, 541);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // His_and_pie_gb
            // 
            this.His_and_pie_gb.Controls.Add(this.label2);
            this.His_and_pie_gb.Controls.Add(this.panel1);
            this.His_and_pie_gb.Controls.Add(this.Visualize_btn);
            this.His_and_pie_gb.Controls.Add(this.label1);
            this.His_and_pie_gb.Controls.Add(this.comboBox1);
            this.His_and_pie_gb.Location = new System.Drawing.Point(62, 19);
            this.His_and_pie_gb.Name = "His_and_pie_gb";
            this.His_and_pie_gb.Size = new System.Drawing.Size(595, 473);
            this.His_and_pie_gb.TabIndex = 3;
            this.His_and_pie_gb.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(202, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "label2";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(9, 96);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(567, 371);
            this.panel1.TabIndex = 4;
            // 
            // Visualize_btn
            // 
            this.Visualize_btn.Location = new System.Drawing.Point(394, 14);
            this.Visualize_btn.Name = "Visualize_btn";
            this.Visualize_btn.Size = new System.Drawing.Size(182, 23);
            this.Visualize_btn.TabIndex = 2;
            this.Visualize_btn.Text = "Visualize";
            this.Visualize_btn.UseVisualStyleBackColor = true;
            this.Visualize_btn.Click += new System.EventHandler(this.Visualize_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose Column";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(117, 16);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(254, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // Choose_btn
            // 
            this.Choose_btn.Location = new System.Drawing.Point(273, 29);
            this.Choose_btn.Name = "Choose_btn";
            this.Choose_btn.Size = new System.Drawing.Size(227, 23);
            this.Choose_btn.TabIndex = 1;
            this.Choose_btn.Text = "Choose";
            this.Choose_btn.UseVisualStyleBackColor = true;
            this.Choose_btn.Click += new System.EventHandler(this.Choose_btn_Click);
            // 
            // charts_comboBox
            // 
            this.charts_comboBox.FormattingEnabled = true;
            this.charts_comboBox.Items.AddRange(new object[] {
            "Histogram ",
            "Pie Chart",
            "Column Chart"});
            this.charts_comboBox.Location = new System.Drawing.Point(6, 31);
            this.charts_comboBox.Name = "charts_comboBox";
            this.charts_comboBox.Size = new System.Drawing.Size(233, 21);
            this.charts_comboBox.TabIndex = 0;
            this.charts_comboBox.SelectedIndexChanged += new System.EventHandler(this.charts_comboBox_SelectedIndexChanged);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.Help_btn});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.Open_File_btn,
            this.Exit_File_btn});
            this.menuItem1.Text = "File";
            // 
            // Open_File_btn
            // 
            this.Open_File_btn.Index = 0;
            this.Open_File_btn.Text = "Open";
            this.Open_File_btn.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // Exit_File_btn
            // 
            this.Exit_File_btn.Index = 1;
            this.Exit_File_btn.Text = "Exit";
            this.Exit_File_btn.Click += new System.EventHandler(this.Exit_File_btn_Click);
            // 
            // Help_btn
            // 
            this.Help_btn.Index = 1;
            this.Help_btn.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2});
            this.Help_btn.Text = "Help";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "How To Use";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click_1);
            // 
            // GeneralVisualization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1362, 694);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Menu = this.mainMenu1;
            this.Name = "GeneralVisualization";
            this.Text = "GeneralVisualization";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.GeneralVisualization_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.His_and_pie_gb.ResumeLayout(false);
            this.His_and_pie_gb.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button Choose_btn;
        private System.Windows.Forms.ComboBox charts_comboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox His_and_pie_gb;
        private System.Windows.Forms.Button Visualize_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem Open_File_btn;
        private System.Windows.Forms.MenuItem Exit_File_btn;
        private System.Windows.Forms.MenuItem Help_btn;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MenuItem menuItem2;

    }
}