using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace datagrid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();
            dataGridView1.Dock = DockStyle.Fill;


        }
        List<PPs> spp = new List<PPs>();
        class PPs
        {
            string name;
            decimal x;
            decimal y;

            public string Name
            {
                get
                {
                    return name;
                }

                set
                {
                    name = value;
                }
            }

            public decimal X
            {
                get
                {
                    return x;
                }

                set
                {
                    x = value;
                }
            }

            public decimal Y
            {
                get
                {
                    return y;
                }

                set
                {
                    y = value;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < 10; i++)
            {
                PPs p = new PPs();
                p.Name = "n" + i.ToString();
                p.X = i + 1;
                p.Y = i + 2;
                spp.Add(p);
            }
            for (int i =0; i< spp.Count; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = spp[i].Name;
                dataGridView1.Rows[i].Cells[1].Value = spp[i].X;
                dataGridView1.Rows[i].Cells[2].Value = spp[i].Y;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                spp.Clear();
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    PPs pp = new PPs();
                    pp.Name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    pp.X = Convert.ToDecimal(dataGridView1.Rows[i].Cells[1].Value);
                    pp.Y = Convert.ToDecimal(dataGridView1.Rows[i].Cells[2].Value);
                    spp.Add(pp);
                }
                int aaa = 0;
            }
            else
            {
                MessageBox.Show("ok");
            }
            
        }
    }
}
