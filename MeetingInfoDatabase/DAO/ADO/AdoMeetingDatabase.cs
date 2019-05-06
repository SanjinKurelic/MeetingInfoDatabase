using MeetingInfoDatabase.Models;
using MeetingInfoDatabase.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace MeetingInfoDatabase.DAO.ADO
{
    public class AdoMeetingDatabase : TableAccessObject, IMeetingTableAccess
    {

        public AdoMeetingDatabase(string connectionString) : base(connectionString)
        {
        }

        public override TableType GetTableType()
        {
            return TableType.MEETING;
        }

        public IEnumerable<Meeting> GetMeetings(DateTime date)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            // ReSharper disable once PossibleNullReferenceException
            Calendar cal = dfi.Calendar;
            int week = cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = DatabaseProcedureName.GetMeetings;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter weekNumber = new SqlParameter
                    {
                        Value = week,
                        SqlDbType = SqlDbType.Int,
                        ParameterName = DatabaseParameterName.WeekNumber
                    };
                    cmd.Parameters.Add(weekNumber);
                    cmd.Parameters.AddWithValue(DatabaseParameterName.Year, date.Year);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
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
                }
            }
        }

        public bool AddMeeting(Meeting meeting)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = DatabaseProcedureName.AddMeeting;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.Date)), meeting.Date);
                    cmd.Parameters.AddWithValue(DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.Place)), meeting.Place);
                    cmd.Parameters.AddWithValue(DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.Description)), meeting.Description);
                    cmd.Parameters.AddWithValue(DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.Title)), meeting.Title);
                    cmd.Parameters.AddWithValue(DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.ClientID)), meeting.ClientID);

                    meeting.IDMeeting = Convert.ToInt32(cmd.ExecuteScalar());
                    return meeting.IDMeeting > 0;
                }
            }
        }

        public bool ChangeMeeting(Meeting meeting)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = DatabaseProcedureName.ChangeMeeting;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.IDMeeting)), meeting.IDMeeting);
                    cmd.Parameters.AddWithValue(DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.Date)), meeting.Date);
                    cmd.Parameters.AddWithValue(DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.Place)), meeting.Place);
                    cmd.Parameters.AddWithValue(DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.Description)), meeting.Description);
                    cmd.Parameters.AddWithValue(DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.Title)), meeting.Title);
                    cmd.Parameters.AddWithValue(DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.ClientID)), meeting.ClientID);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool RemoveMeeting(Meeting meeting)
        {
            return RemoveMeeting(meeting.IDMeeting);
        }

        public bool RemoveMeeting(int meetingId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = DatabaseProcedureName.RemoveMeeting;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.IDMeeting)), meetingId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public Meeting GetMeeting(int meetingId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = DatabaseProcedureName.GetMeeting;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.IDMeeting)), meetingId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
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
            }
            return null;
        }
    }
}
