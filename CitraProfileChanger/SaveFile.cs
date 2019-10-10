using System;
using System.CodeDom;
using System.IO;

namespace CitraProfileChanger
{

    
        public class SaveFile
        {
            private readonly FileInfo _info;

            public SaveFile(string game, string path)
            {
                Game = game;
                Path = path;
                _info = new FileInfo(path);
                if (String.IsNullOrEmpty(game)) throw new NullReferenceException(nameof(game));
              //  if (!_info.Exists) throw new FileNotFoundException("File not found", path);
            }

            public string Game { get; }
            public string Path { get; }

            public bool Exist => _info.Exists;


        public static bool operator <(SaveFile a, SaveFile b)
        {
            return a._info.LastWriteTime < b._info.LastWriteTime;
        }
        public static bool operator >(SaveFile a, SaveFile b)
        {
            return a._info.LastWriteTime > b._info.LastWriteTime;
        }

        public void MoveTo(string dest)
        {
            _info.CopyTo(dest, true);
        }

        public void Backup()
        {
            _info.CopyTo($"{_info.FullName}.backup",true);
        }
    }
    
}