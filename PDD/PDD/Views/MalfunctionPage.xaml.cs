using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PDD.Models;
using PDD.Utility;

namespace PDD.Views
{
    public sealed partial class MalfunctionPage
    {
        public MalfunctionPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                ObservableCollection<MalfunctionGroup> malfunctionGroupItems = ReadDataHelper.GetAll<MalfunctionGroup>();
                ObservableCollection<Malfunction> malfunctions = ReadDataHelper.GetAll<Malfunction>();

                foreach (MalfunctionGroup item in malfunctionGroupItems)
                {
                    var stackPanel = new StackPanel();
                    int itemId = item.Id;
                    foreach (Malfunction malfunction in malfunctions.Where(i => i.GroupId == itemId))
                    {
                        stackPanel.Children.Add(LayoutObjectFactory.CreateTextBlock(malfunction.Text));
                    }

                    if (MalfunctionBlock.Items != null)
                    {
                        MalfunctionBlock.Items.Add(LayoutObjectFactory.CreatePivotItem(item.Text, stackPanel));
                    }
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
