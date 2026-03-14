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
    public class RevueTests
    {
        [TestMethod()]
        public void RevueTest()
        {
            string idRevue = "R001";
            string titre = "Science et Vie";
            string image = "images/science_et_vie.jpg";
            string idGenre = "G001";
            string genre = "Science";
            string idPublic = "P001";
            string lePublic = "Adulte";
            string idRayon = "R001";
            string rayon = "Revues";
            string periodicite = "Mensuelle";
            int delaiMiseADispo = 7;

            Revue revue = new Revue(idRevue, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon, periodicite, delaiMiseADispo);

            Assert.AreEqual(idRevue, revue.Id);
            Assert.AreEqual(titre, revue.Titre);
            Assert.AreEqual(image, revue.Image);
            Assert.AreEqual(idGenre, revue.IdGenre);
            Assert.AreEqual(genre, revue.Genre);
            Assert.AreEqual(idPublic, revue.IdPublic);
            Assert.AreEqual(lePublic, revue.Public);
            Assert.AreEqual(idRayon, revue.IdRayon);
            Assert.AreEqual(rayon, revue.Rayon);
            Assert.AreEqual(periodicite, revue.Periodicite);
            Assert.AreEqual(delaiMiseADispo, revue.DelaiMiseADispo);
        }
    }
}