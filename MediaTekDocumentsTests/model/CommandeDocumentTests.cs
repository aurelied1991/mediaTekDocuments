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
    public class CommandeDocumentTests
    {
        [TestMethod()]
        public void CommandeDocumentTest()
        {
            string idCommande = "1";
            DateTime dateCommande = new DateTime(2026, 3, 13);
            double montant = 100.0;
            int nbExemplaires = 5;
            string idLivreDvd = "L1";
            string idSuivi = "S1";
            string libelleSuivi = "En cours";

            CommandeDocument commande = new CommandeDocument(idCommande, dateCommande, montant, nbExemplaires, idLivreDvd, idSuivi, libelleSuivi);

            Assert.AreEqual(idCommande, commande.IdCommande);
            Assert.AreEqual(dateCommande, commande.DateCommande);
            Assert.AreEqual(montant, commande.Montant);
            Assert.AreEqual(nbExemplaires, commande.NbExemplaire);
            Assert.AreEqual(idLivreDvd, commande.IdLivreDvd);
            Assert.AreEqual(idSuivi, commande.IdSuivi);
            Assert.AreEqual(libelleSuivi, commande.LibelleSuivi);
        }
    }
}