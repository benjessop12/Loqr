﻿using System;
using System.Collections.Generic;
using System.Text;
using Loqr.Config.Security;
using Loqr.Converters;
using Loqr.Database;

namespace Loqr.Helpers
{
    public class LoqrControllerHelper : Converter
    {
        private const string auth = "auth";
        private const string base_db = "base_db";
        private const string stateless = "";
        private const string column_key = "col";

        public static string ReturnResults(string id)
        {
            return ConvertDataTabletoJson(DatabaseHandlers.SelectById(base_db, long.Parse(id)));
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
            bool authorized = Authorisation(base_vals);

            foreach (KeyValuePair<string, string> key_val in base_vals)
            {
                if (key_val.Key != auth)
                {
                    keys.Append($"{key_val.Key},");
                    vals.Append($"'{key_val.Value}',");
                }
            }

            if (authorized == false)
            {
                return "Unauthorized response";
            }

            string insert_keys = RemoveLast(Convert.ToString(keys), ",");
            string insert_vals = RemoveLast(Convert.ToString(vals), ",");
            return DatabaseHandlers.InsertNewValues(base_db, insert_keys, insert_vals);
        }

        public static string EditProcessor(string id, string payload)
        {
            Dictionary<string, string> base_vals = ConvertURLPayloadToInsertStringDict(null, payload);
            StringBuilder set_val_statement = new StringBuilder();
            bool authorized = Authorisation(base_vals);

            foreach (KeyValuePair<string, string> key_val in base_vals)
            {
                if (key_val.Key != auth) { set_val_statement.Append($"{key_val.Key} = '{key_val.Value}',"); }
            }

            if (authorized == true)
            {
                string edit_statement = RemoveLast(Convert.ToString(set_val_statement), ",");
                DatabaseHandlers.AlterItem(edit_statement, id);
                return "Success";
            }
            return "Authentication failure";
        }

        public static string DeleteProcessor(string id, string payload)
        {
            Dictionary<string, string> base_vals = ConvertURLPayloadToInsertStringDict(null, payload);
            bool authorized = Authorisation(base_vals);

            if (authorized == true)
            {
                DatabaseHandlers.DeleteItem(id);
                return "Success";
            }
            return "Authentication failure";
        }

        public static string AlterDatabase(string payload)
        {
            string column_val = stateless;
            Dictionary<string, string> base_vals = ConvertURLPayloadToInsertStringDict("nil", payload);
            bool authorized = Authorisation(base_vals);

            foreach (KeyValuePair<string, string> key_val in base_vals)
            {
                if (key_val.Key == column_key) { column_val = key_val.Value; }
            }

            if (authorized == true && column_val != stateless)
            {
                DatabaseHandlers.AlterDb(column_val);
                return "Success";
            }
            return "Fail";
        }

        public static string GetConfigProcessor()
        {
            return DatabaseHandlers.GetColumnNames();
        }

        private static bool Authorisation(Dictionary<string, string> base_vals)
        {
            bool authorized = false;
            string auth_details = base_vals.TryGetValue(auth, out auth_details) ? base_vals[auth] : "nil";
            string _id = auth_details.Substring(0, 1);
            string _authToken = auth_details.Substring(1, auth_details.Length - 1);
            if (Validator.VerifyAuth(_id, _authToken) == true) { authorized = true; }
            return authorized;
        }
    }
}
