using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 附和导线近似平差
{
    public partial class FormMain : Form
    {

        Calculate Cal = new Calculate();
        public FormMain()
        {
            InitializeComponent();
        }
        #region Tool 打开
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            打开ToolStripMenuItem_Click(sender, e);
        }
        #endregion
        #region Tool 计算
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            toolStripMenuItem2_Click(sender, e);
            toolStripMenuItem3_Click(sender, e);
        }
        #endregion
        #region Menu 打开
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
                    line = sr.ReadLine();
                    strs = line.Split(',');
                    Cal.Data.Mid = double.Parse(strs[0]);//中误差存储
                    Cal.Data.Ad = double.Parse(strs[1]);//加常数存储
                    Cal.Data.Mu = double.Parse(strs[2]);//乘常数存储

                    Point kp1 = new Point();
                    //存第一个已知点
                    line = sr.ReadLine();
                    strs = line.Split(',');
                    kp1.Cstation = strs[0];
                    kp1.X = double.Parse(strs[1]);
                    kp1.Y = double.Parse(strs[2]);



                    //记录第二个已知点
                    line = sr.ReadLine();
                    strs = line.Split(',');
                    string Kc1 = strs[0];
                    kp1.aimStation2 = Kc1;
                    Cal.Points.Add(kp1);
                    double KX1 = double.Parse(strs[1]);
                    double KY1 = double.Parse(strs[2]);

                    //记录第三个已知点
                    line = sr.ReadLine();
                    strs = line.Split(',');
                    string Kc2 = strs[0];
                    double KX2 = double.Parse(strs[1]);
                    double KY2 = double.Parse(strs[2]);

                    //记录第四个已知点
                    line = sr.ReadLine();
                    strs = line.Split(',');
                    string Kc3 = strs[0];
                    double KX3 = double.Parse(strs[1]);
                    double KY3 = double.Parse(strs[2]);



                    int count = 1;
                    Point tp = new Point();

                    while ((line = sr.ReadLine()) != null)//如果还有数据就进行
                    {
                        strs = line.Split(',');//分割
                        Point p = new Point();
                        if (strs.Count() >= 2)//如果分割后得项目大于等于2项
                        {
                            for (int i = 0; i < strs.Count(); i++)//进行一个不大于分割后总项目数得循环
                            {
                                if (strs[i] == "S")//如果分割项目中有S项
                                {
                                    for (int j = 0; j < Cal.Points.Count(); j++)//进行一个不大于已存储点数得循环
                                    {
                                        if (Cal.Points[j].Cstation == strs[0])//如果已存储点中得点名和S前得点名相同
                                        {
                                            Cal.Points[j - 1].S = double.Parse(strs[2]);//将S后得数据存储进相同点名得数据中
                                        }
                                    }
                                }
                                else if (strs[i] == "L")//如果分割项目中有L项
                                {
                                    if (count % 2 == 0)//如果是偶数行 则将数据存入tp中
                                    {
                                        tp.aimStation1 = strs[0];
                                        tp.Ql = double.Parse(strs[2]);
                                        count++;
                                    }
                                    else if (count % 2 != 0)//如果不是偶数行 则将数据从tp存入p中 和 数据直接存入p
                                    {
                                        p.Cstation = tp.Cstation;
                                        p.aimStation1 = tp.aimStation1;
                                        p.Ql = tp.Ql;
                                        p.aimStation2 = strs[0];
                                        p.Hl = double.Parse(strs[2]);

                                        if (p.Cstation == Kc1)//如果测站名字和第二个已知数据测站一样 则导入XY
                                        {
                                            p.X = KX1;
                                            p.Y = KY1;
                                        }
                                        else if (p.Cstation == Kc2)//如果测站名字和第三个已知数据测站一样 则导入XY
                                        {
                                            p.X = KX2;
                                            p.Y = KY2;
                                        }
                                        Cal.Points.Add(p);

                                    }

                                }
                            }
                        }
                        else
                        {
                            tp.Cstation = strs[0];
                            count++;

                        }

                    }

                    //存储第四个已知测站
                    Point kp2 = new Point();
                    kp2.Cstation = Kc3;
                    kp2.X = KX3;
                    kp2.Y = KY3;
                    kp2.aimStation1 = Kc2;
                    Cal.Points.Add(kp2);

                }
                //show
                int n = 0;
                for (int i = 0; n < Cal.Points.Count(); i+=2)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows.Add();
       
                    dataGridView1.Rows[i].Cells[0].Value = Cal.Points[n].Cstation;
                    dataGridView1.Rows[i].Cells[1].Value = Cal.Points[n].Hl;
                    n++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        #endregion

        #region 保存事件
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
           
        }
        #endregion
        #region Menu 绘图
        Bitmap image;
        private void 绘制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[1];
            //int width = pictureBox1.Width;
            //int height = pictureBox1.Height;

            //image = new Bitmap(width, height);

            //Graphics g = Graphics.FromImage(image);
            //Pen pAxis = new Pen(Brushes.Black, 2);

            //float move = 10;
            //float newX = image.Width - move;
            //float newY = image.Height - move;


            //g.FillEllipse(Brushes.Red, 200, 220, 4, 4);


            ///* 绘制X轴 */
            //PointF px1 = new PointF(move + 10f, newY);
            //PointF px2 = new PointF(newX, newY);
            //g.DrawLine(pAxis, px1, px2);
            ///* 绘制Y轴 */
            //PointF py1 = new PointF(move + 10f, move);
            //PointF py2 = new PointF(move + 10f, newY);
            //g.DrawLine(pAxis, py2, py1);
            ////Y轴三角形  
            //PointF[] yPt = new PointF[3]{
            // new   PointF(py1.X,py1.Y-10),
            // new   PointF(py1.X+5,py1.Y),
            // new   PointF(py1.X-5,py1.Y)};//Y轴三角形  
            //g.DrawPolygon(pAxis, yPt);
            ////X轴三角形  
            //PointF[] xPt = new PointF[3]{
            // new   PointF(px2.X + 10,px2.Y),
            // new   PointF(px2.X,px2.Y + 5),
            // new   PointF(px2.X,px2.Y - 5)};//Y轴三角形  
            //g.DrawPolygon(pAxis, xPt);
            try
            {


                double XMax = 0, YMax = 0, XMin = Cal.Points[0].X, YMin = Cal.Points[0].Y;
                for (int i = 0; i < Cal.Points.Count; i++)
                {
                    double x = Cal.Points[i].X;
                    double y = Cal.Points[i].Y;
                    if (x > XMax)
                        XMax = x;
                    if (x < XMin)
                        XMin = x;
                    if (y > YMax)
                        YMax = y;
                    if (y < YMin)
                        YMin = y;
                }
                int height = (int)(YMax - YMin + 200);
                int width = (int)(XMax - XMin + 200);

                image = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(image);
                Pen pAxis = new Pen(Brushes.Black, 2);
                SolidBrush brush = new SolidBrush(Color.Black);//定义填充

                PointF[] pf = new PointF[Cal.Points.Count];
                for (int i = 0; i < Cal.Points.Count; i++)
                {
                    float x = (float)(Cal.Points[i].X - XMin + 100);
                    float y = (float)(Cal.Points[i].Y - YMin + 100);
                    g.FillEllipse(brush, x - 2.5f, y - 2.5f, 15, 15);
                    pf[i].X = x;
                    pf[i].Y = y;
                }
                g.DrawLines(pAxis, pf);

                //绘制坐标轴
                PointF[] xpt = new PointF[3] { new PointF(50, 35), new PointF(40, 50), new PointF(60, 50) };
                PointF[] ypt = new PointF[3] { new PointF(width - 35, height - 50), new PointF(width - 50, height - 60), new PointF(width - 50, height - 40) };
                g.DrawLine(pAxis, 50, height - 50, 50, 50);//画x轴
                g.DrawLine(pAxis, 50, height - 50, width - 50, height - 50);//画y轴
                g.FillPolygon(brush, xpt);//x轴箭头
                g.FillPolygon(brush, ypt);//y轴箭头
                g.DrawString("X", new Font("宋体", 30), Brushes.Black, 20, 40);
                g.DrawString("Y", new Font("宋体", 30), Brushes.Black, width - 60, height - 40);//注记文字是在点位的右上角绘制

                double deltx = XMax - XMin;
                double delty = YMax - YMin;

                for (int i = 0; i < deltx; i += (int)(deltx / 10))
                {
                    g.DrawString(((int)(XMin + i)).ToString(), new Font("宋体", 20), Brushes.Black, 0, height - 100 - i);
                    g.DrawLine(pAxis, 50, height - 100 - i, 65, height - 100 - i);
                }

                for (int i = 0; i < delty; i += (int)(delty / 10))
                {
                    g.DrawString(((int)(YMin + i)).ToString(), new Font("宋体", 20), Brushes.Black, 100 + i, height - 40);
                    g.DrawLine(pAxis, 100 + i, height - 50, 100 + i, height - 65);
                }

                pictureBox1.Image = image;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region Menu 图形保存为bmp
        private void 保存为bmpToolStripMenuItem_Click(object sender, EventArgs e)
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
        #region Menu 图形保存为dxf
        private void 保存为dxfToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion
        #region Menu 保存报告
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

        #region Tool 缩放
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Width < 5000)
            {
                pictureBox1.Width = Convert.ToInt32(pictureBox1.Width * 1.2);
                pictureBox1.Height = Convert.ToInt32(pictureBox1.Height * 1.2);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Width > 100)
            {
                pictureBox1.Width = Convert.ToInt32(pictureBox1.Width / 1.2);
                pictureBox1.Height = Convert.ToInt32(pictureBox1.Height / 1.2);
            }
        }
        #endregion



        #region 角度计算
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                //起始方位角
                double detaY = Cal.Points[1].Y - Cal.Points[0].Y;
                double detaX = Cal.Points[1].X - Cal.Points[0].X;
                double Firsta = Math.Atan(detaY / detaX * 1.00);

                //坐标反算边长
                double Firsts = detaY / Math.Sin(Firsta);
                Firsta = Firsta / Math.PI * 180;
                Firsta = count.tentoangle(Firsta);

                //起始方位角判断
                if (detaY != 0)
                {
                    if (detaX < 0)
                    {
                        Firsta = 180 + Firsta;
                    }
                    else if (detaX > 0)
                    {

                    }
                }

                if (detaX == 0)
                {
                    if (detaY > 0)
                    {
                        Firsta = 90;
                    }
                    else if (detaY < 0)
                    {
                        Firsta = 270;
                    }
                }

                if (Firsta < 0)
                {
                    Firsta = Firsta + 360;
                }
                else if (Firsta > 360)
                {
                    Firsta = Firsta - 360;
                }

                angle k = new angle();
                k.alpha = Firsta;
                Cal.betas.Add(k);

                double alpha = Firsta + 360;

                for (int i = 1; i < Cal.Points.Count - 1; i++)
                {
                    //计算转角
                    angle kp = new angle();
                    double beta = 0;
                    beta = Math.Round(Cal.Points[i].Hl - Cal.Points[i].Ql, 7);

                    kp.beta = beta;


                    //计算目标方位角
                    //if (beta < 180)
                    //{
                    double a1 = 0;
                    a1 = count.toS(alpha);
                    double b1 = 0;
                    b1 = count.toS(beta);
                    double c1 = 0;
                    c1 = count.toS(180);
                    alpha = a1 + b1 - c1;
                    //}
                    //else if (beta > 180)
                    //{
                    //    double a2 = 0;
                    //    a2 = count.toS(alpha);
                    //    double b2 = 0;
                    //    b2 = count.toS(beta);
                    //    double c2 = 0;
                    //    c2 = count.toS(180);
                    //    alpha = a2 - b2 + c2;
                    //}

          
                    alpha = count.toAngle(alpha);
                    kp.alpha = alpha;
                    Cal.betas.Add(kp);
                }
             }
            catch(Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }

            int n = 0;
            for(int i =1; n < Cal.Points.Count()-1; i+=2)
            {
                dataGridView1.Rows[i].Cells[2].Value = Cal.betas[n].alpha;
                n++;
            }
        }
        #endregion
        #region 生成报告
        private void 生成报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = 0;

            tabControl1.SelectedTab = tabControl1.TabPages[2];
            try
            {
                textBox1.Text = "***************导线基本信息*****************\r\n";
                textBox1.Text += "测站数: " + Convert.ToString(Cal.Points.Count - 2) + "\r\n";
                textBox1.Text += "---------------------角度数据------------------\r\n";
                while (i < dataGridView1.RowCount)
                {
                    textBox1.Text += dataGridView1.Rows[i].Cells[0].Value + "\t" + dataGridView1.Rows[1].Cells[1].Value + "\t" + dataGridView1.Rows[i].Cells[2].Value+"\r\n";
                    i++;
                }
                textBox1.Text += "---------------------坐标数据------------------\r\n";
                for(int j = 0; j < Cal.Points.Count; j++)
                {
                    textBox1.Text += Cal.Points[j].Cstation + "\t \t" + Math.Round(Cal.Points[j].X,5) + "\t" + Math.Round(Cal.Points[j].Y,5)+ "\r\n";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


        }
#endregion
        #region 坐标改正
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                //方位角闭合差
                double sum = 0;
                for (int i = 0; i < Cal.betas.Count(); i++)
                {
                    sum += count.angle2t(Cal.betas[i].beta);
                }
                double a = count.angle2t(Cal.betas[0].alpha);
                double fwjbhc = -1.0 * count.angle2t(Cal.betas[Cal.betas.Count - 1].alpha) + count.angle2t(Cal.betas[0].alpha) + sum - 180.0 * (Cal.betas.Count() - 1) + 360;
                fwjbhc = count.tentoangle(fwjbhc);

                //限差比较
                double limte1 = 0.0024 * Math.Sqrt(Cal.betas.Count() - 1) - 0.004 * Math.Sqrt(Cal.betas.Count() - 1);
                double limte2 = 0.0024 * Math.Sqrt(Cal.betas.Count() - 1) + 0.004 * Math.Sqrt(Cal.betas.Count() - 1);
                if (limte1 <= fwjbhc && fwjbhc <= limte2)
                {
                    Console.WriteLine("合格");
                }
                else
                {
                    Console.WriteLine("不合格");
                }

                //计算改正后的各转折角
                double vBeta = -fwjbhc / (Cal.betas.Count() - 1);

                double calpha = Cal.betas[0].alpha;
                correction k = new correction();
                k.cAlpha = calpha;
                Cal.cBetas.Add(k);
                calpha = calpha + 360;
                for (int i = 1; i < Cal.betas.Count(); i++)
                {
                    correction kp = new correction();
                    double cbeta = count.toS(Cal.betas[i].beta) + count.toS(vBeta);

                    calpha = count.toS(calpha) + cbeta - count.toS(180);
                    calpha = count.toAngle(calpha);

                    cbeta = count.toAngle(cbeta);
                    kp.cBeta = cbeta;
                    kp.cAlpha = calpha;
                    Cal.cBetas.Add(kp);
                }


                ///XY
                //计算纵横坐标增量
                
                for (int i = 1; i < Cal.betas.Count - 1; i++)
                {
                    dGc kd = new dGc();
                    double dx = Cal.Points[i].S * Math.Cos(count.angle2t(Cal.cBetas[i].cAlpha) / 180 * Math.PI);
                    double dy = Cal.Points[i].S * Math.Sin(count.angle2t(Cal.cBetas[i].cAlpha) / 180 * Math.PI);
                    kd.dX = dx;
                    kd.dY = dy;
                    Cal.dGcs.Add(kd);
                }

                //闭合差计算以及限差检验
                double x = 0;
                double y = 0;
                double s = 0;

                for (int i = 0; i < Cal.dGcs.Count; i++)
                {
                    x += Cal.dGcs[i].dX;
                    y += Cal.dGcs[i].dY;

                }

                for (int i = 1; i < Cal.betas.Count - 1; i++)
                {
                    s += Cal.Points[i].S;
                }
                double fx = Cal.Points[1].X - Cal.Points[Cal.Points.Count - 1].X + x;
                double fy = Cal.Points[1].Y - Cal.Points[Cal.Points.Count - 1].Y + y;
                double fs = Math.Sqrt((fx * fx) + (fy * fy));
                if ((fs / s) <= (1 / 5000))
                {
                    Console.WriteLine("合格");
                }
                else
                {
                    Console.WriteLine("不合格");
                }

                //计算坐标增量的改正数
                for (int i = 1; i < Cal.betas.Count - 1; i++)
                {
                    vXY ka = new vXY();
                    double vx = -fx / s * Cal.Points[i].S;
                    double vy = -fy / s * Cal.Points[i].S;
                    ka.vX = vx;
                    ka.vY = vy;
                    Cal.vXYs.Add(ka);
                }

                //计算坐标
                double X = Cal.Points[1].X;
                double Y = Cal.Points[1].Y;
                for (int i = 0; i < Cal.dGcs.Count - 1; i++)
                {
                    X = X + Cal.dGcs[i].dX + Cal.vXYs[i].vX;
                    Y = Y + Cal.dGcs[i].dY + Cal.vXYs[i].vY;

                    Cal.Points[i + 2].X = X;
                    Cal.Points[i + 2].Y = Y;

                }
                int n = 0;
                for (int i = 0; n < Cal.Points.Count; i += 2)
                {
                    dataGridView1.Rows[i].Cells[3].Value = Cal.Points[n].X;
                    dataGridView1.Rows[i].Cells[4].Value = Cal.Points[n].Y;
                    n++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


      

        }
    }
    #endregion
}
