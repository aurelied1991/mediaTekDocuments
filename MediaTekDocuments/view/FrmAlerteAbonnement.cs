using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Fenêtre affichant les abonnements de revues se terminant dans moins de 30 jours
    /// </summary>
    public partial class FrmAlerteAbonnement : Form
    {
        /// <summary>
        /// Source de données utilisée pour l'affichage des abonnements dans le DataGridView
        /// </summary>
        private readonly BindingSource bdgAbonnementsFinissantListe = new BindingSource();
        /// <summary>
        /// Liste des abonnements se terminant prochainement
        /// </summary>
        private List<AbonnementFinissant> lesAbonnementsFinissant = new List<AbonnementFinissant>();
        /// <summary>
        /// Contrôleur permettant d'accéder aux données de l'application
        /// </summary>
        private readonly FrmMediatekController controller;

        /// <summary>
        /// Initialise la fenêtre d'alerte des abonnements
        /// </summary>
        /// <param name="controllerMediatek">Contrôleur principal de l'application</param>
        public FrmAlerteAbonnement(FrmMediatekController controllerMediatek)
        {
            InitializeComponent();
            controller = controllerMediatek;
        }

        /// <summary>
        /// Charge les abonnements se terminant prochainement lors de l'ouverture de la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAlerteAbonnement_Load(object sender, EventArgs e)
        {
            lesAbonnementsFinissant = controller.GetAbonnementsFinissant();
            RemplirAbonnementsFinissantListe(lesAbonnementsFinissant);
        }

        /// <summary>
        /// Remplit le DataGridView avec la liste des abonnements se terminant prochainement
        /// </summary>
        /// <param name="abonnementsFinissant">Liste des abonnements à afficher</param>
        public void RemplirAbonnementsFinissantListe(List<AbonnementFinissant> abonnementsFinissant)
        {
            if (abonnementsFinissant != null && abonnementsFinissant.Count > 0)
            {
                bdgAbonnementsFinissantListe.DataSource = abonnementsFinissant;
                dgvListeRevuesFinAbonnement.DataSource = bdgAbonnementsFinissantListe;
                dgvListeRevuesFinAbonnement.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvListeRevuesFinAbonnement.Columns["titreRevue"].DisplayIndex = 0;
                dgvListeRevuesFinAbonnement.Columns["titreRevue"].HeaderText = "Nom de la revue";
                dgvListeRevuesFinAbonnement.Columns["dateFinAbonnement"].DisplayIndex = 1;
                dgvListeRevuesFinAbonnement.Columns["dateFinAbonnement"].HeaderText = "Date de fin d'abonnement";
            }
            else
            {
                dgvListeRevuesFinAbonnement.DataSource = null;
            }
        }

        /// <summary>
        /// Ferme la fenêtre d'alerte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFermerFenêtre_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
