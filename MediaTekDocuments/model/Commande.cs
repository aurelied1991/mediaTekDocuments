using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Commande : contient des propriétés spécifiques à une commande
    /// </summary>
    public class Commande
    {
        /// <summary>
        /// Id de la commande : clé primaire de la table Commande
        /// </summary>
        public string IdCommande { get; set; }
        /// <summary>
        /// Date de la commande : date à laquelle la commande a été passée
        /// </summary>
        public DateTime DateCommande { get; set; }
        /// <summary>
        /// Montant de la commande : montant total de la commande
        /// </summary>
        public double Montant { get; set; }

        /// <summary>
        /// Constructeur de la classe Commande : initialise les propriétés de la commande
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateCommande"></param>
        /// <param name="montant"></param>
        public Commande(string id, DateTime dateCommande, double montant)
        {
            this.IdCommande = id;
            this.DateCommande = dateCommande;
            this.Montant = montant;
        }
    }
}
