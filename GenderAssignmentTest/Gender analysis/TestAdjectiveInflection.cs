// 1.5 Adjektivflexion

namespace GenderAssignmentTest;

[TestClass]
public sealed class TestAdjectiveInflection : TestGenderAssignment
{
    // 1.5.1 Feminine Genuszuweisung
    [TestMethod]
    public void FemNomAkk4() => RunTest("Hexe", "Die schöne Hexe.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemNomAkk5() => RunTest("Hexe", "Die von dem Mann geschlagene Hexe.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemDat12() => RunTest("Hexe", "Ich rede mit einer tollen Hexe.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemGen10() => RunTest("Hexe", "Die Lieblingsfarbe der netten Hexe ist gelb.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemNomAkk6() => RunTest("Hexe", "Schöne Hexe!", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemDatGen() => RunTest("Hexe", "Weg gesperrt wegen schöner Hexe.", GenusFinder.GenderAssignment.FEM);

    // 1.5.2 Nicht-feminine Genuszuweisung
    [TestMethod]
    public void NonFemNom10() => RunTest("Kaffee", "Der starke Kaffee schmeckt gut.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NeutNom() => RunTest("Ende", "Das tolle Ende der Höhepunkt des Films.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NeutNom2() => RunTest("Ende", "Das Lunder Ende.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemAkk11() => RunTest("Kaffee", "Ich habe den teuersten Kaffee gekauft.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NeutAkk() => RunTest("Ende", "Ich habe das tolle Ende des Buches nur erfunden.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemDat12() => RunTest("Kaffee", "Ich habe gestern den Tag mit einem guten Kaffee beendet.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NeutDat() => RunTest("Ende", "Dann habe ich ihm vom schönen Ende erzählt.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemGen11() => RunTest("Kaffees", "Ich erinnere mich des guten Kaffees, den wir getrunken haben.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemGen12() => RunTest("Kaffee", "Die Farbe des leckeren Kaffees ist schwarz.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemNom11() => RunTest("Kaffee", "Das ist alter Kaffee.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemAkk12() => RunTest("Kaffee", "Tante Vasa hat guten Kaffee.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NeutNomAkk() => RunTest("Ende", "Echt gutes Ende!", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemDat13() => RunTest("Kaffee", "Er widmet sich lieber gutem Kaffee.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemGen13() => RunTest("Kaffee", "Wegen guten Kaffees ist das Café immer voll.", GenusFinder.GenderAssignment.NON_FEM);

    // 1.5.3 Nicht feststellbare Genuszuweisung
    [TestMethod]
    public void CannotDetermineGeography() => RunTest("Ende", "Lunder Ende.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void CannotDetermineGeography2() => RunTest("Kaffee", "Mit Lunder Kaffee kommt man weit.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void CannotDetermineGerne() => RunTest("Kaffee", "Gerne Kaffee.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void CannotDetermineWie() => RunTest("Kaffee", "Das hier schmeckt wie Kaffee!", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void CannotDetermineSowie() => RunTest("Kaffee", "In der Fabrik in Slöinge finden Sie eine wirklich tolle Eisdiele und ein Café mit allen Kugeleissorten, Stangen und Waffeln sowie Kaffee und anderen Getränken.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void CannotDetermineBeispielsweise() => RunTest("Kaffee", "Wir können beispielsweise Kaffee trinken.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void CannotDetermineIrgendjemanden() => RunTest("Kaffee", "Ich musste noch nie irgendjemanden Kaffee kochen.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void CannotDetermineTrotzdem() => RunTest("Kaffee", "Ich trinke trotzdem Kaffee.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void CannotDetermineVerb() => RunTest("Kaffee", "Ich trinke Kaffee.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void CannotDetermineVerb2() => RunTest("Kaffee", "Er akzeptierte Kaffee als Belohnung.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void CannotDetermineVerb3() => RunTest("Hexen", "Ich rede gerne mit netten Hexen.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void CannotDetermineVerb4() => RunTest("Kaffee", "Er trinkt vor allem Kaffee.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);
}
