using System;
using System.Data;
using System.Security.Cryptography;
using Loqr.Database;

namespace Loqr.Config.Security
{
    public class Validator : HashGenerator
    {
        public static bool VerifyAuth(string id, string auth)
        {
            try
            {
                DataTable dt = DatabaseHandlers.SelectById("admin_db", long.Parse(id));
                return Verify(auth, Convert.ToString(dt.Rows[0]["hash"]), Convert.ToString(dt.Rows[0]["salt"]));
            }
            catch
            {
                return false;
            }
        }

        private static bool Verify(string enteredPassword, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000);
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storedHash;
        }
    }
}
