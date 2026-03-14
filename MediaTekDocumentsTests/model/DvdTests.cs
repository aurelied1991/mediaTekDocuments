using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model.Tests
{
    [TestClass()]
    public class DvdTests
    {
        [TestMethod()]
        public void DvdTest()
        {
            string id = "DVD1";
            string titre = "Le Seigneur des Anneaux : La Communauté de l'Anneau";
            string image = "images/dvd1.jpg";
            int duree = 178;
            string realisateur = "Peter Jackson";
            string synopsis = "Un jeune hobbit nommé Frodon Sacquet hérite d'un anneau magique qui doit être détruit pour sauver la Terre du Milieu. Accompagné de ses amis, il entreprend un périlleux voyage pour accomplir cette mission.";
            string idGenre = "001";
            string genre = "Fantastique";
            string idPublic = "P1";
            string lePublic = "Tout public";
            string idRayon = "R1";
            string rayon = "Films";

            Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idGenre, genre, idPublic, lePublic, idRayon, rayon);

            Assert.AreEqual(id, dvd.Id);
            Assert.AreEqual(titre, dvd.Titre);
            Assert.AreEqual(image, dvd.Image);
            Assert.AreEqual(duree, dvd.Duree);
            Assert.AreEqual(realisateur, dvd.Realisateur);
            Assert.AreEqual(synopsis, dvd.Synopsis);
            Assert.AreEqual(idGenre, dvd.IdGenre);
            Assert.AreEqual(genre, dvd.Genre);
            Assert.AreEqual(idPublic, dvd.IdPublic);
            Assert.AreEqual(lePublic, dvd.Public);
            Assert.AreEqual(idRayon, dvd.IdRayon);
            Assert.AreEqual(rayon, dvd.Rayon);

        }
    }
}