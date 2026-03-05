using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Linq;
using System.Data.SqlTypes;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi = "http://localhost/rest_mediatekdocuments/";
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour update
        /// </summary>>
        private const string PUT = "PUT";
        /// <summary>
        /// méthode HTTP pour delete
        /// </summary>
        private const string DELETE = "DELETE";

        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            String authenticationString;
            try
            {
                authenticationString = "admin:adminpwd";
                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if(instance == null)
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
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre", null);
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
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
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument, null);
            return lesExemplaires;
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
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire", "champs=" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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

            // IMPORTANT ⭐ Encodage URL du JSON
            string parametres = "champs=" + Uri.EscapeDataString(jsonElement);

            List<Object> liste = TraitementRecup<Object>(
                POST,
                typeElement,
                parametres
            );

            return liste != null;
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

            List<Object> liste = TraitementRecup<Object>(
                PUT,
                typeElement + "/" + id,
                parametres
            );

            return liste != null;
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

            List<Object> liste = TraitementRecup<Object>(
                DELETE,
                typeElement + "/" + jsonIdElement,
                null
            );

            return liste != null;
        }

        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée dans l'url</param>
        /// <param name="parametres">paramètres à envoyer dans le body, au format "chp1=val1&chp2=val2&..."</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T> (String methode, String message, String parametres)
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
                        Console.WriteLine(resultString);
                    }
                }
                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                }
            }catch(Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : "+e.Message);
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
            String jsonCommande = JsonConvert.SerializeObject(
                commandeDocument,
                new CustomDateTimeConverter()
            );

            List<CommandeDocument> resultat =
                TraitementRecup<CommandeDocument>(
                    POST,
                    "commandedocument",
                    "champs=" + jsonCommande
                );

            return resultat != null;
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
            var champs = new
            {
                idSuivi = idSuivi
            };

            string jsonChamps = JsonConvert.SerializeObject(champs);
            string parametres = "champs=" + Uri.EscapeDataString(jsonChamps);

            List<Object> resultat = TraitementRecup<Object>(
                PUT,
                "commandedocument/" + idCommande,
                parametres
            );

            return resultat != null;
        }

        /// <summary>
        /// Suppression d'une commande
        /// </summary>
        public bool SupprimerCommande(string idCommande)
        {
            string jsonIdCommande = convertToJson("id", idCommande);

            JObject retour =
                api.RecupDistant(
                    DELETE,
                    "commandedocument/" + jsonIdCommande,
                    null
                );

            if (retour == null)
                return false;

            string code = (string)retour["code"];

            return code == "200";
        }

        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private String convertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
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
