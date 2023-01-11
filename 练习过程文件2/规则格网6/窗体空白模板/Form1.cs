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

                    for(int i = 0; i < Cal.Points.Count; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = Cal.Points[i].Id;
                        dataGridView1.Rows[i].Cells[1].Value = Cal.Points[i].X;
                        dataGridView1.Rows[i].Cells[2].Value = Cal.Points[i].Y;
                        dataGridView1.Rows[i].Cells[3].Value = Cal.Points[i].H;
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
                Cal.TB();
                Cal.Rect(5);
                
               
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
            tabControl1.SelectTab(tabControl1.TabPages[2]);
            textBox1.Text = "-----------------Grid-------------------\r\n";
            for(int i = 0; i < Cal.Points.Count; i++)
            {
                textBox1.Text += Cal.Points[i].Id + "\t" + Cal.Points[i].X + "\t" + Cal.Points[i].Y + "\t" + Cal.Points[i].H + "\r\n";
            }
            textBox1.Text += "-----------------TB-------------------\r\n";
            for (int i = 0; i < Cal.S.Count; i++)
            {
                textBox1.Text += Cal.S[i].Id + "\t" + Cal.S[i].X + "\t" + Cal.S[i].Y + "\t" + Cal.S[i].H + "\r\n";
            }
        }
        #endregion
        #region Menu 绘制
        private void 绘制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[1];
            try
            {
                Pen p = new Pen(Color.Black, 2.5f);
                Pen p2 = new Pen(Color.Blue, 2);
                SolidBrush brush = new SolidBrush(Color.Black);//定义填充
                int width = (int)((Cal.YMax - Cal.YMin) * 30 + 200);//图幅长
                int heigth = (int)((Cal.XMax - Cal.XMin) * 30 + 200);//图幅宽
                image = new Bitmap(width, heigth);//显示图形范围
                Graphics g = Graphics.FromImage(image);
                PointF[] pf1 = new PointF[Cal.S.Count];
                for (int i = 0; i < Cal.S.Count; i++)
                {
                    pf1[i].Y = (float)((Cal.S[i].Y - Cal.YMin) * 10 + 10);
                    pf1[i].X = -(float)((Cal.S[i].X - Cal.XMax) * 10 - 100);
                }
             

                PointF[] pf = new PointF[Cal.S.Count];
                for (int i = 0; i < Cal.S.Count; i++)
                {
                    g.DrawEllipse(p, (int)((Cal.S[i].Y - Cal.YMin + 10) * 10 - 6), -(int)((Cal.S[i].X - Cal.XMax - 100) * 10 - 6), 12, 12);
                    g.FillEllipse(new SolidBrush(Color.Black), (int)((Cal.S[i].Y - Cal.YMin + 10) * 10 - 6), -(int)((Cal.S[i].X - Cal.XMax - 100) * 10 - 6), 12, 12);
                   
                }
                g.FillPolygon(new SolidBrush(Color.Yellow), pf1);
                //g.FillPolygon(new SolidBrush(Color.Black), pf);
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
