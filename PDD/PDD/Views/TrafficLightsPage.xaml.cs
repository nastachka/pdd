using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PDD.Models;
using PDD.Utility;

namespace PDD.Views
{
    public sealed partial class TrafficLightsPage
    {
        public TrafficLightsPage()
        {
            InitializeComponent();
        }

        private static StackPanel AddBlockToStackPanel(IEnumerable<TrafficLight> lights)
        {
            var stackPanel = new StackPanel();

            foreach (TrafficLight light in lights)
            {
                stackPanel.Children.Add(LayoutObjectFactory.CreateTextBlock(light.Title));
                Picture.AddImageItems(stackPanel, Picture.GetPicturesByGroupId(light.PictureGroupId));
            }

            return stackPanel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                ObservableCollection<TrafficLight> trafficLights = ReadDataHelper.GetAll<TrafficLight>();
                ObservableCollection<TrafficLightGroup> trafficLightsGroups = ReadDataHelper.GetAll<TrafficLightGroup>();

                foreach (TrafficLightGroup item in trafficLightsGroups)
                {
                    if (TrafficBlock.Items != null)
                    {
                        List<TrafficLight> trafficLightsInGroup =
                            trafficLights.Where(i => i.GroupId == item.Id).ToList();
                        TrafficBlock.Items.Add(LayoutObjectFactory.CreatePivotItem(item.Text,
                            AddBlockToStackPanel(trafficLightsInGroup)));
                    }
                }

                if (e.Parameter != null)
                {
                    TrafficBlock.SelectedIndex = int.Parse(e.Parameter.ToString());
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
