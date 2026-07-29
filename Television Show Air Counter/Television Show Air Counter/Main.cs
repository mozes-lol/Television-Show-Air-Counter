using Newtonsoft.Json;
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
using System.IO;

namespace Television_Show_Air_Counter
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            LoadShows();
        }

        private void StartDate_ValueChanged(object sender, EventArgs e)
        {
            if (StartDate.Value > EndDate.Value)
            {
                // If the start date is greater than the end date, adjust the end date to match the start date
                EndDate.Value = StartDate.Value;
            }
            UpdateDaysCount();
            UpdateDaysDescription();
        }

        private void EndDate_ValueChanged(object sender, EventArgs e)
        {

            if (EndDate.Value < StartDate.Value)
            {
                // If the end date is before the start date, adjust the start date to match the end date
                StartDate.Value = EndDate.Value;
            }
            UpdateDaysCount();
            UpdateDaysDescription();
        }

        private int CountTotalDays(DateTime start, DateTime end)
        {
            if (start.Date > end.Date) return 0;    
            return (int)(end.Date - start.Date).TotalDays + 1; // +1 to include both start and end dates
        }   

        public Dictionary<DayOfWeek, int> CountDaysOfWeek(DateTime start, DateTime end)
        {
            var counts = new Dictionary<DayOfWeek, int>();

            // Initialize count for each day of the week
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                counts[day] = 0;
            }

            if (start.Date > end.Date) return counts;

            // Calculate total days including both start and end dates
            int totalDays = (int)(end.Date - start.Date).TotalDays + 1;
            int fullWeeks = totalDays / 7;

            // Add full weeks
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                counts[day] = fullWeeks;
            }

            // Add the remaining days 
            int remainder = totalDays % 7;
            for (int i = 0; i < remainder; i++)
            {
                counts[start.Date.AddDays(i).DayOfWeek]++;
            }

            return counts;
        }

        public void LoadShows()
        {
            string showsFile = "shows.json";
            if (File.Exists(showsFile))
            {
                try
                {
                    string json = File.ReadAllText(showsFile);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var shows = JsonConvert.DeserializeObject<List<TVShow>>(json);

                        // Keep track of current selection if any
                        string selectedShowName = TVShowToFilter.SelectedItem != null
                            ? ((TVShow)TVShowToFilter.SelectedItem).Name
                            : null;

                        TVShowToFilter.DataSource = null; // reset
                        TVShowToFilter.DataSource = shows;
                        TVShowToFilter.DisplayMember = "Name";

                        if (selectedShowName != null)
                        {
                            var showToSelect = shows.FirstOrDefault(s => s.Name == selectedShowName);
                            if (showToSelect != null)
                            {
                                TVShowToFilter.SelectedItem = showToSelect;
                            }
                        }
                        else
                        {
                            TVShowToFilter.SelectedIndex = -1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading shows: " + ex.Message);
                }
            }
        }

        private void UpdateDaysCount()
        {
            Monday.Text = CountDaysOfWeek(StartDate.Value, EndDate.Value)[DayOfWeek.Monday].ToString();
            Tuesday.Text = CountDaysOfWeek(StartDate.Value, EndDate.Value)[DayOfWeek.Tuesday].ToString();
            Wednesday.Text = CountDaysOfWeek(StartDate.Value, EndDate.Value)[DayOfWeek.Wednesday].ToString();
            Thursday.Text = CountDaysOfWeek(StartDate.Value, EndDate.Value)[DayOfWeek.Thursday].ToString();
            Friday.Text = CountDaysOfWeek(StartDate.Value, EndDate.Value)[DayOfWeek.Friday].ToString();
            Saturday.Text = CountDaysOfWeek(StartDate.Value, EndDate.Value)[DayOfWeek.Saturday].ToString();
            Sunday.Text = CountDaysOfWeek(StartDate.Value, EndDate.Value)[DayOfWeek.Sunday].ToString();
        }

        private void UpdateDaysDescription()
        {
            TotalDaysDescription.Text = "[The show] has been aired for " + CountTotalDays(StartDate.Value, EndDate.Value) + " days.";
        }

        private void showsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Shows showsForm = new Shows();
            showsForm.Show();
            this.Enabled = false;
        }

        private void ShowToFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
