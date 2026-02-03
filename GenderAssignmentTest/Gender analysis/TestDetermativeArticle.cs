// 1.1 Determinativer Artikel

namespace GenderAssignmentTest;

[TestClass]
public sealed class TestDetermativeArticle : TestGenderAssignment
{
    // 1.1.1 Feminine Genuszuweisung
    [TestMethod]
    public void FemNom() => RunTest("Hexe", "Die Hexe ist süß.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemAkk() => RunTest("Hexe", "Ich treffe morgen die Hexe.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemDat() => RunTest("Hexe", "Ich rede mit der Hexe.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemGen() => RunTest("Hexe", "Die Lieblingsfarbe der Hexe ist rot.", GenusFinder.GenderAssignment.FEM);

    // 1.1.2 Nicht-feminine Genuszuweisung
    [TestMethod]
    public void NonFemNom() => RunTest("Kaffee", "Der Kaffee schmeckt gut", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemAkk() => RunTest("Kaffee", "Ich hole mir jetzt den Kaffee.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemDat() => RunTest("Kaffee", "Kommt hierher mit dem Kaffee!", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemGen() => RunTest("Kaffee", "Der Preis des Kaffees ist sehr hoch.", GenusFinder.GenderAssignment.NON_FEM);
}