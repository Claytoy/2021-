using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHDM
{
    public class Data
    {
        private string c_name;//测站名
        private string h_station;//后测站名
        private double h_angle = 0;//后测站角度
        private string q_station;//前侧站名
        private double q_angle = 0;//前测站角度
        private double q_longth = 0;//前侧站距离
        private double x = 0;//测站X
        private double y = 0;//测站Y
        private double alpha = 0;//方位角
        private double v_beta = 0;//改正后的转折角
        private double v_alpha = 0;//改正后的方位角
        public double D_X = 0;//X坐标增量
        public double D_Y = 0;//Y坐标增量

        public string C_name
        {
            get
            {
                return c_name;
            }

            set
            {
                c_name = value;
            }
        }

        public string H_station
        {
            get
            {
                return h_station;
            }

            set
            {
                h_station = value;
            }
        }

        public double H_angle
        {
            get
            {
                return h_angle;
            }

            set
            {
                h_angle = value;
            }
        }

        public string Q_station
        {
            get
            {
                return q_station;
            }

            set
            {
                q_station = value;
            }
        }

        public double Q_angle
        {
            get
            {
                return q_angle;
            }

            set
            {
                q_angle = value;
            }
        }

        public double Q_longth
        {
            get
            {
                return q_longth;
            }

            set
            {
                q_longth = value;
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

        public double Alpha
        {
            get
            {
                return alpha;
            }

            set
            {
                alpha = value;
            }
        }

        public double V_beta
        {
            get
            {
                return v_beta;
            }

            set
            {
                v_beta = value;
            }
        }

        public double V_alpha
        {
            get
            {
                return v_alpha;
            }

            set
            {
                v_alpha = value;
            }
        }
    }

    
}
