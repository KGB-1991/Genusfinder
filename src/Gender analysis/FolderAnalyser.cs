/*  
    1. analyses the folder
        2. analyses each file in the folder one by one
            3. analyses each line in the file and loads the line data for the specific line (not position yet, a noun might occure more than once on a line). 
                4. analyses each position (occurence) of the Noun on the specific line in 3) - like in "Das schöne Ende was ein gutes Ende", 
                    we have >1 occurences of the Noun on the same line. 
                    Load the LoadAnalysisDataForGenderPosition fot the extra data needed for the specific position in the loop
                5. stores the output for each line
        6. saves the output for the analysis fo the whole folder

 We assume that the user uses the same foundGender on the whole line and breaks the loop once we have found one foundGender

 Note that this code does not support Umlaut plurals of the noun like "Äpfel"
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Reflection.Metadata.Ecma335;

namespace GenusFinder;

/// <summary>
/// Analysis all foundGender assignments based on an output folder with the corpus data.
/// </summary>
public class FolderAnalyser
{
    private const string _GENDER_ANALYSIS_FILE_NAME = @"\000 Gender Analysis.csv";

    /// <summary>
    /// Same for all the words that we analyse
    /// </summary>
    private static readonly Verbs _verbs = new();
    /// <summary>
    /// Noun, foundGender (Fem, Non Fem or "Cannot determine"), number of occurences - for the output to print out to csv
    /// </summary>
    private readonly ConcurrentDictionary<string, Dictionary<string, int>> _nounGender = new();

    /// <summary>
    /// debug data output for each word with the foundGender assigned
    /// </summary>
    private readonly string _nounOccurrencesFolder;
    /// <summary>
    /// index for keeping tack in the output files each outcome that we analyse will get a unique index
    /// </summary>
    private int _index = 1;

    private int _nFiles;
    private int _currentFile = 0;

    public FolderAnalyser(string nounOccurrencesFolder) => _nounOccurrencesFolder = nounOccurrencesFolder;

    /// <summary>
    /// Analyses a folder with files containing the corpus data. The files are analysed one by one and the results are stored in a csv file.
    /// </summary>
    /// <param name="folderPath"></param>
    public void AnalyseFolder(string folderPath) 
    {
        // Delete old gender analysis file
        File.Delete(folderPath + _GENDER_ANALYSIS_FILE_NAME);

        // Delete the old debug and output files since we append to them in this code
        DirectoryInfo di = new(_nounOccurrencesFolder);
        foreach (FileInfo file in di.GetFiles())
            file.Delete();

        // Analysing each file in the folder using one thread per task
        string[] filePaths = Directory.GetFiles(folderPath, "*.csv", SearchOption.TopDirectoryOnly);
        _nFiles = filePaths.Length - 1; // Do not analyse the summary file

        Parallel.ForEach(filePaths, filePath => 
        {
            if (Path.GetFileNameWithoutExtension(filePath) != Path.GetFileNameWithoutExtension(CorpusSearcher.ALL_WORDS_FILE_NAME)) // Do not analyse the summary file
                AnalyseFile(filePath);
        });

        // save output files in csv with the result
        Console.WriteLine("Saving summary results...");
        SaveSummary(folderPath);
    }

    /// <summary>
    /// Analyses a file and stores the result in the _nounGender dictionary. The result is stored in a csv file.
    /// </summary>
    /// <param name="file"></param>
    private void AnalyseFile(string file)
    {
        _currentFile++;
        int fileNumber = _currentFile; // the file number is based on this particular file, not the sum number of files done

        Console.WriteLine("Analysing the gender assignment of " + Path.GetFileNameWithoutExtension(file) +
            " (file " + fileNumber + "/" + _nFiles + ") ...");

        FileReader fileData = new();
        fileData.OpenFile(file);
        AnalyseLines(file, fileData); // for each line in the file that we have imported

        Console.WriteLine("The analysis of the gender assignment of " + Path.GetFileNameWithoutExtension(file) +
            " (file " + fileNumber + "/" + _nFiles + ") has been completed!\n");
    }

    /// <summary>
    /// Analyse each line in the file and store the result in the _nounGender dictionary. The result is stored in a csv file.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="fileData"></param>
    private void AnalyseLines(string file, FileReader fileData)
    {
        string noun = Path.GetFileNameWithoutExtension(file);

        int numberOfLines = fileData.GetNumbeOfLines();

        // if we do not have any lines the void will do nothing
        if (numberOfLines == 0) 
        {
            Console.WriteLine("The noun " + noun + " does not occur in the corpus data, nothing to analyse.");
            return;
        }
            
        // otherwise: loop each line in the file and add data to StoreResult for each line
        for (int i = 0; i < numberOfLines; i++)
        {
            // Split the line with all the text into words, finds all the positions on the line on which the Noun occurs
            string line = fileData.GetLine(i);
            string[] words = line.Split(' '); 

            // If there is just one word on the line, go to the next line we cannot analyse this line, don't even save the statstics for it
            if (words.Length == 1)
                continue;

            // Find all the positions on the line on which the noun occurs
            int[] nounPositions = words
                .Select((word, index) =>
                    FileReader.AcceptedEndings.Any(suffix => word.EndsWith(noun + suffix)) ? 
                                                   index : -1)
                .Where(index => index != -1)
                .ToArray();

            // If the noun does not occur on this line, go to the next line
            // This can happen if the file contains compound nouns like "Meerhexe" which are not the Noun we are looking for
            // An example why we do not support compound nouns: Committee =/= Tee
            if (nounPositions.Length == 0)
                continue;

            // Check the foundGender for each occerence of the Noun on the line (each value in the array NounPosition) until we either find a foundGender or run out of occerence check, in that case we store "Cannot determine"
            string foundGender = default;
            string methodUsed = default;
            LineAndPositionData analysisData = default;

            for (int j = 0; j < nounPositions.Length; j++)
            {
                // loading the data needed for the analysis at the specific position
                analysisData = LoadAnalysisDataForGenderPosition(nounPositions[j], noun, words);

                // if we do not have any word before the Noun we cannot analyse the foundGender on this line. For instance: "Ende ist schön." there is nothing before "Ende"
                // the other case is that we have the word before the Noun as " " like in " Ende ist schön". Then we have a word before the Noun, " " (blank space), but nothing to analyse
                if (nounPositions[j] == 0 || words[nounPositions[j] - 1].Length == 0)
                {
                    foundGender = GenderAssignment.CANNOT_DETERMINE;
                    methodUsed = "No word before the noun on the this position on the line to analyse the gender.";

                    // if there is no other Noun on this line in the file, and the loop has not already been broken,
                    // we have to give up this line and store this line as "Cannot determine"
                    // for instance a file with only: "Ende ist schön."
                    if (j == nounPositions.Length - 1)
                        break;
                }
                // We have a word before the noun on the line
                else 
                {
                    // Analyse the gender at the specific position
                    GenderAssignment genderAnalysis = new(analysisData, _verbs, null);
                    var resultAtPosition = genderAnalysis.OutcomeGenderDeterminer();
                    foundGender = resultAtPosition.outcome;
                    methodUsed = resultAtPosition.method;

                    // We assume that the user uses the same foundGender on the whole line and breaks the loop once we have found one foundGender
                    // Or we have run out of positions to check on the line
                    if (foundGender != GenderAssignment.CANNOT_DETERMINE || 
                        j == nounPositions.Length - 1)
                        break; 
                } // end else
            } // end for

            StoreResult(noun,
                        foundGender,
                        methodUsed,
                        analysisData);
        } // end for
    } // end AnalyseLines

    /// <summary>
    /// Load the analysis data for the current position of the noun. This is the data that is specific for the current position of the noun and not the data that is common for the whole line.
    /// </summary>
    /// <param name="nounPosition"></param>
    /// <param name="noun"></param>
    /// <param name="words"></param>
    /// <returns>Tha analyis data for the current position of the noun.</returns>
    public static LineAndPositionData LoadAnalysisDataForGenderPosition(int nounPosition, string noun, string[] words)
    {
        // NounAsWritten - the Noun as it is written on the line (and not as the Noun which is just the Noun in the pure form, like "Ende"), for instance (die Farbe des) "Endes.", with "s" and "."
        string nounAsWritten = words[nounPosition];

        // This is used to check if we have a pharentesis before the Noun like in 'Zitat (Absatz Nahrungsmittel) "Der Tee enthält'
        // Here, we would otherwiese consider this as genitive construction since we have "der" before "Tee", but it is not the case since we have a parenthesis before the Noun
        string wordBeforeAsWritten;
        if (nounPosition - 1 > 0)
            wordBeforeAsWritten = words[nounPosition - 1];
        else
            wordBeforeAsWritten = default;

        string twoWordBeforeAsWritten;
        if (nounPosition - 2 > 0)
            twoWordBeforeAsWritten = words[nounPosition - 2];
        else
            twoWordBeforeAsWritten = default;

        // LastNounChar - if the Noun ends with like "Was für ein Ende..." or "Eines der Enden???" remov the "..." on by one to get the last letter of the Noun
        string lastChar = nounAsWritten[nounAsWritten.Length - 1].ToString();
        int i = 2;

        while (FileReader.AcceptedEndings.Contains(lastChar))
        {
            lastChar = nounAsWritten[nounAsWritten.Length - i].ToString();
            i++;
        }
        char lastNounChar = lastChar.ToCharArray()[0];
        LineAndPositionData analysisData = new(
            noun: noun,
            words: words,
            nounAsWritten: nounAsWritten,
            wordBeforeAsWritten: wordBeforeAsWritten,
            twoWordsBeforeAsWritten: twoWordBeforeAsWritten,
            nounPosition: nounPosition,
            lastNounChar: lastNounChar
            );
        return analysisData;
    }

    /// <summary>
    /// Store the result of the analysis in the _nounGender dictionary.
    /// </summary>
    /// <param name="noun"></param>
    /// <param name="gender"></param>
    /// <param name="methodUsed"></param>
    /// <param name="analysisData"></param>
    private void StoreResult(string noun, string gender, string methodUsed, LineAndPositionData analysisData)
    {
        if (_nounGender.ContainsKey(noun) && _nounGender[noun].ContainsKey(gender)) // Add 1 to the existing key if we have both the key for the Noun and the foundGender
        {
            _nounGender[noun][gender] += 1;
        }
        else if (_nounGender.ContainsKey(noun)) // We have to add the foundGender key, but not the Noun key
        {
            _nounGender[noun].Add(gender, 1);
        }
        else // we have to add both foundGender key and the noun key
        {
            Dictionary<string, int> genderCount = new() 
            {
                { gender, 1 }
            };
            _nounGender.TryAdd(noun, genderCount);
        }

        SaveNounOutput(noun, gender, methodUsed, analysisData);
    }

    /// <summary>
    /// Create an output file with all the info that was used in order to determine the foundGender for a specific noun.
    /// </summary>
    /// <param name="noun"></param>
    /// <param name="gender"></param>
    /// <param name="methodUsed"></param>
    /// <param name="analysisData"></param>
    private void SaveNounOutput(string noun, string gender, string methodUsed, LineAndPositionData analysisData)
    {
        // output statistic specific to the Noun
        string wordBeforeNoun = default;
        bool wordBeforeNounStartsCapital = default;
        char lastCharBeforeNoun = default;
        string twoWordsBeforeNoun = "NA";

        if (analysisData.Words.Length >= analysisData.NounPosition && analysisData.NounPosition > 0)
        {
            wordBeforeNoun = analysisData.Words[analysisData.NounPosition - 1];
            if (wordBeforeNoun.Length > 0)
            {
                wordBeforeNounStartsCapital = Char.IsUpper(wordBeforeNoun.First());
                lastCharBeforeNoun = wordBeforeNoun[wordBeforeNoun.Length - 1];
            }
            if (analysisData.NounPosition > 1) // this is the only varible that might not exist and not get a default value during a run, like in "Das Ende"
                twoWordsBeforeNoun = analysisData.Words[analysisData.NounPosition - 2];
        }

        string filePath = _nounOccurrencesFolder + noun + " " + gender + ".csv";

        using (StreamWriter textWriter = new(filePath, true, Program.ENCODING))
        {
            CsvWriter outputFile = new(textWriter, CultureInfo.InvariantCulture);

            // _index for keeping tack in the output files each outcome that we analyse will get a unique inex
            outputFile.WriteField("[" + _index + "]");
            outputFile.NextRecord();
            _index++;

            string words = "";
            foreach (string word in analysisData.Words)
                words += " " + word;

            var dataUsedInTheAnalysis = new List<(string caption, string val)>
            {
                ("Method used to determine the gender: ", methodUsed),
                ("Noun as written: ", analysisData.NounAsWritten),
                ("Two words before noun: ", twoWordsBeforeNoun),
                ("Word before noun: ", wordBeforeNoun),
                ("Word before noun starts with a capital letter: ", wordBeforeNounStartsCapital.ToString()),
                ("Last noun char: ", analysisData.LastNounChar.ToString()),
                ("Larst char before noun: ", lastCharBeforeNoun.ToString()),
                ("Words (noun at position " + analysisData.NounPosition.ToString() + ") :", words)
            };

            foreach (var output in dataUsedInTheAnalysis)
            {  
                outputFile.WriteField(output.caption + output.val);
                outputFile.NextRecord();
            }

            outputFile.NextRecord();
            outputFile.NextRecord();
        } // end using
    } // end SaveNounOutput

    /// <summary>
    /// Saves the summary of the analysis in a csv file. The file is saved in the folder where the corpus data is located.
    /// </summary>
    /// <param name="folderPath"></param>
    private void SaveSummary(string folderPath)
    {
        string filePath = folderPath + _GENDER_ANALYSIS_FILE_NAME;
        string[] summaryHeaders = { "Substantiv", "Genus", "Anzahl", "Anteil" };

        using (var textWriter = new StreamWriter(filePath, false, Program.ENCODING)) 
        {
            var outputFile = new CsvWriter(textWriter, CultureInfo.InvariantCulture);

            // headers
            foreach(var header in summaryHeaders)
                outputFile.WriteField(header);

            outputFile.NextRecord();

            // output for each noun
            foreach (var noun in _nounGender.Keys) 
            {
                // count of number foundGender occurences for each foundGender (if they occure at all, otherwise set them to 0)
                int fem = _nounGender[noun].ContainsKey(GenderAssignment.FEM) ? _nounGender[noun][GenderAssignment.FEM] : 0;
                int nonFem = _nounGender[noun].ContainsKey(GenderAssignment.NON_FEM) ? _nounGender[noun][GenderAssignment.NON_FEM] : 0;
                int cannotDetermine = _nounGender[noun].ContainsKey(GenderAssignment.CANNOT_DETERMINE) ? _nounGender[noun][GenderAssignment.CANNOT_DETERMINE] : 0;
                int sum = fem + nonFem + cannotDetermine;

                IReadOnlyList<(string genderOutcome, int n)> outcomeStatistics =
                [
                    (GenderAssignment.FEM, fem),
                    (GenderAssignment.NON_FEM, nonFem),
                    (GenderAssignment.CANNOT_DETERMINE, cannotDetermine)
                ];

                Console.WriteLine("\n\n" + "-----------" + noun + "-----------");

                // add three columns and three lines to a csv, one line for each type of foundGender: "Ende, Fem, 100"
                foreach (var outcome in outcomeStatistics) 
                {
                    double prop = (double)outcome.n / (double)sum; // proportion of sum occurences

                    outputFile.WriteField(noun);
                    outputFile.WriteField(outcome.genderOutcome);
                    outputFile.WriteField(outcome.n);
                    outputFile.WriteField(prop);
                    outputFile.NextRecord();

                    Console.WriteLine(outcome.genderOutcome + ": " + outcome.n + ", " + prop * 100 + " %");
                } // end foreach outcome
            } // end foreach
        } // end using
    } // end SaveSummary
} // end class