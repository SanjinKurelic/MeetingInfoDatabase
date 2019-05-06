using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingInfoDatabase.Tables
{
    public abstract class TableAccessObject
    {

        protected string connectionString;

        public TableAccessObject(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public abstract TableType GetTableType();

    }
}
