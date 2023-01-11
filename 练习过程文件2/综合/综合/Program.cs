using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 综合
{
    class Program
    {
        Calculate1 cal1 = new Calculate1();
        static void Main(string[] args)
        {
            string filepath = "E:\\1测绘程序\\帮助文档与TXT\\程序设计txt\\坐标数据.txt";
            Program pro = new Program();
            pro.cal1.Getdata(filepath);
            int a = 0;
        }
    }
}
