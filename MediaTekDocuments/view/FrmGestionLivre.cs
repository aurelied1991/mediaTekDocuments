using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaTekDocuments.dal;
using Newtonsoft.Json;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;


namespace MediaTekDocuments.view
{
    /// <summary>
    /// Formulaire de gestion des livres : permet d'ajouter un nouveau livre ou de modifier un livre existant en fonction du constructeur utilisé (avec ou sans paramètre)
    /// </summary>
    public partial class FrmGestionLivre : Form
    {
        /// <summary>
        /// Instance du controller de gestion des documents pour pouvoir appeler les méthodes de création, modification et vérification d'un id de document
        /// </summary>
        private FrmGestionDocumentsController controllerLivres;
        /// <summary>
        /// Booléen pour savoir si le formulaire est utilisé pour la modification d'un livre ou pour l'ajout d'un nouveau livre : permet d'adapter le comportement du formulaire en fonction de l'utilisation (remplissage des champs, titre du formulaire, texte du bouton de validation)
        /// </summary>
        private bool modifLivre = false;
        /// <summary>
        /// Instance du livre actuel pour la modification : permet de remplir les champs du formulaire avec les informations du livre à modifier et de récupérer son id pour la modification
        /// </summary>
        private Livre livreActuel = null;
        /// <summary>
        /// Instance de la classe d'accès aux données pour pouvoir appeler les méthodes d'ajout, de modification et de récupération des documents (notamment pour remplir les combobox avec les genres, publics et rayons disponibles)
        /// </summary>
        private Access access = Access.GetInstance();

        /// <summary>
        /// Constructeur du formulaire de gestion des livres : si un livre est passé en paramètre, le formulaire est en mode modification et les champs sont remplis avec les informations du livre, sinon le formulaire est en mode ajout et les champs sont vides
        /// </summary>
        //=/ <param name="livre"></param>
        public FrmGestionLivre(Livre livre = null)
        {
            InitializeComponent();
            controllerLivres = new FrmGestionDocumentsController();
            if (livre != null)
            {
                modifLivre = true;
                livreActuel = livre;

                RemplirChamps(livre);
                txtLivreAjoutNumero.ReadOnly = true;
                btnValiderAjoutLivre.Text = "Modifier le livre";
                lblTitreForm.Text = "Modification d'un livre";
                this.Text = "Modification d'un livre";
            }
            else
            {
                modifLivre = false;
                lblTitreForm.Text = "Ajout d'un nouveau livre";
            }
        }

        /// <summary>
        /// Méthode d'initialisation du formulaire : remplit les combobox avec les genres, publics et rayons disponibles en appelant les méthodes de récupération de la classe d'accès aux données, et si le formulaire est en mode ajout, les champs des combobox sont vides (index à -1)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAddLivre_Load(object sender, EventArgs e)
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

            if (!modifLivre)
            {
                cboGenre.SelectedIndex = -1;
                cboPublic.SelectedIndex = -1;
                cboRayon.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Méthode de remplissage des champs du formulaire avec les informations du livre à modifier : permet de pré-remplir les champs du formulaire pour faciliter la modification et éviter les erreurs de saisie
        /// </summary>
        /// <param name="livre"></param>
        private void RemplirChamps(Livre livre)
        {
            txtLivreAjoutNumero.Text = livre.Id;
            txtTitre.Text = livre.Titre;
            txtAuteur.Text = livre.Auteur;
            txtISBN.Text = livre.Isbn;
            txtCollection.Text = livre.Collection;
            txtCheminImage.Text = livre.Image;
            cboGenre.SelectedValue = livre.IdGenre;
            cboPublic.SelectedValue = livre.IdPublic;
            cboRayon.SelectedValue = livre.IdRayon;
        }

        /// <summary>
        /// Méthode de validation de l'ajout ou de la modification d'un livre : vérifie que tous les champs obligatoires sont remplis, récupère les sélections des combobox, et en fonction du mode (ajout ou modification), crée un nouvel objet Livre avec les informations saisies et appelle la méthode de création ou de modification du controller. Affiche un message de confirmation en cas de succès et ferme le formulaire, ou un message d'erreur en cas d'échec.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerAjoutLivre_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Méthode de validation de l'ajout ou de la modification d'un livre : vérifie que tous les champs obligatoires sont remplis, récupère les sélections des combobox, et en fonction du mode (ajout ou modification), crée un nouvel objet Livre avec les informations saisies et appelle la méthode de création ou de modification du controller. Affiche un message de confirmation en cas de succès et ferme le formulaire, ou un message d'erreur en cas d'échec.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValiderAjoutLivre_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitre.Text) ||
                string.IsNullOrWhiteSpace(txtLivreAjoutNumero.Text) ||
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
            if (modifLivre && livreActuel != null)
            {
                if (MessageBox.Show("Confirmer la modification de ce livre ?",
                    "Modification",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    // Création nouvel objet modifié (objet métier immuable ⭐)
                    Livre livreModifie = new Livre(
                        id: livreActuel.Id,
                        titre: txtTitre.Text,
                        image: txtCheminImage.Text,
                        isbn: txtISBN.Text,
                        auteur: txtAuteur.Text,
                        collection: txtCollection.Text,
                        idGenre: genreSelection.Id,
                        genre: genreSelection.Libelle,
                        idPublic: publicSelection.Id,
                        lePublic: publicSelection.Libelle,
                        idRayon: rayonSelection.Id,
                        rayon: rayonSelection.Libelle
                    );

                    if (controllerLivres.ModifierLivre(livreModifie))
                    {
                        MessageBox.Show("Livre modifié avec succès !");
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }

                return;
            }

            // CAS AJOUT
            if (MessageBox.Show("Êtes-vous sûr de vouloir ajouter ce nouveau livre ?",
                    "Confirmer l'ajout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //Vérification ID disponible
                    if (!controllerLivres.VerifierIdDisponible(txtLivreAjoutNumero.Text.Trim()))
                    {
                        MessageBox.Show("Ce numéro de document existe déjà");
                        return;
                    }
                   
                    // Création du Livre avec le constructeur complet
                    Livre nouveauLivre = new Livre(
                        id: txtLivreAjoutNumero.Text.Trim(),
                        titre: txtTitre.Text,
                        image: txtCheminImage.Text,
                        isbn: txtISBN.Text,
                        auteur: txtAuteur.Text,
                        collection: txtCollection.Text,
                        idGenre: genreSelection.Id,
                        genre: genreSelection.Libelle,
                        idPublic: publicSelection.Id,
                        lePublic: publicSelection.Libelle,
                        idRayon: rayonSelection.Id,
                        rayon: rayonSelection.Libelle
                    );

                    bool ok = controllerLivres.CreerLivre(nouveauLivre);

                    if (ok)
                    {
                        MessageBox.Show("Livre ajouté avec succès !");
                        this.DialogResult = DialogResult.OK;  // IMPORTANT
                        this.Close();                        // ferme la fenêtre
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout du livre.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires : titre, auteur, n° de document, rayon, public et genre", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
