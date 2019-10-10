using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using CitraProfileChanger.IniSharp.Exceptions;
using CitraProfileChanger.IniSharp.Extensions;

namespace CitraProfileChanger.IniSharp
{
    internal class IniFile
    {
        [DllImport("kernel32")] private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);


        private static string _path;
        private const string CommentChar = ";";

        private static readonly Regex SectionRegex = new Regex(@"\[(.+?)\]");
        private static readonly Regex PairRegex = new Regex(@"^(.+?)=(.*?)$");

        public List<Section> Sections { get; private set; }

        public IniFile(string path)
        {
            Sections = new List<Section>();
            _path = Path.IsPathRooted(path) ? path : Path.Combine(Environment.CurrentDirectory, path);
            if (File.Exists(_path))
            {
                ProcessFile();
            }
            else
            {
               File.Create(_path).Close();
            }
        }

        public void Write(string section, string key, string newValue)
        {
            WritePrivateProfileString(section, key, newValue, _path);
            ClearAll();
            ProcessFile();
        }

        private void ClearAll()
        {
            Sections = new List<Section>();
        }

        private void ProcessFile()
        {
            using (var reader = new StreamReader(_path))
            {
                string line;
                
                var section = new Section();

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith(CommentChar)) continue;
                    if (String.IsNullOrEmpty(line)) continue;

                    var isSection = SectionRegex.IsMatch(line);
                    var isPair = PairRegex.IsMatch(line);
                    if (!isSection && !isPair) throw new ParsingException();

                    if (isSection)
                    {
                        section = new Section(SectionRegex.GetGroupValue(line, 1));
                    }

                    if (isPair)
                    {
                        var pair = new Pair(PairRegex.GetGroupValue(line, 1), PairRegex.GetGroupValue(line, 2));
                        section.Pairs.Add(pair);
                    }

					if (section.Pairs.Count != 0 && !Sections.Contains(section)) Sections.Add(section);
				}
            }
        }
    }
}
