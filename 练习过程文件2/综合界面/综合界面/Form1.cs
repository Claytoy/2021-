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
namespace 综合界面
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[1];
            saveFileDialog1.Title = "baocun";
            saveFileDialog1.Filter = "|*.txt";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file = saveFileDialog1.FileName;
                if(textBox1.Text != "")
                {
                    using (StreamWriter writer = new StreamWriter(file))
                        writer.Write(textBox1.Text);
                }
                else
                {
                    MessageBox.Show("w ");
                }
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            //double a = double.Parse(textBox2.Text);
            //MessageBox.Show(Convert.ToString(a));
            openFileDialog1.Title = "dakai";
            openFileDialog1.Filter = "|*.txt";
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                dataGridView1.Rows.Add();
                dataGridView1.Rows[0].Cells[0].Value = "dd";

            }
        }
    }
}
