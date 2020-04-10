﻿using System;
using System.Data;
using System.Text;
using Loqr.Config;

namespace Loqr.Database
{
    public class DatabaseHandlers : QueryStrings
    {
        public static string InsertNewValues(string base_cols, string base_vals)
        {
            return Execute(InsertToDB("base_db", base_cols, base_vals));
        }

        public static void AlterDb(string column_name)
        {
            Execute(AlterDb("base_db", column_name));
        }

        public static DataTable SelectById(long id)
        {
            return MassRead(SelectFromDb("base_db", id));
        }

        public static DataTable MassSelect()
        {
            return MassRead(MassSelectFromDb("base_db"));
        }

        public static void AlterItem(string edit_statement, string id)
        {
            Execute(EditItem("base_db", edit_statement, id));
        }

        public static void DeleteItem(string id)
        {
            Execute(DeleteItem("base_db", id));
        }

        public static string GetColumnNames()
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = MassRead(TableInfo("base_db"));
            string lastRow = dt.Rows[dt.Rows.Count - 1][1].ToString();
            sb.Append("Fields:\n\n{\n");
            foreach (DataRow row in dt.Rows)
            {
                sb.Append($"  {row[1].ToString()}");
                if (row[1].ToString() != lastRow)
                {
                    sb.Append(",\n");
                }
            }
            sb.Append("\n}");
            return Convert.ToString(sb);
        }
    }
}
