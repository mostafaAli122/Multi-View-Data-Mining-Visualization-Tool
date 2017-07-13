using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multi_View_Data_Visualization_Tool
{
    class Bin_data
    {
        public double minimum;
       public double maximum;
       public int frequency;
       public Bin_data(double min, double max)
       {
           minimum = min;
           maximum = max;
           frequency = 0;
       }
        
    }
}
