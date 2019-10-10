using System;

namespace CitraProfileChanger.IniSharp.Exceptions
{
    internal class ParsingException : Exception
    {
        public ParsingException()
        {
        }

        public ParsingException(string message)
            : base(message)
        {
        }
    }
}

