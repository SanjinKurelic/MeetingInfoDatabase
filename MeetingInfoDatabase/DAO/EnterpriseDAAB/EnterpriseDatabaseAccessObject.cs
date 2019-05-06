namespace MeetingInfoDatabase.DAO.EnterpriseDAAB
{
    class EnterpriseDatabaseAccessObject : DatabaseAccessObject
    {
        public EnterpriseDatabaseAccessObject(string connectionString) : base(connectionString)
        {
            TableAccessObjects.Add(new EnterpriseClientDatabase(connectionString));
            TableAccessObjects.Add(new EnterpriseMeetingDatabase(connectionString));
        }

        public override DatabaseType GetDatabaseType()
        {
            return DatabaseType.EnterpriseDaab;
        }
    }
}
