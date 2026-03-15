using Newtonsoft.Json;
using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Exemplaire (exemplaire d'une revue)
    /// </summary>
    public class Exemplaire
    {
        /// <summary>
        /// Numéro de l'exemplaire
        /// </summary>
        public int Numero { get; set; }
        /// <summary>
        /// Photo de l'exemplaire
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// Date d'achat de l'exemplaire
        /// </summary>
        public DateTime DateAchat { get; set; }
        /// <summary>
        /// Id de l'état de l'exemplaire
        /// </summary>
        public string IdEtat { get; set; }
        /// <summary>
        /// Id du document de l'exemplaire
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Libellé de l'état de l'exemplaire
        /// </summary>
        [JsonIgnore] public string LibelleEtat { get; set; }

        /// <summary>
        /// Constructeur de la classe Exemplaire
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="dateAchat"></param>
        /// <param name="photo"></param>
        /// <param name="idEtat"></param>
        /// <param name="idDocument"></param>
        /// <param name="libelleEtat"></param>
        public Exemplaire(int numero, DateTime dateAchat, string photo, string idEtat, string idDocument, string libelleEtat)
        {
            this.Numero = numero;
            this.DateAchat = dateAchat;
            this.Photo = photo;
            this.IdEtat = idEtat;
            this.Id = idDocument;
            this.LibelleEtat = libelleEtat;
        }
    }
}
