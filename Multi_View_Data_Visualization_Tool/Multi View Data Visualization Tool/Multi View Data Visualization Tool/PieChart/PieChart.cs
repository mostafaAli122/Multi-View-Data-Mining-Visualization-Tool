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
   public class PieChart
    {
        public DataTable _table;
       // Histogram_Data[] Data;
       public PieChart(DataTable data)
        {
            _table = data;
        }
       public PieChart()
        { }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="attribute_name"></param>
       /// <returns></returns>
       public PieChart_Data[] count_data(string attribute_name)
       {
           
           List<PieChart_Data> dat= new List<PieChart_Data>();
           string val;
           int ret;
           int rows = _table.Rows.Count;
           for (int i = 0; i <rows; i++)
           {
             val=  _table.Rows[i][attribute_name].ToString();
             ret = Exist(dat, val);
               if (ret==-1)
               {
                   PieChart_Data d = new PieChart_Data();
                   d.bin_name = val;
                   d.frequency++;
                   dat.Add(d);
               }
               else
               dat[ret].frequency++;
           }
           return dat.ToArray() ;
       }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="data"></param>
       /// <param name="name"></param>
       /// <returns></returns>
       private int Exist(List<PieChart_Data> data, string name)
       { 
           int count= data.Count;
           for (int i = 0; i < count; i++)
           {
               if (data[i].bin_name== name)
                   return i;
           }
           return -1;
       }


       /// <summary>
       /// ///////////////////////
       /// </summary>
       public void fill_table()
        {
            _table = new DataTable("EMployee");
            _table.Columns.Add("Name", typeof(string));
            _table.Columns.Add("Nationality", typeof(string));
            _table.Columns.Add("Age", typeof(int));
            _table.Columns.Add("Gender", typeof(string));
            DataRow dt = _table.NewRow();
            dt[0] = "Mohamed";
            dt[1] = "Egyption";
            dt[2] = 25;
            dt[3] = "Male";
            _table.Rows.Add(dt);
            dt = _table.NewRow();
            dt[0] = "Ali";
            dt[1] = "Egyption";
            dt[2] = 30;
            dt[3] = "Female";
            _table.Rows.Add(dt);
            /////////////////////////
           
            dt = _table.NewRow();
            dt[0] = "Divd";
            dt[1] = "Italian";
            dt[2] = 25;
            dt[3] = "Male"; 
           _table.Rows.Add(dt);
            dt = _table.NewRow();
            dt[0] = "Max";
            dt[1] = "Russian";
            dt[2] = 26;
            dt[3] = "Male";
            _table.Rows.Add(dt);
            dt = _table.NewRow();
            dt[0] = "Mustafa";
            dt[1] = "Egyption";
            dt[2] = 30;
            dt[3] = "Male";
            _table.Rows.Add(dt);
            ////////////////
            dt = _table.NewRow();

            dt[0] = "Davd";
            dt[1] = "Italian";
            dt[2] = 40;
            dt[3] = "Female";
            _table.Rows.Add(dt);
            dt = _table.NewRow();
            dt[0] = "Mex";
            dt[1] = "Russian";
            dt[2] = 38;
            dt[3] = "Female";
            _table.Rows.Add(dt);
            dt = _table.NewRow();
            dt[0] = "Darsh";
            dt[1] = "Egyption";
            dt[2] = 39;
            dt[3] = "Female";
            _table.Rows.Add(dt);
            ///////////////////
            dt = _table.NewRow();

            dt[0] = "peter";
            dt[1] = "Italian";
            dt[2] = 20;
            dt[3] = "Female";
            _table.Rows.Add(dt);
            dt = _table.NewRow();
            dt[0] = "lili";
            dt[1] = "Russian";
            dt[2] = 42;
            dt[3] = "Female";
            _table.Rows.Add(dt);
            dt = _table.NewRow();
            dt[0] = "kariem";
            dt[1] = "Egyption";
            dt[2] = 29;
            dt[3] = "Male";
            _table.Rows.Add(dt);
        }
       public string[] get_column_names()
       {
           List<string> list = new List<string>();
           for (int i = 0; i < _table.Columns.Count; i++)
           {
               string name = _table.Columns[i].ColumnName;
               list.Add(name);
           }
           return list.ToArray();
       }
    }
}
