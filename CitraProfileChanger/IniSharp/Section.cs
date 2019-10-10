

using System;
using System.Collections.Generic;

namespace CitraProfileChanger.IniSharp
{
    class Section
    {
        public string Key { get; set; }
        public List<Pair> Pairs { get; set; }

        public Section()
        {
            Pairs = new List<Pair>();
        }
        public Section(string key) : this()
        {
            Key = key.Trim();
        }

        public Section(string key, List<Pair> pairs)
        {
            Key = key;
            Pairs = pairs;
        }

        public override string ToString()
        {
            return $"Key: {Key} Pairs: Count = {Pairs.Count}";
        }
    }
}
