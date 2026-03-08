using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using System.ComponentModel.Design;
using System.Collections;
using System.Windows.Input;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        private const string EN_COURS = "0001";
        private const string LIVREE = "0002";
        private const string REGLEE = "0003";
        private const string RELANCEE = "0004";
        public const string LIBELLE_EN_COURS = "en cours";
        private bool chargementCombo = false;


        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek()
        {
            InitializeComponent();
            controller = new FrmMediatekController();
            FrmAlerteAbonnement frmAlerte =
                new FrmAlerteAbonnement(controller);
            frmAlerte.ShowDialog();
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Affiche les informations d'un document (ilvre ou dvd) les zones d'affichage prévues à cet effet
        /// </summary>
        /// <param name="idDocument"></param>
        /// <param name="estLivre"></param>
        private void ChargerDocumentEtCommandes(string idDocument, bool estLivre)
        {
            if (string.IsNullOrWhiteSpace(idDocument))
            {
                MessageBox.Show("Identifiant invalide");
                return;
            }

            documentSelectionne = controller.GetDocumentById<Document>("document", idDocument);

            if (estLivre)
            {
                if (lesLivres == null || lesLivres.Count == 0)
                    lesLivres = controller.GetAllLivres();

                Livre livre = lesLivres.Find(x => x.Id == idDocument);

                if (livre == null)
                {
                    MessageBox.Show("Livre non trouvé");
                    return;
                }

                ViderChamps();
                RemplirInfosDocument(documentSelectionne);
                RemplirInfosLivre(livre);

                lesCommandes = controller.GetCommandesByDocument(livre.Id);

                RemplirCommandesListe(lesCommandes);
            }
            else
            {
                if (lesDvd == null || lesDvd.Count == 0)
                    lesDvd = controller.GetAllDvd();
                Dvd dvd = lesDvd.Find(x => x.Id == idDocument);

                if (dvd == null)
                {
                    MessageBox.Show("DVD non trouvé");
                    return;
                }

                documentSelectionneDvd = documentSelectionne;

                ViderChampsDvd();
                RemplirInfosDocumentDvd(documentSelectionne);
                RemplirInfosDvd(dvd);

                lesCommandesDvd = controller.GetCommandesByDocument(dvd.Id);

                RemplirCommandesListeDvd(lesCommandesDvd);
            }
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text.Trim()));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }

        /// <summary>
        /// Récupère la liste des livres depuis l'API et affiche la liste complète dans le datagrid
        /// </summary>
        private void ChargerListeLivres()
        {
            lesLivres = controller.GetAllLivres();
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Evenement clic qui permet d'ouvrir le formulaire d'ajout d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddLivre_Click(object sender, EventArgs e)
        {
            // Crée l’instance du formulaire d’ajout
            FrmGestionLivre frmGestion = new FrmGestionLivre();

            if (frmGestion.ShowDialog() == DialogResult.OK)
            {
                //Recharger la liste depuis l’API
                lesLivres = controller.GetAllLivres();

                //Rafraîchir l’affichage
                ChargerListeLivres();
            }
        }

        /// <summary>
        /// Evenement clic qui permet d'ouvrir le formulaire de modification d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifLivre_Click(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner un livre à modifier.");
                return;
            }
            // Récupération de l'objet sélectionné
            Livre selectLivre = (Livre)dgvLivresListe.CurrentRow.DataBoundItem;
            // Ouverture du formulaire en mode modification
            FrmGestionLivre frm = new FrmGestionLivre(selectLivre);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                lesLivres = controller.GetAllLivres();
                ChargerListeLivres();
            }
        }

        /// <summary>
        /// Evenement clic qui permet de supprimer un livre après confirmation de l'utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteLivre_Click(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner un livre à supprimer.");
                return;
            }
            string id = dgvLivresListe.CurrentRow.Cells["id"].Value.ToString();
            DialogResult confirm = MessageBox.Show(
                "Voulez-vous vraiment supprimer ce livre ?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                bool suppressionOk = controller.SupprimerDocument("livre", id);

                if (suppressionOk)
                {
                    MessageBox.Show("Livre supprimé avec succès.");
                    ChargerListeLivres(); // méthode qui recharge le DataGridView
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression.");
                }
            }
        }

        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }

        /// <summary>
        /// Récupère la liste des Dvd depuis l'API et affiche la liste complète dans le datagrid
        /// </summary>
        private void ChargerListeDvd()
        {
            lesDvd = controller.GetAllDvd();
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Evenement clic qui permet d'ouvrir le formulaire d'ajout d'un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDVD_Click(object sender, EventArgs e)
        {
            // Crée l’instance du formulaire d’ajout
            FrmGestionDvd frmDvd = new FrmGestionDvd();

            if (frmDvd.ShowDialog() == DialogResult.OK)
            {
                //Recharger la liste depuis l’API
                lesDvd = controller.GetAllDvd();

                //Rafraîchir l’affichage
                ChargerListeDvd();
            }
        }

        /// <summary>
        /// Evenement clic qui permet d'ouvrir le formulaire de modification d'un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifDVD_Click(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner un dvd à modifier.");
                return;
            }

            // Récupération de l'objet sélectionné
            Dvd selectDvd = (Dvd)dgvDvdListe.CurrentRow.DataBoundItem;

            // Ouverture du formulaire en mode modification
            FrmGestionDvd frm = new FrmGestionDvd(selectDvd);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                lesDvd = controller.GetAllDvd();
                ChargerListeDvd();
            }
        }

        /// <summary>
        /// Evenement clic qui permet de supprimer un dvd après confirmation de l'utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteDVD_Click(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner un dvd à supprimer.");
                return;
            }
            string id = dgvDvdListe.CurrentRow.Cells["id"].Value.ToString();
            DialogResult confirm = MessageBox.Show(
                "Voulez-vous vraiment supprimer ce dvd ?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                bool suppressionOk = controller.SupprimerDocument("dvd", id);

                if (suppressionOk)
                {
                    MessageBox.Show("Dvd supprimé avec succès.");
                    ChargerListeDvd(); // méthode qui recharge le DataGridView
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression.");
                }
            }
        }

        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }

        /// <summary>
        /// Récupère la liste des revues depuis l'API et affiche la liste complète dans le datagrid
        /// </summary>
        private void ChargerListeRevues()
        {
            lesRevues = controller.GetAllRevues();
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Evenement clic qui permet d'ouvrir le formulaire d'ajout d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddRevue_Click(object sender, EventArgs e)
        {
            // Crée l’instance du formulaire d’ajout
            FrmGestionRevue frmRevue = new FrmGestionRevue();

            if (frmRevue.ShowDialog() == DialogResult.OK)
            {
                //Recharger la liste depuis l’API
                lesRevues = controller.GetAllRevues();

                //Rafraîchir l’affichage
                ChargerListeRevues();
            }
        }

        /// <summary>
        /// Evenement clic qui permet d'ouvrir le formulaire de modification d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifRevue_Click(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner une revue à modifier.");
                return;
            }

            // Récupération de l'objet sélectionné
            Revue selectRevue = (Revue)dgvRevuesListe.CurrentRow.DataBoundItem;

            // Ouverture du formulaire en mode modification
            FrmGestionRevue frm = new FrmGestionRevue(selectRevue);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                lesRevues = controller.GetAllRevues();
                ChargerListeRevues();
            }
        }

        /// <summary>
        /// Evenement clic qui permet de supprimer une revue après confirmation de l'utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteRevue_Click(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner une revue à supprimer.");
                return;
            }
            string id = dgvRevuesListe.CurrentRow.Cells["id"].Value.ToString();
            DialogResult confirm = MessageBox.Show(
                "Voulez-vous vraiment supprimer cette revue ?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                bool suppressionOk = controller.SupprimerDocument("revue", id);

                if (suppressionOk)
                {
                    MessageBox.Show("Revue supprimée avec succès.");
                    ChargerListeRevues(); // méthode qui recharge le DataGridView
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression.");
                }
            }
        }
        #endregion

        #region Onglet Parutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }
        #endregion

        #region Onglet Commandes de livres
        private readonly BindingSource bdgCommandesListe = new BindingSource();
        private List<CommandeDocument> lesCommandes = new List<CommandeDocument>();
        private Document documentSelectionne;

        /// <summary>
        /// Ouverture de l'onglet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCommandes_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
        }

        /// <summary>
        /// Affichage des informations du document sélectionné
        /// </summary>
        /// <param name="documentSelectionne"></param>
        private void RemplirInfosDocument(Document documentSelectionne)
        {
            if (documentSelectionne == null) return;
            txtTitre.Text = documentSelectionne.Titre;
            txtCheminImage.Text = documentSelectionne.Image;
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre"></param>
        private void RemplirInfosLivre(Livre livre)
        {
            if (livre == null) return;

            txtISBN.Text = livre.Isbn;
            txtAuteur.Text = livre.Auteur;
            txtCollection.Text = livre.Collection;
            txtLivreGenre.Text = livre.Genre;
            txtLivrePublic.Text = livre.Public;
            txtLivreRayon.Text = livre.Rayon;
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du document et du livre
        /// </summary>
        private void ViderChamps()
        {
            txtTitre.Text = "";
            txtCheminImage.Text = "";
            txtISBN.Text = "";
            txtAuteur.Text = "";
            txtCollection.Text = "";
            txtLivreGenre.Text = "";
            txtLivrePublic.Text = "";
            txtLivreRayon.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid avec la liste de commandes reçue en paramètre
        /// </summary>
        /// <param name="commandes"></param>
        private void RemplirCommandesListe(List<CommandeDocument> commandes)
        {
            if (commandes != null)
            {
                dgvListeCommandes.Columns.Clear();
                dgvListeCommandes.AutoGenerateColumns = true;
                bdgCommandesListe.DataSource = commandes;
                dgvListeCommandes.DataSource = bdgCommandesListe;
                dgvListeCommandes.Refresh();
                dgvListeCommandes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvListeCommandes.AllowUserToOrderColumns = true;

                if (dgvListeCommandes.Columns.Contains("idLivreDvd"))
                {
                    dgvListeCommandes.Columns["idLivreDvd"].Visible = false;
                }

                if (dgvListeCommandes.Columns.Contains("idSuivi"))
                {
                    dgvListeCommandes.Columns["idSuivi"].Visible = false;
                }
                if (dgvListeCommandes.Columns.Contains("idCommande"))
                {
                    dgvListeCommandes.Columns["idCommande"].DisplayIndex = 0;
                    dgvListeCommandes.Columns["idCommande"].HeaderText = "N°commande";
                }

                if (dgvListeCommandes.Columns.Contains("dateCommande"))
                {
                    dgvListeCommandes.Columns["dateCommande"].DisplayIndex = 1;
                    dgvListeCommandes.Columns["dateCommande"].HeaderText = "Date";
                }

                if (dgvListeCommandes.Columns.Contains("montant"))
                {
                    dgvListeCommandes.Columns["montant"].DisplayIndex = 2;
                    dgvListeCommandes.Columns["montant"].HeaderText = "Montant";
                }

                if (dgvListeCommandes.Columns.Contains("nbExemplaire"))
                {
                    dgvListeCommandes.Columns["nbExemplaire"].DisplayIndex = 3;
                    dgvListeCommandes.Columns["nbExemplaire"].HeaderText = "Exemplaires";
                }

                if (dgvListeCommandes.Columns.Contains("libelleSuivi"))
                {
                    dgvListeCommandes.Columns["libelleSuivi"].DisplayIndex = 4;
                    dgvListeCommandes.Columns["libelleSuivi"].HeaderText = "Suivi";
                }
            }
            else
            {
                bdgCommandesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Tri sur les colonnes du datagrid des commandes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvListeCommandes_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string nomColonne = dgvListeCommandes.Columns[e.ColumnIndex].DataPropertyName;

            List<CommandeDocument> sortedList = lesCommandes;

            switch (nomColonne)
            {
                case "IdCommande":
                    sortedList = lesCommandes.OrderBy(o => o.IdCommande).ToList();
                    break;

                case "DateCommande":
                    sortedList = lesCommandes
                        .OrderBy(o => o.DateCommande)
                        .ToList();
                    break;

                case "Montant":
                    sortedList = lesCommandes.OrderBy(o => o.Montant).ToList();
                    break;

                case "NbExemplaire":
                    sortedList = lesCommandes.OrderBy(o => o.NbExemplaire).ToList();
                    break;

                case "LibelleSuivi":
                    sortedList = lesCommandes.OrderBy(o => o.LibelleSuivi).ToList();
                    break;
            }

            lesCommandes = sortedList;
            bdgCommandesListe.DataSource = lesCommandes;
            bdgCommandesListe.ResetBindings(false);
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercheCommandeLivre_Click(object sender, EventArgs e)
        {
            ChargerDocumentEtCommandes(txtIdLivre.Text.Trim(), true);
        }

        /// <summary>
        /// Enregistrement d'une nouvelle commande pour le livre sélectionné.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnregistrerCommandeLivre_Click(object sender, EventArgs e)
        {
            if (documentSelectionne == null)
            {
                MessageBox.Show("Aucun livre sélectionné");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNumeroCommande.Text))
            {
                MessageBox.Show("Numéro de commande obligatoire");
                return;
            }

            if (!double.TryParse(txtMontantCommandeLivres.Text, out double montant))
            {
                MessageBox.Show("Montant invalide");
                return;
            }

            // Construction des données pour l'API
            CommandeDocument nouvelleCommande = new CommandeDocument(
                txtNumeroCommande.Text.Trim(),
                dtpNouvelleCommandeLivre.Value,
                montant,
                (int)nudNbExemplairesLivres.Value,
                documentSelectionne.Id,
                "0001",            
                "En cours"
            );

            bool ok = controller.CreerCommandeDocument(nouvelleCommande);

            if (!ok)
            {
                MessageBox.Show("Erreur lors de l'ajout (id déjà existant ?)");
                return;
            }

            MessageBox.Show("Commande ajoutée avec succès");

            // Rafraîchissement
            lesCommandes = controller.GetCommandesByDocument(documentSelectionne.Id);
            RemplirCommandesListe(lesCommandes);
        }

        /// <summary>
        /// Affichage du suivi de la commande sélectionnée et possibilité de le modifier.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifierSuiviCommandeLivre_Click(object sender, EventArgs e)
        {
            if (dgvListeCommandes.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner une commande.");
                return;
            }
            grbModifierEtapeCommandeLivre.Enabled = true;
            string idCommande = dgvListeCommandes.CurrentRow.Cells["IdCommande"].Value.ToString();

            txtNumeroCommandeLivre.Text = idCommande;
            chargementCombo = true;

            cbxEtapeCommande.DataSource = controller.GetAllSuivis();
            cbxEtapeCommande.DisplayMember = "libelleSuivi";
            cbxEtapeCommande.ValueMember = "idSuivi";

            cbxEtapeCommande.SelectedValue = dgvListeCommandes.CurrentRow.Cells["idSuivi"].Value.ToString();
            chargementCombo = false;
        }

        /// <summary>
        /// Enregistrement du changement de suivi de la commande sélectionnée.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValiderChangementEtapeCommande_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNumeroCommandeLivre.Text))
            {
                MessageBox.Show("Aucune commande sélectionnée.");
                return;
            }

            string idCommande = txtNumeroCommandeLivre.Text;
            string nouveauIdSuivi = cbxEtapeCommande.SelectedValue.ToString();

            bool ok = controller.ModifierSuiviCommandeDocument(idCommande, nouveauIdSuivi);

            if (ok)
            {
                MessageBox.Show("Suivi mis à jour avec succès !");
                lesCommandes =controller.GetCommandesByDocument(dgvListeCommandes.CurrentRow.Cells["idLivreDvd"].Value.ToString());
                RemplirCommandesListe(lesCommandes);
                grbModifierEtapeCommandeLivre.Enabled = false;
            }
            else
            {
                MessageBox.Show("Erreur lors de la mise à jour.");
            }
        }

        /// <summary>
        /// Validation du changement de suivi : vérification de la cohérence des étapes (ex : une commande réglée ne peut pas être remise en cours, etc.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxEtapeCommande_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cbxEtapeCommande.Focused) return;

            if (dgvListeCommandes.CurrentRow == null) return;
            if (cbxEtapeCommande.SelectedValue == null) return;

            string etatActuel =
                dgvListeCommandes.CurrentRow.Cells["idSuivi"].Value.ToString();

            string nouvelEtat =
                cbxEtapeCommande.SelectedValue.ToString();

            if (etatActuel == EN_COURS && nouvelEtat == REGLEE)
            {
                MessageBox.Show("Impossible de passer directement de En cours à Réglée.");
                chargementCombo = true;
                cbxEtapeCommande.SelectedValue = etatActuel;
                chargementCombo = false;
                return;
            }
            if (etatActuel == REGLEE)
            {
                MessageBox.Show("Une commande réglée ne peut plus être modifiée.");
                cbxEtapeCommande.SelectedValue = etatActuel;
                return;
            }

            if (etatActuel == LIVREE && nouvelEtat != REGLEE)
            {
                MessageBox.Show("Une commande livrée ne peut être réglée uniquement.");
                cbxEtapeCommande.SelectedValue = etatActuel;
                return;
            }

            if ((etatActuel == LIVREE || etatActuel == REGLEE) &&
                nouvelEtat == EN_COURS)
            {
                MessageBox.Show("Retour à l'état précédent non autorisé.");
                cbxEtapeCommande.SelectedValue = etatActuel;
            }

            if (etatActuel == RELANCEE && nouvelEtat == REGLEE)
            {
                MessageBox.Show("Une commande relancée doit être livrée avant d'être réglée.");
                cbxEtapeCommande.SelectedValue = etatActuel;
                return;
            }
        }

        /// <summary>
        /// Annulation du changement de suivi : réinitialisation de la combo et désactivation du groupe de modification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerChangementSuiviLivre_Click(object sender, EventArgs e)
        {
            grbModifierEtapeCommandeLivre.Enabled = false;
            cbxEtapeCommande.SelectedIndex = -1;
            txtNumeroCommandeLivre.Clear();
        }

        /// <summary>
        /// Suppression de la commande sélectionnée après confirmation de l'utilisateur et rafraîchissement de la liste des commandes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimerCommandeLivre_Click(object sender, EventArgs e)
        {
            if (dgvListeCommandes.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner une commande.");
                return;
            }

            string idCommande = dgvListeCommandes.CurrentRow.Cells["idCommande"].Value.ToString();

            DialogResult result =
                MessageBox.Show(
                    "Confirmer la suppression de la commande ?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

            if (result != DialogResult.Yes)
                return;

            bool ok = controller.SupprimerCommande(idCommande);

            if (!ok)
            {
                MessageBox.Show("Suppression impossible");
                return;
            }

            MessageBox.Show("Commande supprimée avec succès");

            // Rafraîchir la liste
            lesCommandes = controller.GetCommandesByDocument(dgvListeCommandes.CurrentRow.Cells["idLivreDvd"].Value.ToString());
            RemplirCommandesListe(lesCommandes);
        }
        #endregion

        #region Onglet Commandes de Dvd
        private readonly BindingSource bdgCommandesListeDvd = new BindingSource();
        private List<CommandeDocument> lesCommandesDvd = new List<CommandeDocument>();
        private Document documentSelectionneDvd;

        /// <summary>
        /// Ouverture de l'onglet : récupère les dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCommandesDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
        }

        /// <summary>
        /// Affichage des informations du document sélectionné
        /// </summary>
        /// <param name="documentSelectionneDvd"></param>
        private void RemplirInfosDocumentDvd(Document documentSelectionneDvd)
        {
            if (documentSelectionneDvd == null) return;
            txtTitreDvd.Text = documentSelectionneDvd.Titre;
            txtCheminImageDvd.Text = documentSelectionneDvd.Image;
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd"></param>
        private void RemplirInfosDvd(Dvd dvd)
        {
            if (dvd == null) return;

            txtRealisateurDvd.Text = dvd.Realisateur;
            txtDureeDvd.Text = dvd.Duree.ToString();
            txtSynopsisDvd.Text = dvd.Synopsis;
            txtGenreDvd.Text = dvd.Genre;
            txtPublicDvd.Text = dvd.Public;
            txtRayonDvd.Text = dvd.Rayon;
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du document et du dvd
        /// </summary>
        private void ViderChampsDvd()
        {
            txtTitreDvd.Text = "";
            txtCheminImageDvd.Text = "";
            txtDureeDvd.Text = "";
            txtRealisateurDvd.Text = "";
            txtSynopsisDvd.Text = "";
            txtGenreDvd.Text = "";
            txtPublicDvd.Text = "";
            txtRayonDvd.Text = "";

        }

        /// <summary>
        /// Remplit le dategrid avec la liste de commandes reçue en paramètre
        /// </summary>
        /// <param name="commandes"></param>
        private void RemplirCommandesListeDvd(List<CommandeDocument> commandes)
        {
            if (commandes != null)
            {
                dgvListeCommandesDvd.Columns.Clear();
                dgvListeCommandesDvd.AutoGenerateColumns = true;

                bdgCommandesListeDvd.DataSource = commandes;
                dgvListeCommandesDvd.DataSource = bdgCommandesListeDvd;
                dgvListeCommandesDvd.Refresh();
                dgvListeCommandesDvd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                if (dgvListeCommandesDvd.Columns.Contains("idLivreDvd"))
                {
                    dgvListeCommandesDvd.Columns["idLivreDvd"].Visible = false;
                }

                if (dgvListeCommandesDvd.Columns.Contains("idSuivi"))
                {
                    dgvListeCommandesDvd.Columns["idSuivi"].Visible = false;
                }

                if (dgvListeCommandesDvd.Columns.Contains("idCommande"))
                {
                    dgvListeCommandesDvd.Columns["idCommande"].DisplayIndex = 0;
                    dgvListeCommandesDvd.Columns["idCommande"].HeaderText = "N°commande";
                }

                if (dgvListeCommandesDvd.Columns.Contains("dateCommande"))
                {
                    dgvListeCommandesDvd.Columns["dateCommande"].DisplayIndex = 1;
                    dgvListeCommandesDvd.Columns["dateCommande"].HeaderText = "Date";
                }

                if (dgvListeCommandesDvd.Columns.Contains("montant"))
                {
                    dgvListeCommandesDvd.Columns["montant"].DisplayIndex = 2;
                    dgvListeCommandesDvd.Columns["montant"].HeaderText = "Montant";
                }

                if (dgvListeCommandesDvd.Columns.Contains("nbExemplaire"))
                {
                    dgvListeCommandesDvd.Columns["nbExemplaire"].DisplayIndex = 3;
                    dgvListeCommandesDvd.Columns["nbExemplaire"].HeaderText = "Exemplaires";
                }

                if (dgvListeCommandesDvd.Columns.Contains("libelleSuivi"))
                {
                    dgvListeCommandesDvd.Columns["libelleSuivi"].DisplayIndex = 4;
                    dgvListeCommandesDvd.Columns["libelleSuivi"].HeaderText = "Suivi";
                }
            }
            else
            {
                bdgCommandesListeDvd.DataSource = null;
            }
        }

        /// <summary>
        /// Tri sur les colonnes du datagrid des commandes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvListeCommandesDvd_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string nomColonne = dgvListeCommandesDvd.Columns[e.ColumnIndex].DataPropertyName;

            List<CommandeDocument> sortedList = lesCommandesDvd;

            switch (nomColonne)
            {
                case "IdCommande":
                    sortedList = lesCommandesDvd.OrderBy(x => x.IdCommande).ToList();
                    break;

                case "DateCommande":
                    sortedList = lesCommandesDvd.OrderBy(x => x.DateCommande).ToList();
                    break;

                case "Montant":
                    sortedList = lesCommandesDvd.OrderBy(x => x.Montant).ToList();
                    break;

                case "NbExemplaire":
                    sortedList = lesCommandesDvd.OrderBy(x => x.NbExemplaire).ToList();
                    break;

                case "LibelleSuivi":
                    sortedList = lesCommandesDvd.OrderBy(x => x.LibelleSuivi).ToList();
                    break;
            }

            bdgCommandesListeDvd.DataSource = sortedList;
            bdgCommandesListeDvd.ResetBindings(false);
        }

        /// <summary>
        /// Recherche et affichage du dvd dont on a saisi le numéro.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercheDvd_Click(object sender, EventArgs e)
        {
            ChargerDocumentEtCommandes(txtIdDvd.Text.Trim(), false);
        }

        /// <summary>
        /// Enregistrement d'une nouvelle commande pour le dvd sélectionné.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnregistrerCommandeDvd_Click(object sender, EventArgs e)
        {
            if (documentSelectionneDvd == null)
            {
                MessageBox.Show("Aucun DVD sélectionné");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNumCommande.Text))
            {
                MessageBox.Show("Numéro de commande obligatoire");
                return;
            }

            if (!double.TryParse(txtMontantCommandeDvd.Text, out double montant))
            {
                MessageBox.Show("Montant invalide");
                return;
            }

            // Construction des données pour l'API
            CommandeDocument nouvelleCommande = new CommandeDocument(
                txtNumCommande.Text.Trim(),
                dtpCommandeDvd.Value,
                montant,
                (int)nudNbExemplairesDvd.Value,
                documentSelectionneDvd.Id,
                "0001",            // EN COURS
                "En cours"
            );

            bool ok = controller.CreerCommandeDocument(nouvelleCommande);

            if (!ok)
            {
                MessageBox.Show("Erreur lors de l'ajout (id déjà existant ?)");
                return;
            }

            MessageBox.Show("Commande ajoutée avec succès");

            // Rafraîchissement
            lesCommandesDvd = controller.GetCommandesByDocument(documentSelectionneDvd.Id);
            RemplirCommandesListeDvd(lesCommandesDvd);
        }

        /// <summary>
        /// Affichage du suivi de la commande sélectionnée et possibilité de le modifier.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifierSuiviCommandeDvd_Click(object sender, EventArgs e)
        {
            if (dgvListeCommandesDvd.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner une commande.");
                return;
            }

            gbxSuiviDvd.Enabled = true;

            string idCommande = dgvListeCommandesDvd.CurrentRow.Cells["IdCommande"].Value.ToString();

            txtNumeroCommandeDvd.Text = idCommande;

            chargementCombo = true;

            cbxEtapeCommandeDvd.DataSource = controller.GetAllSuivis();
            cbxEtapeCommandeDvd.DisplayMember = "libelleSuivi";
            cbxEtapeCommandeDvd.ValueMember = "idSuivi";
            cbxEtapeCommandeDvd.SelectedValue = dgvListeCommandesDvd.CurrentRow.Cells["idSuivi"].Value.ToString();
            chargementCombo = false;
        }

        /// <summary>
        /// Annulation du changement de suivi : réinitialisation de la combo et désactivation du groupe de modification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerSuiviDvd_Click(object sender, EventArgs e)
        {
            gbxSuiviDvd.Enabled = false;
            cbxEtapeCommandeDvd.SelectedIndex = -1;
            txtNumeroCommandeDvd.Clear();
        }

        /// <summary>
        /// Validation du changement de suivi : vérification de la cohérence des étapes (ex : une commande réglée ne peut pas être remise en cours, etc.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValiderSuiviDvd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNumeroCommandeDvd.Text))
            {
                MessageBox.Show("Aucune commande sélectionnée.");
                return;
            }

            if (cbxEtapeCommandeDvd.SelectedValue == null)
            {
                MessageBox.Show("Veuillez sélectionner un état de suivi.");
                return;
            }

            string idCommande = txtNumeroCommandeDvd.Text;
            string nouveauIdSuivi = cbxEtapeCommandeDvd.SelectedValue.ToString();

            bool ok = controller.ModifierSuiviCommandeDocument(idCommande, nouveauIdSuivi);

            if (ok)
            {
                MessageBox.Show("Suivi mis à jour avec succès !");
                if (documentSelectionneDvd != null)
                {
                    lesCommandesDvd = controller.GetCommandesByDocument(documentSelectionneDvd.Id);
                    RemplirCommandesListeDvd(lesCommandesDvd);
                }
                gbxSuiviDvd.Enabled = false;
            }
            else
            {
                MessageBox.Show("Erreur lors de la mise à jour.");
            }
        }

        /// <summary>
        /// Validation du changement de suivi : vérification de la cohérence des étapes (ex : une commande réglée ne peut pas être remise en cours, etc.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxEtapeCommandeDvd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cbxEtapeCommandeDvd.Focused) return;

            if (dgvListeCommandesDvd.CurrentRow == null) return;
            if (cbxEtapeCommandeDvd.SelectedValue == null) return;

            string etatActuel =
                dgvListeCommandesDvd.CurrentRow.Cells["idSuivi"].Value.ToString();

            string nouvelEtat =
                cbxEtapeCommandeDvd.SelectedValue.ToString();

            if (etatActuel == EN_COURS && nouvelEtat == REGLEE)
            {
                MessageBox.Show("Impossible de passer directement de En cours à Réglée.");
                chargementCombo = true;
                cbxEtapeCommandeDvd.SelectedValue = etatActuel;
                chargementCombo = false;
                return;
            }
            if (etatActuel == REGLEE)
            {
                MessageBox.Show("Une commande réglée ne peut plus être modifiée.");
                cbxEtapeCommandeDvd.SelectedValue = etatActuel;
                return;
            }

            if (etatActuel == LIVREE && nouvelEtat != REGLEE)
            {
                MessageBox.Show("Une commande livrée ne peut être réglée uniquement.");
                cbxEtapeCommandeDvd.SelectedValue = etatActuel;
                return;
            }

            if ((etatActuel == LIVREE || etatActuel == REGLEE) &&
                nouvelEtat == EN_COURS)
            {
                MessageBox.Show("Retour à l'état précédent non autorisé.");
                cbxEtapeCommandeDvd.SelectedValue = etatActuel;
            }
            if (etatActuel == RELANCEE && nouvelEtat == REGLEE)
            {
                MessageBox.Show("Une commande relancée doit être livrée avant d'être réglée.");
                cbxEtapeCommandeDvd.SelectedValue = etatActuel;
                return;
            }
        }

        /// <summary>
        /// Suppression de la commande sélectionnée après confirmation de l'utilisateur et rafraîchissement de la liste des commandes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteCommandeDvd_Click(object sender, EventArgs e)
        {
            if (dgvListeCommandesDvd.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner une commande.");
                return;
            }

            string idCommande = dgvListeCommandesDvd.CurrentRow.Cells["idCommande"].Value.ToString();

            DialogResult result =
                MessageBox.Show(
                    "Confirmer la suppression de la commande ?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

            if (result != DialogResult.Yes)
                return;

            bool ok = controller.SupprimerCommande(idCommande);

            if (!ok)
            {
                MessageBox.Show("Suppression impossible");
                return;
            }

            MessageBox.Show("Commande supprimée avec succès");

            if (documentSelectionneDvd != null)
            {
                lesCommandesDvd = controller.GetCommandesByDocument(documentSelectionneDvd.Id);
                RemplirCommandesListeDvd(lesCommandesDvd);
            }
        }

        #endregion

        #region Onglet Commandes de revues
        private readonly BindingSource bdgCommandesListeRevue = new BindingSource();
        private List<Abonnement> lesAbonnements = new List<Abonnement>();
        private Document documentSelectionneRevue;

        /// <summary>
        /// Ouverture de l'onglet : récupère les revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCommandesRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
        }

        /// <summary>
        /// Affichage des informations du document sélectionné
        /// </summary>
        /// <param name="documentSelectionneRevue"></param>
        private void RemplirInfosDocumentRevue(Document documentSelectionneRevue)
        {
            if (documentSelectionneRevue == null) 
                return;
            txtTitreRevue.Text = documentSelectionneRevue.Titre;
            txtCheminImageRevue.Text = documentSelectionneRevue.Image;
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée
        /// </summary>
        /// <param name="revue"></param>
        private void RemplirInfosRevue(Revue revue)
        {
            if (revue == null) 
                return;
            txtPeriodiciteRevue.Text = revue.Periodicite;
            txtMiseADispoRevue.Text = revue.DelaiMiseADispo.ToString();
            txtGenreRevue.Text = revue.Genre;
            txtPublicRevue.Text = revue.Public;
            txtRayonRevue.Text = revue.Rayon;
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du document et de la revue
        /// </summary>
        private void ViderChampsRevue()
        {
            txtTitreRevue.Text = "";
            txtCheminImageRevue.Text = "";
            txtPeriodiciteRevue.Text = "";
            txtMiseADispoRevue.Text = "";
            txtGenreRevue.Text = "";
            txtPublicRevue.Text = "";
            txtRayonRevue.Text = "";

        }

        /// <summary>
        /// Remplit le dategrid avec la liste d'abonnements reçue en paramètre
        /// </summary>
        /// <param name="abonnements"></param>
        private void RemplirCommandesListeRevue(List<Abonnement> abonnements)
        {
            if (abonnements != null)
            {
                dgvAbonnementsRevue.Columns.Clear();
                dgvAbonnementsRevue.AutoGenerateColumns = true;

                bdgCommandesListeRevue.DataSource = abonnements;
                dgvAbonnementsRevue.DataSource = bdgCommandesListeRevue;
                dgvAbonnementsRevue.Refresh();

                dgvAbonnementsRevue.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.AllCells;
                if (dgvAbonnementsRevue.Columns.Contains("idRevue"))
                {
                    dgvAbonnementsRevue.Columns["idRevue"].Visible = false;
                }

                if (dgvAbonnementsRevue.Columns.Contains("idCommande"))
                {
                dgvAbonnementsRevue.Columns["idCommande"].DisplayIndex = 0;
                dgvAbonnementsRevue.Columns["idCommande"].HeaderText = "N° commande";
                }

                if (dgvAbonnementsRevue.Columns.Contains("dateCommande"))
                {
                    dgvAbonnementsRevue.Columns["dateCommande"].DisplayIndex = 1;
                    dgvAbonnementsRevue.Columns["dateCommande"].HeaderText = "Date commande";
                }

                if (dgvAbonnementsRevue.Columns.Contains("montant"))
                {
                    dgvAbonnementsRevue.Columns["montant"].DisplayIndex = 2;
                    dgvAbonnementsRevue.Columns["montant"].HeaderText = "Montant";
                }

                if (dgvAbonnementsRevue.Columns.Contains("dateFinAbonnement"))
                {
                    dgvAbonnementsRevue.Columns["dateFinAbonnement"].DisplayIndex = 3;
                    dgvAbonnementsRevue.Columns["dateFinAbonnement"].HeaderText = "Fin abonnement";
                }
            }
            else
            {
                bdgCommandesListeRevue.DataSource = null;
            }
        }

        /// <summary>
        /// Tri sur les colonnes du datagrid des abonnements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAbonnementsRevue_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string nomColonne = dgvAbonnementsRevue.Columns[e.ColumnIndex].DataPropertyName;

            List<Abonnement> sortedList = lesAbonnements;

            switch (nomColonne)
            {
                case "IdCommande":
                    sortedList = lesAbonnements.OrderBy(x => x.IdCommande).ToList();
                    break;

                case "DateCommande":
                    sortedList = lesAbonnements.OrderBy(x => x.DateCommande).ToList();
                    break;

                case "Montant":
                    sortedList = lesAbonnements.OrderBy(x => x.Montant).ToList();
                    break;

                case "DateFinAbonnement":
                    sortedList = lesAbonnements.OrderBy(x => x.DateFinAbonnement).ToList();
                    break;
            }

            bdgCommandesListeRevue.DataSource = sortedList;
            bdgCommandesListeRevue.ResetBindings(false);
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRechercherRevue_Click(object sender, EventArgs e)
        {
            string idRevue = txtNumeroRevue.Text;
            if (string.IsNullOrWhiteSpace(idRevue))
            {
                MessageBox.Show("Identifiant invalide");
                return;
            }
            if (lesRevues == null || lesRevues.Count == 0)
                lesRevues = controller.GetAllRevues();

            Revue revue = lesRevues.Find(x => x.Id == idRevue);
            if (revue == null)
            {
                MessageBox.Show("Ce document n'est pas une revue");
                ViderChampsRevue();
                dgvAbonnementsRevue.DataSource = null;
                return;
            }

            Document document = controller.GetDocumentById<Document>("document", idRevue);

            documentSelectionneRevue = document;
            RemplirInfosDocumentRevue(document);
            RemplirInfosRevue(revue);
            lesAbonnements = controller.GetAllAbonnements(revue.Id);
            RemplirCommandesListeRevue(lesAbonnements);
        }

        /// <summary>
        /// Enregistrement d'une nouvelle commande d'abonnement pour la revue sélectionnée.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnregistreCommandeRevue_Click(object sender, EventArgs e)
        {
            if (documentSelectionneRevue == null)
            {
                MessageBox.Show("Aucune revue sélectionnée");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNumeroCommandeRevue.Text))
            {
                MessageBox.Show("Numéro de commande obligatoire");
                return;
            }

            if (!double.TryParse(txtMontantCommandeRevue.Text, out double montant))
            {
                MessageBox.Show("Montant invalide");
                return;
            }

            if (dtpDateFinAbonnementRevue.Value.Date < dtpDateCommandeRevue.Value.Date)
            {
                MessageBox.Show("La date de fin doit être après la date de commande");
                return;
            }

            Abonnement abonnement = new Abonnement(
                 txtNumeroCommandeRevue.Text.Trim(),
                 dtpDateCommandeRevue.Value,
                 montant,
                 documentSelectionneRevue.Id,
                 dtpDateFinAbonnementRevue.Value
             );

            bool ok = controller.CreerAbonnementRevue(abonnement);

            if (!ok)
            {
                MessageBox.Show("Erreur lors de l'ajout (id déjà existant ?)");
                return;
            }

            MessageBox.Show("Commande abonnement ajoutée avec succès");

            lesAbonnements =
                controller.GetAllAbonnements(documentSelectionneRevue.Id);

            RemplirCommandesListeRevue(lesAbonnements);
        }

        /// <summary>
        /// Suppression de la commande d'abonnement sélectionnée après vérification de la règle métier (pas d'exemplaire livré durant la période d'abonnement) et confirmation de l'utilisateur, puis rafraîchissement de la liste des abonnements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteCommandeRevue_Click(object sender, EventArgs e)
        {
            if (dgvAbonnementsRevue.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une commande.");
                return;
            }

            // Récupérer abonnement sélectionné
            Abonnement abonnement =
                (Abonnement)bdgCommandesListeRevue.Current;

            // Récupérer les exemplaires de la revue
            List<Exemplaire> lesExemplaires =
                controller.GetExemplairesRevue(abonnement.IdRevue);

            bool exemplaireDansPeriode = false;

            foreach (Exemplaire ex in lesExemplaires)
            {
                if (controller.ParutionDansAbonnement(
                    abonnement.DateCommande,
                    abonnement.DateFinAbonnement,
                    ex.DateAchat))
                {
                    exemplaireDansPeriode = true;
                    break;
                }
            }

            if (exemplaireDansPeriode)
            {
                MessageBox.Show(
                    "Impossible de supprimer cette commande : " +
                    "un exemplaire a été livré durant l’abonnement.");
                return;
            }

            DialogResult confirmation =
                MessageBox.Show(
                    "Voulez-vous vraiment supprimer cette commande ?",
                    "Confirmation",
                    MessageBoxButtons.YesNo);

            if (confirmation != DialogResult.Yes)
                return;

            bool supprimeOk =
                controller.SupprimerAbonnementRevue(abonnement.IdCommande);

            if (supprimeOk)
            {
                RemplirCommandesListeRevue(
                    controller.GetAllAbonnements(abonnement.IdRevue));

                MessageBox.Show("Commande supprimée avec succès.");
            }
            else
            {
                MessageBox.Show("Problème lors de la suppression.");
            }
        }
        #endregion
    }
}