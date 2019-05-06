using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using MeetingInfoDatabase.Models;
using MeetingInfoDatabase.Tables;

namespace MeetingInfoDatabase.DAO.DataSet
{
    class DataSetMeetingDatabase : TableAccessObject, IMeetingTableAccess
    {
        private System.Data.DataSet _dataSet;
        private SqlDataAdapter _dataAdapter;
        private DataTable _meetingTable;

        public DataSetMeetingDatabase(string connectionString) : base(connectionString)
        {
            ConfiguraDataSet();
            FillDataSet();
        }

        private SqlParameter BuildParameter(string columnName, SqlDbType columnType)
        {
            return BuildParameter(columnName, columnType, 0);
        }

        private SqlParameter BuildParameter(string columnName, SqlDbType columnType, int size)
        {
            SqlParameter parameter = new SqlParameter(DatabaseParameterName.TransformToSqlParameterName(columnName), columnType);
            if (size != 0)
            {
                parameter.Size = size;
            }
            parameter.SourceColumn = columnName;
            return parameter;
        }

        private void ConfiguraDataSet()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            _dataSet = new System.Data.DataSet();//(new SqlConnectionStringBuilder(connectionString)).InitialCatalog);
            _dataAdapter = new SqlDataAdapter();// DatabaseProcedureName.GetAllMeetings, connection);

            // SELECT
            _dataAdapter.SelectCommand = connection.CreateCommand();
            _dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            _dataAdapter.SelectCommand.CommandText = DatabaseProcedureName.GetAllMeetings;

            // INSERT
            _dataAdapter.InsertCommand = connection.CreateCommand();
            _dataAdapter.InsertCommand.CommandType = CommandType.StoredProcedure;
            _dataAdapter.InsertCommand.CommandText = DatabaseProcedureName.AddMeeting;
            _dataAdapter.InsertCommand.Parameters.Add(BuildParameter(nameof(Meeting.Date), SqlDbType.DateTime2));
            _dataAdapter.InsertCommand.Parameters.Add(BuildParameter(nameof(Meeting.Place), SqlDbType.NVarChar, 20));
            _dataAdapter.InsertCommand.Parameters.Add(BuildParameter(nameof(Meeting.Description), SqlDbType.NVarChar, 120));
            _dataAdapter.InsertCommand.Parameters.Add(BuildParameter(nameof(Meeting.Title), SqlDbType.NVarChar, 40));
            _dataAdapter.InsertCommand.Parameters.Add(BuildParameter(nameof(Meeting.ClientID), SqlDbType.Int));

            // UPDATE
            _dataAdapter.UpdateCommand = connection.CreateCommand();
            _dataAdapter.UpdateCommand.CommandType = CommandType.StoredProcedure;
            _dataAdapter.UpdateCommand.CommandText = DatabaseProcedureName.ChangeMeeting;
            _dataAdapter.UpdateCommand.Parameters.Add(BuildParameter(nameof(Meeting.IDMeeting), SqlDbType.Int));
            _dataAdapter.UpdateCommand.Parameters.Add(BuildParameter(nameof(Meeting.Date), SqlDbType.DateTime2));
            _dataAdapter.UpdateCommand.Parameters.Add(BuildParameter(nameof(Meeting.Place), SqlDbType.NVarChar, 20));
            _dataAdapter.UpdateCommand.Parameters.Add(BuildParameter(nameof(Meeting.Description), SqlDbType.NVarChar, 120));
            _dataAdapter.UpdateCommand.Parameters.Add(BuildParameter(nameof(Meeting.Title), SqlDbType.NVarChar, 40));
            _dataAdapter.UpdateCommand.Parameters.Add(BuildParameter(nameof(Meeting.ClientID), SqlDbType.Int));

            // DELETE
            _dataAdapter.DeleteCommand = connection.CreateCommand();
            _dataAdapter.DeleteCommand.CommandType = CommandType.StoredProcedure;
            _dataAdapter.DeleteCommand.CommandText = DatabaseProcedureName.RemoveMeeting;
            _dataAdapter.DeleteCommand.Parameters.Add(BuildParameter(nameof(Meeting.IDMeeting), SqlDbType.Int));
        }

        private void FillDataSet()
        {
            _dataAdapter.Fill(_dataSet, nameof(Meeting));
            _meetingTable = _dataSet.Tables[nameof(Meeting)];

            // PRIMARY KEY
            _meetingTable.PrimaryKey = new[] { _meetingTable.Columns[nameof(Meeting.IDMeeting)] };

            DataColumn primaryColumn = _meetingTable.Columns[nameof(Meeting.IDMeeting)];
            primaryColumn.AutoIncrement = true;
            primaryColumn.AutoIncrementStep = 1;
            primaryColumn.AutoIncrementSeed = GetStartId();
        }

        private int GetStartId()
        {
            int max = 0;
            foreach (DataRow row in _meetingTable.Rows)
            {
                int id = (int)row[0];
                if (id > max)
                    max = id;
            }
            return ++max;
        }

        private bool UpdateDatabase()
        {
            try
            {
                return _dataAdapter.Update(_dataSet, nameof(Meeting)) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override TableType GetTableType()
        {
            return TableType.MEETING;
        }

        private int GetWeekNumber(DateTime date)
        {
            DateTimeFormatInfo formatInfo = DateTimeFormatInfo.CurrentInfo;
            Calendar calendar = formatInfo.Calendar;
            return calendar.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
        }

        public IEnumerable<Meeting> GetMeetings(DateTime date)
        {
            // Get all meetings from that date
            foreach (DataRow row in _meetingTable.AsEnumerable().Where(
                m => (GetWeekNumber(date) == GetWeekNumber(DateTime.Parse(m[nameof(Meeting.Date)].ToString()))) &&
                date.Year == DateTime.Parse(m[nameof(Meeting.Date)].ToString()).Year)
                )
            {
                yield return new Meeting()
                {
                    IDMeeting = (int)row[nameof(Meeting.IDMeeting)],
                    Date = DateTime.Parse(row[nameof(Meeting.Date)].ToString()),
                    Place = row[nameof(Meeting.Place)].ToString(),
                    Description = row[nameof(Meeting.Description)].ToString(),
                    Title = row[nameof(Meeting.Title)].ToString(),
                    ClientID = (int)row[nameof(Meeting.ClientID)],
                };
            }
        }

        public bool AddMeeting(Meeting meeting)
        {
            DataRow newMeeting = _meetingTable.NewRow();
            newMeeting[nameof(Meeting.Date)] = meeting.Date;
            newMeeting[nameof(Meeting.Place)] = meeting.Place;
            newMeeting[nameof(Meeting.Description)] = meeting.Description;
            newMeeting[nameof(Meeting.Title)] = meeting.Title;
            newMeeting[nameof(Meeting.ClientID)] = meeting.ClientID;

            if (newMeeting.HasErrors)
            {
                return false;
            }

            _meetingTable.Rows.Add(newMeeting);
            bool ret = UpdateDatabase();
            // Fill real ID of meeting
            FillDataSet();
            meeting.IDMeeting = GetStartId() - 1;
            return ret;
        }

        public bool ChangeMeeting(Meeting meeting)
        {
            DataRow changeMeeting = _meetingTable.Rows.Find(meeting.IDMeeting);
            changeMeeting.BeginEdit();
            changeMeeting[nameof(Meeting.Date)] = meeting.Date;
            changeMeeting[nameof(Meeting.Place)] = meeting.Place;
            changeMeeting[nameof(Meeting.Description)] = meeting.Description;
            changeMeeting[nameof(Meeting.Title)] = meeting.Title;
            changeMeeting[nameof(Meeting.ClientID)] = meeting.ClientID;
            changeMeeting.EndEdit();

            if (changeMeeting.HasErrors)
            {
                return false;
            }

            return UpdateDatabase();
        }

        public bool RemoveMeeting(Meeting meeting)
        {
            return RemoveMeeting(meeting.IDMeeting);
        }

        public bool RemoveMeeting(int meetingId)
        {
            try
            {
                DataRow row = _meetingTable.Rows.Find(meetingId);
                row.Delete();
            }
            catch (Exception)
            {
                return false;
            }
            return UpdateDatabase();
        }

        public Meeting GetMeeting(int meetingId)
        {
            DataRow row = _meetingTable.Rows.Find(meetingId);

            if (row == null)
            {
                return null;
            }

            return new Meeting()
            {
                IDMeeting = (int)row[nameof(Meeting.IDMeeting)],
                Date = DateTime.Parse(row[nameof(Meeting.Date)].ToString()),
                Place = row[nameof(Meeting.Place)].ToString(),
                Description = row[nameof(Meeting.Description)].ToString(),
                Title = row[nameof(Meeting.Title)].ToString(),
                ClientID = (int)row[nameof(Meeting.ClientID)],
            };

        }
    }
}
