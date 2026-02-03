using System;
using System.Linq;

namespace GenusFinder;
internal class UndeterminalbleCases : GenderDeterminer
{
    private string _nounAsWritten => _analysisData.NounAsWritten;
    private string _wordBeforeNoun => _analysisData.Words[_nounPosition - 1];
    private char _lastCharBeforeNoun => _wordBeforeNoun.Length > 0 ? _wordBeforeNoun[^1] : default;

    public UndeterminalbleCases(LineAndPositionData analysisData, Verbs verbs, ContextData contexData) : 
        base(analysisData, verbs, contexData)
    {

    }

    public override (string outcome, string method) OutcomeGenderDeterminer()
    {
        // If we do not have a word before the noun or if the noun is the first word in the sentence
        if (_nounPosition - 1 < 0)
            return (
                CANNOT_DETERMINE,
                "The noun was the first word in the sentence, could not analyse gender assignment."
            );

        bool punctuationBeforeNoun = _endOfSentencePunctuation.Contains(_lastCharBeforeNoun);
        bool nounHasInnerDot =
            !_nounAsWritten.EndsWith('.') && _nounAsWritten.Contains('.'); // Tagtraum.Tee
        bool nounHasBracket =
            _nounAsWritten.Contains('(') ||
            _nounAsWritten.Contains('{') ||
            _nounAsWritten.Contains('[');

        // Cases where we cannot determine the gender at all
        if (punctuationBeforeNoun ||
            nounHasInnerDot ||
            nounHasBracket)
            return (
                CANNOT_DETERMINE,
                "The character before the noun was . ! ? : or ;, or the noun contained '.' or a bracket."
            );

        return
            UndeterminableCase() ?
                (CANNOT_DETERMINE, default) :
                (default);
    }

    /// <summary>
    /// Checks for various undeterminable cases that make it impossible to determine the gender
    /// </summary>
    /// <returns></returns>
    private bool UndeterminableCase()
    {
        return
            IsPrepositionBefore() ||
            IsInflexibleWord() ||
            IsTwoNounsAfterEachOther() ||
            IchSubjectFollowedByVerb() ||
            HyphenAfterNoun() ||
            WordBeforeIsVerb() ||
            ContainsUnderscore() ||
            WordBeforeContainsWeise() ||
            ExpressionBeforeNoun() ||
            GeographicMarker() ||
            DativePlural() ||
            GenitivePlural();
    }

    private bool IsPrepositionBefore() =>
        _contextData.WordBefore != null && WordsToDetermineGender.Prepositions.Contains( _contextData.WordBefore);

    private bool IsInflexibleWord() =>
         _contextData.WordBefore != null && WordsToDetermineGender.InflexibleWords.Contains( _contextData.WordBefore);

    private bool IsTwoNounsAfterEachOther()
    {
        char last = _analysisData.NounAsWritten.Last();
        return _contextData.WordAfterStartsCapital && !_endOfSentencePunctuation.Contains(last);
    }

    private bool IchSubjectFollowedByVerb() =>
         _contextData.TwoWordsBefore == "ich" &&  _contextData.WordBeforeLastChar.Equals('e');

    private bool HyphenAfterNoun()
    {
        string written = _analysisData.NounAsWritten;
        int index = written.IndexOf('-');
        if (index <= 0)
            return false;

        string before = written.Substring(0, index);
        return before.Contains(_analysisData.Noun);
    }

    private bool WordBeforeIsVerb() =>
        _verbs.IsVerb( _contextData.WordBefore);

    private bool ContainsUnderscore() =>
        _analysisData.NounAsWritten.Contains("_");

    private bool WordBeforeContainsWeise() =>
         _contextData.WordBefore?.Contains("weise") ?? false;

    private bool ExpressionBeforeNoun()
    {
        foreach (var expr in WordsToDetermineGender.InflexibleExpressions)
            if (expr.Item1 == _contextData.TwoWordsBefore && 
                expr.Item2 == _contextData.WordBefore)
                return true;
        return false;
    }

    private bool GeographicMarker()
    {
        if (! _contextData.WordBeforeStartsCapital)
            return false;

        // One word before
        bool marker =
             _contextData.WordBefore != null &&
             _contextData.WordBefore.EndsWith("er") &&
            !WordsToDetermineGender.IndefiniteArticles.Contains( _contextData.WordBefore + "er") &&
            !WordsToDetermineGender.DefiniteArticles.Contains( _contextData.WordBefore) &&
            !WordsToDetermineGender.PossessiveAndDemonstrativePronouns.Contains( _contextData.WordBefore) &&
            !IsExcludedPronoun(_contextData.WordBefore);

        // Two words before
        if ( _contextData.TwoWordsBefore != null) 
        {
            marker =
                marker &&
                !WordsToDetermineGender.IndefiniteArticles.Contains( _contextData.TwoWordsBefore + "er") &&
                !WordsToDetermineGender.DefiniteArticles.Contains( _contextData.TwoWordsBefore) &&
                !WordsToDetermineGender.PossessiveAndDemonstrativePronouns.Contains( _contextData.TwoWordsBefore) &&
                !IsExcludedPronoun(_contextData.TwoWordsBefore);
        }

        return marker;
    }

    private bool IsExcludedPronoun(string s) =>
        s == "ihren" || s == "ihre" || s == "ihrem" ||
        s == "ihres" || s == "ihrer" ||
        s == "jener" || s == "dieser";

    private bool DativePlural()
    {
        return
             _contextData.TwoWordsBefore != null &&
            _analysisData.NounAsWritten.EndsWith('n') &&
            WordsToDetermineGender.PrepositionsWithDative.Contains( _contextData.TwoWordsBefore);
    }

    private bool GenitivePlural() => 
            PluralConstruction() &&
            _contextData.WordBefore != null &&
            (_contextData.WordBefore == "der" ||
             _contextData.WordBefore == "den" ||
             _contextData.WordBefore == "einer" ||
             _contextData.WordBefore == "meiner" ||
             _contextData.WordBefore == "deiner" ||
             _contextData.WordBefore == "seiner" ||
             _contextData.WordBefore == "ihrer" ||
             _contextData.WordBefore == "unserer" ||
             _contextData.WordBefore == "eurer" ||
             _contextData.WordBefore == "ihrer");
}