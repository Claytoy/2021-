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
        Count CC = new Count();
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
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = CC.points[i].Name;
                        dataGridView1.Rows[i].Cells[1].Value = CC.points[i].X;
                        dataGridView1.Rows[i].Cells[2].Value = CC.points[i].Y;
                        dataGridView1.Rows[i].Cells[3].Value = CC.points[i].R;
                        dataGridView1.Rows[i].Cells[4].Value = CC.points[i].Ls;

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
                CC.crmath();
                CC.huanhequxian();
                for (int i = 0; i < CC.points.Count; i++)
                {
                    dataGridView2.Rows.Add();
                    dataGridView2.Rows[i].Cells[0].Value = "X1";
                    dataGridView2.Rows[i].Cells[1].Value = CC.points[i].X;
          
                }
                this.Alert("缓和曲线计算成功", Form_Alert.enmType.Success);
            }
            catch
            {
                this.Alert("缓和曲线计算失败", Form_Alert.enmType.Error);
            }
            this.Focus();
        }
        private void 横断面计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CC.lichengzhuang();
                //for (int i = 0; i < test.cal.H_points.Count; i++)
                //{
                //    //dataGridView2.Rows.Add();
                //    //dataGridView2.Rows[Curcount].Cells[0].Value = "H2";
                //    //dataGridView2.Rows[Curcount].Cells[1].Value = test.cal.H_points[i].Z;
                //    //Curcount++;
                //}
                dataGridView2.Columns.Add("colums3", "变量2");
                dataGridView2.Columns.Add("colums4", "变量2值");
                for (int i = 0; i < CC.points.Count; i++)
                {
                    
                    dataGridView2.Rows[i].Cells[2].Value = "y1";
                    dataGridView2.Rows[i].Cells[3].Value = CC.points[i].Y;
                    
                }
                this.Alert("里程桩计算成功", Form_Alert.enmType.Success);
            }
            catch
            {
                this.Alert("里程桩计算失败", Form_Alert.enmType.Error);
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
                chart1.ChartAreas[0].AxisX.Maximum = xmax + 20;
                chart1.ChartAreas[0].AxisX.Minimum = xmin - 20;
                chart1.ChartAreas[0].AxisY.Maximum = ymax + 20;
                chart1.ChartAreas[0].AxisY.Minimum = ymin - 20;

                chart1.Series.Add("曲线点");
                chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                chart1.Series[0].Name = "道路";
                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                for (int i = 0; i < CC.points.Count; i++)
                {
                    chart1.Series[0].Points.AddXY(CC.points[i].X, CC.points[i].Y);
                }
                for (int i = 0; i < CC.Dian.Count; i++)
                {
                    chart1.Series[1].Points.AddXY(CC.Dian[i].X, CC.Dian[i].Y);
                }

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

                richTextBox1.Text += "\n道路曲线要素计算与里程桩计算\n";
                richTextBox1.Text += "\n圆曲线的要素计算成果\n";
                richTextBox1.Text += "\n----------------------------------------\n";

                richTextBox1.Text += "\n里程点计算成果\n";
                richTextBox1.Text += "\n----------------------------------------\n";
                for (int i = 0; i < CC.Dian.Count; i++)
                {
                    str += CC.Dian[i].Name + "\t" + CC.Dian[i].X.ToString("f5") + "\t" + CC.Dian[i].Y.ToString("f5") + "\n";
                }

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
                toolStripButton9.Image = ResourcePic.ImageLabel_ON1;
                详细信息ToolStripMenuItem1.Checked = true;
            }
            else if (详细信息ToolStripMenuItem1.Checked == true)
            {
                chart1.Series[0].Label = "";
                chart1.Series[1].Label = "";
                toolStripButton9.Image = ResourcePic.ImageLabel_ON;
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
                toolStripButton10.Image = ResourcePic.ImageMouse_ON1;
                拖拽缩放ToolStripMenuItem.Checked = true;
            }
            else if (拖拽缩放ToolStripMenuItem.Checked == true)
            {
                chart1.ChartAreas[0].CursorX.IsUserEnabled = false;
                chart1.ChartAreas[0].CursorY.IsUserEnabled = false;
                chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = false;
                chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = false;
                toolStripButton10.Image = ResourcePic.ImageMouse_ON;
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
            纵断面计算ToolStripMenuItem_Click(sender, e);
            横断面计算ToolStripMenuItem_Click(sender, e);
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
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
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

            //    saveFileDialog1.Title = "保存正确性文件";
            //    saveFileDialog1.Filter = "文本文档(*.txt)| *.txt";
            //    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //    {
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
            }
            catch { MessageBox.Show("导线测量"); }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FormCode fc = new FormCode();
            fc.ShowDialog();
        }
        string str1 = "";
        private void 纵断面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Title = "111";
                saveFileDialog1.Filter = "文本文档(*.txt)|*.txt";
                if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < CC.points.Count; i++)
                    {
                        str1 += CC.points[i].Ls + "\t" + CC.points[i].R + "\r\n";

                    }
                    //StreamWriter sww = new StreamWriter(saveFileDialog1.FileName);
                    using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName))
                        writer.Write(str1);
              
                    //sww.Write(this.str1);
                }
               
            }
            catch { };

        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            详细信息ToolStripMenuItem1_Click(sender, e);
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            拖拽缩放ToolStripMenuItem_Click(sender, e);
            {
            }
        }
        #endregion
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            纵断面计算ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            横断面计算ToolStripMenuItem_Click(sender, e);
        }

        private void 横断面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Title = "111";
                saveFileDialog1.Filter = "文本文档(*.txt)|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < CC.points.Count; i++)
                    {
                        str1 += CC.points[i].Ls + "\t" + CC.points[i].R + "\r\n";

                    }
                    //StreamWriter sww = new StreamWriter(saveFileDialog1.FileName);
                    using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName))
                        writer.Write(str1);

                    //sww.Write(this.str1);
                }

            }
            catch { };
        }

        private void 保存报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            保存报告ToolStripMenuItem1_Click(sender, e);
        }
    }
}
