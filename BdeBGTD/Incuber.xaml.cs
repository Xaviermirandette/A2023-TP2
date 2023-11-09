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
    /// Logique d'interaction pour Incuber.xaml
    /// </summary>
    public partial class Incuber : Window
    {
        //Annuler
        public static RoutedCommand AnnulerCommand = new RoutedCommand(); 

        //Gestionnaire
        public GestionnaireGTD _gestionnaire;

        /**
         * est la fenêtre Incuber cette dernière s'ouvre à partir de la fenêtre Traiter lorsque l'utilisateur clique sur le bouton Incuber. 
         * Cette fenêtre offre la possibilité de changer la date de rappel d'un élément dans la listeEntré et de le déplacer dans listeSuivi
         */
        public Incuber(GestionnaireGTD gestionnaire) 
        {
            _gestionnaire = gestionnaire;
            InitializeComponent();
            AffichageCentrée(); //affiche la fenêtre centré
            CommandBindings.Add(new CommandBinding(AnnulerCommand, AnnulerCommandExecuted, AnnulerCommandCanExecute));
        }

        //Incuber
        private void CalendrierInteraction(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) // on doit cliquer deux fois sur la date pour changer l'élément
            {
                if (sender is Calendar calendar)
                {
                    DateTime? dateSelection = calendar.SelectedDate;
                    if (dateSelection.HasValue)
                    {
                        _gestionnaire.ListeEntrees[1].DateDeRappel = dateSelection.Value;
                        _gestionnaire.ListeEntrees[1].Statu = ElementGTD.statuts.Suivi;
                        MessageBox.Show("La date de Suivi de l'élément est" + _gestionnaire.ListeEntrees[1].Nom + " est maintenant " + _gestionnaire.ListeEntrees[1].DateDeRappel);
                        _gestionnaire.ListeSuivis.Add(_gestionnaire.ListeEntrees[1]);


                        // Décalez les indices des éléments restants vers le haut
                        for (int i = 1; i < (_gestionnaire.ListeEntrees.Count) - 1; i++)
                        {
                            // Mettre à jour l'élément actuel avec l'élément suivant
                            _gestionnaire.ListeEntrees[i] = _gestionnaire.ListeEntrees[i + 1];
                        }

                        //statut Archive

                        _gestionnaire.ListeEntrees.RemoveAt(_gestionnaire.ListeEntrees.Count - 1); //la dernière case est vide alors on l'enlève
                        Close();

                    }
                }
            }
        }

        //Annuler
        private void AnnulerCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Ferme la fenêtre Incuber
            this.Close();
        }

        private void AnnulerCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            
            e.CanExecute = true; 
        }

        //Permet d'afficher la fenêtre au centre
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
