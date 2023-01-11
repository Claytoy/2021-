using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace 窗体空白模板
{
    public class Calculate
    {
        public List<Point> Points = new List<Point>();
        public List<Point> Sorts = new List<Point>();
        public List<Point> S = new List<Point>();//TB
        List<Point> Centers = new List<Point>();
        double h0;//参考高程
        double h;
        double w;
        double num_w;
        double num_h;
        double num_inhull;
        public double XMax;
        public double XMin;
        public double YMax;
        public double YMin;
        public void GetData(string filepath)
        {
            string line;
            string[] strs;
            StreamReader sr = new StreamReader(filepath);
            line = sr.ReadLine();
            strs = line.Split(',');
            h0 = double.Parse(strs[1]);
            line = sr.ReadLine();
            while ((line = sr.ReadLine()) != null)
            {
                strs = line.Split(',');
                Point p = new Point();
                p.Id = strs[0];
                p.X = double.Parse(strs[1]);
                p.Y = double.Parse(strs[2]);
                p.H = double.Parse(strs[3]);
                Points.Add(p);

            }

        }

        public void TB()
        {
            //find basic Point
            Point P0 = new Point();
            double xmin = Points[0].X;
            double ymin = Points[0].Y;
            for (int i = 0; i < Points.Count; i++)
            {
                double x = Points[i].X;
                double y = Points[i].Y;
                if (y < ymin)
                {
                    ymin = y;
                    P0 = Points[i];
                }
                else if (ymin == y)
                {
                    if (x < P0.X)
                        P0 = Points[i];
                }

            }
            Points.Remove(P0);

            //order Points by size of angle
            for (int i = 0; i < Points.Count(); i++)
            {
                Point Pi = Points[i];
                double angle = 0;
                angle = Math.Atan(Math.Abs((Pi.Y - P0.Y)) / Math.Abs((Pi.X - P0.X)));//tan
                if ((Pi.X - P0.X) < 0)
                {
                    angle = Math.PI - angle;
                }
                Points[i].Angle = angle;
            }
            //Sorts = Points;C#中list相等是传地址与Cpp不同注意
            //list1.ForEach(i => list2.Add(i));// This will copy all the items from list 1 to list 2
            Points.ForEach(i => Sorts.Add(i));
            for (int i = 0; i < Sorts.Count; i++)
            {
                for (int j = 0; j < Sorts.Count() - i - 1; j++)
                {
                    if (Sorts[j].Angle > Sorts[j + 1].Angle)
                    {
                        Point temp = new Point();
                        temp = Sorts[j];
                        Sorts[j] = Sorts[j + 1];
                        Sorts[j + 1] = temp;
                    }

                }

            }

            //set S basic on TB

            S.Add(P0);
            S.Add(Sorts[0]);
            S.Add(Sorts[1]);

            for (int i = 3; i < Sorts.Count; i++)
            {
                Point Pi = new Point();//second of stack
                Point Pj = new Point();//Top of stack
                Point Pk = new Point();//not in stack

                Pi = S[S.Count - 2];
                Pj = S[S.Count - 1];
                Pk = Sorts[i];

                double x1 = Pi.X;
                double y1 = Pi.Y;
                double x2 = Pj.X;
                double y2 = Pj.Y;
                double x3 = Pk.X;
                double y3 = Pk.Y;

                double m = ((x1 - x2) * (y3 - y2)) - ((y1 - y2) * (x3 - x2));
                if (m > 0)
                {
                    S.Remove(Pj);
                    i--;
                }
                else if (m < 0)
                {
                    S.Add(Pk);
                }

            }

            S.Add(P0);
        }
        //规则格网的生成
        public void Rect(double L)
        {
            //建立外包矩形，并进行二维格网划分
                 double Xmax = S[0].X;
                 double Xmin = S[0].X;
                 double Ymax = S[0].Y;
                 double Ymin = S[0].Y;
            for (int i = 0; i < S.Count; i++)
            {
                double x = S[i].X;
                double y = S[i].Y;
                if (x >= Xmax)
                    Xmax = x;
                else if (x <= Xmin)
                    Xmin = x;
                if (y >= Ymax)
                    Ymax = y;
                else if (y <= Ymin)
                    Ymin = y;

                Point P = new Point();
                P.X = Xmin;
                P.Y = Ymin;
                h = Ymax - Ymin;
                w = Xmax - Xmin;
                num_h = Math.Ceiling(h / L);
                num_w = Math.Ceiling(w / L);
            }
            XMax = Xmax;
            XMin = Xmin;
            YMax = Ymax;
            YMin = Ymin;

            //判断格网中心点是否在凸包内
            for (int i = 0; i < num_h; i++)
            {
                double x;
                double y = YMin + i * L + L / 2;
                for (int j = 0; j < num_w; j++)
                {
                    int num_insides = 0;
                    x = XMin + j * L + L / 2;
                    for (int k = 0; k < S.Count - 1; k++)
                    {
                        if ((S[k].Y - y) * (S[k + 1].Y - y) < 0)
                        {
                            double xk;
                            xk = ((S[k + 1].X - S[k].X) * (y - S[k].Y)) / (S[k + 1].Y - S[k].Y);
                            xk += S[k].X;
                            if (xk > x)
                                num_insides++;
                        }
                    }
                    if (num_insides % 2 == 1)
                    {
                        Point P = new Point();
                        P.X = x;
                        P.Y = y;
                        Centers.Add(P);
                        num_inhull++;
                    }
                }

            }
            int aa = 0;
        }

        List<Point> Qi = new List<Point>();
        public double GetHeight(Point P)
        {
            double x = P.X;
            double y = P.Y;
            double r = (h + w) / 2 * 0.4;
            double up = 0;
            double down = 0;
            for (int i = 0; i < Points.Count; i++)
            {
                double xi = Points[i].X;
                double yi = Points[i].Y;
                double d = Math.Sqrt(Math.Pow((x - xi), 2) + Math.Pow((y - yi), 2));
                if (d < r)
                {
                    double hi = Points[i].H;
                    up += hi / d;
                    down += 1 / d;
                }


            }

            return up / down;
        }


        public void Volume(double L)
        {
            double V = 0; //Voluem of Grid
            for (int i = 0; i < Centers.Count; i++)
            {
                Point P1 = new Point();
                Point P2 = new Point();
                Point P3 = new Point();
                Point P4 = new Point();

                P1.X = Centers[i].X - L / 2;
                P1.Y = Centers[i].Y - L / 2;
                P2.X = Centers[i].X + L / 2;
                P2.Y = Centers[i].Y - L / 2;
                P3.X = Centers[i].X + L / 2;
                P3.Y = Centers[i].Y + L / 2;
                P4.X = Centers[i].X - L / 2;
                P4.Y = Centers[i].Y + L / 2;

                double h1 = GetHeight(P1);
                double h2 = GetHeight(P2);
                double h3 = GetHeight(P3);
                double h4 = GetHeight(P4);

                double Vi = ((h1 + h2 + h3 + h4) / 4 - h0) * L * L;
                V += Vi;
            }
        }

    }
}
