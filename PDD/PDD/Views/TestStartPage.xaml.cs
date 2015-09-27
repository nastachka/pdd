using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using PDD.Utility;

namespace PDD.Views
{
    public sealed partial class TestStartPage
    {
        public TestStartPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                Title.Foreground = LayoutObjectFactory.GetThemeColor();

                LayoutObjectFactory.AddBottomAppBar(this);
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }

        private void StartTest(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame.Navigate(typeof (TestsPage));
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }
    }
}
