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
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void oldGridViewButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(OldGridViewDemo));
        }

        private void coolListViewButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CoolListViewDemo));
        }

        private void coolGridViewButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CoolGridViewDemo));
        }
    }
}
