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
    public class PublicTests
    {
        [TestMethod()]
        public void PublicTest()
        {
            string id = "1";
            string libelle = "Adulte";

            Public lePublic = new Public(id, libelle);

            Assert.AreEqual(id, lePublic.Id);
            Assert.AreEqual(libelle, lePublic.Libelle);
        }
    }
}