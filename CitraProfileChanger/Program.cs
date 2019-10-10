using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CitraProfileChanger.IniSharp;

namespace CitraProfileChanger
{
    class Program
    {
        private static readonly IniSharp.IniFile IniFile = new IniFile("profile.ini");

        private static readonly string CitraPath = @"C:\Users\aless\AppData\Local\Citra\canary-mingw\citra-qt.exe";

        private static readonly string info =
            "Press Q for exit. Press BACKSPACE for return in the previous section. Press ENTER for selecting entry\n";

        static void Main(string[] args)
        {

            bool exit = false;
            bool alreadySelected = false;

            do
            {
                string title = info + (!alreadySelected ? "" : "\nYour profile is already active on this game.") + "\nSelect profile:";

                Menu profileMenu = new Menu(title,
                    IniFile.Sections.Where(section => section.Key != "Games" && section.Key != "Active")
                        .Select(section => new ProfileEntry(section.Key,
                            section.Pairs.Select(pair => new SaveFile(pair.Key, pair.Value)))));

                if (!profileMenu.LoopSelected()) return; // Exit anyway with Q or BACKSPACE

                ProfileEntry selectedProfile = profileMenu.EntrySelected as ProfileEntry;

                if (selectedProfile == null) throw new NullReferenceException(nameof(selectedProfile));

                title = new StringBuilder().AppendLine(info).AppendLine($"Profile selected {selectedProfile.Name}")
                    .AppendLine("Select game:").ToString();


                Menu gameMenu = new Menu(title, IniFile.Sections.Find(section => section.Key == "Games").Pairs.Select(
                    pair => new GameEntry(pair.Key, pair.Value,
                        IniFile.Sections.Find(section => section.Key == "Active").Pairs
                            .First(pair1 => pair1.Key == pair.Key).Value)));


                if (gameMenu.LoopSelected())
                {
                    GameEntry gameSelected = gameMenu.EntrySelected as GameEntry;

                    if (gameSelected == null) throw new NullReferenceException(nameof(gameSelected));

                    if (gameSelected.ActiveProfile != selectedProfile.Name)
                    {
                        alreadySelected = false;
                        
                        SaveFile selectedProfileSaveFile =
                            selectedProfile.Saves.Find(save => save.Game == gameSelected.Name);

                        SaveFile mainSaveFile =
                            new SaveFile(gameSelected.Name, Path.Combine(gameSelected.SavePath, "main"));

                        string destFile = IniFile.Sections.Find(section => section.Key == gameSelected.ActiveProfile)
                            .Pairs.First(pair => pair.Key == gameSelected.Name).Value;
                        SaveFile destSaveFile = new SaveFile(gameSelected.Name, destFile);

                        
                        if (!mainSaveFile.Exist) throw new NullReferenceException(nameof(mainSaveFile));
                        

                        destSaveFile.Backup();
                        mainSaveFile.MoveTo(destFile);


                        if (selectedProfileSaveFile.Exist) selectedProfileSaveFile.MoveTo(mainSaveFile.Path);

                        IniFile.Write("Active",gameSelected.Name,selectedProfile.Name);
                    }
                    

                    Process.Start(CitraPath);
                    return;
                }

                exit = gameMenu.Closed;

            } while (!exit);



        }
    }
}
