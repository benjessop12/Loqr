using System;

namespace Loqr
{
    public class LoqrItem
    {
        public LoqrItem(string name, string value, Type type)
        {
            this.FieldName = name;
            this.FieldValue = value;
            this.FieldType = type;
        }

        public string FieldName;

        public string FieldValue;

        public Type FieldType;
    }
}
