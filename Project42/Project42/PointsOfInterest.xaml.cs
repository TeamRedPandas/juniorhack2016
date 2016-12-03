using Project42;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Devices.Enumeration;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Project42
{
    public sealed partial class PointsOfInterest : PageBase
    {
        public static readonly DependencyProperty SelectionProperty = DependencyProperty.Register(nameof(Selection), typeof(bool), typeof(PointsOfInterest), null);
        
        private ObservableCollection<PointOfInterestData> _Collection
        {
            get { return Collection; }
        }
        
        BlueToothBackgroundWorker BTWorker;

        PointOfInterestData model;

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

            model = PointOfInterestData.AssignFromResource("CharlesBridge");
            
            //AddPoint(model);

            //AddPoint(PointOfInterestData.AssignFromResource("CharlesBridge"));

            BTWorker = new BlueToothBackgroundWorker();
            BTWorker.UpdateUI = ContentControlHandler;
        }

        private async void ContentControlHandler()
        {
            bool IsAvailable = BTWorker._device.FirstOrDefault(obj => BTWorker.Compare(obj, BTWorker.DEMO_DESTINATION_ONE)) != null ? true : false;
            
            if (!LV_Points.Items.Contains(model) && IsAvailable)
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        AddPoint(model);
                    });
            if (LV_Points.Items.Contains(model) && !IsAvailable)
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        DeletePoint(model);
                    });
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
