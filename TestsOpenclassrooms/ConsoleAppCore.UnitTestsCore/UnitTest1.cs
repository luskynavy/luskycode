//Fine Code Coverage : Ok en lançant le Run Tests de visual (pas celui de CodeRush) par le menu Test -> Run all tests, bouton droit Run Tests ou du Test Explorer -> Run
//CodeRush, Run with coverage : montre bien le code couvert avec l'interface de CodeRush.

using FluentAssertions;
using Moq;

namespace ConsoleAppCore.UnitTestsCore
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGagne()
        {
            // Arrange
            var fournisseurMeteo = Mock.Of<IFournisseurMeteo>();
            Mock.Get(fournisseurMeteo).Setup(m => m.QuelTempsFaitIl(It.IsAny<DateTime>())).Returns(Meteo.Pluie);
            Jeu jeu = new Jeu(fournisseurMeteo);

            // Act
            var resultat = jeu.Tour(6, 1);

            // Assert
            //Version en plusieurs lignes
            if (resultat != Resultat.Gagne)
                Assert.Fail();
            //Version en une seule ligne
            Assert.AreEqual(Resultat.Gagne, resultat);
            //Version avec FluentAssertions
            resultat.Should().Be(Resultat.Gagne);
            resultat.Should().NotBe(Resultat.Perdu);
            Assert.AreEqual(1, jeu.Heros.Points);
            Assert.AreEqual(15, jeu.Heros.PointDeVies);
        }

        [TestMethod]
        public void TestEgalite()
        {
            // Arrange
            var fournisseurMeteo = Mock.Of<IFournisseurMeteo>();
            Mock.Get(fournisseurMeteo).Setup(m => m.QuelTempsFaitIl(It.IsAny<DateTime>())).Returns(Meteo.Pluie);
            Jeu jeu = new Jeu(fournisseurMeteo);

            // Act
            var resultat = jeu.Tour(5, 5);

            // Assert
            Assert.AreEqual(Resultat.Gagne, resultat);
            Assert.AreEqual(1, jeu.Heros.Points);
            Assert.AreEqual(15, jeu.Heros.PointDeVies);
        }

        [TestMethod]
        public void TestPerduSoleil()
        {
            // Arrange
            var fournisseurMeteo = Mock.Of<IFournisseurMeteo>();
            Mock.Get(fournisseurMeteo).Setup(m => m.QuelTempsFaitIl(It.IsAny<DateTime>())).Returns(Meteo.Pluie);
            Jeu jeu = new Jeu(fournisseurMeteo);

            // Act
            var resultat = jeu.Tour(2, 4);

            // Assert
            resultat.Should().Be(Resultat.Perdu);
            Assert.AreEqual(0, jeu.Heros.Points);
            Assert.AreEqual(13, jeu.Heros.PointDeVies);
        }

        [TestMethod]
        public void TestPerduPluie()
        {
            // Arrange
            var fournisseurMeteo = Mock.Of<IFournisseurMeteo>();
            Mock.Get(fournisseurMeteo).Setup(m => m.QuelTempsFaitIl(DateTime.Now)).Returns(Meteo.Pluie);
            Jeu jeu = new Jeu(fournisseurMeteo);

            // Act
            var resultat = jeu.Tour(2, 4);

            // Assert
            resultat.Should().Be(Resultat.Perdu);
            Assert.AreEqual(0, jeu.Heros.Points);
            Assert.AreEqual(13, jeu.Heros.PointDeVies);
        }

        [TestMethod]
        public void TestPerduTempete()
        {
            // Arrange
            var fournisseurMeteo = Mock.Of<IFournisseurMeteo>();
            Mock.Get(fournisseurMeteo).Setup(m => m.QuelTempsFaitIl(DateTime.Now)).Returns(Meteo.Tempete);
            Jeu jeu = new Jeu(fournisseurMeteo);

            // Act
            var resultat = jeu.Tour(2, 4);

            // Assert
            resultat.Should().Be(Resultat.Perdu);
            jeu.Heros.Points.Should().Be(0);
            jeu.Heros.PointDeVies.Should().Be(13);
        }

        [TestMethod]
        //Vérifie qu'on a bien cette exception
        [ExpectedException(typeof(DivideByZeroException))]
        public void VerifieLaDivisionParZero()
        {
            // Arrange
            int entier = 0;
            int autreEntier = 5;

            // Act & Assert
            Assert.ThrowsException<DivideByZeroException>(() => autreEntier / entier);
            Action division = () => { int resultat = autreEntier / entier; };
            division.Should().Throw<DivideByZeroException>();
            //Pas top, message du système traduit selon langue installée et éventuelle mise à jour
            division.Should().Throw<DivideByZeroException>().WithMessage("Attempted to divide by zero."); // en Français : Tentative de division par zéro.
            int division2 = autreEntier / entier;
        }

        [TestMethod]
        public void VerifieException()
        {
            new Test().Invoking(t => t.VaLeverUneException()).Should().Throw<InvalidOperationException>().WithMessage("Impossible de faire ça !");
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Moq_ExemplesException()
        {
            var fauxObjet = Mock.Of<Demo>();
            Mock.Get(fauxObjet).Setup(x => x.DemoMethode(It.IsAny<int>()))
                .Throws<NotImplementedException>();

            fauxObjet.DemoMethode(1);
        }

        [TestMethod]
        public void Moq_ExemplesReturnsWithIsAny()
        {
            var fauxObjet = Mock.Of<Demo>();
            Mock.Get(fauxObjet).Setup(x => x.DemoMethode(It.IsAny<int>())).Returns(4);

            fauxObjet.DemoMethode(0).Should().Be(4);
            fauxObjet.DemoMethode(1).Should().Be(4);
            fauxObjet.DemoMethode(-16).Should().Be(4);
        }

        [TestMethod]
        public void Moq_ExemplesMultipleReturns()
        {
            var fauxObjet = Mock.Of<Demo>();
            Mock.Get(fauxObjet).SetupSequence(x => x.DemoMethode(It.IsAny<int>()))
                .Returns(4)
                .Returns(5)
                .Returns(6);

            fauxObjet.DemoMethode(0).Should().Be(4);
            fauxObjet.DemoMethode(1).Should().Be(5);
            fauxObjet.DemoMethode(-16).Should().Be(6);
        }

        [TestMethod]
        public void Moq_ExemplesReturnsWithValue()
        {
            var fauxObjet = Mock.Of<Demo>();
            Mock.Get(fauxObjet).Setup(x => x.DemoMethode(1)).Returns(4);
            Mock.Get(fauxObjet).Setup(x => x.DemoMethode(6)).Returns(0);

            fauxObjet.DemoMethode(6).Should().Be(0);
            fauxObjet.DemoMethode(1).Should().Be(4);
            fauxObjet.DemoMethode(6).Should().Be(0);
        }

        [TestMethod]
        public void QuelTempsFaitIl_AvecDuSoleil_RetourneDuSoleil()
        {
            // arrange
            IFournisseurMeteo fournisseurMeteo = new MeteoRepository(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestsOpenclassrooms;Integrated Security=True;");

            // act
            var temps = fournisseurMeteo.QuelTempsFaitIl(new DateTime(2023, 8, 2));

            // assert
            temps.Should().Be(Meteo.Soleil);
        }
    }

    public class Test
    {
        public void VaLeverUneException()
        {
            throw new InvalidOperationException("Impossible de faire ça !");
        }
    }

    public class Demo
    {
        public virtual int DemoMethode(int valeur)
        {
            return 1;
        }
    }
}