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

namespace BdeBGTD
{
    /// <summary>
    /// Logique d'interaction pour ListeAction.xaml
    /// </summary>
    public partial class ListeAction : Window 
    {
        //Annuler
        public static RoutedCommand Annuler = new RoutedCommand();

        //Terminer Action 
        public static RoutedCommand TerminerAction = new RoutedCommand();

        //Gestionnaire
        GestionnaireGTD _gestionnaire;

        //variable utiliser pour terminerAction
        private ElementGTD _element;

        /**
         * Est la fenêtre ListeAction cette dernière s'ouvre en double cliquant sur n'importe quel action afficher dans le MainWindow. 
         * ListAction permet de gérer une action précise et précédement sélectionner dans listAction afin de l'archiver.
         */
        public ListeAction(ElementGTD element, GestionnaireGTD gestionnaire)
        {
            InitializeComponent();
            AffichageCentrée(); //pour afficher au centre
            _gestionnaire = gestionnaire;
            _element = element;
            AffichageNomDesc(); //affiche le nom et la description du premier élément de la listeEntre
            
            CommandBindings.Add(new CommandBinding(Annuler, Annuler_Executed, Annuler_CanExecute));
            CommandBindings.Add(new CommandBinding(TerminerAction, TerminerAction_Executed, TerminerAction_CanExecute));
        }

        //Terminer Action
        private void TerminerAction_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
      

            //on recherche l'emplacement de la donnée sélectionner
            int emplacementDonnée = 0;
            for(int j =1; j< _gestionnaire.ListeActions.Count -1; j++)
            {
                if (_gestionnaire.ListeActions[j].Nom == _element.Nom)
                {
                    emplacementDonnée = j;
                }
            }

            //on décale tous les éléments depuis la position de la donnée qui écrasera les données de sont précédents n'effacant que la donnée à la position emplacementDonnée
            for (int i = emplacementDonnée; i < (_gestionnaire.ListeActions.Count) - 1; i++)
            {
                // Mettre à jour l'élément suivant avec l'élément précédent 
                if (i != 0)
                {
                    _gestionnaire.ListeActions[i] = _gestionnaire.ListeActions[i + 1];
                }
            }
            _gestionnaire.ListeActions[emplacementDonnée].Statu = ElementGTD.statuts.Archive;
            _gestionnaire.ListeActions.RemoveAt(_gestionnaire.ListeActions.Count - 1); //on retire la copie de surplus
           // Ferme la fenêtre ListeAction
            this.Close();
        }

      
        private void TerminerAction_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            
            e.CanExecute = true;
        }


        //Annuler 
        private void Annuler_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Ferme la fenêtre ListeAction
            this.Close();
        }

        private void Annuler_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        //Affiche nom et description de l'element initialement
        private void AffichageNomDesc()
        {
            
            saisieNom.Text = _element.Nom.ToString(); //Nom
            saisieDescription.Text = _element.Description.ToString(); //Description
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

    }
}
