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
    public class CategorieTests
    {
        [TestMethod()]
        public void CategorieTest()
        {
            string id = "CAT01";
            string libelle = "Informatique";

            Categorie categorie = new Categorie(id, libelle);

            Assert.AreEqual(id, categorie.Id);
            Assert.AreEqual(libelle, categorie.Libelle);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            string id = "CAT01";
            string libelle = "Informatique";
            Categorie categorie = new Categorie(id, libelle);

            string resultat = categorie.ToString();
            Assert.AreEqual(libelle, resultat);
        }
    }
}