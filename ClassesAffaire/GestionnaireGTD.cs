using System.Collections.ObjectModel;

namespace GTD
{
    public class GestionnaireGTD
    {
        public ObservableCollection<ElementGTD> ListeEntrees { get; private set; }
        public ObservableCollection<ElementGTD> ListeActions { get; private set; }
        public ObservableCollection<ElementGTD> ListeSuivis { get; private set; }

        public GestionnaireGTD()
        {
            ListeEntrees = new ObservableCollection<ElementGTD>();
            ListeActions = new ObservableCollection<ElementGTD>();
            ListeSuivis = new ObservableCollection<ElementGTD>();
        }
    }
}
