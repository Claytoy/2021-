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
                    Cal.GetData(filepath);
                    for (int i = 0; i < Cal.Points.Count; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = Cal.Points[i].Start;
                        dataGridView1.Rows[i].Cells[1].Value = Cal.Points[i].End;
                        dataGridView1.Rows[i].Cells[2].Value = Cal.Points[i].Hsjl1;
                        dataGridView1.Rows[i].Cells[3].Value = Cal.Points[i].Hszsds1;
                        dataGridView1.Rows[i].Cells[4].Value = Cal.Points[i].Qsjl1;
                        dataGridView1.Rows[i].Cells[5].Value = Cal.Points[i].Qszsds1;
                        dataGridView1.Rows[i].Cells[6].Value = Cal.Points[i].Qsjl2;
                        dataGridView1.Rows[i].Cells[7].Value = Cal.Points[i].Qszsds2;
                        dataGridView1.Rows[i].Cells[8].Value = Cal.Points[i].Hsjl2;
                        dataGridView1.Rows[i].Cells[9].Value = Cal.Points[i].Hszsds2;
                    }
                }
                toolStripStatusLabel2.Text = "数据导入完成";
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
                Cal.Leveling();
                
                for(int i = 0; i < Cal.Points.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[10].Value = Cal.Points[i].Gc;
                }
                toolStripStatusLabel2.Text = "Calculate Done";
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
            textBox1.Text = "已知点数据--------------------------------------------------------------------------------------------------\r\n";
            textBox1.Text += "测站数：" + Cal.Points.Count+"\r\n";
            textBox1.Text += "总距离: " + Cal.R.S.Sum();
            textBox1.Text += "后视点         前视点         后距1(m)      前距1(m)      前距2(m)      后距2(m)      后尺中丝1(m)    前尺中丝1(m)    前尺中丝2(m)    后尺中丝2(m)    \r\n";
            for(int i = 0; i < Cal.Points.Count; i++)
            {
                textBox1.Text += Cal.Points[i].Start + "\t" + Cal.Points[i].End + "\t" + Cal.Points[i].Hsjl1 + "\t" + Cal.Points[i].Hszsds1 + "\t" +
                    Cal.Points[i].Qsjl1 + "\t" + Cal.Points[i].Qszsds1 + "\t" + Cal.Points[i].Qsjl2 + "\t" + Cal.Points[i].Qszsds2 + "\t" + Cal.Points[i].Hsjl2 + "\t" + Cal.Points[i].Hszsds2+ "\r\n";
            }

        }
        #endregion
        #region Menu 绘制
        private void 绘制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[1];
            try
            {
                for(int i = 0; i < Cal.R.S.Count;i++)
                {
                    Cal.R.Lis.Add(0);
                    if(i>0)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            Cal.R.Lis[i] += Cal.R.S[j];
                        }
                        Cal.R.Lis[i] += Cal.R.S[0];
                    }
                    else if(i==0)
                    {
                        Cal.R.Lis[i] = Cal.R.S[i];
                    }
                  
                }
                Pen p = new Pen(Color.Black,2.5f);
                int Ys = (int)(Cal.R.HIS.Max() - Cal.R.HIS.Min())*1000 + 200;
                int Xs = (int)(Cal.R.Lis.Max() - Cal.R.Lis.Min()+200);
                image = new Bitmap(Xs, Ys);//显示图形范围
                Graphics g = Graphics.FromImage(image);
                PointF[] pf = new PointF[Cal.Points.Count];
                for(int i =0; i < Cal.R.S.Count; i++)
                {
                    pf[i].X = (float)(Cal.R.Lis[i] - Cal.R.Lis.Min()+100);
                    pf[i].Y = -(float)(Cal.R.HIS[i]-Cal.R.HIS.Max())*1000 - 100;
                }
                g.DrawLines(p, pf);
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
