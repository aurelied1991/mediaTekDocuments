using MediaTekDocuments.controller;
using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Formulaire de gestion d'une revue : ajout ou modification
    /// </summary>
    public partial class FrmGestionRevue : Form
    {
        /// <summary>
        /// Controller pour gérer la création et modification de revues
        /// </summary>
        private FrmGestionDocumentsController controllerRevue;
        /// <summary>
        /// Indique si l'opération est une modification (true) ou un ajout (false)
        /// </summary>
        private bool modifRevue = false;
        /// <summary>
        /// Revue actuelle à modifier
        /// </summary>
        private Revue revueActuelle = null;

        /// <summary>
        /// Constructeur : initialise le formulaire en fonction de la revue passée en paramètre (null = ajout)
        /// </summary>
        /// <param name="revue">Revue à modifier ou null pour un ajout</param>
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
        /// Chargement du formulaire : remplit les combobox avec les genres, publics et rayons
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
        /// Remplit les champs du formulaire avec les données de la revue à modifier
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
        /// Validation de l'ajout ou de la modification : vérifie les champs, crée l'objet Revue et appelle le controller
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
                    this.DialogResult = DialogResult.OK;
                    this.Close();                      
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
        /// Annule l'ajout ou la modification et ferme le formulaire
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
