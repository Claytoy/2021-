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
        public List<Point> Points = new List<Point>();
        public Result R = new Result();


        public void GetData(string filepath)
        {
            string line;
            string[] strs;
            Point tp1 = new Point();
            Point tp2 = new Point();
            StreamReader sr = new StreamReader(filepath);
            line = sr.ReadLine();
            strs = line.Split(',');
            tp1.Start = strs[0];
            tp1.Height = double.Parse(strs[1]);
            line = sr.ReadLine();
            strs = line.Split(',');
            tp2.Start = strs[0];
            tp2.Height = double.Parse(strs[1]);

            sr.ReadLine();
            while ((line = sr.ReadLine()) != null)
            {
                Point p = new Point();
                strs = line.Split(',');
                p.Start = strs[0];
                p.End = strs[1];
                p.Hsjl1 = double.Parse(strs[2]);
                p.Hszsds1 = double.Parse(strs[3]);
                p.Qsjl1 = double.Parse(strs[4]);
                p.Qszsds1 = double.Parse(strs[5]);
                p.Qsjl2 = double.Parse(strs[6]);
                p.Qszsds2 = double.Parse(strs[7]);
                p.Hsjl2 = double.Parse(strs[8]);
                p.Hszsds2 = double.Parse(strs[9]);
                Points.Add(p);
            }

            for (int i = 0; i < Points.Count(); i++)
            {
                if (tp1.Start == Points[i].Start)
                {
                    Points[i].Height = tp1.Height;
                }
                else if (tp2.Start == Points[i].End)
                {
                    Points[i].Height = tp2.Height;
                }
                else
                {
                    Points[i].Height = 0;
                }
            }
        }



        public void Leveling()
        {
            double a9 = 0, a10 = 0, a11 = 0, a12 = 0, a13 = 0, a14 = 0, a15 = 0, a16 = 0, a17 = 0, a18 = 0;
            for (int i = 0; i < Points.Count(); i++)
            {
                Point p = Points[i];

                double a1 = p.Hsjl1;
                double a2 = p.Hszsds1;
                double a3 = p.Qsjl1;
                double a4 = p.Qszsds1;
                double a5 = p.Qsjl2;
                double a6 = p.Qszsds2;
                double a7 = p.Hsjl2;
                double a8 = p.Hszsds2;

                a9 = a4 - a6;
                a10 = a2 - a8;
                a11 = a10 - a9;

                a12 = a1 - a3;
                a13 = a7 - a5;
                a14 = (a12 + a13) / 2;
                a15 = a14 + a15;

                a16 = a2 - a4;
                a17 = a8 - a6;
                a18 = (a16 + a17) / 2;
                Points[i].D = a14;
                // Points[i].EpsilonD = a15;
                Points[i].Jlc1 = a12;
                Points[i].Jlc2 = a13;
                Points[i].Gc1 = a16;
                Points[i].Gc2 = a17;
                Points[i].Gc = a18;
                Points[i].Hszsc = a10;
                Points[i].Qszsc = a9;
                Points[i].Zsc = a11;
            }
            double tt = 0; double ss = 0; double gc = 0;
            for (int i = 0; i < Points.Count; i++)
            {
                if (Points[i].End == "-1")
                {
                    tt += Points[i].D;
                    gc += Points[i].Gc;
                    double s = (Points[i].Qsjl1 + Points[i].Qsjl2 + Points[i].Hsjl1 + Points[i].Hsjl2) / 2;
                    Points[i].S = s;
                    ss += s;
                }
                else if (Points[i].End != "-1")
                {
                    Points[i].EpsilonD = tt + Points[i - 1].D;
                    R.EDS.Add(Points[i].EpsilonD);
                    R.S.Add(ss + Points[i - 1].S);
                    R.GCS.Add(gc + Points[i].Gc);
                    tt = 0; ss = 0; gc = 0;
                }
            }
            //水准路线闭合高差计算
            double hh = 0;
            for (int i = 0; i < Points.Count; i++)
            {
                hh += Points[i].Gc;
            }
            double fh = Math.Abs(hh) - (Points[0].Height - Points[Points.Count - 1].Height);
            R.Fh = fh;




            //高差改正数计算
            double Li = 0; double LT = 0;
            for (int j = 0; j < R.S.Count; j++)
            {
                LT += R.S[j];
            }
            for (int i = 0; i < R.S.Count(); i++)
            {
                double vi = fh / LT * R.S[i];
                R.Vis.Add(vi);
                double hiba = R.GCS[i] + vi;
                R.hibas.Add(hiba);
                double Hi = 0;
                Hi = Points[0].Height;
                double aaa = 0;
                for (int j = 0; j < i; j++)
                {
                    aaa += R.hibas[j];
                    Hi = aaa + Points[0].Height;

                }
                R.HIS.Add(Hi);

            }

        }
    
    }
}
