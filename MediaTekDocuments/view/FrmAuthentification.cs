using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Fenêtre d'authentification permettant à un utilisateur de se connecter à l'application.
    /// </summary>
    public partial class FrmAuthentification : Form
    {
        /// <summary>
        /// Contrôleur gérant la logique d'authentification
        /// </summary>
        private readonly FrmAuthentificationController controller;

        /// <summary>
        /// Initialise la fenêtre d'authentification
        /// </summary>
        public FrmAuthentification()
        {
            InitializeComponent();
            this.controller = new FrmAuthentificationController();
        }

        /// <summary>
        /// Génère le hash SHA256 d'un mot de passe
        /// </summary>
        /// <param name="mdp">Mot de passe en clair</param>
        /// <returns>Mot de passe hashé</returns>
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
        /// Tente d'authentifier l'utilisateur lors du clic sur le bouton de connexion
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
        /// Réinitialise les champs de connexion
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
