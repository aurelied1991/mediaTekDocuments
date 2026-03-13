using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Fenêtre d'authentification de l'application, permettant aux utilisateurs de saisir leurs informations d'identification (login et mot de passe) pour accéder à l'application. Cette fenêtre interagit avec le contrôleur FrmAuthentificationController pour gérer la logique d'authentification, en vérifiant les informations fournies par l'utilisateur et en déterminant si l'accès doit être accordé ou refusé en fonction des données stockées dans la base de données. En cas de succès, la fenêtre redirige l'utilisateur vers la fenêtre principale de l'application (FrmMediatek) en passant les informations de l'utilisateur authentifié, tandis qu'en cas d'échec, elle affiche un message d'erreur approprié pour informer l'utilisateur des problèmes d'authentification.
    /// </summary>
    public partial class FrmAuthentification : Form
    {
        /// <summary>
        /// Instance du contrôleur FrmAuthentificationController, utilisée pour gérer la logique d'authentification des utilisateurs en interagissant avec la couche d'accès aux données (DAL) pour vérifier les informations d'identification fournies par l'utilisateur et retourner les informations de l'utilisateur authentifié si les informations sont correctes. Cette instance est initialisée dans le constructeur de la classe FrmAuthentification, assurant ainsi que la logique d'authentification est prête à être utilisée dès que la fenêtre est affichée.
        /// </summary>
        private readonly FrmAuthentificationController controller;

        /// <summary>
        /// Constructeur de la classe FrmAuthentification, qui initialise les composants de la fenêtre et crée une instance du contrôleur FrmAuthentificationController pour gérer la logique d'authentification des utilisateurs. Cette initialisation garantit que la fenêtre est prête à être utilisée pour l'authentification dès son affichage, en permettant aux utilisateurs de saisir leurs informations d'identification et de les vérifier contre les données stockées dans la base de données via le contrôleur.
        /// </summary>
        public FrmAuthentification()
        {
            InitializeComponent();
            this.controller = new FrmAuthentificationController();
        }

        /// <summary>
        /// Méthode pour hasher les mots de passes saisis par les utilisateurs, utilisant l'algorithme de hachage SHA256 pour transformer le mot de passe en une chaîne de caractères sécurisée. Cette méthode prend en paramètre le mot de passe en clair saisi par l'utilisateur, le convertit en un tableau de bytes, puis applique l'algorithme de hachage pour obtenir une représentation sécurisée du mot de passe. Le résultat est ensuite converti en une chaîne hexadécimale pour être stocké ou comparé avec les données de la base de données, assurant ainsi que les mots de passe ne sont jamais stockés ou transmis en clair, renforçant la sécurité de l'application contre les attaques potentielles visant à compromettre les informations d'identification des utilisateurs.
        /// </summary>
        /// <param name="mdp"></param>
        /// <returns></returns>
        public static string HasherMotDePasse(string mdp)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(mdp);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le clic sur le bouton de validation de la connexion, qui récupère les informations d'identification saisies par l'utilisateur (login et mot de passe), vérifie qu'elles ne sont pas vides, puis utilise le contrôleur FrmAuthentificationController pour authentifier l'utilisateur. Si l'authentification est réussie, la méthode vérifie le service de l'utilisateur pour déterminer s'il a accès à l'application, puis redirige l'utilisateur vers la fenêtre principale (FrmMediatek) en passant les informations de l'utilisateur authentifié. En cas d'échec de l'authentification ou si le service de l'utilisateur n'a pas accès à l'application, la méthode affiche un message d'erreur approprié pour informer l'utilisateur des problèmes d'authentification ou d'accès, assurant ainsi une expérience utilisateur claire et sécurisée lors du processus de connexion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValiderConnexion_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtMdp.Text;
            txtLogin.Focus();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Veuillez saisir votre login et votre mot de passe");
                return;
            }

            Utilisateur utilisateurConnecte = controller.AuthentifierUtilisateur(login, password);

            if (utilisateurConnecte != null)
            {
                if (utilisateurConnecte.IdService == 4) // Culture
                {
                    MessageBox.Show("Votre service n'a pas accès à cette application.");
                    Application.Exit();
                    return;
                }

                FrmMediatek frm = new FrmMediatek(utilisateurConnecte);
                this.Hide();
                frm.ShowDialog();
                this.Show();
                txtLogin.Clear();
                txtMdp.Clear();
                txtLogin.Focus();
            }
            else
            {
                MessageBox.Show("Login ou mot de passe incorrect");
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le clic sur le bouton d'annulation de la connexion, qui efface les champs de saisie du login et du mot de passe, puis remet le focus sur le champ de saisie du login pour permettre à l'utilisateur de saisir à nouveau ses informations d'identification. Cette méthode offre une fonctionnalité simple pour réinitialiser les champs de connexion en cas d'erreur ou si l'utilisateur souhaite annuler sa tentative de connexion, améliorant ainsi l'expérience utilisateur en facilitant la correction des erreurs de saisie ou en permettant une nouvelle tentative de connexion sans avoir à redémarrer l'application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerConnexion_Click(object sender, EventArgs e)
        {
            txtLogin.Clear();
            txtMdp.Clear();
            txtLogin.Focus();
        }
    }
}
