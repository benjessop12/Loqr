using System;
using System.Data;
using System.Text;
using Loqr.Config;

namespace Loqr.Database
{
    public class DatabaseHandlers : QueryStrings
    {
        public static void InsertNewValues(string base_cols, string base_vals)
        {
            Execute(InsertToDB("base_db", base_cols, base_vals));
        }

        public static DataTable SelectById(long id)
        {
            return MassRead(SelectFromDb("base_db", id));
        }

        public static string GetColumnNames()
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = MassRead(TableInfo("base_db"));
            string lastRow = dt.Rows[dt.Rows.Count - 1][1].ToString();
            foreach (DataRow row in dt.Rows)
            {
                sb.Append($"{row[1].ToString()}");
                if (row[1].ToString() != lastRow)
                {
                    sb.Append(",");
                }
            }
            return Convert.ToString(sb);
        }
    }
}
