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
    public class DocumentTests
    {
        [TestMethod()]
        public void DocumentTest()
        {
            string id = "D001";
            string titre = "Le Petit Prince";
            string image = "images/le_petit_prince.jpg";
            string idGenre = "G001";
            string libelleGenre = "Roman";
            string idPublic = "P001";
            string libellePublic = "Jeunesse";
            string idRayon = "R001";
            string libelleRayon = "Fiction";

            Document document = new Document(id, titre, image, idGenre, libelleGenre, idPublic, libellePublic, idRayon, libelleRayon);

            Assert.AreEqual(id, document.Id);
            Assert.AreEqual(titre, document.Titre);
            Assert.AreEqual(image, document.Image);
            Assert.AreEqual(idGenre, document.IdGenre);
            Assert.AreEqual(libelleGenre, document.Genre);
            Assert.AreEqual(idPublic, document.IdPublic);
            Assert.AreEqual(libellePublic, document.Public);
            Assert.AreEqual(idRayon, document.IdRayon);
            Assert.AreEqual(libelleRayon, document.Rayon);

        }
    }
}