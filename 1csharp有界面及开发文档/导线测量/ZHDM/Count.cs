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
        public List<Data> points = new List<Data>();
        double zhongwucha = 0;//测角中误差
        double jiachangshu = 0;//加常数
        double chengchangshu = 0;//乘常数

        #region 导入数据
        public void dataIn(string filepath)
        {
            string line;
            string[] strs;
            StreamReader sr = new StreamReader(filepath);

            double x1, x2, x3, x4;
            double y1, y2, y3, y4;
            string a, b;

            //存中误差等数据
            line = sr.ReadLine();
            strs = line.Split(',');
            zhongwucha = double.Parse(strs[0]);
            jiachangshu = double.Parse(strs[1]);
            chengchangshu = double.Parse(strs[2]);

            //已知4个点的XY
            Data k0 = new Data();

            line = sr.ReadLine();
            strs = line.Split(',');
            x1 = double.Parse(strs[1]);
            y1 = double.Parse(strs[2]);
            k0.C_name = strs[0];
            k0.X = x1;
            k0.Y = y1;
            points.Add(k0);

            line = sr.ReadLine();
            strs = line.Split(',');
            x2 = double.Parse(strs[1]);
            y2 = double.Parse(strs[2]);

            line = sr.ReadLine();
            strs = line.Split(',');
            x3 = double.Parse(strs[1]);
            y3 = double.Parse(strs[2]);
            b = strs[0];

            line = sr.ReadLine();
            strs = line.Split(',');
            x4 = double.Parse(strs[1]);
            y4 = double.Parse(strs[2]);
            a = strs[0];


            //存所有点基础信息和角度

            for (int i = 0; ; i++)
            {
                Data kp = new Data();
                line = sr.ReadLine();
                strs = line.Split(',');
                kp.C_name = strs[0];

                line = sr.ReadLine();
                strs = line.Split(',');
                kp.H_station = strs[0];
                kp.H_angle = double.Parse(strs[2]);

                line = sr.ReadLine();
                strs = line.Split(',');
                kp.Q_station = strs[0];
                kp.Q_angle = double.Parse(strs[2]);

                points.Add(kp);
                if (strs[0] == a)
                {
                    break;
                }
            }
            Data k1 = new Data();
            k1.C_name = a;
            k1.X = x4;
            k1.Y = y4;
            k1.H_station = points[points.Count - 1].C_name;
            points.Add(k1);

            points[0].Q_station = points[1].C_name;

            //存已知4个点得XY
            points[1].X = x2;
            points[1].Y = y2;


            points[points.Count - 2].X = x3;
            points[points.Count - 2].Y = y3;

            //存测站距离
            for (int i = 0; ; i++)
            {
                line = sr.ReadLine();
                line = sr.ReadLine();
                strs = line.Split(',');
                points[i + 1].Q_longth = double.Parse(strs[2]);
                if (strs[0] == b)
                {
                    break;
                }
            }
        }
        #endregion

        #region 弧度角度计算
        public static double toS(double jiaodu)
        {
            double d, m, s;
            d = Math.Floor(jiaodu);
            m = Math.Floor((jiaodu - d) * 100.000);
            s = ((jiaodu * 100.000) - (d * 100.000 + m)) * 100.000;
            double A = 0;
            A = d * 3600.000 + m * 60.000 + s;
            A = Math.Round(A, 5);
            return A;
        }

        public static double toAngle(double S)
        {
            double d, m, s;
            s = Math.Round(S % 60, 3);
            m = Math.Round(((S - s) / 60) % 60, 2);
            d = Math.Round((S - (m * 60) - s) / 3600, 2);
            double A = 0;
            A = d + (m / 100.000) + (s / 10000.000);
            A = Math.Round(A, 6);
            return A;
        }

        public static double tentoangle(double hudu)//计算过程中数据导出需要
        {
            double du, d, m, s;
            du = hudu;
            d = Math.Floor(du);
            m = Math.Floor((du - d) * 10000);
            m = m / 10000 * 60;
            s = Math.Round((m - Math.Floor(m)) * 60, 1);
            double A = d + Math.Floor(m) / 100 + s / 10000;
            A = Math.Round(A, 6);


            return A;
        }

        public static double angle2t(double T)
        {
            double d = Math.Floor(T);
            double m = Math.Floor((T - d) * 100);
            double s = Math.Round(((T - d) * 100 - m) * 100, 3);//保留到0.1秒
            double A = d + m / 60 + s / 6000;

            return A;
        }
        #endregion

        #region 1.观测值记录簿
        public void guance()
        {
            //起始方位角
            double alpha12 = Math.Atan((points[1].Y - points[0].Y) / (points[1].X - points[0].X));

            //方位角限定条件
            if ((points[1].X - points[0].X) < 0)
            {
                alpha12 = alpha12 + Math.PI;
            }
            if ((points[1].X - points[0].X) > 0)
            {
                alpha12 = alpha12;
            }
            if ((points[1].Y - points[0].Y) > 0 && (points[1].X - points[0].X) == 0)
            {
                alpha12 = Math.PI / 2;
            }
            if ((points[1].Y - points[0].Y) < 0 && (points[1].X - points[0].X) == 0)
            {
                alpha12 = Math.PI * 3 / 2;
            }
            if (alpha12 < 0)
            {
                alpha12 = alpha12 + 2 * Math.PI;
            }
            if (alpha12 > 2 * Math.PI)
            {
                alpha12 = alpha12 - 2 * Math.PI;
            }
            points[0].Alpha = alpha12;
            //坐标反算边长
            double S12 = (points[1].Y - points[0].Y) / Math.Sin(alpha12);

            alpha12 = alpha12 + 2 * Math.PI;

            //转角计算
            for (int i = 1; i < points.Count - 1; i++)
            {
                double beta = 0;
                beta = points[i].Q_angle - points[i].H_angle;
                beta = angle2t(beta);
                beta = beta / 180 * Math.PI;
                if (i == 1)
                {
                    points[i].Alpha = alpha12 + beta - Math.PI;
                }
                else
                {
                    points[i].Alpha = points[i - 1].Alpha + beta - Math.PI;
                }

            }
        }
        #endregion

        #region 2.角度近似平差
        public void jinsipingcha()
        {
            //计算方位角闭合差
            double Beta = 0;
            for (int i = 0; i < points.Count; i++)
            {
                double c = 0;
                c = angle2t(points[i].Q_angle) / 180 * Math.PI;
                Beta = c + Beta;
            }
            double fwjbhc = 0;
            fwjbhc = -1.0 * points[points.Count - 2].Alpha + points[0].Alpha + Beta - Math.PI * (points.Count - 1) + Math.PI * 3;
            fwjbhc = tentoangle(fwjbhc / Math.PI * 180);

            //计算改正后得各转角
            double vbeta = 0;
            vbeta = -fwjbhc / (points.Count - 2);

            points[0].V_alpha = points[0].Alpha;
            for (int i = 1; i < points.Count - 1; i++)
            {

                double d = angle2t(points[i].Q_angle + vbeta) / 180 * Math.PI;
                points[i].V_beta = d;
                if (i == 1)
                {
                    points[i].V_alpha = points[i - 1].Alpha + 2 * Math.PI + d - Math.PI;
                }
                else
                {
                    points[i].V_alpha = points[i - 1].Alpha + d - Math.PI;
                }
            }
        }
        #endregion

        #region 3.坐标近似平差
        public void zuobiao()
        {
            //计算横纵坐标增量
            for(int i = 1;i < points.Count -2;i++)
            {
                points[i].D_X = points[i].Q_longth * Math.Cos(points[i].V_alpha);
                points[i].D_Y = points[i].Q_longth * Math.Sin(points[i].V_alpha);
            }

            //计算坐标增量和坐标
            double DX = 0;
            double DY = 0;
            for(int i = 0;i < points.Count;i++)
            {
                DX += points[i].D_X;
                DY += points[i].D_Y;
            }
            double fx = 0;
            double fy = 0;
            fx = points[1].X - points[points.Count - 1].X + DX;
            fy = points[1].Y - points[points.Count - 1].Y + DY;

            double s = 0;
            for(int i = 1;i < points.Count-1;i++)
            {
                s += points[i].Q_longth;
            }

            double X = points[1].X;
            double Y = points[1].Y;
            for(int i = 1;i < points.Count-2;i++)
            {
                double Vx = 0;
                double Vy = 0;
                Vx = -fx / s * points[i].Q_longth;
                Vy = -fy / s * points[i].Q_longth;

                X = X + points[i].D_X + Vx;
                Y = Y + points[i].D_Y + Vy;

                points[i].X = X;
                points[i].Y = Y;
            }
        }
        #endregion
    }
}
