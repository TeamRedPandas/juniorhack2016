using System.Collections.ObjectModel;

namespace Project42
{
    public sealed partial class PointsOfInterest : PageBase
    {
        private ObservableCollection<PointOfInterestData> _Collection
        {
            get { return Collection; }
        }

        public PointsOfInterest()
        {
            InitializeComponent();
        }
    }
}
