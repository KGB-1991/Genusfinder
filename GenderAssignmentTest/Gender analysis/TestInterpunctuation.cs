// 1.8 Interpunktionsteste

namespace GenderAssignmentTest;

[TestClass]
public sealed class TestInterpunctuation : TestGenderAssignment
{
    // 1.8.1 Feminine Genuszuweisung
    [TestMethod]
    public void Period() => RunTest("Hexe", "Hier ist die Hexe.", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void QuestionMark() => RunTest("Hexe", "Wo ist die Hexe?", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void ExclamationMark() => RunTest("Hexe", "Ich rede mit einer Hexe!", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void Parenthesis() => RunTest("Hexe", "Ist sie (die Hexe) wirklich echt?", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void SubordinateClause() => RunTest("Hexe", "Meinst du, dass die Hexe nett ist?", GenusFinder.GenderAssignment.FEM);
    
    [TestMethod]
    public void ExclamationAndQuestionMark() => RunTest("Hexe", "Ist sie eine Hexe?!?", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void Colon() => RunTest("Hexe", "Er ist keine Hexe:", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void Semicolon() => RunTest("Hexe", "Er ist keine Hexe;", GenusFinder.GenderAssignment.FEM);

    [TestMethod]
    public void QuotationMark() => RunTest("Hexe", "Die \"Hexe\" hier ist schön.", GenusFinder.GenderAssignment.FEM);
}