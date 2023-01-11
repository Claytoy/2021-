using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 窗体空白模板
{
    class turn
    {
        public static double angle2rad(double T)
        {
            double d = Math.Floor(Math.Abs(T));
            double m = Math.Floor(100 * (Math.Abs(T) - d));
            double s = Math.Round(100 * (100 * (Math.Abs(T) - d) - m), 5);
            double angle = d + m / 60 + s / 3600;
            double rad = Math.PI * angle / 180.0;
            if (T < 0)
            {
                rad = -rad;
            }
            return rad;
        }
    }
}
