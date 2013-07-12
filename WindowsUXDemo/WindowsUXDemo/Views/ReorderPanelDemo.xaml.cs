using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WindowsUXDemo.Views
{
    public sealed partial class ReorderPanelDemo : Page
    {
        public ReorderPanelDemo()
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
            ObservableCollection<int> items1 = new ObservableCollection<int>();
            ObservableCollection<int> items2 = new ObservableCollection<int>();

            for (int i = 0; i < 10; i++)
            {
                items1.Add(i);
                items2.Add(i);
            }
            oldListView.ItemsSource = items1;
            xListView.ItemsSource = items2;
        }
    }
}
