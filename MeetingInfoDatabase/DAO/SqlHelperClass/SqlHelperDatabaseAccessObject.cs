namespace MeetingInfoDatabase.DAO.SqlHelperClass
{
    class SqlHelperDatabaseAccessObject : DatabaseAccessObject
    {
        public SqlHelperDatabaseAccessObject(string connectionString) : base(connectionString)
        {
            TableAccessObjects.Add(new SqlHelperClientDatabase(connectionString));
            TableAccessObjects.Add(new SqlHelperMeetingDatabase(connectionString));
        }

        public override DatabaseType GetDatabaseType()
        {
            return DatabaseType.SqlHelper;
        }
    }
}
