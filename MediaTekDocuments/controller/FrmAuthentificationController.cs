using MediaTekDocuments.dal;
using MediaTekDocuments.model;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur de la fenêtre d'authentification qui gère la logique d'authentification et retourne l'utilisateur si les identifiants sont corrects.
    ///</summary>
    public class FrmAuthentificationController
    {
        /// <summary>
        /// Instance de la classe Access
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Initialise le contrôleur et récupère l'instance unique de Access.
        /// </summary>
        public FrmAuthentificationController()
        {
            // On récupère l’instance unique de Access
            access = Access.GetInstance();
        }

        /// <summary>
        /// Méthode d'authentification des utilisateurs avec le login et le mdp fournis
        /// </summary>
        /// <param name="login">Login de l'utilisateur</param>
        /// <param name="password">Mot de passe de l'utilisateur</param>
        /// <returns></returns>
        public Utilisateur AuthentifierUtilisateur(string login, string password)
        {
            return access.AuthentifierUtilisateur(login, password);
        }
    }
}
