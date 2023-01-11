using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace LevelLib
{
    /// <summary>
    /// 观测值
    /// </summary>
    public class ObsData
    {
        public PointInfo StartPoint, EndPoint;
        public List<Station> Data;

        public ObsData()
        {
            Data = new List<Station>();
        }
        /// <summary>
        /// 增加一组记录
        /// </summary>
        /// <param name="record">一次测量记录</param>
        public void Add(Station record)
        {
            Data.Add(record);
        }
        /// <summary>
        /// 转化为DataTable的格式
        /// </summary>
        /// <returns>包含有观测数据和测站计算后的数据表</returns>
        public DataTable ToTable()
        {
            DataTable table = InitTable();
            for (int i = 0; i < Data.Count; i++)
            {
                DataRow row = table.NewRow();
                row["测站编号"] = Data[i].Sn;
                row["后视点名"] = Data[i].BackPoint;
                row["前视点名"] = Data[i].ForePoint;
                row["后距1"] = string.Format("{0:f3}", Data[i].BackDist1);
                row["后距2"] = string.Format("{0:f3}", Data[i].BackDist2);
                row["前距1"] = string.Format("{0:f3}", Data[i].ForeDist1);
                row["前距2"] = string.Format("{0:f3}", Data[i].ForeDist2);
                row["距离差1"] = string.Format("{0:f3}", Data[i].DistDiff1);
                row["距离差2"] = string.Format("{0:f3}", Data[i].DistDiff2);
                row["距离差d"] = string.Format("{0:f3}", Data[i].DistDiff);
                row["Σd"] = string.Format("{0:f3}", Data[i].DistDiffSum);

                row["后视中丝1"] = string.Format("{0:f3}", Data[i].BackSight1);
                row["后视中丝2"] = string.Format("{0:f3}", Data[i].BackSight2);
                row["前视中丝1"] = string.Format("{0:f3}", Data[i].ForeSight1);
                row["前视中丝2"] = string.Format("{0:f3}", Data[i].ForeSight2);
                row["后视中丝差"] = string.Format("{0:f3}", Data[i].BackSightDiff);
                row["前视中丝差"] = string.Format("{0:f3}", Data[i].ForesightDiff);
                row["高差1"] = string.Format("{0:f3}", Data[i].HeightDiff1);
                row["高差2"] = string.Format("{0:f3}", Data[i].HeightDiff2);
                row["中丝差"] = string.Format("{0:f3}", Data[i].SightDiff);
                row["高差"] = string.Format("{0:f3}", Data[i].HeightDiff);

                table.Rows.Add(row);
            }
            return table;
        }

        DataTable InitTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("测站编号", typeof(string));
            table.Columns.Add("后视点名", typeof(string));
            table.Columns.Add("前视点名", typeof(string));
            table.Columns.Add("后距1", typeof(double));
            table.Columns.Add("后距2", typeof(double));
            table.Columns.Add("前距1", typeof(double));
            table.Columns.Add("前距2", typeof(double));
            table.Columns.Add("距离差1", typeof(double));
            table.Columns.Add("距离差2", typeof(double));
            table.Columns.Add("距离差d", typeof(double));
            table.Columns.Add("Σd", typeof(double));

            table.Columns.Add("后视中丝1", typeof(double));
            table.Columns.Add("后视中丝2", typeof(double));
            table.Columns.Add("前视中丝1", typeof(double));
            table.Columns.Add("前视中丝2", typeof(double));
            table.Columns.Add("后视中丝差", typeof(double));
            table.Columns.Add("前视中丝差", typeof(double));
            table.Columns.Add("高差1", typeof(double));
            table.Columns.Add("高差2", typeof(double));
            table.Columns.Add("中丝差", typeof(double));
            table.Columns.Add("高差", typeof(double));
            return table;
        }

        public override string ToString()
        {
            string res = "\n已知点信息\n";
            res += "----------------------------------------------------------\n";
            res += StartPoint.ToString() + "\n" + EndPoint.ToString()+"\n";

            res += "\n测量数据及测站计算\n";
            res += "----------------------------------------------------------\n";
            //res += "测站 后视点名    后 距1   后 距2            后视中丝1   后视中丝2   后视中丝差\n";
            //res += "编号 前视点名    前 距1   前 距2   距离差d   前视中丝1   前视中丝2   前视中丝差\n";
            //res += "     后-前      距离差1  距离差2	 ∑d       高 差1      高 差2     中丝差        高差\n";
            res += "测站 后视点名  后 距1     后 距2              后视中丝1  后视中丝2 后视中丝差\n";
            res += "编号 前视点名  前 距1     前 距2     距离差d   前视中丝1  前视中丝2 前视中丝差\n";
            res += "     后-前   距离差1    距离差2    ∑d       高 差1       高 差2      中丝差    高差\n";
            for (int i = 0; i < Data.Count; i++)
            {
                res += Data[i].ToString();
            }
            return res;
        }
    }
}
