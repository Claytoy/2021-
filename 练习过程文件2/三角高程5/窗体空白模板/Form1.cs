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
        Bitmap image;
        PRO pro = new PRO();
        Calculate Cal = new Calculate();
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
            }
        }





        #endregion
        #region 时钟
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel3.Text = DateTime.Now.ToString();
        }
        #endregion
        #endregion
        #region Menu打开
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            pictureBox1.Image = null;
            try
            {
                openFileDialog1.Title = "文件打开";
                openFileDialog1.Filter = "文本文档(*.txt)|*.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filepath = openFileDialog1.FileName;
                    string line;
                    string[] strs;
                    StreamReader sr = new StreamReader(filepath);
                    Point kp = new Point();
                    line = sr.ReadLine();
                    strs = line.Split(',');
                    kp.Name = strs[0];
                    kp.Height = double.Parse(strs[1]);
                    Cal.points.Add(kp);

                    line = sr.ReadLine();

                    while ((line = sr.ReadLine()) != null)
                    {
                        strs = line.Split('-');
                        data kp1 = new data();
                        kp1.Start = strs[0];
                        kp1.End = strs[1];

                        line = sr.ReadLine();
                        strs = line.Split(',');
                        kp1.G_station = double.Parse(strs[0]);
                        kp1.G_aim = double.Parse(strs[1]);
                        kp1.G_longth = double.Parse(strs[2]);
                        kp1.G_angle = double.Parse(strs[3]);

                        line = sr.ReadLine();
                        strs = line.Split(',');
                        kp1.C_station = double.Parse(strs[0]);
                        kp1.C_aim = double.Parse(strs[1]);
                        kp1.C_longth = double.Parse(strs[2]);
                        kp1.C_angle = double.Parse(strs[3]);

                        Cal.Data.Add(kp1);
                    }

                    int n = 0;
                    for (int i = 0; i < Cal.Data.Count; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = Cal.Data[i].Start;
                        dataGridView1.Rows[n].Cells[1].Value = Cal.Data[i].End;
                        dataGridView1.Rows[n].Cells[2].Value = Cal.Data[i].G_station;
                        dataGridView1.Rows[n].Cells[3].Value = Cal.Data[i].G_aim;
                        dataGridView1.Rows[n].Cells[4].Value = Cal.Data[i].G_longth;
                        dataGridView1.Rows[n].Cells[5].Value = Cal.Data[i].G_angle;
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[n+1].Cells[2].Value = Cal.Data[i].C_station;
                        dataGridView1.Rows[n+1].Cells[3].Value = Cal.Data[i].C_aim;
                        dataGridView1.Rows[n+1].Cells[4].Value = Cal.Data[i].C_longth;
                        dataGridView1.Rows[n+1].Cells[5].Value = Cal.Data[i].C_angle;
                        n += 2;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
         }
        #endregion
        #region Menu计算1
        private void 计算1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[0];
            try
            {
                //球气差改正数计算
                for (int i = 0; i < Cal.Data.Count; i++)
                {
                    double R = 6378137;
                    double k = 0.15;


                    double G_alpha = 0;
                    G_alpha = turn.angle2rad(Cal.Data[i].G_aim);
                    double C_alpha = 0;
                    C_alpha = turn.angle2rad(Cal.Data[i].C_aim);

                    //往平距
                    double G_D = 0;
                    G_D = Math.Round((Cal.Data[i].G_station + Cal.Data[i].C_station) / 2, 4);

                    //往地球曲率
                    double G_p = 0;
                    G_p = Math.Pow(G_D, 2) / (2 * R);

                    //往大气折光
                    double G_r = 0;
                    G_r = (-k * Math.Pow(G_D, 2)) / (2 * R);

                    //往球气差
                    double G_f = 0;
                    G_f = G_p + G_r;
                    G_f = Math.Round(G_f, 4);

                    //返平距

                    double C_D = 0;
                    C_D = G_D;


                    //返地球曲率
                    double C_p = 0;
                    C_p = Math.Pow(C_D, 2) / (2 * R);

                    //返大气折光
                    double C_r = 0;
                    C_r = (-k * Math.Pow(C_D, 2)) / (2 * R);

                    //返球气差
                    double C_f = 0;
                    C_f = C_p + C_r;
                    C_f = Math.Round(C_f, 4);

                    Cal.Data[i].G_D = G_D;
                    Cal.Data[i].G_f = G_f;
                    Cal.Data[i].C_D = C_D;
                    Cal.Data[i].C_f = C_f;

                    //往测高差
                    double G_h = 0;
                    double aaa = Math.Tan(G_alpha);
                    G_h = Math.Round(G_D * Math.Tan(G_alpha) + Cal.Data[i].G_longth - Cal.Data[i].G_angle + Math.Round(G_f, 4), 4);

                    //返测高差
                    double C_h = 0;
                    C_h = Math.Round(C_D * Math.Tan(C_alpha) + Cal.Data[i].C_longth - Cal.Data[i].C_angle + Math.Round(C_f, 4), 4);

                    Cal.Data[i].G_h = G_h;
                    Cal.Data[i].C_h = C_h;

                    //超限检查
                    double a = 0;
                    a = Math.Abs(G_h + C_h);
                    double deta = 0;
                    deta = 60 * G_D;
                    if (a < G_D)
                    {
                        Console.WriteLine("对向观测高差较差合格");
                    }
                    else if (a > G_D)
                    {
                        Console.WriteLine("对向观测高差较差不合格");
                    }

                    //高程计算
                    double h = 0;
                    h = (G_h - C_h) / 2;
                    Cal.Data[i].H = h;

                    double H = 0;
                    H = Cal.points[i].Height + h;

                    Point kp = new Point();
                    kp.Name = Cal.Data[i].End;
                    kp.Height = H;
                    Cal.points.Add(kp);
                }
                ////////////
                //计算高差闭合差
                //计算高差闭合差
                double fh = 0;
                double hi = 0;
                double Ds = 0;
                for (int i = 0; i < Cal.Data.Count; i++)
                {
                    hi = Math.Round(hi + Cal.Data[i].H, 4);
                    Ds = Ds + Cal.Data[i].G_D;
                    Cal.R.Ls.Add(Ds);
                }
                fh = Math.Round(hi, 4);
                Cal.R.fh = fh;
                for (int i = 0; i < Cal.Data.Count; i++)
                {


                    //高差改正数计算
                    double delt = 0;

                    delt = Math.Round(-fh * Cal.Data[i].G_D / Ds, 4);
                    Cal.Data[i].V = delt;

                    //计算改正后的高差
                    double v_hi = 0;
                    v_hi = Math.Round(Cal.Data[i].H + delt, 4);

                    //计算观测点高程
                    double v_H = 0;
                    if (i == 0)
                    {
                        v_H = Math.Round(Cal.points[i].Height + v_hi, 4);
                    }
                    else if (i != 0)
                    {
                        v_H = Math.Round(Cal.points[i].V_height + v_hi, 4);
                    }

                    Cal.points[i + 1].V_height = v_H;
                    Cal.R.Hs.Add(v_H);

                }



                /////
                int n = 0;
                for(int i = 0; i < Cal.points.Count; i++)
                {
                    dataGridView1.Rows[n].Cells[6].Value = Cal.points[i].Height;
                    dataGridView1.Rows[n].Cells[7].Value = Cal.points[i].V_height;


                    n += 2;
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region Menu显示报告
        private void 显示报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[2];
            textBox1.Text = "-------------------三角高程---------------\r\n";
            textBox1.Text += "闭合差：" + Cal.R.fh+"\r\n";
            textBox1.Text += "测段       改正前高差           改正后高差\r\n";
            for(int i = 0; i<Cal.Data.Count; i++)
            {
                textBox1.Text += Cal.Data[i].Start + "-" + Cal.Data[i].End + "\t" + Cal.points[i].Height + "\t                      " + Convert.ToString(Cal.points[i].V_height)+"\r\n";
            }





        }
        #endregion
        #region Menu 绘制
        private void 绘制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[1];
            try
            {
                int width = (int)(Cal.R.Ls.Max() - Cal.R.Ls.Min() + 200);
                int height = (int)(1000*(Cal.R.Hs.Max() - Cal.R.Hs.Min()) + 200);
                image = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(image);
                PointF[] pf = new PointF[Cal.R.Ls.Count];
                Pen p = new Pen(Color.Black, 2.5f);//定义画笔
                Pen p2 = new Pen(Color.Blue, 2);
                SolidBrush brush = new SolidBrush(Color.Black);//定义填充
                for(int i = 0; i < Cal.R.Ls.Count; i++)
                {
                    pf[i].X = (float)(Cal.R.Ls[i] - Cal.R.Ls.Min() + 100);
                    pf[i].Y = (float)(Math.Abs((Cal.R.Hs[i] - Cal.R.Hs.Max())) * 1000 + 100);
                }
                g.DrawLines(p, pf);


                PointF[] xpt = new PointF[3] { new PointF(50, 35), new PointF(40, 50), new PointF(60, 50) };
                PointF[] ypt = new PointF[3] { new PointF(width - 35, height - 50), new PointF(width - 50, height - 60), new PointF(width - 50, height - 40) };
                g.DrawLine(p, 50, height - 50, 50, 50);//画x轴
                g.DrawLine(p, 50, height - 50, width - 50, height - 50);//画y轴
                g.FillPolygon(brush, xpt);//x轴箭头
                g.FillPolygon(brush, ypt);//y轴箭头
                g.DrawString("X/高程", new Font("宋体", 10), Brushes.Black, 0, 40);
                g.DrawString("Y/距离", new Font("宋体", 10), Brushes.Black, width - 60, height - 20);//注记文字是在点位的右上角绘制
                pictureBox1.Image = image;
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
