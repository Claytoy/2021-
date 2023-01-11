using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelLib
{
    public class DataEnitity
    {
        public List<Segment> Data;
        public List<PointInfo> Points;
        /// <summary>
        /// 将观测值数据进行分片处理
        /// </summary>
        /// <param name="obsData"></param>
        public DataEnitity(ObsData obsData)
        {
            Data = Stations2Segments(obsData.Data);
            Points = Segments2Point(Data, obsData.StartPoint, obsData.EndPoint);
        }


        /// <summary>
        /// 由测段数据生成点列表
        /// </summary>
        /// <param name="data">测段数据</param>
        /// <returns></returns>
        private List<PointInfo> Segments2Point(List<Segment> data, PointInfo startPoint, PointInfo endPoint)
        {
            List<PointInfo> ptList = new List<PointInfo>();
            //开始站
            startPoint.Distance = 0;
            ptList.Add(startPoint);
            //中间测段
            double dist = 0;
            for (int i = 0; i < data.Count-1; i++)
            {
                PointInfo pt = data[i].EndPoint;
                dist += data[i].Distance;
                pt.Distance = dist;           
                pt.PointType = 2;
                ptList.Add(pt);
            }
            //最后测段
            dist += data[data.Count - 1].Distance;
            endPoint.Distance = dist;
            ptList.Add(endPoint);
            return ptList;
        }

        /// <summary>
        /// 将测站数据列表转化为测段列表
        /// </summary>
        /// <param name="stationDataList">测站数据列表</param>
        /// <returns></returns>
        List<Segment> Stations2Segments(List<Station> stationDataList)
        {
            List<Segment> data = new List<Segment>();
            Segment segment = new Segment();
            PointInfo pt1 = new PointInfo();
            PointInfo pt2 = new PointInfo();
            double dist = 0, htDiff = 0;
            int stationSum = 0;
            foreach (var d in stationDataList)
            {
                if (d.BackPoint != "-1" && d.ForePoint != "-1") //后视点和前视点都不是转点
                {
                    pt1 = new PointInfo(d.BackPoint);
                    pt2 = new PointInfo(d.ForePoint);
                    dist = (d.BackDist1 + d.BackDist2 + d.ForeDist1 + d.ForeDist2) / 2.0;
                    htDiff = d.HeightDiff;
                    stationSum = 1;

                    segment = new Segment(pt1, pt2, dist, htDiff, stationSum);
                    data.Add(segment);
                }
                else if (d.BackPoint != "-1" && d.ForePoint == "-1") //后视点不是转点，前视点是转点
                {
                    pt1 = new PointInfo(d.BackPoint);
                    dist = (d.BackDist1 + d.BackDist2 + d.ForeDist1 + d.ForeDist2) / 2.0;
                    htDiff = d.HeightDiff;
                    stationSum = 1;
                }
                else if (d.BackPoint == "-1" && d.ForePoint == "-1") //后视点和前视点都是转点
                {
                    dist += (d.BackDist1 + d.BackDist2 + d.ForeDist1 + d.ForeDist2) / 2.0;
                    htDiff += d.HeightDiff;
                    stationSum += 1;
                }
                else if (d.BackPoint == "-1" && d.ForePoint != "-1") //后视点是转点,前视点不是转点
                {
                    pt2 = new PointInfo(d.ForePoint);
                    dist += (d.BackDist1 + d.BackDist2 + d.ForeDist1 + d.ForeDist2) / 2.0;
                    htDiff += d.HeightDiff;
                    stationSum += 1;

                    segment = new Segment(pt1, pt2, dist, htDiff, stationSum);
                    data.Add(segment);
                }
            }
            return data;
        }

        string SegmentInfo()
        {
            string str = "\n高程配赋表\n";
            str += "----------------------------------------------------------\n";
            str += "起点  终点            距离          高差         改正数 \n";
            foreach (var d in Data)
            {
                str += string.Format("{0,-7}{1,-7}  {2,10:f3}  {3,10:f3}  {4,10:f3}\n",
                    d.StartPoint.Name, d.EndPoint.Name, d.Distance, d.HeightDiff, d.Residuals);
            }
            return str;
        }
        string HeightInfo()
        {
            string str = "\n计算结果信息表\n";
            str += "----------------------------------------------------------\n";
            str += "点名    距离    高程   精度\n";
            foreach (var p in Points)
            {
                str += p.ToString()+"\n";
            }
            return str;
        }

        public override string ToString()
        {
            return SegmentInfo()+HeightInfo();
        }
    }

    public class PointInfo
    {
        public string Name;     //点名
        public double Height;   //高程
        public double Distance; //距离第一个点的距离
        public int PointType;    //1-已知高程点，2-待测高程点，3-转点 
        public double Presion; //点位精度
        public PointInfo()
        {
            Name = "";
            Height = 0.0;
            PointType = 0;
            Presion = 0.0;
        }

        public PointInfo(string name)
        {
            Name = name;
            if (name == "-1")
            {
                PointType = 3;
            }
            else
            {
                PointType = 2;
            }
        }
        public PointInfo(string name, double height)
        {
            Name = name;
            Height = height;
            if (name == "-1")
            {
                PointType = 3;
            }
            else
            {
                PointType = 1;
            }
        }

        public override string ToString()
        {
            string line = string.Format("{0,-7}{1,10:f3}{2,10:f3}{3,10:f3}", Name, Distance, Height, Presion);
            return line;
        }

    }

    /// <summary>
    /// 一个测段，即从一个已知点（或未知点）到相邻另一个已知点（或未知点）
    ///  其中可以包含若干个转点
    /// </summary>
    public class Segment
    {
        public PointInfo StartPoint; //开始点
        public PointInfo EndPoint;   //结束点
        public double Distance;      //距离
        public double HeightDiff;    //高差
        public int StationSum;       //测站数
        public double Residuals;     //改正数
        public double Precison;      //精度
        public Segment()
        {
            StartPoint = new PointInfo();
            EndPoint = new PointInfo();
        }

        public Segment(PointInfo startPoint, PointInfo endPoint, double distance,
            double heightDiff, int stationSum)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            Distance = distance;
            HeightDiff = heightDiff;
            StationSum = stationSum;
        }
    }

}
