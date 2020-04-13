﻿using System;
using System.Collections.Generic;
using System.Text;
using Loqr.Config.Security;
using Loqr.Converters;
using Loqr.Database;

namespace Loqr.Helpers
{
    public class AdminControllerHelper : HashGenerator
    {
        public static string CreateAuth(string id, string payload)
        {
            if (DatabaseHandlers.CountDb("admin_db") != 0)
            {
                return "Fail";
            }
            Dictionary<string, string> base_vals = Converter.ConvertURLPayloadToInsertStringDict(id, payload);
            StringBuilder keys = new StringBuilder();
            StringBuilder vals = new StringBuilder();
            HashSalt hashSalt;

            foreach (KeyValuePair<string, string> key_val in base_vals)
            {
                if (key_val.Key == "auth")
                {
                    hashSalt = GenerateSaltedHash(64, key_val.Value);
                    keys.Append($"hash,");
                    vals.Append($"'{hashSalt.Hash}',");
                    keys.Append($"salt,");
                    vals.Append($"'{hashSalt.Salt}',");
                }
                else
                {
                    keys.Append($"{key_val.Key},");
                    vals.Append($"'{key_val.Value}',");
                }
            }

            string insert_keys = Converter.RemoveLast(Convert.ToString(keys), ",");
            string insert_vals = Converter.RemoveLast(Convert.ToString(vals), ",");
            if (insert_keys == "id,hash,salt")
            {
                DatabaseHandlers.InsertNewValues("admin_db", insert_keys, insert_vals);
                return "Success";
            }
            return "Fail";
        }
    }
}
