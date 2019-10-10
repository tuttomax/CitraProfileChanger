
using System;

namespace CitraProfileChanger.IniSharp
{
    class Pair
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public Pair()
        {
            
        }
        public Pair(string key, string value)
        {
            Key = key.Trim();
            Value = value.Trim();
        }

        public override string ToString()
        {
            return $"{Key}={Value}";
        }
    }
}
