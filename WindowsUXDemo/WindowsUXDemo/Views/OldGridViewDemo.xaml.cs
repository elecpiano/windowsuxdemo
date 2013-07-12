using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WindowsUXDemo.Views
{
    public sealed partial class OldGridViewDemo : Page
    {
        public OldGridViewDemo()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ShowPageTitle.Begin();
            LoadSampleData();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void LoadSampleData()
        {
            ObservableCollection<int> items = new ObservableCollection<int>();
            for (int i = 0; i < 32; i++)
            {
                items.Add(i);
            }
            gridView.ItemsSource = items;
        }
    }
}
