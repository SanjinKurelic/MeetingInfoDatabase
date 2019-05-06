using MeetingInfoDatabase.DAO.ADO;
using MeetingInfoDatabase.DAO.EnterpriseDAAB;
using MeetingInfoDatabase.DAO.SqlHelperClass;
using System.Collections.Generic;
using System.Linq;
using MeetingInfoDatabase.DAO.DataSet;

namespace MeetingInfoDatabase.DAO
{
    class DatabaseFactory
    {

        private readonly List<DatabaseAccessObject> _databaseAccesses;
        private readonly DatabaseType _databaseType;

        public DatabaseFactory(string connectionString, DatabaseType databaseType)
        {
            _databaseAccesses = new List<DatabaseAccessObject>();
            _databaseType = databaseType;

            // Fill database types without using reflection
            _databaseAccesses.Add(new AdoDatabaseAccessObject(connectionString));
            _databaseAccesses.Add(new EnterpriseDatabaseAccessObject(connectionString));
            _databaseAccesses.Add(new SqlHelperDatabaseAccessObject(connectionString));
            _databaseAccesses.Add(new DataSetDatabaseAccessObject(connectionString));
        }

        public DatabaseAccessObject GetDatabaseAccessObject()
        {
            return _databaseAccesses.FirstOrDefault(p => p.GetDatabaseType().Equals(_databaseType));
        }

    }
}
