using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ChartDirector;

namespace LevelLib
{
    /// <summary>
    /// 图形绘制
    /// </summary>
    public class DrawImage
    {
        public static Image Draw(List<PointInfo> points, int width, int height)
        {
            Chart.setLicenseCode("SXZVFNRN9MZ9L8LGA0E2B1BB");
            int count = points.Count;
            double[] dataX = new double[count];
            double[] dataY = new double[count];
            string[] labels = new string[count];
            for (int i = 0; i < count; i++)
            {
                dataX[i] = points[i].Distance;
                dataY[i] = points[i].Height;
                labels[i] = points[i].Name;
            }

            XYChart c = new XYChart(width, height); //520, 300


            c.setPlotArea(55, 5, width - 70, height - 50, -1, -1, 0xc0c0c0, -1, -1);


            c.yAxis().setTitle("高程(m)", "宋体", 10);
            c.xAxis().setTitle("距离(m)", "宋体", 10);

            // Set the axes line width to 3 pixels
            c.xAxis().setWidth(2);
            c.yAxis().setWidth(2);


            ScatterLayer layer = c.addScatterLayer(dataX, dataY, "", Chart.CircleSymbol, 11, 0x33ff33);

            layer.addExtraField(labels);
            // Set the data label format to display the extra field
            layer.setDataLabelFormat("{field0}");

            TextBox textbox = layer.setDataLabelStyle("Arial Bold", 8);
            textbox.setAlignment(Chart.Left);
            textbox.setPos(3, 0);

          
            var bmp = c.makeImage();

            return bmp;
        }
    }
}
