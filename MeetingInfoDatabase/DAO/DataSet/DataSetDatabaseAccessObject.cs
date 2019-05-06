namespace MeetingInfoDatabase.DAO.DataSet
{
    class DataSetDatabaseAccessObject : DatabaseAccessObject
    {
        public DataSetDatabaseAccessObject(string connectionString) : base(connectionString)
        {
            TableAccessObjects.Add(new DataSetClientDatabase(connectionString));
            TableAccessObjects.Add(new DataSetMeetingDatabase(connectionString));
        }

        public override DatabaseType GetDatabaseType()
        {
            return DatabaseType.DataSet;
        }
    }
}
