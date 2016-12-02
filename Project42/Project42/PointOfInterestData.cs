using System;
using System.Threading.Tasks;
using UWPHelper.Utilities;
using Windows.UI.Xaml.Controls;

namespace Project42
{
    public sealed class PointOfInterestData : NotifyPropertyChangedBase
    {
        internal string FileName { get; set; }
        internal bool IsMoreOpened
        {
            get { return (bool)GetValue(nameof(IsMoreOpened)); }
            set { SetValue(nameof(IsMoreOpened), ref value); }
        }

        public string Name
        {
            get { return (string)GetValue(nameof(Name)); }
            set { SetValue(nameof(Name), ref value); }
        }
        public string Description
        {
            get { return (string)GetValue(nameof(Description)); }
            set { SetValue(nameof(Description), ref value); }
        }
        public Coordinate Latitude
        {
            get { return (Coordinate)GetValue(nameof(Latitude)); }
            set { SetValue(nameof(Coordinate), ref value); }
        }
        public Coordinate Longtitude
        {
            get { return (Coordinate)GetValue(nameof(Longtitude)); }
            set { SetValue(nameof(Longtitude), ref value); }
        }
        public Image ImagePreview
        {
            get { return (Image)GetValue(nameof(ImagePreview)); }
            set { SetValue(nameof(ImagePreview), ref value); }
        }
        public DateTime LastVisit
        {
            get { return (DateTime)GetValue(nameof(LastVisit)); }
            set { SetValue(nameof(LastVisit), ref value); }
        }

        public PointOfInterestData()
        {
            RegisterProperty(nameof(IsMoreOpened), typeof(bool), false);

            RegisterProperty(nameof(Name), typeof(string), "");
            RegisterProperty(nameof(Description), typeof(string), "");
            RegisterProperty(nameof(Latitude), typeof(Coordinate), new Coordinate());
            RegisterProperty(nameof(Longtitude), typeof(Coordinate), new Coordinate());
            RegisterProperty(nameof(ImagePreview), typeof(Image), null);
            RegisterProperty(nameof(LastVisit), typeof(DateTime), new DateTime());
        }

        public async Task SaveAsync()
        {
            await StorageFileHelper.SaveObjectAsync(this, FileName, await PointsOfInterest.GetFolderAsync());
        }
    }
}
