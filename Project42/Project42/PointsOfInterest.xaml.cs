using Microsoft.Toolkit.Uwp.Notifications;
using Project42;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Devices.Enumeration;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Notifications;
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

        MediaElement PlayMusic;
        BlueToothBackgroundWorker BTWorker;
        PointOfInterestData model;

        ToastContent content;
        
        /*private static string GetToastNotificationText(string key)
        {
            return ResourceLoader.GetString("Resources/Destination/ToastNotification").ToString();
        }*/

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

            model = PointOfInterestData.AssignFromResource("Karlstejn");

            //AddPoint(model);

            //AddPoint(PointOfInterestData.AssignFromResource("CharlesBridge"));

            content = new ToastContent()
            {
                Launch = "app-defined-string",

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = ResourceLoader.GetForViewIndependentUse("Resources").GetString("Destination/ToastNotification") //ResourceLoader.GetString("Destination/ToastNotification")
                                    },

                                    new AdaptiveText()
                                    {
                                        Text = model.Name
                                    }
                                },

                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = model.ImageUri,
                            AlternateText = "proc"
                        }
                    }
                },

                Actions = new ToastActionsCustom()
                {
                    Buttons =
                            {
                                new ToastButton("OK", "ok")
                                {

                                }
                            }
                },

                Audio = new ToastAudio()
                {
                    Src = new Uri("ms-winsoundevent:Notification.Reminder")
                }
            };

            BTWorker = new BlueToothBackgroundWorker();

        }

        private async void ContentControlHandler()
        {
            bool IsAvailable = BTWorker._device.FirstOrDefault(obj => BTWorker.Compare(obj, BTWorker.DEMO_DESTINATION_ONE)) != null ? true : false;
            
            if (!LV_Points.Items.Contains(model) && IsAvailable)
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        AddPoint(model);
                        ToastNotification notification = new ToastNotification(content.GetXml());
                        ToastNotificationManager.CreateToastNotifier().Show(notification);
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

            Selection = false;
        }

        private void ABB_Update_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => {
                BTWorker.Scan();
                ContentControlHandler();
            }); 
        }

        private async void PlaySound_Click(object sender, RoutedEventArgs e)
        {
            PlayMusic = new MediaElement();

            PlayMusic.AudioCategory = Windows.UI.Xaml.Media.AudioCategory.Speech;
            StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file = await folder.GetFileAsync(model.SoundUri);
            var stream = await file.OpenAsync(FileAccessMode.Read);
            PlayMusic.SetSource(stream, file.ContentType);
            PlayMusic.Play();
        }
    }
}
