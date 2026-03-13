
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Revue hérite de Document : contient des propriétés spécifiques aux revues
    /// </summary>
    public class Revue : Document
    {
        /// <summary>
        /// Périodicité de la revue : périodicité de la revue (hebdomadaire, mensuelle, etc.)
        /// </summary>
        public string Periodicite { get; set; }
        /// <summary>
        /// Délai de mise à disposition de la revue : délai de mise à disposition de la revue (en jours) après sa parution
        /// </summary>
        public int DelaiMiseADispo { get; set; }

        /// <summary>
        /// Constructeur de la classe Revue : initialise les propriétés du document (id, titre, image, idGenre, genre, idPublic, public, idRayon, rayon) et les propriétés spécifiques à la revue (periodicite, delaiMiseADispo)
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
        /// <param name="periodicite"></param>
        /// <param name="delaiMiseADispo"></param>
        public Revue(string id, string titre, string image, string idGenre, string genre,
            string idPublic, string lePublic, string idRayon, string rayon,
            string periodicite, int delaiMiseADispo)
             : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            Periodicite = periodicite;
            DelaiMiseADispo = delaiMiseADispo;
        }
    }
}
