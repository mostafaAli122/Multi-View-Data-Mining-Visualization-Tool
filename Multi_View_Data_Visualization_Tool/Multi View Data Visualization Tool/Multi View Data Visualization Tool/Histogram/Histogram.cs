using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  System.IO;

using System.Data;
namespace Multi_View_Data_Visualization_Tool
{
    class Histogram
    {
        public DataTable _table;
       public Histogram(DataTable t)
        {
            _table = t;
        }
        public Bin_data[] get_Bins(string attribute_name)
        {
            List<Bin_data> list = new List<Bin_data>();
            double min,max;
            if (double.TryParse(_table.Rows[0][attribute_name].ToString(), out min))
            {
                // get maximum value and ,minimum value of attribute to determine width of bins by using this equation width = (max-min)/sqrt(arrtibute count);
                max = min;  
                for (int i = 0; i < _table.Rows.Count; i++)
                {
                    double val = double.Parse(_table.Rows[i][attribute_name].ToString());
                    if (val > max)
                        max = val;
                    else if (val < min)
                        min = val;
                }
                int sqrt = (int)Math.Ceiling(Math.Sqrt((double)_table.Rows.Count));
                double width = Math.Ceiling((max - min) / sqrt);

                while (min<=max)
                {
                    Bin_data bin = new Bin_data(min, (min + width));
                    list.Add(bin);
                    min += width;
                    //if (min > (max + width))
                    //    break;
                }
                for (int i = 0; i < _table.Rows.Count; i++)
                {
                    double val = double.Parse(_table.Rows[i][attribute_name].ToString());
                
                    for (int k = 0; k < list.Count; k++)
                    {
                      
                         if (val >= list[k].minimum && val < list[k].maximum)
                        {
                            list[k].frequency++;
                            break;
                        }
                    }
                   
                }

        }
            else
                return null;
            return list.ToArray();
        }
      
    }
}
