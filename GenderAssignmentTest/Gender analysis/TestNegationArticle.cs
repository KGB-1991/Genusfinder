// 1.3 Negationsartikel

namespace GenderAssignmentTest;

[TestClass]
public sealed class TestNegationArticle : TestGenderAssignment
{
    // 1.3.1 Feminine Genuszuweisung
    [TestMethod]
    public void FemNomAkk3() => RunTest("Hexe", "Keine Hexe wohnt bei mir zuhause.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemDat() => RunTest("Hexe", "Ich vertraue keiner Hexe.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemGen2() => RunTest("Hexe", "Er gedenkt keiner Hexe.", GenusFinder.GenderAssignment.FEM);

    // 1.3.2 Nicht-feminine Genuszuweisung
    [TestMethod]
    public void NonFemNom2() => RunTest("Kaffee", "Kein Kaffee schmeckt gut.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemAkk3() => RunTest("Kaffee", "Ich trinke keinen Kaffee.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemDat4() => RunTest("Kaffee", "Schlechte Erfahrung haben wir ehrlich gesagt bislang mit keinem Kaffee gehabt.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemGen3() => RunTest("Kaffee", "Die Farbe keines Kaffees ist braun.", GenusFinder.GenderAssignment.NON_FEM);
}