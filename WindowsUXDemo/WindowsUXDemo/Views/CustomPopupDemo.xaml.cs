using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WindowsUXDemo.Controls;

namespace WindowsUXDemo.Views
{
    public sealed partial class CustomPopupDemo : Page
    {
        public CustomPopupDemo()
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

        private void item_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            MyUserControl1 uc = new MyUserControl1();
            popupContainer.Show(uc, sender);
        }

    }
}
