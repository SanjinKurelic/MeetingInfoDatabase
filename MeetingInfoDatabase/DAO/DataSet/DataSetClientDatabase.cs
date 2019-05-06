using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MeetingInfoDatabase.Models;
using MeetingInfoDatabase.Tables;

namespace MeetingInfoDatabase.DAO.DataSet
{
    class DataSetClientDatabase : TableAccessObject, IClientTableAccess
    {

        private System.Data.DataSet _dataSet;
        private DataTable _clientTable;
        private SqlDataAdapter _dataAdapter;

        public DataSetClientDatabase(string connectionString) : base(connectionString)
        {
            FillDataSet();
        }

        private void FillDataSet()
        {
            _dataSet = new System.Data.DataSet((new SqlConnectionStringBuilder(connectionString)).InitialCatalog);
            _dataAdapter = new SqlDataAdapter(DatabaseProcedureName.GetClients, new SqlConnection(connectionString));
            _dataAdapter.Fill(_dataSet);

            _clientTable = _dataSet.Tables[0];
        }

        public override TableType GetTableType()
        {
            return TableType.CLIENT;
        }

        public List<Client> GetClients()
        {
            List<Client> clients = new List<Client>();
            foreach (DataRow row in _clientTable.Rows)
            {
                clients.Add(new Client()
                {
                    IDClient = (int)row[nameof(Client.IDClient)],
                    Name = row[nameof(Client.Name)].ToString(),
                    Surname = row[nameof(Client.Surname)].ToString(),
                    Email = row[nameof(Client.Email)].ToString(),
                    Phone = row[nameof(Client.Phone)].ToString(),
                });
            }

            return clients;
        }
    }
}
