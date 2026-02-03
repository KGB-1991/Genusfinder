using System;
using System.Collections.Generic;
using System.Linq;

namespace GenusFinder;
internal class DefiniteArticle : GenderDeterminer
{
    public DefiniteArticle(LineAndPositionData analysisData, Verbs verbs, ContextData contexData) : 
        base(analysisData, verbs, contexData)
    {

    }

    /// <summary>
    /// Checks if the word before the Noun is a definite article, like der, die, das, den, dem, des.
    /// </summary>
    /// <returns></returns>
    public override (string outcome, string method) OutcomeGenderDeterminer()
    {
        string gender = default;
        bool possibleGenitiveConstruction = PossibleGenitiveConstruction();

        // Get the last char of the noun as written, ignoring punctuation at the end
        char lastNounCharAsWritten = _analysisData.NounAsWritten.Last();
        if (_analysisData.NounAsWritten.Length >= 2 &&
            _endOfSentencePunctuation.Contains(lastNounCharAsWritten) ||
            lastNounCharAsWritten == ')' ||
            lastNounCharAsWritten == ']' ||
            lastNounCharAsWritten == '}')
            lastNounCharAsWritten = _analysisData.NounAsWritten[^2];

        if (_contextData.TwoWordsBeforeLastChar.Equals(',') ||                                   // Leute, die Tee trinken
            (_contextData.TwoWordsBeforeLastChar.Equals(':') && _contextData.TwoWordsBeforeStartCapital) ||    // Leute: die Tee trinken  Leute: die Kaffe kochen....
            (_contextData.TwoWordsBeforeLastChar.Equals(';') && _contextData.TwoWordsBeforeStartCapital))     // But not if we do not have a capital letter before ":" or ";" like in: Persönlich kann ich dazu aber nur sagen: der beschissenst schmeckende Tee der Welt
        {
            gender = CANNOT_DETERMINE;
        }
        else if (_contextData.WordBefore == "die" && _analysisData.LastNounChar == lastNounCharAsWritten)      // Fem sing, if it does not end on it, it is plural
        {
            gender = FEM;
        }
        else if (_contextData.WordBefore == "das" || _contextData.WordBefore == "dasselbe" ||
                 _contextData.WordBefore == "dem" || _contextData.WordBefore == "des" ||
                (_contextData.WordBefore == "den" && !_analysisData.LastNounChar.Equals('n')))        // Enden = dativ plural, not ackusativ, thus we have to check last char...
        {
            gender = NON_FEM;
        }
        else if ((_contextData.WordBefore == "der" ||
                  _contextData.WordBefore == "derselbe") &&
                 !possibleGenitiveConstruction) // could be dativ Fem, partitative or mask
        {
            // mit der Frau -> Fem, in der Klasse -> Fem, or der Name der Klasse                
            if (WordsToDetermineGender.Prepositions.Contains(_contextData.TwoWordsBefore)) 
            {
                gender = FEM;
            }
            // check two words before instead of one word line in eines der Enden too see if we have a partitative "der"
            else 
            {
                LoadContextData(_analysisData.NounPosition - 1);
                var methods = new List<GenderDeterminer>
                {
                    CreateDeterminerInstance<IndefiniteArticle>(typeof(IndefiniteArticle)),// einer der Russen -> Fem, eines der Enden -> neutrum, mit einem der Enden -> neutrum/mask...
                    CreateDeterminerInstance<NegationPronoun>(typeof(NegationPronoun)), // keiner der Russen -> Fem, keines der Enden -> neutrum...
                };

                foreach (var method in methods) 
                {
                    (string gender, string method) outcome = method.OutcomeGenderDeterminer();
                    // We found a method that determines the gender
                    if (outcome.gender != CANNOT_DETERMINE)
                        return outcome;
                }

                // if we do not get a hit above -> it is non Fem 
                return (NON_FEM, "Definite article");
            } // end else
        } // end else if
        // Check if we have an adjective ending before the noun and a definite article there
        else if (_contextData.WordBeforeLastChar.Equals('e')) // Weak adjective ending
        {
            if (_contextData.TwoWordsBefore == "die")
                gender = FEM;
            else if (_contextData.TwoWordsBefore == "der" ||
                     _contextData.TwoWordsBefore == "das")
                gender = NON_FEM;
        }
        // Genitive: Die Lieblingsfarbe der Hexe ist rot.
        else if (possibleGenitiveConstruction) 
        {
            bool parenthesis = 
                _analysisData.TwoWordsBeforeAsWritten.Length > 0 &&
                (_analysisData.TwoWordsBeforeAsWritten.Last() == ')' ||
                _analysisData.TwoWordsBeforeAsWritten.Last() == ']' ||
                _analysisData.TwoWordsBeforeAsWritten.Last() == '}');
            if (_contextData.WordBefore == "der" &&
                !parenthesis &&
                _contextData.ThreeWordsBefore != "in") // in Ostfiresland der Tee
                gender = FEM;
            // Zitat (Absatz Nahrungsmittel) "Der Tee enthält -> Non fem det article
            else if (parenthesis)
                gender = NON_FEM;
        }
        // Word before is geographic marker 
        else if (_contextData.TwoWordsBefore != null && _contextData.WordBeforeStartsCapital && _contextData.WordBeforeTwoLastChars == "er")
        {
            if (_contextData.TwoWordsBefore == "die")
                gender = FEM;
            else if (_contextData.TwoWordsBefore == "der" ||
                     _contextData.TwoWordsBefore == "das")
                gender = NON_FEM;
        }

        return string.IsNullOrEmpty(gender) ? 
            (CANNOT_DETERMINE, default) : 
            (gender, "Definite article");
    }
}