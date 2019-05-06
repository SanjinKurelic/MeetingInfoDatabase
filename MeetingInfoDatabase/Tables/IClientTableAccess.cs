using MeetingInfoDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingInfoDatabase.Tables
{
    public interface IClientTableAccess
    {

        List<Client> GetClients();

    }
}
