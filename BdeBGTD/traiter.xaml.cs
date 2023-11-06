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
    /// Logique d'interaction pour traiter.xaml
    /// </summary>
    public partial class Traiter : Window
    {
        public Traiter()
        {
            InitializeComponent();

            
            AffichageCentrée();
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
