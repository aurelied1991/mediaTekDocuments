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
    public class ExemplaireTests
    {
        [TestMethod()]
        public void ExemplaireTest()
        {
            int numero = 1;
            DateTime dateAchat = new DateTime(2026, 3, 13);
            string photo = "images/photo.jpg";
            string idEtat = "E1";
            string idDocument = "D1";
            string libelleEtat = "Bon état";

            Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument, libelleEtat);

            Assert.AreEqual(numero, exemplaire.Numero);
            Assert.AreEqual(dateAchat, exemplaire.DateAchat);
            Assert.AreEqual(photo, exemplaire.Photo);
            Assert.AreEqual(idEtat, exemplaire.IdEtat);
            Assert.AreEqual(idDocument, exemplaire.Id);
            Assert.AreEqual(libelleEtat, exemplaire.LibelleEtat);
        }
    }
}