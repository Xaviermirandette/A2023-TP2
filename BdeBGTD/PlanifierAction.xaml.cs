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
using System.Xml.Linq;

namespace BdeBGTD
{
    /// <summary>
    /// Logique d'interaction pour PlanifierAction.xaml
    /// </summary> 
    public partial class PlanifierAction : Window 
    {
        //Annuler 
        public static RoutedCommand AnnulerCommand = new RoutedCommand(); 

      


        private GestionnaireGTD _gestionnaire;

        public PlanifierAction(GestionnaireGTD gestionnaire)
        {
            InitializeComponent();
            AffichageCentrée();
            _gestionnaire = gestionnaire;
            CommandBindings.Add(new CommandBinding(AnnulerCommand, AnnulerCommandExecuted, AnnulerCommandCanExecute));
           
        }


        //Planifier Action 
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
                        _gestionnaire.ListeEntrees[1].Statu = ElementGTD.statuts.Action;
                       
                        MessageBox.Show("La date de l'action " + _gestionnaire.ListeEntrees[1].Nom + " est maintenant " + _gestionnaire.ListeEntrees[1].DateDeRappel);
                        _gestionnaire.ListeActions.Add(_gestionnaire.ListeEntrees[1]);


                        // Décalez les indices des éléments restants vers le haut
                        for (int i = 1; i < (_gestionnaire.ListeEntrees.Count) - 1; i++)
                        {
                            // Mettre à jour l'élément actuel avec l'élément suivant
                            _gestionnaire.ListeEntrees[i] = _gestionnaire.ListeEntrees[i + 1];
                        }

                        //statut Archive
                      
                        _gestionnaire.ListeEntrees.RemoveAt(_gestionnaire.ListeEntrees.Count - 1); //la dernière case est vide alors on l'enlève
                        this.Close(); 
                        
                    }
                }
            }
        }
        private void AnnulerCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Ferme la fenêtre PlanifierAction
            this.Close();
        }
       

        private void AnnulerCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            
            e.CanExecute = true; 
        }
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
