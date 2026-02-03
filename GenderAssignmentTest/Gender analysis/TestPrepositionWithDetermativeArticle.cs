// 1.6 Präposition mit einem bestimmten Artikel

namespace GenderAssignmentTest;

[TestClass]
public sealed class TestPrepositionWithDetermativeArticle : TestGenderAssignment
{
    // 1.6.1 Nicht-feminine Genuszuweisung
    [TestMethod]
    public void NeutAnDas() => RunTest("Ende", "Ich denke ans Ende.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NeutAnDem() => RunTest("Ende", "Ich bin am Ende.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NeutAufDas() => RunTest("Ende", "Wir warten aufs Ende.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NeutFürDas() => RunTest("Ende", "Auf der Suche nach einem Freund fürs Ende der Welt.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NeutFürDas2() => RunTest("Ende", "Auf der Suche nach einem Freund fuers Ende der Welt.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NeutInDas() => RunTest("Ende", "Bevor wir gleich ins Ende springen.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemInDem() => RunTest("Kaffee", "Es gibt keinen Zucker im Kaffee.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemZuDem() => RunTest("Kaffee", "Zum Kaffee gibt’s auch einen Kuchen.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NonFemBeiDem() => RunTest("Kaffee", "Ich plane, die Nachricht morgen früh beim Kaffee zu verkünden.", GenusFinder.GenderAssignment.NON_FEM);

    [TestMethod]
    public void NeutDurchDas() => RunTest("Ende", "Ich reise 100.000 Blöcke durchs Ende von Minecraft.", GenusFinder.GenderAssignment.NON_FEM);
}
