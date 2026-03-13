
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Livre hérite de LivreDvd : contient des propriétés spécifiques aux livres
    /// </summary>
    public class Livre : LivreDvd
    {
        /// <summary>
        /// Propriété spécifique à la classe Livre : le numéro ISBN du livre
        /// </summary>
        public string Isbn { get; }
        /// <summary>
        /// Propriété spécifique à la classe Livre : le nom de l'auteur du livre
        /// </summary>
        public string Auteur { get; }
        /// <summary>
        /// Propriété spécifique à la classe Livre : le nom de la collection du livre
        /// </summary>
        public string Collection { get; }

        /// <summary>
        /// Constructeur de la classe Livre : appelle le constructeur de la classe mère LivreDvd pour initialiser les propriétés communes à tous les documents
        /// </summary>
        /// <param name="id"></param>
        /// <param name="titre"></param>
        /// <param name="image"></param>
        /// <param name="isbn"></param>
        /// <param name="auteur"></param>
        /// <param name="collection"></param>
        /// <param name="idGenre"></param>
        /// <param name="genre"></param>
        /// <param name="idPublic"></param>
        /// <param name="lePublic"></param>
        /// <param name="idRayon"></param>
        /// <param name="rayon"></param>
        public Livre(string id, string titre, string image, string isbn, string auteur, string collection,
            string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            this.Isbn = isbn;
            this.Auteur = auteur;
            this.Collection = collection;
        }
    }
}
