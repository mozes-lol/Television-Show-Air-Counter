using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
                var showsForm = (Shows)Application.OpenForms["Shows"];
                showsForm.Enabled = true;
                showsForm.LoadShows();
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

        private void Save_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show($"Are you sure you want to add '{textBox1.Text}'?",
                             "Add new show",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                string showsFile = "shows.json";
                List<TVShow> shows = new List<TVShow>();

                if (File.Exists(showsFile))
                {
                    string json = File.ReadAllText(showsFile);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        shows = JsonConvert.DeserializeObject<List<TVShow>>(json);
                    }
                }

                TVShow newShow = new TVShow
                {
                    Name = textBox1.Text.Trim(),
                    AiringDays = new List<DayOfWeek>
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday,
                    DayOfWeek.Sunday
                },
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date
                };

                shows.Add(newShow);

                File.WriteAllText(showsFile, JsonConvert.SerializeObject(shows, Formatting.Indented));

                // Optionally, update the Shows form if it requires a refresh.

                this.Close();
                MessageBox.Show("Show added successfully!", "Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
