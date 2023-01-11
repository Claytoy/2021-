using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ZHDM
{
    class count
    {
        //六个外方位元素
        public double Xs = -6911.42788;
        public double Ys = 4181.15686;
        public double Zs = 157.77319;
        public double o = 0.34831;
        public double p = -0.30914;
        public double q = 0.08136;
        public double f = -165.37034;

        double Xs1 = -6922.01146;
        double Ys1 = 4203.66508;
        double Zs1 = 151.62205;
        double o1 = 0.38231;
        double p1 = -0.33532;
        double q1 = 0.08277;
        double f1 = -165.37034;

        //数据定义
        public List<data> points = new List<data>();

        //导入数据
        public void dataIn(string filepath)
        {
            string line;
            string[] strs;
            StreamReader sr = new StreamReader(filepath);
            while ((line = sr.ReadLine()) != null)
            {
                data kp = new data();
                strs = line.Split(',');
                kp.point = double.Parse(strs[0]);
                kp.x1 = double.Parse(strs[1]);
                kp.y1 = double.Parse(strs[2]);
                kp.x2 = double.Parse(strs[3]);
                kp.y2 = double.Parse(strs[4]);
                points.Add(kp);
            }
        }

        //计算像辅助坐标
        public void fuzhu()
        {
            double pI = Math.PI;
            //x1,x2的参数
            double a1 = Math.Cos(o / 360 * 2 * pI) * Math.Cos(q / 360 * 2 * pI) - Math.Sin(o / 360 * 2 * pI) * Math.Sin(p / 360 * 2 * pI) * Math.Sin(q / 360 * 2 * pI);
            double a2 = -Math.Cos(o / 360 * 2 * pI) * Math.Sin(q / 360 * 2 * pI) - Math.Sin(o / 360 * 2 * pI) * Math.Sin(p / 360 * 2 * pI) * Math.Sin(q / 360 * 2 * pI);
            double a3 = -Math.Sin(o / 360 * 2 * pI) * Math.Cos(p / 360 * 2 * pI);
            double b1 = Math.Cos(p / 360 * 2 * pI) * Math.Sin(q / 360 * 2 * pI);
            double b2 = Math.Cos(p / 360 * 2 * pI) * Math.Cos(q / 360 * 2 * pI);
            double b3 = -Math.Sin(p / 360 * 2 * pI);
            double c1 = Math.Sin(o / 360 * 2 * pI) * Math.Cos(q / 360 * 2 * pI) + Math.Cos(o / 360 * 2 * pI) * Math.Sin(p / 360 * 2 * pI) * Math.Sin(q / 360 * 2 * pI);
            double c2 = -Math.Sin(p / 360 * 2 * pI) * Math.Cos(q / 360 * 2 * pI) + Math.Cos(o / 360 * 2 * pI) * Math.Sin(p / 360 * 2 * pI) * Math.Sin(q / 360 * 2 * pI);
            double c3 = Math.Cos(o / 360 * 2 * pI) * Math.Cos(q / 360 * 2 * pI);

            //x2,y2的参数
            double a11 = Math.Cos(o1 / 360 * 2 * pI) * Math.Cos(q1 / 360 * 2 * pI) - Math.Sin(o1 / 360 * 2 * pI) * Math.Sin(p1 / 360 * 2 * pI) * Math.Sin(q1 / 360 * 2 * pI);
            double a21 = -Math.Cos(o1 / 360 * 2 * pI) * Math.Sin(q1 / 360 * 2 * pI) - Math.Sin(o1 / 360 * 2 * pI) * Math.Sin(p1 / 360 * 2 * pI) * Math.Sin(q1 / 360 * 2 * pI);
            double a31 = -Math.Sin(o1 / 360 * 2 * pI) * Math.Cos(p1 / 360 * 2 * pI);
            double b11 = Math.Cos(p1 / 360 * 2 * pI) * Math.Sin(q1 / 360 * 2 * pI);
            double b21 = Math.Cos(p1 / 360 * 2 * pI) * Math.Cos(q1 / 360 * 2 * pI);
            double b31 = -Math.Sin(p1 / 360 * 2 * pI);
            double c11 = Math.Sin(o1 / 360 * 2 * pI) * Math.Cos(q1 / 360 * 2 * pI) + Math.Cos(o1 / 360 * 2 * pI) * Math.Sin(p1 / 360 * 2 * pI) * Math.Sin(q1 / 360 * 2 * pI);
            double c21 = -Math.Sin(p1 / 360 * 2 * pI) * Math.Cos(q1 / 360 * 2 * pI) + Math.Cos(o1 / 360 * 2 * pI) * Math.Sin(p1 / 360 * 2 * pI) * Math.Sin(q1 / 360 * 2 * pI);
            double c31 = Math.Cos(o1 / 360 * 2 * pI) * Math.Cos(q1 / 360 * 2 * pI);
            for (int i = 0; i < points.Count; i++)
            {
                //x1,y1的像辅助坐标
                points[i].u1 = a1 * points[i].x1 + a2 * points[i].y1 + a3 * f;
                points[i].v1 = b1 * points[i].x1 + b2 * points[i].y1 + b3 * f;
                points[i].w1 = c1 * points[i].x1 + c2 * points[i].y1 + c3 * f;
                //x2,y2的像辅助坐标
                points[i].u2 = a11 * points[i].x2 + a21 * points[i].y2 + a31 * f;
                points[i].v2 = b11 * points[i].x2 + b21 * points[i].y2 + b31 * f;
                points[i].w2 = c11 * points[i].x2 + c21 * points[i].y2 + c31 * f;
            }
        }

        //计算投影系数
        public void touying()
        {
            double Bu = 0;
            double Bv = 0;
            double Bw = 0;

            Bu = Xs1 - Xs;
            Bv = Ys1 - Ys;
            Bw = Zs1 - Zs;
            for (int i = 0; i < points.Count; i++)
            {
                //投影系数1
                points[i].N1 = (Bu * points[i].w2 - Bw * points[i].u2) / (points[i].u1 * points[i].w2 - points[i].u2 * points[i].w1);
                //投影系数2
                points[i].N2 = (Bu * points[i].w1 - Bw * points[i].u1) / (points[i].u1 * points[i].w2 - points[i].u2 * points[i].w1);
            }
        }

        //计算大地坐标
        public void dimianzuobiao()
        {
            for (int i = 0; i < points.Count; i++)
            {
                points[i].X = Xs + points[i].N1 * points[i].u1;//X
                points[i].Y = 0.5 * ((Ys + points[i].N1 * points[i].v1) + (Ys1 + points[i].N1 * points[i].v2));//Y
                points[i].Z = Zs + points[i].N1 * points[i].w1;//Z
            }
        }
    }
}
