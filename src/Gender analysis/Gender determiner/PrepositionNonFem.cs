using System;
using System.Linq;

namespace GenusFinder;

/// <summary>
/// Checks if the word before the noun is a preposition marking non fem gender, like (like ans, ins, im...)
/// </summary>
internal class PrepositionNonFem : GenderDeterminer
{
    public PrepositionNonFem(LineAndPositionData analysisData, Verbs verbs, ContextData contexData) : 
        base(analysisData, verbs, contexData)
    {

    }

    public override (string outcome, string method) OutcomeGenderDeterminer() =>
        WordsToDetermineGender.PrepositionsMarkingNonFemGender.Contains(_contextData.WordBefore) ?
            (NON_FEM, "PrepositionNonFem") :
            (CANNOT_DETERMINE, default);
}