using GTD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClassesAffaire;
using System.Collections.ObjectModel;

namespace BdeBGTD
{
    /// <summary>
    /// Logique d'interaction pour AjoutéEntrées.xaml
    /// </summary> 
    /// 

   
    public partial class AjoutéEntrées : Window
    {
        //Anuler
        public static RoutedCommand fermerActionEntrées = new RoutedCommand();

        //Confirmer
        public static RoutedCommand confirmerActionEntrées = new RoutedCommand();


     
        private GestionnaireGTD _gestionnaire;
        

        public AjoutéEntrées(GestionnaireGTD gestionnaire)
        {
            InitializeComponent();
            
            _gestionnaire = gestionnaire;
          
            CommandBindings.Add(new CommandBinding(fermerActionEntrées, fermerActionEntrées_Executed, fermerActionEntrées_CanExecute));

            AffichageCentrée();
        }

       

        private void confirmerActionEntrées_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Ajoutez l'élément avec le nom et la description à la liste ListeEntrees
            string nom = saisieNom.Text;
            string description = saisieDescription.Text;
            if (!string.IsNullOrEmpty(nom))
            {
                ElementGTD nouvelElement = new ElementGTD
                {
                    Nom = nom,
                    Description = description,
                    Statu = ElementGTD.statuts.Entree
                };
                _gestionnaire.ListeEntrees.Add(nouvelElement);
            }

            // Fermez la fenêtre actuelle 
            if (checkBoxGarderOuverte.IsChecked == false)
            {
                Close();
            }
        }

        private void confirmerActionEntrées_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; 
        }


        //Cette algorithme permet à la fenêtre de toujours s'afficher au centre de la page mère
        private void AffichageCentrée()
        {
            
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            double left = mainWindow.Left + (mainWindow.Width - Width) / 2;
            double top = mainWindow.Top + (mainWindow.Height - Height) / 2;
            Left = left;
            Top = top;
        }
        private void checkBoxGarderOuverte_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void checkBoxGarderOuverte_Unchecked(object sender, RoutedEventArgs e)
        {
            // Empêche la fermeture de la fenêtre lorsque la case est décochée
            CommandBindings.Remove(new CommandBinding(ApplicationCommands.Close, fermerActionEntrées_Executed, fermerActionEntrées_CanExecute));
        }


        private void fermerActionEntrées_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (checkBoxGarderOuverte.IsChecked == true)
            {
                // Si la case est cochée elle rouvre une nouvelle fenêtre anulant ainsi ce que l'on a écrit à l'intérieur
                AjoutéEntrées nouvelleFenetre = new AjoutéEntrées(_gestionnaire);
                nouvelleFenetre.checkBoxGarderOuverte.IsChecked = true;
                nouvelleFenetre.Show();
                
            }

            // Fermez la fenêtre actuelle
            Close();
        }
        private void fermerActionEntrées_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
