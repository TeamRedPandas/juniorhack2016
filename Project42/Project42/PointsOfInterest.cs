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

        public static async void AddPoint(PointOfInterestData pointOfInterestData)
        {
            await pointOfInterestData.SaveAsync();
            Collection.Add(pointOfInterestData);
            pointOfInterestData.PropertyChanged += PointOfInterestDataPropertyChangedEventHandler;
        }

        public static async void DeletePoint(PointOfInterestData pointOfInterestData)
        {
            pointOfInterestData.PropertyChanged -= PointOfInterestDataPropertyChangedEventHandler;
            Collection.Remove(pointOfInterestData);

            StorageFile pointFile = await (await GetFolderAsync()).TryGetItemAsync(pointOfInterestData.FileName) as StorageFile;

            if (pointFile != null)
            {
                await pointFile.DeleteAsync();
            }
        }

        public static async Task SaveAsync()
        {
            foreach (PointOfInterestData pointOfInterestData in Collection)
            {
                if (pointOfInterestData.FileName != "CharlesBridge.json")
                {
                    await pointOfInterestData.SaveAsync();
                }
            }
        }

        public static async Task LoadAsync()
        {
            await Task.CompletedTask;
            Collection = new ObservableCollection<PointOfInterestData>();
            Collection.Add(PointOfInterestData.AssignFromResource("CharlesBridge"));

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
        }

        private static async void PointOfInterestDataPropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            await ((PointOfInterestData)sender).SaveAsync();
        }
    }
}
