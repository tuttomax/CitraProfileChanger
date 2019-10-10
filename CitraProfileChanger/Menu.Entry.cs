using System;
using System.Collections.Generic;
using System.IO;
using CitraProfileChanger.IniSharp;

namespace CitraProfileChanger
{

    public abstract class Entry
    {
      
        protected Entry(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }


    public sealed class GameEntry : Entry
    {
        public GameEntry(string name,string savePath,string profile) : base(name)
        {
            if (!Directory.Exists(savePath)) throw new DirectoryNotFoundException(savePath);
            if (String.IsNullOrEmpty(profile)) throw new NullReferenceException(nameof(profile));
            SavePath = savePath;
            ActiveProfile = profile;
        }

        public string SavePath { get; }
        public string ActiveProfile { get; }
    }

    public sealed class ProfileEntry : Entry
    {
        public ProfileEntry(string name,IEnumerable<SaveFile> saves) : base(name)
        {
            Saves = new List<SaveFile>(saves);
        }

        public List<SaveFile> Saves { get; } 
        
    }

    
}