using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Project42
{
    public sealed partial class PointsOfInterest : PageBase
    {
        public static readonly DependencyProperty SelectionProperty = DependencyProperty.Register(nameof(Selection), typeof(bool), typeof(PointsOfInterest), null);

        private ObservableCollection<PointOfInterestData> _Collection
        {
            get { return Collection; }
        }
        private bool Selection
        {
            get { return (bool)GetValue(SelectionProperty); }
            set
            {
                if (Selection != value)
                {
                    if (!value)
                    {
                        LV_Points.SelectedItems.Clear();
                    }
                    
                    SetValue(SelectionProperty, value);
                }
            }
        }

        public PointsOfInterest()
        {
            InitializeComponent();
        }

        private void ToggleSelection(object sender, RoutedEventArgs e)
        {
            Selection = !Selection;
        }

        private async void DeleteSelection(object sender, RoutedEventArgs e)
        {
            foreach (PointOfInterestData point in LV_Points.SelectedItems.ToList())
            {
                DeletePoint(point);

                StorageFile pointFile = await (await GetFolderAsync()).TryGetItemAsync(point.FileName) as StorageFile;

                if (pointFile != null)
                {
                    await pointFile.DeleteAsync();
                }
            }
        }
    }
}
