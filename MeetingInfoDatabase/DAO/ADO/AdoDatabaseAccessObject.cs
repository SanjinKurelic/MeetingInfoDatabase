namespace MeetingInfoDatabase.DAO.ADO
{
    class AdoDatabaseAccessObject : DatabaseAccessObject
    {

        public AdoDatabaseAccessObject(string connectionString) : base(connectionString)
        {
            TableAccessObjects.Add(new AdoClientDatabase(connectionString));
            TableAccessObjects.Add(new AdoMeetingDatabase(connectionString));
        }

        public override DatabaseType GetDatabaseType()
        {
            return DatabaseType.Ado;
        }
    }
}
