using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using PDD.Models;
using PDD.Utility;

namespace PDD.Views
{
    public sealed partial class PointsmanPage
    {
        public PointsmanPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                PointsmanTitle.Foreground = LayoutObjectFactory.GetThemeColor();

                ObservableCollection<Pointsman> pointsmans = ReadDataHelper.GetAll<Pointsman>();

                foreach (Pointsman item in pointsmans)
                {
                    PointsmanPanel.Children.Add(LayoutObjectFactory.CreateTextBlock(item.Title));

                    Picture.AddImageItems(PointsmanPanel, Picture.GetPicturesByGroupId(item.PictureGroupId));

                    if (item.Description != null)
                    {
                        PointsmanPanel.Children.Add(LayoutObjectFactory.GetDescription(item.Description));
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
