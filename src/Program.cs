using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GenusFinder;

/*
 * This program is devloped by Gunnar Brådvik (2026).
 * It is available under the GPL-3 license: https://www.gnu.org/licenses/gpl-3.0.en.html
 * If this model or parts of it is used, you are obliged to credite and cite Gunnar Brådvik, https://doi.org/10.56290/27.28845
 * If you have any questions regarding the model, feel free to email me at gunnar@bradvik.se
 */

class Program
{
    /// <summary>
    /// Encoding used to read all input files
    /// </summary>
    public static readonly Encoding ENCODING = Encoding.UTF8;

    private const string _MAIN_DIRECTORY = @"C:\German\";
    // Hardcoded input folders used - if the user does not chose to specify own paths
    private const string _NOUN_LIST_FILE = _MAIN_DIRECTORY + @"Input\Substantive.csv";
    private const string _CORPUS_FILE = _MAIN_DIRECTORY + @"Input\wdd17.i5.xml";
    //private const string _CORPUS_FILE = _MAIN_DIRECTORY + @"Input\Testinput.xml";
    private const string _OUTPUT_FOLDER = _MAIN_DIRECTORY + "Output";

    static void Main()
    {
        // Welcome message
        var fullVersion =
            Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;
        var cleanVersion = fullVersion?.Split('+')[0];
        Console.WriteLine($"Genusfinder version {cleanVersion}. (C) Gunnar Brådvik 2026");

        try 
        {
            // If debugger is attatched - we use default paths
            bool turnOffFolderSelection = Debugger.IsAttached;
            InputSelection(turnOffFolderSelection);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.InnerException.ToString());
            Console.ReadKey();
        } // end catch

        var endTime = DateTime.Now.ToString(@"yyyy-MM-dd hh\:mm\:ss");
        Console.Write(endTime + ". Done! Press any key to close the application...");
        Console.ReadKey();
        Environment.Exit(0);
    } // end method

    private static void InputSelection(bool turnOffFolderSelection)
    {
        // See if the user wants to specify input and output files or just want to use the hardcoded paths
        string input = "";
        while (!input.Equals("y", StringComparison.CurrentCultureIgnoreCase) &&
               !input.Equals("n", StringComparison.CurrentCultureIgnoreCase) &&
               !turnOffFolderSelection) 
        {
            Console.Write("\nDo you want to specify folder paths? If you select no, the hardcoded paths will be used. (Y/N): ");
            input = Console.ReadLine();
        }

        // Define path variables
        string nounListFile = default;
        string corpusFile = default;
        string outputFolder = default;
        DateTime startTime;

        // Use default paths
        if (input.Equals("n", StringComparison.CurrentCultureIgnoreCase) ||
            turnOffFolderSelection) 
        {
            startTime = DateTime.Now;
            nounListFile = _NOUN_LIST_FILE;
            corpusFile = _CORPUS_FILE;
            // Create unique output folder based on time (time suffix added)
            string timeStamp = startTime.ToString("yyyy-MM-dd_HH-mm-ss");
            outputFolder = _OUTPUT_FOLDER + " " + timeStamp + @"\";
        }
        // Specify file paths
        else 
        {
            // Noun file list
            bool validNounFileList = false;
            while (!validNounFileList) 
            {
                Console.Write("\n\nSpecify the path of the .csv file with the nouns that are to be investigated: ");
                nounListFile = Console.ReadLine();
                bool exists = FileExists(nounListFile);
                if (exists)
                    validNounFileList = Path.GetExtension(nounListFile) == ".csv";
            }

            // Corpus file
            bool validCorpusFile = false;
            while (!validCorpusFile) 
            {
                Console.Write("\n\nSpecify the path of the .xml corpus file that is to be used: ");
                corpusFile = Console.ReadLine();
                bool exists = FileExists(corpusFile);
                if (exists)
                    validCorpusFile = Path.GetExtension(corpusFile) == ".xml";
            }

            // Output folder
            bool validOutputFolder = false;
            while (!validOutputFolder) 
            {
                Console.Write("\n\nSpecify the path of the output folder (it will be created it it does not exist): ");
                outputFolder = Console.ReadLine();
                try 
                {
                    ValidatePath(outputFolder);
                    // If the last char is not \, add it to the folder string
                    if (outputFolder.Last() != '\\')
                        outputFolder += @"\";
                    validOutputFolder = true;
                }
                catch (Exception) 
                {
                    Console.Write("Invalid output path.");
                } // end try
            } // end while
            startTime = DateTime.Now;
        } // end if

        // Search and analyse the file
        SearchAndAnalyse(startTime, nounListFile, corpusFile, outputFolder);
    }

    private static bool FileExists(string path)
    {
        if (File.Exists(path)) 
        {
            return true;
        }
        else 
        {
            Console.Write("The specified file does not exist!");
            return false;
        }
    }

    private static void ValidatePath(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path).Delete();
    }

    private static void SearchAndAnalyse(DateTime startTime, string nounListFile, string corpusFile, string outputFolder)
    {
        const string DUBUG_OUTPUT_FOLDER_NAME = "Debug output";
        string debugOutputFolder = outputFolder + DUBUG_OUTPUT_FOLDER_NAME + @"\";

        // Create the folders if they do not exists
        System.IO.Directory.CreateDirectory(outputFolder);
        System.IO.Directory.CreateDirectory(debugOutputFolder);

        // search corpus and analyse gender
        SearchCorpus(nounListFile, corpusFile, outputFolder);
        AnalyseGender(outputFolder, debugOutputFolder); // from the same path as the outputfiles are produced in searchcorpus()

        // time statistics for the user
        TimeSpan timeElapsed = DateTime.Now - startTime;
        Console.WriteLine("Run time: " + timeElapsed.ToString(@"hh\:mm\:ss") + " (h)");
    }

    /// <summary>
    /// Searches the corpus for the nouns in the Noun list and exports the lines where they occure to a csv file for each of the nouns.
    /// </summary>
    /// <param name="nounListFile"></param>
    /// <param name="corpusFile"></param>
    /// <param name="outputFolder"></param>
    private static void SearchCorpus(string nounListFile, string corpusFile, string outputFolder)
    {
        // load the nouns that are going to be investigated to a lilst
        Console.WriteLine("\nLoading the list with the nouns to be investigated...");
        NounImporter nounFile = new(nounListFile);
        IReadOnlyCollection<string> nouns = nounFile.Nouns;
        Console.WriteLine("Noun list successfully loaded!\n");

        // load the corpus data and search for the nouns in the corpus
        CorpusSearcher corpusSearcher = new(nouns, corpusFile, outputFolder);
        corpusSearcher.RunCorpusSearch();
        Console.WriteLine("\nFinished searching the corpus!\n");
    }

    /// <summary>
    /// Analyse the assigned gender in a specific folder with the .csv files with the occurences of the nouns.
    /// </summary>
    /// <param name="folderToAnalyse">The folder with the .csv files with the occurences of the nouns</param>
    /// <param name="outputFolder">Where the output for each of the nouns is stored</param>
    private static void AnalyseGender(string folderToAnalyse, string outputFolder)
    {
        Console.WriteLine("Starting the analysis of the files, this will take a while...");
        Console.WriteLine("\n");

        FolderAnalyser Gender = new(outputFolder);
        Gender.AnalyseFolder(folderToAnalyse);

        Console.WriteLine("\n");
        Console.WriteLine("Analysis done! The output has been exported to " + folderToAnalyse);
    }
}