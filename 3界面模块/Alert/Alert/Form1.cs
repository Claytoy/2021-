using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alert
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void SAlert(string msg, Alert.enmType type)
        {
            Alert fm = new Alert();
            fm.showAlert(msg, type);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SAlert("1", Alert.enmType.Success);
        }
    }
}
