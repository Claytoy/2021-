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

namespace 坐标转换
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }
        #region 定义变量
        List<string> point_ID = new List<string>();//创建一个库存放点的ID
        List<double> point_B = new List<double>();//创建一个库存放点的X
        List<double> point_L = new List<double>();//创建一个库存放点的Y
        List<double> point_H = new List<double>();//创建一个库存放点的Z

        List<double> point_X = new List<double>();//放X的库
        List<double> point_Y = new List<double>();//放Y的库
        List<double> point_Z = new List<double>();//放Z的库

        List<double> point_b = new List<double>();//放之后算出来的B的库
        List<double> point_l = new List<double>();//放之后算出来的L的库
        List<double> point_h = new List<double>();//放之后算出来的H的库

        List<double> length = new List<double>();//放中央弧长

        List<double> Gauss_x = new List<double>();//放高斯投影x
        List<double> Gauss_y = new List<double>();//放高斯投影y

        List<double> latitude = new List<double>();//放底点纬度

        List<double> Gauss_B = new List<double>();
        List<double> Gauss_L = new List<double>();
        double a;
        double f;
        double L0;
        double b;
        double e1, e2;//第一偏心率，第二偏心率
        double M0;
        int s;
        Bitmap image;
        #endregion
        #region 目录打开文件
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            pictureBox1.Image = null;
            try
            {
                openFileDialog2.Title = "文件打开";
                openFileDialog2.Filter = "文本文档(*.txt)|*.txt";
                if (openFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    string filepath = openFileDialog2.FileName;
                    string line;
                    string[] strs;//创建一个数组

                    StreamReader sr = new StreamReader(filepath);//将文件数据导入
                    line = sr.ReadLine();//将获取的数据放入string类型line
                    strs = line.Split(',');//以,分割获取的数据
                    a = double.Parse(strs[1]);//将分割的第二个数据放入a

                    line = sr.ReadLine();
                    strs = line.Split(',');
                    f = double.Parse(strs[1]);

                    line = sr.ReadLine();
                    strs = line.Split(',');
                    L0 = double.Parse(strs[1]);

                    line = sr.ReadLine();//略过空行


                    s = 0;
                    while ((line = sr.ReadLine()) != null)//将ID，B，L，H放入库中
                    {
                        //line = sr.ReadLine();
                        strs = line.Split(',');

                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[s].Cells[0].Value = strs[0];
                        dataGridView1.Rows[s].Cells[1].Value = strs[1];
                        dataGridView1.Rows[s].Cells[2].Value = strs[2];
                        dataGridView1.Rows[s].Cells[3].Value = strs[3];

                        point_ID.Add(strs[0]);//ID
                        point_B.Add(double.Parse(strs[1]));//B
                        point_L.Add(double.Parse(strs[2]));//L
                        point_H.Add(double.Parse(strs[3]));//H
                        s++;

                    }

                    b = a * (1 - 1 / f);//计算短半轴



                    e1 = ((a * a) - (b * b)) / (a * a);//e1
                    e2 = (e1 * e1) / (1 - (e1 * e1));//e2

                    M0 = a * (1 - (e1 * e1));//子午圈赤道处的曲率半径






                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }
        #endregion
        #region 目录计算
        private void 计算ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int z = 0;
            for (int i = 0; i < s; i++)//BLH转XYZ转BLH
            {

                double W, n, t, N, M;

                double sinB, cosB, tanB;
                sinB = Math.Round(Math.Sin(point_B[z] * 1.00 / 180 * Math.PI), 5);//sinB
                cosB = Math.Round(Math.Cos(point_B[z] * 1.00 / 180 * Math.PI), 5);//cosB
                tanB = Math.Round(Math.Tan(point_H[z] * 1.00 / 180 * Math.PI), 5);//tanB

                W = Math.Sqrt(1 - (e1 * e1) * (sinB) * sinB);
                n = e2 * cosB;
                t = tanB;

                N = a / W;//卯酉圈的曲率半径
                M = (a * (1 - (e1 * e1)) / (W * W * W));//子午圈曲率半径

                double sinL, cosL;
                sinL = Math.Round(Math.Sin(point_L[z] * 1.00 / 180 * Math.PI), 5);
                cosL = Math.Round(Math.Cos(point_L[z] * 1.00 / 180 * Math.PI), 5);

                point_X.Add((N + point_H[z]) * cosB * cosL);//B转换为X
                point_Y.Add((N + point_H[z]) * cosB * sinL);//L转换为Y
                point_Z.Add((N * (1 - (e1 * e1)) + point_H[z]) * sinB);//H转换为Z




                dataGridView1.Rows[i].Cells[0].Value = point_ID[i];
                dataGridView1.Rows[i].Cells[1].Value = point_B[i];
                dataGridView1.Rows[i].Cells[2].Value = point_L[i];
                dataGridView1.Rows[i].Cells[3].Value = point_H[i];
                dataGridView1.Rows[i].Cells[4].Value = point_X[i];
                dataGridView1.Rows[i].Cells[5].Value = point_Y[i];
                dataGridView1.Rows[i].Cells[6].Value = point_Z[i];

                double X = point_X[z] + 1000;//X+1000
                double Y = point_Y[z] + 1000;//Y+1000
                double Z = point_Z[z] + 1000;//Z+1000

                double tan = Math.Sqrt((Z + Z) + ((Math.Sqrt((X * X) + (Y * Y))) * (Math.Sqrt((X * X) + (Y * Y)))));//求斜边
                sinB = (Z) / tan;//求sinB
                double B1 = Math.Asin(sinB) / Math.PI * 180;//将sinB中的B作为第一个初始的B1
                double B2;

                while (true)//迭代计算 根据B1代入公式算出B2 如果B2达标则跳出循环 如果不达标则将B2算作B1 继续计算
                {
                    double W1 = Math.Sqrt(1 - (e1 * e1) * (sinB) * sinB);
                    double N1 = a / W;
                    B2 = (Math.Atan(((point_Z[z] + 1000) + (N1 * e1 * sinB)) / (Math.Sqrt(((X) * (X)) + ((Y) * (Y)))))) / Math.PI * 180;

                    if (Math.Abs(B2 - B1) <= 0.0000000001)//如果差值小于它则跳出循环
                    {
                        break;
                    }
                    B1 = B2;
                }

                point_l.Add((Math.Atan(Y / X)) / Math.PI * 180);//将算出的L添加入库
                point_b.Add(B2);//将B添加入库
                point_h.Add(((Math.Sqrt((X * X) + (Y * Y))) / (Math.Round(Math.Cos(point_b[z] * 1.00 / 180 * Math.PI), 5))) - N);//将H添加入库

                //高斯正算

                //子午弧长计算准备
                double Ac, Bc, Cc, Dc, Ec, Fc;

                Ac = 1 + ((3 / 4) * e1) + ((45 / 64) * Math.Pow(e1, 2)) + ((175 / 256) * Math.Pow(e1, 3)) + ((11025 / 16384) * Math.Pow(e1, 4)) + ((43659 / 65536) * Math.Pow(e1, 5));
                Bc = ((3 / 4) * e1) + ((15 / 16) * Math.Pow(e1, 2)) + ((252 / 512) * Math.Pow(e1, 3)) + ((2205 / 2048) * Math.Pow(e1, 4)) + ((72765 / 65536) * Math.Pow(e1, 5));
                Cc = ((15 / 64) * Math.Pow(e1, 2)) + ((105 / 256) * Math.Pow(e1, 3)) + ((2205 / 4096) * Math.Pow(e1, 4)) + ((10395 / 16384) * Math.Pow(e1, 5));
                Dc = ((35 / 512) * Math.Pow(e1, 3)) + ((315 / 2048) * Math.Pow(e1, 4)) + ((31185 / 131072) * Math.Pow(e1, 5));
                Ec = ((315 / 16384) * Math.Pow(e1, 4)) + ((3465 / 65536) * Math.Pow(e1, 5));
                Fc = ((693 / 131072) * Math.Pow(e1, 5));

                double alpha, beta, gamma, delta, epsilon, zeta;
                alpha = Ac * M0;
                beta = -1 / 2 * Bc * M0;
                gamma = 1 / 4 * Cc * M0;
                delta = -1 / 6 * Dc * M0;
                epsilon = 1 / 8 * Ec * M0;
                zeta = -1 / 10 * Fc * M0;

                //子午弧长计算

                double sin2B, sin4B, sin6B, sin8B, sin10B;
                sin2B = Math.Round(Math.Sin(point_B[z] * 2.00 / 180 * Math.PI), 5);
                sin4B = Math.Round(Math.Sin(point_B[z] * 4.00 / 180 * Math.PI), 5);
                sin6B = Math.Round(Math.Sin(point_B[z] * 6.00 / 180 * Math.PI), 5);
                sin8B = Math.Round(Math.Sin(point_B[z] * 8.00 / 180 * Math.PI), 5);
                sin10B = Math.Round(Math.Sin(point_B[z] * 10.00 / 180 * Math.PI), 5);
                length.Add(alpha * point_B[z] + beta * sin2B + gamma * sin4B + delta * sin6B + epsilon * sin8B + zeta * sin10B);

                //经差计算准备
                double l = point_L[z] - L0;

                //计算辅助量
                double alpha0, alpha1, alpha2, alpha3, alpha4, alpha5, alpha6;
                alpha0 = length[z];
                alpha1 = N * cosB;
                alpha2 = 1 / 2 * N * Math.Pow(cosB, 2) * t;
                alpha3 = 1 / 6 * N * Math.Pow(cosB, 3) * (1 - t * t + n * n);
                alpha4 = 1 / 24 * N * Math.Pow(cosB, 4) * (5 - t * t + 9 * n * n + 4 * n * n * n * n) * t;
                alpha5 = 1 / 120 * N * Math.Pow(cosB, 5) * (5 - 18 * t * t + t * t * t * t + 15 * n * n - 58 * n * n * t * t);
                alpha6 = 1 / 720 * N * Math.Pow(cosB, 6) * (61 - 58 * t * t + t * t * t * t + 270 * n * n - 330 * n * n * t * t) * t;

                //高斯正算公式
                Gauss_x.Add(alpha0 + alpha2 * l * l + alpha * l * l * l * l + alpha6 * Math.Pow(l, 6));
                Gauss_y.Add((alpha1 * l + alpha3 * l * l * l + alpha5 * Math.Pow(l, 5)) + 500);


                //高斯反算

                //计算底点纬度
                double Bf;
                double XX, YY;
                XX = Gauss_x[z] + 1000;
                YY = Gauss_y[z] + 1000;
                while (true)//迭代计算求底点纬度
                {
                    double B0 = XX / alpha;
                    double sin2B0, sin4B0, sin6B0, sin8B0, sin10B0;
                    sin2B0 = Math.Round(Math.Sin(B0 * 2.00 / 180 * Math.PI), 5);
                    sin4B0 = Math.Round(Math.Sin(B0 * 4.00 / 180 * Math.PI), 5);
                    sin6B0 = Math.Round(Math.Sin(B0 * 6.00 / 180 * Math.PI), 5);
                    sin8B0 = Math.Round(Math.Sin(B0 * 8.00 / 180 * Math.PI), 5);
                    sin10B0 = Math.Round(Math.Sin(B0 * 10.00 / 180 * Math.PI), 5);
                    double triangle = beta * sin2B0 + gamma * sin4B0 + delta * sin6B0 + epsilon * sin8B0 + zeta * sin10B0;
                    Bf = (XX - triangle) / alpha;

                    if (Math.Abs(Bf - B0) < 0.00000001)
                    {
                        break;
                    }
                    B0 = Bf;
                }

                latitude.Add(Bf);//将底点纬度放入库

                //计算辅助量
                double Wf, Nf, nf, Mf, tf;
                double sinBf, cosBf, tanBf;
                sinBf = Math.Round(Math.Sin(latitude[z] * 1.00 / 180 * Math.PI), 5);//sinBf
                cosBf = Math.Round(Math.Cos(latitude[z] * 1.00 / 180 * Math.PI), 5);//cosBf
                tanBf = Math.Round(Math.Tan(latitude[z] * 1.00 / 180 * Math.PI), 5);//tanBf

                Wf = Math.Sqrt(1 - (e1 * e1) * (sinBf) * sinBf);//Wf
                nf = e2 * cosBf;//nf
                tf = tanBf;//tf
                Nf = a / Wf;//卯酉圈的曲率半径
                Mf = (a * (1 - (e1 * e1)) / (Wf * Wf * Wf));//子午圈曲率半径

                double b0, b1, b2, b3, b4, b5, b6;
                b0 = latitude[z];
                b1 = 1 / (Nf * cosBf);
                b2 = -tf / (2 * Mf * Nf);
                b3 = -((1 + (2 * tf * tf) + (nf * nf)) / 6 * (Nf * Nf)) * b1;
                b4 = -((5 + (3 * tf * tf + nf * nf) - (9 * nf * nf * tf * tf)) / (12 * Nf * Nf)) * b2;
                b5 = -((5 + (28 * tf * tf) + (24 * tf * tf * tf * tf) + (6 * nf * nf) + (8 * nf * nf * tf * tf)) / (120 * Nf * Nf * Nf * Nf)) * b1;
                b6 = ((61 + (90 * tf * tf) + (45 * tf * tf * tf * tf)) / (360 * Nf * Nf * Nf * Nf)) * b2;

                //高斯反算公式
                Gauss_B.Add(b0 + b2 * YY * YY + b4 * YY * YY * YY * YY + b6 * Math.Pow(YY, 6));
                Gauss_L.Add(b1 * YY + b3 * YY * YY * YY + b5 * Math.Pow(YY, 5) + L0);

                z++;
            }
        }
        #endregion
        #region 目录显示报告
        private void 报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[2];
            textBox1.Text = "********************坐标转换*******************\r\n";
            textBox1.Text += "\n---------------BLH2XYZ---------------\r\n";
            textBox1.Text += "ID \t B \t L \t H \t X \t Y \t Z \r\n";
            for (int i = 0; i < s; i++)
            {
                textBox1.Text += point_ID[i] + "\t" + Math.Round(point_B[i], 5) + "\t" + Math.Round(point_L[i], 5) + "\t" + Math.Round(point_H[i], 5) + "\t" + Math.Round(point_X[i], 5)
                    + "\t" + Math.Round(point_Y[i], 5) + "\t" + Math.Round(point_Z[i], 5) + "\r\n";

            }

        }
        #endregion
        #region 图形目录打开文件
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            打开ToolStripMenuItem_Click(sender, e);
        }
        #endregion
        #region 目录图像保存
        private void 保存ToolStripMenuItem1_Click(object sender, EventArgs e)
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
        #region 目录报告保存
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
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
        #region 目录绘制
        private void 绘制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[1];
            Graphics g;
            int width = 100;
            int height = 100;


            image = new Bitmap(width, height);
            g = Graphics.FromImage(image);

            Color color = Color.FromArgb(255, 0, 0);//颜色为红色
            Pen p = new Pen(color, 3);//创建一个画笔对象,该画笔的颜色为红色，笔触大小为3个像素
            g.DrawEllipse(p, 50, 50, 100, 100);

            pictureBox1.Image = (Image)image;


        }
        #endregion
    }
}
