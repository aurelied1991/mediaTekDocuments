using MediaTekDocuments.controller;
using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System;
using System.Windows.Forms;


namespace MediaTekDocuments.view
{
    /// <summary>
    /// Formulaire de gestion des livres : ajout ou modification
    /// </summary>
    public partial class FrmGestionLivre : Form
    {
        /// <summary>
        /// Controller pour gérer la création et modification de livres
        /// </summary>
        private FrmGestionDocumentsController controllerLivres;
        /// <summary>
        /// Indique si l'opération est une modification (true) ou un ajout (false)
        /// </summary>
        private bool modifLivre = false;
        /// <summary>
        /// Livre actuel à modifier
        /// </summary>
        private Livre livreActuel = null;

        /// <summary>
        /// Constructeur : initialise le formulaire en mode ajout ou modification selon le paramètre
        /// <param name="livre">Livre à modifier ou null pour un ajout</param>
        /// </summary>
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
        /// Chargement du formulaire : remplit les combobox avec les genres, publics et rayons
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
        /// Remplit les champs du formulaire avec les informations du livre à modifier
        /// </summary>
        /// <param name="livre">Livre source</param>
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
        /// Annule l'ajout ou la modification et ferme le formulaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerAjoutLivre_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Validation de l'ajout ou de la modification : vérifie les champs, crée l'objet Livre et appelle le controller.
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

                    // Création nouvel objet modifié
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
                    this.DialogResult = DialogResult.OK;
                    this.Close();
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
