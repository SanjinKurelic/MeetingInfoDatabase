using MeetingInfoDatabase.Models;
using MeetingInfoDatabase.Tables;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MeetingInfoDatabase.DAO.ADO
{
    public class AdoClientDatabase : TableAccessObject, IClientTableAccess
    {

        public AdoClientDatabase(string connectionString) : base(connectionString)
        {
        }

        public override TableType GetTableType()
        {
            return TableType.CLIENT;
        }

        public List<Client> GetClients()
        {
            List<Client> clients = new List<Client>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = DatabaseProcedureName.GetClients;
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
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
                    }
                }
            }
            return clients;
        }
    }
}
