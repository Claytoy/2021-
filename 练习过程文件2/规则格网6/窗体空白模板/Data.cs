using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 窗体空白模板
{
    public class Point
    {
        string id;
        double x;
        double y;
        double h;
        double angle;

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
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

        public double Angle
        {
            get
            {
                return angle;
            }

            set
            {
                angle = value;
            }
        }
    }
}
