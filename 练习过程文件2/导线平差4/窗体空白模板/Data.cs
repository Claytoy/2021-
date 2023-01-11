using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 窗体空白模板
{
    public class Result
    {
        double _fh;
        public List<double> Vis = new List<double>();
        public List<double> hibas = new List<double>();
        public List<double> Lis = new List<double>();

        public List<double> EDS = new List<double>();
        public List<double> S = new List<double>();
        public List<double> GCS = new List<double>();
        public List<double> HIS = new List<double>();
        public double Fh
        {
            get
            {
                return _fh;
            }

            set
            {
                _fh = value;
            }
        }
    }
    public class Point
    {
        string _start;
        string _end;
        double _hsjl1;
        double _hszsds1;
        double _qsjl1;
        double _qszsds1;
        double _qsjl2;
        double _qszsds2;
        double _hsjl2;
        double _hszsds2;
        double _height;

        double jlc1;
        double jlc2;
        double _epsilonD;
        double _d;
        double _gc1;
        double _gc2;
        double _hszsc;
        double _qszsc;
        double _zsc;
        double _gc;

        double _s;
        public double Hszsds2
        {
            get
            {
                return _hszsds2;
            }

            set
            {
                _hszsds2 = value;
            }
        }

        public string Start
        {
            get
            {
                return _start;
            }

            set
            {
                _start = value;
            }
        }

        public string End
        {
            get
            {
                return _end;
            }

            set
            {
                _end = value;
            }
        }

        public double Hsjl1
        {
            get
            {
                return _hsjl1;
            }

            set
            {
                _hsjl1 = value;
            }
        }

        public double Hszsds1
        {
            get
            {
                return _hszsds1;
            }

            set
            {
                _hszsds1 = value;
            }
        }

        public double Qsjl1
        {
            get
            {
                return _qsjl1;
            }

            set
            {
                _qsjl1 = value;
            }
        }

        public double Qszsds1
        {
            get
            {
                return _qszsds1;
            }

            set
            {
                _qszsds1 = value;
            }
        }

        public double Qsjl2
        {
            get
            {
                return _qsjl2;
            }

            set
            {
                _qsjl2 = value;
            }
        }

        public double Qszsds2
        {
            get
            {
                return _qszsds2;
            }

            set
            {
                _qszsds2 = value;
            }
        }

        public double Hsjl2
        {
            get
            {
                return _hsjl2;
            }

            set
            {
                _hsjl2 = value;
            }
        }

        public double Height
        {
            get
            {
                return _height;
            }

            set
            {
                _height = value;
            }
        }

        public double EpsilonD
        {
            get
            {
                return _epsilonD;
            }

            set
            {
                _epsilonD = value;
            }
        }

        public double Jlc1
        {
            get
            {
                return jlc1;
            }

            set
            {
                jlc1 = value;
            }
        }

        public double Jlc2
        {
            get
            {
                return jlc2;
            }

            set
            {
                jlc2 = value;
            }
        }

        public double D
        {
            get
            {
                return _d;
            }

            set
            {
                _d = value;
            }
        }

        public double Gc1
        {
            get
            {
                return _gc1;
            }

            set
            {
                _gc1 = value;
            }
        }

        public double Gc2
        {
            get
            {
                return _gc2;
            }

            set
            {
                _gc2 = value;
            }
        }

        public double Hszsc
        {
            get
            {
                return _hszsc;
            }

            set
            {
                _hszsc = value;
            }
        }

        public double Qszsc
        {
            get
            {
                return _qszsc;
            }

            set
            {
                _qszsc = value;
            }
        }

        public double Zsc
        {
            get
            {
                return _zsc;
            }

            set
            {
                _zsc = value;
            }
        }

        public double Gc
        {
            get
            {
                return _gc;
            }

            set
            {
                _gc = value;
            }
        }

        public double S
        {
            get
            {
                return _s;
            }

            set
            {
                _s = value;
            }
        }
    }
}
