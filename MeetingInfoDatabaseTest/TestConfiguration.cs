using MeetingInfoDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingInfoDatabaseTest
{
    class TestConfiguration
    {
        public static string ConnectionString { get { return "Server =.; Database = MeetingDatabase; Uid = sa; Pwd = SQL;"; } }
        public static DateTime ValidDateTime { get { return new DateTime(2019, 5, 6); } }
        public static DateTime InvalidDateTime { get { return new DateTime(1992, 5, 6); } }
        public static Meeting ValidMeeting {
            get {
                return new Meeting {
                    ClientID = 1,
                    Title = "Test title",
                    Description = "Test description",
                    Place = "Test place",
                    Date = new DateTime(2012, 1, 1)
                };
            }
        }
        public static string UpdatedTitle { get { return "New Test title"; } }

    }
}
