using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelLib
{
    /// <summary>
    /// 近似平差计算
    /// </summary>
    public class Adjustment
    {
        public DataEnitity Data;
        public double HeightClossure;
        public double DistanceSum;
        public double Mu;
        public Adjustment(DataEnitity enitities)
        {
            Data = enitities;
            HeightClossure = GetHeightClosure();
            DistanceSum = GetDistanceSum();
        }
        /// <summary>
        /// 水准路线的高程闭合差
        /// </summary>
        /// <returns>高程闭合差</returns>
        double GetHeightClosure()
        {
            double sum = Data.Points[0].Height;
            for (int i = 0; i < Data.Data.Count; i++)
            {
                sum += Data.Data[i].HeightDiff;

            }
            sum -= Data.Points[Data.Points.Count - 1].Height;
            return sum;
        }
        /// <summary>
        /// 计算距离之和
        /// </summary>
        /// <returns>距离之和</returns>
        double GetDistanceSum()
        {
            return Data.Points[Data.Points.Count - 1].Distance;
        }
        /// <summary>
        /// 计算改正数
        /// </summary>
        void Corrections()
        {



            for (int i = 0; i < Data.Data.Count; i++)
            {
                double Li = Data.Data[i].Distance;
                double v = -HeightClossure / DistanceSum * Li;
                Data.Data[i].Residuals = v;
            }
        }
        /// <summary>
        /// 计算各测站高程
        /// </summary>
        public void HeightAdjustment()
        {
            Corrections();

            Mu = Precision();
            StationPrecision();

            double H = Data.Points[0].Height;

            for (int i = 0; i < Data.Data.Count; i++)
            {
                double h = Data.Data[i].HeightDiff + Data.Data[i].Residuals;
                H += h;
                Data.Points[i + 1].Height = H;

                Data.Points[i].Presion = Data.Data[i].Precison;
            }



        }
        /// <summary>
        /// 精度评估
        /// </summary>
        /// <returns>单位权中误差</returns>
        public double Precision()
        {
            double totalLenth = GetDistanceSum();
            double C = 1000.0; //1.0km     

            double Pi = C / Data.Data[0].Distance;
            double vi = Data.Data[0].Residuals;
            double sum = Pi * vi * vi;
            for (int i = 1; i < Data.Data.Count; i++)
            {
                Pi = C / (Data.Data[i].Distance);// - Data.Data[i - 1].Distance);
                vi = Data.Data[i].Residuals;
                sum += Pi * vi * vi;
            }

            int n = Data.Points.Count - 1;
            int t = Data.Points.Count - 2;
            Mu = Math.Sqrt(sum / (n - t));
            return Mu;
        }

        /// <summary>
        /// 精度评估
        /// </summary>
        /// <returns>单位权中误差</returns>
        public void StationPrecision()
        {
            double totalLenth = GetDistanceSum();
            double C = 1000; //1.0km           
                             // double sum = 0;
            for (int i = 1; i < Data.Points.Count - 1; i++)
            {
                double Pi = C / Data.Points[i].Distance + C / (totalLenth - Data.Points[i].Distance);
                Data.Data[i].Precison = Mu / Math.Sqrt(Pi);
            }
        }

        string CorrectionInfo()
        {
            string str = "\n闭合差信息\n";
            str += "----------------------------------------------------------\n";
            str += string.Format("闭合差: {0,10:f4} \n", HeightClossure);
            str += string.Format("总距离: {0,10:f4} \n", DistanceSum);
            return str;
        }
        public override string ToString()
        {
            string res = CorrectionInfo();
            res += Data.ToString();
            res += "\n精度评估\n";
            res += "----------------------------------------------------------\n";
            res += string.Format("Mu={0:f4}", Precision());
            return res;
        }

        public string ToString1()
        {
            string res = CorrectionInfo();
            res += Data.ToString();
            return res;
        }

    }
}
