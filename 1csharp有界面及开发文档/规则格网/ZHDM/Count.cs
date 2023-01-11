using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ZHDM
{
    public class Count
    {
        public List<Data> points = new List<Data>();
        public List<S> Jdian = new List<S>();
        public List<Zheng> zhengshu = new List<Zheng>();
        public List<Fu> fushu = new List<Fu>();
        public List<daicha> Daicha = new List<daicha>();
        public double ddd = 0;
        public double h = 0;
        public void DataIn(string filepath)
        {
            string Line;
            string[] strs;
            StreamReader sr = new StreamReader(filepath);
            Line = sr.ReadLine();
            strs = Line.Split(',');
            h = double.Parse(strs[0]);

            Line = sr.ReadLine();

            while ((Line = sr.ReadLine()) != null)
            {
                Data kp = new Data();
                strs = Line.Split(',');
                kp.Name = strs[0];
                kp.X = double.Parse(strs[1]);
                kp.Y = double.Parse(strs[2]);
                kp.H = double.Parse(strs[3]);
                points.Add(kp);
            }
        }


        public void DBX()
        {
            //寻找点P0
            for (int i = 0; i < points.Count - 1; i++)//寻找Y最小的点
            {
                for (int j = 0; j < points.Count - 1 - i; j++)
                {
                    if (points[j].Y > points[j + 1].Y)
                    {
                        Data p = new Data();
                        p = points[j];
                        points[j] = points[j + 1];
                        points[j + 1] = p;
                    }
                }
            }
            S sss = new S();
            Jdian.Add(sss);
            Jdian[0].name = "P0";
            Jdian[0].X = points[0].X;
            Jdian[0].Y = points[0].Y;

            //按夹角由小到大进行排序
            for (int i = 1; i < points.Count; i++)
            {
                double k = 0;
                k = Math.Atan(Math.Abs((points[i].Y - Jdian[0].Y)) / Math.Abs((points[i].X - Jdian[0].X)));
                double s = 0;
                s = Math.Sqrt(Math.Pow((points[i].Y - Jdian[0].Y), 2) + Math.Pow((points[i].X - Jdian[0].X), 2));

                if (points[i].X - Jdian[0].X < 0)
                {
                    k = Math.PI - k;
                }
                points[i].K = k;
                points[i].S = s;
            }
            points.Remove(points[0]);
            for (int i = 0; i < points.Count; i++)
            {
                Zheng w = new Zheng();
                w.name = points[i].Name;
                w.X = points[i].X;
                w.Y = points[i].Y;
                w.K = points[i].K;
                w.S = points[i].S;
                zhengshu.Add(w);
            }

            //for (int i = 1; i < points.Count; i++)
            //{
            //    if (points[i].K > 0)
            //    {
            //        Zheng Z = new Zheng();
            //        Z.name = points[i].Name;
            //        Z.X = points[i].X;
            //        Z.Y = points[i].Y;
            //        Z.K = points[i].K;
            //        Z.S = points[i].S;
            //        zhengshu.Add(Z);
            //    }
            //    if (points[i].K < 0)
            //    {
            //        Fu F = new Fu();
            //        F.name = points[i].Name;
            //        F.X = points[i].X;
            //        F.Y = points[i].Y;
            //        F.K = points[i].K;
            //        F.S = points[i].S;
            //        fushu.Add(F);
            //    }
            //}

            for (int i = 0; i < zhengshu.Count - 1; i++)//K正由小到大排序
            {
                for (int j = 0; j < zhengshu.Count - 1 - i; j++)
                {
                    if (zhengshu[j].K > zhengshu[j + 1].K)
                    {
                        Zheng p = new Zheng();
                        p = zhengshu[j];
                        zhengshu[j] = zhengshu[j + 1];
                        zhengshu[j + 1] = p;
                    }
                }
            }

            //for (int i = 0; i < fushu.Count - 1; i++)//K负由小到大排序
            //{
            //    for (int j = 0; j < fushu.Count - 1 - i; j++)
            //    {
            //        if (fushu[j].K < fushu[j + 1].K)
            //        {
            //            Fu p = new Fu();
            //            p = fushu[j];
            //            fushu[j] = fushu[j + 1];
            //            fushu[j + 1] = p;
            //        }
            //    }
            //}

            //for (int i = 0; i < fushu.Count; i++)
            //{
            //    Zheng w = new Zheng();
            //    w.name = fushu[i].name;
            //    w.X = fushu[i].X;
            //    w.Y = fushu[i].Y;
            //    w.K = fushu[i].K;
            //    w.S = fushu[i].S;
            //    zhengshu.Add(w);
            //}

            //建立由凸包点构成的列表或堆栈S
            for (int i = 0; i < 2; i++)
            {
                S r = new S();
                r.name = zhengshu[i].name;
                r.X = zhengshu[i].X;
                r.Y = zhengshu[i].Y;
                r.K = zhengshu[i].K;
                r.SS = zhengshu[i].S;
                Jdian.Add(r);
            }


            for (int i = 2; i < zhengshu.Count; i++)
            {

                S r1 = new S();
                double m = 0;
                double j = 0;
                m = (Jdian[Jdian.Count - 2].X - Jdian[Jdian.Count - 1].X) * (zhengshu[i].Y - Jdian[Jdian.Count - 1].Y) - (Jdian[Jdian.Count - 2].Y - Jdian[Jdian.Count - 1].Y) * (zhengshu[i].X - Jdian[Jdian.Count - 1].X);
                if (m > 0)
                {
                    Jdian.Remove(Jdian[Jdian.Count - 1]);

                    j = Jdian.Count;
                    //r1.name = zhengshu[i].name;
                    //r1.X = zhengshu[i].X;
                    //r1.Y = zhengshu[i].Y;
                    //r1.K = zhengshu[i].K;
                    //r1.SS = zhengshu[i].S;
                    i--;
                }
                else if (m < 0)
                {
                    j = Jdian.Count;
                    r1.name = zhengshu[i].name;
                    r1.X = zhengshu[i].X;
                    r1.Y = zhengshu[i].Y;
                    r1.K = zhengshu[i].K;
                    r1.SS = zhengshu[i].S;
                    Jdian.Add(r1);
                }

            }


            S c = new S();
            c.name = Jdian[0].name;
            c.X = Jdian[0].X;
            c.Y = Jdian[0].Y;
            c.K = Jdian[0].K;
            c.SS = Jdian[0].SS;
            Jdian.Add(c);
        }
        double b = 0;
        double c = 0;

        public void guizegewang(double l)
        {
            //建立外包矩形，并进行二维格网划分

            double Xmin = 999999999;
            double Xmax = 0;
            double Ymin = 999999999;
            double Ymax = 0;

            for (int i = 0; i < Jdian.Count - 1; i++)
            {
                if (Jdian[i].X > Xmax)
                {
                    Xmax = Jdian[i].X;
                }
                if (Jdian[i].X < Xmin)
                {
                    Xmin = Jdian[i].X;
                }
                if (Jdian[i].Y > Ymax)
                {
                    Ymax = Jdian[i].Y;
                }
                if (Jdian[i].Y < Ymin)
                {
                    Ymin = Jdian[i].Y;
                }
            }
            c = (Xmax - Xmin + Ymax - Ymin) / 2 * 0.4;
            //l = ddd;
            for (int i = 1; i * l < Xmax - Xmin; i++)
            {
                for (int j = 1; j * l < Ymax - Ymin; j++)
                {
                    double X = 0;
                    double Y = 0;
                    X = (i * l) - (l / 2) + Xmin;
                    Y = (j * l) - (l / 2) + Ymin;
                    double a = 0;
                    for (int q = 0; q < Jdian.Count - 1; q++)
                    {
                        double xj = 0;
                        double xi = 0;
                        double yj = 0;
                        double yi = 0;
                        xj = Jdian[q + 1].X;
                        xi = Jdian[q].X;
                        yj = Jdian[q + 1].Y;
                        yi = Jdian[q].Y;
                        double x_ = 0;
                        x_ = (xj - xi) / (yj - yi) * (Y - yi) + xi;
                        if (x_ > X)
                        {
                            a++;
                        }

                    }
                    if (a % 2 == 0)
                    {

                    }
                    else
                    {
                        daicha e = new daicha();
                        e.X = X;
                        e.Y = Y;
                        Daicha.Add(e);
                        b++;
                    }
                }
            }



        }

        double Vmax = 0;
        public void tiji()
        {
            //采用反距离加权法求歌王四个顶点的高程
            for (int i = 0; i < Daicha.Count; i++)
            {
                double h1 = 0, h2 = 0, h3 = 0, h4 = 0;
                double s1 = 0, s2 = 0, s3 = 0, s4 = 0;
                double x1 = 0, x2 = 0, x3 = 0, x4 = 0;

                for (int j = 0; j < points.Count; j++)
                {
                    for (int q = 0; q < 4; q++)
                    {
                        if (q == 0)//左上
                        {
                            double d = 0;
                            d = Math.Sqrt(Math.Pow(Daicha[i].X - ddd - points[j].X, 2) + Math.Pow(Daicha[i].Y + ddd - points[j].Y, 2));
                            if (d < c)
                            {
                                s1 = s1 + points[j].H / d;
                                x1 = x1 + 1 / d;
                            }
                        }
                        if (q == 1)//左下
                        {
                            double d = 0;
                            d = Math.Sqrt(Math.Pow(Daicha[i].X - ddd - points[j].X, 2) + Math.Pow(Daicha[i].Y - ddd - points[j].Y, 2));
                            if (d < c)
                            {
                                s2 = s2 + points[j].H / d;
                                x2 = x2 + 1 / d;
                            }
                        }
                        if (q == 2)//右上
                        {
                            double d = 0;
                            d = Math.Sqrt(Math.Pow(Daicha[i].X + ddd - points[j].X, 2) + Math.Pow(Daicha[i].Y + ddd - points[j].Y, 2));
                            if (d < c)
                            {
                                s3 = s3 + points[j].H / d;
                                x3 = x3 + 1 / d;
                            }
                        }
                        if (q == 3)//右下
                        {
                            double d = 0;
                            d = Math.Sqrt(Math.Pow(Daicha[i].X + ddd - points[j].X, 2) + Math.Pow(Daicha[i].Y - ddd - points[j].Y, 2));
                            if (d < c)
                            {
                                s4 = s4 + points[j].H / d;
                                x4 = x4 + 1 / d;
                            }
                        }

                    }
                }
                h1 = s1 / x1;
                Daicha[i].P1 = h1;
                h2 = s2 / x2;
                h3 = s3 / x3;
                h4 = s4 / x4;
                Daicha[i].P2 = h2;
                Daicha[i].P3 = h3;
                Daicha[i].P4 = h4;

            }

            //体积计算
            for (int i = 0; i < Daicha.Count; i++)
            {
                double v = 0;
                v = (((Daicha[i].P1 + Daicha[i].P2 + Daicha[i].P3 + Daicha[i].P4) / 4) - h) * ddd * ddd;
                Daicha[i].V = v;

                Vmax = Vmax + v;
            }
        }
    }
}
