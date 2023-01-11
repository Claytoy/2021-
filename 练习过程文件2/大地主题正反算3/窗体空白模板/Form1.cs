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
            toolStripStatusLabel2.Text = "大地正反算";

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
        #region Menu打开（废弃）
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
         }
        #endregion
        #region Menu打开正算数据
        private void 正算数据ToolStripMenuItem_Click(object sender, EventArgs e)
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
                    Cal.GetZSData(filepath);
                    int n = Cal.ZSPoints.Count();
                    for(int i = 0; i < n; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = Cal.ZSPoints[i].Start;
                        dataGridView1.Rows[i].Cells[1].Value = Cal.ZSPoints[i].Latitude;
                        dataGridView1.Rows[i].Cells[2].Value = Cal.ZSPoints[i].Longitude;
                        dataGridView1.Rows[i].Cells[3].Value = Cal.ZSPoints[i].Azimuth;
                        dataGridView1.Rows[i].Cells[4].Value = Cal.ZSPoints[i].Logw;
                        dataGridView1.Rows[i].Cells[5].Value = Cal.ZSPoints[i].End;
                    }
                }
                toolStripStatusLabel2.Text = "正算数据导入完成";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        #endregion
        #region Menu反算数据打开
        private void 反算数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            while (dataGridView1.Rows.Count > 1)
            {
                dataGridView1.Rows.RemoveAt(0);
            }
            pictureBox1.Image = null;
            try
            {
                openFileDialog1.Title = "文件打开";
                openFileDialog1.Filter = "文本文档(*.txt)|*.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filepath = openFileDialog1.FileName;
                    Cal.GetFSData(filepath);
                    for(int i = 0; i < Cal.FSPoints.Count; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = Cal.FSPoints[i].Start;
                        dataGridView1.Rows[i].Cells[1].Value = Cal.FSPoints[i].Latitude1;
                        dataGridView1.Rows[i].Cells[2].Value = Cal.FSPoints[i].Longitude1;
                        dataGridView1.Rows[i].Cells[5].Value = Cal.FSPoints[i].End;
                        dataGridView1.Rows[i].Cells[6].Value = Cal.FSPoints[i].Latitude2;
                        dataGridView1.Rows[i].Cells[7].Value = Cal.FSPoints[i].Longitude2;
                    }
                    
                }
                toolStripStatusLabel2.Text = "反算数据导入完成";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        #endregion

        #region Menu正算
        private void 计算1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[0];
            try
            {
                Cal.ZSCal();
                for(int i = 0; i < dataGridView1.Rows.Count-1; i++)
                {
                    dataGridView1.Rows[i].Cells[6].Value = Cal.R.B2[i];
                    dataGridView1.Rows[i].Cells[7].Value = Cal.R.L2[i];
                    dataGridView1.Rows[i].Cells[8].Value = Cal.R.A2[i];
                }
                toolStripStatusLabel2.Text = "正算完成";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region Menu反算
        private void 计算2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[0];
            try
            {
                Cal.FSCal();
                for(int i = 0; i < dataGridView1.Rows.Count-1;i++)
                {
                    dataGridView1.Rows[i].Cells[4].Value = Cal.R.S[i];
                    dataGridView1.Rows[i].Cells[3].Value = Cal.R.A1[i];
                }
                toolStripStatusLabel2.Text = "反算完成";
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
            textBox1.Text = "**************************************************\r\n";
            textBox1.Text += "                  大地主题正算\r\n";
            textBox1.Text += "**************************************************\r\n";
            textBox1.Text += "计算点对总数  ：" + Cal.ZSPoints.Count + "\r\n";
            textBox1.Text += "椭球长半轴：  " + Cal.a + "\r\n";
            textBox1.Text += "椭球扁率：  " + Cal.f + "\r\n";
            textBox1.Text += "----------------------计算结果---------------------\r\n";
            textBox1.Text += "点名\t纬度(B)  经度(L)  大地方位角(A) 大地线长(S)\r\n";
            for(int i = 0; i < Cal.ZSPoints.Count;i++)
            {
                textBox1.Text += Cal.ZSPoints[i].Start + '\t' + Cal.ZSPoints[i].Latitude + '\t' + Cal.ZSPoints[i].Longitude + '\t' + Cal.ZSPoints[i].Azimuth + '\t' + Cal.ZSPoints[i].Logw + "\r\n";
            }
          
        }
        #endregion
        #region Menu 绘制
        private void 绘制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[1];
            try
            {
                double xmax=0, xmin= Cal.ZSPoints[0].Latitude, ymax=0, ymin=Cal.ZSPoints[0].Longitude;
               for(int i = 0; i < Cal.ZSPoints.Count;i++)
                {
                    double x = Cal.ZSPoints[i].Latitude;
                    double y = Cal.ZSPoints[i].Longitude;
                    if (x > xmax)
                        xmax = x;
                    else if (y > ymax)
                        ymax = y;

                    if (x < xmin)
                        xmin = x;
                    else if (y < ymin)
                        ymin = y;
                }
                Pen p = new Pen(Color.Black, 10f);//定义画笔
                SolidBrush brush = new SolidBrush(Color.Red);//定义填充
                int width = (int)(xmax - xmin + 400);
                int height = (int)(ymax - ymin + 400);
                //height = pictureBox1.Height;
                //width = pictureBox1.Width;
                image = new Bitmap(width , height);
                Graphics g = Graphics.FromImage(image);
      
                //g.DrawEllipse(p, 90, 30, 10, 10);
                //g.DrawEllipse(p, 80, 25, 10, 10);

                //g.DrawEllipse(p, 30, 90, 10, 10);
                //g.DrawEllipse(p, 25, 80, 10, 10);
                for (int i = 0; i < Cal.ZSPoints.Count; i++)
                {
                   
                }


                pictureBox1.Image = (Image)image;




                    toolStripStatusLabel2.Text = "绘制完成";
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
