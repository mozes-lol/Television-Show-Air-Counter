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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void PushStartAndEndDates()
        {


        }

        private void StartDate_ValueChanged(object sender, EventArgs e)
        {
            if (StartDate.Value > EndDate.Value)
            {
                // If the start date is greater than the end date, adjust the end date to match the start date
                EndDate.Value = StartDate.Value;
            }
        }

        private void EndDate_ValueChanged(object sender, EventArgs e)
        {

            if (EndDate.Value < StartDate.Value)
            {
                // If the end date is before the start date, adjust the start date to match the end date
                StartDate.Value = EndDate.Value;
            }
        }
    }
}
