using CsvHelper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenusFinder;

/// <summary>
/// Reads a file line by line and stores the lines in a list.
/// </summary>
public class FileReader
{
    /// <summary>
    /// Suffixes that are allowed for the noun. The noun might end with a suffix like "s" or "n", like in "des Endes" or "Enden", but we still want to analyse the noun as "Ende".
    /// </summary>
    public static readonly IReadOnlyList<string> AcceptedEndings = new[]
    {
        "", "s", "en", "n",                    // Genitive and dative endings
        "\"", "„", ".", "..", "...", ",", ":", ";", "!", "!!", "!!!",
        "?", "??", "???", "?!?", "!?!",       // punctuation marks
        ")", "]",                              // Parentheses and brackets
        "s.", "en.", "n."
    };

    /// <summary>
    /// the corpus data line by line
    /// </summary>
    private readonly List<string> _corpusData = new();
    /// <summary>
    ///  Noun, the lines on which it occures
    /// </summary>
    private readonly ConcurrentDictionary<string, List<int>> _nounLines = new();

    /// <summary>
    /// Opens a file and reads it line by line. The lines are stored in the _corpusData list.
    /// </summary>
    /// <param name="filePath"></param>
    public void OpenFile(string filePath)
    {
        // looping each line in the file
        using (StreamReader sr = new(filePath, Program.ENCODING))
        {
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                // cleaning up the data a bit by removing HTML code
                if (line.Contains(">") | line.Contains("<"))
                    line = Regex.Replace(line, "<.*?>", string.Empty);
                if (line != "" & line != " " & line != "  ") // not adding empty lines
                {
                    line = line.Replace('"', '„'); // remove quotation marks
                    _corpusData.Add(line);
                } // end if
            } // end while
        } // end using
    } // end OpenFile

    /// <summary>
    /// this returns
    /// </summary>
    /// <returns>The number of lines for the whole corpus that has been loaded. Note that it is used when we load a corpus for a specific item which occurences have already been exported by the class, thus the retun is the number of lines for a specific Noun</returns>
    public int GetNumbeOfLines() => _corpusData.Count;

    /// <summary>
    /// Get the content of a specific line- 
    /// </summary>
    /// <param name="line"></param>
    /// <returns>Returns the line at the given _index</returns>
    public string GetLine(int line) => _corpusData[line];

    public void FindAllLinesForWord(string searchWord)
    {
        if (string.IsNullOrWhiteSpace(searchWord))
            return;

        // If we already computed this word, skip
        if (_nounLines.ContainsKey(searchWord))
            return;

        var foundLines = new ConcurrentBag<int>();

        Parallel.For(0, _corpusData.Count, i => {
            string line = _corpusData[i];
            var words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var rawWord in words) 
            {
                string word = NormalizeWord(rawWord);

                // If the word is not the search word, go on
                if (word != searchWord)
                    continue;

                // Must start with uppercase to be considered a noun
                if (!char.IsUpper(word[0]))
                    continue;

                if (MatchesSearchWord(word, searchWord)) 
                {
                    foundLines.Add(i);
                    break; // no need to look further in this line
                }
            }
        });

        // Sort for deterministic order
        var orderedLines = foundLines.Distinct().OrderBy(x => x).ToList();

        // Add to dictionary (if someone else added it in the meantime, this will just fail silently)
        _nounLines.TryAdd(searchWord, orderedLines);
    }

    /// <summary>
    /// Remove allowed trailing punctuation characters, e.g. "Ende." -> "Ende"
    /// </summary>
    private static string NormalizeWord(string word)
    {
        try 
        {
            if (string.IsNullOrEmpty(word))
                return word;

            // get the last letter of the Noun to check if it is a :, a ; or ] we cannot analayse Kriegsende. -> nde. if we take the four last letters including "."
            string lastLetter = string.Concat(word.TakeLast(1));

            // remove the last letter if it is a punctuation mark: Ende. -> Ende, Ende? -> Ende, Ende?!? -> Ende
            while (AcceptedEndings.Contains(lastLetter) && 
                   word.Length > 0) 
            { 
                word = word.Remove(word.Length - 1); 
                lastLetter = string.Concat(word.TakeLast(1)); 
            } // end while

            return word;
        }
        catch 
        {
            return word;
        }
    }

    /// <summary>
    /// Check whether the word ends with the searchWord (case-insensitive) or
    /// its plural/genitive forms (searchWord + "n" / searchWord + "s").
    /// </summary>
    private static bool MatchesSearchWord(string word, string searchWord)
    {
        // direct match at end
        if (word == searchWord || 
            word.EndsWith(searchWord, StringComparison.CurrentCultureIgnoreCase))
            return true;

        // plural ending: "Enden"
        else if (word.EndsWith(searchWord + "n", StringComparison.CurrentCultureIgnoreCase))
            return true;

        // genitive (or plural) ending: "Endes"
        else if (word.EndsWith(searchWord + "s", StringComparison.CurrentCultureIgnoreCase))
            return true;

        // If we want to add more endings, we can do it here (e.g., "en", "es", "r", etc.)

        else
            return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchWord"></param>
    /// <returns>How many times a Noun occures</returns>
    public int WordCount(in string searchWord) => _nounLines[searchWord].Count;

    /// <summary>
    /// Export all the lines where a Noun occures to a csv file.
    /// </summary>
    /// <param name="searchWord"></param>
    /// <param name="filePath"></param>
    public void ExportLines (in string searchWord, in string filePath)
    {
        using (var textWriter = new StreamWriter(filePath, false, Program.ENCODING))
        {
            var outputFile = new CsvWriter(textWriter, CultureInfo.InvariantCulture);
            //writer.Configuration.Delimiter = ","; // "," is a very problematic deliminater since we have "," in the lines as separators, but since we put each line on a seperate line it does not matter
            foreach (int lineNumber in _nounLines[searchWord])
            {
                string lineContent = _corpusData[lineNumber];
                outputFile.WriteField(lineContent);
                outputFile.NextRecord();
            } // end foreach
        } // end using
    } // end ExportLines

    /// <summary>
    /// Export all the words and the number of times they occure in the corpus to a csv file.
    /// </summary>
    /// <param name="filePath"></param>
    public void ExportWordCount(in string filePath)
    {
        using (var textWriter = new StreamWriter(filePath, false, Program.ENCODING))
        {
            var outputFile = new CsvWriter(textWriter, CultureInfo.InvariantCulture);

            // getting each item and the times it occures in the corpus
            for (int i = 0; i < _nounLines.Count; i++)
            {
                string word = _nounLines.ElementAt(i).Key;
                int occurences = WordCount(word);

                outputFile.WriteField(word);
                outputFile.WriteField(occurences);
                outputFile.NextRecord();
            } // end for
        } // end using
    } // end ExportWordCount
} // end class