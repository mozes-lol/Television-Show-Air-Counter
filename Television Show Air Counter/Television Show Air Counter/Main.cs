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

        private int CountAiredDays(DateTime start, DateTime end, TVShow show)
        {
            // If no show is selected or dates are invalid, return 0
            if (show == null || start.Date > end.Date) return 0;

            // Get the total count of every day of the week in the date range
            var dayCounts = CountDaysOfWeek(start, end);
            int totalAiredDays = 0;

            // Sum up the counts only for the days the show actually airs
            foreach (DayOfWeek day in show.AiringDays)
            {
                totalAiredDays += dayCounts[day];
            }

            return totalAiredDays;
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

                        // Testing /////////////

                        Console.WriteLine(shows[0].AiringDays[0]);
                        foreach (var show in shows)
                        {
                            Console.WriteLine($"Show: {show.Name}, Start: {show.StartDate}, End: {show.EndDate}, Airing Days: {string.Join(", ", show.AiringDays)}");
                        }

                        ////////////////////////

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
            var counts = CountDaysOfWeek(StartDate.Value, EndDate.Value);
            TVShow selectedShow = TVShowToFilter.SelectedItem as TVShow;

            // If a show is selected, only display the count if it airs on that day. Otherwise, show 0.
            // If no show is selected, default to showing the total occurrences of that day.
            Monday.Text = (selectedShow == null || selectedShow.AirsOn(DayOfWeek.Monday)) ? counts[DayOfWeek.Monday].ToString() : "0";
            Tuesday.Text = (selectedShow == null || selectedShow.AirsOn(DayOfWeek.Tuesday)) ? counts[DayOfWeek.Tuesday].ToString() : "0";
            Wednesday.Text = (selectedShow == null || selectedShow.AirsOn(DayOfWeek.Wednesday)) ? counts[DayOfWeek.Wednesday].ToString() : "0";
            Thursday.Text = (selectedShow == null || selectedShow.AirsOn(DayOfWeek.Thursday)) ? counts[DayOfWeek.Thursday].ToString() : "0";
            Friday.Text = (selectedShow == null || selectedShow.AirsOn(DayOfWeek.Friday)) ? counts[DayOfWeek.Friday].ToString() : "0";
            Saturday.Text = (selectedShow == null || selectedShow.AirsOn(DayOfWeek.Saturday)) ? counts[DayOfWeek.Saturday].ToString() : "0";
            Sunday.Text = (selectedShow == null || selectedShow.AirsOn(DayOfWeek.Sunday)) ? counts[DayOfWeek.Sunday].ToString() : "0";
        }

        private void UpdateDaysDescription()
        {
            // Check if a show is actually selected in the ComboBox
            if (TVShowToFilter.SelectedItem is TVShow selectedShow)
            {
                int airedDays = CountAiredDays(StartDate.Value, EndDate.Value, selectedShow);
                TotalDaysDescription.Text = $"'{selectedShow.Name}' has been aired for {airedDays} days.";
            }
            else
            {
                TotalDaysDescription.Text = "Please select a show to see air days.";
            }
        }

        private void showsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Shows showsForm = new Shows();
            showsForm.Show();
            this.Enabled = false;
        }

        private void ShowToFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDaysCount();
            UpdateDaysDescription();
        }
    }
}
