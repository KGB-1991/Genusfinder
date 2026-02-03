using GenusFinder;
namespace GenderAssignmentTest;

/// <summary>
/// Abstract class for all gender assignment tests.
/// </summary>
public abstract class TestGenderAssignment
{
    public required TestContext TestContextInstance;
    /// <summary>
    /// Gets or sets the test context which provides information about and functionality for the current test run.
    /// </summary>
    public TestContext TestContext
    {
        get { return TestContextInstance; }
        set { TestContextInstance = value; }
    }

    private static readonly Verbs _verbs = new();

    /// <summary>
    /// Runs the test for the given noun and sentence and compares the outcome to the expected gender. Also logs the method used.
    /// </summary>
    /// <param name="sentence">Sentence to analyse</param>
    protected void RunTest(string noun, string sentence, string expectedGender)
    {
        // Arrange
        string[] words = sentence.Split(' ');

        // Act
        var gender = CheckGenderAssignment(noun, words);
        string foundGender = gender.outcome;
        string methodUsed = gender.method;

        // Assert
        TestContext.WriteLine($"Method used: {methodUsed}");
        Assert.AreEqual(expectedGender, foundGender);
    }

    /// <summary>
    /// Checks the assignment of the gender based on the analysis data for each position in the file that the noun occurs.
    /// </summary>
    /// <returns>The assigned gender, the method used</returns>
    private static (string outcome, string method) CheckGenderAssignment(string noun, string[] words) 
    {
        // Simulate noun position on the line on which the noun occurs
        int[] nounPositions = words
            .Select((word, index) =>
                FileReader.AcceptedEndings.Any(suffix => word.EndsWith(noun + suffix,
                                                                       StringComparison.InvariantCultureIgnoreCase)) ?
                                               index : -1)
            .Where(index => index != -1)
            .ToArray();

        (string outcome, string method) gender = (default, default);

        for (int i = 0; i < nounPositions.Length; i++) 
        {
            LineAndPositionData _analysisData = FolderAnalyser.LoadAnalysisDataForGenderPosition(nounPositions[i], noun, words);
            GenderAssignment _genderAssignmentTest = new(_analysisData,
                                                         _verbs,
                                                         null);
            gender = _genderAssignmentTest.OutcomeGenderDeterminer();
            // We assume that the user uses the same gender on the whole line and breaks the loop once we have found one gender
            if (gender.outcome != GenderAssignment.CANNOT_DETERMINE || 
                i == nounPositions.Length - 1)
                break; // we found a gender, break the loop
        }
        return gender;
    }
}