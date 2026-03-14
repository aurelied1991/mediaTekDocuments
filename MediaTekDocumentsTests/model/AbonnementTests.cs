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
    public class AbonnementTests
    {
        [TestMethod()]
        public void AbonnementTest()
        {
            string id = "01";
            DateTime dateCommande = new DateTime(2026, 1, 1);
            double montant = 100.0;
            string idRevue = "001";
            DateTime dateFinAbonnement = new DateTime(2026, 12, 31);

            Abonnement abonnement = new Abonnement(id, dateCommande, montant, idRevue, dateFinAbonnement);

            Assert.AreEqual(id, abonnement.IdCommande);
            Assert.AreEqual(dateCommande, abonnement.DateCommande);
            Assert.AreEqual(montant, abonnement.Montant);
            Assert.AreEqual(idRevue, abonnement.IdRevue);
            Assert.AreEqual(dateFinAbonnement, abonnement.DateFinAbonnement);
        }
    }
}