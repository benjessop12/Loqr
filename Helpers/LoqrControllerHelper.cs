using System;
using System.Collections.Generic;
using System.Text;
using Loqr.Converters;
using Loqr.Database;

namespace Loqr.Helpers
{
    public class LoqrControllerHelper : Converter
    {
        public static string ReturnResults(string id)
        {
            return ConvertDataTabletoJson(DatabaseHandlers.SelectById(long.Parse(id)));
        }

        public static string MassReturnResults()
        {
            return ConvertDataTabletoJson(DatabaseHandlers.MassSelect());
        }

        public static string InsertProcessor(string id, string payload)
        {
            Dictionary<string, string> base_vals = ConvertURLPayloadToInsertStringDict(id, payload);
            StringBuilder keys = new StringBuilder();
            StringBuilder vals = new StringBuilder();

            foreach (KeyValuePair<string, string> key_val in base_vals)
            {
                keys.Append($"{key_val.Key},");
                vals.Append($"'{key_val.Value}',");
            }

            string insert_keys = RemoveLast(Convert.ToString(keys), ",");
            string insert_vals = RemoveLast(Convert.ToString(vals), ",");
            return DatabaseHandlers.InsertNewValues(insert_keys, insert_vals);
        }

        public static void EditProcessor(string id, string payload)
        {
            Dictionary<string, string> base_vals = ConvertURLPayloadToInsertStringDict(null, payload);
            StringBuilder set_val_statement = new StringBuilder();

            foreach (KeyValuePair<string, string> key_val in base_vals)
            {
                set_val_statement.Append($"{key_val.Key} = '{key_val.Value}',");
            }

            string edit_statement = RemoveLast(Convert.ToString(set_val_statement), ",");
            DatabaseHandlers.AlterItem(edit_statement, id);
        }

        public static void DeleteProcessor(string id)
        {
            DatabaseHandlers.DeleteItem(id);
        }

        public static void AlterDatabase(string payload)
        {
            string column_key = "col";
            string column_val;
            Dictionary<string, string> base_vals = ConvertURLPayloadToInsertStringDict("nil", payload);
            if (!base_vals.TryGetValue(column_key, out column_val))
            {
                return;
            }
            DatabaseHandlers.AlterDb(column_val);
        }

        public static string GetConfigProcessor()
        {
            return DatabaseHandlers.GetColumnNames();
        }
    }
}
