using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WindowsUXDemo.Data;
using Utility;

namespace WindowsUXDemo.Views
{
    public sealed partial class PanoramaDemo : Page
    {
        public PanoramaDemo()
        {
            this.InitializeComponent();
            LoadSampleData();
            panorama.AttachTo = itemGridView;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ShowPageTitle.Begin();
            SunshineStory.Begin();
        }

        private void LoadSampleData()
        {
            var sampleDataGroups = SampleDataSource.GetGroups("AllGroups");
            this.DataContext = sampleDataGroups;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
