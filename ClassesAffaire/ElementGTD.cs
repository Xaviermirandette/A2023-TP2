using ClassesAffaire;
using System.Globalization;
using System.Xml;

namespace GTD
{ 
    /**
     * la class ElementGTD est une classe d'objet qui détient un Nom, une Description (optionnel), Une date de rappel (Optionnel) et un statu
     * qui est soit une Entree, une action, un suivi ou Archive. les Elements sont récolté au moment de la lecture du fichier
     */
    public class ElementGTD :IConversionXML
    {
        public enum statuts
        {
            Entree,
            Action,
            Suivi,
            Archive
        }

        public string Nom { get; set; }
        public string Description { get; set; }
        public DateTime? DateDeRappel { get; set; }
        public statuts Statu { get; set; }
        

       

        public ElementGTD()
        {
        }
        public override string ToString()
        {
            return Nom;
        }
        public void DeXML(XmlElement elem)
        {
            Nom = elem.GetAttribute("nom");

            // Converti la chaîne en enum statuts
            string statutStr = elem.GetAttribute("statut");
            if (Enum.TryParse(statutStr, out statuts statut))
            {
                Statu = statut;
            }
            else
            {
                // Cas par défaut Archive
                Statu = statuts.Archive;
            }

            // Convertir la date du format "yyyy-MM-dd"
            string dateRappelStr = elem.GetAttribute("dateRappel");
            if (!string.IsNullOrEmpty(dateRappelStr) && DateTime.TryParseExact(dateRappelStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateRappel))
            {
                DateDeRappel = dateRappel;
            }
            else
            {
                // Si la date n'est pas présente, cette valeur devient null
                DateDeRappel = null;
            }

            XmlElement descriptionElement = elem["description"];
            Description = elem.InnerText.Trim();

            //Gère l'exception si la description est null
            if (Description == null)
            {
             
                //Si la Description est null elle contient une chaine de charactère vide
                Description = ""; 
            }
        }
        public XmlElement VersXML(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement("element_gtd");  

            element.SetAttribute("nom", Nom);
            element.SetAttribute("statut", Statu.ToString());

            if (DateDeRappel.HasValue)
            {
                element.SetAttribute("dateRappel", DateDeRappel.Value.ToString("yyyy-MM-dd"));
            }

            // Crée un élément pour la description et l'ajoute comme nœud enfant
            XmlElement descriptionElement = doc.CreateElement("description");
            descriptionElement.InnerText = Description;
            element.AppendChild(descriptionElement);

            return element;
        }
    }
}