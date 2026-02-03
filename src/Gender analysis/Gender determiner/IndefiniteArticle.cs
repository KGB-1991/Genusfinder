using System;
using System.Linq;

namespace GenusFinder;
public class IndefiniteArticle : GenderDeterminer
{
    public IndefiniteArticle(LineAndPositionData analysisData, Verbs verbs, ContextData contexData) :
        base(analysisData, verbs, contexData)
    {

    }

    /// <summary>
    /// Checks if the word before the Noun is an indefinite article, like ein, eine, einen, einem, eines, eins.
    /// </summary>
    /// <returns></returns>
    public override (string outcome, string method) OutcomeGenderDeterminer()
    {
        string[] femIndefiniteArticle = { "eine", "einer",                  
                                          "ner", "ne" };                          // mit einer Katze, ner is an abbrivation for "einer" 
        string[] nonFemIndefiniteAricles = {"ein", "einen", "einem",
                                            "en", "em", "nem",              // abbrivations for "einen" and "einem"
                                            "eines", "eins"};               // genitive Die Farbe eines Endes

        string gender = default;
        if (femIndefiniteArticle.Contains(_contextData.WordBefore)) 
            gender = FEM;
        else if (nonFemIndefiniteAricles.Contains(_contextData.WordBefore))
            gender = NON_FEM;

        return string.IsNullOrEmpty(gender) ? 
            (CANNOT_DETERMINE, "IndefiniteArticle") : 
            (gender, "IndefiniteArticle");
    }
}