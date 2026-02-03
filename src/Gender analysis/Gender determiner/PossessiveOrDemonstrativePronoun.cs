using System;
using System.Linq;

namespace GenusFinder;

/// <summary>
/// Checks if the word before the Noun is a possessive or demonstrative pronoun, like 
/// mein, dein, sein, ihr, unser, euer, dies, diese, dieser.
/// </summary>
internal class PossessiveOrDemonstrativePronoun : GenderDeterminer
{
    public PossessiveOrDemonstrativePronoun(LineAndPositionData analysisData, Verbs verbs, ContextData contexData) : 
        base(analysisData, verbs, contexData)
    {

    }

    public override (string outcome, string method) OutcomeGenderDeterminer()
    {
        const string METHOD_NAME = "PossessiveOrDemonstrativePronoun";

        // ich meine Püree schmeckt gut = "meine" is a verb, not possessive pronoun we care about
        // sein can be a verb too: es scheint doof zu sein Kaffee zu trinken, check "zu" before "sein"
        bool meinPlusIch = _contextData.WordBefore == "mein" && _contextData.TwoWordsBefore == "ich";
        bool meinePlusIch = _contextData.WordBefore == "meine" && _contextData.TwoWordsBefore == "ich";
        bool meinenPlusWirOrSie = _contextData.WordBefore == "meinen" &&
                                  (_contextData.TwoWordsBefore == "wir" ||
                                   _contextData.TwoWordsBefore == "sie" ||
                                   _contextData.TwoWordsBefore == "die");
        bool seinPlusZu = _contextData.WordBefore == "sein" && _contextData.TwoWordsBefore == "zu";

        // If any of these are verb-like constructions, we do not try to determine gender here
        if (meinPlusIch || meinePlusIch || meinenPlusWirOrSie || seinPlusZu)
            return (CANNOT_DETERMINE, METHOD_NAME);

        // "dies-" is a special case (demonstrative pronoun) – we can say "dieser Tee", but not "*meiner Tee"
        if ((_contextData.WordBefore == "diesen" && _analysisData.LastNounChar == 'e') ||
            _contextData.WordBefore == "diesem" ||
            _contextData.WordBefore == "dieses") // non plural
            return (NON_FEM, METHOD_NAME);

        if (_contextData.WordBefore == "diese" && _analysisData.NounAsWritten.Last() == 'e') // non plural
            return (FEM, METHOD_NAME);

        // Special case: euer / eurer
        if (_contextData.WordBefore == "eurer" &&
            (WordsToDetermineGender.PrepositionsWithDative.Contains(_contextData.TwoWordsBefore) ||
            PossibleGenitiveConstruction()))
            return (FEM, METHOD_NAME);

        if (_contextData.WordBefore == "eures")
            return (NON_FEM, METHOD_NAME);

        if (_contextData.WordBefore == "dieser") 
        {
            // mit dieser Frau, einer dieser Frauen
            bool prepBefore = WordsToDetermineGender.Prepositions.Contains(_contextData.TwoWordsBefore);
            bool notEndingOnE = _analysisData.LastNounChar != 'e';
            bool genitiveConstr = PossibleGenitiveConstruction();

            var gender = (prepBefore || notEndingOnE || genitiveConstr) ?
                FEM : 
                NON_FEM;
            return (gender, METHOD_NAME);
        } // end "dieser"

        // Other possessive and demonstrative pronouns
        foreach (string pronoun in WordsToDetermineGender.PossessiveAndDemonstrativePronouns) 
        {
            // Feminine:
            //  - meine, seine ... (pronoun + "e") with singular noun ending on 'e'
            //  - meiner, seiner ... (pronoun + "er")
            if ((_contextData.WordBefore == pronoun + "e" && _analysisData.LastNounChar == 'e') ||
                _contextData.WordBefore == pronoun + "er")
                // If we have found a gender, do not continue the search
                return (FEM, METHOD_NAME);

            // Non-feminine:
            //  - mein, sein
            //  - meinem, seinem (pronoun + "em")
            //  - meinen, deinen (Akkusativ) but not dativ plural like "Enden" (ends on 'n')
            //  - meines, seines (pronoun + "es") with noun ending on 's'
            bool nonFemMatch =
                _contextData.WordBefore == pronoun ||
                _contextData.WordBefore == pronoun + "em" ||
                (_contextData.WordBefore == pronoun + "en" &&
                 _analysisData.NounAsWritten.Last() != 'n') ||
                (_contextData.WordBefore == pronoun + "es" &&
                 _analysisData.NounAsWritten.Last() == 's');

            if (nonFemMatch)
                return (NON_FEM, METHOD_NAME);
        } // end foreach

        // If nothing matched, we cannot determine the gender here
        return (CANNOT_DETERMINE, METHOD_NAME);
    }
}