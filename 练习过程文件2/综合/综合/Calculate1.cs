using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace 综合
{
    class Calculate1
    {
        double a;
        double f;
        double L0;
        public void Getdata(string filepath)
        {
            string line;
            string[] strs;
            StreamReader sr = new StreamReader(filepath);
            line = sr.ReadLine();
            strs = line.Split(',');
            a = double.Parse(strs[1]);
            line = sr.ReadLine();
            strs = line.Split(',');
            f = double.Parse(strs[1]);
            f = 1 / f;
            line = sr.ReadLine();
            strs = line.Split(',');
            L0 = double.Parse(strs[1]);
            line = sr.ReadLine();

            while((line = sr.ReadLine()) != null)
            {

            }

            int aa = 0;
        }
    }
}
