// 1.7 Possessives Pronomen

namespace GenderAssignmentTest;

[TestClass]
public sealed class TestDemonstrative : TestGenderAssignment
{
    // 1.7.1 Feminine Genuszuweisung
    [TestMethod]
    public void FemNom() => RunTest("Hexe", "Diese Hexe ist süß.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemAkk() => RunTest("Hexe", "Ich habe diese Hexe nicht verurteilt.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemDat13() => RunTest("Hexe", "Ich komme mit dieser Hexe nicht klar.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemGen11() => RunTest("Hexe", "Die Meinung dieser Hexe ist klug.", GenusFinder.GenderAssignment.FEM);

    // 1.7.2 Nicht-feminine Genuszuweisung
    [TestMethod]
    public void NonFemNom12() => RunTest("Kaffee", "Dieser Kaffee ist teuer.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemAkk13() => RunTest("Kaffee", "Ich habe diesen Kaffee nicht probiert.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemDat14() => RunTest("Kaffee", "Ich kann mit diesem Kaffee den Tag nicht starten.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemGen14() => RunTest("Kaffee", "Der Preis dieses Kaffees ist teuer.", GenusFinder.GenderAssignment.NON_FEM);
}
