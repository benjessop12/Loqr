using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;

namespace Loqr.Converters
{
    public class Converter
    {
        public static string ConvertDataTabletoJson(DataTable dataTable)
        {
            var loqrItem = new List<LoqrItem>();
            foreach (DataRow row in dataTable.Rows)
            {
                long id = long.Parse(row["id"].ToString());
                foreach(DataColumn col in dataTable.Columns)
                {
                    if (col.ToString() != "id")
                    {
                        loqrItem.Add(new LoqrItem(id, col.ToString(), row[col].ToString(), typeof(string)));
                    }
                }
            }
            var groupedItem = loqrItem
                .GroupBy(i => i.Id)
                .Select(grp => grp.ToList())
                .ToList();
            return JsonConvert.SerializeObject(groupedItem);
        }

        public static Dictionary<string, string> ConvertURLPayloadToInsertStringDict(string id, string payload)
        {
            Dictionary<string, string> payload_converter = new Dictionary<string, string>();
            if (id != null)
            {
                payload_converter.Add("id", id);
            }
            var elemns = payload.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string item in elemns)
            {
                string key = item.Substring(0, item.LastIndexOf("="));
                string value = item.Substring(item.LastIndexOf("=") + 1);
                payload_converter.Add(key, value);
            }
            return payload_converter;
        }

        public static string RemoveLast(string text, string character)
        {
            if (text.Length < 1) return text;
            return text.Remove(text.LastIndexOf(character), character.Length);
        }
    }
}
