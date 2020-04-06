using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;

namespace Loqr.Converters
{
    public class Converter
    {
        public static string ConvertDataTabletoString(DataTable dataTable)
        {
            var loqrItem = new List<LoqrItem>();
            foreach (DataRow row in dataTable.Rows)
            {
                foreach(DataColumn col in dataTable.Columns)
                {
                    loqrItem.Add(new LoqrItem(col.ToString(), row[col].ToString(), typeof(string)));
                }
            }
            return JsonConvert.SerializeObject(loqrItem);
        }

        public static Dictionary<string, string> ConvertURLPayloadToInsertStringArray(string id, string payload)
        {
            Dictionary<string, string> payload_converter = new Dictionary<string, string>();
            payload_converter.Add("id", id);
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
