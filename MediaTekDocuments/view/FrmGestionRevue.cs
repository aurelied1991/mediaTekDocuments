using MediaTekDocuments.controller;
using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    public partial class FrmGestionRevue : Form
    {
        /// <summary>
        /// Controller pour la gestion des documents : contient les méthodes de création, modification et vérification d'un id de document
        /// </summary>
        private FrmGestionDocumentsController controllerRevue;
        /// <summary>
        /// Booléen pour différencier les cas d'ajout et de modification d'une revue : true si modification, false si ajout
        /// </summary>
        private bool modifRevue = false;
        /// <summary>
        /// Instance de la revue actuelle pour stocker les informations de la revue à modifier et les comparer avec les nouvelles valeurs saisies par l'utilisateur avant de lancer la modification dans la base de données
        /// </summary>
        private Revue revueActuelle = null;
        /// <summary>
        /// Instance de la classe d'accès aux données pour pouvoir appeler les méthodes d'ajout, de modification et de récupération des documents, des genres, des publics et des rayons
        /// </summary>
        private Access access = Access.GetInstance();

        /// <summary>
        /// Constructeur de la fenêtre de gestion d'une revue : différencie les cas d'ajout et de modification en fonction de la présence ou non d'une revue en paramètre, remplit les champs de la fenêtre avec les informations de la revue à modifier et adapte le titre et le texte du bouton de validation en conséquence
        /// </summary>
        /// <param name="revue"></param>
        public FrmGestionRevue(Revue revue = null)
        {
            InitializeComponent();
            controllerRevue = new FrmGestionDocumentsController();
            if (revue != null)
            {
                modifRevue = true;
                revueActuelle = revue;

                RemplirChamps(revue);
                txtAjoutNumero.ReadOnly = true;
                btnValiderAjoutRevue.Text = "Modifier la revue";
                lblTitreFormRevue.Text = "Modification de la revue";
                this.Text = "Modification de la revue";
            }
            else
            {
                modifRevue = false;
                lblTitreFormRevue.Text = "Ajout d'une nouvelle revue";
            }
        }

        /// <summary>
        /// Méthode d'initialisation de la fenêtre : remplit les combobox avec les données des genres, des publics et des rayons récupérées dans la base de données, et sélectionne les éléments correspondants aux informations de la revue à modifier si on est en cas de modification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmGestionRevue_Load(object sender, EventArgs e)
        {
            var access = Access.GetInstance();

            cboGenre.DataSource = access.GetAllGenres();
            cboGenre.DisplayMember = "Libelle";
            cboGenre.ValueMember = "Id";

            cboPublic.DataSource = access.GetAllPublics();
            cboPublic.DisplayMember = "Libelle";
            cboPublic.ValueMember = "Id";


            cboRayon.DataSource = access.GetAllRayons();
            cboRayon.DisplayMember = "Libelle";
            cboRayon.ValueMember = "Id";

            if (!modifRevue)
            {
                cboGenre.SelectedIndex = -1;
                cboPublic.SelectedIndex = -1;
                cboRayon.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Méthode de remplissage des champs de la fenêtre avec les informations de la revue à modifier : remplit les champs de texte, le numeric up down et sélectionne les éléments correspondants dans les combobox en fonction des propriétés de la revue passée en paramètre
        /// </summary>
        /// <param name="revue"></param>
        private void RemplirChamps(Revue revue)
        {
            txtAjoutNumero.Text = revue.Id;
            txtTitre.Text = revue.Titre;
            txtPeriodicite.Text = revue.Periodicite;
            nudDelaiMiseADispo.Value = revue.DelaiMiseADispo;
            txtCheminImage.Text = revue.Image;
            cboGenre.SelectedValue = revue.IdGenre;
            cboPublic.SelectedValue = revue.IdPublic;
            cboRayon.SelectedValue = revue.IdRayon;
        }

        /// <summary>
        /// Méthode de validation de l'ajout ou de la modification d'une revue : vérifie que tous les champs obligatoires sont remplis, récupère les sélections des combobox, différencie les cas d'ajout et de modification, crée un nouvel objet revue avec les nouvelles valeurs saisies par l'utilisateur, appelle la méthode de création ou de modification du controller en fonction du cas, affiche un message de succès ou d'erreur en fonction du résultat de l'opération et ferme la fenêtre en cas de succès
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValiderAjoutRevue_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitre.Text) ||
                string.IsNullOrWhiteSpace(txtAjoutNumero.Text) ||
                string.IsNullOrWhiteSpace(txtPeriodicite.Text) ||
                nudDelaiMiseADispo.Value <= 0 ||
                cboPublic.SelectedIndex == -1 ||
                cboGenre.SelectedIndex == -1 ||
                cboRayon.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires");
                return;
            }
            // Récupération des sélections des combobox
            var genreSelection = (Categorie)cboGenre.SelectedItem;
            var publicSelection = (Categorie)cboPublic.SelectedItem;
            var rayonSelection = (Categorie)cboRayon.SelectedItem;

            //CAS MODIFICATION
            if (modifRevue && revueActuelle != null)
            {
                if (MessageBox.Show("Confirmer la modification de cette revue ?",
                    "Modification",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    // Création nouvel objet modifié (objet métier immuable ⭐)
                    Revue revueModifiee = new Revue(
                        id: revueActuelle.Id,
                        titre: txtTitre.Text,
                        image: txtCheminImage.Text,
                        periodicite: txtPeriodicite.Text,
                        delaiMiseADispo: (int)nudDelaiMiseADispo.Value,
                        idGenre: genreSelection.Id,
                        genre: genreSelection.Libelle,
                        idPublic: publicSelection.Id,
                        lePublic: publicSelection.Libelle,
                        idRayon: rayonSelection.Id,
                        rayon: rayonSelection.Libelle
                    );

                    if (controllerRevue.ModifierRevue(revueModifiee))
                    {
                        MessageBox.Show("Revue modifiée avec succès !");
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
                return;
            }

            // CAS AJOUT
            if (MessageBox.Show("Êtes-vous sûr de vouloir ajouter cette nouvelle revue ?",
                    "Confirmer l'ajout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //Vérification ID disponible
                if (!controllerRevue.VerifierIdDisponible(txtAjoutNumero.Text.Trim()))
                {
                    MessageBox.Show("Ce numéro de document existe déjà");
                    return;
                }

                // Création du Livre avec le constructeur complet
                Revue nouvelleRevue = new Revue(
                    id: txtAjoutNumero.Text.Trim(),
                    titre: txtTitre.Text,
                    image: txtCheminImage.Text,
                    periodicite: txtPeriodicite.Text,
                    delaiMiseADispo: (int)nudDelaiMiseADispo.Value,
                    idGenre: genreSelection.Id,
                    genre: genreSelection.Libelle,
                    idPublic: publicSelection.Id,
                    lePublic: publicSelection.Libelle,
                    idRayon: rayonSelection.Id,
                    rayon: rayonSelection.Libelle
                );

                bool ok = controllerRevue.CreerRevue(nouvelleRevue);

                if (ok)
                {
                    MessageBox.Show("Revue ajoutée avec succès !");
                    this.DialogResult = DialogResult.OK;  // IMPORTANT
                    this.Close();                        // ferme la fenêtre
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout de la revue.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires : titre, n° de document, périodicité, délai de mise à disposition, rayon, public et genre", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Méthode d'annulation de l'ajout ou de la modification d'une revue : affiche une confirmation d'annulation, et si l'utilisateur confirme, ferme la fenêtre sans enregistrer les modifications en cours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerAjoutRevue_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
