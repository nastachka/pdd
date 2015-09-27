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
    public sealed partial class SignsPage
    {
        public SignsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                ObservableCollection<SignGroup> signGroups = ReadDataHelper.GetAll<SignGroup>();
                ObservableCollection<Sign> signs = ReadDataHelper.GetAll<Sign>();

                foreach (SignGroup sign in signGroups)
                {
                    var stackPanel = new StackPanel();

                    int signId = sign.Id;
                    foreach (Sign signItem in signs.Where(i => i.GroupId == signId))
                    {
                        if (signItem.Title != null)
                        {
                            TextBlock signTitle = LayoutObjectFactory.CreateTextBlock(signItem.Title);
                            stackPanel.Children.Add(signTitle);
                        }

                        Picture.AddImageItems(stackPanel, Picture.GetPicturesByGroupId(signItem.PictureGroupId));

                        if (signItem.Description != null)
                        {
                            TextBlock signDescription = LayoutObjectFactory.CreateTextBlock(signItem.Description);
                            signDescription.FontStyle = FontStyle.Italic;
                            stackPanel.Children.Add(signDescription);
                        }
                    }

                    if (SignsBlock.Items != null)
                    {
                        SignsBlock.Items.Add(LayoutObjectFactory.CreatePivotItem(sign.Text, stackPanel));
                    }
                }

                if (e.Parameter != null)
                {
                    SignsBlock.SelectedIndex = int.Parse(e.Parameter.ToString());
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
