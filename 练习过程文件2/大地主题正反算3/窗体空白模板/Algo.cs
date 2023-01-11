using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 窗体空白模板
{
    public class ZSPoint
    {
        string _start;
        string _end;
        double _longitude;//经度
        double _latitude;//纬度
        double _azimuth;//大地方位角
        double _logw;//大地线长度

        public string Start
        {
            get { return _start; }
            set { _start = value; }
        }
        public string End
        {
            get { return _end; }
            set { _end = value; }
        }
        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }
        public double Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }
        public double Azimuth
        {
            get { return _azimuth; }
            set { _azimuth = value; }
        }
        public double Logw
        {
            get { return _logw; }
            set { _logw = value; }
        }

    }
    public class FSPoint
    {
        string _start;
        string _end;
        double _longitude1;//经度1
        double _latitude1;//纬度1
        double _longitude2;//2
        double _latitude2;//2

        public string Start
        {
            get { return _start; }
            set { _start = value; }
        }
        public string End
        {
            get { return _end; }
            set { _end = value; }
        }
        public double Longitude1
        {
            get { return _longitude1; }
            set { _longitude1 = value; }
        }
        public double Latitude1
        {
            get { return _latitude1; }
            set { _latitude1 = value; }
        }
        public double Longitude2
        {
            get { return _longitude2; }
            set { _longitude2 = value; }
        }
        public double Latitude2
        {
            get { return _latitude2; }
            set { _latitude2 = value; }
        }

    }
    class Result
    {
        public List<double> S = new List<double>();
        public List<string> A1 = new List<string>();

        public List<string> B2 = new List<string>();
        public List<string> A2 = new List<string>();
        public List<string> L2 = new List<string>();
    }
    class Algo
    {
        #region ddmmss转弧度
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
        public static double Rad2Angle(double rad)
        {
            double angle = rad * 180.0 / Math.PI;
            return angle;
        }
        #endregion

        #region 转换为用于显示的角度ddmmss
        public static string  Angle2ShowAgle(double S)
        {
            
            double d, m, s;
            d = Math.Floor(S);
            m = Math.Floor((S - d) * 10000);
            m = m / 10000 * 60;
            s = Math.Round((m - Math.Floor(m)) * 60, 1);
            //double A = d + Math.Floor(m) / 100 + s / 10000;
            string A = d + "°"+(Math.Floor(m)) +"'"+ s +"''";

            return A;
        }
        #endregion
    }
}
