using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace LevelLib
{
    /// <summary>
    /// 读写文件
    /// </summary>
    public class FileHandler
    {
        /// <summary>
        /// 读取观测值文件，数据存储在Obs之中
        /// </summary>
        /// <param name="filePath">观测值文件</param>
        public static ObsData Read(string filePath)
        {
            ObsData obs = new ObsData();
            StreamReader reader = new StreamReader(filePath);

            //读取已知高程点信息
            string line = reader.ReadLine();
            string name = line.Split(',')[0];
            double height = double.Parse(line.Split(',')[1]);
            PointInfo startPoint = new PointInfo(name, height);
            obs.StartPoint = startPoint;

            line = reader.ReadLine();
            name = line.Split(',')[0];
            height = double.Parse(line.Split(',')[1]);
            PointInfo endPoint = new PointInfo(name, height);
            obs.EndPoint = endPoint;

            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (line.Length > 1)
                {
                    Station reacord = ReadStationData(line);
                    obs.Data.Add(reacord);
                }
            }
            reader.Close();
           // PostRead(ref obs);
            return obs;
        }
        /// <summary>
        /// 对读入的数据进行后处理
        /// </summary>
        /// <param name="obs">观测值</param>
        public static void PostRead(ref ObsData obs)
        {
            double distDiffSum = 0;
            for (int i = 0; i < obs.Data.Count; i++)
            {
                obs.Data[i].Sn = string.Format("{0}", i + 1);
                if(obs.Data[i].BackPoint!="-1")
                {
                    distDiffSum = 0;
                }
                distDiffSum += obs.Data[i].DistDiff;
                obs.Data[i].DistDiffSum = distDiffSum;
            }
        }

        /// <summary>
        /// 从一行文本中读取一组数据
        /// </summary>
        /// <param name="line">数据行</param>
        /// <returns>测站观测值</returns>
        private static Station ReadStationData(string line)
        {

            Station record = new Station();
            string[] info = line.Split(',');
            //点名
            record.BackPoint = info[0];
            record.ForePoint = info[1];
            //后
            record.BackDist1 = double.Parse(info[2]);
            record.BackSight1 = double.Parse(info[3]);

            //前
            record.ForeDist1 = double.Parse(info[4]);
            record.ForeSight1 = double.Parse(info[5]);

            //前
            record.ForeDist2 = double.Parse(info[6]);
            record.ForeSight2 = double.Parse(info[7]);

            //后
            record.BackDist2 = double.Parse(info[8]);
            record.BackSight2 = double.Parse(info[9]);


            record.StationComputation();
            return record;
        }

        /// <summary>
        /// 将报告保存到文件之中
        /// </summary>
        /// <param name="filePath">要保存的文件名称</param>
        /// <param name="report">数据处理报告</param>
        public void Write(string filePath, Report report)
        {


        }

        public static void SaveTable(string path, DataTable LevelTable)
        {
            try
            {
                Excel.Application app = new Excel.Application();
                Excel.Workbook book = app.Workbooks.Add(Missing.Value);
                Excel.Worksheet sheet = (Excel.Worksheet)book.ActiveSheet;
                app.Visible = false;
                for (int j = 0; j < LevelTable.Columns.Count; j++)
                {
                    sheet.Cells[1, j + 1] = LevelTable.Columns[j].ColumnName;
                }

                for (int i = 0; i < LevelTable.Rows.Count; i++)
                {
                    for (int j = 0; j < LevelTable.Columns.Count; j++)
                    {
                        sheet.Cells[i + 2, j + 1] = LevelTable.Rows[i][j];
                    }
                }

                book.SaveAs(path, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlShared, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                app.Workbooks.Close();
                app.Quit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SaveDxf(string path, DataEnitity LevelPro)
        {
            try
            {
                int fonth = 1;
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine("0");
                    sw.WriteLine("SECTION");
                    sw.WriteLine("2");
                    sw.WriteLine("ENTITIES");

                    for (int i = 0; i < LevelPro.Points.Count; ++i)
                    {
                        sw.WriteLine("0");
                        sw.WriteLine("POINT");
                        sw.WriteLine("8");
                        sw.WriteLine("点层");
                        sw.WriteLine("10");
                        sw.WriteLine(LevelPro.Points[i].Distance / 100);
                        sw.WriteLine("20");
                        sw.WriteLine(LevelPro.Points[i].Height);

                        sw.WriteLine("0");
                        sw.WriteLine("TEXT");
                        sw.WriteLine("8");
                        sw.WriteLine("注记");
                        sw.WriteLine("10");
                        sw.WriteLine(LevelPro.Points[i].Distance / 100);
                        sw.WriteLine("20");
                        sw.WriteLine(LevelPro.Points[i].Height);
                        sw.WriteLine("40");
                        sw.WriteLine(fonth);
                        sw.WriteLine("1");
                        sw.WriteLine(LevelPro.Points[i].Name);
                    }
                    for (int i = 0; i < LevelPro.Points.Count - 1; ++i)
                    {
                        sw.WriteLine("0");
                        sw.WriteLine("LINE");
                        sw.WriteLine("8");
                        sw.WriteLine("线层");
                        sw.WriteLine("10");
                        sw.WriteLine(LevelPro.Points[i].Distance / 100);
                        sw.WriteLine("20");
                        sw.WriteLine(LevelPro.Points[i].Height);
                        sw.WriteLine("11");
                        sw.WriteLine(LevelPro.Points[i + 1].Distance / 100);
                        sw.WriteLine("21");
                        sw.WriteLine(LevelPro.Points[i + 1].Height);
                    }
                    sw.WriteLine("0");
                    sw.WriteLine("ENDSEC");
                    sw.WriteLine("0");
                    sw.WriteLine("EOF");
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
