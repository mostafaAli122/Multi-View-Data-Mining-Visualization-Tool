using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Multi_View_Data_Visualization_Tool
{
    class LoadTransactions
    {
        public StreamReader sr { get; set; }

        public LoadTransactions(StreamReader sr)
        {
            this.sr = sr;
        }

        public List<List<string>> Load()
        {
            List<List<string>> transactions = new List<List<string>>();
            List<string> trans;

            string line;
            string pattern = @"^([a-z0-9]+)(\s*)(,\s*[a-z0-9\s]+)*$";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);

            while ((line = sr.ReadLine()) != null)
            {
                trans = new List<string>();

                foreach(var item in line.Split(','))
                {
                    if (!rgx.IsMatch(line))
                    {
                        throw new FormatException("Invalid or corrupted file. Please try again.");
                    }
                    trans.Add(item.Trim().ToLower());
                }
                transactions.Add(trans);
            }

            if(transactions.Count != 0)
                return transactions;

            throw new FormatException("Invalid or corrupted file. Please try again.");
        }        
    }
}
