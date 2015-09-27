using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PDD.Models;
using PDD.Utility;

namespace PDD.Views
{
    public sealed partial class MarkingsPage
    {
        public MarkingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                ObservableCollection<MarkingGroup> markingGroups = ReadDataHelper.GetAll<MarkingGroup>();
                ObservableCollection<Marking> markings = ReadDataHelper.GetAll<Marking>();

                foreach (MarkingGroup marking in markingGroups)
                {
                    var stackPanel = new StackPanel();

                    int markingId = marking.Id;
                    foreach (Marking item in markings.Where(i => i.GroupId == markingId))
                    {
                        if (item.Title != null)
                        {
                            TextBlock titleBlock = LayoutObjectFactory.CreateTextBlock(item.Title);
                            stackPanel.Children.Add(titleBlock);
                        }

                        Picture.AddImageItems(stackPanel, Picture.GetPicturesByGroupId(item.PictureGroupId));

                        if (item.Description != null)
                        {
                            TextBlock descriptionBlock = LayoutObjectFactory.CreateTextBlock(item.Description);
                            descriptionBlock.FontStyle = FontStyle.Italic;
                            stackPanel.Children.Add(descriptionBlock);
                        }
                    }

                    if (MarkingsBlock.Items != null)
                    {
                        MarkingsBlock.Items.Add(LayoutObjectFactory.CreatePivotItem(marking.Text, stackPanel));
                    }
                }

                if (e.Parameter != null)
                {
                    MarkingsBlock.SelectedIndex = int.Parse(e.Parameter.ToString());
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
