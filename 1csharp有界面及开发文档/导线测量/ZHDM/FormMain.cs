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
            dataGridView1.Rows.Clear();
         
            cal_tag = false;
            cal_tag2 = false;
            button1_Click(sender, e);
            try
            {

                openFileDialog1.Title = "打开文件";
                openFileDialog1.Filter = "文本文件|*.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    dataGridView1.Rows.Clear();
                    CC.dataIn(openFileDialog1.FileName);
                    int count1 = 0;
                    for (int i = 0; i < CC.points.Count(); i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[count1].Cells[0].Value = CC.points[i].C_name;
                        dataGridView1.Rows[count1].Cells[6].Value = CC.points[i].X;
                        dataGridView1.Rows[count1].Cells[7].Value = CC.points[i].Y;
                        dataGridView1.Rows.Add();
                        count1++;
                        dataGridView1.Rows[count1].Cells[2].Value = CC.points[i].H_angle;
                        dataGridView1.Rows[count1].Cells[3].Value = CC.points[i].Q_longth;
                        dataGridView1.Rows[count1].Cells[4].Value = CC.points[i].D_X;
                        dataGridView1.Rows[count1].Cells[5].Value = CC.points[i].D_Y;
                        count1++;
                    }

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
            catch { this.Alert("导入数据失败", Form_Alert.enmType.Error); }
            this.Focus();
        }
        private void 纵断面计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CC.guance();
                CC.jinsipingcha();
                int count1 = 0;
                for (int i = 0; i < CC.points.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[count1].Cells[0].Value = CC.points[i].C_name;
                    dataGridView1.Rows[count1].Cells[6].Value = CC.points[i].X;
                    dataGridView1.Rows[count1].Cells[7].Value = CC.points[i].Y;
                    dataGridView1.Rows.Add();
                    count1++;
                    dataGridView1.Rows[count1].Cells[2].Value = CC.points[i].H_angle;
                    dataGridView1.Rows[count1].Cells[3].Value = CC.points[i].Q_longth;
                    dataGridView1.Rows[count1].Cells[4].Value = CC.points[i].D_X;
                    dataGridView1.Rows[count1].Cells[5].Value = CC.points[i].D_Y;
                    count1++;
                }
                for (int i = 0; i < CC.points.Count; i++)
                {
                    dataGridView2.Rows.Add();
                    dataGridView2.Rows[Curcount].Cells[0].Value = i+1;
                    dataGridView2.Rows[Curcount].Cells[1].Value = "X1";
                    dataGridView2.Rows[Curcount].Cells[2].Value = CC.points[i].X;
                    Curcount++;
                }
                this.Alert("近似平差计算成功", Form_Alert.enmType.Success);
            }
            catch
            {
                this.Alert("近似平差计算失败", Form_Alert.enmType.Error);
            }
            this.Focus();
        }
        int index = 0;
        private void 横断面计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cal_tag2 == false)
            {
                try
                {
                    
                    CC.zuobiao();
                    int count1 = 0;
                    for (int i = 0; i < CC.points.Count(); i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[count1].Cells[0].Value = CC.points[i].C_name;
                        dataGridView1.Rows[count1].Cells[6].Value = CC.points[i].X;
                        dataGridView1.Rows[count1].Cells[7].Value = CC.points[i].Y;
                        dataGridView1.Rows.Add();
                        count1++;
                        dataGridView1.Rows[count1].Cells[2].Value = CC.points[i].H_angle;
                        dataGridView1.Rows[count1].Cells[3].Value = CC.points[i].Q_longth;
                        dataGridView1.Rows[count1].Cells[4].Value = CC.points[i].D_X;
                        dataGridView1.Rows[count1].Cells[5].Value = CC.points[i].D_Y;
                        count1++;
                    }
                    //dataGridView2.Columns.Add("Columns3", "变量2");
                    //dataGridView2.Columns.Add("Columns4", "变量2值");
                    for (int i = 0; i < CC.points.Count; i++)
                    {
                        dataGridView2.Rows[index].Cells[3].Value = "Y坐标";
                        //dataGridView2.Rows[index].Cells[4].Value = "y";
                        dataGridView2.Rows[index].Cells[4].Value = CC.points[i].Y;
                        index++;
                    }
                    this.Alert("坐标计算成功", Form_Alert.enmType.Success);
                }
                catch
                {
                    this.Alert("坐标计算失败", Form_Alert.enmType.Error);
                }
                cal_tag2 = true;
            }
            else { Alert("请勿重复计算", Form_Alert.enmType.Error); }
            this.Focus();
        }
        private void 绘制图形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);

            try
            {
                chart1.Series[0].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                chart1.Series[0].Points.Clear();
                double Xmax = CC.points[0].X;
                double Xmin = CC.points[0].X;
                double Ymax = CC.points[0].Y;
                double Ymin = CC.points[0].Y;
                for (int i = 0; i < CC.points.Count; i++)
                {
                    Xmax = Xmax > CC.points[i].X ? Xmax : CC.points[i].X;
                    Xmin = Xmin < CC.points[i].X ? Xmin : CC.points[i].X;
                    Ymax = Ymax > CC.points[i].Y ? Ymax : CC.points[i].Y;
                    Ymin = Ymin < CC.points[i].Y ? Ymin : CC.points[i].Y;
                }
                chart1.ChartAreas[0].AxisX.Maximum = Xmax + 1000;
                chart1.ChartAreas[0].AxisX.Minimum = Xmin - 1000;
                chart1.ChartAreas[0].AxisY.Maximum = Ymax + 1000;
                chart1.ChartAreas[0].AxisY.Minimum = Ymin - 1000;
                for (int i = 0; i < CC.points.Count; i++)
                {
                    chart1.Series[0].Points.AddXY(CC.points[i].X, CC.points[i].Y);
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

                richTextBox1.Text = "已知点数据--------------------------------------------------------------------------------------------------\r\n";
                richTextBox1.Text += "测站数：" + CC.points.Count + "\r\n";
                richTextBox1.Text += "点名  \t\t   X  \t\t\t  Y    \r\n";
                for (int i = 0; i < CC.points.Count; i++)
                {
                   str += CC.points[i].C_name +"     \t" + CC.points[i].X.ToString("f5") + "\t" + CC.points[i].Y.ToString("f5") + "\r\n";

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
                toolStripButton9.Image = ResourcePic.ImageLabel_ON1;
                详细信息ToolStripMenuItem1.Checked = true;
            }
            else if (详细信息ToolStripMenuItem1.Checked == true)
            {
                chart1.Series[0].Label = "";
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
            if (cal_tag)
            {

            }
            else
            {
                纵断面计算ToolStripMenuItem_Click(sender, e);
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
            }catch { MessageBox.Show("规则格网1.0"); }
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

                    for (int i = 0; i < CC.points.Count; i++)
                    {
                            str += CC.points[i].Q_angle+"\r\n";
                    }
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
    }
    #endregion
}
