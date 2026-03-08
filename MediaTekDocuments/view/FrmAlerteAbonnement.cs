using MediaTekDocuments.controller;
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
    public partial class FrmAlerteAbonnement : Form
    {
        private readonly BindingSource bdgAbonnementsFinissantListe = new BindingSource();
        private List<AbonnementFinissant> lesAbonnementsFinissant = new List<AbonnementFinissant>();
        private FrmMediatekController controller;

        /// <summary>
        /// Constructeur de la fenêtre d'alerte d'abonnement, qui prend en paramètre le contrôleur de FrmMediatek pour pouvoir accéder aux données nécessaires à l'affichage
        /// </summary>
        /// <param name="controllerMediatek"></param>
        public FrmAlerteAbonnement(FrmMediatekController controllerMediatek)
        {
            InitializeComponent();
            controller = controllerMediatek;
        }

        /// <summary>
        /// Événement de chargement de la fenêtre d'alerte d'abonnement, qui récupère la liste des abonnements finissant à partir du contrôleur et remplit le DataGridView avec ces données
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAlerteAbonnement_Load(object sender, EventArgs e)
        {
            lesAbonnementsFinissant = controller.GetAbonnementsFinissant();

            RemplirAbonnementsFinissantListe(lesAbonnementsFinissant);
        }

        /// <summary>
        /// Méthode qui remplit le DataGridView avec la liste des abonnements finissant, en utilisant un BindingSource pour faciliter la liaison des données
        /// </summary>
        /// <param name="abonnementsFinissant"></param>
        public void RemplirAbonnementsFinissantListe(List<AbonnementFinissant> abonnementsFinissant)
        {
            if (abonnementsFinissant != null && abonnementsFinissant.Count > 0)
            {
                bdgAbonnementsFinissantListe.DataSource = abonnementsFinissant;
                dgvListeRevuesFinAbonnement.DataSource = bdgAbonnementsFinissantListe;

                dgvListeRevuesFinAbonnement.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.AllCells;
            }
            else
            {
                dgvListeRevuesFinAbonnement.DataSource = null;
            }
        }

        /// <summary>
        /// Événement de clic sur le bouton de fermeture de la fenêtre d'alerte d'abonnement, qui ferme simplement la fenêtre lorsque l'utilisateur clique dessus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFermerFenêtre_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
