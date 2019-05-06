using MeetingInfoDatabase.Models;
using MeetingInfoDatabase.Tables;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace MeetingInfoDatabase.DAO.EnterpriseDAAB
{
    class EnterpriseClientDatabase : TableAccessObject, IClientTableAccess
    {

        public EnterpriseClientDatabase(string connectionString) : base(connectionString)
        {
        }

        public override TableType GetTableType()
        {
            return TableType.CLIENT;
        }

        public List<Client> GetClients()
        {
            List<Client> clients = new List<Client>();
            SqlDatabase db = new SqlDatabase(connectionString);
            DbCommand command = db.GetStoredProcCommand(DatabaseProcedureName.GetClients);

            using(IDataReader reader = db.ExecuteReader(command))
            {
                while(reader.Read())
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
