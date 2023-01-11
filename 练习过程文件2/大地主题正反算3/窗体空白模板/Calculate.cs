using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace 窗体空白模板
{
    class Calculate
    {
        public double a;
        double b;
        public double f;
        public double e1;
        public double e2;
        public List<ZSPoint> ZSPoints = new List<ZSPoint>();
        public List<FSPoint> FSPoints = new List<FSPoint>();
        public Result R = new Result();
        public void GetFSData(string filepath)
        {
            string line;
            string[] strs;
            StreamReader sr = new StreamReader(filepath);
            line = sr.ReadLine();
            strs = line.Split(',');
            a = double.Parse(strs[0]);
            f = double.Parse(strs[1]);
            f = 1 / f;
            b = a * (1 - f);
            e1 = (a * a - b * b) / (a * a);
            e2 = e1 / (1 - e1);
            while ((line = sr.ReadLine()) != null)
            {
                strs = line.Split(',');
                FSPoint p = new FSPoint();
                p.Start = strs[0];
                p.Latitude1 = double.Parse(strs[1]);
                p.Longitude1 = double.Parse(strs[2]);
                p.End = strs[3];
                p.Latitude2 = double.Parse(strs[4]);
                p.Longitude2 = double.Parse(strs[5]);
                FSPoints.Add(p);

            }
        }

        public void GetZSData(string filepath)
        {
            string line;
            string[] strs;
            StreamReader sr = new StreamReader(filepath);
            line = sr.ReadLine();
            strs = line.Split(',');
            a = double.Parse(strs[0]);
            f = double.Parse(strs[1]);
            f = 1 / f;
            b = a * (1 - f);
            e1 = (a * a - b * b) / (a * a);
            e2 = e1 / (1 - e1);
            while ((line = sr.ReadLine()) != null)
            {
                strs = line.Split(',');
                ZSPoint p = new ZSPoint();
                p.Start = strs[0];
                p.Latitude = double.Parse(strs[1]);
                p.Longitude = double.Parse(strs[2]);
                p.Azimuth = double.Parse(strs[3]);
                p.Logw = double.Parse(strs[4]);
                p.End = strs[5];
                ZSPoints.Add(p);

            }
        }

        public void FSCal()
        {
            for (int i = 0; i < FSPoints.Count(); i++)
            {
                //辅助计算
                double B1 = Algo.Angle2Rad(FSPoints[i].Latitude1);//纬度1
                double B2 = Algo.Angle2Rad(FSPoints[i].Latitude2);//纬度2
                double L1 = Algo.Angle2Rad(FSPoints[i].Longitude1);//经度1
                double L2 = Algo.Angle2Rad(FSPoints[i].Longitude2);//经度2

                double u1 = Math.Atan((Math.Sqrt(1 - e1)) * Math.Tan(B1));
                double u2 = Math.Atan((Math.Sqrt(1 - e1)) * Math.Tan(B2));
                double a1 = Math.Sin(u1) * Math.Sin(u2);
                double a2 = Math.Cos(u1) * Math.Cos(u2);
                double b1 = Math.Cos(u1) * Math.Sin(u2);
                double b2 = Math.Sin(u1) * Math.Cos(u2);
                double l = L2 - L1;

                //计算起点大地方位角
                double sinA0;
                double A1 = 0;
                double cosA0;
                double delta1 = 0;
                double delta2;
                double lambda = l;
                double sigma;
                double sigma1;
                do
                {
                    delta2 = delta1;
                    double p = Math.Cos(u2) * Math.Sin(lambda);
                    double q = b1 - b2 * Math.Cos(lambda);
                    A1 = Math.Atan(p / q);
                    if (p > 0)
                    {
                        if (q > 0)
                        {
                            A1 = Math.Abs(A1);
                        }
                        else if (q < 0)
                        {
                            A1 = Math.PI - Math.Abs(A1);
                        }
                        else if (p < 0)
                        {
                            if (q < 0)
                            {
                                A1 = Math.PI + Math.Abs(A1);
                            }
                            else if (q > 0)
                            {
                                A1 = Math.PI * 2 - Math.Abs(A1);
                            }
                        }
                    }
                    if (A1 < 0)
                    {
                        A1 = A1 + 2 * Math.PI;
                    }
                    else if (A1 > 2 * Math.PI)
                    {
                        A1 = A1 - 2 * Math.PI;
                    }

                    double sinsigma = p * Math.Sin(A1) + q * Math.Cos(A1);
                    double cossigma = a1 + a2 * Math.Cos(lambda);
                    sigma = Math.Atan(sinsigma / cossigma);

                    if (cossigma > 0)
                    {
                        sigma = Math.Abs(sigma);
                    }
                    else if (cossigma < 0)
                    {
                        sigma = Math.PI - Math.Abs(sigma);
                    }

                    sinA0 = Math.Cos(u1) * Math.Sin(A1);
                    sigma1 = Math.Atan(Math.Tan(u1) / Math.Cos(A1));

                    //计算系数alpha,beta,gamma
                    cosA0 = Math.Sqrt(1 - sinA0 * sinA0);
                    double alpha = (e1 / 2 + e1 * e1 / 8 + e1 * e1 * e1 / 16) - (e1 * e1 / 16 +
                        Math.Pow(e1, 3) / 16) * cosA0 * cosA0 + (e1 * Math.Pow(e1, 3) / 128) *
                        Math.Pow(cosA0, 4);
                    double beta = (e1 * e1 / 16 + Math.Pow(e1, 3) / 16) * cosA0 * cosA0 -
                        (Math.Pow(e1, 3) / 32) * Math.Pow(cosA0, 4);
                    double gamma = Math.Pow(e1, 3) / 256 * Math.Pow(cosA0, 4);

                    delta1 = (alpha * sigma + beta * Math.Cos(2 * sigma1 + sigma) * Math.Sin(sigma) +
                        gamma * Math.Sin(2 * sigma) * Math.Cos(4 * sigma1 + 2 * sigma)) * sinA0;


                }
                while (Math.Abs(delta2 - delta1) > 0.0000000001);
                R.A1.Add(Algo.Angle2ShowAgle(A1));
                //计算ABC及大地线长度S
                cosA0 = Math.Sqrt(1 - sinA0 * sinA0);
                double k2 = e2 * cosA0 * cosA0;
                double A = (1 - k2 / 4 + 7 * k2 * k2 / 64 - 15 * Math.Pow(k2, 3) / 256) / b;
                double B = (k2 / 4 - k2 * k2 / 8 + 37 * Math.Pow(k2, 3) / 512);
                double C = (k2 * k2 / 128 - Math.Pow(k2, 3) / 128);
                sigma1 = Math.Atan(Math.Tan(u1) / Math.Cos(A1));
                double Xs = C * Math.Sin(2 * sigma) * Math.Cos(4 * sigma1 + 2 * sigma);
                double S = (sigma - B * Math.Sin(sigma) * Math.Cos(2 * sigma1 + sigma) - Xs) / A;
                R.S.Add(S);
            }
        }


        public void ZSCal()
        {
            for (int i = 0; i < ZSPoints.Count(); i++)
            {
                double B1 = Algo.Angle2Rad(ZSPoints[i].Latitude);//纬度
                double L1 = Algo.Angle2Rad(ZSPoints[i].Longitude);//经度
                double A1 = Algo.Angle2Rad(ZSPoints[i].Azimuth);//大地方位角
                double S = ZSPoints[i].Logw;

                //计算起点的归化纬度
                double w1 = Math.Sqrt(1 - e1 * Math.Sin(B1) * Math.Sin(B1));
                double sinu1 = Math.Sin(B1) * Math.Sqrt(1 - e1) / w1;
                double cosu1 = Math.Cos(B1) / w1;
                //计算辅助函数值
                double sinA0 = cosu1 * Math.Sin(A1);
                double cotsigma1 = cosu1 * Math.Cos(A1) / sinu1;
                double sigma1 = Math.Atan(1 / cotsigma1);
                //计算系数A，B，C及
                double cosA0 = Math.Sqrt(1 - sinA0 * sinA0);
                double k2 = e2 * cosA0 * cosA0;

                double A = (1 - k2 / 4 + 7 * k2 * k2 / 64 - 15 * Math.Pow(k2, 3) / 256) / b;
                double B = (k2 / 4 - k2 * k2 / 8 + 37 * Math.Pow(k2, 3) / 512);
                double C = (k2 * k2 / 128 - Math.Pow(k2, 3) / 128);

                double alpha = (e1 / 2 + e1 * e1 / 8 + e1 * e1 * e1 / 16) - (e1 * e1 / 16 +
                       Math.Pow(e1, 3) / 16) * cosA0 * cosA0 + (e1 * Math.Pow(e1, 3) / 128) *
                       Math.Pow(cosA0, 4);
                double beta = (e1 * e1 / 16 + Math.Pow(e1, 3) / 16) * cosA0 * cosA0 -
                    (Math.Pow(e1, 3) / 32) * Math.Pow(cosA0, 4);
                double gamma = Math.Pow(e1, 3) / 256 * Math.Pow(cosA0, 4);
                //计算球面长度
                double AS = A * S;
                double sigma = AS;
                double temp = 0;
                do
                {
                    temp = sigma;
                    sigma = AS + B * Math.Sin(sigma) * Math.Cos(2 * sigma1 + sigma) + C * Math.Sin(2 * sigma) * Math.Cos(4 * sigma1 + 2 * sigma);
                }
                while (Math.Abs(temp - sigma) > 0.0000000001);

                //计算经度差改正数
                double delta = 0;
                double lambda = 0;
                double L = 0;
                delta = (alpha * sigma + beta * Math.Sin(sigma) * Math.Cos(2 * sigma1 + sigma)
                    + gamma * Math.Sin(2 * sigma) * Math.Cos(4 * sigma1 + 2 * sigma)) * sinA0;
                //计算终点大地坐标及坐标方位角
                double sinu2 = sinu1 * Math.Cos(sigma) + cosu1 * Math.Cos(A1) * Math.Sin(sigma);
                double B2 = Math.Atan(sinu2 / (Math.Sqrt(1 - e1) * Math.Sqrt(1 - sinu2 * sinu2)));
                lambda = Math.Atan(Math.Sin(A1) * Math.Sin(sigma) / (cosu1 * Math.Cos(sigma) -
                    sinu1 * Math.Sin(sigma) * Math.Cos(A1)));
                if (Math.Sin(A1) > 0)
                {
                    if (Math.Tan(lambda) > 0)
                    { lambda = Math.Abs(lambda); }
                    else if (Math.Tan(lambda) < 0)
                    { lambda = Math.PI - Math.Abs(lambda); }
                }
                else if (Math.Sin(A1) < 0)
                {
                    if (Math.Tan(lambda) < 0)
                    { lambda = -Math.Abs(lambda); }
                    else if (Math.Tan(lambda) > 0)
                    { lambda = Math.Abs(lambda) - Math.PI; }
                }
                double L2 = L1 + lambda - delta;
                double A2 = Math.Atan(cosu1 * Math.Sin(A1) / (cosu1 * Math.Cos(sigma) *
                    Math.Cos(A1) - sinu1 * Math.Sin(sigma)));
                if (Math.Sin(A1) < 0)
                {
                    if (Math.Tan(A2) > 0)
                    { A2 = Math.Abs(A2); }
                    else if (Math.Tan(A2) < 0)
                    { A2 = Math.PI - Math.Abs(A2); }
                }
                else if (Math.Sin(A1) > 0)
                {
                    if (Math.Tan(A2) > 0)
                    { A2 = Math.PI + Math.Abs(A2); }
                    else if (Math.Tan(A2) < 0)
                    { A2 = Math.PI * 2 - Math.Abs(A2); }
                }
                if (A2 < 0)
                    A2 = A2 + Math.PI * 2;
                else if (A2 > Math.PI * 2)
                    A2 = A2 - Math.PI * 2;

                A2 = Algo.Rad2Angle(A2 - Math.PI);
                B2 = Algo.Rad2Angle(B2);
                L2 = Algo.Rad2Angle(L2);
                R.A2.Add(Algo.Angle2ShowAgle(A2));
                R.B2.Add(Algo.Angle2ShowAgle(B2));
                R.L2.Add(Algo.Angle2ShowAgle(L2));
            }
        }
    }
}
