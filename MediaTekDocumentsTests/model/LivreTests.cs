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
    public class LivreTests
    {
        [TestMethod()]
        public void LivreTest()
        {
            string id = "L1";
            string titre = "Le Seigneur des Anneaux";
            string image = "images/seigneur_des_anneaux.jpg";
            string isbn = "978-0261102385";
            string auteur = "J.R.R. Tolkien";
            string collection = "Fantasy";
            string idGenre = "G1";
            string libelleGenre = "Fantasy";
            string idPublic = "P1";
            string libellePublic = "Adulte";
            string idRayon = "R1";
            string libelleRayon = "Fiction";

            Livre livre = new Livre(id, titre, image, isbn, auteur, collection, idGenre, libelleGenre, idPublic, libellePublic, idRayon, libelleRayon);

            Assert.AreEqual(id, livre.Id);
            Assert.AreEqual(titre, livre.Titre);
            Assert.AreEqual(image, livre.Image);
            Assert.AreEqual(isbn, livre.Isbn);
            Assert.AreEqual(auteur, livre.Auteur);
            Assert.AreEqual(collection, livre.Collection);
            Assert.AreEqual(idGenre, livre.IdGenre);
            Assert.AreEqual(libelleGenre, livre.Genre);
            Assert.AreEqual(idPublic, livre.IdPublic);
            Assert.AreEqual(libellePublic, livre.Public);
            Assert.AreEqual(idRayon, livre.IdRayon);
            Assert.AreEqual(libelleRayon, livre.Rayon);
        }
    }
}