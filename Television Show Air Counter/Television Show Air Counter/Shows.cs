using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Television_Show_Air_Counter
{
    public partial class Shows : Form
    {
        private bool showIsSelected = false;
        public Shows()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void Shows_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Application.OpenForms["Main"] != null)
            {
                Application.OpenForms["Main"].Enabled = true;
            }
        }

        private void NewShow_Click(object sender, EventArgs e)
        {
            NewShow newShowForm = new NewShow();
            newShowForm.Show();
            this.Enabled = false;
        }
    }
}
