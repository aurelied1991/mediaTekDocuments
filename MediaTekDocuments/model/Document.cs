
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Document (réunit les infomations communes à tous les documents : Livre, Revue, Dvd)
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Id du document : clé primaire de la table LivreDvd, Revue, etc. (en fonction du type de document)
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Titre du document : titre du livre, de la revue ou du dvd
        /// </summary>
        public string Titre { get; }
        /// <summary>
        /// Image du document : chemin de l'image du livre, de la revue ou du dvd (stockée dans le dossier "images" du projet)
        /// </summary>
        public string Image { get; }
        /// <summary>
        /// Id du genre du document : clé étrangère de la table LivreDvd, Revue, etc. vers la table Genre
        /// </summary>
        public string IdGenre { get; }
        /// <summary>
        /// Libellé du genre du document : libellé du genre du livre, de la revue ou du dvd (roman, science-fiction, etc.)
        /// </summary>
        public string Genre { get; }
        /// <summary>
        /// Id du public du document : clé étrangère de la table LivreDvd, Revue, etc. vers la table Public
        /// </summary>
        public string IdPublic { get; }
        /// <summary>
        /// Libellé du public du document : libellé du public du livre, de la revue ou du dvd (enfant, adolescent, adulte, etc.)
        /// </summary>
        public string Public { get; }
        /// <summary>
        /// Id du rayon du document : clé étrangère de la table LivreDvd, Revue, etc. vers la table Rayon
        /// </summary>
        public string IdRayon { get; }
        /// <summary>
        /// Libellé du rayon du document : libellé du rayon du livre, de la revue ou du dvd (fiction, non-fiction, etc.)
        /// </summary>
        public string Rayon { get; }

        /// <summary>
        /// Constructeur de la classe Document : initialise les propriétés du document (id, titre, image, idGenre, genre, idPublic, public, idRayon, rayon)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="titre"></param>
        /// <param name="image"></param>
        /// <param name="idGenre"></param>
        /// <param name="genre"></param>
        /// <param name="idPublic"></param>
        /// <param name="lePublic"></param>
        /// <param name="idRayon"></param>
        /// <param name="rayon"></param>
        public Document(string id, string titre, string image, string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
        {
            Id = id;
            Titre = titre;
            Image = image;
            IdGenre = idGenre;
            Genre = genre;
            IdPublic = idPublic;
            Public = lePublic;
            IdRayon = idRayon;
            Rayon = rayon;
        }
    }
}
