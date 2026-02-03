using System;
using System.Linq;

namespace GenusFinder;

/// <summary>
/// Contains all the data for the specific line in the file (with the Noun) at the specific position that we need in the _analysisMethods for our analysis
/// </summary>
public readonly struct ContextData
{
    // Word before
    public string WordBefore { get; }
    public bool WordBeforeStartsCapital { get; }
    public string WordBeforeTwoLastChars { get; }
    public char WordBeforeLastChar { get; }

    // Two words before
    public string TwoWordsBefore { get; }
    public bool TwoWordsBeforeStartCapital { get; }
    public char TwoWordsBeforeLastChar { get; }

    // Three words before
    public string ThreeWordsBefore { get; }
    public bool ThreeWordsBeforeStartCapital { get; }
    public char ThreeWordsBeforeLastChar { get; }

    // One word after
    public bool WordAfterStartsCapital { get; }

    /// <summary>
    /// Creates a ContextData instance exactly like LoadContextData() populates the fields.
    /// </summary>
    public static ContextData CreateDataFromAnalysisPosition(
        int pos,
        Func<int, string, string> getWord,
        Func<string, string> wordWithoutSpecialChars)
    {
        // Detect sentence boundary → wipe all data
        bool sentenceBoundary =
            pos > 0 &&
            wordWithoutSpecialChars(getWord(pos - 1, null)).Length > 1 &&
            getWord(pos - 1, null).Last() == '.' &&
            Char.IsUpper(getWord(pos, "caseSensitive").FirstOrDefault());

        if (sentenceBoundary) 
        {
            return new ContextData(
                default, false, default, default,
                default, false, default,
                default, false, default,
                false
            );
        }

        // -------- WORD BEFORE --------
        string wordBefore = wordWithoutSpecialChars(getWord(pos - 1, null));
        bool wordBeforeStartsCapital = false;
        char wordBeforeLastChar = default;
        string wordBeforeTwoLastChars = default;

        if (wordBefore != null) 
        {
            wordBeforeStartsCapital = Char.IsUpper(getWord(pos - 1, "caseSensitive").FirstOrDefault());
            wordBeforeLastChar = wordBefore.Length > 1 ? wordBefore[^1] : default;
            wordBeforeTwoLastChars = wordBefore.Length > 1 ? wordBefore[^2..] : default;
        }

        // -------- TWO WORDS BEFORE --------
        string twoWordsBefore = wordWithoutSpecialChars(getWord(pos - 2, null));
        bool twoWordsStartsCapital = false;
        char twoWordsLastChar = default;

        if (twoWordsBefore != null) 
        {
            twoWordsStartsCapital = Char.IsUpper(getWord(pos - 2, "caseSensitive").FirstOrDefault());
            twoWordsLastChar = twoWordsBefore.Length > 0 ? twoWordsBefore[^1] : default;
        }

        // -------- THREE WORDS BEFORE --------
        string threeWordsBefore = getWord(pos - 3, null);
        bool threeWordsStartsCapital = false;
        char threeWordsLastChar = default;

        if (threeWordsBefore != null) 
        {
            threeWordsStartsCapital = Char.IsUpper(getWord(pos - 3, "caseSensitive").FirstOrDefault());
            threeWordsLastChar = threeWordsBefore.Length > 0 ? threeWordsBefore[^1] : default;
        }

        // -------- WORD AFTER --------
        string wordAfter = getWord(pos + 1, "caseSensitive");
        bool wordAfterStartsCapital = wordAfter != null &&
                                      Char.IsUpper(wordAfter.FirstOrDefault());

        return new ContextData(
            wordBefore, wordBeforeStartsCapital, wordBeforeTwoLastChars, wordBeforeLastChar,
            twoWordsBefore, twoWordsStartsCapital, twoWordsLastChar,
            threeWordsBefore, threeWordsStartsCapital, threeWordsLastChar,
            wordAfterStartsCapital
        );
    }

    private ContextData(
        string wordBefore,
        bool wordBeforeStartsCapital,
        string wordBeforeTwoLastChars,
        char wordBeforeLastChar,

        string twoWordsBefore,
        bool twoWordsBeforeStartsCapital,
        char twoWordsBeforeLastChar,

        string threeWordsBefore,
        bool threeWordsBeforeStartsCapital,
        char threeWordsBeforeLastChar,

        bool wordAfterStartsCapital)
    {
        WordBefore = wordBefore;
        WordBeforeStartsCapital = wordBeforeStartsCapital;
        WordBeforeTwoLastChars = wordBeforeTwoLastChars;
        WordBeforeLastChar = wordBeforeLastChar;

        TwoWordsBefore = twoWordsBefore;
        TwoWordsBeforeStartCapital = twoWordsBeforeStartsCapital;
        TwoWordsBeforeLastChar = twoWordsBeforeLastChar;

        ThreeWordsBefore = threeWordsBefore;
        ThreeWordsBeforeStartCapital = threeWordsBeforeStartsCapital;
        ThreeWordsBeforeLastChar = threeWordsBeforeLastChar;

        WordAfterStartsCapital = wordAfterStartsCapital;
    }
}