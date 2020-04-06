using System;
using System.Text;
using Loqr.Database;

namespace Loqr.Config
{
    public class QueryStrings : LoqrDatabase
    {
        public static string CreateDB(string table_name, string [] column_names)
        {
            return $"CREATE TABLE {table_name} ({ArrayHandler(column_names)})";
        }

        public static string InsertToDB(string table_name, string column_names, string insert_values)
        {
            return $"INSERT INTO {table_name} ({column_names}) VALUES ({insert_values})";
        }

        public static string SelectFromDb(string table_name, long id)
        {
            return $"SELECT * FROM {table_name} WHERE id = '{id.ToString()}'";
        }

        public static string TableInfo(string table_name)
        {
            return $"PRAGMA table_info({table_name})";
        }

        private static string ArrayHandler(string [] content_array)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string lastString = content_array[content_array.Length - 1];
            foreach(string str in content_array)
            {
                stringBuilder.Append($"'{str}'");
                if (str != lastString)
                {
                    stringBuilder.Append(",");
                }
            }
            return Convert.ToString(stringBuilder);
        }
    }
}
