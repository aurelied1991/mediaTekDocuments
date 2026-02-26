namespace MediaTekDocuments.view
{
    partial class FrmGestionDvd
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
            this.lblTitreFormDvd = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.cboPublic = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAjoutNumero = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTitre = new System.Windows.Forms.TextBox();
            this.txtRealisateur = new System.Windows.Forms.TextBox();
            this.cboGenre = new System.Windows.Forms.ComboBox();
            this.cboRayon = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSynopsis = new System.Windows.Forms.TextBox();
            this.nudDuree = new System.Windows.Forms.NumericUpDown();
            this.btnValiderAjoutDvd = new System.Windows.Forms.Button();
            this.btnAnnulerAjoutDvd = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCheminImage = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudDuree)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitreFormDvd
            // 
            this.lblTitreFormDvd.AutoSize = true;
            this.lblTitreFormDvd.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitreFormDvd.Location = new System.Drawing.Point(249, 27);
            this.lblTitreFormDvd.Name = "lblTitreFormDvd";
            this.lblTitreFormDvd.Size = new System.Drawing.Size(281, 29);
            this.lblTitreFormDvd.TabIndex = 28;
            this.lblTitreFormDvd.Text = "Ajout d\'un nouveau dvd";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(37, 96);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(62, 17);
            this.label19.TabIndex = 29;
            this.label19.Text = "Public :";
            // 
            // cboPublic
            // 
            this.cboPublic.FormattingEnabled = true;
            this.cboPublic.Location = new System.Drawing.Point(193, 94);
            this.cboPublic.Name = "cboPublic";
            this.cboPublic.Size = new System.Drawing.Size(121, 24);
            this.cboPublic.TabIndex = 32;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(416, 96);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 17);
            this.label7.TabIndex = 40;
            this.label7.Text = "N° de document :";
            // 
            // txtAjoutNumero
            // 
            this.txtAjoutNumero.Location = new System.Drawing.Point(571, 94);
            this.txtAjoutNumero.Name = "txtAjoutNumero";
            this.txtAjoutNumero.Size = new System.Drawing.Size(121, 22);
            this.txtAjoutNumero.TabIndex = 41;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(37, 156);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 17);
            this.label10.TabIndex = 42;
            this.label10.Text = "Titre :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(416, 156);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 17);
            this.label1.TabIndex = 43;
            this.label1.Text = "Réalisateur :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(37, 219);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 17);
            this.label2.TabIndex = 44;
            this.label2.Text = "Durée (en min)  :";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(37, 272);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(63, 17);
            this.label22.TabIndex = 46;
            this.label22.Text = "Genre :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(416, 219);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 17);
            this.label4.TabIndex = 47;
            this.label4.Text = "Rayon :";
            // 
            // txtTitre
            // 
            this.txtTitre.Location = new System.Drawing.Point(194, 156);
            this.txtTitre.Name = "txtTitre";
            this.txtTitre.Size = new System.Drawing.Size(120, 22);
            this.txtTitre.TabIndex = 48;
            // 
            // txtRealisateur
            // 
            this.txtRealisateur.Location = new System.Drawing.Point(571, 156);
            this.txtRealisateur.Name = "txtRealisateur";
            this.txtRealisateur.Size = new System.Drawing.Size(121, 22);
            this.txtRealisateur.TabIndex = 49;
            // 
            // cboGenre
            // 
            this.cboGenre.FormattingEnabled = true;
            this.cboGenre.Location = new System.Drawing.Point(194, 270);
            this.cboGenre.Name = "cboGenre";
            this.cboGenre.Size = new System.Drawing.Size(121, 24);
            this.cboGenre.TabIndex = 52;
            // 
            // cboRayon
            // 
            this.cboRayon.FormattingEnabled = true;
            this.cboRayon.Location = new System.Drawing.Point(571, 218);
            this.cboRayon.Name = "cboRayon";
            this.cboRayon.Size = new System.Drawing.Size(121, 24);
            this.cboRayon.TabIndex = 53;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(37, 374);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 17);
            this.label5.TabIndex = 54;
            this.label5.Text = "Synopsis :";
            // 
            // txtSynopsis
            // 
            this.txtSynopsis.AcceptsReturn = true;
            this.txtSynopsis.Location = new System.Drawing.Point(194, 372);
            this.txtSynopsis.Multiline = true;
            this.txtSynopsis.Name = "txtSynopsis";
            this.txtSynopsis.Size = new System.Drawing.Size(515, 90);
            this.txtSynopsis.TabIndex = 55;
            // 
            // nudDuree
            // 
            this.nudDuree.Location = new System.Drawing.Point(194, 219);
            this.nudDuree.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.nudDuree.Name = "nudDuree";
            this.nudDuree.Size = new System.Drawing.Size(120, 22);
            this.nudDuree.TabIndex = 56;
            // 
            // btnValiderAjoutDvd
            // 
            this.btnValiderAjoutDvd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValiderAjoutDvd.Location = new System.Drawing.Point(226, 482);
            this.btnValiderAjoutDvd.Name = "btnValiderAjoutDvd";
            this.btnValiderAjoutDvd.Size = new System.Drawing.Size(108, 34);
            this.btnValiderAjoutDvd.TabIndex = 57;
            this.btnValiderAjoutDvd.Text = "Valider";
            this.btnValiderAjoutDvd.UseVisualStyleBackColor = true;
            this.btnValiderAjoutDvd.Click += new System.EventHandler(this.btnValiderAjoutDvd_Click);
            // 
            // btnAnnulerAjoutDvd
            // 
            this.btnAnnulerAjoutDvd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAnnulerAjoutDvd.Location = new System.Drawing.Point(459, 482);
            this.btnAnnulerAjoutDvd.Name = "btnAnnulerAjoutDvd";
            this.btnAnnulerAjoutDvd.Size = new System.Drawing.Size(108, 34);
            this.btnAnnulerAjoutDvd.TabIndex = 58;
            this.btnAnnulerAjoutDvd.Text = "Annuler";
            this.btnAnnulerAjoutDvd.UseVisualStyleBackColor = true;
            this.btnAnnulerAjoutDvd.Click += new System.EventHandler(this.btnAnnulerAjoutDvd_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(37, 325);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(150, 17);
            this.label8.TabIndex = 59;
            this.label8.Text = "Chemin de l\'image :";
            // 
            // txtCheminImage
            // 
            this.txtCheminImage.Location = new System.Drawing.Point(194, 323);
            this.txtCheminImage.Name = "txtCheminImage";
            this.txtCheminImage.Size = new System.Drawing.Size(120, 22);
            this.txtCheminImage.TabIndex = 60;
            // 
            // FrmGestionDvd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 528);
            this.Controls.Add(this.txtCheminImage);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnAnnulerAjoutDvd);
            this.Controls.Add(this.btnValiderAjoutDvd);
            this.Controls.Add(this.nudDuree);
            this.Controls.Add(this.txtSynopsis);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboRayon);
            this.Controls.Add(this.cboGenre);
            this.Controls.Add(this.txtRealisateur);
            this.Controls.Add(this.txtTitre);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtAjoutNumero);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cboPublic);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.lblTitreFormDvd);
            this.Name = "FrmGestionDvd";
            this.Text = "Ajout d\'un nouveau Dvd";
            this.Load += new System.EventHandler(this.FrmGestionDvd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudDuree)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitreFormDvd;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox cboPublic;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtAjoutNumero;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTitre;
        private System.Windows.Forms.TextBox txtRealisateur;
        private System.Windows.Forms.ComboBox cboGenre;
        private System.Windows.Forms.ComboBox cboRayon;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSynopsis;
        private System.Windows.Forms.NumericUpDown nudDuree;
        private System.Windows.Forms.Button btnValiderAjoutDvd;
        private System.Windows.Forms.Button btnAnnulerAjoutDvd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCheminImage;
    }
}