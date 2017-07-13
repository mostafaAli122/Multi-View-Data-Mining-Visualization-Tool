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
    public partial class Help : MetroFramework.Forms.MetroForm
    {
        string S = "";
        public Help(string s)
        {
            InitializeComponent();
            S = s;
        }

        private void Help_Load(object sender, EventArgs e)
        {
            if (S == "DataMining And Visualization")
            {
                groupBox1.Visible = true;
            }

            else if (S == "General Visualization")
            {
                groupBox2.Visible = true;
            }
            
        }

        
        
    }
}
