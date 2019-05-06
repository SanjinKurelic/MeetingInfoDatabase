using System;

namespace MeetingInfoDatabase.DAO
{
    class DatabaseParameterName
    {

        public const string WeekNumber = "@weekNumber";

        public const string Year = "@year";

        public static string TransformToSqlParameterName(string variable)
        {
            return '@' + char.ToLower(variable[0]).ToString() + variable.Substring(1);
        }
    }
}
