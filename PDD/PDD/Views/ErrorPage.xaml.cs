using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using PDD.Utility;

namespace PDD.Views
{
    public sealed partial class ErrorPage
    {
        public ErrorPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ErrorPageTitle.Foreground = LayoutObjectFactory.GetThemeColor();
            LayoutObjectFactory.AddBottomAppBar(this);
        }

        private async void FeedbackClick(object sender, RoutedEventArgs e)
        {
            var mailto = new Uri("mailto:tyshkavets@gmail.com");
            await Launcher.LaunchUriAsync(mailto);
        }

        private void MainMenClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (MainPage));
        }
    }
}
