using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Controller pour la gestion des documents (livres, dvd, revues) : contient les méthodes de création, modification et vérification d'un id de document
    /// </summary>
    public class FrmGestionDocumentsController
    {
        /// <summary>
        /// Instance de la classe d'accès aux données pour pouvoir appeler les méthodes d'ajout, de modification et de récupération des documents
        /// </summary>
        private Access access = Access.GetInstance();

        /// <summary>
        /// Méthode de création d'un livre : appelle la méthode d'ajout de document de la classe d'accès aux données en précisant le type de document et le livre à ajouter
        /// </summary>
        /// <param name="livre"></param>
        /// <returns></returns>
        public bool CreerLivre(Livre livre)
        {
            return access.AjouterDocument("livre",livre);    
        }

        /// <summary>
        /// Méthode de création d'un dvd : appelle la méthode d'ajout de document de la classe d'accès aux données en précisant le type de document et le dvd à ajouter
        /// </summary>
        /// <param name="dvd"></param>
        /// <returns></returns>
        public bool CreerDvd(Dvd dvd)
        {
            return access.AjouterDocument("dvd", dvd);
        }

        /// <summary>
        /// Méthode de création d'une revue : appelle la méthode d'ajout de document de la classe d'accès aux données en précisant le type de document et la revue à ajouter
        /// </summary>
        /// <param name="revue"></param>
        /// <returns></returns>
        public bool CreerRevue(Revue revue)
        {
            return access.AjouterDocument("revue", revue);
        }

        /// <summary>
        /// Méthode de modification d'un livre : appelle la méthode de modification de document de la classe d'accès aux données en précisant le type de document, le livre à modifier et son id
        /// </summary>
        /// <param name="livre"></param>
        /// <returns></returns>
        public bool ModifierLivre(Livre livre)
        {
            return access.ModifierDocument("livre", livre, livre.Id);
        }

        /// <summary>
        /// Méthode de modification d'un dvd : appelle la méthode de modification de document de la classe d'accès aux données en précisant le type de document, le dvd à modifier et son id
        /// </summary>
        /// <param name="dvd"></param>
        /// <returns></returns>
        public bool ModifierDvd(Dvd dvd)
        {
            return access.ModifierDocument("dvd", dvd, dvd.Id);
        }

        /// <summary>
        /// Méthode de modification d'une revue : appelle la méthode de modification de document de la classe d'accès aux données en précisant le type de document, la revue à modifier et son id
        /// </summary>
        /// <param name="revue"></param>
        /// <returns></returns>
        public bool ModifierRevue(Revue revue)
        {
            return access.ModifierDocument("revue", revue, revue.Id);
        }

        /// <summary>
        /// Méthode de vérification de la disponibilité d'un id de document : vérifie que l'id n'existe pas déjà dans les livres, les dvd et les revues en récupérant tous les documents et en cherchant une correspondance d'id
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
        /// Méthode de récupération de tous les livres : appelle la méthode de récupération de tous les documents de la classe d'accès aux données en précisant le type de document et retourne la liste des livres récupérés
        /// </summary>
        /// <returns></returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// Méthode de récupération de tous les dvd : appelle la méthode de récupération de tous les documents de la classe d'accès aux données en précisant le type de document et retourne la liste des dvd récupérés
        /// </summary>
        /// <returns></returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// Méthode de récupération de toutes les revues : appelle la méthode de récupération de tous les documents de la classe d'accès aux données en précisant le type de document et retourne la liste des revues récupérées
        /// </summary>
        /// <returns></returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }
    }
}
