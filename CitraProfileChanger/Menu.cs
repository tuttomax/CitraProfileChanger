using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CitraProfileChanger
{
    public partial class Menu
    {
        private int _selectedIndex = 0;
        
        private readonly List<Entry> _entries;
        private int MaxIndex => _entries.Count - 1;
        public string Title { get; }
        public bool Closed { get; private set; }
        public Entry EntrySelected { get; private set; }
        public Menu(string title,IEnumerable<Entry> collection)
        {
            _entries = new List<Entry>(collection);
            Title = title;
        }

        private void MoveUp() => _selectedIndex = (_selectedIndex - 1 < 0) ? MaxIndex : _selectedIndex - 1;
        private void MoveDown() => _selectedIndex = (_selectedIndex + 1 > MaxIndex) ? 0 : _selectedIndex + 1;

        private void DisplayEntries()
        {
           
            for (var i = 0; i < _entries.Count; i++)
            {
                Console.WriteLine($"[{(_selectedIndex == i ? "X" : " ")}] {_entries.ElementAt(i).Name}");
            }
        }

        

        public bool LoopSelected()
        {
            Console.Clear();
            Console.WriteLine(Title);

            var left = Console.CursorLeft;
            var top = Console.CursorTop;
           
            DisplayEntries();
            ConsoleKey key;
            while ((key = Console.ReadKey(true).Key) != ConsoleKey.Q)
            {
                Console.SetCursorPosition(left,top);
                

                if (key == ConsoleKey.UpArrow) MoveUp();
                else if (key == ConsoleKey.DownArrow) MoveDown();
                
                else if (key == ConsoleKey.Enter)
                {
                    EntrySelected = _entries[_selectedIndex];
                    return true;
                }
                else if (key == ConsoleKey.Backspace) return (Closed = false);
                DisplayEntries();
            }
            Console.SetCursorPosition(0,0);
            
            return (Closed = true);
        }

    }
}
