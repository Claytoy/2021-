using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace 窗体空白模板
{
    class PRO
    {
        Calculate Cal = new Calculate();
        public void dataIn(string filepath)//导入数据
        {
            string line;
            string[] strs;
            StreamReader sr = new StreamReader(filepath);
            Point kp = new Point();
            line = sr.ReadLine();
            strs = line.Split(',');
            kp.Name = strs[0];
            kp.Height = double.Parse(strs[1]);
            Cal.points.Add(kp);

            line = sr.ReadLine();

            while ((line = sr.ReadLine()) != null)
            {
                strs = line.Split('-');
                data kp1 = new data();
                kp1.Start = strs[0];
                kp1.End = strs[1];

                line = sr.ReadLine();
                strs = line.Split(',');
                kp1.G_station = double.Parse(strs[0]);
                kp1.G_aim = double.Parse(strs[1]);
                kp1.G_longth = double.Parse(strs[2]);
                kp1.G_angle = double.Parse(strs[3]);

                line = sr.ReadLine();
                strs = line.Split(',');
                kp1.C_station = double.Parse(strs[0]);
                kp1.C_aim = double.Parse(strs[1]);
                kp1.C_longth = double.Parse(strs[2]);
                kp1.C_angle = double.Parse(strs[3]);

                Cal.Data.Add(kp1);
            }
        }

        public void prepare()//编程实现记录簿记录手簿与数据预处理
        {
            //球气差改正数计算
            for (int i = 0; i < Cal.Data.Count; i++)
            {
                double R = 6378137;
                double k = 0.15;


                double G_alpha = 0;
                G_alpha = turn.angle2rad(Cal.Data[i].G_aim);
                double C_alpha = 0;
                C_alpha = turn.angle2rad(Cal.Data[i].C_aim);

                //往平距
                double G_D = 0;
                G_D = (Cal.Data[i].G_station + Cal.Data[i].C_station) / 2;

                //往地球曲率
                double G_p = 0;
                G_p = Math.Pow(G_D, 2) / (2 * R);

                //往大气折光
                double G_r = 0;
                G_r = (-k * Math.Pow(G_D, 2)) / (2 * R);

                //往球气差
                double G_f = 0;
                G_f = G_p + G_r;

                //返平距

                double C_D = 0;
                C_D = G_D;


                //返地球曲率
                double C_p = 0;
                C_p = Math.Pow(C_D, 2) / (2 * R);

                //返大气折光
                double C_r = 0;
                C_r = (-k * Math.Pow(C_D, 2)) / (2 * R);

                //返球气差
                double C_f = 0;
                C_f = C_p + C_r;

                Cal.Data[i].G_D = G_D;
                Cal.Data[i].G_f = G_f;
                Cal.Data[i].C_D = C_D;
                Cal.Data[i].C_f = C_f;

                //往测高差
                double G_h = 0;
                G_h = G_D * Math.Tan(G_alpha) + Cal.Data[i].G_longth - Cal.Data[i].G_angle + G_f;

                //返测高差
                double C_h = 0;
                C_h = C_D * Math.Tan(C_alpha) + Cal.Data[i].C_longth - Cal.Data[i].C_angle + C_f;

                Cal.Data[i].G_h = G_h;
                Cal.Data[i].C_h = C_h;

                //超限检查
                double a = 0;
                a = Math.Abs(G_h + C_h);
                double deta = 0;
                deta = 60 * G_D;
                if (a < G_D)
                {
                    Console.WriteLine("对向观测高差较差合格");
                }
                else if (a > G_D)
                {
                    Console.WriteLine("对向观测高差较差不合格");
                }

                //高程计算
                double h = 0;
                h = (G_h - C_h) / 2;
                Cal.Data[i].H = h;

                double H = 0;
                H = Cal.points[i].Height + h;

                Point kp = new Point();
                kp.Name = Cal.Data[i].End;
                kp.Height = H;
                Cal.points.Add(kp);
            }
        }

        public void adjustment()//近似平差计算
        {
            //计算高差闭合差
            double fh = 0;
            double hi = 0;
            double Ds = 0;
            for (int i = 0; i < Cal.Data.Count; i++)
            {
                hi = hi + Cal.Data[i].H;
                fh = hi - (Cal.points[i + 1].Height - Cal.points[i].Height);

                //高差改正数计算
                double delt = 0;
                Ds = Ds + Cal.Data[i].G_D;
                delt = -fh * Cal.Data[i].G_D / Ds;
                Cal.Data[i].V = delt;

                //计算改正后的高差
                double v_hi = 0;
                v_hi = hi + delt;

                //计算观测点高程
                double v_H = 0;
                v_H = Cal.points[i].Height + v_hi;

                Cal.points[i + 1].V_height = v_H;

            }



        }

        public void evaluate()
        {

        }

        //static void Main(string[] args)
        //{
        //    Program pro = new Program();
        //    pro.dataIn("D://2333//帮助文档与TXT//程序设计txt//三角高程.txt");
        //    pro.prepare();
        //    pro.adjustment();
        //}
    }
}

