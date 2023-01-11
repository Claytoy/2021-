using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHDM
{
    public class Z_Data
    {
        private string start;//起点
        private double weidu1 = 0;//纬度1
        private double jingdu1 = 0;//经度1
        private string weidu2;//纬度2
        private string jingdu2;//经度2
        private double angle1 = 0;//大地方位角1
        private string angle2;//大地方位角2
        private double longth = 0;//大地线长度
        private string end;//终点

        public string Start
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

        public double Weidu
        {
            get
            {
                return weidu1;
            }

            set
            {
                weidu1 = value;
            }
        }

        public double Jingdu
        {
            get
            {
                return jingdu1;
            }

            set
            {
                jingdu1 = value;
            }
        }

        public double Angle
        {
            get
            {
                return angle1;
            }

            set
            {
                angle1 = value;
            }
        }

        public double Longth
        {
            get
            {
                return longth;
            }

            set
            {
                longth = value;
            }
        }

        public string End
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

        public string Weidu2
        {
            get
            {
                return weidu2;
            }

            set
            {
                weidu2 = value;
            }
        }

        public string Jingdu2
        {
            get
            {
                return jingdu2;
            }

            set
            {
                jingdu2 = value;
            }
        }

        public string Angle2
        {
            get
            {
                return angle2;
            }

            set
            {
                angle2 = value;
            }
        }
    }

    public class F_Data
    {
        private string start;//起点
        private double weidu1 = 0;//纬度1
        private double jingdu1 = 0;//经度1
        private string end;//终点
        private double weidu2 = 0;//纬度2
        private double jingdu2 = 0;//经度2
        private string angle1;//大地角度1
        private string angle2;//大地角度2
        private double longth = 0;//长度

        public string Start
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

        public double Weidu1
        {
            get
            {
                return weidu1;
            }

            set
            {
                weidu1 = value;
            }
        }

        public double Jingdu1
        {
            get
            {
                return jingdu1;
            }

            set
            {
                jingdu1 = value;
            }
        }

        public string End
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

        public double Weidu2
        {
            get
            {
                return weidu2;
            }

            set
            {
                weidu2 = value;
            }
        }

        public double Jingdu2
        {
            get
            {
                return jingdu2;
            }

            set
            {
                jingdu2 = value;
            }
        }

        public string Angle1
        {
            get
            {
                return angle1;
            }

            set
            {
                angle1 = value;
            }
        }

        public string Angle2
        {
            get
            {
                return angle2;
            }

            set
            {
                angle2 = value;
            }
        }

        public double Longth
        {
            get
            {
                return longth;
            }

            set
            {
                longth = value;
            }
        }
    }


}
