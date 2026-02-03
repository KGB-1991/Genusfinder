// 1.4 Possessives Pronomen

namespace GenderAssignmentTest;

[TestClass]
public sealed class TestPossessive : TestGenderAssignment
{
    // 1.4.1 Feminine Genuszuweisung
    [TestMethod]
    public void FemNom() => RunTest("Hexe", "Meine Hexe ist süß.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemAkk() => RunTest("Hexe", "Ich habe meine Hexe nicht verurteilt.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemDat5() => RunTest("Hexe", "Ich komme mit meiner Hexe nicht klar.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemGen3() => RunTest("Hexe", "Die Meinung meiner Hexe ist klug.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemNom2() => RunTest("Hexe", "Deine Hexe ist süß.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemAkk2() => RunTest("Hexe", "Ich habe deine Hexe nicht verurteilt.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemDat6() => RunTest("Hexe", "Ich komme mit deiner Hexe nicht klar.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemGen4() => RunTest("Hexe", "Die Meinung deiner Hexe ist klug.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemNom3() => RunTest("Hexe", "Seine Hexe ist süß.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemAkk3() => RunTest("Hexe", "Ich habe seine Hexe nicht verurteilt.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemDat7() => RunTest("Hexe", "Ich komme mit seiner Hexe nicht klar.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemGen5() => RunTest("Hexe", "Die Meinung seiner Hexe ist klug.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemNom4() => RunTest("Hexe", "Ich denke, ihre Hexe ist süß.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemAkk4() => RunTest("Hexe", "Ich habe ihre Hexe nicht verurteilt.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemDat8() => RunTest("Hexe", "Ich komme mit ihrer Hexe nicht klar.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemGen6() => RunTest("Hexe", "Die Meinung ihrer Hexe ist klug.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemNom5() => RunTest("Hexe", "Ihre Hexe ist süß.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemAkk5() => RunTest("Hexe", "Ich habe Ihre Hexe nicht verurteilt.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemDat9() => RunTest("Hexe", "Ich komme mit Ihrer Hexe nicht klar.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemGen7() => RunTest("Hexe", "Die Meinung Ihrer Hexe ist klug.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemNom6() => RunTest("Hexe", "Eure Hexe ist süß.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemAkk6() => RunTest("Hexe", "Ich habe eure Hexe nicht verurteilt.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemDat10() => RunTest("Hexe", "Ich komme mit eurer Hexe nicht klar.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void FemGen8() => RunTest("Hexe", "Die Meinung eurer Hexe ist klug.", GenusFinder.GenderAssignment.FEM);


    // 1.4.2 Nicht-feminine Genuszuweisung
    [TestMethod]
    public void NonFemNom3() => RunTest("Kaffee", "Mein Kaffee ist teuer.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemAkk4() => RunTest("Kaffee", "Ich habe meinen Kaffee nicht probiert.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemDat5() => RunTest("Kaffee", "Ich kann mit meinem Kaffee den Tag nicht starten.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemGen4() => RunTest("Kaffee", "Der Preis meines Kaffees ist teuer.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemNom4() => RunTest("Kaffee", "Dein Kaffee ist teuer.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemAkk5() => RunTest("Kaffee", "Ich habe deinen Kaffee nicht probiert.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemDat6() => RunTest("Kaffee", "Ich komme mit deinem Kaffee den Tag nicht starten.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemGen5() => RunTest("Kaffee", "Der Preis deines Kaffees ist teuer.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemNom5() => RunTest("Kaffee", "Sein Kaffee ist teuer.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemAkk6() => RunTest("Kaffee", "Ich habe seinen Kaffee nicht probiert.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemDat7() => RunTest("Kaffee", "Ich komme mit seinem Kaffee den Tag nicht starten.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemGen6() => RunTest("Kaffee", "Der Preis seines Kaffees ist teuer.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemNom6() => RunTest("Kaffee", "Ich denke, ihr Kaffee ist teuer.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemAkk7() => RunTest("Kaffee", "Ich habe ihren Kaffee nicht probiert.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemDat8() => RunTest("Kaffee", "Ich komme mit ihrem Kaffee den Tag nicht starten.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemGen7() => RunTest("Kaffee", "Der Preis ihres Kaffees ist teuer.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemNom7() => RunTest("Kaffee", "Ihr Kaffee ist teuer.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemAkk8() => RunTest("Kaffee", "Ich habe Ihren Kaffee nicht probiert.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemDat9() => RunTest("Kaffee", "Ich komme mit Ihrem Kaffee den Tag nicht starten.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemGen8() => RunTest("Kaffees", "Der Preis Ihres Kaffees ist teuer.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemNom8() => RunTest("Kaffee", "Euer Kaffee ist teuer.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemAkk9() => RunTest("Kaffee", "Ich habe euren Kaffee nicht probiert.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemDat10() => RunTest("Kaffee", "Ich komme mit eurem Kaffee den Tag nicht starten.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemGen9() => RunTest("Kaffee", "Der Preis eures Kaffees ist teuer.", GenusFinder.GenderAssignment.NON_FEM);


    // 1.4.3 Nicht feststellbare Genuszuweisung
    [TestMethod]
    public void CannotDeterminePlural() => RunTest("Hexe", "Ich rede mit diesen Hexen nicht.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void CannotDetermineVerb1l() => RunTest("Kaffee", "Ich meine, Kaffee schmeckt gut.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void CannotDetermineVerb2() => RunTest("Kaffee", "Ich mein, Kaffee schmeckt gut.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void CannotDetermineVerb3() => RunTest("Kaffee", "Sie meinen, Kaffee schmeckt gut.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void CannotDetermineVerb4() => RunTest("Kaffee", "Wir meinen, Kaffee schmeckt gut.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);

    [TestMethod]
    public void CannotDetermineVerb5() => RunTest("Kaffee", "scheint doof zu sein, Kaffee zu trinken.", GenusFinder.GenderAssignment.CANNOT_DETERMINE);
}