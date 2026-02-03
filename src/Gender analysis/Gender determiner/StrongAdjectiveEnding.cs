using System;
using System.Linq;

namespace GenusFinder;

/// <summary>
/// Checks if the word before the noun is a strong adjective ending, like in "schöner Tee" or "schöne Katze".
/// </summary>
public class StrongAdjectiveEnding : GenderDeterminer
{
    public StrongAdjectiveEnding(LineAndPositionData analysisData, Verbs verbs, ContextData contexData) :
        base(analysisData, verbs, contexData)
    {

    }

    public override (string outcome, string method) OutcomeGenderDeterminer()
    {
        // Use the class name as the "method" label
        string methodName = GetType().Name;

        // Get the last char of the noun as written, ignoring punctuation at the end
        char lastNounCharAsWritten = _analysisData.NounAsWritten.Last();
        if (_analysisData.NounAsWritten.Length >= 2 &&
            _endOfSentencePunctuation.Contains(lastNounCharAsWritten) ||
            lastNounCharAsWritten == ')' ||
            lastNounCharAsWritten == ']' ||
            lastNounCharAsWritten == '}')
            lastNounCharAsWritten = _analysisData.NounAsWritten[^2];
        char lastNounChar = _analysisData.LastNounChar;

        // 1. Non-feminine if preceded by genitive article "des" or "eines" and ending on -s
        bool nonFemFenitiveArticle =
            _contextData.TwoWordsBefore == "des" ||
            _contextData.TwoWordsBefore == "eines";
        string wordBeforeTwoLastChars = _contextData.WordBeforeTwoLastChars;
        if (wordBeforeTwoLastChars == "en" && lastNounCharAsWritten == 's' && lastNounChar != 's' && 
            (nonFemFenitiveArticle || _contextData.TwoWordsBefore == "wegen"))
            return (NON_FEM, methodName);


        // 2. Plural noun → cannot determine, as in "spezielle Tees"
        if ((lastNounCharAsWritten == 'e' && lastNounChar != 'e') ||
            (lastNounCharAsWritten == 'r' && lastNounChar != 'r') || 
            (lastNounCharAsWritten == 's' && lastNounChar != 's') || 
            (lastNounCharAsWritten == 'n' && lastNounChar != 'n'))
            return (CANNOT_DETERMINE, methodName);

        // 3. Dative plural: mit schönen Enden → cannot determine
        if (lastNounCharAsWritten == 'n' &&
            WordsToDetermineGender.PrepositionsWithDative.Contains(_contextData.TwoWordsBefore))
            return (CANNOT_DETERMINE, methodName);

        // 4. Capitalized word before (geographic adjective etc.) or "die/der" in special roles
        //    Exception: only if there IS a two-words-before token; otherwise we still analyze
        if ((_contextData.WordBeforeStartsCapital ||
             _contextData.WordBefore == "die" ||
             _contextData.WordBefore == "der") &&
            !string.IsNullOrEmpty(_contextData.TwoWordsBefore))
            return (CANNOT_DETERMINE, methodName);

        // 5. Strong -e adjective ending → feminine unless clearly a relative clause ("der/die/das" + comma)
        if (_contextData.WordBeforeLastChar == 'e' &&
            _analysisData.LastNounChar == 'e') 
        {
            bool possibleRelativePronoun =
                (_contextData.TwoWordsBefore == "der" ||
                 _contextData.TwoWordsBefore == "die" ||
                 _contextData.TwoWordsBefore == "das") &&
                 _contextData.ThreeWordsBeforeLastChar == ',';

            // OK, der weiße Tee → cannot determine (relative vs. article)
            if (possibleRelativePronoun) 
                return (CANNOT_DETERMINE, methodName);
            else 
                if (_contextData.TwoWordsBefore != null &&
                    _contextData.TwoWordsBefore.StartsWith("dies", 
                                                           StringComparison.OrdinalIgnoreCase))
                    // Dieser gekochte Tee
                    return (NON_FEM, methodName + " 'dies-' case");
                else
                    return (FEM, methodName);
        }

        // 6. -em / -es / -en (but not dative plural)
        if (_contextData.WordBeforeTwoLastChars == "em" ||
            _contextData.WordBeforeTwoLastChars == "es" ||
            (_contextData.WordBeforeTwoLastChars == "en" && lastNounCharAsWritten != 'n')) // not dative plural
        {
            // Ich rede mit einer tollen Hexe.
            string twoWordsBefore = _contextData.TwoWordsBefore;
            string twoWordsBeforeTwoLastChars =
                !string.IsNullOrEmpty(twoWordsBefore) && twoWordsBefore.Length > 1
                    ? twoWordsBefore[^2..]
                    : null;

            if (twoWordsBeforeTwoLastChars == "er" &&
                WordsToDetermineGender.PrepositionsWithDative.Contains(_contextData.ThreeWordsBefore))
                return (FEM, methodName);
            else
                return (NON_FEM, methodName);
        }

        // 7. -er adjective ending (but not article "der")
        if (_contextData.WordBeforeTwoLastChars == "er" &&
            _contextData.WordBefore != "der") 
        {
            // 5a. Genitive with capital word before, no sentence-boundary punctuation around
            bool twoBeforeIsCapitalNoBoundary =
                _contextData.TwoWordsBeforeStartCapital &&
                !_endOfSentencePunctuation.Contains(_contextData.TwoWordsBeforeLastChar) &&
                !_endOfSentencePunctuation.Contains(_contextData.ThreeWordsBeforeLastChar);

            bool adverbOfPlace =
                _contextData.TwoWordsBefore == "hier" ||
                _contextData.TwoWordsBefore == "dort" ||
                _contextData.TwoWordsBefore == "überall" ||
                _contextData.ThreeWordsBefore == "in"; // in Atlantis schwarzer Kaffee

            // Da Vivaldi ein Begräbnis dritter Klasse erhielt → genitive
            if (twoBeforeIsCapitalNoBoundary && !adverbOfPlace)
                return (FEM, methodName);

            // 5b. Linear search backwards for prepositions / boundaries
            string gender = null;

            for (int i = _analysisData.NounPosition - 1; i >= 0; i--) 
            {
                /**
                 * Load data at position i.
                 * Don't reset the bool if the new word-before is not capital or is the first word.
                 * Stop at:
                 *   - start of sentence (word before becomes default),
                 *   - "und",
                 *   - a comma right before the noun, etc.
                 */
                if (i < 0)
                    break;

                LoadContextData(i);

                if (_contextData.WordBefore == null ||
                    _contextData.WordBefore == "und" ||
                    _contextData.WordBeforeLastChar == ',')
                    break;

                if (WordsToDetermineGender.Prepositions.Contains(_contextData.WordBefore) ||
                    _contextData.WordBefore == "wegen") 
                {
                    // -er cannot mark non-fem in either accusative or dative, so preposition → fem
                    gender = FEM;
                    break;
                }

                if (_contextData.WordBeforeStartsCapital) 
                {
                    // Ich bin schwedischer Abstammung → genitive/dative; treat as non-fem
                    gender = NON_FEM;
                    break;
                }

                // Don't cross these boundaries
                if (!_endOfSentencePunctuation.Contains(_contextData.WordBeforeLastChar))
                    break;
            }

            // We have not found anything marking dative or something that marks it as cannot determine thus it is a sentence like:
            // "Starker Tee ist schön."
            if (string.IsNullOrEmpty(gender))
                gender = NON_FEM;

            return (gender, methodName);
        }

        // 8. Default: cannot determine in this method
        return (CANNOT_DETERMINE, methodName);
    }
}
