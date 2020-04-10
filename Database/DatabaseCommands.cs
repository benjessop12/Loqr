using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Loqr.Config;

namespace Loqr.Database
{
    public class LoqrDatabase
    {
        private const string root_db = "Loqr.db";

        private static SQLiteConnection CreateConnection(string connection_string)
        {
            if (!File.Exists(connection_string))
            {
                string[] base_columns = { "id INT PRIMARY KEY NOT NULL" };
                SQLiteConnection.CreateFile(connection_string);
                Execute(QueryStrings.CreateDB("base_db", base_columns));
            }
            SQLiteConnection sqlite_conn = new SQLiteConnection($"Data Source={connection_string};Version=3;New=True;Compress=True;");
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to root sqlite file: {ex}");
            }
            return sqlite_conn;
        }

        public static string Execute(string statement)
        {
            try
            {
                using (SQLiteConnection sqlite_conn = CreateConnection(root_db))
                {
                    SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = statement;
                    sqlite_cmd.ExecuteNonQuery();
                }
            }
            catch(SQLiteException ex)
            {
                if(ex.ErrorCode == 19)
                {
                    return "Unique constraint failed";
                }
            }
            return "Success";
        }

        public static DataTable MassRead(string statement)
        {
            DataTable return_datatable = new DataTable();
            using(SQLiteConnection sqlite_conn = CreateConnection(root_db))
            {
                SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = statement;
                SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();
                return_datatable.Load(sqlite_datareader);
            }
            return return_datatable;
        }
    }
}
