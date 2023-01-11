using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZHDM
{
    public partial class FormMain : Form
    {
        #region 窗体全局变量
        count CC = new count();
        int Curcount = 0;
        #endregion
        #region 窗体函数
        public void Alert(string msg, Form_Alert.enmType type)
        {
            Form_Alert frm = new Form_Alert();
            frm.showAlert(msg, type);
        }
        void FormInit()
        {
            panelData.Visible = false;
            panelChart.Visible = false;
            panelReport.Visible = false;
            panelHome.Visible = false;
            panelResult.Visible = false;
        }
        void ButtonInit()
        {
            button1.BackColor = Color.FromArgb(45, 45, 45);
            button2.BackColor = Color.FromArgb(45, 45, 45);
            button3.BackColor = Color.FromArgb(45, 45, 45);
            button4.BackColor = Color.FromArgb(45, 45, 45);
        }
        public FormMain()
        {

            InitializeComponent();
            panelData.Dock = DockStyle.Fill;
            panelChart.Dock = DockStyle.Fill;
            panelReport.Dock = DockStyle.Fill;
            panelHome.Dock = DockStyle.Fill;
            panelResult.Dock = DockStyle.Fill;
            FormInit();
            panelHome.Visible = true;
        }
        #endregion
        #region 界面切换
        private void button1_Click(object sender, EventArgs e)
        {
            FormInit();
            panelData.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormInit();
            panelChart.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormInit();
            panelReport.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //currect?
            FormInit();
            panelResult.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //help
            帮助ToolStripMenuItem_Click(sender, e);
        }

        #endregion
        #region Menu功能实现
        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            button1_Click(sender, e);
            try
            {

                openFileDialog1.Title = "导入数据";
                openFileDialog1.Filter = "文本文件(*.txt)| *.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    CC.dataIn(openFileDialog1.FileName);
                    for (int i = 0; i < CC.points.Count; i++)
                    {
                        textBox1.Text = Convert.ToString(CC.points[i].x1);
                        textBox2.Text = Convert.ToString(CC.points[i].y1);
                        textBox3.Text = Convert.ToString(CC.points[i].x2);
                        textBox4.Text = Convert.ToString(CC.points[i].y2);
                    }
                    this.Alert("导入数据成功", Form_Alert.enmType.Success);
                    //using (StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default))
                    //{
                    //    string[] str = sr.ReadLine().Split(',');
                    //    str = sr.ReadLine().Split(',');
                    //    sr.ReadLine();
                    //    txt_dianming.Text = string.Join(",", str);
                    //    int i = 0;
                    //    while (!sr.EndOfStream)
                    //    {
                    //        str = sr.ReadLine().Split(',');
                    //        dataGridView1.Rows.Add();
                    //        for (int j = 0; j < str.Length; j++)
                    //        {
                    //            dataGridView1.Rows[i].Cells[j].Value = str[j];
                    //        }
                    //        i++;
                    //    }
                    //}
                }
            }
            catch { this.Alert("导入数据失败", Form_Alert.enmType.Error); }
            this.Focus();
        }
        private void 纵断面计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CC.fuzhu();
                textBox5.Text = Convert.ToString(CC.points[0].u1);
                textBox6.Text = Convert.ToString(CC.points[0].v1);
                textBox7.Text = Convert.ToString(CC.points[0].w1);
                textBox8.Text = Convert.ToString(CC.points[0].y1);
                textBox9.Text = Convert.ToString(CC.points[0].u2);
                textBox10.Text = Convert.ToString(CC.points[0].v2);
                textBox11.Text = Convert.ToString(CC.points[0].w2);
                textBox12.Text = Convert.ToString(CC.points[0].y2);
                this.Alert("辅助坐标计算成功", Form_Alert.enmType.Success);
            }
            catch
            {
                this.Alert("辅助坐标计算失败", Form_Alert.enmType.Error);
            }
            this.Focus();
        }
        private void 横断面计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CC.touying();
                textBox16.Text = Convert.ToString(CC.points[0].N1);
                textBox15.Text = Convert.ToString(CC.points[0].N2);

                this.Alert("投影系数计算成功", Form_Alert.enmType.Success);
            }
            catch
            {
                this.Alert("投影系数计算失败", Form_Alert.enmType.Error);
            }
            this.Focus();
        }
        private void 最终坐标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CC.dimianzuobiao();
                textBox18.Text = Convert.ToString(CC.points[0].X);
                textBox17.Text = Convert.ToString(CC.points[0].Y);
                textBox14.Text = Convert.ToString(CC.points[0].Z);
                this.Alert("坐标计算成功", Form_Alert.enmType.Success);
            }
            catch
            {
                this.Alert("坐标计算失败", Form_Alert.enmType.Error);
            }
            this.Focus();
        }
        private void 绘制图形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
            try
            {
                double xmax = CC.points[0].X;
                double xmin = CC.points[0].X;
                double ymax = CC.points[0].Y;
                double ymin = CC.points[0].Y;
                for (int i = 0; i < CC.points.Count; i++)
                {
                    xmax = xmax > CC.points[i].X ? xmax : CC.points[i].X;
                    xmin = xmin < CC.points[i].X ? xmin : CC.points[i].X;
                    ymax = ymax > CC.points[i].Y ? ymax : CC.points[i].Y;
                    ymin = ymin < CC.points[i].Y ? ymin : CC.points[i].Y;
                }
                //chart1.ChartAreas[0].AxisX.Maximum = xmax + 20;
                //chart1.ChartAreas[0].AxisX.Minimum = xmin - 20;
                //chart1.ChartAreas[0].AxisY.Maximum = ymax + 20;
                //chart1.ChartAreas[0].AxisY.Minimum = ymin - 20;
                //chart1.ChartAreas[0].Area3DStyle.Enable3D = true;
                //System.Windows.Forms.DataVisualization.Charting.Point3D p3D = new System.Windows.Forms.DataVisualization.Charting.Point3D();
                //chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                //chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                chart1.Series[0].Points.AddXY(CC.points[0].X,CC.points[0].Y);
                chart1.Series[0].Points.AddXY(CC.points[0].x1, CC.points[0].y1);
                chart1.Series[0].Points[0].Label = "P";
                chart1.Series[0].Points[1].Label = "a";
                chart1.Series.Add("线形3");
                chart1.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series[2].Points.AddXY(CC.points[0].X, CC.points[0].Y);
                chart1.Series[2].Points.AddXY(CC.points[0].x2, CC.points[0].y2);
                chart1.Series[2].Points[1].Label = "b";
                //chart1.Series[0].Points.AddXY(p3D);

                chart1.Series[1].Points.AddXY(xmax / 2-200, ymax / 2 -200);
                chart1.Series[1].Points.AddXY(xmax / 2 + 400, ymax / 2 + 400);
                chart1.Series[1].Points[0].Label = "投影面";
                //chart1.Series[1].Points.AddXY(CC.points[0].X, CC.points[0].Y);
                //chart1.Series[1].Points.AddXY(CC.points[0].x2, CC.points[0].y2);


                this.Alert("图形生成成功", Form_Alert.enmType.Success);
            }
            catch
            {
                this.Alert("图形生成失败", Form_Alert.enmType.Error);
            }
            this.Focus();
        }

        private void 生成报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
            
            try
            {
                string str = "";
                str += "--------------------前方交会------------------\r\n";
                str += CC.points[0].u1 + "\t" + CC.points[0].v1 + "\t" + CC.points[0].w1 + "\t" + CC.points[0].y1 + "\t" + CC.points[0].u2 + "\t" + CC.points[0].v2 + "\t" + CC.points[0].w2 + "\t" + CC.points[0].y2 + "\r\n";


                richTextBox1.Text += str;

                this.Alert("报告生成成功", Form_Alert.enmType.Success);
            }
            catch
            {
                this.Alert("报告生成失败", Form_Alert.enmType.Error);
            }
            this.Focus();
        }
        private int n_x = 100;
        private int n_y = 100;
        private void 放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (n_x > 10)
                {
                    n_x--;
                    n_y--;
                }

                chart1.ChartAreas[0].AxisX.ScaleView.Size = n_x;
                chart1.ChartAreas[0].AxisY.ScaleView.Size = n_y;
                chart1.ChartAreas[0].AxisX.ScaleView.Position = (chart1.ChartAreas[0].AxisX.Maximum + chart1.ChartAreas[0].AxisX.Minimum) / 2;
                chart1.ChartAreas[0].AxisY.ScaleView.Position = (chart1.ChartAreas[0].AxisY.Maximum + chart1.ChartAreas[0].AxisY.Minimum) / 2;
            }
            catch
            {
                this.Alert("请确认图形是否生成", Form_Alert.enmType.Error);
            }
            this.Focus();
        }
        private void 缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (n_x < 100)
                {
                    n_x++;
                    n_y++;
                }

                chart1.ChartAreas[0].AxisX.ScaleView.Size = n_x;
                chart1.ChartAreas[0].AxisY.ScaleView.Size = n_y;
                chart1.ChartAreas[0].AxisX.ScaleView.Position = (chart1.ChartAreas[0].AxisX.Maximum + chart1.ChartAreas[0].AxisX.Minimum) / 2;
                chart1.ChartAreas[0].AxisY.ScaleView.Position = (chart1.ChartAreas[0].AxisY.Maximum + chart1.ChartAreas[0].AxisY.Minimum) / 2;
            }
            catch
            {
                this.Alert("请确认图形是否生成", Form_Alert.enmType.Error);
            }
            this.Focus();
        }
        private void 视图复位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
            chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset(0);
        }
        private void 详细信息ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (详细信息ToolStripMenuItem1.Checked == false)
            {
                chart1.Series[0].Label = "#VAL";
                chart1.Series[1].Label = "#VAL";
                chart1.Series[2].Label = "#VAL";
                toolStripComboBox1.Image = ResourcePic.ImageLabel_OFF;

                详细信息ToolStripMenuItem1.Checked = true;
            }
            else if (详细信息ToolStripMenuItem1.Checked == true)
            {
                chart1.Series[0].Label = "";
                chart1.Series[1].Label = "";
                chart1.Series[2].Label = "";
                toolStripComboBox1.Image = ResourcePic.ImageLabel_ON;

                详细信息ToolStripMenuItem1.Checked = false;
            }
            this.Focus();
        }
        private void 拖拽缩放ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (拖拽缩放ToolStripMenuItem.Checked == false)
            {
                chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
                chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
                chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
                toolStripButton9.Image = ResourcePic.ImageMouse_OFF;
                拖拽缩放ToolStripMenuItem.Checked = true;
            }
            else if (拖拽缩放ToolStripMenuItem.Checked == true)
            {
                chart1.ChartAreas[0].CursorX.IsUserEnabled = false;
                chart1.ChartAreas[0].CursorY.IsUserEnabled = false;
                chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = false;
                chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = false;
                toolStripButton9.Image = ResourcePic.ImageMouse_ON;
                拖拽缩放ToolStripMenuItem.Checked = false;
            }
            this.Focus();
        }
        #endregion
        #region Tool功能实现
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            导入数据ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            button8_Click_1(sender, e);
            纵断面计算ToolStripMenuItem_Click(sender, e);
            横断面计算ToolStripMenuItem_Click(sender, e);
            最终坐标ToolStripMenuItem_Click(sender, e);
            绘制图形ToolStripMenuItem_Click(sender, e);
            生成报告ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            绘制图形ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            放大ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            缩小ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            视图复位ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            生成报告ToolStripMenuItem_Click(sender, e);
        }
        #endregion

        #region save and help
        private void 保存报告ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (richTextBox1.Text != "")
                {
                    saveFileDialog1.Title = "保存报告";
                    saveFileDialog1.Filter = "文本文档(*.txt)| *.txt";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamWriter Writer = new StreamWriter(saveFileDialog1.FileName))
                            Writer.Write(this.richTextBox1.Text);
                    }
                }

            }
            catch { }
           
        }

        private void 保存图形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Title = "图形保存";
                saveFileDialog1.Filter = "图形文件(*.jpg)| *.jpg";
                if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    chart1.SaveImage(saveFileDialog1.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Jpeg);
                }

            }
            catch { }
        }

        private void 保存正确性文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{

            //        saveFileDialog1.Title = "保存正确性文件";
            //        saveFileDialog1.Filter = "文本文档(*.txt)| *.txt";
            //        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //        {
            //        string strs = "";
            //        using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
            //        {
            //            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            //            {
            //                strs += Convert.ToString(this.dataGridView2.Rows[i].Cells[0].Value) + "\t" + this.dataGridView2.Rows[i].Cells[1].Value + "\r\n";
            //            }
            //            sw.Write(strs);
            //        }
            //    }

            //}
            //catch { }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            try
            {
                if (panelChart.Visible == true)
                {
                    保存图形ToolStripMenuItem_Click(sender, e);
                }
                else if (panelResult.Visible == true)
                {
                    保存正确性文件ToolStripMenuItem_Click(sender, e);
                }
                else
                {
                    保存报告ToolStripMenuItem1_Click(sender, e);
                }
            }
            catch { };
           
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo("Helper.doc");
            p.StartInfo = psi;
            try
            {
                p.Start();
            }catch { MessageBox.Show("e"); }
        }

        private void button6_Click(object sender, EventArgs e)
        {
           FormCode fc =  new FormCode();
            fc.ShowDialog();
        }

        private void 辅助坐标系ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string str = "";
            richTextBox1.Text = "";
            try
            {
                richTextBox1.Text += "--------------------前方交会------------------\r\n";
                richTextBox1.Text += CC.points[0].u1 + "\t" + CC.points[0].v1 + "\t" + CC.points[0].w1 + "\t" + CC.points[0].y1 + "\t" + CC.points[0].u2 + "\t" + CC.points[0].v2 + "\t" + CC.points[0].w2 + "\t" + CC.points[0].y2 + "\r\n";

                saveFileDialog1.ShowDialog();
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                    sw.Write(richTextBox1.Text);
                
              
            }
            catch { }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            纵断面计算ToolStripMenuItem_Click(sender, e);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            横断面计算ToolStripMenuItem_Click(sender, e);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            最终坐标ToolStripMenuItem_Click(sender, e);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox25.Text = CC.Xs.ToString();
            textBox24.Text = CC.Ys.ToString();
            textBox23.Text = CC.Zs.ToString();
            textBox22.Text = CC.o.ToString();
            textBox21.Text = CC.p.ToString();
            textBox20.Text = CC.q.ToString();
            textBox19.Text = CC.f.ToString();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            textBox25.Text = CC.Xs.ToString();
            textBox24.Text = CC.Ys.ToString();
            textBox23.Text = CC.Zs.ToString();
            textBox22.Text = CC.o.ToString();
            textBox21.Text = CC.p.ToString();
            textBox20.Text = CC.q.ToString();
            textBox19.Text = CC.f.ToString();
        }

        private void 投影系数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = "";
            try
            {
                saveFileDialog1.Filter = "文本文档(*.txt)|*.txt";
                saveFileDialog1.ShowDialog();
                str += "投影系数中间量 \r\n";
                for(int i =0; i<CC.points.Count; i++)
                {
                    str += CC.points[i].u1 + "\t" + CC.points[i].u2 + "\t" + CC.points[i].v1;
                }
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                    sw.Write(str);
            }
            catch { }
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            详细信息ToolStripMenuItem1_Click(sender, e);
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            拖拽缩放ToolStripMenuItem_Click(sender, e);
        }
    }
    #endregion
}
