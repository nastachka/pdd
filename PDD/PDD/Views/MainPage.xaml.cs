using System;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using PDD.Models;
using PDD.Utility;

namespace PDD.Views
{
    public sealed partial class MainPage
    {
        private const int MenuItemsInRow = 3;
        private const int NumberOfRows = 3;

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = this;
        }

        private static Border GetMenuItem(MenuItem menuItem)
        {
            double elementWidth = (Window.Current.Bounds.Width - 30)/MenuItemsInRow;
            const int imgWidth = 64;
            double imgMargin = (elementWidth - 24 - imgWidth)/2;

            var border = new Border
            {
                Background = LayoutObjectFactory.GetThemeColor(),
                Height = elementWidth,
                Width = elementWidth,
                Margin = new Thickness(5, 5, 5, 5),
                Tag = menuItem.Type,
            };

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(0, 0, 0, 3),
            };

            var image = new Image
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Source = new BitmapImage(new Uri("ms-appx:/Assets/Menu/" + menuItem.Image)),
                Margin = new Thickness(0, imgMargin, 0, imgMargin),
                Width = imgWidth,
                Height = imgWidth,
            };

            var text = new TextBlock
            {
                Text = menuItem.Text,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(8, 0, 0, 0),
                FontSize = 16,
                TextWrapping = TextWrapping.Wrap,
            };

            stackPanel.Children.Add(image);
            stackPanel.Children.Add(text);
            border.Child = stackPanel;

            return border;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

                int n = 0;
                foreach (MenuItem menuItem in MenuStructure.GetMenuItems())
                {
                    Border border = GetMenuItem(menuItem);

                    border.Tapped += OnMenuClick;

                    Grid.SetRow(border, n/MenuItemsInRow);
                    Grid.SetColumn(border, n%NumberOfRows);

                    MainGrid.Children.Add(border);
                    n++;
                }

                LayoutObjectFactory.AddBottomAppBar(this);
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }

        private void OnMenuClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = (Border) sender;
                object tag = item.Tag;
                if (tag is Type)
                {
                    Frame.Navigate(tag as Type);
                }
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }
    }
}