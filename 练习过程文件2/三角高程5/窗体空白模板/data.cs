using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 窗体空白模板
{
    public class data
    {
        private string end;
        private double g_station = 0;
        private double g_aim = 0;
        private double g_longth = 0;
        private string start;
        private double g_angle = 0;
        private double c_station = 0;
        private double c_aim = 0;
        private double c_longth = 0;
        private double c_angle = 0;
        private double g_D = 0;
        private double g_f = 0;
        private double c_D = 0;
        private double c_f = 0;
        private double g_h = 0;
        private double c_h = 0;
        private double h = 0;
        private double v = 0;

        public string Start//起始点
        {
            get
            {
                return start;
            }

            set
            {
                start = value;
            }
        }

        public string End//终止点
        {
            get
            {
                return end;
            }

            set
            {
                end = value;
            }
        }

        public double G_station//往测斜距
        {
            get
            {
                return g_station;
            }

            set
            {
                g_station = value;
            }
        }

        public double G_aim//往测垂直角
        {
            get
            {
                return g_aim;
            }

            set
            {
                g_aim = value;
            }
        }

        public double G_longth//往测仪器高
        {
            get
            {
                return g_longth;
            }

            set
            {
                g_longth = value;
            }
        }

        public double G_angle//往测目标高
        {
            get
            {
                return g_angle;
            }

            set
            {
                g_angle = value;
            }
        }

        public double C_station//返测斜距
        {
            get
            {
                return c_station;
            }

            set
            {
                c_station = value;
            }
        }

        public double C_aim//返测垂直角
        {
            get
            {
                return c_aim;
            }

            set
            {
                c_aim = value;
            }
        }

        public double C_longth//返测仪器高
        {
            get
            {
                return c_longth;
            }

            set
            {
                c_longth = value;
            }
        }

        public double C_angle//返测目标高
        {
            get
            {
                return c_angle;
            }

            set
            {
                c_angle = value;
            }
        }

        public double G_D//往测平距
        {
            get
            {
                return g_D;
            }

            set
            {
                g_D = value;
            }
        }

        public double G_f//往测球气差
        {
            get
            {
                return g_f;
            }

            set
            {
                g_f = value;
            }
        }

        public double C_D//返测平距
        {
            get
            {
                return c_D;
            }

            set
            {
                c_D = value;
            }
        }

        public double C_f//返测球气差
        {
            get
            {
                return c_f;
            }

            set
            {
                c_f = value;
            }
        }

        public double G_h//往测高差
        {
            get
            {
                return g_h;
            }

            set
            {
                g_h = value;
            }
        }

        public double C_h//返测高差
        {
            get
            {
                return c_h;
            }

            set
            {
                c_h = value;
            }
        }

        public double H//高差平均值
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

        public double V//改正数
        {
            get
            {
                return v;
            }

            set
            {
                v = value;
            }
        }
    }

    public class Point
    {
        private string name;
        private double height = 0;
        private double v_height = 0;


        public string Name//点名
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

        public double Height//高程
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
            }
        }

        public double V_height
        {
            get
            {
                return v_height;
            }

            set
            {
                v_height = value;
            }
        }
    }

    public class Result
    {
        public double fh;
        public List<double> Ls = new List<double>();
        public List<double> Hs = new List<double>();
    }


    public class Calculate
    {
        public List<data> Data = new List<data>();
        public List<Point> points = new List<Point>();
        public Result R = new Result();
    }
}
