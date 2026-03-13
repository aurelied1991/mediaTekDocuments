
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Categorie (réunit les informations des classes Public, Genre et Rayon)
    /// </summary>
    public class Categorie
    {
        /// <summary>
        /// Id de la catégorie : clé primaire de la table Categorie
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Libellé de la catégorie : nom de la catégorie (ex : "Roman", "Science-fiction", "Adulte", "Enfant", etc.)
        /// </summary>
        public string Libelle { get; }

        /// <summary>
        /// Constructeur de la classe Categorie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Categorie(string id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }

        /// <summary>
        /// Récupération du libellé pour l'affichage dans les combos
        /// </summary>
        /// <returns>Libelle</returns>
        public override string ToString()
        {
            return this.Libelle;
        }
    }
}
