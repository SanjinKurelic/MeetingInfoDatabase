using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingInfoDatabase.Models
{
    public class Meeting
    {
        public int IDMeeting { get; set; }

        public DateTime Date { get; set; }

        public string Place { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int ClientID { get; set; }

        public override string ToString()
        {
            return Title + " " + Description + " [" + Date + "]";
        }

    }
}
