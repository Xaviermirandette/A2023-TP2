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
        //Quitter
        public static RoutedCommand fermerAppCommand = new RoutedCommand();

        //Ajouter entré
        public static RoutedCommand AjouterEntreesCommand = new RoutedCommand();

        //Traiter 
        public static RoutedCommand AfficherTraitementCommand = new RoutedCommand();

        //variables utilisé pour le traitement de fichier
        private string _pathFichier; 
        private string _dossierBase; 
        private char DIR_SEPARATOR = '\\';
       
        private GestionnaireGTD _gestionnaire;

       
        public MainWindow()
        {
            _dossierBase = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}{DIR_SEPARATOR}" +
                          $"Fichiers-3GP";
            _pathFichier = _dossierBase + DIR_SEPARATOR + "bdeb_gtd.xml";
            _gestionnaire = new GestionnaireGTD();

            
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(fermerAppCommand, CloseApp_Executed, CloseApp_CanExecute));

            _listeEntrees.ItemsSource = _gestionnaire.ListeEntrees;
            _gestionnaire.ListeEntrees.Add(new ElementGTD());

            _listeAction.ItemsSource = _gestionnaire.ListeActions; 
            _gestionnaire.ListeActions.Add(new ElementGTD());

            _listeSuivi.ItemsSource = _gestionnaire.ListeSuivis;
            _gestionnaire.ListeSuivis.Add(new ElementGTD());

            OuvrirFichier();
            dateTextBlock.Text = DateTime.Now.ToString("yyyy-MM-dd"); 

        }

        //Traiter
        private void AfficherTraitement_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //si aucun élément n'est détecter dans listeEntrees un message d'erreur apparait
            if (_gestionnaire.ListeEntrees.Count < 2)
            {
                MessageBox.Show("Vous ne pouvez pas ouvrir la fenêtre Traiter avec moins de 2 éléments dans ListeEntrees.", "Erreur d'ouverture", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            { 
                //sinon traiter s'ouvre normalement
                Traiter traiteWindow = new Traiter(_gestionnaire);
                traiteWindow.Show();
            } 
        }
        private void AfficherTraitement_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(_gestionnaire.ListeEntrees.Count <2)
            {
                e.CanExecute = false;
            }
            else{ 

            e.CanExecute = true;
            }
        }
        //AjouterEntrees
        private void AjouterEntrees_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            AjoutéEntrées deuxiemePage = new AjoutéEntrées(_gestionnaire);
            deuxiemePage.Show();
        }

        private void AjouterEntrees_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
           
            e.CanExecute = true; 
        }

        //Quitter
        private void CloseApp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
       
        private void CloseApp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void MenuItemAPropos_Click(object sender, RoutedEventArgs e)
        {
            // Afficher la boîte de dialogue "À propos" avec votre message
            MessageBox.Show("BdeB GTD\nVersion 1.0\nAuteur: Xavier Mirandette", "À propos" );
        }

        //Ouvrir le Fichier
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
        /*
         * La méthode ChargerGtd permet lors de la lecture du fichier xml d'insérer les éléments lu dans les liste approprié en fonction
         * de leur statut. Deplus il génère un nouvel objet Élément en fonction toujours des éléments lu dans le fichier.
         */
        private void ChargerGtd(string nomFichier)
        {
            if (!File.Exists(nomFichier))
            { 
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(nomFichier);
            XmlNodeList gtds = doc.DocumentElement.GetElementsByTagName("element_gtd");

            foreach (XmlElement gtd in gtds)
            {
                ElementGTD element = new ElementGTD();
                element.DeXML(gtd); // Remplit l'objet ElementGTD depuis le XML

                // En fonction du statut de l'élément on place l'ElementGTD dans la bonne liste où ce dernier corespond
                switch (element.Statu)
                {
                    case ElementGTD.statuts.Entree: //dans la liste Entree

                        _gestionnaire.ListeEntrees.Add(element);
                        break;
                    case ElementGTD.statuts.Action: //dans la liste Action

                        _gestionnaire.ListeActions.Add(element);
                        break;
                    case ElementGTD.statuts.Suivi: //dans la liste Suivi

                        _gestionnaire.ListeSuivis.Add(element);
                        break;

                    //si un statuts est différent exemple Archive on ne le traite pas
                    default: 

                        break;
                }
            }
        }
    }
   

}
