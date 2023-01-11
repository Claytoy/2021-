using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 附和导线近似平差
{
    public class Date
    {
        double mid = 0;
        double ad = 0;
        double mu = 0;

        public double Mid//中误差
        {
            get { return mid; }
            set { mid = value; }
        }
        public double Ad//加常数
        {
            get { return ad; }
            set {ad = value; }
        }

        public double Mu//乘常数
        {
            get { return mu; }
            set { mu = value; }
        }

    }
    public class Point
    {
        public double X = 0;//已知坐标X
        public double Y = 0;//已知坐标Y
        public string Cstation;//测站
        public string aimStation1, aimStation2;//前后测站
        public double Ql = 0;//前侧站L
        public double Hl = 0;//后测站l
        public double S = 0;//测站距离
    }

    public class angle
    {
        public double beta = 0;//转角
        public double alpha = 0;//方位角
    }

    public class correction
    {
        public double cBeta = 0;
        public double cAlpha = 0;
    }

    public class dGc
    {
        public double dX = 0;
        public double dY = 0;
    }

    public class vXY
    {
        public double vX = 0;
        public double vY = 0;
    }

    public class Calculate
    {
        public Date Data = new Date();
     

       // public List<Point> KPoints = new List<Point>();
        public List<Point> Points = new List<Point>();

        public List<angle> betas = new List<angle>();

        public List<correction> cBetas = new List<correction>();

        public List<dGc> dGcs = new List<dGc>();

        public List<vXY> vXYs = new List<vXY>();
    }
}
