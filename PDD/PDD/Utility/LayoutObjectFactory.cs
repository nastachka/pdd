using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using PDD.Models;
using PDD.Views;

namespace PDD.Utility
{
    internal class LayoutObjectFactory
    {
        public static TextBlock CreateTextBlock(string text)
        {
            return new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 10, 10, 10),
                TextWrapping = TextWrapping.Wrap,
                FontSize = 18,
                Text = text,
            };
        }

        public static PivotItem CreatePivotItem(string header, StackPanel stackPanel)
        {
            var grid = new Grid();
            grid.Children.Add(stackPanel);

            var scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                Content = grid
            };

            return new PivotItem
            {
                Header = header,
                Content = scrollViewer
            };
        }

        public static SolidColorBrush GetThemeColor()
        {
            return Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush ??
                   new SolidColorBrush(Color.FromArgb(0xFF, 0x1B, 0xA1, 0xE2));
        }

        /*
         * Bottom App Bar with search and app information
         */
        private static readonly Frame RootFrame = Window.Current.Content as Frame;

        private static void AboutButtonClick(object sender, RoutedEventArgs e)
        {
            RootFrame.Navigate((typeof (AboutPage)));
        }

        private static void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            RootFrame.Navigate((typeof (SearchPage)));
        }

        public static void AddBottomAppBar(Page myPage)
        {
            var commandBar = new CommandBar();
            var searchButton = new AppBarButton
            {
                Label = "Поиск",
                Icon = new SymbolIcon(Symbol.Find),
            };

            var aboutButton = new AppBarButton
            {
                Label = "О прилож.",
                Icon = new SymbolIcon(Symbol.Help),
            };

            if (RootFrame != null)
            {
                searchButton.Click += SearchButtonClick;
                aboutButton.Click += AboutButtonClick;
            }

            commandBar.PrimaryCommands.Add(searchButton);
            commandBar.PrimaryCommands.Add(aboutButton);
            myPage.BottomAppBar = commandBar;
        }

        public static bool HasMatch(string text, string searchWord)
        {
            if (text == null)
            {
                return false;
            }
            Match matchDescription = Regex.Match(text, searchWord, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return matchDescription.Success;
        }

        public static TextBlock GetResultTextBlockCase(string textString, List<string> searchWords)
        {
            TextBlock textblock = CreateTextBlock("");

            List<string> arr = textString.Split(new[] {" "}, StringSplitOptions.None).ToList();
            foreach (string item in arr)
            {
                var run = new Run {Text = item + " "};

                if (searchWords.Any(searchWord => HasMatch(item, searchWord)))
                {
                    run.Foreground = GetThemeColor();
                }

                textblock.Inlines.Add(run);
            }

            return textblock;
        }

        public static void AddPenalty(List<PenaltyArticle> penalties, List<Penalty> penaltyItems, StackPanel stackPanel,
            List<string> searchWords)
        {
            double windowWidth = Window.Current.Bounds.Width;

            foreach (PenaltyArticle penalty in penalties)
            {
                var border = new Border
                {
                    Background = GetThemeColor(),
                    Margin = new Thickness(0, 0, 0, 10),
                };
                var hearder = new TextBlock
                {
                    Text = penalty.Text,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(10, 10, 10, 10),
                    FontSize = 18,
                    TextWrapping = TextWrapping.Wrap,
                };
                border.Child = hearder;

                stackPanel.Children.Add(border);

                int fk = penalty.Id;
                foreach (Penalty item in penaltyItems.Where(i => i.PenaltyArticleId == fk))
                {
                    var block = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(0, 0, 0, 10),
                    };

                    const int spaces = 70;
                    const int widthUnit = 150;
                    double sanctionsWidth = windowWidth >= widthUnit*2 + spaces ? widthUnit : (windowWidth - spaces)/2;

                    TextBlock text = searchWords != null
                        ? GetResultTextBlockCase(item.Text, searchWords)
                        : new TextBlock {Text = item.Text};
                    text.Width = (item.Fine != null) ? windowWidth - sanctionsWidth - spaces : windowWidth - 40;
                    text.HorizontalAlignment = HorizontalAlignment.Left;
                    text.VerticalAlignment = VerticalAlignment.Top;
                    text.Margin = new Thickness(10, 5, 10, 5);
                    text.FontSize = 18;
                    text.TextWrapping = TextWrapping.Wrap;

                    block.Children.Add(text);

                    if (item.Fine != null)
                    {
                        TextBlock sanctions = searchWords != null
                            ? GetResultTextBlockCase(item.Fine, searchWords)
                            : new TextBlock {Text = item.Fine};
                        sanctions.Width = sanctionsWidth;
                        sanctions.HorizontalAlignment = HorizontalAlignment.Left;
                        sanctions.VerticalAlignment = VerticalAlignment.Top;
                        sanctions.Margin = new Thickness(10, 5, 10, 5);
                        sanctions.FontSize = 18;
                        sanctions.TextWrapping = TextWrapping.Wrap;

                        block.Children.Add(sanctions);
                    }
                    stackPanel.Children.Add(block);
                }
            }
        }

        public static TextBlock GetDescription(string description)
        {
            TextBlock textBlock = CreateTextBlock(description);
            textBlock.FontStyle = FontStyle.Italic;
            return textBlock;
        }
    }
}
