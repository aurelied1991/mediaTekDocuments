namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Suivi : contient des propriétés spécifiques au suivi d'une commande
    /// </summary>
    public class Suivi
    {
        /// <summary>
        /// Id du suivi de la commande
        /// </summary>
        public string IdSuivi { get; set; }
        /// <summary>
        /// Libellé du suivi de la commande
        /// </summary>
        public string LibelleSuivi { get; set; }

        /// <summary>
        /// Constructeur de la classe Suivi (initialise un état de suivi)
        /// </summary>
        /// <param name="idSuivi"></param>
        /// <param name="libelleSuivi"></param>
        public Suivi(string idSuivi, string libelleSuivi)
        {
            this.IdSuivi = idSuivi;
            this.LibelleSuivi = libelleSuivi;
        }
    }
}
