using MeetingInfoDatabase.Tables;
using System.Collections.Generic;
using System.Linq;

namespace MeetingInfoDatabase.DAO
{
    abstract class DatabaseAccessObject
    {

        protected readonly string ConnectionString;
        protected List<TableAccessObject> TableAccessObjects;

        protected DatabaseAccessObject(string connectionString)
        {
            ConnectionString = connectionString;
            TableAccessObjects = new List<TableAccessObject>();
        }

        public IClientTableAccess GetClientsTable()
        {
            return (IClientTableAccess)TableAccessObjects.FirstOrDefault(t => t.GetTableType().Equals(TableType.CLIENT));
        }

        public IMeetingTableAccess GetMeetingsTable()
        {
            return (IMeetingTableAccess)TableAccessObjects.FirstOrDefault(t => t.GetTableType() == TableType.MEETING);
        }

        public abstract DatabaseType GetDatabaseType();

    }
}
