using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingInfoDatabase.Models
{
    public class Client
    {

        public int IDClient { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public override string ToString()
        {
            return Name + " " + Surname;
        }

    }
}
