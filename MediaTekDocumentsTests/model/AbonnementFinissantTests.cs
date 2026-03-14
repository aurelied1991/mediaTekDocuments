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
    public class AbonnementFinissantTests
    {
        [TestMethod()]
        public void AbonnementFinissantTest()
        {
            string titre = "Découvrir le Monde";
            DateTime dateFinAbonnement = new DateTime(2026, 12, 31);

            AbonnementFinissant abonnementFinissant = new AbonnementFinissant(titre, dateFinAbonnement);

            Assert.AreEqual(titre, abonnementFinissant.TitreRevue);
            Assert.AreEqual(dateFinAbonnement, abonnementFinissant.DateFinAbonnement);
        }
    }
}