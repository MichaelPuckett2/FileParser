using System;
using System.Collections.Generic;

namespace TextFieldParserFramework
{
    /// <summary>
    /// Configuration used to describe to the parser the type and other derived settings needed to parse a string to a type.
    /// </summary>
    public interface IParseConfiguration 
    {
        /// <summary>
        /// The type this configuration is meant to parse a string into.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Used to show one or more example of how the string may look before parsed.
        /// This is useful when debugging, testing, or deciding which parser to use (if saved).
        /// May be used in exeption throwing as examples of what the string 'could' be vs what it is when the exeption was thrown.
        /// </summary>
        ICollection<string> Examples { get; }

        /// <summary>
        /// Describes the parsing configuration.
        /// </summary>
        string Description { get; }
    }

    public interface IParseConfiguration<T> : IParseConfiguration { }
}
