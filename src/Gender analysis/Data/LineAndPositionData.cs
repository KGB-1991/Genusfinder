namespace GenusFinder;

/// <summary>
/// This struct is used to store the data for each noun that is found in the corpus at a specific line.
/// </summary>
public readonly struct LineAndPositionData
{
    /// <summary>
    /// The noun that is found in the corpus
    /// </summary>
    public readonly string Noun { get; }
    /// <summary>
    /// The Noun as it is written in the corpus. It might be different from the Noun in the Noun list, for instance "des Endes" -> "Ende"
    /// </summary>
    public readonly string NounAsWritten { get; }
    /// <summary>
    /// This is used to check if we have a pharentesis before the Noun like in 'Zitat (Absatz Nahrungsmittel) "Der Tee enthält'
    /// Here, we would otherwiese consider this as genitive construction since we have "der" before "Tee", but it is not the case since we have a parenthesis before the Noun
    /// </summary>
    public readonly string WordBeforeAsWritten { get; }
    public readonly string TwoWordsBeforeAsWritten { get; }
    /// <summary>
    /// Words in the line where the Noun is found.
    /// </summary>
    public readonly string[] Words { get; }
    public readonly int NounPosition { get; }
    /// <summary>
    /// The Noun might end on -n or -s and thus we might need to check the endning of the Noun
    /// </summary>
    public readonly char LastNounChar { get; }

    public LineAndPositionData(string noun, string nounAsWritten, string wordBeforeAsWritten, 
                               string twoWordsBeforeAsWritten, string[] words, int nounPosition, char lastNounChar)
    {
        Noun = noun;
        NounAsWritten = nounAsWritten;
        Words = words;
        NounPosition = nounPosition;
        LastNounChar = lastNounChar;
        WordBeforeAsWritten = wordBeforeAsWritten;
        TwoWordsBeforeAsWritten = twoWordsBeforeAsWritten;
    }
}