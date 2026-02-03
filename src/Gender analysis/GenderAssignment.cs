/* 
 each method below consists of a string that returns one of the three string outcomes of the analysis:
      *  Fem
      *  Non Fem
      *  Cannot determine

 Note that we call the _analysisMethods in the order they are listed here, that is DefiniteArticle is called before IndefiniteArticle etc.
 Also not that each method analysis the word before the Noun - so if we have a case like "eine der Katzen", we analysis it in the sub DefiniteArticle since we find der
 definite article then calls LoadContextData and moves back one position and thereafter calls indefintiearticle to analysis the "eine" etc
*/

using System.Collections.Generic;

namespace GenusFinder;
public class GenderAssignment : GenderDeterminer
{
    /// <summary>
    /// The analyse methods used to determine the gender
    /// </summary>
    private readonly IReadOnlyCollection<GenderDeterminer> _analysisMethods;

    private IReadOnlyCollection<GenderDeterminer> InitiateAnalyisMethods() =>
[
        CreateDeterminerInstance<DefiniteArticle>(typeof(DefiniteArticle)),
        CreateDeterminerInstance<IndefiniteArticle>(typeof(IndefiniteArticle)),
        CreateDeterminerInstance<NegationPronoun>(typeof(NegationPronoun)),
        CreateDeterminerInstance<PossessiveOrDemonstrativePronoun>(typeof(PossessiveOrDemonstrativePronoun)),
        CreateDeterminerInstance<PrepositionNonFem>(typeof(PrepositionNonFem)),
        CreateDeterminerInstance<PrepositionFem>(typeof(PrepositionFem)),
        CreateDeterminerInstance<WeakAdjectiveEnding>(typeof(WeakAdjectiveEnding)),
        CreateDeterminerInstance<StrongAdjectiveEnding>(typeof(StrongAdjectiveEnding)),
    ];


    public GenderAssignment(LineAndPositionData analysisData, Verbs verbs, ContextData? contexData) : base(analysisData, verbs, contexData)
    {
        // We have to load analysis data to try to determine the gender
        // This might be changed later when we try each method but only within that specific method
        LoadContextData(_analysisData.NounPosition);
        _analysisMethods = InitiateAnalyisMethods();
    }

    /// <summary>
    /// Analysis the gender of a noun at a specific position in a line, if we do not have a word before the Noun we do not even get to this step, the string analysegenderatposition
    /// Stores the gender and returns true if we have found a gender for the Noun at the line
    ///Ootherwise it returns false and we have to call the method again if we have more occurences of the Noun on the line
    /// </summary>
    /// <returns>Gender and method caption (the caption of the method used to determine the gender, if applicable)</returns>
    public override (string outcome, string method) OutcomeGenderDeterminer()
    {
        // Certain cases where we explicitly cannot determine the gender
        UndeterminalbleCases undeterminalbleCases = new(_analysisData, _verbs, _contextData);
        bool undeterminable = undeterminalbleCases.OutcomeGenderDeterminer().outcome == GenderDeterminer.CANNOT_DETERMINE;
        if (undeterminable)
            return (
                CANNOT_DETERMINE,
                "The gender of the noun was deemed indeterminable because Undeterminable() returned true."
            );

        // Try each analysis method in order until one determines a gender
        foreach (var analysis in _analysisMethods) 
        {
            var gender = analysis.OutcomeGenderDeterminer();
            if (gender.outcome != CANNOT_DETERMINE)
                return gender;
        }

        // Fallback: none of the methods could determine the gender
        return (CANNOT_DETERMINE, "None of the methods could determine the gender.");
    }
}