using System.Collections.ObjectModel;
using Windows.ApplicationModel.Resources;

namespace Project42
{
    public sealed partial class PointsOfInterest : PageBase
    {
        private ObservableCollection<PointOfInterestData> _Collection
        {
            get { return Collection; }
        }

        BlueToothBackgroundWorker BTWorker;

        public PointsOfInterest()
        {
            InitializeComponent();

            BTWorker = new BlueToothBackgroundWorker();


            PointOfInterestData DEMO_1 = new PointOfInterestData();

            PointOfInterestData DEMO_2 = new PointOfInterestData()
            {

            };

            Collection.Add(DEMO_1);
            Collection.Add(DEMO_2);
        }
    }
}
