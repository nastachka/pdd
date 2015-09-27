using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PDD.Models;
using PDD.Utility;

namespace PDD.Views
{
    public sealed partial class RuleChapterPage
    {
        public RuleChapterPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                object param = e.Parameter;

                if (param == null)
                {
                    return;
                }

                string[] arr = param.ToString().Split('|').ToArray();

                var headerBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(10, 10, 10, 20),
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 24,
                    Text = arr[1],
                    Foreground = LayoutObjectFactory.GetThemeColor(),
                };

                RuleChapterPanel.Children.Add(headerBlock);

                foreach (Rule item in ReadDataHelper.GetAll<Rule>().Where(item => item.GroupId == Int32.Parse(arr[0])))
                {
                    RuleChapterPanel.Children.Add(item.GetTextBlock());
                }

                LayoutObjectFactory.AddBottomAppBar(this);
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }
    }
}
