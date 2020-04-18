using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using Loqr.Models;

namespace Loqr.Converters
{
    public class Converter
    {
        public static string ConvertDataTabletoJson(DataTable dataTable)
        {
            var groupedItem = DataTableToModelList(dataTable)
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
            text = text.Length < 1 ? text : text.Remove(text.LastIndexOf(character), character.Length);
            return text;
        }

        private static List<LoqrItem> DataTableToModelList(DataTable dataTable)
        {
            List<LoqrItem> returnList = new List<LoqrItem>();
            foreach (DataRow row in dataTable.Rows)
            {
                long id = long.Parse(row["id"].ToString());
                foreach (DataColumn col in dataTable.Columns)
                {
                    if (col.ToString() != "id")
                    {
                        returnList.Add(new LoqrItem(id, col.ToString(), row[col].ToString(), typeof(string)));
                    }
                }
            }
            return returnList;
        }
    }
}
