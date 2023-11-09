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
using System.Diagnostics.Eventing.Reader;
using System.ComponentModel;

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

        //DateBouton
        public static RoutedCommand AugmenterDateCommand = new RoutedCommand();

        //variables utilisé pour le traitement de fichier
        private string _pathFichier; 
        private string _dossierBase; 
        private char DIR_SEPARATOR = '\\';
       

        //Variable utilisé pour la date
        public static string dateActuelle="";

        //gestionnaire
        private GestionnaireGTD _gestionnaire;

       
        public MainWindow()
        { 

            //Variable utilisé pour la gestion de fichier
            _dossierBase = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}{DIR_SEPARATOR}" +
                          $"Fichiers-3GP";
            _pathFichier = _dossierBase + DIR_SEPARATOR + "bdeb_gtd.xml";
      
            _gestionnaire = new GestionnaireGTD();

            
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(fermerAppCommand, CloseApp_Executed, CloseApp_CanExecute)); 
            CommandBindings.Add(new CommandBinding(AugmenterDateCommand,AugmenterDate_Executed, AugmenterDate_CanExecute));

            //listeEntree
            _listeEntrees.ItemsSource = _gestionnaire.ListeEntrees;
            _gestionnaire.ListeEntrees.Add(new ElementGTD());

            //listeAciton
            FiltrerEtAfficherActions();
            _gestionnaire.ListeActions.Add(new ElementGTD());

            //listeSuivi
            FilterSuivi();
            _listeSuivi.ItemsSource = _gestionnaire.ListeSuivis;
            _gestionnaire.ListeSuivis.Add(new ElementGTD());

            OuvrirFichier(); //on ouvre le fichier bdeb_gtd.xml
             dateActuelle = DateTime.Now.ToString("yyyy-MM-dd"); //on prend la date actuel
            dateTextBlock.Text = dateActuelle; 

            _listeAction.MouseDoubleClick += ListeAction_MouseDoubleClick; //utiliser pour afficher la fenêtre lorsqu'on clique sur une action

            Closing += MainWindow_Closing; //sauvegarde
        }

        //fin du programme et sauvegarde obligatoire

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            // lance la Sauvegarde
            SauvegarderGtd(_pathFichier);

          
        }
        private void SauvegarderGtd(string nomFichier)
        {
            XmlDocument doc = new XmlDocument();

            // Crée l'élément racine
            XmlElement racine = doc.CreateElement("gtd");
            doc.AppendChild(racine);

            List<ElementGTD> listeEntrees = _gestionnaire.ListeEntrees.ToList();
            List<ElementGTD> listeActions = _gestionnaire.ListeActions.ToList();
            List<ElementGTD> listeSuivis = _gestionnaire.ListeSuivis.ToList();
            // Sauvegarde les éléments de ListeEntrees
            SauvegarderElements(listeEntrees, "entrees", doc, racine);

            // Sauvegarde les éléments de ListeActions
            SauvegarderElements(listeActions, "actions", doc, racine);

            // Sauvegarde les éléments de ListeSuivis
            SauvegarderElements(listeSuivis, "suivis", doc, racine);

            // Enregistre le document XML dans le fichier
            doc.Save(nomFichier); 

        }

        private void SauvegarderElements(List<ElementGTD> elements, string balise, XmlDocument doc, XmlElement parent)
        {
            foreach (ElementGTD element in elements)
            {
                // Créez un élément XML pour chaque élément de la liste
                XmlElement elem = element.VersXML(doc);
                elem.SetAttribute("statut", balise);
                parent.AppendChild(elem);
            }
        }

        private void Sauvegarder_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SauvegarderGtd(_pathFichier);
            Close();
        }



        //est le filtre de suivi qui trace la date en fonciton de la date avancé
        private void FilterSuivi()
        {
            DateTime dateActuelleObj;
            ElementGTD element;
            ElementGTD suivi;
            int indiceAChange = 1;
            if (DateTime.TryParse(dateActuelle, out dateActuelleObj))
            {
                string formattedDate = dateActuelleObj.ToString("yyyy-MM-dd");

                // Boucle sur les éléments de ListeSuivis
                for (int x=1; x<_gestionnaire.ListeSuivis.Count-1; x++)
                {
                  
                    suivi = _gestionnaire.ListeSuivis[x];
                    if (suivi.DateDeRappel.Equals(formattedDate)) // Vérifie si la date correspond à la date actuelle
                    {

                  
                        //dans cette boucle on recherche l'element avec le même nom pour trouver sa position
                        for ( int j=1; j<_gestionnaire.ListeSuivis.Count-1; j++)
                        {
                   
                            if (_gestionnaire.ListeSuivis[j].Nom == suivi.Nom)
                            {
                                indiceAChange = j; 

                            }
                        }

                        _gestionnaire.ListeSuivis[indiceAChange].Statu = ElementGTD.statuts.Entree;
                        element = _gestionnaire.ListeSuivis[indiceAChange];
                        _gestionnaire.ListeEntrees.Add(element);


                        // on écrase la donnée en décalant les données sur elle
                        for (int i = indiceAChange; i < (_gestionnaire.ListeSuivis.Count) - 1; i++)
                        {
                            // Mettre à jour l'élément actuel avec l'élément suivant
                            _gestionnaire.ListeSuivis[i] = _gestionnaire.ListeEntrees[i + 1];
                        }
                        _gestionnaire.ListeSuivis.RemoveAt(_gestionnaire.ListeSuivis.Count - 1); //on retire la dernière donnée
                    }
                    
                } 
                
                _listeSuivi.ItemsSource = _gestionnaire.ListeSuivis;
            }
        }

        //Boutton + sert à augmenter la date actuel à 1 pour les besoins de la simulation
        private void AugmenterDate_Executed(object sender, ExecutedRoutedEventArgs e)
        {
             dateActuelle = dateTextBlock.Text;

            if (DateTime.TryParse(dateActuelle, out DateTime date))
            {
                date = date.AddDays(1);
                dateActuelle = date.ToString("yyyy-MM-dd"); //on réassigne la date à date actuel
                dateTextBlock.Text = dateActuelle;
                FiltrerEtAfficherActions();
                FilterSuivi();
            }
        }

        private void AugmenterDate_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; 
        }

        //filtre et affiche liste Action si la date est inférieur ou égale à la date actuel
        private void FiltrerEtAfficherActions()
        {
           
            DateTime dateActuelleObj;

            if (DateTime.TryParse(dateActuelle, out dateActuelleObj))
            {
                var actionsAFaire = new List<ElementGTD>();

                //on boucle sur chacun des elements de ListeActions
                foreach (var action in _gestionnaire.ListeActions)
                {
                    if (action.DateDeRappel == null || action.DateDeRappel <= dateActuelleObj)
                    {
                        actionsAFaire.Add(action);
                    }
                }

               
            }





        }

        //ListeAction
        private void ListeAction_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Vérifie si un élément a été sélectionné
            if (_listeAction.SelectedItem is ElementGTD elementSelection)
            {
                // Crée la fenêtre ListeAction en lui passant l'élément sélectionné
                ListeAction listeActionWindow = new ListeAction(elementSelection, _gestionnaire);

                // Affiche la fenêtre ListeAction
                listeActionWindow.Show();
                
            }
        }

        //Traiter
        private void AfficherTraitement_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // empeche l'ouverture de traiter
            if (_gestionnaire.ListeEntrees.Count < 2)
            {
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
            // Affiche la boîte de dialogue "À propos" 
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
