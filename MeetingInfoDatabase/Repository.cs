using MeetingInfoDatabase.DAO;
using MeetingInfoDatabase.Tables;

namespace MeetingInfoDatabase
{
    public class Repository
    {

        private readonly DatabaseAccessObject _databaseAccessObject;

        public Repository(string connectionString, DatabaseType databaseType)
        {
            _databaseAccessObject = (new DatabaseFactory(connectionString, databaseType)).GetDatabaseAccessObject();
        }

        public IClientTableAccess GetClientTable()
        {
            return _databaseAccessObject.GetClientsTable();
        }

        public IMeetingTableAccess GetMeetingsTable()
        {
            return _databaseAccessObject.GetMeetingsTable();
        }

    }
}
