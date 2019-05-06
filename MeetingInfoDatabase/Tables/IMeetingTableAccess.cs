using MeetingInfoDatabase.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingInfoDatabase.Tables
{
    public interface IMeetingTableAccess
    {

        IEnumerable<Meeting> GetMeetings(DateTime date);

        Meeting GetMeeting(int meetingId);

        bool AddMeeting(Meeting meeting);

        bool ChangeMeeting(Meeting meeting);

        bool RemoveMeeting(Meeting meeting);

        bool RemoveMeeting(int meetingId);

    }
}
