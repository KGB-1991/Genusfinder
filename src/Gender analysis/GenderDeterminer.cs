using System;
using System.Linq;

namespace GenusFinder;
public abstract class GenderDeterminer
{
    public const string FEM = "Fem";
    public const string NON_FEM = "Non fem";
    public const string CANNOT_DETERMINE = "Cannot determine";

    protected static char?[] _endOfSentencePunctuation = { '.', ',', ':', ';', '!', '?' };

    /// <summary>
    /// Contains all the data for the specific line in the file (with the Noun) at the specific position that we need in the _analysisMethods for our analysis
    /// </summary>
    protected readonly LineAndPositionData _analysisData;
    protected static Verbs _verbs;
    protected ContextData _contextData;

    /// <summary>
    /// The specific positoiin of the noun in the data
    /// </summary>
    protected readonly int _nounPosition;

    public abstract (string outcome, string method) OutcomeGenderDeterminer();

    public GenderDeterminer(LineAndPositionData analysisData, Verbs verbs, ContextData? contexData)
    {
        _analysisData = analysisData;
        _verbs = verbs;
        _contextData = contexData ?? new ContextData(); // if null, create an empty context data
        _nounPosition = analysisData.NounPosition;
    }

    /// <summary>
    /// Create an instance of the specified table type using the needed parameters
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="classType"></param>
    /// <returns></returns>
    protected T CreateDeterminerInstance<T>(Type classType)
    {
        object[] parameters = [_analysisData,
                               _verbs,
                               _contextData];
        return (T)Activator.CreateInstance(classType,
                                           parameters);
    }

    /// <summary>
    /// Load the data for the analysis position. This is used to get the word before and after the noun and the two words before it.
    /// The analysis position is reset to _analysisData.NounPosition for each new method we call from GenderAnalyser to the position of the gender
    /// This can be used in order to move stepwise when we try to analysise stuff, we analyse three words before the noun and one word after it
    /// </summary>
    /// <param name="analysisPosition"></param>
    protected void LoadContextData(int analysisPosition)
    {
        _contextData = ContextData.CreateDataFromAnalysisPosition(
            analysisPosition,
            (i, cs) => GetWord(i, cs == "caseSensitive" ? false : true),
            WordWithoutSpecialChars
        );
    }

    /// <summary>
    /// This method is used to get a word at a specific position (could be before the noun for example) in the analysis data.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="caseInSensitive"></param>
    /// <returns>The word or default if we are out of bounds or if we try to get two words before when there is just one</returns>
    private string GetWord(int position, bool caseInSensitive = true)
    {
        string word = default;
        if (position < _analysisData.Words.Length && position >= 0)
            word = WordWithoutSpecialChars(_analysisData.Words[position]);
        return caseInSensitive && word != default ? word.ToLower() : word;
    }

    /// <summary>
    /// Checks if we have a genitive construction like "der Name der Klasse"
    /// </summary>
    /// <returns></returns>
    protected bool PossibleGenitiveConstruction()
    {
        bool twoBeforeValid =
            _contextData.TwoWordsBeforeStartCapital &&
            _contextData.TwoWordsBeforeLastChar != '.';

        bool threeBeforeValid =
            !string.IsNullOrEmpty(_contextData.ThreeWordsBefore) &&
            _contextData.ThreeWordsBeforeLastChar is not ('.' or '?' or '!');

        return twoBeforeValid && threeBeforeValid;
    }

    /// <summary>
    /// Checks if we have a plural construction like "die Katzen"
    /// </summary>
    /// <returns></returns>
    protected bool PluralConstruction()
    {
        char lastNounCharAsWritten = _analysisData.NounAsWritten.Last();
        char lastNounChar = _analysisData.LastNounChar;
        return
            (lastNounCharAsWritten == 'e' && lastNounChar != 'e') ||
            (lastNounCharAsWritten == 'r' && lastNounChar != 'r') ||
            (lastNounCharAsWritten == 's' && lastNounChar != 's') ||
            (lastNounCharAsWritten == 'n' && lastNounChar != 'n');
    }

    /// <summary>
    /// Removes special characters from the word, like quotation marks, brackets and parentheses.
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    private static string WordWithoutSpecialChars(string word)
    {
        // quotation marks like in: "Der Tee ist gut" and parenteses etc.
        char[] charsToRemove = { '"', '„', '\'', '[', '(', ')', ']' };

        // if we have a null, like in two words before not existing, do not change the word
        if (word != default)
            foreach (char c in charsToRemove)
                word = word.Replace(c.ToString(), "");
        return word;
    }
}