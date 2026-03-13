using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Abonnement : contient des propriétés spécifiques à un abonnement, qui hérite de la classe Commande
    /// </summary>
    public class Abonnement : Commande
    {
        /// <summary>
        /// Id de la revue : clé étrangère de la table Abonnement, référence à la table Revue
        /// </summary>
        public string IdRevue { get; }
        /// <summary>
        /// Date de fin d'abonnement : date à laquelle l'abonnement prend fin
        /// </summary>
        public DateTime DateFinAbonnement { get; }

        /// <summary>
        /// Constructeur de la classe Abonnement : initialise les propriétés de l'abonnement, en appelant le constructeur de la classe Commande pour initialiser les propriétés communes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateCommande"></param>
        /// <param name="montant"></param>
        /// <param name="idRevue"></param>
        /// <param name="dateFinAbonnement"></param>
        public Abonnement(string id, DateTime dateCommande, double montant, string idRevue, DateTime dateFinAbonnement) : base(id, dateCommande, montant)
        {
            this.IdRevue = idRevue;
            this.DateFinAbonnement = dateFinAbonnement;
        }
    }
}
