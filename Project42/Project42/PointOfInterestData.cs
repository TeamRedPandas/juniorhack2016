using System;
using System.Threading.Tasks;
using UWPHelper.Utilities;
using Windows.ApplicationModel.Resources;

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
        public string ImageUri
        {
            get { return (string)GetValue(nameof(ImageUri)); }
            set { SetValue(nameof(ImageUri), ref value); }
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
            RegisterProperty(nameof(ImageUri), typeof(string), null);
            RegisterProperty(nameof(LastVisit), typeof(DateTime), new DateTime());
        }

        public async Task SaveAsync()
        {
            await StorageFileHelper.SaveObjectAsync(this, FileName, await PointsOfInterest.GetFolderAsync());
        }

        public static PointOfInterestData AssignFromResource(string name)
        {
            ResourceLoader resourceLoader = ResourceLoader.GetForViewIndependentUse("Points");

            PointOfInterestData pointOfInterestData = new PointOfInterestData();

            pointOfInterestData.FileName            = name + ".json";
            pointOfInterestData.ImageUri            = @"ms-appx:Assets/" + name + ".jpg";
            pointOfInterestData.Name                = resourceLoader.GetString(name + "/Name");
            pointOfInterestData.Description         = resourceLoader.GetString(name + "/Description");
            pointOfInterestData.Latitude.Degrees    = int.Parse(resourceLoader.GetString(name + "/Latitude/Degrees"));
            pointOfInterestData.Latitude.Minutes    = int.Parse(resourceLoader.GetString(name + "/Latitude/Minutes"));
            pointOfInterestData.Latitude.Seconds    = float.Parse(resourceLoader.GetString(name + "/Latitude/Seconds"));

            pointOfInterestData.Latitude.IsPositive = pointOfInterestData.Latitude.Degrees > 0;
            
            pointOfInterestData.Longtitude.Degrees  = int.Parse(resourceLoader.GetString(name + "/Longtitude/Degrees"));
            pointOfInterestData.Longtitude.Minutes  = int.Parse(resourceLoader.GetString(name + "/Longtitude/Minutes"));
            pointOfInterestData.Longtitude.Seconds  = float.Parse(resourceLoader.GetString(name + "/Longtitude/Seconds"));

            pointOfInterestData.Longtitude.IsPositive = pointOfInterestData.Longtitude.Degrees > 0;

            pointOfInterestData.LastVisit           = DateTime.Now;

            return pointOfInterestData;
        }
    }
}
