using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenusFinder;

/// <summary>
/// Searches the corpus for the nouns in the noun list and exports the lines where they occure to a .csv file for each of the nouns.
/// </summary>
internal class CorpusSearcher
{
    public const string ALL_WORDS_FILE_NAME = "000 All words.csv";

    private readonly IReadOnlyCollection<string> _nouns;
    private readonly string _corpusFilePath;
    private readonly string _outputFolder;

    private FileReader _corpusFile;

    private int _nounCount = 0;

    public CorpusSearcher(IReadOnlyCollection<string> nouns, string corpusFilePath, string outputFolder)
    {
        _nouns = nouns;
        _corpusFilePath = corpusFilePath;
        _outputFolder = outputFolder;
    }

    /// <summary>
    /// Import the corpus to memory and search for the nouns in the corpus
    /// </summary>
    public void RunCorpusSearch()
    {
        try 
        {
            // import the corpus to memory
            ImportCorpus();
            Parallel.ForEach(_nouns,
                             noun => {
                                 SearchWord(noun);
                             });
            ExportWordCount();
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.InnerException.ToString());
            Console.Write("\nPress any key to close the program...");
            Console.ReadLine();
            System.Environment.Exit(-1);
        }
    }

    /// <summary>
    /// Import the corpus to memory
    /// </summary>
    private void ImportCorpus()
    {
        _corpusFile = new FileReader();
        Console.WriteLine("Importing the corpus...");
        _corpusFile.OpenFile(_corpusFilePath);
        Console.WriteLine("Corpus with " + _corpusFile.GetNumbeOfLines() + " lines successfully imported!");
        Console.WriteLine("\n");
    }

    /// <summary>
    /// Search for the word in the corpus and export the lines where it occures to a csv file
    /// </summary>
    /// <param name="searchWord"></param>
    private void SearchWord(string searchWord) 
    {
        DateTime startTime = DateTime.Now;

        // search all the lines in the corpus where the word occures
        string timeString = startTime.ToString("HH:mm:ss") + " Searching for the noun " + searchWord + "...";
        Console.WriteLine(timeString);

        _corpusFile.FindAllLinesForWord(searchWord);

        // export the line where the word occures to a csv file
        string exportFile = _outputFolder + searchWord + ".csv";
        _corpusFile.ExportLines(searchWord, exportFile);

        // keep track of the progress
        _nounCount++;
        double progress = (double)_nounCount / _nouns.Count * 100;
        progress = Math.Round(progress, 2);

        int occurences = _corpusFile.WordCount(searchWord);
        string timeElapsed = (DateTime.Now - startTime).ToString(@"mm\:ss");
        string timeStringDone = $"The search for {searchWord} took {timeElapsed} (min).";
        string occurrenceString = $"It occures {occurences} times in the corpus.";
        string searchString = $"All the lines with the noun {searchWord} have been exported to {exportFile}.\n";

        Console.WriteLine(
            "\n" + timeStringDone + " " + occurrenceString + "\n" + searchString +
            $"Noun {_nounCount} of {_nouns.Count} searched. " + $"{progress:0.##} % of the search done.\n"
        );
    }

    /// <summary>
    /// Export the word and word count to a .csv file (the number of lines in the corpus in which they occure)
    /// </summary>
    private void ExportWordCount()
    {
        // export the word count to a csv file
        Console.WriteLine("Counting the occurences for all the nouns and exporting the table to a file...");
        _corpusFile.ExportWordCount(_outputFolder + ALL_WORDS_FILE_NAME);
        Console.WriteLine("Table created and saved at this path: " + _outputFolder + ALL_WORDS_FILE_NAME);
    }
}