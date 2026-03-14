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
    public class CommandeTests
    {
        [TestMethod()]
        public void CommandeTest()
        {
            string idCommande = "C001";
            DateTime dateCommande = new DateTime(2026, 3, 13);
            double montant = 150;

            Commande commande = new Commande(idCommande, dateCommande, montant);
            Assert.AreEqual(idCommande, commande.IdCommande);
            Assert.AreEqual(dateCommande, commande.DateCommande);
            Assert.AreEqual(montant, commande.Montant);
        }
    }
}