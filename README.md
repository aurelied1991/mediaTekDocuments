# MediatekDocuments
Cette application permet de gérer les documents (livres, DVD, revues) d'une médiathèque. Elle a été codée en C# sous Visual Studio 2022. C'est une application de bureau, prévue d'être installée sur plusieurs postes accédant à la même base de données. Cette version inclut plusieurs nouvelles fonctionnalités pour faciliter la gestion des documents et des abonnements.<br>
L'application exploite une API REST pour accéder à la BDD MySQL. Des explications sont données plus loin, ainsi que le lien de récupération.
Voici le lien vers le dépôt d'origine dans lequel se trouve le README avec la présentation de l'application d'origine : https://github.com/CNED-SLAM/MediaTekDocuments <br> <br>

## Les nouvelles fonctionnalités ajoutées à l'application
### Fenêtre d'authentification
<img width="403" height="275" alt="Capture d&#39;écran 2026-03-17 153627" src="https://github.com/user-attachments/assets/f222b0b2-c72a-48f0-a82c-8c5df5cb6156" /> <br> <br>
Une nouvelle fenêtre permet aux utilisateurs de s'identifier en saisissant leur login et leur mot de passe. Si les informations sont incorrectes, un message d'erreur apparaît. Il est aussi possible d'effacer les champs en cliquant sur "Annuler". Toutefois, si les données entrées sont correctes, l'utilisateur peut se connecter et accéder à l'application selon les droits d'accès qui lui sont attribués. Le service culture n'a aucun accès à l'application, le service prêt peut consulter et rechercher des documents, tandis que les administrateurs et le service administratif disposent d'un accès complet à l'application. <br> <br>

### Fenêtre d'alerte sur les abonnements se terminant
<img width="526" height="307" alt="Capture d&#39;écran 2026-03-17 153833" src="https://github.com/user-attachments/assets/ac2c4827-96d4-454c-935c-03418352acde" /> <br> <br>
Cette fenêtre n'apparaît que pour les administrateurs et le service administratif afin de signaler les abonnements de revue se terminant dans moins de trente jours. Elle affiche le nom de la revue concernée et la date de la fin de l'abonnement, permettant ainsi de suivre facilement les renouvellements à prévoir. <br> <br>

### Onglet 1 : Livres
Cet onglet présente toujours la liste des livres mais avec différentes fonctionnalités supplémentaires afin d'offrir une gestion complète et sécurisée des livres. <br><br>
<img width="764" height="712" alt="Capture d&#39;écran 2026-03-17 153847" src="https://github.com/user-attachments/assets/81dd1820-cd9d-4505-80ea-ef80b617d6e3" /> <br>

#### Ajout d'un livre
En cliquant sur "Ajouter un livre", la fenêtre ci-dessous s'ouvre, permettant de saisir les informations du livre à ajouter. Les champs obligatoires incluent le numéro de document, le titre, l’auteur, le genre, le public et le rayon. Si des données obligatoires sont absentes, un message le signale. <br>
En cliquant sur le bouton "Valider", un message de confirmation s'affiche afin que l'utilisateur confirme ou annule l'ajout. S'il confirme, un message de validation apparaît pour signaler le succès de l'opération et le livre est ajouté dans la base de données. Il est aussi possible d'annuler l'ajout en cliquant sur le bouton "Annuler", et revenir ainsi à la fenêtre principale de l'application. <br><br>
<img width="524" height="344" alt="Capture d&#39;écran 2026-03-17 153858" src="https://github.com/user-attachments/assets/5d00bb42-6176-44dd-a0ad-b284f3d0f23e" /> <br>

#### Modification d'un livre
En sélectionnant un livre (obligatoire) et en cliquant sur "Modifier un livre", la fenêtre ci-dessous s'ouvre. Elle affiche les informations actuelles du livre sélectionné, pré-remplies dans les champs correspondants. Il est alors possible de modifier ces informations mais les champs obligatoires ne peuvent pas être laissés vides.<br>
Pour que les modifications soient prises en compte, l'utilisateur doit cliquer sur "Modifier", un message de confirmation s'affiche, permettant de valider ou annuler l'opération. Si l'utilisateur approuve, un message s'apparaît, confirmant la prise en compte des modifications et la base de données est mise à jour. Il est également possible d'annuler la modification en cours en cliquant sur "Annuler", revenant ainsi à la fenêtre principale de l'application.<br><br>
<img width="522" height="341" alt="Capture d&#39;écran 2026-03-17 153907" src="https://github.com/user-attachments/assets/9a2e4eb2-0842-492a-8a71-eff04881fff5" /> <br>

#### Suppression d'un livre
Un livre peut être supprimé de la base de données après sélection, en cliquant sur le bouton "Supprimer un livre". Cependant, si le livre est lié à des commandes en cours ou à des exemplaires, il ne sera pas possible de le supprimer et un message s'affichera pour en informer l'utilisateur, garantissant ainsi l'intégrité des données. Si la suppression est possible, une fenêtre de confirmation s'ouvre alors afin que l'utilisateur valide ou annule la suppression. <br>

#### Consultation des exemplaires d'un livre
Sélectionner un livre permet de visualiser ses exemplaires disponibles, avec leur numéro, la date d’achat et l’état actuel (voir capture d'écran ci-dessous). La liste est triable par chaque colonne pour faciliter la recherche.<br><br>
<img width="461" height="123" alt="Capture d&#39;écran 2026-03-17 153928" src="https://github.com/user-attachments/assets/fc5b7b98-1679-4bdb-b6b7-46c83df5bbcb" /> <br>

#### Modification de l'état d'un exemplaire d'un livre
En sélectionnant un exemplaire, il est possible de changer son état en cliquant sur "Modifier l'état", ce qui permet d'accéder au groupbox dédié. Les champs du numéro et de l’état actuel sont automatiquement remplis selon l’exemplaire sélectionné. Pour l'état, il suffit de changer celui-ci dans la combobox. Après modification, l’utilisateur peut valider ou annuler le changement avant que la base de données ne soit mise à jour.<br>

#### Suppression d'un exemplaire
La suppression d’un exemplaire se fait en sélectionnant celui-ci dans la liste, puis en cliquant sur le bouton "Supprimer". Une fenêtre de confirmation s’affiche alors afin de permettre à l’utilisateur de valider ou d’annuler l’opération, ce qui limite les risques de suppression accidentelle. Une fois la suppression confirmée, l’exemplaire est définitivement retiré de la base de données et n’apparaît plus dans la liste.<br> <br>

### Onglet 2 : DVD
Cet onglet présente toujours la liste des DVD mais avec différentes fonctionnalités supplémentaires afin d'offrir une gestion complète et sécurisée des DVD.<br><br>
<img width="771" height="711" alt="Capture d&#39;écran 2026-03-17 153944" src="https://github.com/user-attachments/assets/7531152b-f03f-4682-9d87-9343826f88d7" /> <br>

#### Ajout d'un DVD
En cliquant sur "Ajouter un DVD", la fenêtre ci-dessous s'ouvre, permettant de saisir les informations du dvd à ajouter. Les champs obligatoires incluent le numéro de document, le titre, le réalisateur, le synopsis, la durée, le genre, le public et le rayon. Si des données obligatoires sont absentes, un message le signale. <br>
En cliquant sur le bouton "Valider", un message de confirmation s'affiche afin que l'utilisateur confirme ou annule l'ajout. S'il confirme, un message de validation apparaît pour signaler le succès de l'opération et le DVD est ajouté dans la base de données. Il est aussi possible d'annuler l'ajout en cliquant sur le bouton "Annuler", et revenir ainsi à la fenêtre principale de l'application. <br><br>
<img width="518" height="396" alt="Capture d&#39;écran 2026-03-17 153954" src="https://github.com/user-attachments/assets/3a833e11-ae82-47b3-a77b-1069c7c12af7" /> <br>

#### Modification d'un DVD
En sélectionnant un DVD (obligatoire) et en cliquant sur "Modifier un DVD", la fenêtre ci-dessous s'ouvre. Elle affiche les informations actuelles du DVD sélectionné, pré-remplies dans les champs correspondants. Il est alors possible de modifier ces informations mais les champs obligatoires ne peuvent pas être laissés vides.<br>
Pour que les modifications soient prises en compte, l'utilisateur doit cliquer sur "Modifier", un message de confirmation s'affiche, permettant de valider ou annuler l'opération. Si l'utilisateur approuve, un message apparaît, confirmant la prise en compte des modifications et la base de données est mise à jour. Il est également possible d'annuler la modification en cours en cliquant sur "Annuler", revenant ainsi à la fenêtre principale de l'application.<br><br>
<img width="522" height="398" alt="Capture d&#39;écran 2026-03-17 154006" src="https://github.com/user-attachments/assets/201e2ad2-5204-46ed-81a6-204346510345" /> <br>

#### Suppression d'un DVD
Un DVD peut être supprimé de la base de données après sélection, en cliquant sur le bouton "Supprimer un DVD". Cependant, si le DVD est lié à des commandes en cours ou à des exemplaires, il ne sera pas possible de le supprimer et un message s'affichera pour en informer l'utilisateur, garantissant ainsi l'intégrité des données. Si la suppression est possible, une fenêtre de confirmation s'ouvre alors afin que l'utilisateur valide ou annule la suppression. <br>

#### Consultation des exemplaires d'un DVD
Sélectionner un DVD permet de visualiser ses exemplaires disponibles, avec leur numéro, la date d’achat et l’état actuel (voir capture d'écran ci-dessous). La liste est triable par chaque colonne pour faciliter la recherche.<br><br>
<img width="767" height="713" alt="Capture d&#39;écran 2026-03-17 154037" src="https://github.com/user-attachments/assets/fee736b7-408b-414d-a47e-2828fa62bed9" /> <br>

#### Modification de l'état d'un exemplaire d'un DVD
En sélectionnant un exemplaire, il est possible de changer son état en cliquant sur "Modifier l'état", ce qui permet d'accéder au groupbox dédié. Les champs du numéro et de l’état actuel sont automatiquement remplis selon l’exemplaire sélectionné. Pour l'état, il suffit de changer celui-ci dans la combobox. Après modification, l’utilisateur peut valider ou annuler le changement avant que la base de données ne soit mise à jour.<br>

#### Suppression d'un exemplaire
La suppression d’un exemplaire se fait en sélectionnant celui-ci dans la liste, puis en cliquant sur le bouton "Supprimer". Une fenêtre de confirmation s’affiche alors afin de permettre à l’utilisateur de valider ou d’annuler l’opération, ce qui limite les risques de suppression accidentelle. Une fois la suppression confirmée, l’exemplaire est définitivement retiré de la base de données et n’apparaît plus dans la liste.<br> <br>

### Onglet 3 : Revues
Cet onglet présente toujours la liste des revues mais avec différentes fonctionnalités supplémentaires afin d'offrir une gestion complète et sécurisée des revues.<br><br>
<img width="767" height="713" alt="Capture d&#39;écran 2026-03-17 154037" src="https://github.com/user-attachments/assets/8c0ecc69-4056-4f46-81e9-621e181ca6ff" /> <br>
#### Ajout d'une revue
En cliquant sur "Ajouter une revue", la fenêtre ci-dessous s'ouvre, permettant de saisir les informations de la revue à ajouter. Les champs obligatoires incluent le numéro de document, le titre, la périodicité, le délai de mise à disposition, le genre, le public et le rayon. Si des données obligatoires sont absentes, un message le signale. <br>
En cliquant sur le bouton "Valider", un message de confirmation s'affiche afin que l'utilisateur confirme ou annule l'ajout. S'il confirme, un message de validation apparaît pour signaler le succès de l'opération et la revue est ajouté dans la base de données. Il est aussi possible d'annuler l'ajout en cliquant sur le bouton "Annuler", et revenir ainsi à la fenêtre principale de l'application. <br><br>
<img width="520" height="344" alt="Capture d&#39;écran 2026-03-17 154103" src="https://github.com/user-attachments/assets/e8157f90-6b75-4ab5-a330-9b1b15d9ece7" /> <br>

#### Modification d'une revue
En sélectionnant une revue (obligatoire) et en cliquant sur "Modifier un revue", la fenêtre ci-dessous s'ouvre. Elle affiche les informations actuelles de la revue sélectionnée, pré-remplies dans les champs correspondants. Il est alors possible de modifier ces informations mais les champs obligatoires ne peuvent pas être laissés vides.<br>
Pour que les modifications soient prises en compte, l'utilisateur doit cliquer sur "Modifier", un message de confirmation s'affiche, permettant de valider ou annuler l'opération. Si l'utilisateur approuve, un message apparaît, confirmant la prise en compte des modifications et la base de données est mise à jour. Il est également possible d'annuler la modification en cours en cliquant sur "Annuler", revenant ainsi à la fenêtre principale de l'application.<br><br>
<img width="520" height="339" alt="Capture d&#39;écran 2026-03-17 154111" src="https://github.com/user-attachments/assets/2a8ec328-e393-46ad-a0e2-18655ff38fd1" /> <br>

#### Suppression d'une revue
Une revue peut être supprimée de la base de données après sélection, en cliquant sur le bouton "Supprimer une revue". Cependant, si la revue est liée à des abonnements en cours ou à des exemplaires, il ne sera pas possible de la supprimer et un message s'affichera pour en informer l'utilisateur, garantissant ainsi l'intégrité des données. Si la suppression est possible, une fenêtre de confirmation s'ouvre alors afin que l'utilisateur valide ou annule la suppression. <br><br>

### Onglet 4 : Parutions des revues
Cet onglet permet d'enregistrer la réception de nouvelles parutions d'une revue, de visualiser les exemplaires et modifier l'état de ces derniers.
Il se décompose désormais en 3 parties (groupbox).<br><br>
<img width="767" height="712" alt="Capture d&#39;écran 2026-03-17 154140" src="https://github.com/user-attachments/assets/4e262593-3404-4c1f-932a-8acf8a3bab53" /> <br>

#### Consultation les exemplaires d'une revue
Sélectionner une revue permet de visualiser ses exemplaires disponibles, avec leur numéro, la date d’achat et l’état actuel (voir capture d'écran ci-dessus). La liste est triable par chaque colonne pour faciliter la recherche.<br>

#### Modification de l'état d'un exemplaire d'une revue
En sélectionnant un exemplaire, il est possible de changer son état en cliquant sur "Modifier l'état", ce qui permet d'accéder au groupbox dédié. Les champs du numéro et de l’état actuel sont automatiquement remplis selon l’exemplaire sélectionné. Pour l'état, il suffit de changer celui-ci dans la combobox. Après modification, l’utilisateur peut valider ou annuler le changement avant que la base de données ne soit mise à jour.<br>

#### Suppression d'un exemplaire
La suppression d’un exemplaire se fait en sélectionnant celui-ci dans la liste, puis en cliquant sur le bouton "Supprimer". Une fenêtre de confirmation s’affiche alors afin de permettre à l’utilisateur de valider ou d’annuler l’opération, ce qui limite les risques de suppression accidentelle. Une fois la suppression confirmée, l’exemplaire est définitivement retiré de la base de données et n’apparaît plus dans la liste.<br> <br>

### Onglet 5 : Commandes de livres
Cet onglet permet de rechercher un livre afin d'ajouter et gérer les commandes qui lui sont associées.<br><br>
<img width="770" height="609" alt="Capture d&#39;écran 2026-03-17 154214" src="https://github.com/user-attachments/assets/8dd75ff9-f16d-4890-a2d3-eaa1f941570d" /> <br>
#### Rechercher un livre
La première partie permet de saisir un numéro de document afin de rechercher un livre. Si le numéro existe, l'ensemble des informations du livre s'affiche dans les champs correspondants ainsi que la liste des commandes déjà enregistrées. Pour chaque commande, sont affichés son numéro, la date de la commande, le montant, le nombre d'exemplaires ainsi que son état de suivi.<br>
#### Nouvelle commande du livre
Cette section, située en bas de la fenêtre, permet d'ajouter une nouvelle commande pour le livre sélectionné. Tous les champs sont obligatoires : numéro de la commande, date de la commande, nombre d'exmplaires et le montant. Une fois les informations saisies, il suffit ensuite de cliquer sur "Enregistrer la commande" pour que celle-ci soit ajoutée en base de données et apparaisse dans la liste des commandes. Les champs de saisie sont ensuite réinitialisés. Si jamais le numéro de commande existe déjà, l'enregistrement est refusé et un message apparaît pour en informer l'utilisateur.<br>
#### Modifier le suivi de la commande
Pour accéder à cette partie, il est nécessaire de sélectionner une commande du livre puis de cliquer sur "Modifier le suivi de la commande". Les champs sont alors pré-remplis avec les informations de la commande sélectionnée. L’utilisateur peut faire évoluer l’état de la commande. Si cette dernière est "en cours", elle peut être "livrée" ou "relancée". Pour passer à l'étape "réglée", la commande doit obligatoirement avoir été livrée auparavant. Aucun retour en arrière vers une étape précédente n'est possibe. <br>
Lorsqu'une commande passe à l'étape de suivi "livrée", les exemplaires correspondants sont automatiquement créés dans la base de données. Ils deviennent alors visibles dans les onglets précédents, dans la partie dédiée aux exemplaires. <br>
Il est aussi possible d’annuler une modification en cliquant sur "Annuler", ce qui permet de revenir à l’état initial sans enregistrer de changement.<br>
#### Suppression d'une commande
Une commande peut être supprimée après sélection en cliquant sur "Supprimer la commande". Cependant, seules les commandes en cours ou relancées peuvent être supprimées. Si l’utilisateur tente de supprimer une commande livrée ou réglée, un message s’affiche pour l’en informer. Lorsque la suppression est autorisée, une demande de confirmation apparaît afin de valider ou d’annuler l’opération, ce qui permet d’éviter toute suppression involontaire.<br> <br>

### Onglet 6 : Commandes de DVD
Cet onglet permet de rechercher un DVD afin d'ajouter et gérer les commandes qui lui sont associées.<br><br>
<img width="781" height="629" alt="Capture d&#39;écran 2026-03-17 154223" src="https://github.com/user-attachments/assets/0e111814-0cde-4d95-aaed-4c2b92673d14" /> <br>
#### Rechercher un DVD
La première partie permet de saisir un numéro de document afin de rechercher un DVD. Si le numéro existe, l'ensemble des informations du DVD s'affiche dans les champs correspondants ainsi que la liste des commandes déjà enregistrées. Pour chaque commande, sont affichés son numéro, la date de la commande, le montant, le nombre d'exemplaires ainsi que son état de suivi.<br>
#### Nouvelle commande du DVD
Cette section, située en bas de la fenêtre, permet d'ajouter une nouvelle commande pour le DVD sélectionné. Tous les champs sont obligatoires : numéro de la commande, date de la commande, nombre d'exmplaires et le montant. Une fois les informations saisies, il suffit ensuite de cliquer sur "Enregistrer la commande" pour que celle-ci soit ajoutée en base de données et apparaisse dans la liste des commandes. Les champs de saisie sont ensuite réinitialisés. Si jamais le numéro de commande existe déjà, l'enregistrement est refusé et un message apparaît pour en informer l'utilisateur.<br>
#### Modifier le suivi de la commande
Pour accéder à cette partie, il est nécessaire de sélectionner une commande du DVD puis de cliquer sur "Modifier le suivi de la commande". Les champs sont alors pré-remplis avec les informations de la commande sélectionnée. L’utilisateur peut faire évoluer l’état de la commande. Si cette dernière est "en cours", elle peut être "livrée" ou "relancée". Pour passer à l'étape "réglée", la commande doit obligatoirement avoir été livrée auparavant. Aucun retour en arrière vers une étape précédente n'est possibe. <br>
Lorsqu'une commande passe à l'étape de suivi "livrée", les exemplaires correspondants sont automatiquement créés dans la base de données. Ils deviennent alors visibles dans les onglets précédents, dans la partie dédiée aux exemplaires. <br>
#### Suppression d'une commande
Une commande peut être supprimée après sélection en cliquant sur "Supprimer la commande". Cependant, seules les commandes en cours ou relancées peuvent être supprimées. Si l’utilisateur tente de supprimer une commande livrée ou réglée, un message s’affiche pour l’en informer. Lorsque la suppression est autorisée, une demande de confirmation apparaît afin de valider ou d’annuler l’opération, ce qui permet d’éviter toute suppression involontaire.<br> <br>

### Onglet 7 : Commandes de revues
Cet onglet permet de rechercher une revue afin d'ajouter et gérer les commandes (abonnements) qui lui sont associées.<br><br>
<img width="786" height="606" alt="Capture d&#39;écran 2026-03-17 155604" src="https://github.com/user-attachments/assets/3ad53c66-36be-4cf3-ad28-9806fd40ce2b" /> <br>
#### Rechercher une revue
La première partie permet de saisir un numéro de document afin de rechercher une revue. Si le numéro existe, l'ensemble des informations de la revue s'affiche dans les champs correspondants ainsi que la liste des commandes déjà enregistrées. Pour chaque commande, sont affichés son numéro, la date de la commande, le montant et la date de la fin de l'abonnement.<br>
#### Nouvelle commande d'une revue
Cette section, située en bas de la fenêtre, permet d'ajouter une nouvelle commande (abonnement) pour la revue sélectionnée. Tous les champs sont obligatoires : numéro de la commande, date de la commande, montant et date de fin de l'abonnement. Une fois les informations saisies, il suffit ensuite de cliquer sur "Enregistrer la commande" pour que celle-ci soit ajoutée en base de données et apparaisse dans la liste des commandes. Les champs de saisie sont ensuite réinitialisés. Si jamais le numéro de commande existe déjà, l'enregistrement est refusé et un message apparaît pour en informer l'utilisateur.<br>
#### Suppression d'une commande
Une commande peut être supprimée après sélection en cliquant sur "Supprimer la commande". Cependant, les commandes n'ayant pas un exemplaire de revue reçu entre la date de début et la date de fin de l'abonnement ne peuvent pas être supprimées et un message en informe l'utilisateur. Lorsque la suppression est autorisée, une demande de confirmation apparaît afin de valider ou d’annuler l’opération, ce qui permet d’éviter toute suppression involontaire.<br> <br>

## La base de données
La base de données 'mediatek86 ' est au format MySQL.<br>
Voici sa structure :<br><br>
![mcd_mediatekdocuments_fin](https://github.com/user-attachments/assets/0610496f-0e7c-4536-b951-7ace1c9f46e1) <br>

### L'évolution de la base de données
La base de données a été enrichie afin de prendre en compte les nouvelles fonctionnalités liées à l’authentification et à la gestion des commandes. Trois nouvelles tables ont ainsi été ajoutées : <br>
- Suivi : table contenant les différentes étapes possibles de suivi d'une commande de livre ou de DVD (en cours, relancée, livrée, réglée). Chaque commande est associée à une seule étape de suivi, mais une même étape peut concerner plusieurs commandes.
- Service : table recensant les différents services de la médiathèque auxquels peuvent appartenir le personnel (administrateur, administratif, prêt et culture) et permettant de gérer les droits d'accès à l'application.
- Utilisateur : table contenant l'ensemble des utilisateurs de l'application qui peuvent s'authentifier sur cette dernière. Chaque utilisateur est rattaché à un seul service, ce qui détermine ses droits et son niveau d’accès.<br> <br>

## L'API REST
L'accès à la BDD se fait à travers une API REST protégée par une authentification basique.<br>
Le code de l'API se trouve ici :<br>
https://github.com/aurelied1991/rest_mediatekdocuments<br>
avec toutes les explications pour l'utiliser (dans le readme).<br> <br>

## Installation de l'application
Ce mode opératoire permet d'installer et d'utiliser l'application. <br>
### Installation avec l’installateur (recommandée)
Pour une utilisation simple de l’application :
 - Télécharger le projet puis le décompresser
 - Ouvrir le dossier du projet
 - Lancer l’installateur MediaTekDocumentsInstalleur
 - Suivre les étapes d’installation
 - Lancer l’application via le raccourci créé sur le bureau ou dans le menu Démarrer
L’application est configurée pour fonctionner avec l’API en ligne (apirestmediatekdocuments.atwebpages.com). Aucune modification n’est nécessaire.<br>

### Utilisation avec une API locale (optionnel)
Pour utiliser l’application avec une API REST locale, il est nécessaire de :
 - S’assurer d’avoir téléchargé le projet
 - Récupérer et installer l'API REST (https://github.com/aurelied1991/rest_mediatekdocuments) ainsi que la base de données (les explications sont données dans le readme correspondant).<br>
 - Avec une API locale, il est nécessaire de modifier la valeur de uriApi en indiquant l’URL de l’API locale dans App.config : http://localhost/rest_mediatekdocuments/
