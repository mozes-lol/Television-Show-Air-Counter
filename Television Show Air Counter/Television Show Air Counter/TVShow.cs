using System;
using System.Collections.Generic;

namespace Television_Show_Air_Counter
{
    public class TVShow
    {
        public string Name { get; set; }

        // This will store only the days the show actually airs (e.g. Monday, Wednesday)
        public List<DayOfWeek> AiringDays { get; set; } = new List<DayOfWeek>();

        //Not sure why these are included. Be sure to remove these once done
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Optional helper to check if a show airs on a given day
        public bool AirsOn(DayOfWeek day)
        {
            return AiringDays.Contains(day);
        }
    }
}