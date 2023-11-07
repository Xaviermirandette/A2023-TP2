﻿using GTD;
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
    /// Logique d'interaction pour traiter.xaml
    /// </summary>
    public partial class Traiter : Window
    { 

        //Retour 
        public static RoutedCommand traiterFermer = new RoutedCommand();

        //Poubelle 
        public static RoutedCommand PoubelleCommand = new RoutedCommand();

        //Action Rapide 
        public static RoutedCommand ActionRapide=new RoutedCommand(); 

        //Planifier Action 
        public static RoutedCommand planifierAction = new RoutedCommand(); 

        //Incuber
        public static RoutedCommand incuber = new RoutedCommand();

        private GestionnaireGTD _gestionnaire;
        public Traiter(GestionnaireGTD gestionnaire)
        {
            _gestionnaire = gestionnaire;

            InitializeComponent();
            if (_gestionnaire.ListeEntrees.Count>1 ) { 
            AffichageCentrée();

            AffichageNomDesc();
            }

            CommandBindings.Add(new CommandBinding(traiterFermer, TraiterFermerExecuted, TraiterFermerCanExecute));

            CommandBindings.Add(new CommandBinding(PoubelleCommand, PoubelleCommandExecuted, PoubelleCommandCanExecute)); 

            CommandBindings.Add(new CommandBinding(ActionRapide,ActionRapideCommandExecuted, ActionRapideCommandCanExecute)); 

            CommandBindings.Add(new CommandBinding(planifierAction,PlanifierActionCommandExecuted, PlanifierActionCommandCanExecute)); 

            CommandBindings.Add(new CommandBinding(incuber,IncuberActionCommandExecuted, IncuberActionCommandCanExecute));


        }

        private void AffichageNomDesc()
        {
            traiterNom.Items.Add(_gestionnaire.ListeEntrees[1].Nom);
            traiterDescription.Items.Add(_gestionnaire.ListeEntrees[1].Description);
        }

        //Incuber
        private void IncuberActionCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Incuber incuberWindow = new Incuber();
            incuberWindow.ShowDialog(); //affiche Incuber
        }

        private void IncuberActionCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        } 

        //Planifier Action
        private void PlanifierActionCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            PlanifierAction planifierActionWindow = new PlanifierAction();
            planifierActionWindow.ShowDialog(); //afiche PlanAction
        }

        private void PlanifierActionCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; 
        }





        //Action Rapide
        private void ActionRapideCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Vérifie si la liste contient au moins un élément
            if (_gestionnaire.ListeEntrees.Count > 1)
            {
                ElementGTD element = _gestionnaire.ListeEntrees[1]; // Récupérez le premier élément

                // Décalez les indices des éléments restants vers le haut
                for (int i = 1; i < (_gestionnaire.ListeEntrees.Count) - 1; i++)
                {
                    // Mettre à jour l'élément actuel avec l'élément suivant
                    _gestionnaire.ListeEntrees[i] = _gestionnaire.ListeEntrees[i + 1];
                }

                //statut Archive
                element.Statu = ElementGTD.statuts.Archive;
                _gestionnaire.ListeEntrees.RemoveAt(_gestionnaire.ListeEntrees.Count-1);
                Close(); //on ferme la page pour rafraichir la page
              
            }
        }
       

        private void ActionRapideCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Autorise la commande uniquement si la liste contient au moins un élément
            e.CanExecute = _gestionnaire.ListeEntrees.Count > 1;
        } 

        //Poubelle
        private void PoubelleCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Vérifie si un élément est sélectionné
            if (_gestionnaire.ListeEntrees.Count > 1)
            {
                int indexASupprimer = 1; // Indice de l'élément à supprimer
                _gestionnaire.ListeEntrees.RemoveAt(indexASupprimer); // Supprime l'élément spécifié

                // Décale les indices des éléments restants
                for (int i = indexASupprimer; i < (_gestionnaire.ListeEntrees.Count) -1; i++)
                {
                    // Mettre à jour l'élément suivant avec l'élément précédent
                    _gestionnaire.ListeEntrees[i] = _gestionnaire.ListeEntrees[i + 1];
                }
                Close(); //on ferme la fenêtre pour raffraichir la page
                
            }
        }

        private void PoubelleCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Autorise la commande uniquement si un élément est sélectionné
            e.CanExecute = _gestionnaire.ListeEntrees.Count > 1;
        }


        //Retour
        private void TraiterFermerExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close(); // Fermez la fenêtre actuelle
        }

        private void TraiterFermerCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; // Vous pouvez ajouter une logique de condition ici si nécessaire
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