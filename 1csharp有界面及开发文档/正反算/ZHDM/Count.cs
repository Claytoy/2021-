using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ZHDM
{
    class Count
    {
        public List<Z_Data> Z_points = new List<Z_Data>();
        public List<F_Data> F_points = new List<F_Data>();
        double a = 0;//长半轴
        double f = 0; //扁率
        double b = 0;//短半轴
        double e1 = 0;//第一偏心率
        double e2 = 0;//第二偏心率
        #region 读数据
        public void Z_DataIn(string filepath)
        {
            string line;
            string[] strs;
            StreamReader sr = new StreamReader(filepath);

            //读长半轴和扁率
            line = sr.ReadLine();
            strs = line.Split(',');
            a = double.Parse(strs[0]);
            f = 1 / double.Parse(strs[1]);

            //读基础数据
            while ((line = sr.ReadLine()) != null)
            {
                strs = line.Split(',');
                Z_Data k1 = new Z_Data();
                k1.Start = strs[0];
                k1.Weidu = double.Parse(strs[1]);
                k1.Jingdu = double.Parse(strs[2]);
                k1.Angle = double.Parse(strs[3]);
                k1.Longth = double.Parse(strs[4]);
                k1.End = strs[5];
                k1.Weidu2 = "0";
                k1.Jingdu2 = "0";
                k1.Angle2 = "0";
                Z_points.Add(k1);
            }
        }

        public void F_DataIn(string filepath)
        {
            string line;
            string[] strs;
            StreamReader sr = new StreamReader(filepath);
            line = sr.ReadLine();
            // 读基础数据
            while ((line = sr.ReadLine()) != null)
            {
                strs = line.Split(',');
                F_Data k2 = new F_Data();
                k2.Start = strs[0];
                k2.Weidu1 = double.Parse(strs[1]);
                k2.Jingdu1 = double.Parse(strs[2]);
                k2.End = strs[3];
                k2.Weidu2 = double.Parse(strs[4]);
                k2.Jingdu2 = double.Parse(strs[5]);
                F_points.Add(k2);
            }
        }
        #endregion

        #region 角度转弧度
        public static double Angle2Rad(double dms)
        {
            double d = Math.Floor(dms);
            double m = Math.Floor(100 * (dms - d));
            double s = Math.Round(100 * (100 * (dms - d) - m), 5);
            double angle = d + m / 60 + s / 3600;
            double rad = Math.PI * angle / 180.0;
            return rad;
        }
        #endregion

        #region 弧度转角度
        public static string Angle2ShowAgle(double S)
        {
            S = S * 180 / Math.PI;
            double d, m, s;
            d = Math.Floor(S);
            m = Math.Floor((S - d) * 10000);
            m = m / 10000 * 60;
            s = Math.Round((m - Math.Floor(m)) * 60, 1);
            //double A = d + Math.Floor(m) / 100 + s / 10000;
            string A = d + "°" + (Math.Floor(m)) + "'" + s + "''";

            return A;
        }
        #endregion

        #region 基本参数
        public void basic()
        {
            b = a * (1 - f);
            e1 = (a * a - b * b) / (a * a);
            e2 = e1 / (1 - e1);
        }
        #endregion

        #region 大地反算
        public void FS()
        {
            for (int i = 0; i < F_points.Count; i++)
            {
                double B1 = Angle2Rad(F_points[i].Weidu1);
                double B2 = Angle2Rad(F_points[i].Weidu2);
                double L1 = Angle2Rad(F_points[i].Jingdu1);
                double L2 = Angle2Rad(F_points[i].Jingdu2);

                //辅助计算
                double u1 = Math.Atan((Math.Sqrt(1 - e1)) * Math.Tan(B1));
                double u2 = Math.Atan((Math.Sqrt(1 - e1)) * Math.Tan(B2));
                double a1 = Math.Sin(u1) * Math.Sin(u2);
                double a2 = Math.Cos(u1) * Math.Cos(u2);
                double b1 = Math.Cos(u1) * Math.Sin(u2);
                double b2 = Math.Sin(u1) * Math.Cos(u2);
                double l = L2 - L1;

                //计算大地方位角
                double gama = 0;
                double xigama = 0;
                double A1 = 0;
                double fai = 0;
                double fai1 = 0;
                double sinA0 = 0;
                double cos2A0 = 0;


                for (int j = 0; ; j++)
                {
                    if (j == 0)
                    {
                        gama = l + 0;
                        double p = 0;
                        double q = 0;

                        p = Math.Cos(u2) * Math.Sin(gama);
                        q = b1 - b2 * Math.Cos(gama);
                        A1 = Math.Atan(p / q);

                        if (A1 > 2 * Math.PI)
                        {
                            A1 = A1 - Math.PI * 2;
                        }
                        if (A1 < 0)
                        {
                            A1 = A1 + 2 * Math.PI;
                        }

                        if (p > 0 && q > 0)
                        {
                            A1 = Math.Abs(A1);
                        }
                        if (p > 0 && q < 0)
                        {
                            A1 = Math.PI - Math.Abs(A1);
                        }
                        if (p < 0 && q < 0)
                        {
                            A1 = Math.PI + Math.Abs(A1);
                        }
                        if (p < 0 && q > 0)
                        {
                            A1 = 2 * Math.PI - Math.Abs(A1);
                        }

                        double sinfai = 0;
                        double cosfai = 0;

                        sinfai = p * Math.Sin(A1) + q * Math.Cos(A1);
                        cosfai = a1 + a2 * Math.Cos(gama);
                        fai = Math.Atan(sinfai);
                        if (cosfai > 0)
                        {
                            fai = Math.Abs(fai);
                        }
                        if (cosfai < 0)
                        {
                            fai = Math.PI - Math.Abs(fai);
                        }




                        cos2A0 = 1 - (sinA0 * sinA0);
                        sinA0 = Math.Cos(u1) * Math.Sin(A1);
                        fai1 = Math.Atan(Math.Tan(u1) / Math.Cos(A1));
                        double alpha = (e1 / 2 + e1 * e1 / 8 + e1 * e1 * e1 / 16) - (e1 * e1 / 16 +
                        Math.Pow(e1, 3) / 16) * cos2A0 + (e1 * Math.Pow(e1, 3) / 128) *
                        Math.Pow(cos2A0, 2);
                        double beta = (e1 * e1 / 16 + Math.Pow(e1, 3) / 16) * cos2A0 -
                            (Math.Pow(e1, 3) / 32) * Math.Pow(cos2A0, 2);
                        double gamma = Math.Pow(e1, 3) / 256 * Math.Pow(cos2A0, 2);

                        double xigama1 = 0;
                        xigama1 = (alpha * fai + beta * Math.Cos(2 * fai1 + fai) * Math.Sin(fai) +
                        gamma * Math.Sin(2 * fai) * Math.Cos(4 * fai1 + 2 * fai)) * sinA0;
                        xigama = xigama1;

                    }
                    else
                    {
                        gama = l + xigama;
                        double p = 0;
                        double q = 0;

                        p = Math.Cos(u2) * Math.Sin(gama);
                        q = b1 - b2 * Math.Cos(gama);
                        A1 = Math.Atan(p / q);

                        if (p > 0 && q > 0)
                        {
                            A1 = Math.Abs(A1);
                        }
                        if (p > 0 && q < 0)
                        {
                            A1 = Math.PI - Math.Abs(A1);
                        }
                        if (p < 0 && q < 0)
                        {
                            A1 = Math.PI + Math.Abs(A1);
                        }
                        if (p < 0 && q > 0)
                        {
                            A1 = 2 * Math.PI - Math.Abs(A1);
                        }
                        if (A1 > 2 * Math.PI)
                        {
                            A1 = A1 - Math.PI * 2;
                        }
                        if (A1 < 0)
                        {
                            A1 = A1 + 2 * Math.PI;
                        }
                        double sinfai = 0;
                        double cosfai = 0;

                        sinfai = p * Math.Sin(A1) + q * Math.Cos(A1);
                        cosfai = a1 + a2 * Math.Cos(gama);
                        fai = Math.Atan(sinfai);
                        if (cosfai > 0)
                        {
                            fai = Math.Abs(fai);
                        }
                        if (cosfai < 0)
                        {
                            fai = Math.PI - Math.Abs(fai);
                        }




                        cos2A0 = 1 - (sinA0 * sinA0);
                        sinA0 = Math.Cos(u1) * Math.Sin(A1);
                        fai1 = Math.Atan(Math.Tan(u1) / Math.Cos(A1));
                        double alpha = (e1 / 2 + e1 * e1 / 8 + e1 * e1 * e1 / 16) - (e1 * e1 / 16 +
                        Math.Pow(e1, 3) / 16) * cos2A0 + (e1 * Math.Pow(e1, 3) / 128) *
                        Math.Pow(cos2A0, 2);
                        double beta = (e1 * e1 / 16 + Math.Pow(e1, 3) / 16) * cos2A0 -
                            (Math.Pow(e1, 3) / 32) * Math.Pow(cos2A0, 2);
                        double gamma = Math.Pow(e1, 3) / 256 * Math.Pow(cos2A0, 2);

                        double xigama1 = 0;
                        xigama1 = (alpha * fai + beta * Math.Cos(2 * fai1 + fai) * Math.Sin(fai) +
                        gamma * Math.Sin(2 * fai) * Math.Cos(4 * fai1 + 2 * fai)) * sinA0;
                        if ((xigama1 - xigama) < 0.0000000001)
                        {
                            xigama = xigama1;
                            break;
                        }
                        xigama = xigama1;

                    }
                }
                //计算大地线长度
                fai1 = Math.Atan(Math.Tan(u1) / Math.Cos(A1));
                double k2 = e2 * cos2A0;
                double A = (1 - k2 / 4 + 7 * k2 * k2 / 64 - 15 * Math.Pow(k2, 3) / 256) / b;
                double B = (k2 / 4 - k2 * k2 / 8 + 37 * Math.Pow(k2, 3) / 512);
                double C = (k2 * k2 / 128 - Math.Pow(k2, 3) / 128);
                double Xs = C * Math.Sin(2 * fai) * Math.Cos(4 * fai1 + 2 * fai);
                double S = (fai - B * Math.Sin(fai) * Math.Cos(2 * fai1 + fai) - Xs) / A;
                double A2 = 0;
                A2 = Math.Atan((Math.Cos(u1) * Math.Sin(gama)) / ((b1 * Math.Cos(gama)) - b2));
                if (A2 < 0)
                {
                    A2 = A2 + Math.PI * 2;
                }
                if (A2 > Math.PI * 2)
                {
                    A2 = A2 - Math.PI * 2;
                }
                if (A1 < Math.PI && A2 < Math.PI)
                {
                    A2 = A2 + Math.PI;
                }
                if (A1 > Math.PI && A2 > Math.PI)
                {
                    A2 = A2 - Math.PI;
                }

                F_points[i].Angle1 = Angle2ShowAgle(A1);
                F_points[i].Angle2 = Angle2ShowAgle(A2);
                F_points[i].Longth = S;
            }
        }
        #endregion

        #region 大地正算
        public void ZS()
        {
            for (int i = 0; i < Z_points.Count; i++)
            {
                double B1 = Angle2Rad(Z_points[i].Weidu);
                double L1 = Angle2Rad(Z_points[i].Jingdu);
                double A1 = Angle2Rad(Z_points[i].Angle);

                //计算起点的归化纬度
                double W1 = Math.Sqrt(1 - (e1 * Math.Sin(B1) * Math.Sin(B1)));
                double sinu1 = (Math.Sin(B1) * Math.Sqrt(1 - e1)) / W1;
                double cosu1 = Math.Cos(B1) / W1;

                //计算辅助函数
                double sinA0 = cosu1 * Math.Sin(A1);
                double cotfai1 = cosu1 * Math.Sin(A1) / sinu1;
                double fai1 = Math.Atan(1 / cotfai1);

                //计算系数
                double cos2A0 = 1 - Math.Pow(sinA0, 2);
                double k2 = e2 * cos2A0;
                double A = (1 - k2 / 4 + 7 * k2 * k2 / 64 - 15 * Math.Pow(k2, 3) / 256) / b;
                double B = (k2 / 4 - k2 * k2 / 8 + 37 * Math.Pow(k2, 3) / 512);
                double C = (k2 * k2 / 128 - Math.Pow(k2, 3) / 128);
                double alpha = (e1 / 2 + e1 * e1 / 8 + e1 * e1 * e1 / 16) - (e1 * e1 / 16 +
                        Math.Pow(e1, 3) / 16) * cos2A0 + (e1 * Math.Pow(e1, 3) / 128) *
                        Math.Pow(cos2A0, 2);
                double beta = (e1 * e1 / 16 + Math.Pow(e1, 3) / 16) * cos2A0 -
                    (Math.Pow(e1, 3) / 32) * Math.Pow(cos2A0, 2);
                double gamma = Math.Pow(e1, 3) / 256 * Math.Pow(cos2A0, 2);

                //计算球面长度
                double S = Z_points[i].Longth;
                double xigama = A * S;
                for (int j = 0; ; i++)
                {
                    double x = A * S + B * Math.Sin(xigama) * Math.Cos(2 * fai1 + xigama)
                        + C * Math.Sin(2 * xigama) * Math.Cos(4 * fai1 + 2 * xigama);
                    if (Math.Abs(x - xigama) < 0.0000000001)
                    {
                        break;
                    }
                    else
                    {
                        xigama = x;
                    }

                }

                //计算经度差改正数
                double aa = (alpha * xigama + beta * Math.Cos(2 * fai1 + xigama) * Math.Sin(xigama) +
                        gamma * Math.Sin(2 * xigama) * Math.Cos(4 * fai1 + 2 * xigama)) * sinA0;

                //计算终点大地坐标及坐标方位角
                double sinu2 = sinu1 * Math.Cos(xigama) + cosu1 * Math.Cos(A1) * Math.Sin(xigama);
                double B2 = Math.Atan(sinu2 / (Math.Sqrt(1 - e1) * Math.Sqrt(1 - sinu2 * sinu2)));
                double rou = Math.Atan(Math.Sin(A1) * Math.Sin(xigama) / (cosu1 * Math.Cos(xigama) - sinu1 * Math.Sin(xigama) * Math.Cos(A1)));
                if (Math.Sin(A1) > 0 && Math.Tan(rou) > 0)
                {
                    rou = Math.Abs(rou);
                }
                if (Math.Sin(A1) > 0 && Math.Tan(rou) < 0)
                {
                    rou = Math.PI - Math.Abs(rou);
                }
                if (Math.Sin(A1) < 0 && Math.Tan(rou) < 0)
                {
                    rou = -Math.Abs(rou);
                }
                if (Math.Sin(A1) < 0 && Math.Tan(rou) > 0)
                {
                    rou = Math.Abs(rou) - Math.PI;
                }
                double L2 = L1 + rou - aa;
                double A2 = Math.Atan(cosu1 * Math.Sin(A1) / (cosu1 * Math.Cos(xigama) * Math.Cos(A1) - sinu1 * Math.Sin(xigama)));
                if (Math.Sin(A1) < 0 && Math.Tan(A2) > 0)
                {
                    A2 = Math.Abs(A2);
                }
                if (Math.Sin(A1) < 0 && Math.Tan(A2) < 0)
                {
                    A2 = Math.PI - Math.Abs(A2);
                }
                if (Math.Sin(A1) > 0 && Math.Tan(A2) > 0)
                {
                    A2 = Math.PI + Math.Abs(A2);
                }
                if (Math.Sin(A1) < 0 && Math.Tan(A2) < 0)
                {
                    A2 = Math.PI * 2 - Math.Abs(A2);
                }
                if (A2 < 0)
                {
                    A2 = A2 + Math.PI * 2;
                }
                if (A2 > Math.PI * 2)
                {
                    A2 = A2 - Math.PI * 2;
                }

                Z_points[i].Weidu2 = Angle2ShowAgle(B2);
                Z_points[i].Jingdu2 = Angle2ShowAgle(L2);
                Z_points[i].Angle2 = Angle2ShowAgle(A2);
            }
        }
        #endregion
    }
}
