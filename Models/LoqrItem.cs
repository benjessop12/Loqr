using System;

namespace Loqr.Models
{
    public class LoqrItem
    {
        public LoqrItem(long id,  string name, string value)
        {
            this.Id = id;
            this.Key = name;
            this.Value = value;
        }

        public long Id;

        public string Key;

        public string Value;
    }
}
