using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Television_Show_Air_Counter
{
    public partial class NewShow : Form
    {
        public NewShow()
        {
            InitializeComponent();
        }

        private void NewShow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Application.OpenForms["Shows"] != null)
            {
                Application.OpenForms["Shows"].Enabled = true;
                Console.WriteLine("Shows form re-enabled.");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                Save.Enabled = true;
            }
            else
            {
                Save.Enabled = false;
            }
        }
    }
}
