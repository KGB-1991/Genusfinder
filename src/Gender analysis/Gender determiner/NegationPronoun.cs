using System;
using System.Linq;

namespace GenusFinder;
internal class NegationPronoun : GenderDeterminer
{
    public NegationPronoun(LineAndPositionData analysisData, Verbs verbs, ContextData contexData) : 
        base(analysisData, verbs, contexData)
    {

    }
    public override (string outcome, string method) OutcomeGenderDeterminer()
    {
        string gender = default;
        if (
            (_contextData.WordBefore == "keine" &&
            (_analysisData.NounAsWritten.Last().Equals('e'))) || // not plural
            (_contextData.WordBefore == "keiner" &&
                (WordsToDetermineGender.Prepositions.Contains(_contextData.TwoWordsBefore) || _verbs.IsDativeVerb(_contextData.TwoWordsBefore) || _verbs.IsGenitiveVerb(_contextData.TwoWordsBefore)))  // or like in: mit einer Katze
            )
            gender = FEM;
        else if (_contextData.WordBefore == "kein" || _contextData.WordBefore == "keinem" ||
            (_contextData.WordBefore == "keinen" && !_analysisData.NounAsWritten.Last().Equals('n')) || // Ackusative, but Enden = dativ plural, not ackusativ, thus we have to check last char...
             _contextData.WordBefore == "keines" ||                                            // Keines der Probleme
            (_contextData.WordBefore == "keiner" && !WordsToDetermineGender.Prepositions.Contains(_contextData.TwoWordsBefore)))      // Keiner der Russen ist hier.
            gender = NON_FEM;

        return string.IsNullOrEmpty(gender) ?
            (CANNOT_DETERMINE, "NegationPronoun") :
            (gender, "NegationPronoun");
    }
}