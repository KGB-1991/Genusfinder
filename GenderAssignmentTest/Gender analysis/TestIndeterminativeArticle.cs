// 1.2 Indefiniter Artikel

namespace GenderAssignmentTest;

[TestClass]
public sealed class TestIndeterminativeArticle : TestGenderAssignment
{
    // 1.2.1 Feminine Genuszuweisung
    [TestMethod]
    public void FemNom() => RunTest("Hexe", "Eine Hexe wohnt bei mir zuhause.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]

    public void FemDat() => RunTest("Hexe", "Ich bin nicht bereit, mit einer Hexe zu reden.", GenusFinder.GenderAssignment.FEM);
    [TestMethod]
    public void FemGen() => RunTest("Hexe", "Die Anklage einer Hexe ist lügenhaft.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemNomAkk() => RunTest("Hexe", "Ich habe hier ne Hexe.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemNomAkk2() => RunTest("Hexe", "Ich rede mit ner Hexe.", GenusFinder.GenderAssignment.FEM);


    // 1.2.2 Nicht-feminine Genuszuweisung
    [TestMethod]
    public void NonFemNom() => RunTest("Kaffee", "Ein Kaffee würde gut schmecken.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemAkk() => RunTest("Kaffee", "Ich trinke einen Kaffee.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemDat() => RunTest("Kaffee", "Ich sitze hier mit einem Kaffee in der Hand.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemGen() => RunTest("Kaffee", "Die Farbe des Kaffees ist braun.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemGen2() => RunTest("Kaffee", "Eines des Kaffees schmeckt gut.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemAkk2() => RunTest("Kaffee", "Kann ich noch en Kaffee haben?", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemDat2() => RunTest("Kaffee", "Bist du mit em Kaffee in der Hand hierher gekommen?", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemDat3() => RunTest("Kaffee", "Kannst du bitte mit nem Kaffee kommen?", GenusFinder.GenderAssignment.NON_FEM);
}