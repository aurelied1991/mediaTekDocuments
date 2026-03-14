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
    public class RayonTests
    {
        [TestMethod()]
        public void RayonTest()
        {
            string id = "R1";
            string libelle = "Fiction";

            Rayon rayon = new Rayon(id, libelle);

            Assert.AreEqual(id, rayon.Id);
            Assert.AreEqual(libelle, rayon.Libelle);
        }
    }
}