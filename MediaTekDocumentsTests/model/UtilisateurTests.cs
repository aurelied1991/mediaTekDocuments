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
    public class UtilisateurTests
    {
        [TestMethod()]
        public void UtilisateurTest()
        {
            string id = "u123";
            string login = "user123";
            string motDePasse = "motdepasse";
            int idService = 1;

            Utilisateur utilisateur = new Utilisateur(id, login, motDePasse, idService);

            Assert.AreEqual(id, utilisateur.Id);
            Assert.AreEqual(login, utilisateur.Login);
            Assert.AreEqual(motDePasse, utilisateur.MotDePasse);
            Assert.AreEqual(idService, utilisateur.IdService);
        }
    }
}