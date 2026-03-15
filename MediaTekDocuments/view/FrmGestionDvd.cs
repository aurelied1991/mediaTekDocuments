using MediaTekDocuments.controller;
using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Formulaire de gestion des DVD : ajout ou modification
    /// </summary>
    public partial class FrmGestionDvd : Form
    {
        /// <summary>
        /// Controller pour gérer la création et modification de DVD
        /// </summary>
        private FrmGestionDocumentsController controllerDvd;
        /// <summary>
        /// Indique si l'opération est une modification (true) ou un ajout (false)
        /// </summary>
        private bool modifDvd = false;
        /// <summary>
        /// DVD actuel à modifier
        /// </summary>
        private Dvd dvdActuel = null;

        /// <summary>
        /// Constructeur : initialise le formulaire en mode ajout ou modification selon le paramètre
        /// </summary>
        /// <param name="dvd">DVD à modifier ou null pour un ajout</param>
        public FrmGestionDvd(Dvd dvd = null)
        {
            InitializeComponent();
            controllerDvd = new FrmGestionDocumentsController();
            if (dvd != null)
            {
                modifDvd = true;
                dvdActuel = dvd;

                RemplirChamps(dvd);
                txtAjoutNumero.ReadOnly = true;
                btnValiderAjoutDvd.Text = "Modifier le dvd";
                lblTitreFormDvd.Text = "Modification d'un dvd";
                this.Text = "Modification d'un dvd";
            }
            else
            {
                modifDvd = false;
                lblTitreFormDvd.Text = "Ajout d'un nouveau dvd";
            }
        }

        /// <summary>
        /// Chargement du formulaire : remplit les combobox avec les genres, publics et rayons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmGestionDvd_Load(object sender, EventArgs e)
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

            if (!modifDvd)
            {
                cboGenre.SelectedIndex = -1;
                cboPublic.SelectedIndex = -1;
                cboRayon.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Remplit les champs du formulaire avec les informations du DVD à modifier
        /// </summary>
        /// <param name="dvd">DVD source</param>
        private void RemplirChamps(Dvd dvd)
        {
            txtAjoutNumero.Text = dvd.Id;
            txtTitre.Text = dvd.Titre;
            txtRealisateur.Text = dvd.Realisateur;
            nudDuree.Value = dvd.Duree;
            txtSynopsis.Text = dvd.Synopsis;
            txtCheminImage.Text = dvd.Image;
            cboGenre.SelectedValue = dvd.IdGenre;
            cboPublic.SelectedValue = dvd.IdPublic;
            cboRayon.SelectedValue = dvd.IdRayon;
        }

        /// <summary>
        /// Validation de l'ajout ou de la modification : vérifie les champs, crée l'objet Dvd et appelle le controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValiderAjoutDvd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitre.Text) ||
                string.IsNullOrWhiteSpace(txtRealisateur.Text) ||
                string.IsNullOrWhiteSpace(txtAjoutNumero.Text) ||
                string.IsNullOrWhiteSpace(txtSynopsis.Text) ||
                nudDuree.Value <= 0 ||
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
            if (modifDvd && dvdActuel != null)
            {
                if (MessageBox.Show("Confirmer la modification de ce dvd ?",
                    "Modification",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    // Création nouvel objet modifié (objet métier immuable ⭐)
                    Dvd dvdModifie = new Dvd(
                        id: dvdActuel.Id,
                        titre: txtTitre.Text,
                        image: txtCheminImage.Text,
                        synopsis: txtSynopsis.Text,
                        realisateur: txtRealisateur.Text,
                        duree: (int)nudDuree.Value,
                        idGenre: genreSelection.Id,
                        genre: genreSelection.Libelle,
                        idPublic: publicSelection.Id,
                        lePublic: publicSelection.Libelle,
                        idRayon: rayonSelection.Id,
                        rayon: rayonSelection.Libelle
                    );

                    if (controllerDvd.ModifierDvd(dvdModifie))
                    {
                        MessageBox.Show("Dvd modifié avec succès !");
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
                return;
            }

            // CAS AJOUT
            if (MessageBox.Show("Êtes-vous sûr de vouloir ajouter ce nouveau dvd ?",
                    "Confirmer l'ajout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //Vérification ID disponible
                if (!controllerDvd.VerifierIdDisponible(txtAjoutNumero.Text.Trim()))
                {
                    MessageBox.Show("Ce numéro de document existe déjà");
                    return;
                }

                // Création du Livre avec le constructeur complet
                Dvd nouveauDvd = new Dvd(
                    id: txtAjoutNumero.Text.Trim(),
                    titre: txtTitre.Text,
                    image: txtCheminImage.Text,
                    synopsis: txtSynopsis.Text,
                    realisateur: txtRealisateur.Text,
                    duree: (int)nudDuree.Value,
                    idGenre: genreSelection.Id,
                    genre: genreSelection.Libelle,
                    idPublic: publicSelection.Id,
                    lePublic: publicSelection.Libelle,
                    idRayon: rayonSelection.Id,
                    rayon: rayonSelection.Libelle
                );

                bool ok = controllerDvd.CreerDvd(nouveauDvd);

                if (ok)
                {
                    MessageBox.Show("Dvd ajouté avec succès !");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout du dvd.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires : titre, réalisateur, n° de document, rayon, public et genre", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Annule l'ajout ou la modification et ferme le formulaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerAjoutDvd_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
