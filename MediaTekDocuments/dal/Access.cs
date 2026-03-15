using MediaTekDocuments.manager;
using MediaTekDocuments.model;
using MediaTekDocuments.view;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using Serilog;


namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// Adresse de l'API
        /// </summary>
        private static readonly string uriApi = ConfigurationManager.AppSettings["uriApi"];
        /// <summary>
        /// Instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// Instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// Méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// Méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// Méthode HTTP pour update
        /// </summary>>
        private const string PUT = "PUT";
        /// <summary>
        /// Méthode HTTP pour delete
        /// </summary>
        private const string DELETE = "DELETE";
 

        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            // Configuration Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) // logs généraux
                .WriteTo.File("logs/errorlog.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error) // logs d'erreur
                .CreateLogger();
            try
            {
                string connectionName = "MediaTekDocuments.Properties.Settings.mediatekAuthenticationString";
                string authenticationString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
                Log.Information("Récupération de l'authenticationString réussie pour {ConnectionName}", connectionName);
                api = ApiRest.GetInstance(uriApi, authenticationString);
                Log.Information("API initialisée avec succès");
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Erreur lors de l'initialisation de Access");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Authentifie un utilisateur à partir de son login et de son mot de passe, en comparant le hash du mot de passe avec celui stocké en base de données
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Utilisateur AuthentifierUtilisateur(string login, string password)
        {
            String jsonLogin = convertToJson("login", login);

            // récupère la liste des utilisateurs correspondant au login
            List<Utilisateur> utilisateurs = TraitementRecup<Utilisateur>(GET, "utilisateur/" + jsonLogin, null);

            if (utilisateurs != null && utilisateurs.Count > 0)
            {
                Utilisateur utilisateur = utilisateurs[0];

                // Compare le hash du mot de passe avec celui en BDD
                if (utilisateur.MotDePasse.Equals(FrmAuthentification.HasherMotDePasse(password)))
                {
                    return utilisateur;
                }
            }
            else
            {
                MessageBox.Show("Utilisateur non trouvé !");
            }

            return null;
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if (instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre", null);
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon", null);
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public", null);
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne tous les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre", null);
            return lesLivres;
        }

        /// <summary>
        /// Retourne tous les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd", null);
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue", null);
            return lesRevues;
        }

        /// <summary>
        /// Retourne tous les exemplaires d'un document
        /// </summary>
        /// <param name="idDocument">id du document concerné</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplaires(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument, null);
            return lesExemplaires;
        }

        /// <summary>
        /// Retourne tous les états à partir de la BDD
        /// </summary>
        /// <returns></returns>
        public List<Etat> GetAllEtats()
        {
            return TraitementRecup<Etat>(GET, "etat", null);
        }

        /// <summary>
        /// Retourne les commandes associées à un document (livre ou dvd) via l'API distante
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
        public List<CommandeDocument> GetCommandesByDocument(string idDocument)
        {
            // Conversion en JSON comme les autres méthodes de l’API
            String jsonIdDocument = convertToJson("idLivreDvd", idDocument);

            List<CommandeDocument> lesCommandesDocuments =
                TraitementRecup<CommandeDocument>(
                    GET,
                    "commandedocument/" + jsonIdDocument,
                    null
                );
            return lesCommandesDocuments;
        }

        /// <summary>
        /// Retourne les abonnements associés à une revue via l'API distante
        /// </summary>
        /// <param name="idRevue"></param>
        /// <returns></returns>
        public List<Abonnement> GetAllAbonnements(string idRevue)
        {
            string jsonIdRevue = convertToJson("idRevue", idRevue);

            List<Abonnement> lesAbonnements =
                TraitementRecup<Abonnement>(
                    GET,
                    "abonnement/" + jsonIdRevue,
                    null
                );
            
            return lesAbonnements;
        }

        /// <summary>
        /// Retourne les abonnements finissants dans les 30 prochains jours via l'API distante
        /// </summary>
        /// <returns></returns>
        public List<AbonnementFinissant> GetAbonnementsFinissant()
        {
            List<AbonnementFinissant> lesAbonnements =
                TraitementRecup<AbonnementFinissant>(
                    GET,
                    "abonnementfinissant",
                    null
                );

            return lesAbonnements;
        }

        /// <summary>
        /// Récupère un document à partir de son id via l'API distante
        /// </summary>
        /// <typeparam name="T"></typeparam> Type d'objet à récupérer
        /// <param name="typeElement"></param> Route de l'API pour le type d'élément à récupérer
        /// <param name="id"></param> Id du document à récupérer
        /// <returns></returns>
        public T GetDocumentById<T>(string typeElement, string id)
        {
            string jsonId = convertToJson("id", id);

            List<T> elements =
                TraitementRecup<T>(GET, typeElement + "/" + jsonId, null);

            if (elements == null || elements.Count == 0)
                return default;

            return elements[0];
        }

        /// <summary>
        /// Récupère tous les suivis de commandes à partir de l'API distante
        /// </summary>
        /// <returns></returns>
        public List<Suivi> GetAllSuivis()
        {
            List<Suivi> lesSuivis = TraitementRecup<Suivi>(GET, "suivi", null);
            return lesSuivis;
        }

        /// <summary>
        /// Écriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            Log.Information("JSON envoyé à l'API : {Json}", jsonExemplaire);
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire", "champs=" + jsonExemplaire);
                if (liste != null)
                {
                    Log.Information("Exemplaire créé avec succès : document id={Id}, numéro={Numero}", exemplaire.Id, exemplaire.Numero);
                    return true;
                }
                else
                {
                    Log.Error("Échec création exemplaire : document id={Id}, numéro={Numero}", exemplaire.Id, exemplaire.Numero);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erreur lors de la création de l'exemplaire : json={Json}", jsonExemplaire);
            }
            return false;
        }

        /// <summary>
        /// Ajout d'un document (livre, dvd ou revue) en base de données
        /// </summary>
        /// <param name="typeElement"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool AjouterDocument(string typeElement, Object element)
        {
            String jsonElement = JsonConvert.SerializeObject(element);
            string parametres = "champs=" + Uri.EscapeDataString(jsonElement);

            try
            {
                List<Object> liste = TraitementRecup<Object>(
                POST,
                typeElement,
                parametres
                );

                if (liste != null)
                {
                    Log.Information("Document ajouté avec succès : type={Type}", typeElement);
                    return true;
                }
                else
                {
                    Log.Error("Échec ajout du document : type={Type}", typeElement);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erreur lors de l'ajout du document : type={Type}, json={Json}", typeElement, jsonElement);
            }
            return false;
        }

        /// <summary>
        /// Modification d'un document (livre, dvd ou revue) en base de données
        /// </summary>
        /// <param name="typeElement"></param>
        /// <param name="element"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ModifierDocument(string typeElement, object element, string id)
        {
            string jsonElement = JsonConvert.SerializeObject(element);
            string parametres = "champs=" + Uri.EscapeDataString(jsonElement);

            try
            { 
                List<Object> liste = TraitementRecup<Object>(PUT, typeElement + "/" + id, parametres);
                if (liste != null)
                {
                    Log.Information("Document modifié avec succès : type={Type}, id={Id}", typeElement, id);
                    return true;
                }
                else
                {
                    Log.Error("Échec modification du document : type={Type}, id={Id}", typeElement, id);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erreur lors de la modification du document : type={Type}, id={Id}, json={Json}", typeElement, id, jsonElement);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'un document (livre, dvd ou revue) en base de données
        /// </summary>
        /// <param name="typeElement"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SupprimerDocument(string typeElement, string id)
        {
            string jsonIdElement = "{\"Id\":\"" + id + "\"}";

            try 
            {
                List<Object> liste = TraitementRecup<Object>(DELETE, typeElement + "/" + jsonIdElement, null);
                if (liste != null)
                {
                    Log.Information("Document supprimé avec succès : type={Type}, id={Id}", typeElement, id);
                    return true;
                }
                else
                {
                    Log.Error("Échec suppression du document : type={Type}, id={Id}", typeElement, id);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erreur lors de la suppression du document : type={Type}, id={Id}", typeElement, id);
            }
            return false;
        }

        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée dans l'url</param>
        /// <param name="parametres">paramètres à envoyer dans le body, au format "chp1=val1&chp2=val2&..."</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T>(String methode, String message, String parametres)
        {
            // trans
            List<T> liste = new List<T>();
            try
            {
                JObject retour = api.RecupDistant(methode, message, parametres);
                // extraction du code retourné
                String code = (String)retour["code"];
                if (code == "200")
                {
                    // dans le cas du GET (select), récupération de la liste d'objets
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        // construction de la liste d'objets à partir du retour de l'api
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }
                }
                else
                {
                    Log.Error("Erreur API : code={Code} message={Message}", code, (string)retour["message"]);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Erreur lors de l'accès à l'API avec {Message}", message);
                return new List<T>();
            }
            return liste;
        }

        /// <summary>
        /// Création d'une commande de document (livre ou dvd) en base de données
        /// </summary>
        /// <param name="commandeDocument">Commander à créer</param>
        /// <returns>True si la création réussie</returns>
        public bool CreerCommandeDocument(CommandeDocument commandeDocument)
        {
            String jsonCommande = JsonConvert.SerializeObject(commandeDocument, new CustomDateTimeConverter());
            try
            {
                List<CommandeDocument> resultat = TraitementRecup<CommandeDocument>(POST, "commandedocument", "champs=" + jsonCommande);
                if (resultat != null)
                {
                    Log.Information("Commande créée avec succès pour le document id={Id}", commandeDocument.IdLivreDvd);
                    return true;
                }
                else
                {
                    Log.Error("Échec création commande pour le document id={Id}", commandeDocument.IdLivreDvd);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erreur lors de la création de la commande pour json={Champs}", commandeDocument.IdLivreDvd);
            }
            return false;
        }

        /// <summary>
        /// Modification de l'état de suivi d'une commande de document en base de données
        /// </summary>
        /// <param name="idCommande"></param>
        /// <param name="idSuivi"></param>
        /// <returns></returns>
        public bool ModifierSuiviCommande(string idCommande, string idSuivi)
        {
            // Créer un objet anonyme contenant uniquement le champ à modifier
            var champs = new { idSuivi };
            string jsonChamps = JsonConvert.SerializeObject(champs);
            string parametres = "champs=" + Uri.EscapeDataString(jsonChamps);

            try
            {
                List<Object> resultat = TraitementRecup<Object>(PUT, "commandedocument/" + idCommande, parametres);
                if (resultat != null)
                {
                    Log.Information("Suivi de la commande id={IdCommande} modifié avec succès", idCommande);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erreur lors de la modification du suivi de la commande {IdCommande} avec idSuivi={IdSuivi}", idCommande, idSuivi);

            }
            return false;
        }

        /// <summary>
        /// Suppression d'une commande
        /// </summary>
        /// <param name="idCommande"></param>
        /// <returns></returns>
        public bool SupprimerCommande(string idCommande)
        {
            string jsonIdCommande = convertToJson("id", idCommande);
            try
            {
                JObject retour = api.RecupDistant(DELETE, "commandedocument/" + jsonIdCommande, null);
                if (retour != null && (string)retour["code"] == "200")
                {
                    Log.Information("Commande id={IdCommande} supprimée avec succès", idCommande);
                    return true;
                }
                else
                {
                    Log.Error("Échec suppression commande id={IdCommande}", idCommande);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Erreur lors de la suppression de la commande id={IdCommande}", idCommande);
            }
            return false;
        }

        /// <summary>
        /// Création d'un abonnement de revue en base de données
        /// </summary>
        /// <param name="abonnement"></param>
        /// <returns></returns>
        public bool CreerAbonnementRevue(Abonnement abonnement)
        {
            string jsonAbonnement = JsonConvert.SerializeObject(abonnement, new CustomDateTimeConverter());
            try
            { 
                List<Abonnement> resultat = TraitementRecup<Abonnement>(POST, "abonnement", "champs=" + jsonAbonnement);
                if (resultat != null)
                {
                    Log.Information("Abonnement créé avec succès pour la revue id={IdRevue}", abonnement.IdRevue);
                    return true;
                }
                else
                {
                    Log.Error("Échec création abonnement pour la revue id={IdRevue}", abonnement.IdRevue);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erreur lors de la création de l'abonnement pour json={Champs}", jsonAbonnement);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'un abonnement de revue de la bdd
        /// </summary>
        /// <param name="idCommande"></param>
        /// <returns></returns>
        public bool SupprimerAbonnementRevue(string idCommande)
        {
            string jsonId = convertToJson("id", idCommande);
            try
            {
                JObject retour = api.RecupDistant(DELETE, "abonnement/" + jsonId, null);
                if (retour != null && (string)retour["code"] == "200")
                {
                    Log.Information("Abonnement supprimé avec succès pour idCommande={IdCommande}", idCommande);
                    return true;
                }
                else
                {
                    Log.Error("Échec suppression abonnement pour idCommande={IdCommande} code retour={Code}", idCommande, retour?["code"]);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erreur lors de la suppression de l'abonnement pour idCommande={IdCommande}", idCommande);
            }

            return false;
        }

        /// <summary>
        /// Modification de l'état d'un exemplaire d'un document en base de données
        /// </summary>
        /// <param name="idDocument"></param>
        /// <param name="numero"></param>
        /// <param name="idEtat"></param>
        /// <returns></returns>
        public bool ModifierEtatExemplaire(string idDocument, int numero, string idEtat)
        {
            var champs = new
            {
                numero,
                idEtat
            };

            string json = JsonConvert.SerializeObject(champs);
            string parametres = "champs=" + Uri.EscapeDataString(json);

            try
            {
                List<Object> liste = TraitementRecup<Object>(PUT, "exemplaire/" + idDocument, parametres);
                if (liste != null)
                {
                    Log.Information("État de l'exemplaire modifié avec succès pour le document id={IdDocument}, numéro={Numero}, idEtat={IdEtat}", idDocument, numero, idEtat);
                    return true;
                }
                else
                {
                    Log.Error("Échec modification état de l'exemplaire pour le document id={IdDocument}, numéro={Numero}, idEtat={IdEtat}", idDocument, numero, idEtat);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erreur lors de la modification de l'état de l'exemplaire pour le document id={IdDocument}, numéro={Numero}", idDocument, numero);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'un exemplaire d'un document de la bdd
        /// </summary>
        /// <param name="idExemplaire"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
        public bool SupprimerExemplaire(string idExemplaire, int numero)
        {
            string json = $"{{\"id\":\"{idExemplaire}\",\"numero\":{numero}}}";

            try
            {
                JObject retour =api.RecupDistant(DELETE, "exemplaire/" + json, null);

                if (retour != null && (string)retour["code"] == "200")
                {
                    Log.Information("Exemplaire supprimé avec succès : id={Id}, numéro={Numero}", idExemplaire, numero);
                    return true;
                }
                else
                {
                    Log.Error("Échec suppression de l'exemplaire : id={Id}, numéro={Numero}", idExemplaire, numero);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erreur lors de la suppression de l'exemplaire : id={Id}, numéro={Numero}", idExemplaire, numero);
            }
            return false;
        }

        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private static String convertToJson(Object nom, Object valeur)
        {
            var dictionary = new Dictionary<object, object>
            {
                { nom, valeur }
            };
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }
    }
}
