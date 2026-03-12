using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe représentant un service du réseau MediaTek86, avec un identifiant et un nom.
    /// </summary>
    public class Service
    {
        /// <summary>
        /// Identifiant unique du service, utilisé pour différencier les services entre eux.
        /// </summary>
        public int IdService { get; set; }
        /// <summary>
        /// Nom du service, utilisé pour l'affichage et la compréhension de l'utilisateur.
        /// </summary>
        public string NomService { get; set; }

        /// <summary>
        /// Constructeur de la classe Service, qui initialise les propriétés IdService et NomService avec les valeurs fournies en paramètres.
        /// </summary>
        /// <param name="idService"></param>
        /// <param name="nomService"></param>
        public Service(int idService, string nomService)
        {
            this.IdService = idService;
            this.NomService = nomService;
        }
    }
}
