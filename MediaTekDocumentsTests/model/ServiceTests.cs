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
    public class ServiceTests
    {
        [TestMethod()]
        public void ServiceTest()
        {
            int idService = 1;
            string nomService = "Prêt";

            Service service = new Service(idService, nomService);

            Assert.AreEqual(idService, service.IdService);
            Assert.AreEqual(nomService, service.NomService);
        }
    }
}