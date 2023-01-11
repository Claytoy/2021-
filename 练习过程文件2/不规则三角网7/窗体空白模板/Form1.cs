using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace 窗体空白模板
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        #region 窗体全局对象
        double gaocheng;//存储基准高程
        double xmax, xmin, ymax, ymin;//存储初始矩形信息
        List<Point1> point1;//存储散点信息
        List<Line> S;//存储边信息
        List<sjx> T1;//存储三角形信息
        List<double> V;//存储斜三棱柱体积信息
        Bitmap image;
        public void chushihua()
        {
            point1 = new List<Point1>();
            S = new List<Line>();
            T1 = new List<sjx>();
            V = new List<double>();
        }
        #endregion
        #region 小工具
        #region Tool放大
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Width < 5000)
            {
                pictureBox1.Width = (int)(pictureBox1.Width * 1.2);
                pictureBox1.Height = (int)(pictureBox1.Height * 1.2);
            }
        }
        #endregion
        #region Tool缩小
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Width > 100)
            {
                pictureBox1.Width = (int)(pictureBox1.Width / 1.2);
                pictureBox1.Height = (int)(pictureBox1.Height / 1.2);
                pictureBox1.Image = image;
            }
        }





        #endregion

        #endregion
        #region Menu打开
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            pictureBox1.Image = null;
            textBox1.Text = "";
            try
            {
                openFileDialog1.Title = "文件打开";
                openFileDialog1.Filter = "文本文件(*.txt)|*.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default))
                    {
                        string[] str;
                        int i = 0;
                        while (!sr.EndOfStream)
                        {
                            dataGridView1.Rows.Add();
                            str = sr.ReadLine().Split(',');
                            for (int j = 0; j < str.Length; j++)
                            {
                                dataGridView1.Rows[i].Cells[j].Value = str[j];
                            }
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        #endregion
        #region Menu计算1
        private void 计算1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chushihua();
            dataGridView1.AllowUserToAddRows = false;

            #region 数据导入
            //try
            //{
            //    gaocheng = Convert.ToDouble(textBox1.Text.Replace(" ", ""));
            //}
            //catch
            //{
            //    MessageBox.Show("请正确输入基准高程！");
            //    return;
            //}
            gaocheng = 9.0;
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    Point1 p = new Point1();
                    p.dianhao = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    p.X = Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value.ToString().Replace(" ", ""));
                    p.Y = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value.ToString().Replace(" ", ""));
                    p.Z = Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value.ToString().Replace(" ", ""));
                    point1.Add(p);
                }
            }
            catch
            {
                MessageBox.Show("请正确输入散点信息！");
                return;
            }
            #endregion
            try
            {
                #region 生成初始三角网
                xmax = 0; ymax = 0;
                xmin = 100000000000;
                ymin = 100000000000;
                for (int i = 0; i < point1.Count; i++)
                {
                    if (xmax < point1[i].X)
                    { xmax = point1[i].X; }
                    if (ymax < point1[i].Y)
                    { ymax = point1[i].Y; }
                    if (xmin > point1[i].X)
                    { xmin = point1[i].X; }
                    if (ymin > point1[i].Y)
                    { ymin = point1[i].Y; }
                }
                Point1 p1 = new Point1();
                Point1 p2 = new Point1();
                Point1 p3 = new Point1();
                Point1 p4 = new Point1();
                p1.X = xmin - 1; p1.Y = ymin - 1;
                p2.X = xmin - 1; p2.Y = ymax + 1;
                p3.X = xmax + 1; p3.Y = ymax + 1;
                p4.X = xmax + 1; p4.Y = ymin - 1;
                sjx sjx1 = new sjx();//添加一个三角形必须实例化一个三角形，否则无法赋值
                sjx1.p1 = p1; sjx1.p2 = p2; sjx1.p3 = p3;
                T1.Add(sjx1);
                sjx sjx2 = new sjx();
                sjx2.p1 = p1; sjx2.p2 = p3; sjx2.p3 = p4;
                T1.Add(sjx2);
                #endregion
                #region 生成平面三角网
                double x0, y0, r;
                for (int i = 0; i < point1.Count; i++)//对每个离散点进行遍历
                {
                    List<sjx> T2 = new List<sjx>();
                    S.Clear();
                    List<int> b = new List<int>();//记录T1中要剪切的三角形索引
                    for (int j = 0; j < T1.Count; j++)//步骤2
                    {
                        x0 = caculate.X0(T1[j].p1.X, T1[j].p1.Y, T1[j].p2.X, T1[j].p2.Y, T1[j].p3.X, T1[j].p3.Y);
                        y0 = caculate.Y0(T1[j].p1.X, T1[j].p1.Y, T1[j].p2.X, T1[j].p2.Y, T1[j].p3.X, T1[j].p3.Y);
                        r = caculate.R(x0, y0, T1[j].p1.X, T1[j].p1.Y);
                        //MessageBox.Show(x0.ToString());
                        if ((point1[i].X - x0) * (point1[i].X - x0) + (point1[i].Y - y0) * (point1[i].Y - y0) < r * r)//在三角形内部
                        {
                            Line line1 = new Line();
                            Line line2 = new Line();
                            Line line3 = new Line();
                            line1.Begin = T1[j].p1; line1.End = T1[j].p2;
                            line2.Begin = T1[j].p2; line2.End = T1[j].p3;
                            line3.Begin = T1[j].p3; line3.End = T1[j].p1;
                            S.Add(line1); S.Add(line2); S.Add(line3);//把T2中所有三角形的边信息存储到S边列表中

                            T2.Add(T1[j]);//T2添加三角形
                            b.Add(j);
                        }
                    }
                    for (int j = b.Count - 1; j >= 0; j--)//列表数据的结构决定了必须从后往前移除数据
                    {
                        T1.Remove(T1[b[j]]);//T1移除三角形，实现T1向T2剪切
                    }
                    //MessageBox.Show(T1.Count.ToString());
                    //MessageBox.Show(T2.Count.ToString());
                    //MessageBox.Show(S.Count.ToString());
                    for (int j = 0; j < S.Count; j++)//步骤4
                    {
                        for (int k = 0; k < S.Count; k++)
                        {
                            if (j != k)//不同边相互比较
                            {
                                if (S[j].Begin == S[k].Begin && S[j].End == S[k].End || S[j].Begin == S[k].End && S[j].End == S[k].Begin)//判断两条边是否重合
                                {
                                    S.Remove(S[k]);//j < k
                                    S.Remove(S[j]);
                                    if (j == 0)//为了减少循环次数
                                    { j = 0; }
                                    else
                                    { j--; }
                                }
                            }
                        }
                    }
                    //MessageBox.Show(S.Count.ToString());
                    for (int j = 0; j < S.Count; j++)//步骤5
                    {
                        sjx sjx3 = new sjx();
                        sjx3.p1 = point1[i]; sjx3.p2 = S[j].Begin; sjx3.p3 = S[j].End;
                        T1.Add(sjx3);
                    }
                }
                #endregion
                #region 构成不规则三角网
                List<int> a = new List<int>();//存储要删除的三角形的索引信息
                for (int i = 0; i < T1.Count; i++)
                {
                    if (T1[i].p1 == p1 || T1[i].p1 == p2 || T1[i].p1 == p3 || T1[i].p1 == p4
                     || T1[i].p2 == p1 || T1[i].p2 == p2 || T1[i].p2 == p3 || T1[i].p2 == p4
                     || T1[i].p3 == p1 || T1[i].p3 == p2 || T1[i].p3 == p3 || T1[i].p3 == p4)
                    {
                        a.Add(i);
                    }
                }
                for (int j = a.Count - 1; j >= 0; j--)
                {
                    T1.Remove(T1[a[j]]);
                }
                #endregion
                //MessageBox.Show(T1.Count.ToString());
                #region 绘制不规则三角网
                image = new Bitmap((int)(ymax - ymin + 20) * 20, (int)(xmax - xmin + 20)*20);//放大整个绘图区域20倍，为了弥补位图分辨率低的缺点
                Graphics g = Graphics.FromImage(image);
                Pen pen = new Pen(Color.Black, 2f);
                SolidBrush brush = new SolidBrush(Color.Red);//定义填充
                for (int i = 0; i < T1.Count; i++)//对所有三角形进行绘制
                {
                    #region 绘制三角形
                    
                    chart1.ChartAreas[0].AxisX.Minimum = xmin-25;
                    chart1.ChartAreas[0].AxisX.Maximum = xmax+25;
                    chart1.ChartAreas[0].AxisY.Minimum = ymin-25;
                    chart1.ChartAreas[0].AxisY.Maximum = ymax+25;

                    for(int k = 0; k < T1.Count; k++)
                    {
                        chart1.Series[0].Points.AddXY(T1[k].p1.X, T1[k].p1.Y);
                        chart1.Series[0].Points.AddXY(T1[k].p2.X, T1[k].p2.Y);

                        chart1.Series[0].Points.AddXY(T1[k].p1.X, T1[k].p1.Y);
                        chart1.Series[0].Points.AddXY(T1[k].p3.X, T1[k].p3.Y);

                        chart1.Series[0].Points.AddXY(T1[k].p2.X, T1[k].p2.Y);
                        chart1.Series[0].Points.AddXY(T1[k].p3.X, T1[k].p3.Y);
                    }































                    //PointF[] pp = new PointF[3];
                    //pp[0].X = (float)(T1[i].p1.Y - (int)ymin + 5) * 20;
                    //pp[0].Y = -(float)(T1[i].p1.X - (int)xmax - 5) * 20;
                    //pp[1].X = (float)(T1[i].p2.Y - (int)ymin + 5) * 20;
                    //pp[1].Y = -(float)(T1[i].p2.X - (int)xmax - 5) * 20;
                    //pp[2].X = (float)(T1[i].p3.Y - (int)ymin + 5) * 20;
                    //pp[2].Y = -(float)(T1[i].p3.X - (int)xmax - 5) * 20;
                    //pp[0].X = (float)(T1[i].p1.Y - (int)ymin + 5) * 20;
                    //pp[0].Y = -(float)(T1[i].p1.X - (int)xmax - 5) * 20;
                    //g.DrawLines(pen, pp);
                    #endregion
                    #region 绘制直线
                    PointF begion = new PointF();
                    PointF end = new PointF();
                    //任意点的X和Y减去最小值归化到原点，再乘以20倍以适应位图大小
                    for (int j = 0; j < 3; j++)
                    {
                        if (j == 0)//p1-p2
                        {
                            begion.X = (float)(T1[i].p1.Y - (int)ymin + 10) * 20; //X和Y调换，(数学坐标系转换到测量坐标系)
                            begion.Y = -(float)(T1[i].p1.X - (int)xmax - 10) * 20;//X减去max再变成正数(像素坐标系转换数学坐标系)
                            end.X = (float)(T1[i].p2.Y - (int)ymin + 10) * 20;
                            end.Y = -(float)(T1[i].p2.X - (int)xmax - 10) * 20;
                        }
                        if (j == 1)//p2-p3
                        {
                            begion.X = (float)(T1[i].p2.Y - (int)ymin + 10) * 20;
                            begion.Y = -(float)(T1[i].p2.X - (int)xmax - 10) * 20;
                            end.X = (float)(T1[i].p3.Y - (int)ymin + 10) * 20;
                            end.Y = -(float)(T1[i].p3.X - (int)xmax - 10) * 20;
                        }
                        if (j == 2)//p3-p1
                        {
                            begion.X = (float)(T1[i].p3.Y - (int)ymin + 10) * 20;
                            begion.Y = -(float)(T1[i].p3.X - (int)xmax - 10) * 20;
                            end.X = (float)(T1[i].p1.Y - (int)ymin + 10) * 20;
                            end.Y = -(float)(T1[i].p1.X - (int)xmax - 10) * 20;
                        }
                        g.DrawLine(pen, begion, end);
                        g.FillEllipse(brush, begion.X - 10, begion.Y - 10, 20, 20);
                    }
                    #endregion
                }
                #region 绘制点
                for (int i = 0; i < point1.Count; i++)
                {
                    g.DrawString(point1[i].dianhao, new Font("宋体", 20), Brushes.Blue, (float)(point1[i].Y - (int)ymin + 10) * 20 + 10f, -(float)(point1[i].X - (int)xmax - 10) * 20 + 10f);
                }
                #endregion
                pictureBox1.Image = image;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region Menu显示报告
        private void 显示报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[2];

        }
        #endregion
        #region Menu 绘制
        private void 绘制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[1];
            try
            {
               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        #endregion

        #region Tool打开
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            打开ToolStripMenuItem_Click(sender, e);
        }
        #endregion
        #region Tool一键计算
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            计算1ToolStripMenuItem_Click(sender, e);
        }

        private void 计算2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region 体积计算
            for (int i = 0; i < T1.Count; i++)
            {
                double SS, H;
                SS = caculate.SS(T1[i]);
                H = caculate.H(T1[i], gaocheng);
                V.Add(SS * H);
            }
            #endregion
            #region 体积排序
            for (int i = 0; i < V.Count; i++)//冒泡排序算法
            {
                for (int j = 0; j < V.Count - 1 - i; j++)
                {
                    if (V[j] > V[j + 1])
                    {
                        double temp = V[j + 1];
                        V[j + 1] = V[j];
                        V[j] = temp;
                    }
                }
            }
            #endregion
            #region 计算报告
            textBox1.Text = "不规则三角网体积计算\n";
            textBox1.Text += "---------------三角网基本信息---------------\r\n";
            textBox1.Text += "基准高程：  " + gaocheng + "\r\n";
            textBox1.Text += "三角形个数：  " + T1.Count + "\r\n";
            textBox1.Text += "体积：  " + Math.Round(V.Sum(), 3) + "\n\n";
            textBox1.Text += "---------------20个三角形说明---------------\r\n";
            textBox1.Text += "序号\t三个顶点\r\n";
            for (int i = 0; i < 20; i++)
            {
                textBox1.Text += (i + 1) + "\t";
                textBox1.Text += T1[i].p1.dianhao + T1[i].p2.dianhao + T1[i].p3.dianhao + "\r\n";
            }
            textBox1.Text += "\n---------------体积最小的5个三棱柱体积---------------\r\n";
            for (int i = 0; i < 5; i++)
            {
                textBox1.Text += (i + 1) + "\t";
                textBox1.Text += Math.Round(V[i], 3) + "\r\n";
            }
            textBox1.Text += "\n---------------体积最大的5个三棱柱体积---------------\r\n";
            for (int i = 0; i < 5; i++)
            {
                textBox1.Text += (i + 1) + "\t";
                textBox1.Text += Math.Round(V[V.Count - 1 - i], 3) + "\r\n";
            }
            #endregion
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Menu保存报告
        private void 保存报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Title = "报告保存";
                saveFileDialog1.Filter = "文本文档(*.txt)|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string path = saveFileDialog1.FileName;
                    if (textBox1.Text != "")
                    {
                        using (StreamWriter writer = new StreamWriter(path))
                            writer.Write(this.textBox1.Text);
                    }
                    MessageBox.Show("保存成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        #endregion
        #region Menu保存bmp
        private void 保存图像为bmpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Title = "bmp图形保存";
                saveFileDialog1.Filter = "位图文件(*.bmp)|*.bmp";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    image.Save(saveFileDialog1.FileName);
                    MessageBox.Show("保存成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        #endregion
        #region Menu保存dxf
        private void 保存图像为dxfToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
