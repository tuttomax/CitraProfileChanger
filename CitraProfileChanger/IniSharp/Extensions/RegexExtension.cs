using System.Text.RegularExpressions;

namespace CitraProfileChanger.IniSharp.Extensions
{
static class RegexExtension
    {
        public static string GetGroupValue(this Regex regex, string input,int index)
        {
            return regex.Match(input).Groups[index].Value;
        }
    }
}
