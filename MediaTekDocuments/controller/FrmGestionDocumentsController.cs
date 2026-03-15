using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System.Collections.Generic;
using System.Linq;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Controller pour la gestion des documents (livres, dvd, revues)
    /// </summary>
    public class FrmGestionDocumentsController
    {
        /// <summary>
        /// Instance de la classe d'accès aux données
        /// </summary>
        private readonly Access access = Access.GetInstance();

        /// <summary>
        /// Ajoute un livre
        /// </summary>
        /// <param name="livre"></param>
        /// <returns></returns>
        public bool CreerLivre(Livre livre)
        {
            return access.AjouterDocument("livre", livre);
        }

        /// <summary>
        ///Ajoute un dvd
        /// </summary>
        /// <param name="dvd"></param>
        /// <returns></returns>
        public bool CreerDvd(Dvd dvd)
        {
            return access.AjouterDocument("dvd", dvd);
        }

        /// <summary>
        /// Ajoute une revue
        /// </summary>
        /// <param name="revue"></param>
        /// <returns></returns>
        public bool CreerRevue(Revue revue)
        {
            return access.AjouterDocument("revue", revue);
        }

        /// <summary>
        /// Modifie un livre existant
        /// </summary>
        /// <param name="livre"></param>
        /// <returns></returns>
        public bool ModifierLivre(Livre livre)
        {
            return access.ModifierDocument("livre", livre, livre.Id);
        }

        /// <summary>
        /// Modifie un dvd existant
        /// </summary>
        /// <param name="dvd"></param>
        /// <returns></returns>
        public bool ModifierDvd(Dvd dvd)
        {
            return access.ModifierDocument("dvd", dvd, dvd.Id);
        }

        /// <summary>
        /// Modifie une revue existante
        /// </summary>
        /// <param name="revue"></param>
        /// <returns></returns>
        public bool ModifierRevue(Revue revue)
        {
            return access.ModifierDocument("revue", revue, revue.Id);
        }

        /// <summary>
        /// Vérifie la disponibilité d'un id de document
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool VerifierIdDisponible(string id)
        {
            bool existe =
                GetAllLivres().Any(d => d.Id == id) ||
                GetAllDvd().Any(d => d.Id == id) ||
                GetAllRevues().Any(d => d.Id == id);

            return !existe;
        }

        /// <summary>
        /// Récupère tous les livres
        /// </summary>
        /// <returns></returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// Récupère tous les dvd
        /// </summary>
        /// <returns></returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// Récupère toutes les revues
        /// <returns></returns>
        /// </summary>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }
    }
}
