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
    public class EtatTests
    {
        [TestMethod()]
        public void EtatTest()
        {
            string id = "1";
            string libelle = "Neuf";

            Etat etat = new Etat(id, libelle);

            Assert.AreEqual(id, etat.Id);
            Assert.AreEqual(libelle, etat.Libelle);
        }
    }
}