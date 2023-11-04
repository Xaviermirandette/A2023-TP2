using GTD;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using ClassesAffaire;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml; 

namespace BdeBGTD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 
    /// 

    public partial class MainWindow : Window
    {

        //private CollectionGtd _lesContacts;
        private string _pathFichier;
        private string _dossierBase;
        private char DIR_SEPARATOR = '\\';
       
        private GestionnaireGTD _gestionnaire;

        //private TextBlock dateTextBlock; // Champ pour le TextBlock de la date
        public MainWindow()
        {
            
            _dossierBase = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}{DIR_SEPARATOR}" +
                          $"Fichiers-3GP";
            _pathFichier = _dossierBase + DIR_SEPARATOR + "bdeb_gtd.xml";

            OuvrirFichier();

            _gestionnaire = new GestionnaireGTD();

            InitializeComponent();

            _listeEntrees.ItemsSource = _gestionnaire.ListeEntrees;
            _gestionnaire.ListeEntrees.Add(new ElementGTD());

            _listeAction.ItemsSource = _gestionnaire.ListeActions; 
            _gestionnaire.ListeActions.Add(new ElementGTD());

            _listeSuivi.ItemsSource = _gestionnaire.ListeSuivis;
            _gestionnaire.ListeSuivis.Add(new ElementGTD());
            
            dateTextBlock.Text = DateTime.Now.ToString("yyyy-MM-dd"); //si cette ligne bug mettre en commentaire et runner le programme après décommenter la et relancer

        }
        private void MenuItemAPropos_Click(object sender, RoutedEventArgs e)
        {
            // Afficher la boîte de dialogue "À propos" avec votre message
            MessageBox.Show("BdeB GTD\nVersion 1.0\nAuteur: Xavier Mirandette", "À propos" );
        }
        private void OuvrirFichier()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "xml files (*.xml)|*.xml";
            openFileDialog.InitialDirectory = _dossierBase;
            bool? resultat = openFileDialog.ShowDialog();

            if (resultat.HasValue && resultat.Value)
            {
                _pathFichier = openFileDialog.FileName;
                ChargerGtd(_pathFichier);
            }
        }
        private void ChargerGtd(string nomFichier)
        {
            if (!File.Exists(nomFichier))
            {
                Debug.WriteLine("Le fichier n'existe pas : " + nomFichier);
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(nomFichier);
            XmlNodeList gtds = doc.DocumentElement.GetElementsByTagName("element_gtd");

            foreach (XmlElement gtd in gtds)
            {
                ElementGTD element = new ElementGTD();
                element.DeXML(gtd); // Remplit l'objet ElementGTD depuis le XML

                Debug.WriteLine("Élément chargé : " + element.Nom); // Ajoutez un message pour le débogage 

                // En fonction du statut de l'élément on place l'ElementGTD dans la bonne liste
                switch (element.Statu)
                {
                    case ElementGTD.statuts.Entree:
                        _gestionnaire.ListeEntrees.Add(element);
                        break;
                    case ElementGTD.statuts.Action:
                        _gestionnaire.ListeActions.Add(element);
                        break;
                    case ElementGTD.statuts.Suivi:
                        _gestionnaire.ListeSuivis.Add(element);
                        break;
                    default: //si un statuts est différent exemple Archive on ne le traite pas
                        break;
                }
            }
        }
    }
   

}
