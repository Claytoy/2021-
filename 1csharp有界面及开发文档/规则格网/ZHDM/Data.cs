using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHDM
{
    public class Data
    {
        private string name;//点名
        private double x = 0;//X分量
        private double y = 0;//Y分量
        private double h = 0;//H分量
        private double k = 0;//斜率
        public double S = 0;//距离

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public double H
        {
            get
            {
                return h;
            }

            set
            {
                h = value;
            }
        }

        public double K
        {
            get
            {
                return k;
            }

            set
            {
                k = value;
            }
        }
    }

    public class S
    {
        public string name;
        public double X = 0;
        public double Y = 0;
        public double K = 0;
        public double SS = 0;
    }

    public class Zheng
    {
        public string name;
        public double X = 0;
        public double Y = 0;
        public double K = 0;
        public double S = 0;
    }

    public class Fu
    {
        public string name;
        public double X = 0;
        public double Y = 0;
        public double K = 0;
        public double S = 0;
    }

    public class daicha
    {
        public double X = 0;
        public double Y = 0;
        public double P1 = 0;
        public double P2 = 0;
        public double P3 = 0;
        public double P4 = 0;
        public double V = 0;
    }
}
