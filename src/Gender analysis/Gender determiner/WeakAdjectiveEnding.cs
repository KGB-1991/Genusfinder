using System;
using System.Collections.Generic;
using System.Linq;

namespace GenusFinder;

/// Checks if the word before the noun is a weak adjective ending, like in "der schöne Tee" or "die schöne Katze".
internal class WeakAdjectiveEnding : GenderDeterminer
{
    public WeakAdjectiveEnding(LineAndPositionData analysisData, Verbs verbs, ContextData contexData) : 
        base(analysisData, verbs, contexData)
    {

    }

    public override (string outcome, string method) OutcomeGenderDeterminer()
    {
        // note that this is a case sensitve analysis wrt the endings of the word before the Noun, unlike the analysis of the words before the nouns
        string gender = default;

        // Get the last char of the noun as written, ignoring punctuation at the end
        char lastNounCharAsWritten = _analysisData.NounAsWritten.Last();
        if (_analysisData.NounAsWritten.Length >= 2 &&
            _endOfSentencePunctuation.Contains(lastNounCharAsWritten))
            lastNounCharAsWritten = _analysisData.NounAsWritten[^2];

        // Berliner Tee -> if the first letter is a capital one AND we have not hitted any other of the cases where it is ok for it to be captial,
        // like in Eine, Der, Die etc. then we cannot determine the gender for adjectives since it is a geographic adjective
        // The exception is that if the word is the first in the sentence, like in Kalter Kaffee, then we analyse it anyway as if it were just any Noun
        if (_contextData.WordBeforeStartsCapital && 
            _contextData.TwoWordsBefore != null && 
            _contextData.TwoWordsBefore.Length > 0) 
        {
            gender = CANNOT_DETERMINE;
        }
        else if (_contextData.WordBeforeLastChar.Equals('e') || 
                (_contextData.WordBeforeTwoLastChars == "en" && _analysisData.LastNounChar.Equals('e'))) 
        {
            // looks for the definite article until we find it and determins the gender from there
            // We cannot look at anything else than singular

            // Die Gedanken der herrschenden Klasse
            // zur herrschenden Klasse
            if (_contextData.WordBeforeTwoLastChars == "en" && 
                (_contextData.TwoWordsBefore == "der" && _analysisData.LastNounChar == lastNounCharAsWritten) || // That is, non plural -> Qualität der der anderen Tees
                WordsToDetermineGender.PrepositionsMarkingFemGender.Contains(_contextData.TwoWordsBefore)) 
            {
                gender = FEM;
            }
            // to avoid cases like: "die durch grünen Tee" where we otherwise would search for "die" and think that it is Fem
            else if (_contextData.WordBeforeTwoLastChars == "en" && 
                     WordsToDetermineGender.PrepositionsWithPossibleAccusative.Contains(_contextData.TwoWordsBefore)) 
            {
                gender = NON_FEM;
            }
            else 
            {
                // if we have the senetence: "Der von der bösen Frau geschlagene Mann", we have to skip the first definite article we find since it belong to and other Noun
                // then we have to go back until we find the next definite article which belongs to our Noun
                bool skipOneDefiniteArticle = _contextData.WordBeforeStartsCapital;

                // linear search until we find the right definite article or run out of words...
                // loops from NounPosition until the start of the line by subtracting one each time
                for (int i = _analysisData.NounPosition; i >= 0; i--) 
                {
                    /** 
                     * load data one position backwards
                     * don't reset the bool if the new word before the Noun is not capital or if it is the first word in the sentence, like in:
                     * "Der von der Frau geschlagene Tee."
                     * Note that we can have the case "Der hellgrüne Bancha-Tee besteht" - then we have to ignore that _contextData.WordBeforeStartsCapital since it is an article and not a Noun
                     * Also note that we can have cases where have a captial letter due to marking a new sentence by having "." on two words before like in "Dieser gekochte Tee"
                     * if the word before ends with a "." it marks a new sentence. Stop looking backwards in that case. (LoadContextData will make _contextData.WordBefore default in that case)
                     * Diese bezieht sich auf einen Durchschnittswert von 6,5g je Standardtasse, die in der Praxis nicht verbreitet ist. Trinken Amerikaner gern ""dünnen"" Kaffee in großen Gefäßen
                     * Abwahlantrag, den ganzen Kaffee -> don't look before the comma
                     * nach sich gezogen und ist inzwischen kalter Kaffee -> don't look before und 
                    **/
                    LoadContextData(i);
                    if (_contextData.WordBefore == default 
                        || _contextData.WordBefore == "und" 
                        || _contextData.WordBeforeLastChar.Equals(','))
                        break;

                    if (_contextData.WordBeforeStartsCapital && !WordsToDetermineGender.DefiniteArticles.Contains(_contextData.WordBefore) &&
                        !skipOneDefiniteArticle && i > 1 && !_contextData.TwoWordsBeforeLastChar.Equals('.'))
                        skipOneDefiniteArticle = true;

                    var methods = new List<GenderDeterminer>
                    {
                        CreateDeterminerInstance<DefiniteArticle>(typeof(DefiniteArticle)),
                        CreateDeterminerInstance<IndefiniteArticle>(typeof(IndefiniteArticle)),
                        CreateDeterminerInstance<NegationPronoun>(typeof(NegationPronoun)),
                        CreateDeterminerInstance<PrepositionNonFem>(typeof(PrepositionNonFem)),
                        CreateDeterminerInstance<PossessiveOrDemonstrativePronoun>(typeof(PossessiveOrDemonstrativePronoun)),
                    };

                    foreach (var method in methods) 
                    {
                        (string gender, string method) outcome = method.OutcomeGenderDeterminer();
                        // We found a method that determines the gender
                        if (outcome.gender != CANNOT_DETERMINE)
                            return outcome;
                    }

                    // _analysisMethods done - check if we should go on with next position
                    if (gender != CANNOT_DETERMINE && !skipOneDefiniteArticle) 
                    {
                        break;
                    }
                    else if (gender != CANNOT_DETERMINE && skipOneDefiniteArticle) // von der bösen Frau -> not the right Noun, go on until we find the next one
                    {
                        gender = CANNOT_DETERMINE;
                        skipOneDefiniteArticle = false;
                    } // end if
                } // end for
            } // end else
        } // end if

        return string.IsNullOrEmpty(gender) ?
            (CANNOT_DETERMINE, default) :
            (gender, "WeakAdjectiveEnding");
    }
}