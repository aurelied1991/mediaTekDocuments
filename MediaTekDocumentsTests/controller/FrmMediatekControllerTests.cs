using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.controller.Tests
{
    [TestClass()]
    public class FrmMediatekControllerTests
    {
        private readonly FrmMediatekController controller = new FrmMediatekController();

        private readonly DateTime dateCommande = new DateTime(2026, 2, 1);
        private readonly DateTime dateFinAbonnement = new DateTime(2026, 2, 28);

        /// <summary>
        /// Test de la méthode ParutionDansAbonnement pour vérifier que la date de parution est considérée comme valide si elle se situe entre la date de commande et la date de fin d'abonnement
        /// </summary>
        [TestMethod]
        public void ParutionEstValideSiEntreDateCommandeEtFinAbonnement()
        {
            DateTime dateParution = new DateTime(2026, 2, 15);

            bool resultat = controller.ParutionDansAbonnement(
                dateCommande,
                dateFinAbonnement,
                dateParution
            );

            Assert.IsTrue(resultat);
        }

        /// <summary>
        /// Test de la méthode ParutionDansAbonnement pour vérifier que la date de parution est considérée comme valide si elle est égale à la date de commande ou à la date de fin d'abonnement
        /// </summary>
        [TestMethod]
        public void ParutionEstValideSiEgaleDateCommande()
        {
            bool resultat = controller.ParutionDansAbonnement(
                dateCommande,
                dateFinAbonnement,
                dateCommande
            );

            Assert.IsTrue(resultat);
        }

        /// <summary>
        /// Test de la méthode ParutionDansAbonnement pour vérifier que la date de parution est considérée comme valide si elle est égale à la date de fin d'abonnement
        /// </summary>
        [TestMethod]
        public void ParutionEstValideSiEgaleDateFinAbonnement()
        {
            bool resultat = controller.ParutionDansAbonnement(
                dateCommande,
                dateFinAbonnement,
                dateFinAbonnement
            );

            Assert.IsTrue(resultat);
        }

        /// <summary>
        /// Test de la méthode ParutionDansAbonnement pour vérifier que la date de parution est considérée comme invalide si elle se situe avant la date de commande ou après la date de fin d'abonnement
        /// </summary>
        [TestMethod]
        public void ParutionEstInvalideSiAvantCommande()
        {
            DateTime dateParution = new DateTime(2026, 1, 1);

            bool resultat = controller.ParutionDansAbonnement(
                dateCommande,
                dateFinAbonnement,
                dateParution
            );

            Assert.IsFalse(resultat);
        }

        /// <summary>
        /// Test de la méthode ParutionDansAbonnement pour vérifier que la date de parution est considérée comme invalide si elle se situe après la date de fin d'abonnement
        /// </summary>
        [TestMethod]
        public void ParutionEstInvalideSiApresFinAbonnement()
        {
            DateTime dateParution = new DateTime(2026, 3, 1);

            bool resultat = controller.ParutionDansAbonnement(
                dateCommande,
                dateFinAbonnement,
                dateParution
            );

            Assert.IsFalse(resultat);
        }
    }
}