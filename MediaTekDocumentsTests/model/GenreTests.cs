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
    public class GenreTests
    {
        [TestMethod()]
        public void GenreTest()
        {
            string idGenre = "G01";
            string libelleGenre = "Science-fiction";

            Genre genre = new Genre(idGenre, libelleGenre);

            Assert.AreEqual(idGenre, genre.Id);
            Assert.AreEqual(libelleGenre, genre.Libelle);
        }
    }
}