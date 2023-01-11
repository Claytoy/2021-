using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LevelLib;
using System.IO;
namespace LevelAdjust
{
    public partial class Form1 : Form
    {
        private ObsData Obs;
        Adjustment adj;
        private Image img=null;
        private bool process = false;
        private bool postProcess = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateViews()
        {
            dataGridView1.DataSource = Obs.ToTable();
            richTextBox1.Text = Report();
        }

        string Report()
        {
            string text = Obs.ToString();

            if (process)
            {
                var enitity = new DataEnitity(Obs);
                Adjustment adj = new Adjustment(enitity);
                adj.HeightAdjustment();
                Report report = new Report(Obs, adj.Data);
                text = report.ToString();
                text += adj.ToString1();

            }
            if (postProcess)
            {
                var enitity = new DataEnitity(Obs);
                Adjustment adj = new Adjustment(enitity);
                adj.HeightAdjustment();
                Report report = new Report(Obs, adj.Data);
                text = report.ToString();
                text += adj.ToString();
            }
            return text;
        }

        #region 文件
        private void toolOpenData_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "(txt文件)|*txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Obs = FileHandler.Read(openFileDialog1.FileName);
                    UpdateViews();
                }
                catch (Exception)
                {

                    throw new Exception("数据导入失败！");
                }

            }
        }
        private void MenuFileOpen_Click(object sender, EventArgs e)
        {
            toolOpenData_Click(sender, e);
        }


        private void toolSaveReport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "(TXT文件)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                StreamWriter writer = new StreamWriter(saveFileDialog1.FileName);
                writer.WriteLine(Report());
                writer.Close();

            }
        }

        private void MenuFileSaveReport_Click(object sender, EventArgs e)
        {
            toolSaveReport_Click(sender, e);
        }

        private void MenuFileSaveChart_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "(Jpeg文件)|*.jpg";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName);
            }
        }

        private void MenuFileSaveXls_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "(xls文件)|*.xls";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileHandler.SaveTable(saveFileDialog1.FileName, Obs.ToTable());
                // MessageBox.Show("保存表格成功", "信息提示");
            }
        }

        private void MenuFileSaveDXF_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "(DXF文件)|*.dxf";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var enitity = new DataEnitity(Obs);
                FileHandler.SaveDxf(saveFileDialog1.FileName, enitity);
            }
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        #endregion


        #region 计算

      private void MenuPreprocess_Click(object sender, EventArgs e)
        {
            FileHandler.PostRead(ref Obs);
            UpdateViews();
        }

        private void MenuAdjustment_Click(object sender, EventArgs e)
        {
            var enitity = new DataEnitity(Obs);
            adj = new Adjustment(enitity);
            adj.HeightAdjustment();
            process = true;
            UpdateViews();
        }


        private void toolPreProcess_Click(object sender, EventArgs e)
        {
            MenuPreprocess_Click(sender, e);
        }

        private void toolProcess_Click(object sender, EventArgs e)
        {
            MenuAdjustment_Click(sender, e);
        }

        private void toolPrecessipm_Click(object sender, EventArgs e)
        {
            postProcess = true;
            UpdateViews();
        }

        private void MenuPrecession_Click(object sender, EventArgs e)
        {
            toolPrecessipm_Click(sender, e);
        }

        private void MenuDoALL_Click(object sender, EventArgs e)
        {
            toolPreProcess_Click(sender, e);
            toolProcess_Click(sender, e);
            MenuPrecession_Click(sender, e);
        }

        #endregion


        #region 视图

     
        private void toolChart_Click(object sender, EventArgs e)
        {
            if (process)
            {
                img = DrawImage.Draw(adj.Data.Points, 520, 300);
                pictureBox1.Image = img;
            }

            tabControl1.SelectedIndex = 1;
        }

        private void toolZoomIn_Click(object sender, EventArgs e)
        {
            if (process)
            {
                int width = Convert.ToInt32(this.img.Width*1.2);
                int height = Convert.ToInt32(this.img.Height*1.2);

                img = DrawImage.Draw(adj.Data.Points, width, height);
                pictureBox1.Image = img;
            }
            tabControl1.SelectedIndex = 1;

        }

        private void toolZoomout_Click(object sender, EventArgs e)
        {
            if (process)
            {
                int width = Convert.ToInt32(this.img.Width/1.2);
                int height = Convert.ToInt32(this.img.Height/1.2);

                img = DrawImage.Draw(adj.Data.Points, width, height);
                pictureBox1.Image = img;
            }
            tabControl1.SelectedIndex = 1;
        }

        private void MenuChart_Click(object sender, EventArgs e)
        {
            toolChart_Click(sender, e);
        }

        private void MenuReport_Click(object sender, EventArgs e)
        {
            toolReport_Click(sender, e);
        }

        private void toolReport_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void MenuZoomIn_Click(object sender, EventArgs e)
        {
            toolZoomIn_Click(sender, e);
        }

        private void MenuZoomOut_Click(object sender, EventArgs e)
        {
            toolZoomout_Click(sender, e);
        }


        #endregion

        #region 帮助

        private void toolHelp_Click(object sender, EventArgs e)
        {

            MessageBox.Show("《测绘程序设计试题集（试题12 水准平差）》配套程序\n作者：李英冰，李萌，辛绍铭，赵望宇\n武汉大学测绘学院\r\nEMAIL: ybli@sgg.whu.edu.cn\r\n2017.6.25");
        }

        private void MenuHelp_Click(object sender, EventArgs e)
        {
            toolHelp_Click(sender, e);
        }

        #endregion
    }
}
