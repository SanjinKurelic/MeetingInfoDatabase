using MeetingInfoDatabase.Models;
using MeetingInfoDatabase.Tables;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace MeetingInfoDatabase.DAO.EnterpriseDAAB
{
    class EnterpriseMeetingDatabase : TableAccessObject, IMeetingTableAccess
    {
        public EnterpriseMeetingDatabase(string connectionString) : base(connectionString)
        {
        }

        public override TableType GetTableType()
        {
            return TableType.MEETING;
        }

        public bool AddMeeting(Meeting meeting)
        {
            SqlDatabase db = new SqlDatabase(connectionString);
            DbCommand command = db.GetStoredProcCommand(DatabaseProcedureName.AddMeeting);

            db.AddInParameter(command, DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.Date)), SqlDbType.DateTime2, meeting.Date);
            db.AddInParameter(command, DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.Place)), SqlDbType.NVarChar, meeting.Place);
            db.AddInParameter(command, DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.Description)), SqlDbType.NVarChar, meeting.Description);
            db.AddInParameter(command, DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.Title)), SqlDbType.NVarChar, meeting.Title);
            db.AddInParameter(command, DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.ClientID)), SqlDbType.Int, meeting.ClientID);

            meeting.IDMeeting = Convert.ToInt32(db.ExecuteScalar(command));
            return meeting.IDMeeting > 0;
        }

        public bool ChangeMeeting(Meeting meeting)
        {
            SqlDatabase db = new SqlDatabase(connectionString);
            DbCommand command = db.GetStoredProcCommand(DatabaseProcedureName.ChangeMeeting);

            db.AddInParameter(command, DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.IDMeeting)), SqlDbType.Int, meeting.IDMeeting);
            db.AddInParameter(command, DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.Date)), SqlDbType.DateTime2, meeting.Date);
            db.AddInParameter(command, DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.Place)), SqlDbType.NVarChar, meeting.Place);
            db.AddInParameter(command, DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.Description)), SqlDbType.NVarChar, meeting.Description);
            db.AddInParameter(command, DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.Title)), SqlDbType.NVarChar, meeting.Title);
            db.AddInParameter(command, DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.ClientID)), SqlDbType.Int, meeting.ClientID);

            return db.ExecuteNonQuery(command) > 0;
        }

        public IEnumerable<Meeting> GetMeetings(DateTime date)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            // ReSharper disable once PossibleNullReferenceException
            Calendar cal = dfi.Calendar;
            int week = cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            SqlDatabase db = new SqlDatabase(connectionString);
            DbCommand command = db.GetStoredProcCommand(DatabaseProcedureName.GetMeetings);

            db.AddInParameter(command, DatabaseParameterName.WeekNumber, SqlDbType.Int, week);
            db.AddInParameter(command, DatabaseParameterName.Year, SqlDbType.Int, date.Year);

            using (IDataReader reader = db.ExecuteReader(command))
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
            SqlDatabase db = new SqlDatabase(connectionString);
            DbCommand command = db.GetStoredProcCommand(DatabaseProcedureName.RemoveMeeting);

            db.AddInParameter(command, DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.IDMeeting)), SqlDbType.Int, meetingId);

            return db.ExecuteNonQuery(command) > 0;
        }

        public Meeting GetMeeting(int meetingId)
        {
            SqlDatabase db = new SqlDatabase(connectionString);
            DbCommand command = db.GetStoredProcCommand(DatabaseProcedureName.GetMeeting);

            db.AddInParameter(command, DatabaseParameterName.TransformToSqlParameterName(nameof(Meeting.IDMeeting)), SqlDbType.Int, meetingId);
            IDataReader reader = db.ExecuteReader(command);

            if(!reader.Read())
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
