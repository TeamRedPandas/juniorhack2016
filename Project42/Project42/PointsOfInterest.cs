using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using UWPHelper.Utilities;
using Windows.Storage;

namespace Project42
{
    public sealed partial class PointsOfInterest
    {
        private const string FOLDER_NAME = "VisitedPoints";
        
        private static StorageFolder _folder;

        public static ObservableCollection<PointOfInterestData> Collection { get; private set; }
        public static bool ShowLoadingError { get; set; }

        public static async Task<StorageFolder> GetFolderAsync()
        {
            if (_folder == null)
            {
                _folder = await ApplicationData.Current.LocalFolder.TryGetItemAsync(FOLDER_NAME) as StorageFolder ?? await ApplicationData.Current.LocalFolder.CreateFolderAsync(FOLDER_NAME);
            }

            return _folder;
        }

        public static void AddPoint(PointOfInterestData pointOfInterestData)
        {
            Collection.Add(pointOfInterestData);
            pointOfInterestData.PropertyChanged += PointOfInterestDataPropertyChangedEventHandler;
        }

        public static void DeletePoint(PointOfInterestData pointOfInterestData)
        {
            pointOfInterestData.PropertyChanged -= PointOfInterestDataPropertyChangedEventHandler;
            Collection.Remove(pointOfInterestData);
        }

        public static async Task SaveAsync()
        {
            foreach (PointOfInterestData pointOfInterestData in Collection)
            {
                await pointOfInterestData.SaveAsync();
            }
        }

        public static async Task LoadAsync()
        {
            await Task.CompletedTask;
            Collection = new ObservableCollection<PointOfInterestData>();

            IReadOnlyList<StorageFile> pointFiles = await (await GetFolderAsync()).GetFilesAsync();

            if (pointFiles != null && pointFiles.Count != 0)
            {
                foreach (StorageFile pointFile in pointFiles)
                {
                    var loadObjectAsyncResult = await StorageFileHelper.LoadObjectAsync<PointOfInterestData>(pointFile);
                    loadObjectAsyncResult.Object.FileName = pointFile.Name;
                    
                    AddPoint(loadObjectAsyncResult.Object);

                    if (!ShowLoadingError)
                    {
                        ShowLoadingError = !loadObjectAsyncResult.Success;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    PointOfInterestData p = new PointOfInterestData();
                    p.Name = "Point";
                    p.Description = "The Eiffel Tower (/ˈaɪfəl ˈtaʊər/ EYE-fəl TOWR; French: Tour Eiffel, pronounced: [tuʁ‿ɛfɛl]  listen) is a wrought iron lattice tower on the Champ de Mars in Paris, France. It is named after the engineer Gustave Eiffel, whose company designed and built the tower.";
                    p.LastVisit = DateTime.Now;

                    p.Latitude.Degrees = 53;
                    p.Latitude.Minutes = 13;
                    p.Latitude.Seconds = 156.43f;

                    p.Longtitude.Degrees = 45;
                    p.Longtitude.Minutes = 82;
                    p.Longtitude.Seconds = 645.83f;
                    p.Longtitude.IsPositive = false;


                    p.FileName = i.ToString() + ".json";

                    AddPoint(p);
                }
            }
        }

        private static async void PointOfInterestDataPropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            await ((PointOfInterestData)sender).SaveAsync();
        }
    }
}
