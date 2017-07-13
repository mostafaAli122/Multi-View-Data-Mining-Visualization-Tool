using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multi_View_Data_Visualization_Tool
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
        
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            using(Form2 frm2=new Form2())
            {
                frm2.ShowDialog();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            //using (General_Visualization frm = new General_Visualization())
            //{
            //    frm.ShowDialog();
            //}
            using (GeneralVisualization frm = new GeneralVisualization())
            {
                frm.ShowDialog();
            }
        }
    }
}
