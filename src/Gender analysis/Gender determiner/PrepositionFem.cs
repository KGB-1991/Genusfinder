using System;
using System.Linq;

namespace GenusFinder;

/// Checks if the word before the noun is a weak adjective ending, like in "der schöne Tee" or "die schöne Katze".
internal class PrepositionFem : GenderDeterminer
{
    public PrepositionFem(LineAndPositionData analysisData, Verbs verbs, ContextData contexData) : 
        base(analysisData, verbs, contexData)
    {

    }

    public override (string outcome, string method) OutcomeGenderDeterminer() => 
        WordsToDetermineGender.PrepositionsMarkingFemGender.Contains(_contextData.WordBefore) ? 
            (NON_FEM, "PrepositionFem") : 
            (CANNOT_DETERMINE, default);
}