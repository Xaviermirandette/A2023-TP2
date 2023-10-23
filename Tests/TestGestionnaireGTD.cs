using GTD;

namespace Tests
{
    public class TestGestionnaireGTD
    {
        private GestionnaireGTD _gestionnaireGTD;

        [SetUp]
        public void Setup()
        {
            _gestionnaireGTD = new GestionnaireGTD();
        }

        // Premier test de l'énoncé
        [Test]
        public void TestActionPosterieurePasDansListe()
        {
            Assert.Pass();
        }

        // Deuxième test de l'énoncé
        [Test]
        public void TestActionVientDansProchaineAction()
        {
            Assert.Pass();
        }

        // Troisième test de l'énoncé
        [Test]
        public void TestSuiviPasseAEntree()
        {
            Assert.Pass();
        }
    }
}