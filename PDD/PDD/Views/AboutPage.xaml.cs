using System;
using Windows.ApplicationModel.Store;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PDD.Models;
using PDD.Utility;

namespace PDD.Views
{
    public sealed partial class AboutPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                AboutTitle.Foreground = LayoutObjectFactory.GetThemeColor();

                foreach (Law laws in ReadDataHelper.GetAll<Law>())
                {
                    LawStackPanel.Children.Add(new TextBlock
                    {
                        Text = laws.Text,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(5, 10, 10, 0),
                        FontSize = 18,
                        TextWrapping = TextWrapping.Wrap,
                        Width = Window.Current.Bounds.Width - 45
                    });
                }

                LayoutObjectFactory.AddBottomAppBar(this);
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }

        private async void RateClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }

        private async void FeedbackClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var mailto = new Uri("mailto:tyshkavets@gmail.com");
                await Launcher.LaunchUriAsync(mailto);
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }
    }
}
