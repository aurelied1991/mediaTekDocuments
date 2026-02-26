using MediaTekDocuments.controller;
using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    public partial class FrmGestionDvd : Form
    {
        /// <summary>
        /// Controller pour la gestion des documents : contient les méthodes de création, modification et vérification d'un id de document
        /// </summary>
        private FrmGestionDocumentsController controllerDvd;
        /// <summary>
        /// Booléen pour différencier les cas d'ajout et de modification d'un dvd : true si modification, false si ajout
        /// </summary>
        private bool modifDvd = false;
        /// <summary>
        /// Instance de la classe métier Dvd pour stocker le dvd en cours de modification (null si ajout)
        /// </summary>
        private Dvd dvdActuel = null;
        /// <summary>
        /// Instance de la classe d'accès aux données pour pouvoir appeler les méthodes d'ajout, de modification et de récupération des documents
        /// </summary>
        private Access access = Access.GetInstance();

        /// <summary>
        /// Constructeur de la fenêtre de gestion des dvd : différencie les cas d'ajout et de modification en fonction de la présence ou non d'un dvd en paramètre, remplit les champs si modification et adapte les éléments de la fenêtre (titre, texte du bouton de validation, etc.)
        /// </summary>
        /// <param name="dvd"></param>
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
        /// Méthode d'initialisation de la fenêtre : remplit les combobox avec les données de la base et ajoute une ligne vide pour forcer l'utilisateur à faire un choix, sauf en cas de modification où les champs sont déjà remplis et que les combobox sont positionnées sur les bonnes valeurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmGestionDvd_Load(object sender, EventArgs e)
        {
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
        }

        /// <summary>
        /// Méthode de remplissage des champs de la fenêtre à partir d'un objet Dvd : utilisée en cas de modification pour afficher les informations du dvd à modifier dans les champs correspondants
        /// </summary>
        /// <param name="dvd"></param>
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
        /// Méthode de validation de l'ajout ou de la modification d'un dvd : vérifie que tous les champs obligatoires sont remplis, récupère les sélections des combobox, crée un nouvel objet Dvd avec les informations saisies et appelle la méthode de création ou de modification du controller en fonction du cas (ajout ou modification), affiche un message de confirmation et ferme la fenêtre si l'opération est réussie
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
                    this.DialogResult = DialogResult.OK;  // IMPORTANT
                    this.Close();                        // ferme la fenêtre
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
        /// Méthode d'annulation de l'ajout ou de la modification d'un dvd : affiche une confirmation et ferme la fenêtre si l'utilisateur confirme, sans enregistrer les modifications
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
