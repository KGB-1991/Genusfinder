using System;
using System.Collections.Generic;

namespace GenusFinder;
internal abstract class WordsToDetermineGender
{
    // prepositions and pronouns
    public static IReadOnlyCollection<string> PossessiveAndDemonstrativePronouns = [
        "mein",
        "dein",
        "sein",
        "ihr", // note that Ihre = ihre since we search case insensitve
        "unser",
        "euer"
    ];
    public static IReadOnlyCollection<string> Prepositions = [
        "aus",
        "außer",
        "bei",
        "entgegen",
        "entsprechend",
        "gegenüber",
        "gemäß",
        "mit",
        "nach",
        "nächst",
        "nahe",
        "nebst",
        "seit",
        "von",
        "zu",
        "an",
        "auf",
        "hinter",
        "in",
        "neben",
        "über",
        "unter",
        "vor",
        "zwischen",
        "bis",
        "durch",
        "für",
        "gegen",
        "je",
        "ohne",
        "um",
        "wider"
    ];
    public static readonly IReadOnlyCollection<string> PrepositionsWithPossibleAccusative = [
        // might also be dative: ich denke an den Tee, der Salz liegt auf dem Tee
        "an",
        "auf",
        "hinter",
        "in",
        "neben",
        "über",
        "unter",
        "vor",
        "zwischen",

        // always accusative
        "bis",
        "durch",
        "für",
        "gegen",
        "je",
        "ohne",
        "um",
        "wider"
    ];
    public static readonly IReadOnlyCollection<string> PrepositionsMarkingNonFemGender = [
        "ans",
        "am",
        "aufs",
        "fürs", "fuers", // the german ü may be written was ue
        "ins",
        "im",
        "zum",
        "beim",
        "durchs"
    ];
    public static readonly IReadOnlyCollection<string> PrepositionsMarkingFemGender = [
        "zur"
    ];
    public static readonly IReadOnlyCollection<string> PrepositionsWithDative = [
        "aus",
        "außer", "ausser",
        "bei",
        "gegenüber", "gegenueber", // the german ü may be written was ue
        "mit",
        "nach",
        "seit",
        "von",
        "zu"
    ];
    public static readonly IReadOnlyCollection<string> InflexibleWords = [
        "gerne",
        "wie",
        "sowie",
        "beispielsweise",
        "irgendjemanden",
        "trotzdem",
        "lieber",
        "gerade",
        "ansonsten",
        "sie",
        "er",
        "lange",
        "aber",
        "oder",
        "hier",
        "weder",
        "respektive",
        "beziehungsweise",
        "the" // for english quotes
    ];
    public static readonly IReadOnlyCollection<Tuple<string, string>> InflexibleExpressions = [
        Tuple.Create("vor", "allem")
    ];
    public static readonly IReadOnlyCollection<string> DefiniteArticles = [
        "der",
        "die",
        "das",
        "den",
        "dem",
        "des"
    ];
    /// <summary>
    /// Eines Tages...
    /// </summary>
    public static readonly IReadOnlyCollection<string> IndefiniteArticles = [
        "ein",
        "eine",
        "einen",
        "einem",
        "eines"
    ];
}
