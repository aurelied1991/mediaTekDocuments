namespace MediaTekDocuments.model
{
    /// <summary>
    /// Représente un utilisateur de l'application, avec ses informations d'identification et son service associé.
    /// </summary>
    public class Utilisateur
    {
        /// <summary>
        /// Identifiant unique de l'utilisateur
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nom de connexion de l'utilisateur, utilisé pour l'authentification
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Mot de passe de l'utilisateur, stocké de manière sécurisée (hashé) pour protéger les informations d'identification
        /// </summary>
        public string MotDePasse { get; set; }
        /// <summary>
        /// Identifiant du service auquel l'utilisateur appartient, utilisé pour déterminer les droits d'accès et les fonctionnalités disponibles dans l'application
        /// </summary>
        public int IdService { get; set; }

        /// <summary>
        /// Constructeur de la classe Utilisateur, permettant d'initialiser les propriétés de l'utilisateur lors de sa création
        /// </summary>
        /// <param name="id"></param>
        /// <param name="login"></param>
        /// <param name="motDePasse"></param>
        /// <param name="idService"></param>
        public Utilisateur(string id, string login, string motDePasse, int idService)
        {
            this.Id = id;
            this.Login = login;
            this.MotDePasse = motDePasse;
            this.IdService = idService;
        }
    }
}
