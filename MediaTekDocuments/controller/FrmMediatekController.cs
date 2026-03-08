using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;
using System.Linq;
using System;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    public class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }

        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// Supprime un document de la bdd
        /// </summary>
        /// <param name="typeElement"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SupprimerDocument(string typeElement, string id)
        {
            return access.SupprimerDocument(typeElement, id);
        }

        /// <summary>
        /// Récupère un document de la bdd à partir de son id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeElement"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetDocumentById<T>(string typeElement, string id)
        {
            return access.GetDocumentById<T>(typeElement, id);
        }

        /// <summary>
        /// Récupère les commandes associées à un document
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CommandeDocument> GetCommandesByDocument(string id)
        {
            return access.GetCommandesByDocument(id);
        }

        /// <summary>
        /// Récupère la liste des états de suivi
        /// </summary>
        /// <returns></returns>
        public List<Suivi> GetAllSuivis()
        {
            return access.GetAllSuivis();
        }

        /// <summary>
        /// Récupère les abonnements associés à une revue
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
        public List<Abonnement> GetAllAbonnements(string idDocument)
        {
            return access.GetAllAbonnements(idDocument);
        }

        /// <summary>
        /// Récupère les abonnements finissant dans les 30 prochains jours
        /// </summary>
        /// <returns></returns>
        public List<AbonnementFinissant> GetAbonnementsFinissant()
        {
            return access.GetAbonnementsFinissant();
        }

        /// <summary>
        /// Crée une commande de document dans la bdd
        /// </summary>
        /// <param name="commandeDocument"></param>
        /// <returns></returns>
        public bool CreerCommandeDocument(CommandeDocument commandeDocument)
        {
            return access.CreerCommandeDocument(commandeDocument);
        }

        /// <summary>
        /// Modifie l'état de suivi d'une commande de document dans la bdd
        /// </summary>
        /// <param name="idCommandeDocument"></param>
        /// <param name="idSuivi"></param>
        /// <returns></returns>
        public bool ModifierSuiviCommandeDocument(string idCommandeDocument, string idSuivi)
        {
            return access.ModifierSuiviCommande(idCommandeDocument, idSuivi);
        }

        /// <summary>
        /// Supprime une commande de document de la bdd
        /// </summary>
        /// <param name="idCommande"></param>
        /// <returns></returns>
        public bool SupprimerCommande(string idCommande)
        {
            return access.SupprimerCommande(idCommande);
        }

        /// <summary>
        /// Crée un abonnement de revue dans la bdd
        /// </summary>
        /// <param name="abonnement"></param>
        /// <returns></returns>
        public bool CreerAbonnementRevue(Abonnement abonnement)
        {
            return access.CreerAbonnementRevue(abonnement);
        }

        /// <summary>
        /// Supprime un abonnement de revue de la bdd
        /// </summary>
        /// <param name="idCommande"></param>
        /// <returns></returns>
        public bool SupprimerAbonnementRevue(string idCommande)
        {
            return access.SupprimerAbonnementRevue(idCommande);
        }

        /// <summary>
        /// Vérifie si une parution de revue est comprise dans la période d'abonnement d'une revue
        /// </summary>
        /// <param name="dateCommande"></param>
        /// <param name="dateFinAbonnement"></param>
        /// <param name="dateParution"></param>
        /// <returns></returns>
        public bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return dateParution >= dateCommande && dateParution <= dateFinAbonnement;
        }


    }
}
