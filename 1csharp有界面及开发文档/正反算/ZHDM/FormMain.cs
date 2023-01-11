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
        bool cal_tag = false;
        bool cal_tag2 = false;
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
            //dataGridView1.Rows.Clear();
         
            //cal_tag = false;
            //cal_tag2 = false;
            //button1_Click(sender, e);
            //try
            //{

            //    //openFileDialog1.Title = "打开文件";
            //    //openFileDialog1.Filter = "文本文件|*.txt";
            //    //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //    //{
            //    //    dataGridView1.Rows.Clear();
            //    //    CC.dataIn(openFileDialog1.FileName);
            //    //    int count1 = 0;
            //    //    for (int i = 0; i < CC.points.Count(); i++)
            //    //    {
            //    //        dataGridView1.Rows.Add();
            //    //        dataGridView1.Rows[count1].Cells[0].Value = CC.points[i].C_name;
            //    //        dataGridView1.Rows[count1].Cells[6].Value = CC.points[i].X;
            //    //        dataGridView1.Rows[count1].Cells[7].Value = CC.points[i].Y;
            //    //        dataGridView1.Rows.Add();
            //    //        count1++;
            //    //        dataGridView1.Rows[count1].Cells[2].Value = CC.points[i].H_angle;
            //    //        dataGridView1.Rows[count1].Cells[3].Value = CC.points[i].Q_longth;
            //    //        dataGridView1.Rows[count1].Cells[4].Value = CC.points[i].D_X;
            //    //        dataGridView1.Rows[count1].Cells[5].Value = CC.points[i].D_Y;
            //    //        count1++;
            //    //    }

            //    //}
            //    this.Alert("导入数据成功", Form_Alert.enmType.Success);
            //        //using (StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default))
            //        //{
            //        //    string[] str = sr.ReadLine().Split(',');
            //        //    str = sr.ReadLine().Split(',');
            //        //    sr.ReadLine();
            //        //    txt_dianming.Text = string.Join(",", str);
            //        //    int i = 0;
            //        //    while (!sr.EndOfStream)
            //        //    {
            //        //        str = sr.ReadLine().Split(',');
            //        //        dataGridView1.Rows.Add();
            //        //        for (int j = 0; j < str.Length; j++)
            //        //        {
            //        //            dataGridView1.Rows[i].Cells[j].Value = str[j];
            //        //        }
            //        //        i++;
            //        //    }
            //        //}
            //}
            //catch { this.Alert("导入数据失败", Form_Alert.enmType.Error); }
            //this.Focus();
        }
        private void 纵断面计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (cal_tag2 == false)
                {
                    this.Alert("运算程式与导入数据不符", Form_Alert.enmType.Error);
                }
                else
                {
                    CC.basic();
                    CC.ZS();
                    for (int i = 0; i < CC.Z_points.Count(); i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = CC.Z_points[i].Start;
                        dataGridView1.Rows[i].Cells[1].Value = CC.Z_points[i].Weidu;
                        dataGridView1.Rows[i].Cells[2].Value = CC.Z_points[i].Jingdu;
                        dataGridView1.Rows[i].Cells[3].Value = CC.Z_points[i].Angle;

                        dataGridView1.Rows[i].Cells[4].Value = CC.Z_points[i].Longth;
                        dataGridView1.Rows[i].Cells[5].Value = CC.Z_points[i].End;
                        dataGridView1.Rows[i].Cells[6].Value = CC.Z_points[i].Weidu2;
                        dataGridView1.Rows[i].Cells[7].Value = CC.Z_points[i].Jingdu2;
                        dataGridView1.Rows[i].Cells[8].Value = CC.Z_points[i].Angle2;

                    }
                    this.Alert("反计算成功", Form_Alert.enmType.Success);
                }
                
            }
            catch
            {
                this.Alert("计算失败", Form_Alert.enmType.Error);
            }
            this.Focus();
        }
        int index = 0;
        private void 横断面计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (cal_tag2)
                {
                    this.Alert("数据不符", Form_Alert.enmType.Error);
                }
                else
                {
                    dataGridView1.Rows.Clear();
                    CC.basic();
                    CC.FS();
                    for (int i = 0; i < CC.F_points.Count(); i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = CC.F_points[i].Start;
                        dataGridView1.Rows[i].Cells[1].Value = CC.F_points[i].Weidu1;
                        dataGridView1.Rows[i].Cells[2].Value = CC.F_points[i].Jingdu1;
                        dataGridView1.Rows[i].Cells[3].Value = CC.F_points[i].Angle1;
                        dataGridView1.Rows[i].Cells[4].Value = CC.F_points[i].End;
                        dataGridView1.Rows[i].Cells[5].Value = CC.F_points[i].Weidu2;
                        dataGridView1.Rows[i].Cells[6].Value = CC.F_points[i].Jingdu2;
                        dataGridView1.Rows[i].Cells[7].Value = CC.F_points[i].Angle2;

                    }
                    this.Alert("正计算成功", Form_Alert.enmType.Success);
                   
                    
                }

            }
            catch
            {
                this.Alert("计算失败", Form_Alert.enmType.Error);
            }
            this.Focus();
        }
        private void 绘制图形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);

            try
            {

                chart1.Series[0].Points.Clear();
                double Xmax = CC.F_points[0].Weidu2;
                double Xmin = CC.F_points[0].Weidu1;
                double Ymax = CC.F_points[0].Jingdu1;
                double Ymin = CC.F_points[0].Jingdu2;
                //for (int i = 0; i < CC.F_points.Count; i++)
                //{
                //    Xmax = Xmax > CC.F_points[i].X ? Xmax : CC.F_points[i].X;
                //    Xmin = Xmin < CC.F_points[i].X ? Xmin : CC.F_points[i].X;
                //    Ymax = Ymax > CC.F_points[i].Y ? Ymax : CC.F_points[i].Y;
                //    Ymin = Ymin < CC.F_points[i].Y ? Ymin : CC.F_points[i].Y;
                //}
                chart1.ChartAreas[0].AxisX.Maximum = Xmax + 100;
                chart1.ChartAreas[0].AxisX.Minimum = Xmin - 10;
                chart1.ChartAreas[0].AxisY.Maximum = Ymax + 100;
                chart1.ChartAreas[0].AxisY.Minimum = Ymin - 100;
                for (int i = 0; i < CC.F_points.Count; i++)
                {
                    chart1.Series[0].Points.AddXY(CC.F_points[i].Weidu1, CC.F_points[i].Jingdu2);
                }
                for (int i = 0; i < CC.F_points.Count; i++)
                {
                    chart1.Series[1].Points.AddXY(CC.F_points[i].Weidu1, CC.F_points[i].Jingdu2);
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

                richTextBox1.Text += "**************************************************\r\n";
                richTextBox1.Text += "                  大地主题反算\n\n";
                richTextBox1.Text += "**************************************************\r\n";
                richTextBox1.Text += "----------------------统计数据---------------------\r\n";
                richTextBox1.Text += "计算点对总数  ：" + dataGridView1.Rows.Count + "\r\n";
                richTextBox1.Text += "----------------------计算结果---------------------\r\n";
                richTextBox1.Text += "点名     \t纬度(B)  \t经度(L)  \t大地方位角(A) \t大地线长(S)\r\n";
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    richTextBox1.Text += string.Format("{0, -8}", dataGridView1.Rows[i].Cells[0].Value) + "\t";
                    richTextBox1.Text += string.Format("{0, -8}", dataGridView1.Rows[i].Cells[1].Value) + "\t";
                    richTextBox1.Text += string.Format("{0, -8}", dataGridView1.Rows[i].Cells[2].Value) + "\t";
                    richTextBox1.Text += string.Format("{0, -8}", dataGridView1.Rows[i].Cells[6].Value) + "\t";
                    richTextBox1.Text += string.Format("{0, -8}", dataGridView1.Rows[i].Cells[8].Value) + "\n";

                    richTextBox1.Text += string.Format("{0, -8}", dataGridView1.Rows[i].Cells[3].Value) + "\t";
                    richTextBox1.Text += string.Format("{0, -8}", dataGridView1.Rows[i].Cells[4].Value) + "\t";
                    richTextBox1.Text += string.Format("{0, -8}", dataGridView1.Rows[i].Cells[5].Value) + "\t";
                    richTextBox1.Text += string.Format("{0, -8}", dataGridView1.Rows[i].Cells[7].Value) + "\t";
                    richTextBox1.Text += string.Format("{0, -8}", dataGridView1.Rows[i].Cells[8].Value) + "\r\n";
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
                详细信息ToolStripMenuItem1.Checked = true;
            }
            else if (详细信息ToolStripMenuItem1.Checked == true)
            {
                chart1.Series[0].Label = "";
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
                拖拽缩放ToolStripMenuItem.Checked = true;
            }
            else if (拖拽缩放ToolStripMenuItem.Checked == true)
            {
                chart1.ChartAreas[0].CursorX.IsUserEnabled = false;
                chart1.ChartAreas[0].CursorY.IsUserEnabled = false;
                chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = false;
                chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = false;
                拖拽缩放ToolStripMenuItem.Checked = false;
            }
            this.Focus();
        }
        #endregion
        #region Tool功能实现
        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            导入反算数据ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (cal_tag2)
            {
                纵断面计算ToolStripMenuItem_Click(sender, e);
                生成报告ToolStripMenuItem_Click(sender, e);
            }
            else
            {
                //纵断面计算ToolStripMenuItem_Click(sender, e);
                //横断面计算ToolStripMenuItem_Click(sender, e);
                横断面计算ToolStripMenuItem_Click(sender, e);
                绘制图形ToolStripMenuItem_Click(sender, e);
                生成报告ToolStripMenuItem_Click(sender, e);
                cal_tag = true;
            }

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
            ProcessStartInfo psi = new ProcessStartInfo("Helper.docx");
            p.StartInfo = psi;
            try
            {
                p.Start();
            }catch { MessageBox.Show("大地主题正反算V1.0"); }
        }

        private void button6_Click(object sender, EventArgs e)
        {
           FormCode fc =  new FormCode();
            fc.ShowDialog();
        }

        private void 保存凸包中间文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Title = "保存正确性";
                saveFileDialog1.Filter = "文本文档(*.txt)|*.txt";
                saveFileDialog1.ShowDialog();
                string str = "";
                if (saveFileDialog1.FileName != "")
                {

                    //for (int i = 0; i < CC.points.Count; i++)
                    //{
                    //        str += CC.Points[i].Q_angle+"\r\n";
                    //}
                    using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                        sw.Write(str);
                    
                }

            }
            catch { }
        }

        private void 保存体积中间文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "保存正确性文件";
            saveFileDialog1.Filter = "文本文档(*.txt)| *.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strs = "";
               
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        strs += Convert.ToString(this.dataGridView2.Rows[i].Cells[0].Value) + "\t" + this.dataGridView2.Rows[i].Cells[1].Value + "\r\n";
                    }
                    using (StreamWriter sww = new StreamWriter(saveFileDialog1.FileName))
                        sww.Write(strs);
                
            }
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            拖拽缩放ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            详细信息ToolStripMenuItem1_Click(sender, e);
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            纵断面计算ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            横断面计算ToolStripMenuItem_Click(sender, e);
        }

        private void 保存报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            保存报告ToolStripMenuItem1_Click(sender, e);
        }

        private void 导入正算数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
            try
            {
                openFileDialog1.ShowDialog();
                if (openFileDialog1.FileName!="")
                {
                    dataGridView1.Rows.Clear();
                    CC.Z_DataIn(openFileDialog1.FileName);
                    cal_tag2 = false;
                    for (int i = 0; i < CC.Z_points.Count(); i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = CC.Z_points[i].Start;
                        dataGridView1.Rows[i].Cells[1].Value = CC.Z_points[i].Weidu;
                        dataGridView1.Rows[i].Cells[2].Value = CC.Z_points[i].Jingdu;
                        dataGridView1.Rows[i].Cells[3].Value = CC.Z_points[i].Angle;
                        dataGridView1.Rows[i].Cells[4].Value = CC.Z_points[i].End;
                        dataGridView1.Rows[i].Cells[5].Value = CC.Z_points[i].Weidu2;
                        dataGridView1.Rows[i].Cells[6].Value = CC.Z_points[i].Jingdu2;
                        dataGridView1.Rows[i].Cells[7].Value = CC.Z_points[i].Angle;

                    }
                }
                this.Alert("导入正算数据成功", Form_Alert.enmType.Success);
            }
            catch { this.Alert("导入数据失败", Form_Alert.enmType.Success); }
            this.Focus();
        }

        private void 导入反算数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
            try
            {
                openFileDialog1.ShowDialog();
                if (openFileDialog1.FileName != "")
                {
                    dataGridView1.Rows.Clear();
                    CC.F_DataIn(openFileDialog1.FileName);
                    cal_tag2 = true;
                    for (int i = 0; i < CC.F_points.Count(); i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = CC.F_points[i].Start;
                        dataGridView1.Rows[i].Cells[1].Value = CC.F_points[i].Weidu1;
                        dataGridView1.Rows[i].Cells[2].Value = CC.F_points[i].Jingdu1;
                        dataGridView1.Rows[i].Cells[3].Value = CC.F_points[i].Angle1;
                        dataGridView1.Rows[i].Cells[4].Value = CC.F_points[i].End;
                        dataGridView1.Rows[i].Cells[5].Value = CC.F_points[i].Weidu2;
                        dataGridView1.Rows[i].Cells[6].Value = CC.F_points[i].Jingdu2;
                        dataGridView1.Rows[i].Cells[7].Value = CC.F_points[i].Angle2;

                    }
                }
                this.Alert("导入反算数据成功", Form_Alert.enmType.Success);
            }
            catch { this.Alert("导入数据失败", Form_Alert.enmType.Error); }
            this.Focus();
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            导入正算数据ToolStripMenuItem_Click(sender, e);
        }
    }
    #endregion
}
