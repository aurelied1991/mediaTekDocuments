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
    public class SuiviTests
    {
        [TestMethod()]
        public void SuiviTest()
        {
            string idSuivi = "S1";
            string libelleSuivi = "En cours de traitement";

            Suivi suivi = new Suivi(idSuivi, libelleSuivi);

            Assert.AreEqual(idSuivi, suivi.IdSuivi);
            Assert.AreEqual(libelleSuivi, suivi.LibelleSuivi);
        }
    }
}