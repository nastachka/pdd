using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using PDD.Models;
using PDD.Utility;

namespace PDD.Views
{
    public sealed partial class RulesPage
    {
        public RulesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                foreach (RuleGroup header in ReadDataHelper.GetAll<RuleGroup>())
                {
                    var stackPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(0, 0, 0, 3),
                        Tag = header.Id + "|" + header.Text,
                    };

                    var border = new Border
                    {
                        Background = LayoutObjectFactory.GetThemeColor(),
                    };

                    int ruleGroupId = header.Id;
                    var ruleNumber = new TextBlock
                    {
                        Text = ruleGroupId < 10 ? "0" + ruleGroupId : ruleGroupId.ToString(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(10, 10, 10, 10),
                        FontSize = 36,
                    };

                    var text = new TextBlock
                    {
                        Text = header.Text,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(10, 10, 10, 10),
                        FontSize = 24,
                        TextWrapping = TextWrapping.Wrap,
                        Width = Window.Current.Bounds.Width - 45
                    };

                    border.Child = ruleNumber;
                    stackPanel.Children.Add(border);
                    stackPanel.Children.Add(text);

                    stackPanel.Tapped += StackPanel_Tapped;

                    RulesPanel.Children.Add(stackPanel);
                }

                LayoutObjectFactory.AddBottomAppBar(this);
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                object chapterId = ((StackPanel) sender).Tag;
                Frame.Navigate(typeof (RuleChapterPage), chapterId);
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }
    }
}
