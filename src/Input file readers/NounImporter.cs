using System.Collections.Generic;
using System.IO;

namespace GenusFinder;

/// <summary>
/// Creates a list with all the nouns in the input file that contains the nouns we want to investigate
/// </summary>
internal class NounImporter
{
    public List<string> Nouns { get; } // the corpus data line by line

    public NounImporter(string filePath)
    {
        Nouns = new();
        // open csv file and read line by line
        // looping each line in the file https://stackoverflow.com/questions/2161895/reading-large-text-files-with-streams-in-c-sharp
        using (StreamReader sr = new(filePath, Program.ENCODING))
        {
            string noun = string.Empty;
            while ((noun = sr.ReadLine()) != null)
                Nouns.Add(noun);
        } // end using
    } // end constructor
} // end class