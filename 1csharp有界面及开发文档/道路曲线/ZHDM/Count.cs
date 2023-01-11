using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ZHDM
{
    class Count
    {
        //数据定义
        public List<Data> points = new List<Data>();
        public List<dian> Dian = new List<dian>();
        int k1 = 3;
        int k2 = 3;
        int k3 = 3;
        //导入数据
        public void dataIn(string filepath)
        {
            string line;
            string[] strs;
            StreamReader sr = new StreamReader(filepath);
            double a = 0;
            while ((line = sr.ReadLine()) != null)
            {
                a++;
            }
            sr = new StreamReader(filepath);
            double b = 0;
            while ((line = sr.ReadLine()) != null)
            {

                Data kp = new Data();
                if (b == 0 || b == a)
                {
                    strs = line.Split(',');
                    kp.Name = strs[0];
                    kp.X = double.Parse(strs[1]);
                    kp.Y = double.Parse(strs[2]);

                    points.Add(kp);
                }
                else
                {
                    strs = line.Split(',');
                    kp.Name = strs[0];
                    kp.X = double.Parse(strs[1]);
                    kp.Y = double.Parse(strs[2]);
                    kp.R = double.Parse(strs[3]);
                    kp.Ls = double.Parse(strs[4]);
                    points.Add(kp);
                }
                b++;

            }
        }
        double Kzy, Kqz1, Kyz;
        double Xyz, Yyz;
        double Xzy, Yzy;
        double alpha1, alpha2, alpha;
        //圆曲线计算
        public void crmath()
        {
            double x1, x2, x3, y1, y2, y3, R;
            x1 = points[0].X;
            x2 = points[1].X;
            x3 = points[2].X;
            y1 = points[0].Y;
            y2 = points[1].Y;
            y3 = points[2].Y;
            R = points[1].R;
            //线路转角alpha

            alpha1 = Math.PI / 2 - Math.Atan((y2 - y1) / (x2 - x1));
            alpha2 = Math.PI / 2 - Math.Atan((y3 - y2) / (x3 - x2));
            alpha = alpha2 - alpha1;
            if (alpha > (-Math.PI) && alpha < Math.PI)
            {

            }
            if (alpha > Math.PI && alpha < (2 * Math.PI))
            {
                alpha = alpha - (2 * Math.PI);
            }
            if (alpha > (-2 * Math.PI) && alpha < (-Math.PI))
            {
                alpha = alpha + (2 * Math.PI);
            }
            if (alpha > 0)
                k1 = 1;
            else if (alpha < 0)
                k1 = -1;

            if (alpha2 > 0)
                k2 = 1;
            else if (alpha2 < 0)
                k2 = -1;
            alpha = Math.Abs(alpha);//输出

            //圆曲线要素
            double T, L, E, q;//切线长，曲线长，外矢距，切曲差
            T = R * Math.Tan(alpha / 2);//输出
            L = R * alpha;//输出
            E = R * (1 / Math.Cos(alpha / 2) - 1);//输出
            q = 2 * T - L;//输出

            //圆曲线主点里程
            double Kjd = 0;
            Kjd = Math.Sqrt((Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)));//输出

            Kzy = Kjd - T;//输出
            Kqz1 = Kzy + L / 2;//输出
            Kyz = Kzy + L;//输出

            //主点ZY,YZ坐标
            //x用sin，y用cos
            Xzy = x2 - T * Math.Sin(alpha1);//输出
            Yzy = y2 - T * Math.Cos(alpha1);//输出
            Xyz = x2 + T * Math.Sin(alpha2);//输出
            Yyz = y2 + T * Math.Cos(alpha2);//输出

            //圆曲线中线点独立坐标
            double fai = 0;
            fai = L / 2 / R * 180 / Math.PI;
            double x = 0;
            double y = 0;
            x = R * Math.Sin(fai);//输出
            y = R * (1 - Math.Cos(fai));//输出
        }


        double Kzh, Khy, Kqz2, Kyh, Khz;
        double Kq = 0;
        double alpha2_, alpha_;
        double m, P, beta0;//缓和曲线参数
        double Xzh, Yzh, Xhz, Yhz;
        //带缓和曲线的圆曲线
        public void huanhequxian()
        {
            double x1, x2, x3, y1, y2, y3, R, Ls;
            x1 = Xyz;
            x2 = points[2].X;
            x3 = points[3].X;
            y1 = Yyz;
            y2 = points[2].Y;
            y3 = points[3].Y;
            R = points[2].R;
            Ls = points[2].Ls;
            //方位角计算


            alpha2_ = Math.PI / 2 - Math.Atan((y3 - y2) / (x3 - x2));
            alpha_ = alpha2_ - alpha2;
            if (alpha_ > (-Math.PI) && alpha_ < Math.PI)
            {

            }
            if (alpha_ > Math.PI && alpha_ < (2 * Math.PI))
            {
                alpha_ = alpha_ - (2 * Math.PI);
            }
            if (alpha_ > (-2 * Math.PI) && alpha_ < (-Math.PI))
            {
                alpha_ = alpha_ + (2 * Math.PI);
            }
            if (alpha_ > 0)
                k3 = 1;
            else if (alpha_ < 0)
                k3 = -1;
            alpha_ = Math.Abs(alpha_);//输出
           

            //计算缓和曲线参数

            m = Ls / 2 - Math.Pow(Ls, 3) / (240 * R * R);
            P = Ls * Ls / (24 * R);
            beta0 = Ls / (2 * R);

            //曲线综合要素计算
            double Th, Lh, Eh, q;//切线长，曲线长，外矢距，切曲差
            Th = m + (R + P) * Math.Tan(alpha_ / 2);
            Lh = R * (alpha_ - 2 * beta0) + 2 * Ls;
            Eh = (R + P) * (1 / Math.Cos(alpha_ / 2)) - R;
            q = 2 * Th - Lh;

            //圆曲线主点里程
            double Kjd = 0;
            Kjd =  Kyz + Math.Sqrt((x2-x1) * (x2-x1) + (y2-y1) *(y2-y1));//输出

            Kzh = Kjd - Th;
            Khy = Kzh + Ls;
            Kqz2 = Kzh + Lh / 2;
            Kyh = Kzh + Lh - Ls;
            Khz = Kyh + Ls;

            //曲线线路主点ZH，HZ坐标计算
            //x用sin，y用cos
            Xzh = x2 - Th * Math.Sin(alpha2);
            Yzh = y2 - Th * Math.Cos(alpha2);
            Xhz = x2 + Th * Math.Sin(alpha2_);
            Yhz = y2 + Th * Math.Cos(alpha2_);


            int a = 0;

            Kq = Math.Sqrt((Math.Pow(points[3].X - Xhz, 2) + Math.Pow(points[3].Y - Yhz, 2)));

        }

        public void lichengzhuang()
        {
            double a = 0;
            for (int i = 0; i < Kyz + Khz + Kq; i += 20)
            {
                dian kp1 = new dian();
                kp1.Name = "K-" + i;
                if (i == 0)//舍弃0
                {

                }
                if (i > 0 && i < Kzy)//第一条直线上的里程桩坐标
                {
                    double detax = 0;
                    double detay = 0;
                    detax = Math.Sin(alpha1) * i;
                    detay = Math.Cos(alpha1) * i;

                    kp1.X = points[0].X + detax;
                    kp1.Y = points[0].Y + detay;
                    Dian.Add(kp1);
                    a++;
                }



                if (i > Kzy && i < Kyz)//第一个弧左面点坐标
                {

                    double fai = 0;
                    double S = 0;
                    S = i - Kzy;
                    fai = S / points[1].R;
                    double x0 = 0;
                    double y0 = 0;
                    x0 = points[1].R * Math.Sin(fai);
                    y0 = points[1].R * (1 - Math.Cos(fai));
                    kp1.X = Xzy + x0 * Math.Sin(alpha1) + k1 * y0 * Math.Cos(alpha1);
                    kp1.Y = Yzy +  x0* Math.Cos(alpha1) - k1 * y0 * Math.Sin(alpha1);
                    Dian.Add(kp1);
                }

                //if (i > Kqz1 && i < Kyz)//第一个弧右半边点坐标
                //{
                //    double fai = 0;
                //    double S = 0;
                //    S = i - Kyz;
                //    fai = S / points[1].R;
                //    double x0 = 0;
                //    double y0 = 0;
                //    x0 = points[1].R * Math.Sin(fai);
                //    y0 = points[1].R * (1 - Math.Cos(fai));
                //    kp1.X = Xyz + x0 * Math.Cos(alpha2) + y0 * Math.Sin(alpha2);
                //    kp1.Y = Yyz + x0 * Math.Sin(alpha2) - y0 * Math.Cos(alpha2);
                //    Dian.Add(kp1);
                //}

                if (i > Kyz && i < Kzh)//第二条直线点坐标
                {
                    double detax = 0;
                    double detay = 0;
                    detax = Math.Sin(alpha2) * (i - Kyz);
                    detay = Math.Cos(alpha2) * (i - Kyz);

                    kp1.X = Xyz + detax;
                    kp1.Y = Yyz + detay;
                    Dian.Add(kp1);
                }

                if (i > Kzh  && i < Khy )//第二个弧左缓和曲线点坐标
                {
                    double S = 0;
                    double x1, y1;
                    S = i  - Kzh;
                    x1 = S - Math.Pow(S, 5) / (40 * Math.Pow(points[2].R, 2) * Math.Pow(points[2].Ls, 2));
                    y1 = Math.Pow(S, 3) / (6 * points[2].R * points[2].Ls);
                    kp1.X = Xzh + x1 * Math.Sin(alpha2) + k3 * y1 * Math.Cos(alpha2);
                    kp1.Y = Yzh + x1 * Math.Cos(alpha2) - k3 * y1 * Math.Sin(alpha2);
                    Dian.Add(kp1);
                }

                if (i > Khy && i <  Kyh)//第二个弧点坐标
                {
                    double S = 0;
                    S = i - Kzh;
                    double fai = 0;
                    fai = beta0 + (S - points[2].Ls) / points[2].R;
                    double x0 = 0;
                    double y0 = 0;
                    x0 = m + points[2].R * Math.Sin(fai);
                    y0 = P + points[2].R * (1 - Math.Cos(fai));
                    kp1.X = Xzh + x0 * Math.Sin(alpha2) + k3*y0 * Math.Cos(alpha2);
                    kp1.Y = Yzh + x0 * Math.Cos(alpha2) - k3*y0 * Math.Sin(alpha2);
                    Dian.Add(kp1);
                }

                //if(i > Kyz + Kqz2 && i < Kyz +Kyh)//第二个弧右半弧点坐标
                //{
                //    double S = 0;
                //    S = Kyz + Khz - i;
                //    double fai = 0;
                //    fai = beta0 + (S - points[2].Ls) / points[2].R ;
                //    double x0 = 0;
                //    double y0 = 0;
                //    x0 = m + points[2].R * Math.Sin(fai);
                //    y0 = P + points[2].R * (1 - Math.Cos(fai));
                //    kp1.X = Xhz + x0 * Math.Cos(alpha2_) + y0 * Math.Sin(alpha2_);
                //    kp1.Y = Yhz + x0 * Math.Sin(alpha2_) - y0 * Math.Cos(alpha2_);
                //    Dian.Add(kp1);
                //}

                if (i > Kyh && i <  Khz)//第二个弧右缓和曲线点坐标
                {
                    double S = 0;
                    double x1, y1;
                    S =  Khz - i;
                    x1 = S - Math.Pow(S, 5) / (40 * Math.Pow(points[2].R, 2) * Math.Pow(points[2].Ls, 2));
                    y1 = Math.Pow(S, 3) / (6 * points[2].R * points[2].Ls);
                    kp1.X = Xhz + x1 * Math.Sin(alpha2_) - k3 * y1 * Math.Cos(alpha2_);
                    kp1.Y = Yhz + x1 * Math.Cos(alpha2_) + k3 * y1 * Math.Sin(alpha2_);
                    Dian.Add(kp1);
                }

                if (i >  Khz && i <  Khz + Kq)//第三条直线上的点坐标
                {
                    double aa = 0;
                    aa = Math.PI/2 - Math.Atan((points[3].Y - Yhz) / (points[3].X - Xhz));
                    double detax = 0;
                    double detay = 0;
                    detax = (i  - Khz) * Math.Sin(aa);
                    detay = (i  - Khz) * Math.Cos(aa);
                    kp1.X = Xhz + detax;
                    kp1.Y = Yhz + detay;
                    Dian.Add(kp1);
                }
            }
        }
    }
}
