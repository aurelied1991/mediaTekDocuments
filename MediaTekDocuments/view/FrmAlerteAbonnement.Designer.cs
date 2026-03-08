namespace MediaTekDocuments.view
{
    partial class FrmAlerteAbonnement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.dgvListeRevuesFinAbonnement = new System.Windows.Forms.DataGridView();
            this.btnFermerFenêtre = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListeRevuesFinAbonnement)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(68, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(606, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Liste des revues dont l\'abonnement se finit dans moins de 30 jours";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dgvListeRevuesFinAbonnement
            // 
            this.dgvListeRevuesFinAbonnement.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListeRevuesFinAbonnement.Location = new System.Drawing.Point(142, 81);
            this.dgvListeRevuesFinAbonnement.Name = "dgvListeRevuesFinAbonnement";
            this.dgvListeRevuesFinAbonnement.ReadOnly = true;
            this.dgvListeRevuesFinAbonnement.RowHeadersWidth = 51;
            this.dgvListeRevuesFinAbonnement.RowTemplate.Height = 24;
            this.dgvListeRevuesFinAbonnement.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListeRevuesFinAbonnement.Size = new System.Drawing.Size(506, 221);
            this.dgvListeRevuesFinAbonnement.TabIndex = 1;
            // 
            // btnFermerFenêtre
            // 
            this.btnFermerFenêtre.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFermerFenêtre.Location = new System.Drawing.Point(276, 323);
            this.btnFermerFenêtre.Name = "btnFermerFenêtre";
            this.btnFermerFenêtre.Size = new System.Drawing.Size(227, 39);
            this.btnFermerFenêtre.TabIndex = 2;
            this.btnFermerFenêtre.Text = "Fermer la fenêtre";
            this.btnFermerFenêtre.UseVisualStyleBackColor = true;
            this.btnFermerFenêtre.Click += new System.EventHandler(this.btnFermerFenêtre_Click);
            // 
            // FrmAlerteAbonnement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 405);
            this.Controls.Add(this.btnFermerFenêtre);
            this.Controls.Add(this.dgvListeRevuesFinAbonnement);
            this.Controls.Add(this.label1);
            this.Name = "FrmAlerteAbonnement";
            this.Text = "Abonnements finissant dans moins de 30 jours";
            this.Load += new System.EventHandler(this.FrmAlerteAbonnement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListeRevuesFinAbonnement)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvListeRevuesFinAbonnement;
        private System.Windows.Forms.Button btnFermerFenêtre;
    }
}