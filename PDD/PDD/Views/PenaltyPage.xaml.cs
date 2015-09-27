using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using PDD.Models;
using PDD.Utility;

namespace PDD.Views
{
    public sealed partial class PenaltyPage
    {
        public PenaltyPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                ObservableCollection<PenaltyArticle> penaltyArticles = ReadDataHelper.GetAll<PenaltyArticle>();
                List<Penalty> penalties = ReadDataHelper.GetAll<Penalty>().ToList();
                LayoutObjectFactory.AddPenalty(penaltyArticles.Where(i => i.PenaltyGroupId == 1).ToList(), penalties,
                    AdministrativePenalty, null);
                LayoutObjectFactory.AddPenalty(penaltyArticles.Where(i => i.PenaltyGroupId == 2).ToList(), penalties,
                    CriminalPenalty, null);

                if (e.Parameter != null)
                {
                    PenaltyPivot.SelectedIndex = int.Parse(e.Parameter.ToString());
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
