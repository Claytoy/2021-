using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeodesyLib
{
    public class DataEntity
    {
        public List<GeodesicInfo> Data;
        public List<PointInfo> Points;
        public Ellipsoid Datum;
        public DataEntity()
        {
            Data = new List<GeodesicInfo>();
            Points = new List<PointInfo>();
            Datum = new Ellipsoid();
        }
    }
    public class PointInfo
    {
        public string Name;
        public double B;
        public double L;

        public override string ToString()
        {
            string line = string.Format("{0},{1:f8},{2:f8}", Name, B, L);
            return line;
        }
    }

    public class GeodesicInfo
    {
        public PointInfo P1;
        public PointInfo P2;
        public double A12;
        public double A21;
        public double S;
    }

    public class Ellipsoid
    {
        public double a;
        public double f;
        public double b;
        public double c;
        public double e1;
        public double e2;

        public Ellipsoid()
        {
            a = 6378137.0;
            f = 0.335281066475e-2;
            Init();
        }
        public Ellipsoid(double a, double f)
        {
            this.a = a;
            this.f = f;
            Init();
        }
        void Init()
        {
            b = a * (1 - f);
            c = a * a / b;
            e1 = Math.Sqrt(a * a - this.b * this.b) / a;
            e2 = Math.Sqrt(a * a - this.b * this.b) / b;
                 
        }
    }

}
