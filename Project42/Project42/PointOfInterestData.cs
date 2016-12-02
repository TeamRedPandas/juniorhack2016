﻿using System;
using System.Threading.Tasks;
using UWPHelper.Utilities;
using Windows.UI.Xaml.Controls;

namespace Project42
{
    public sealed class PointOfInterestData : NotifyPropertyChangedBase
    {
        internal string FileName { get; set; }

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
        public float Longtitude
        {
            get { return (float)GetValue(nameof(Longtitude)); }
            set { SetValue(nameof(Longtitude), ref value); }
        }
        public float Latitude
        {
            get { return (float)GetValue(nameof(Latitude)); }
            set { SetValue(nameof(Latitude), ref value); }
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
            RegisterProperty(nameof(Name), typeof(string), "");
            RegisterProperty(nameof(Description), typeof(string), "");
            RegisterProperty(nameof(Longtitude), typeof(float), 0f);
            RegisterProperty(nameof(Latitude), typeof(float), 0f);
            RegisterProperty(nameof(ImagePreview), typeof(Image), null);
            RegisterProperty(nameof(LastVisit), typeof(DateTime), new DateTime());
        }

        public async Task SaveAsync()
        {
            await StorageFileHelper.SaveObjectAsync(this, FileName, await PointsOfInterest.GetFolderAsync());
        }
    }
}