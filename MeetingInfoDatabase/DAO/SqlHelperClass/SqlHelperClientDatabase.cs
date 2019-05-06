using MeetingInfoDatabase.Models;
using MeetingInfoDatabase.Tables;
using Microsoft.ApplicationBlocks.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MeetingInfoDatabase.DAO.SqlHelperClass
{
    class SqlHelperClientDatabase : TableAccessObject, IClientTableAccess
    {

        public SqlHelperClientDatabase(string connectionString) : base(connectionString)
        {
        }

        public override TableType GetTableType()
        {
            return TableType.CLIENT;
        }

        public List<Client> GetClients()
        {
            List<Client> clients = new List<Client>();
            using (SqlDataReader reader =
                SqlHelper.ExecuteReader(connectionString, DatabaseProcedureName.GetClients))
            {
                while (reader.Read())
                {
                    clients.Add(new Client()
                    {
                        IDClient = (int)reader[nameof(Client.IDClient)],
                        Name = reader[nameof(Client.Name)].ToString(),
                        Surname = reader[nameof(Client.Surname)].ToString(),
                        Email = reader[nameof(Client.Email)].ToString(),
                        Phone = reader[nameof(Client.Phone)].ToString(),
                    });
                }
            }
            return clients;
        }        
    }
}
