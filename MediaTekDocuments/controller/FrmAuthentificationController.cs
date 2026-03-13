using MediaTekDocuments.dal;
using MediaTekDocuments.model;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur de la fenêtre d'authentification, responsable de gérer la logique d'authentification des utilisateurs en interagissant avec la couche d'accès aux données (DAL) pour vérifier les informations d'identification fournies par l'utilisateur et retourner les informations de l'utilisateur authentifié si les informations sont correctes.
    /// </summary>
    public class FrmAuthentificationController
    {
        /// <summary>
        /// Instance de la classe Access, utilisée pour accéder aux méthodes d'authentification et de gestion des utilisateurs dans la couche d'accès aux données (DAL). Cette instance est obtenue via le pattern Singleton pour garantir qu'il n'y a qu'une seule instance de Access utilisée dans toute l'application, assurant ainsi une gestion centralisée des accès aux données et une meilleure performance.
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Constructeur de la classe FrmAuthentificationController, qui initialise l'instance de Access en utilisant le pattern Singleton pour garantir une gestion centralisée des accès aux données dans l'application. Cette approche permet d'assurer que toutes les interactions avec la couche d'accès aux données sont cohérentes et optimisées, tout en facilitant la maintenance et l'évolution de l'application en centralisant la logique d'accès aux données dans une seule instance partagée.
        /// </summary>
        public FrmAuthentificationController()
        {
            // On récupère l’instance unique de Access
            access = Access.GetInstance();
        }

        /// <summary>
        /// Méthode d'authentification des utilisateurs, qui prend en paramètre le login et le mot de passe fournis par l'utilisateur, et utilise la méthode AuthentifierUtilisateur de la classe Access pour vérifier les informations d'identification. Si les informations sont correctes, la méthode retourne un objet Utilisateur contenant les informations de l'utilisateur authentifié, sinon elle retourne null. Cette méthode centralise toute la logique d'authentification, ce qui permet de maintenir une séparation claire entre la logique métier et la logique d'accès aux données, facilitant ainsi la maintenance et l'évolution de l'application.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Utilisateur AuthentifierUtilisateur(string login, string password)
        {
            return access.AuthentifierUtilisateur(login, password);
        }
    }
}
