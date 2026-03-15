using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MediaTekDocuments.view

{

    /// <summary>
    /// Fenêtre principale de l'application Mediatek qui permet la gestion des documents, commandes et abonnements
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        /// <summary>
        /// Contrôleur permettant d'accéder aux données de l'application
        /// </summary>
        private readonly FrmMediatekController controller;
        /// <summary>
        /// Source de données pour les genres
        /// </summary>
        private readonly BindingSource bdgGenres = new BindingSource();
        /// <summary>
        /// Source de données pour les publics
        /// </summary>
        private readonly BindingSource bdgPublics = new BindingSource();
        /// <summary>
        /// Source de données pour les rayons
        /// </summary>
        private readonly BindingSource bdgRayons = new BindingSource();
        /// <summary>
        /// Constantes représentant les états possibles d'une commande
        /// </summary>
        private const string EN_COURS = "0001";
        private const string LIVREE = "0002";
        private const string REGLEE = "0003";
        private const string RELANCEE = "0004";
        /// <summary>
        /// Indique si les combobox sont en cours de chargement
        /// </summary>
        private bool chargementCombo = false;
        /// <summary>
        /// Utilisateur actuellement connecté
        /// </summary>
        private Utilisateur utilisateurConnecte;

        // Messages
        private const string MESSAGE_SELECTION_COMMANDE = "Veuillez sélectionner une commande.";
        private const string NUMERO_INTROUVABLE = "Numéro introuvable";

        // Titres MessageBox
        private const string TITRE_CONFIRMATION = "Confirmation";
        private const string COL_ERREUR = "Erreur";

        // Constantes pour les colonnes des datagridview
        private const string COL_ID_COMMANDE = "IdCommande";
        private const string COL_DATE_COMMANDE = "DateCommande";
        private const string COL_MONTANT = "Montant";
        private const string COL_NB_EXEMPLAIRE = "nbExemplaire";
        private const string COL_ID_LIVRE_DVD = "idLivreDvd";
        private const string COL_ID_SUIVI = "idSuivi";
        private const string COL_LIBELLE_SUIVI = "libelleSuivi";
        private const string COL_ID_ETAT = "IdEtat";
        private const string COL_LIBELLE_ETAT = "LibelleEtat";
        private const string COL_PHOTO = "Photo";
        private const string COL_NUMERO = "Numero";
        private const string COL_DATE_ACHAT = "DateAchat";
        

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        // Constructeur par défaut (si besoin)
        public FrmMediatek()
        {
            InitializeComponent();
            controller = new FrmMediatekController();
        }

        /// <summary>
        /// Nouveau constructeur avec l'utilisateur connecté
        /// </summary>
        /// <param name="utilisateur"></param>
        public FrmMediatek(Utilisateur utilisateur) : this()  // appelle le constructeur par défaut
        {
            utilisateurConnecte = utilisateur;
            GererDroits();
            // vérification droits
            if (utilisateurConnecte.IdService == 1 || utilisateurConnecte.IdService == 2) // Administrteur ou administratif
            {
                FrmAlerteAbonnement frmAlerte = new FrmAlerteAbonnement(controller);
                frmAlerte.ShowDialog();
            }
        }

        /// <summary>
        /// Gère les droits d'accès en fonction du service de l'utilisateur connecté. 
        /// </summary>
        private void GererDroits()
        {
            if (utilisateurConnecte.IdService == 3)
            {
                TabPages.TabPages.Remove(tabCommandesLivres);
                TabPages.TabPages.Remove(tabCommandesDvd);
                TabPages.TabPages.Remove(tabCommandesRevues);
                TabPages.TabPages.Remove(tabReceptionRevue);

                btnAddLivre.Enabled = false;
                btnModifLivre.Enabled = false;
                btnDeleteLivre.Enabled = false;

                btnAddDVD.Enabled = false;
                btnModifDVD.Enabled = false;
                btnDeleteDVD.Enabled = false;

                btnAddRevue.Enabled = false;
                btnModifRevue.Enabled = false;
                btnDeleteRevue.Enabled = false;


                grpExemplaireDvd.Enabled = false;
                grbExemplairesLivre.Enabled = false;
            }
        }

        /// <summary>
        /// Remplit une ComboBox avec une liste de catégories.
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public static void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
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
        /// <summary>
        /// Source de données pour l'affichage de la liste des livres
        /// </summary>
        private readonly BindingSource bdgLivresListe = new BindingSource();
        /// <summary>
        /// Liste des livres récupérés depuis l'API
        /// </summary>
        private List<Livre> lesLivres = new List<Livre>();
        /// <summary>
        /// Source de données pour les exemplaires d'un livre
        /// </summary>
        private readonly BindingSource bdgExemplairesLivre = new BindingSource();
        /// <summary>
        /// Liste des exemplaires du livre sélectionné
        /// </summary>
        private List<Exemplaire> lesExemplairesLivre = new List<Exemplaire>();

        /// <summary>
        /// Ouverture de l'onglet Livres : appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
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
        /// Remplit le dategrid avec la liste de livres reçue en paramètre
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
                    if (utilisateurConnecte.IdService == 1 || utilisateurConnecte.IdService == 2)
                    {
                        grbExemplairesLivre.Enabled = true; // active seulement pour admin/administratif
                    }
                    AfficheExemplairesLivre(livre.Id);
                }
                else
                {
                    MessageBox.Show(NUMERO_INTROUVABLE);
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche avec la saisie.
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
        /// Sur la sélection d'une ligne ou cellule dans le grid, affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null && bdgLivresListe.Position >= 0)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    if (livre != null && !string.IsNullOrEmpty(livre.Id))
                    {
                        AfficheLivresInfos(livre);
                    }
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
        /// Vide les zones de recherche et de filtre
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
        /// Évenement clic qui permet d'ouvrir le formulaire d'ajout d'un livre
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
        /// Évenement clic qui permet d'ouvrir le formulaire de modification d'un livre
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
        /// Évenement clic qui permet de supprimer un livre après confirmation de l'utilisateur
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
                TITRE_CONFIRMATION,
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

        /// <summary>
        /// Méthode qui récupère et affiche les exemplaires du livre sélectionné
        /// </summary>
        /// <param name="exemplaires"></param>
        private void RemplirExemplairesLivre(List<Exemplaire> exemplaires)
        {
            lesExemplairesLivre = exemplaires;
            if (exemplaires != null)
            {
                bdgExemplairesLivre.DataSource = null;
                bdgExemplairesLivre.DataSource = exemplaires;

                dgvExemplairesLivre.DataSource = null;
                dgvExemplairesLivre.DataSource = bdgExemplairesLivre;
                dgvExemplairesLivre.Refresh();

                // Masquer les colonnes inutiles
                dgvExemplairesLivre.Columns["Id"].Visible = false;
                dgvExemplairesLivre.Columns[COL_ID_ETAT].Visible = false;
                dgvExemplairesLivre.Columns[COL_PHOTO].Visible = false;

                dgvExemplairesLivre.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                // Colonnes principales
                dgvExemplairesLivre.Columns[COL_NUMERO].DisplayIndex = 0;
                dgvExemplairesLivre.Columns[COL_NUMERO].HeaderText = "N° exemplaire";

                dgvExemplairesLivre.Columns[COL_DATE_ACHAT].DisplayIndex = 1;
                dgvExemplairesLivre.Columns[COL_DATE_ACHAT].HeaderText = "Date d'achat";
                dgvExemplairesLivre.Columns[COL_DATE_ACHAT].DefaultCellStyle.Format = "dd/MM/yyyy";

                dgvExemplairesLivre.Columns[COL_LIBELLE_ETAT].DisplayIndex = 2;
                dgvExemplairesLivre.Columns[COL_LIBELLE_ETAT].HeaderText = "Etat";
            }
            else
            {
                bdgExemplairesLivre.DataSource = null;
            }
        }

        /// <summary>
        /// Récupère et affiche les exemplaires du livre sélectionné
        /// </summary>
        /// <param name="idDocument">Identifiant du document</param>
        private void AfficheExemplairesLivre(string idDocument)
        {
            // Récupère les exemplaires du livre via l'API / BDD
            lesExemplairesLivre = controller.GetExemplaires(idDocument);
            // Remplit le DataGridView avec la méthode spécifique pour les livres
            RemplirExemplairesLivre(lesExemplairesLivre);
        }

        /// <summary>
        /// Trie la liste des exemplaires selon la colonne sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesLivre_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string col = dgvExemplairesLivre.Columns[e.ColumnIndex].Name;
            List<Exemplaire> sorted;

            switch (col)
            {
                case COL_NUMERO:
                    sorted = lesExemplairesLivre.OrderBy(x => x.Numero).ToList();
                    break;
                case COL_DATE_ACHAT:
                    sorted = lesExemplairesLivre.OrderByDescending(x => x.DateAchat).ToList();
                    break;
                case COL_LIBELLE_ETAT:
                    sorted = lesExemplairesLivre.OrderBy(x => x.LibelleEtat).ToList();
                    break;
                default:
                    sorted = lesExemplairesLivre.ToList();
                    break;
            }
            RemplirExemplairesLivre(sorted);
        }

        /// <summary>
        /// Ouvre le formulaire de modification de l'état d'un exemplaire sélectionné
        /// </summary>
        /// <param name="sender">Objet déclencheur de l'événement</param>
        /// <param name="e">Données de l'événement</param>
        private void btnModiferExemplaireLivre_Click(object sender, EventArgs e)
        {
            if (dgvExemplairesLivre.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner un exemplaire !");
                return;
            }
            // Récupère l'exemplaire sélectionné
            if (dgvExemplairesLivre.CurrentRow.DataBoundItem is Exemplaire exemplaire)
            {
                grbExemplairesLivre.Enabled = false;
                grpModifExemplaireLivre.Enabled = true;

                // Récupérer tous les états
                List<Etat> listeEtats = controller.GetAllEtats();

                // Remplir le ComboBox
                cbxEtatExemplaireLivre.DataSource = listeEtats;
                cbxEtatExemplaireLivre.DisplayMember = "Libelle";
                cbxEtatExemplaireLivre.ValueMember = "Id";

                // Préselectionner l'état actuel de l'exemplaire
                cbxEtatExemplaireLivre.SelectedValue = exemplaire.IdEtat;
                txtNumeroExemplaireLivre.Text = exemplaire.Numero.ToString();
            }
        }

        /// <summary>
        /// Valide la modification de l'état d'un exemplaire de livre
        /// </summary>
        /// <param name="sender">Objet déclencheur de l'événement</param>
        /// <param name="e">Données de l'événement</param>
        private void btnValiderEtatLivre_Click(object sender, EventArgs e)
        {
            string idDocument = txbLivresNumero.Text;
            int numero = int.Parse(txtNumeroExemplaireLivre.Text);
            string idEtat = cbxEtatExemplaireLivre.SelectedValue.ToString();

            bool succes = controller.ModifierEtatExemplaire(idDocument, numero, idEtat);

            if (succes)
            {
                MessageBox.Show("Exemplaire modifié !");
                List<Exemplaire> lesExemplairesLivres = controller.GetExemplaires(idDocument);
                RemplirExemplairesLivre(lesExemplairesLivres);
                grpModifExemplaireLivre.Enabled = false;
                txtNumeroExemplaireLivre.Text = "";
                cbxEtatExemplaireLivre.SelectedIndex = -1;
                grbExemplairesLivre.Enabled = true;
            }
            else
            {
                MessageBox.Show("Erreur lors de la modification !");
            }
        }

        /// <summary>
        /// Annule la modification de l'état d'un exemplaire et réinitialise les champs
        /// </summary>
        /// <param name="sender">Objet déclencheur de l'événement</param>
        /// <param name="e">Données de l'événement</param>
        private void btnAnnulerModifEtatLivre_Click(object sender, EventArgs e)
        {
            grpModifExemplaireLivre.Enabled = false;
            txtNumeroExemplaireLivre.Text = "";
            cbxEtatExemplaireLivre.SelectedIndex = -1;
            grbExemplairesLivre.Enabled = true;
        }

        /// <summary>
        /// Supprime l'exemplaire sélectionné après confirmation de l'utilisateur
        /// </summary>
        /// <param name="sender">Objet déclencheur de l'événement</param>
        /// <param name="e">Données de l'événement</param>
        private void btnSupprimerExemplaireLivre_Click(object sender, EventArgs e)
        {
            var exemplaire = dgvExemplairesLivre.CurrentRow?.DataBoundItem as Exemplaire;

            if (exemplaire == null)
            {
                MessageBox.Show("Veuillez sélectionner un exemplaire à supprimer.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Confirmer la suppression de l'exemplaire {exemplaire.Numero} ?",
                TITRE_CONFIRMATION,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes)
                return;

            string idDocument = txbLivresNumero.Text;
            bool ok = controller.SupprimerExemplaire(idDocument, exemplaire.Numero);

            if (ok)
            {
                MessageBox.Show("Exemplaire supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                List<Exemplaire> lesExemplairesLivresMaj = controller.GetExemplaires(txbLivresNumero.Text);
                RemplirExemplairesLivre(lesExemplairesLivresMaj);
            }
            else
            {
                MessageBox.Show("Impossible de supprimer cet exemplaire.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Onglet Dvd
        /// <summary>
        /// Source de données utilisée pour l'affichage de la liste des DVD
        /// </summary>
        private readonly BindingSource bdgDvdListe = new BindingSource();
        /// <summary>
        /// Liste des DVD récupérés depuis l'API
        /// </summary>
        private List<Dvd> lesDvd = new List<Dvd>();
        /// <summary>
        /// Liste des exemplaires du DVD sélectionné
        /// </summary>
        private List<Exemplaire> lesExemplairesDvd = new List<Exemplaire>();


        /// <summary>
        /// Ouverture de l'onglet Dvds : remplissage des DataGridView et ComboBox (genres, rayons, publics)
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
        /// Remplit le DataGridView avec la liste de DVD passée en paramètre
        /// </summary>
        /// <param name="Dvds">liste de DVD à afficher</param>
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
        /// Recherche et affiche le DVD correspondant au numéro saisi.
        /// Active les options d'exemplaires si l'utilisateur a les droits.
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
                    if (utilisateurConnecte.IdService == 1 || utilisateurConnecte.IdService == 2)
                    {
                        grpExemplaireDvd.Enabled = true; // active seulement pour admin/administratif
                    }
                    AfficheExemplairesDvd(dvd.Id);
                }
                else
                {
                    MessageBox.Show(NUMERO_INTROUVABLE);
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affiche les DVD dont le titre contient la saisie.
        /// Se déclenche à chaque modification du TextBox de recherche.
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
        /// Affiche les informations détaillées du DVD sélectionné dans les zones correspondantes
        /// </summary>
        /// <param name="dvd">DVD à afficher</param>
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
        ///  Filtre la liste des DVD selon le genre sélectionné
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
        /// Filtre la liste des DVD selon le public sélectionné
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
        /// Filtre la liste des DVD selon le rayon sélectionné
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
            if (dgvDvdListe.CurrentCell != null && bdgDvdListe.Position >= 0)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    if (dvd != null && !string.IsNullOrEmpty(dvd.Id))
                    {
                        AfficheDvdInfos(dvd);            // Affiche les infos du DVD dans les TextBox/PictureBox
                    }
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
        /// Annule le filtre sur le public et réaffiche tous les DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Annule le filtre sur le rayon et réaffiche tous les DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Annule le filtre sur le genre et réaffiche tous les DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affiche la liste complète des DVD et annule tous les filtres et recherches
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// Vide toutes les zones de recherche et filtres
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
        /// Tri la liste des DVD selon la colonne cliquée
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
        /// Charge la liste complète des DVD depuis l'API et affiche tous les éléments
        /// </summary>
        private void ChargerListeDvd()
        {
            lesDvd = controller.GetAllDvd();
            RemplirDvdListeComplete();
        }

        /// <summary>
        ///  Ouvre le formulaire d'ajout d'un DVD et recharge la liste si ajout réussi
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
        /// Ouvre le formulaire de modification pour le DVD sélectionné et recharge la liste après modification
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
        /// Supprime le DVD sélectionné après confirmation et recharge la liste
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
                TITRE_CONFIRMATION,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                bool suppressionOk = controller.SupprimerDocument("dvd", id);

                if (suppressionOk)
                {
                    MessageBox.Show("Dvd supprimé avec succès.");
                    ChargerListeDvd();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression.");
                }
            }
        }

        /// <summary>
        /// Affiche les exemplaires du DVD sélectionné dans le DataGridView dédié
        /// </summary>
        /// <param name="idDocument">Identifiant du DVD</param>
        private void AfficheExemplairesDvd(string idDocument)
        {
            // Récupère les exemplaires via l'API / BDD
            lesExemplairesDvd = controller.GetExemplaires(idDocument);
            // Remplit le DGV spécifique DVD
            RemplirExemplairesDvd(lesExemplairesDvd);
        }

        /// <summary>
        /// Remplit le DataGridView des exemplaires DVD avec la liste fournie
        /// </summary>
        /// <param name="exemplaires">Liste des exemplaires à afficher</param>
        private void RemplirExemplairesDvd(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                dgvExemplaireDvd.Columns.Clear();
                dgvExemplaireDvd.AutoGenerateColumns = true;

                // Création et initialisation simplifiée du BindingSource
                BindingSource bdgExemplaireDvd = new BindingSource
                {
                    DataSource = exemplaires
                };

                dgvExemplaireDvd.DataSource = null;
                dgvExemplaireDvd.DataSource = bdgExemplaireDvd;
                dgvExemplaireDvd.Refresh();
                dgvExemplaireDvd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                // Masquer certaines colonnes
                if (dgvExemplaireDvd.Columns.Contains("Id"))
                    dgvExemplaireDvd.Columns["Id"].Visible = false;
                if (dgvExemplaireDvd.Columns.Contains(COL_ID_ETAT))
                    dgvExemplaireDvd.Columns[COL_ID_ETAT].Visible = false;
                if (dgvExemplaireDvd.Columns.Contains(COL_PHOTO))
                    dgvExemplaireDvd.Columns[COL_PHOTO].Visible = false;

                // Colonnes principales
                if (dgvExemplaireDvd.Columns.Contains(COL_NUMERO))
                {
                    dgvExemplaireDvd.Columns[COL_NUMERO].DisplayIndex = 0;
                    dgvExemplaireDvd.Columns[COL_NUMERO].HeaderText = "N° exemplaire";
                }

                if (dgvExemplaireDvd.Columns.Contains(COL_DATE_ACHAT))
                {
                    dgvExemplaireDvd.Columns[COL_DATE_ACHAT].DisplayIndex = 1;
                    dgvExemplaireDvd.Columns[COL_DATE_ACHAT].HeaderText = "Date d'achat";
                    dgvExemplaireDvd.Columns[COL_DATE_ACHAT].DefaultCellStyle.Format = "dd/MM/yyyy";
                }

                if (dgvExemplaireDvd.Columns.Contains(COL_LIBELLE_ETAT))
                {
                    dgvExemplaireDvd.Columns[COL_LIBELLE_ETAT].DisplayIndex = 2;
                    dgvExemplaireDvd.Columns[COL_LIBELLE_ETAT].HeaderText = "Etat";
                }
            }
            else
            {
                dgvExemplaireDvd.DataSource = null;
            }
        }

        /// <summary>
        /// Tri les exemplaires DVD selon la colonne cliquée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplaireDvd_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string col = dgvExemplaireDvd.Columns[e.ColumnIndex].Name;
            List<Exemplaire> sorted = new List<Exemplaire>();

            switch (col)
            {
                case COL_NUMERO:
                    sorted = lesExemplairesDvd.OrderBy(x => x.Numero).ToList();
                    break;
                case COL_DATE_ACHAT:
                    sorted = lesExemplairesDvd.OrderByDescending(x => x.DateAchat).ToList();
                    break;
                case COL_LIBELLE_ETAT:
                    sorted = lesExemplairesDvd.OrderBy(x => x.LibelleEtat).ToList();
                    break;
            }
            RemplirExemplairesDvd(sorted);
        }

        /// <summary>
        /// Ouvre le formulaire de modification pour l'état de l'exemplaire DVD sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifierEtatDvd_Click(object sender, EventArgs e)
        {
            if (dgvExemplaireDvd.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner un exemplaire !");
                return;
            }
            // Récupère l'exemplaire sélectionné
            var exemplaireDvd = dgvExemplaireDvd.CurrentRow.DataBoundItem as Exemplaire;

            if (exemplaireDvd == null) return;

            grpExemplaireDvd.Enabled = false;
            grpModifExemplaireDvd.Enabled = true;
            // Récupérer tous les états
            List<Etat> listeEtats = controller.GetAllEtats();

            // Remplir le ComboBox
            cbxEtatExemplaireDvd.DataSource = listeEtats;
            cbxEtatExemplaireDvd.DisplayMember = "Libelle"; // ce que voit l'utilisateur
            cbxEtatExemplaireDvd.ValueMember = "Id";        // ce qui sera envoyé à l'API

            // Préselectionner l'état actuel de l'exemplaire
            cbxEtatExemplaireDvd.SelectedValue = exemplaireDvd.IdEtat;
            txtNumeroExemplaireDvd.Text = exemplaireDvd.Numero.ToString();
        }

        /// <summary>
        /// Valide la modification de l'état d'un exemplaire DVD et rafraîchit l'affichage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValiderEtatExemplaireDvd_Click(object sender, EventArgs e)
        {
            string idDocument = txbDvdNumero.Text;
            int numero = int.Parse(txtNumeroExemplaireDvd.Text);
            string idEtat = cbxEtatExemplaireDvd.SelectedValue.ToString();

            bool succes = controller.ModifierEtatExemplaire(idDocument, numero, idEtat);

            if (succes)
            {
                MessageBox.Show("Exemplaire modifié !");
                List<Exemplaire> lesExemplairesListeDvd = controller.GetExemplaires(idDocument);
                RemplirExemplairesDvd(lesExemplairesListeDvd);
                grpModifExemplaireDvd.Enabled = false;
                txtNumeroExemplaireDvd.Text = "";
                cbxEtatExemplaireDvd.SelectedIndex = -1;
                grpExemplaireDvd.Enabled = true;
            }
            else
            {
                MessageBox.Show("Erreur lors de la modification !");
            }
        }

        /// <summary>
        /// Annule la modification de l'état d'un exemplaire DVD et restaure l'affichage initial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerModifEtatDvd_Click(object sender, EventArgs e)
        {
            grpModifExemplaireDvd.Enabled = false;
            txtNumeroExemplaireDvd.Text = "";
            cbxEtatExemplaireDvd.SelectedIndex = -1;
            grpExemplaireDvd.Enabled = true;
        }

        /// <summary>
        /// Supprime un exemplaire DVD après confirmation et rafraîchit la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimerExemplaireDvd_Click(object sender, EventArgs e)
        {
            var exemplaire = dgvExemplaireDvd.CurrentRow?.DataBoundItem as Exemplaire;

            if (exemplaire == null)
            {
                MessageBox.Show("Veuillez sélectionner un exemplaire à supprimer.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Confirmer la suppression de l'exemplaire {exemplaire.Numero} ?",
                TITRE_CONFIRMATION,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes)
                return;

            string idDocument = txbDvdNumero.Text;
            bool ok = controller.SupprimerExemplaire(idDocument, exemplaire.Numero);

            if (ok)
            {
                MessageBox.Show("Exemplaire supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                List<Exemplaire> lesExemplairesMajDvd = controller.GetExemplaires(txbDvdNumero.Text);
                RemplirExemplairesDvd(lesExemplairesMajDvd);
            }
            else
            {
                MessageBox.Show("Impossible de supprimer cet exemplaire.", COL_ERREUR, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Onglet Revues
        /// <summary>
        /// BindingSource pour gérer le binding de la DataGridView des revues.
        /// Permet de lier facilement la liste de revues aux colonnes du grid.
        /// </summary>
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        /// <summary>
        /// Liste complète des revues récupérées depuis l'API
        /// </summary>
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : récupère les données depuis l'API,
        /// remplit les combos (genre, rayon, public) et affiche la liste complète.
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
        /// Remplit le DataGridView avec la liste fournie.
        /// Masque certaines colonnes techniques (id, image) et ajuste la largeur des colonnes.
        /// </summary>
        /// <param name="revues">Liste des revues</param>
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
        /// Recherche d'une revue par numéro et affichage dans le datagrid.
        /// Si non trouvée, message d'erreur et affichage de la liste complète.
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
                    MessageBox.Show(NUMERO_INTROUVABLE);
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche dynamique par titre : filtrage au fur et à mesure de la saisie.
        /// Si le textBox est vide et aucun filtre actif, affiche la liste complète.
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
        /// Affiche les informations détaillées de la revue sélectionnée
        /// </summary>
        /// <param name="revue">La revue</param>
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
        /// Vide toutes les zones d'affichage des informations d'une revue
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
        /// Filtre sur le genre et affiche uniquement les revues correspondantes.
        /// Réinitialise les autres filtres et recherches.
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
        /// Filtre sur le public et affiche uniquement les revues correspondantes.
        /// Réinitialise les autres filtres et recherches.
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
        /// Filtre sur le rayon et affiche uniquement les revues correspondantes.
        /// Réinitialise les autres filtres et recherches.
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
        /// Sur la sélection d'une ligne ou cellule dans le DataGridView, affiche les infos de la revue
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
        /// Annule le filtre sur le public et réaffiche la liste complète
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Annule le filtre sur le rayon et réaffiche la liste complète
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Annule le filtre sur le genre et réaffiche la liste complète
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Réaffiche la liste complète et annule toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// Vide toutes les zones de recherche et de filtre
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
        /// Tri la liste des revues selon la colonne cliquée.
        /// Réinitialise les zones de recherche/filtres avant tri.
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
        /// Charge la liste complète des revues depuis l'API et l'affiche
        /// </summary>
        private void ChargerListeRevues()
        {
            lesRevues = controller.GetAllRevues();
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Événement clic qui permet d'ouvrir le formulaire d'ajout d'une revue.
        /// Après validation, recharge la liste depuis l'API et rafraîchit le DataGridView.
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
        /// Événement clic qui permet d'ouvrir le formulaire d'ajout d'une revue.
        /// Après validation, recharge la liste depuis l'API et rafraîchit le DataGridView.
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
        /// Événement clic qui permet de supprimer une revue après confirmation de l'utilisateur.
        /// Recharge le DataGridView si la suppression réussit.
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
                TITRE_CONFIRMATION,
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
        /// <summary>
        /// BindingSource pour gérer l'affichage de la liste des exemplaires d'une revue dans le DataGridView
        /// </summary>
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        /// <summary>
        /// Liste des exemplaires correspondant à la revue sélectionnée dans le DataGridView des revues
        /// </summary>
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        /// <summary>
        /// Liste des revues récupérée depuis l'API, utilisée pour les recherches et l'affichage des états dans le datagrid.
        /// </summary>
        private List<Etat> lesEtats = new List<Etat>();
        /// <summary>
        /// Constante représentant l'état "Neuf" lors de la création d'un nouvel exemplaire.
        /// </summary>
        private const string ETATNEUF = "00001";
        /// <summary>
        /// Constante représentant le libellé de l'état "neuf"
        /// </summary>
        private const string LIBELLE_ETAT = "neuf";

        /// <summary>
        /// Ouverture de l'onglet : Récupère les revues et états, et réinitialise le champ de saisie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            lesEtats = controller.GetAllEtats();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le DataGridView avec la liste d'exemplaires passée en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires à afficher</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;

                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.Columns["photo"].Visible = false;

                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
                dgvReceptionExemplairesListe.Columns["libelleEtat"].DisplayIndex = 2;

                dgvReceptionExemplairesListe.Columns["numero"].HeaderText = "N° exemplaire";
                dgvReceptionExemplairesListe.Columns["dateAchat"].HeaderText = "Date d'achat";
                dgvReceptionExemplairesListe.Columns["dateAchat"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvReceptionExemplairesListe.Columns["libelleEtat"].HeaderText = "Etat";
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
                    MessageBox.Show(NUMERO_INTROUVABLE);
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, réinitialise les champs et désactive la zone des exemplaires
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
        /// Affiche les informations d'une revue sélectionnée et ses exemplaires
        /// </summary>
        /// <param name="revue">La revue à afficher</param>
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
        /// Récupère et affiche les exemplaires de la revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocument = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplaires(idDocument);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Active ou désactive l'accès au groupBox exemplaire et réinitialise les champs
        /// </summary>
        /// <param name="acces">True pour activer, false pour désactiver</param>
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
        /// Enregistre un nouvel exemplaire dans la base
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
                    string libelleEtat = LIBELLE_ETAT;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument, libelleEtat);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", COL_ERREUR);
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
        /// Tri les exemplaires en fonction de la colonne cliquée
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
                case "LibelleEtat":
                    sortedList = lesExemplaires.OrderBy(o => o.LibelleEtat).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// Affiche l'image de l'exemplaire sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            // Vérifie qu'il y a bien au moins un élément et que la position est valide
            if (bdgExemplairesListe.List.Count > 0 && bdgExemplairesListe.Position >= 0)
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
                // Si rien n'est sélectionné ou que la liste est vide, on vide l'image
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }

        /// <summary>
        /// Ouvre le groupBox pour modifier l'état d'un exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifEtatRevue_Click(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner un exemplaire !");
                return;
            }
            // Récupère l'exemplaire sélectionné
            var exemplaireRevue = dgvReceptionExemplairesListe.CurrentRow.DataBoundItem as Exemplaire;

            if (exemplaireRevue == null) return;
            grpModifExemplaireRevue.Enabled = true;
            // Récupérer tous les états
            List<Etat> listeEtats = controller.GetAllEtats();

            // Remplir le ComboBox
            cbxEtatExemplaireRevue.DataSource = listeEtats;
            cbxEtatExemplaireRevue.DisplayMember = "Libelle"; // ce que voit l'utilisateur
            cbxEtatExemplaireRevue.ValueMember = "Id";        // ce qui sera envoyé à l'API

            // Préselectionner l'état actuel de l'exemplaire
            cbxEtatExemplaireRevue.SelectedValue = exemplaireRevue.IdEtat;
            txtNumeroExemplaireRevue.Text = exemplaireRevue.Numero.ToString();
        }

        /// <summary>
        /// Valide la modification de l'état d'un exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValiderEtatExemplaireRevue_Click(object sender, EventArgs e)
        {
            string idDocument = txbReceptionRevueNumero.Text;
            int numero = int.Parse(txtNumeroExemplaireRevue.Text);
            string idEtat = cbxEtatExemplaireRevue.SelectedValue.ToString();

            bool succes = controller.ModifierEtatExemplaire(idDocument, numero, idEtat);

            if (succes)
            {
                MessageBox.Show("Exemplaire modifié !");
                List<Exemplaire> lesExemplairesRevue = controller.GetExemplaires(idDocument);
                RemplirReceptionExemplairesListe(lesExemplairesRevue);
                grpModifExemplaireRevue.Enabled = false;
                txtNumeroExemplaireRevue.Text = "";
                cbxEtatExemplaireRevue.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Erreur lors de la modification !");
            }
        }

        /// <summary>
        /// Annule la modification de l'état d'un exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerModifExemplaireRevue_Click(object sender, EventArgs e)
        {
            grpModifExemplaireRevue.Enabled = false;
            txtNumeroExemplaireRevue.Text = "";
            cbxEtatExemplaireRevue.SelectedIndex = -1;
        }

        /// <summary>
        /// Supprime un exemplaire après confirmation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteExemplaireRevue_Click(object sender, EventArgs e)
        {
            var exemplaire = dgvReceptionExemplairesListe.CurrentRow?.DataBoundItem as Exemplaire;

            if (exemplaire == null)
            {
                MessageBox.Show("Veuillez sélectionner un exemplaire à supprimer.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Confirmer la suppression de l'exemplaire {exemplaire.Numero} ?",
                TITRE_CONFIRMATION,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes)
                return;

            string idDocument = txbReceptionRevueNumero.Text;
            bool ok = controller.SupprimerExemplaire(idDocument, exemplaire.Numero);

            if (ok)
            {
                MessageBox.Show("Exemplaire supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                List<Exemplaire> lesExemplairesMajRevue = controller.GetExemplaires(txbReceptionRevueNumero.Text);
                RemplirReceptionExemplairesListe(lesExemplairesMajRevue);
            }
            else
            {
                MessageBox.Show("Impossible de supprimer cet exemplaire.", COL_ERREUR, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Onglet Commandes de livres
        /// <summary>
        /// BindingSource pour le DataGridView des commandes
        /// </summary>
        private readonly BindingSource bdgCommandesListe = new BindingSource();
        /// <summary>
        /// Liste des commandes de livres récupérées depuis l'API
        /// </summary>
        private List<CommandeDocument> lesCommandes = new List<CommandeDocument>();
        /// <summary>
        /// D Document sélectionné dans le DataGridView
        /// </summary>
        private Document documentSelectionne;

        /// <summary>
        /// Initialisation de l'onglet Commandes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCommandes_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
        }

        /// <summary>
        /// Affiche les informations d'un document sélectionné
        /// </summary>
        /// <param name="documentSelectionne">Document à afficher</param>
        private void RemplirInfosDocument(Document documentSelectionne)
        {
            if (documentSelectionne == null) return;
            txtTitre.Text = documentSelectionne.Titre;
            txtCheminImage.Text = documentSelectionne.Image;
        }

        /// <summary>
        /// Affichage des informations d'un livre sélectionné
        /// </summary>
        /// <param name="livre">Livre à afficher</param>
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
        /// Vide les champs d'affichage du document et du livre
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
        /// Remplit le DataGridView avec la liste de commandes passée en paramètre
        /// </summary>
        /// <param name="commandes">Liste des commandes</param>
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

                // Colonnes masquées
                dgvListeCommandes.Columns[COL_ID_LIVRE_DVD].Visible = false;
                dgvListeCommandes.Columns[COL_ID_SUIVI].Visible = false;

                // Colonnes configurées
                dgvListeCommandes.Columns[COL_ID_COMMANDE].DisplayIndex = 0;
                dgvListeCommandes.Columns[COL_ID_COMMANDE].HeaderText = "N°commande";

                dgvListeCommandes.Columns[COL_DATE_COMMANDE].DisplayIndex = 1;
                dgvListeCommandes.Columns[COL_DATE_COMMANDE].HeaderText = "Date";

                dgvListeCommandes.Columns[COL_MONTANT].DisplayIndex = 2;
                dgvListeCommandes.Columns[COL_MONTANT].HeaderText = COL_MONTANT;

                dgvListeCommandes.Columns[COL_NB_EXEMPLAIRE].DisplayIndex = 3;
                dgvListeCommandes.Columns[COL_NB_EXEMPLAIRE].HeaderText = "Exemplaires";

                dgvListeCommandes.Columns[COL_LIBELLE_SUIVI].DisplayIndex = 4;
                dgvListeCommandes.Columns[COL_LIBELLE_SUIVI].HeaderText = "Suivi";
            }
            else
            {
                bdgCommandesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Tri des commandes selon la colonne cliquée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvListeCommandes_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string nomColonne = dgvListeCommandes.Columns[e.ColumnIndex].DataPropertyName;
            List<CommandeDocument> sortedList = lesCommandes;

            switch (nomColonne)
            {
                case COL_ID_COMMANDE:
                    sortedList = lesCommandes.OrderBy(o => o.IdCommande).ToList();
                    break;

                case COL_DATE_COMMANDE:
                    sortedList = lesCommandes
                        .OrderBy(o => o.DateCommande)
                        .ToList();
                    break;

                case COL_MONTANT:
                    sortedList = lesCommandes.OrderBy(o => o.Montant).ToList();
                    break;

                case COL_NB_EXEMPLAIRE:
                    sortedList = lesCommandes.OrderBy(o => o.NbExemplaire).ToList();
                    break;

                case COL_LIBELLE_SUIVI:
                    sortedList = lesCommandes.OrderBy(o => o.LibelleSuivi).ToList();
                    break;
            }
            lesCommandes = sortedList;
            bdgCommandesListe.DataSource = lesCommandes;
            bdgCommandesListe.ResetBindings(false);
        }

        /// <summary>
        /// Recherche et affichage du livre correspondant au numéro saisi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercheCommandeLivre_Click(object sender, EventArgs e)
        {
            ChargerDocumentEtCommandes(txtIdLivre.Text.Trim(), true);
        }

        /// <summary>
        /// Enregistrement d'une nouvelle commande pour le livre sélectionné
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
        /// Affiche le suivi de la commande et permet sa modification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifierSuiviCommandeLivre_Click(object sender, EventArgs e)
        {
            if (dgvListeCommandes.CurrentRow == null)
            {
                MessageBox.Show(MESSAGE_SELECTION_COMMANDE);
                return;
            }
            grbModifierEtapeCommandeLivre.Enabled = true;
            string idCommande = dgvListeCommandes.CurrentRow.Cells[COL_ID_COMMANDE].Value.ToString();

            txtNumeroCommandeLivre.Text = idCommande;
            chargementCombo = true;

            cbxEtapeCommande.DataSource = controller.GetAllSuivis();
            cbxEtapeCommande.DisplayMember = COL_LIBELLE_SUIVI;
            cbxEtapeCommande.ValueMember = COL_ID_SUIVI;
            cbxEtapeCommande.SelectedValue = dgvListeCommandes.CurrentRow.Cells[COL_ID_SUIVI].Value.ToString();
            chargementCombo = false;
        }

        /// <summary>
        /// Enregistre la modification du suivi de la commande sélectionnée
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
                lesCommandes = controller.GetCommandesByDocument(dgvListeCommandes.CurrentRow.Cells[COL_ID_LIVRE_DVD].Value.ToString());
                RemplirCommandesListe(lesCommandes);
                grbModifierEtapeCommandeLivre.Enabled = false;
            }
            else
            {
                MessageBox.Show("Erreur lors de la mise à jour.");
            }
        }

        /// <summary>
        /// Vérifie la cohérence lors du changement d'étape d'une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxEtapeCommande_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cbxEtapeCommande.Focused) return;

            if (dgvListeCommandes.CurrentRow == null) return;
            if (cbxEtapeCommande.SelectedValue == null) return;

            string etatActuel = dgvListeCommandes.CurrentRow.Cells[COL_ID_SUIVI].Value.ToString();

            string nouvelEtat = cbxEtapeCommande.SelectedValue.ToString();

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
        /// Annule la modification du suivi et réinitialise les champs
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
        /// Supprime la commande sélectionnée après confirmation et met à jour la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimerCommandeLivre_Click(object sender, EventArgs e)
        {
            if (dgvListeCommandes.CurrentRow == null)
            {
                MessageBox.Show(MESSAGE_SELECTION_COMMANDE);
                return;
            }

            string idCommande = dgvListeCommandes.CurrentRow.Cells[COL_ID_COMMANDE].Value.ToString();

            DialogResult result =
                MessageBox.Show(
                    "Confirmer la suppression de la commande ?",
                    TITRE_CONFIRMATION,
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
            lesCommandes = controller.GetCommandesByDocument(dgvListeCommandes.CurrentRow.Cells[COL_ID_LIVRE_DVD].Value.ToString());
            RemplirCommandesListe(lesCommandes);
        }
        #endregion

        #region Onglet Commandes de Dvd
        /// <summary>
        /// BindingSource pour la gestion de l'affichage des commandes de dvd dans le datagridview
        /// </summary>
        private readonly BindingSource bdgCommandesListeDvd = new BindingSource();
        /// <summary>
        /// Liste des commandes de dvd récupérées pour le dvd sélectionné
        /// </summary>
        private List<CommandeDocument> lesCommandesDvd = new List<CommandeDocument>();
        /// <summary>
        /// Document sélectionné dans l'onglet des commandes DVD
        /// </summary>
        private Document documentSelectionneDvd;

        /// <summary>
        /// Initialisation de l'onglet Commandes DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCommandesDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
        }

        /// <summary>
        /// Affiche les informations du document sélectionné
        /// </summary>
        /// <param name="documentSelectionneDvd">Document à afficher</param>
        private void RemplirInfosDocumentDvd(Document documentSelectionneDvd)
        {
            if (documentSelectionneDvd == null) return;
            txtTitreDvd.Text = documentSelectionneDvd.Titre;
            txtCheminImageDvd.Text = documentSelectionneDvd.Image;
        }

        /// <summary>
        /// Affiche les informations du DVD sélectionné
        /// </summary>
        /// <param name="dvd">DVD à afficher</param>
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
        /// Vide les champs d'affichage du document et du DVD
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
        /// Remplit le DataGridView avec la liste de commandes DVD
        /// </summary>
        /// <param name="commandes">Liste des commandes</param>
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

                dgvListeCommandesDvd.Columns[COL_ID_LIVRE_DVD].Visible = false;
                dgvListeCommandesDvd.Columns[COL_ID_SUIVI].Visible = false;
                dgvListeCommandesDvd.Columns[COL_ID_COMMANDE].DisplayIndex = 0;
                dgvListeCommandesDvd.Columns[COL_ID_COMMANDE].HeaderText = "N°commande";
                dgvListeCommandesDvd.Columns[COL_DATE_COMMANDE].DisplayIndex = 1;
                dgvListeCommandesDvd.Columns[COL_DATE_COMMANDE].HeaderText = "Date";
                dgvListeCommandesDvd.Columns[COL_MONTANT].DisplayIndex = 2;
                dgvListeCommandesDvd.Columns[COL_MONTANT].HeaderText = COL_MONTANT;
                dgvListeCommandesDvd.Columns[COL_NB_EXEMPLAIRE].DisplayIndex = 3;
                dgvListeCommandesDvd.Columns[COL_NB_EXEMPLAIRE].HeaderText = "Exemplaires";
                dgvListeCommandesDvd.Columns[COL_LIBELLE_SUIVI].DisplayIndex = 4;
                dgvListeCommandesDvd.Columns[COL_LIBELLE_SUIVI].HeaderText = "Suivi";
            }
            else
            {
                bdgCommandesListeDvd.DataSource = null;
            }
        }

        /// <summary>
        /// Tri des commandes selon la colonne cliquée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvListeCommandesDvd_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string nomColonne = dgvListeCommandesDvd.Columns[e.ColumnIndex].DataPropertyName;
            List<CommandeDocument> sortedList = lesCommandesDvd;

            switch (nomColonne)
            {
                case COL_ID_COMMANDE:
                    sortedList = lesCommandesDvd.OrderBy(x => x.IdCommande).ToList();
                    break;

                case COL_DATE_COMMANDE:
                    sortedList = lesCommandesDvd.OrderBy(x => x.DateCommande).ToList();
                    break;

                case COL_MONTANT:
                    sortedList = lesCommandesDvd.OrderBy(x => x.Montant).ToList();
                    break;

                case COL_NB_EXEMPLAIRE:
                    sortedList = lesCommandesDvd.OrderBy(x => x.NbExemplaire).ToList();
                    break;

                case COL_LIBELLE_SUIVI:
                    sortedList = lesCommandesDvd.OrderBy(x => x.LibelleSuivi).ToList();
                    break;
            }
            bdgCommandesListeDvd.DataSource = sortedList;
            bdgCommandesListeDvd.ResetBindings(false);
        }

        /// <summary>
        /// Recherche et affichage du DVD correspondant au numéro saisi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercheDvd_Click(object sender, EventArgs e)
        {
            ChargerDocumentEtCommandes(txtIdDvd.Text.Trim(), false);
        }

        /// <summary>
        /// Enregistrement d'une nouvelle commande pour le DVD sélectionné
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
            lesCommandesDvd = controller.GetCommandesByDocument(documentSelectionneDvd.Id);
            RemplirCommandesListeDvd(lesCommandesDvd);
        }

        /// <summary>
        /// Affiche le suivi de la commande et permet sa modification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifierSuiviCommandeDvd_Click(object sender, EventArgs e)
        {
            if (dgvListeCommandesDvd.CurrentRow == null)
            {
                MessageBox.Show(MESSAGE_SELECTION_COMMANDE);
                return;
            }

            gbxSuiviDvd.Enabled = true;

            string idCommande = dgvListeCommandesDvd.CurrentRow.Cells["IdCommande"].Value.ToString();

            txtNumeroCommandeDvd.Text = idCommande;
            chargementCombo = true;
            cbxEtapeCommandeDvd.DataSource = controller.GetAllSuivis();
            cbxEtapeCommandeDvd.DisplayMember = COL_LIBELLE_SUIVI;
            cbxEtapeCommandeDvd.ValueMember = COL_ID_SUIVI;
            cbxEtapeCommandeDvd.SelectedValue = dgvListeCommandesDvd.CurrentRow.Cells[COL_ID_SUIVI].Value.ToString();
            chargementCombo = false;
        }

        /// <summary>
        /// Annule la modification du suivi et réinitialise les champs
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
        /// Enregistre la modification du suivi de la commande sélectionnée
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
        /// Vérifie la cohérence lors du changement d'étape d'une commande DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxEtapeCommandeDvd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cbxEtapeCommandeDvd.Focused) return;
            if (dgvListeCommandesDvd.CurrentRow == null) return;
            if (cbxEtapeCommandeDvd.SelectedValue == null) return;

            string etatActuel = dgvListeCommandesDvd.CurrentRow.Cells[COL_ID_SUIVI].Value.ToString();

            string nouvelEtat = cbxEtapeCommandeDvd.SelectedValue.ToString();

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
        /// Supprime la commande DVD sélectionnée après confirmation et met à jour la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteCommandeDvd_Click(object sender, EventArgs e)
        {
            if (dgvListeCommandesDvd.CurrentRow == null)
            {
                MessageBox.Show(MESSAGE_SELECTION_COMMANDE);
                return;
            }

            string idCommande = dgvListeCommandesDvd.CurrentRow.Cells[COL_ID_COMMANDE].Value.ToString();

            DialogResult result =
                MessageBox.Show(
                    "Confirmer la suppression de la commande ?",
                    TITRE_CONFIRMATION,
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
        /// <summary>
        /// BindingSource pour la DataGridView des abonnements d'une revue
        /// </summary>
        private readonly BindingSource bdgCommandesListeRevue = new BindingSource();
        /// <summary>
        /// Liste des abonnements pour la revue sélectionnée
        /// </summary>
        private List<Abonnement> lesAbonnements = new List<Abonnement>();
        /// <summary>
        /// Document de la revue sélectionnée
        /// </summary>
        private Document documentSelectionneRevue;

        /// <summary>
        /// Initialisation de l'onglet Commandes Revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCommandesRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
        }

        /// <summary>
        /// Affiche les informations du document sélectionné
        /// </summary>
        /// <param name="documentSelectionneRevue">Document à afficher</param>
        private void RemplirInfosDocumentRevue(Document documentSelectionneRevue)
        {
            if (documentSelectionneRevue == null)
                return;
            txtTitreRevue.Text = documentSelectionneRevue.Titre;
            txtCheminImageRevue.Text = documentSelectionneRevue.Image;
        }

        /// <summary>
        /// Affiche les informations de la revue sélectionnée
        /// </summary>
        /// <param name="revue">Revue à afficher</param>
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
        /// Vide les champs d'affichage du document et de la revue
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
        /// Remplit le DataGridView avec la liste des abonnements reçue en paramètre
        /// </summary>
        /// <param name="abonnements">Liste des abonnements</param>
        private void RemplirCommandesListeRevue(List<Abonnement> abonnements)
        {
            if (abonnements != null)
            {
                dgvAbonnementsRevue.Columns.Clear();
                dgvAbonnementsRevue.AutoGenerateColumns = true;
                bdgCommandesListeRevue.DataSource = abonnements;
                dgvAbonnementsRevue.DataSource = bdgCommandesListeRevue;
                dgvAbonnementsRevue.Refresh();
                dgvAbonnementsRevue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                // Colonnes masquées
                dgvAbonnementsRevue.Columns["idRevue"].Visible = false;

                // Colonnes configurées
                dgvAbonnementsRevue.Columns[COL_ID_COMMANDE].DisplayIndex = 0;
                dgvAbonnementsRevue.Columns[COL_ID_COMMANDE].HeaderText = "N° commande";

                dgvAbonnementsRevue.Columns[COL_DATE_COMMANDE].DisplayIndex = 1;
                dgvAbonnementsRevue.Columns[COL_DATE_COMMANDE].HeaderText = "Date commande";

                dgvAbonnementsRevue.Columns[COL_MONTANT].DisplayIndex = 2;
                dgvAbonnementsRevue.Columns[COL_MONTANT].HeaderText = COL_MONTANT;

                dgvAbonnementsRevue.Columns["dateFinAbonnement"].DisplayIndex = 3;
                dgvAbonnementsRevue.Columns["dateFinAbonnement"].HeaderText = "Fin abonnement";
            }
            else
            {
                bdgCommandesListeRevue.DataSource = null;
            }
        }

        /// <summary>
        /// Tri des abonnements selon la colonne cliquée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAbonnementsRevue_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string nomColonne = dgvAbonnementsRevue.Columns[e.ColumnIndex].DataPropertyName;

            List<Abonnement> sortedList = lesAbonnements;

            switch (nomColonne)
            {
                case COL_ID_COMMANDE:
                    sortedList = lesAbonnements.OrderBy(x => x.IdCommande).ToList();
                    break;

                case COL_DATE_COMMANDE:
                    sortedList = lesAbonnements.OrderBy(x => x.DateCommande).ToList();
                    break;

                case COL_MONTANT:
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
        /// Recherche et affichage de la revue correspondant au numéro saisi
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
        /// Enregistre une nouvelle commande d'abonnement pour la revue sélectionnée
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
        /// Supprime la commande d'abonnement sélectionnée après vérification des règles métiers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteCommandeRevue_Click(object sender, EventArgs e)
        {
            if (dgvAbonnementsRevue.SelectedRows.Count == 0)
            {
                MessageBox.Show(MESSAGE_SELECTION_COMMANDE);
                return;
            }

            // Récupérer abonnement sélectionné
            Abonnement abonnement =
                (Abonnement)bdgCommandesListeRevue.Current;

            // Récupérer les exemplaires de la revue
            List<Exemplaire> lesExemplairesListeRevue = controller.GetExemplaires(abonnement.IdRevue);

            bool exemplaireDansPeriode = false;

            foreach (Exemplaire ex in lesExemplairesListeRevue)
            {
                if (FrmMediatekController.ParutionDansAbonnement(
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
                    TITRE_CONFIRMATION,
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