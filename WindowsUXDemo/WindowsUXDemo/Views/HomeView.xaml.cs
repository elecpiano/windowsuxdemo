using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WindowsUXDemo.Views
{
    public sealed partial class HomeView : Page
    {
        public HomeView()
        {
            this.InitializeComponent();
            HideMenu.Completed += HideMenu_Completed;
        }

        void HideMenu_Completed(object sender, object e)
        {
            GoToPage();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ShowMenu.Begin();
        }

        string _TargetTag;
        private void GoToPage()
        {
            switch (_TargetTag)
            {
                case "animation":
                    this.Frame.Navigate(typeof(AnimationUtilityDemo));
                    break;
                case "popup":
                    this.Frame.Navigate(typeof(CustomPopupDemo));
                    break;
                case "radial":
                    this.Frame.Navigate(typeof(RadialMenuDemo));
                    break;
                case "panorama":
                    this.Frame.Navigate(typeof(PanoramaDemo));
                    break;
                case "reorder":
                    this.Frame.Navigate(typeof(ReorderPanelDemo));
                    break;
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            _TargetTag = (sender as FrameworkElement).Tag.ToString();
            HideMenu.Begin();
        }
    }
}
