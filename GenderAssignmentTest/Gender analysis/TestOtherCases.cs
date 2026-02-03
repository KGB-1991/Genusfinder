// 1.9 Andere Sonderfälle

namespace GenderAssignmentTest;

[TestClass]
public sealed class TestOtherCases : TestGenderAssignment
{
    // 1.9.1 Feminine Genuszuweisung
    [TestMethod]
    public void FemGenusAssignment1() => RunTest("Hexe", "auch wenn eine Hexe immer schön ist.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemGenusAssignment2() => RunTest("Hexe", "Die Hexe ist das richtige Genus, nicht das Hexe.", GenusFinder.GenderAssignment.FEM);

    // 1.9.2 Nicht-feminine Genuszuweisung
    [TestMethod]
    public void NonFemNomCompound1() => RunTest("Bancha-Kaffee", "Der hellgrüne Bancha-Kaffee ist eigentlich ein Tee.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemNomCompound2() => RunTest("Banchakaffee", "Der hellgrüne Banchakaffee ist eigentlich ein Tee.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemNomCompound3() => RunTest("Kaffee", "Laut Fictio wurde in Atlantis schwarzer Kaffee als Unterstützung bei einer Diät getrunken.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemNomCompound4() => RunTest("Kaffee", "Dieser starke Kaffee schmeckt gut.", GenusFinder.GenderAssignment.NON_FEM);

    // 1.9.3 Nicht-feststellbare Genuszuweisung
    [TestMethod]
    public void IndetGenusAssignment1() => RunTest("Ende", "Ich ende Ende.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void IndetGenusAssignment2() => RunTest("Hexe", "Hexe zu sein ist nicht mein Ding, ich mag Hexen nicht.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void IndetGenusAssignment3() => RunTest("Kaffee", "Ich mag Kaffee.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void IndetGenusAssignment4() => RunTest("Ende", "Er hat laut geschrien, ohne Ende.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void GenitivePluralAssignment4() => RunTest("Kaffees", "Ich kann mir das nur mit einer hypnotischen Wirkung ihrer Kaffees auf ihre Anhänger erklären", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    // Extra tests that work but are not in use
    //[TestMethod]
    //public void Test1() => RunTest("Tee", "kann auch DIE Tees. sein", GenusFinder.GenderAssignment.CANNOT_DETERMINE);
    //[TestMethod]
    //public void Test2() => RunTest("Tee", "Zitat (Absatz Nahrungsmittel) \"Der Tee enthält vor allem Kohlenhydrate, Ballaststoffe, Proteine, Fette, Mineralstoffe und Vitamine\"", GenusFinder.GenderAssignment.NON_FEM);
    //[TestMethod]
    //public void Test3() => RunTest("Tee", "na das ist noch \"geschwurbel\", inwiefern ist seine Qualität der der anderen Tees so überlegen", GenusFinder.GenderAssignment.CANNOT_DETERMINE);
}