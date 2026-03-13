using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier représentant un abonnement finissant, avec le titre de la revue et la date de fin d'abonnement
    /// </summary>
    public class AbonnementFinissant
    {
        /// <summary>
        /// Titre de la revue : titre de la revue pour laquelle l'abonnement est en train de se terminer
        /// </summary>
        public string TitreRevue { get; }
        /// <summary>
        /// Date de fin d'abonnement : date à laquelle l'abonnement prend fin, utilisée pour déterminer si l'abonnement est en train de se terminer ou non
        /// </summary>
        public DateTime DateFinAbonnement { get; }

        /// <summary>
        /// Constructeur de la classe AbonnementFinissant : initialise les propriétés de l'abonnement finissant
        /// </summary>
        /// <param name="titreRevue"></param>
        /// <param name="dateFinAbonnement"></param>
        public AbonnementFinissant(string titreRevue, DateTime dateFinAbonnement)
        {
            this.TitreRevue = titreRevue;
            this.DateFinAbonnement = dateFinAbonnement;
        }
    }
}
