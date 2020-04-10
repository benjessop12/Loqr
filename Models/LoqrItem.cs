using System;

namespace Loqr
{
    public class LoqrItem
    {
        public LoqrItem(long id,  string name, string value, Type type)
        {
            this.Id = id;
            this.Key = name;
            this.Value = value;
            this.Type = type;
        }

        public long Id;

        public string Key;

        public string Value;

        public Type Type;
    }
}
