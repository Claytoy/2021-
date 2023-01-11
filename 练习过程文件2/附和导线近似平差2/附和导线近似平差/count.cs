using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 附和导线近似平差
{
    class count
    {
        public static double toS(double jiaodu)
        {
            double d, m, s;
            d = Math.Floor(jiaodu);
            m = Math.Floor((jiaodu - d )* 100.000) ;
            s =((jiaodu * 100.000) - (d * 100.000 + m))*100.000 ;
            double A = 0;
            A = d * 3600.000 + m * 60.000 + s;
            A = Math.Round(A, 5);
            return A;
        }

        public static double toAngle(double S)
        {
            double d, m, s;
            s = Math.Round(S % 60,3);
            m = Math.Round(((S - s) / 60) % 60,2);
            d = Math.Round((S - (m*60) - s) / 3600,2);
            double A = 0;
            A = d + (m  / 100.000) + (s  / 10000.000);
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
    }
}
