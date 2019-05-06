using MeetingInfoDatabase.Models;
using MeetingInfoDatabase.Tables;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;

namespace MeetingInfoDatabase.DAO.SqlHelperClass
{
    class SqlHelperMeetingDatabase : TableAccessObject, IMeetingTableAccess
    {

        public SqlHelperMeetingDatabase(string connectionString) : base(connectionString)
        {
        }

        public override TableType GetTableType()
        {
            return TableType.MEETING;
        }

        public bool AddMeeting(Meeting meeting)
        {
            meeting.IDMeeting = Convert.ToInt32(SqlHelper.ExecuteScalar(connectionString, DatabaseProcedureName.AddMeeting,
                       meeting.Date, meeting.Place, meeting.Description, meeting.Title, meeting.ClientID));
            return meeting.IDMeeting > 0;
        }

        public bool ChangeMeeting(Meeting meeting)
        {
            return SqlHelper.ExecuteNonQuery(connectionString, DatabaseProcedureName.ChangeMeeting, meeting.IDMeeting,
                       meeting.Date, meeting.Place, meeting.Description, meeting.Title, meeting.ClientID) > 0;
        }

        public IEnumerable<Meeting> GetMeetings(DateTime date)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            // ReSharper disable once PossibleNullReferenceException
            Calendar cal = dfi.Calendar;
            int week = cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            using (SqlDataReader reader =
                SqlHelper.ExecuteReader(connectionString, DatabaseProcedureName.GetMeetings, week, date.Year))
            {
                while (reader.Read())
                {
                    yield return new Meeting()
                    {
                        IDMeeting = (int)reader[nameof(Meeting.IDMeeting)],
                        Date = reader.GetDateTime(reader.GetOrdinal(nameof(Meeting.Date))),
                        Place = reader[nameof(Meeting.Place)].ToString(),
                        Description = reader[nameof(Meeting.Description)].ToString(),
                        Title = reader[nameof(Meeting.Title)].ToString(),
                        ClientID = (int)reader[nameof(Meeting.ClientID)],
                    };
                }
            }
        }

        public bool RemoveMeeting(Meeting meeting)
        {
            return RemoveMeeting(meeting.IDMeeting);
        }

        public bool RemoveMeeting(int meetingId)
        {
            return SqlHelper.ExecuteNonQuery(connectionString, DatabaseProcedureName.RemoveMeeting, meetingId) > 0;
        }

        public Meeting GetMeeting(int meetingId)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(connectionString, DatabaseProcedureName.GetMeeting, meetingId);
            if (!reader.Read())
            {
                return null;
            }
            return new Meeting()
            {
                IDMeeting = (int)reader[nameof(Meeting.IDMeeting)],
                Date = reader.GetDateTime(reader.GetOrdinal(nameof(Meeting.Date))),
                Place = reader[nameof(Meeting.Place)].ToString(),
                Description = reader[nameof(Meeting.Description)].ToString(),
                Title = reader[nameof(Meeting.Title)].ToString(),
                ClientID = (int)reader[nameof(Meeting.ClientID)],
            };

        }
    }
}
