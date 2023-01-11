using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelLib
{
    /// <summary>
    /// 水准测量的测站记录数据类型
    /// </summary>
    public class Station
    {
        public string Sn;              //测站编号
        public string ForePoint;       //前视点名
        public string BackPoint;       //后视点名
        public double BackDist1;       //后距1
        public double BackDist2;       //后距2
        public double ForeDist1;       //前距1
        public double ForeDist2;       //前距2
        public double DistDiff1;       //距离差1
        public double DistDiff2;       //距离差2
        public double DistDiff;         //距离差
        public double DistDiffSum;     //距离差之和
        public double BackSight1;      //后视中丝1
        public double BackSight2;      //后视中丝2
        public double BackSightDiff;   //后视中丝差
        public double ForeSight1;      //前视中丝1
        public double ForeSight2;      //前视中丝2
        public double ForesightDiff;   //前视中丝差
        public double HeightDiff1;     //高差1
        public double HeightDiff2;     //高差2
        public double SightDiff;       //中丝差
        public double HeightDiff;       //高差

        public bool Flag;               //如果测站有超限，为Flase，全部通过检查，则为True
       public Station()
        {
            Flag = true;
        }

        /// <summary>
        /// 测站计算
        /// </summary>
        public void StationComputation()
        {
            //(9)
            ForesightDiff = ForeSight1 - ForeSight2;
            //(10)
            BackSightDiff = BackSight1 - BackSight2;
            //(11)
            SightDiff = BackSightDiff - ForesightDiff;
            //(12)
            DistDiff1 = BackDist1 - ForeDist1;
            //(13)
            DistDiff2 = BackDist2 - ForeDist2;
            //(14)
            DistDiff = (DistDiff1 + DistDiff2) / 2.0;

            //(16)
            HeightDiff1 = BackSight1 - ForeSight1;
            //(17)
            HeightDiff2 = BackSight2 - ForeSight2;
            //(18)
            HeightDiff = (HeightDiff1 + HeightDiff2) / 2.0;

            Flag = DataCheck4Order();
        }
        /// <summary>
        /// 测站计算
        /// </summary>
        /// <param name="previousDistDiffSum">前一站的距离差之和</param>
        public void StationComputation(double previousDistDiffSum)
        {
            DistDiffSum = DistDiff + previousDistDiffSum;
        }
        /// <summary>
        /// 四等水准测量限差
        /// </summary>
        public bool DataCheck4Order()
        {
           
            double distDiffLimit = 5.0;     //后前视距差限差
            double distDiffSumLimit = 10.0; //后前视距差累计限差
            double sightDiffLimit = 3.0 * 0.001;//黑红面读数差
            double sightDiffDiffLimit = 5.0 * 0.001; //黑红面所测高差之差
            if (Math.Abs(DistDiff) >= distDiffLimit)
            {
                return false;
            }
            if (Math.Abs(DistDiffSum) > distDiffSumLimit)
            {
                return false;
            }
            if (Math.Abs(BackSightDiff) > sightDiffLimit)
            {
                return false;
            }
            if (Math.Abs(ForesightDiff) > sightDiffLimit)
            {
                return false;
            }
            if (Math.Abs(SightDiff) > sightDiffDiffLimit)
            {
                return false;
            }

            return true;

        }
        public bool DataCheck3Order()
        {

            double distDiffLimit = 3.0;     //后前视距差限差
            double distDiffSumLimit = 60.0; //后前视距差累计限差
            double sightDiffLimit = 2.0 * 0.001;//黑红面读数差
            double sightDiffDiffLimit = 3.0 * 0.001; //黑红面所测高差之差
            if (Math.Abs(DistDiff) >= distDiffLimit)
                return false;
            if (Math.Abs(DistDiffSum) >= distDiffSumLimit)
                return false;
            if (Math.Abs(BackSightDiff) >= sightDiffLimit)
                return false;
            if (Math.Abs(ForesightDiff) >= sightDiffLimit)
                return false;
            if (Math.Abs(SightDiff) >= sightDiffDiffLimit)
                return false;

            return true;
        }

        /// <summary>
        /// 将测站测量信息输出
        /// </summary>
        /// <returns>格式化的输出字符串</returns>
        public override string ToString()
        {
            string sign = "√";
            if (!Flag)
                sign = "×";
            string res = string.Format("{0,-5}{1,-7}{2,10:f3}{3,10:f3}          ",
                Sn,BackPoint,BackDist1,BackDist2);
            res += string.Format("{0,10:f3}{1,10:f3}{2,10:f3} {3,5}\n",
                BackSight1, BackSight2, BackSightDiff,sign);

            res += string.Format("     {0,-7}{1,10:f3}{2,10:f3}{3,10:f3}",
                ForePoint, ForeDist1, ForeDist2, DistDiff);
            res += string.Format("{0,10:f3}{1,10:f3}{2,10:f3}\n",
                ForeSight1, ForeSight2, ForesightDiff);

            res += string.Format("     后-前 {0,10:f3}{1,10:f3}{2,10:f3}",
                DistDiff1, DistDiff2, DistDiffSum);
            res += string.Format("{0,10:f3}{1,10:f3}{2,10:f3}{3,10:f3}\n",
                HeightDiff1, HeightDiff2, SightDiff, HeightDiff);
            return res;
        }
    }
}
