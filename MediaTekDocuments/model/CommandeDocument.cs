using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier CommandeDocument hérite de Commande : contient des propriétés spécifiques à une commande de document
    /// </summary>
    public class CommandeDocument : Commande
    {
        /// <summary>
        /// Nombre d'exemplaires commandés : nombre d'exemplaires du document commandés
        /// </summary>
        public int NbExemplaire { get; }
        /// <summary>
        /// Id du livre ou du dvd commandé : clé étrangère de la table CommandeDocument vers la table LivreDvd
        /// </summary>
        public string IdLivreDvd { get; }
        /// <summary>
        /// Id du suivi de la commande : clé étrangère de la table CommandeDocument vers la table Suivi
        /// </summary>
        public string IdSuivi { get; }
        /// <summary>
        /// Libellé du suivi de la commande : libellé du suivi de la commande (en cours, livré, annulé, etc.)
        /// </summary>
        public string LibelleSuivi { get; }

        /// <summary>
        /// Constructeur de la classe CommandeDocument : initialise les propriétés de la commande de document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateCommande"></param>
        /// <param name="montant"></param>
        /// <param name="nbExemplaire"></param>
        /// <param name="idLivreDvd"></param>
        /// <param name="idSuivi"></param>
        /// <param name="libelleSuivi"></param>
        public CommandeDocument(string id, DateTime dateCommande, double montant, int nbExemplaire, string idLivreDvd, string idSuivi, string libelleSuivi) : base(id, dateCommande, montant)
        {
            this.NbExemplaire = nbExemplaire;
            this.IdLivreDvd = idLivreDvd;
            this.IdSuivi = idSuivi;
            this.LibelleSuivi = libelleSuivi;
        }
    }
}
