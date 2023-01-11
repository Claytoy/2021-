using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace GeodesyLib
{
    public class DrawChart
    {
        /// <summary>
        /// 直接保存为图形文件
        /// </summary>
        public static void DrawImage(string imgFile, DataEntity data, int width, int height)
        {
            var chart1 = new Chart();
            chart1.Size = new System.Drawing.Size(width, height);
            DrawImage(data, ref chart1);
            chart1.SaveImage(imgFile, ChartImageFormat.Png);

        }
        /// <summary>
        /// 图形绘制
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="chart"></param>
        public static void DrawImage(DataEntity data, ref Chart chart)
        {
            InitChartArea(data, ref chart);
            chart.Series.Clear();
            chart.Legends.Clear();

            GetChartArea(data, ref chart);
            chart.DataBind();
        }

        /// <summary>
        /// 绘制道路基本情况图形
        /// </summary>
        /// <param name="c"></param>
        static void GetChartArea(DataEntity data, ref Chart c)
        {
            Series series = new Series();
            series.ChartType = SeriesChartType.Point;

            //绘制所有散点
            Series series2 = AllPoints(data.Points);
            series2.ChartArea = "A0";
            c.Series.Add(series2);

            foreach (var d in data.Data)
            {
                var s = Line(d.P1, d.P2);
                c.Series.Add(s);
            }

        }

        /// <summary>
        /// 绘制散点
        /// </summary>
        /// <param name="points"> </param>
        /// <returns></returns>
        static Series AllPoints(List<PointInfo> points)
        {
            Series series = new Series();
            series.ChartType = SeriesChartType.Point;
            DataPoint p;
            for (int i = 0; i < points.Count; i++)
            {
                p = new DataPoint();
                p.Label = points[i].Name;
                p.SetValueXY(points[i].L, points[i].B);
                series.Points.Add(p);
            }
            series.MarkerSize = 5;
            series.MarkerColor = Color.Red;
            series.MarkerStyle = MarkerStyle.Circle;
            return series;
        }

        static Series Line(PointInfo p1, PointInfo p2)
        {
            Series series = new Series();
            series.ChartType = SeriesChartType.Line;
            DataPoint p;

            series.Points.AddXY(p1.L, p1.B);
            series.Points.AddXY(p2.L, p2.B);

            //series.MarkerSize = 2;
            //series.MarkerColor = Color.Black;
            return series;

        }

        /// <summary>
        /// 初始化绘图区域
        /// </summary>
        /// <param name="c"></param>
        static void InitChartArea(DataEntity data, ref Chart c)
        {

            //绘图区域设置
            c.ChartAreas.Clear();

            //道路基本情况图
            ChartArea A0 = new ChartArea();
            A0.Name = "A0";

            double xMax, xMin, yMax, yMin;
            MaxMin(data.Points, out xMax, out xMin, out yMax, out yMin);

            A0.AxisX.Minimum = Math.Floor(xMin / 10) * 10;
            A0.AxisX.Maximum = Math.Ceiling(xMax / 10) * 10;
            A0.AxisY.Minimum = Math.Floor(yMin / 10) * 10;
            A0.AxisY.Maximum = Math.Ceiling(yMax / 10) * 10;
            A0.AxisX.Interval = 2;
            A0.AxisY.Interval = 2;

            A0.AxisX.MajorGrid.Enabled = false;
            A0.AxisY.MajorGrid.Enabled = false;
            A0.AxisX.MinorGrid.Enabled = false;
            A0.AxisY.MinorGrid.Enabled = false;

            c.ChartAreas.Add(A0);
        }

        static void MaxMin(List<PointInfo> points, out double xMax, out double xMin,
            out double yMax, out double yMin)
        {
            xMax = points[0].L;
            xMin = points[0].L;
            yMax = points[0].B;
            yMin = points[0].B;
            for (int i = 1; i < points.Count; i++)
            {
                if (xMax < points[i].L)
                    xMax = points[i].L;
                if (xMin > points[i].L)
                    xMin = points[i].L;
                if (yMax < points[i].B)
                    yMax = points[i].B;
                if (yMin > points[i].B)
                    yMin = points[i].B;
            }
        }
    }
}
