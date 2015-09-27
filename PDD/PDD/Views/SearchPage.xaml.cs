using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using PDD.Models;
using PDD.Utility;

namespace PDD.Views
{
    public sealed partial class SearchPage
    {
        private bool _isSearchInProgress;

        public SearchPage()
        {
            InitializeComponent();

            if (!SearchResult.LastSearchResult.BackToPrevSearch)
            {
                Loaded += (s, e) =>
                {
                    SearchTextBox.SelectAll();
                    SearchTextBox.Focus(FocusState.Programmatic);
                };
            }
        }

        private void ClearSearchResults()
        {
            if (SearchResults.Items == null)
            {
                return;
            }
            List<PivotItem> itemsToDelete = SearchResults.Items.Select(item => item as PivotItem).ToList();
            foreach (PivotItem itemToDelete in itemsToDelete)
            {
                SearchResults.Items.Remove(itemToDelete);
            }
        }

        private void AddSearchResultsBlock(string menuItemText, StackPanel stackPanel)
        {
            var pivotItem = new PivotItem
            {
                Header = menuItemText,
                Content = stackPanel
            };

            if (SearchResults.Items != null)
            {
                SearchResults.Items.Add(pivotItem);
            }
        }

        private static HyperlinkButton GetTitleHyperlink(string titleText, SearchLinkParam searchLinkParam, string tag)
        {
            TextBlock hyperlinkText = LayoutObjectFactory.CreateTextBlock(titleText);
            hyperlinkText.FontSize = 24;

            return new HyperlinkButton
            {
                Foreground = LayoutObjectFactory.GetThemeColor(),
                Content = hyperlinkText,
                Tag = tag,
                CommandParameter = searchLinkParam,
            };
        }

        private static List<TG> GetSignGroups<T, TG>(IEnumerable<T> signs, IEnumerable<TG> signGroups)
            where T : BaseSign, new()
            where TG : BaseGroup, new()
        {
            List<int> groupIds = signs.Select(sign => sign.GroupId).Distinct().ToList();
            return signGroups.Where(signGroup => groupIds.Contains(signGroup.Id)).ToList();
        }

        private void DisplaItmesInGroup<T>(StackPanel stackPanel, IEnumerable<T> itemsInGroup, int? groupId,
            string groupName, Type pageType,
            List<string> searchWords, int pivotId) where T : BaseSign, new()
        {
            var searchLinkParam = new SearchLinkParam(pageType, pivotId, groupId);
            HyperlinkButton hyperlink = GetTitleHyperlink(groupName, searchLinkParam, null);
            hyperlink.Click += MoveToSign;
            stackPanel.Children.Add(hyperlink);


            foreach (T sign in itemsInGroup)
            {
                if (sign.Title != null)
                {
                    stackPanel.Children.Add(LayoutObjectFactory.GetResultTextBlockCase(sign.Title, searchWords));
                }

                Picture.AddImageItems(stackPanel, Picture.GetPicturesByGroupId(sign.PictureGroupId));

                if (sign.Description != null)
                {
                    stackPanel.Children.Add(LayoutObjectFactory.GetResultTextBlockCase(sign.Description, searchWords));
                }
            }
        }

        private void DisplayPivotItmesFound<T, TG>(string menuItemText, List<T> foundItems, IEnumerable<TG> itemsGroup,
            Type pageType,
            List<string> searchWords, int pivotId) where T : BaseSign, new() where TG : BaseGroup, new()
        {
            var stackPanel = new StackPanel();
            List<TG> signGroups = GetSignGroups(foundItems, itemsGroup);

            foreach (TG blockResult in signGroups)
            {
                int groupId = blockResult.Id;
                DisplaItmesInGroup(stackPanel, foundItems.Where(i => i.GroupId == groupId).ToList(), blockResult.Id,
                    blockResult.Text, pageType,
                    searchWords, pivotId);
            }

            AddSearchResultsBlock(menuItemText, stackPanel);
        }

        private void DisplayItmesFound<T>(string menuItemText, IEnumerable<T> foundItems, Type pageType,
            List<string> searchWords, int pivotId)
            where T : BaseSign, new()
        {
            var stackPanel = new StackPanel();

            DisplaItmesInGroup(stackPanel, foundItems, null, menuItemText, pageType, searchWords, pivotId);

            AddSearchResultsBlock(menuItemText, stackPanel);
        }

        private static List<T> SearchItems<T>(List<string> searchWords, IEnumerable<T> items) where T : BaseSign, new()
        {
            var resultItems = new List<T>();

            foreach (T item in items)
            {
                bool hasSearchWords =
                    searchWords.Any(searchWord => LayoutObjectFactory.HasMatch(item.Description, searchWord) ||
                                                  LayoutObjectFactory.HasMatch(item.Title, searchWord));
                if (hasSearchWords)
                {
                    resultItems.Add(item);
                }
            }

            return resultItems;
        }

        private void DisplayRulesFound(string menuItemText, List<Rule> blockItemsResult, List<string> searchWords)
        {
            var stackPanel = new StackPanel();

            ObservableCollection<RuleGroup> itemsList = ReadDataHelper.GetAll<RuleGroup>();

            foreach (RuleGroup item in itemsList)
            {
                List<Rule> ruleItemResults = blockItemsResult.Where(result => result.GroupId == item.Id).ToList();

                if (ruleItemResults.Count <= 0)
                {
                    continue;
                }

                HyperlinkButton hyperlink = GetTitleHyperlink(item.Id + ". " + item.Text, null,
                    item.Id + "|" + item.Text);
                hyperlink.Click += MoveToRule;
                stackPanel.Children.Add(hyperlink);

                foreach (Rule ruleItemResult in ruleItemResults)
                {
                    stackPanel.Children.Add(LayoutObjectFactory.GetResultTextBlockCase(ruleItemResult.Text, searchWords));
                }
            }

            AddSearchResultsBlock(menuItemText, stackPanel);
        }

        private List<Rule> SearchRules(List<string> searchWords)
        {
            var blockItemsResult = new List<Rule>();
            foreach (Rule rule in ReadDataHelper.GetAll<Rule>())
            {
                bool hasSearchWords = searchWords.Any(searchWord => LayoutObjectFactory.HasMatch(rule.Text, searchWord));
                if (hasSearchWords)
                {
                    blockItemsResult.Add(rule);
                }
            }

            return blockItemsResult;
        }

        private static IEnumerable<PenaltyArticle> GetPaneltyArticles(IEnumerable<Penalty> items,
            IEnumerable<PenaltyArticle> articles)
        {
            List<int> groupIds = items.Select(i => i.PenaltyArticleId).Distinct().ToList();
            return articles.Where(i => groupIds.Contains(i.Id)).ToList();
        }

        private static IEnumerable<PenaltyGroup> GetPaneltyGroups(IEnumerable<PenaltyArticle> articles,
            IEnumerable<PenaltyGroup> groups)
        {
            List<int> groupIds = articles.Select(i => i.PenaltyGroupId).Distinct().ToList();
            return groups.Where(i => groupIds.Contains(i.Id)).ToList();
        }

        private void DisplayPenaltyFound(string menuItemText, List<Penalty> foundPenalties, List<string> searchWord,
            int pivotId)
        {
            var stackPanel = new StackPanel();
            List<PenaltyArticle> penaltyArticles =
                GetPaneltyArticles(foundPenalties, ReadDataHelper.GetAll<PenaltyArticle>()).ToList();
            IEnumerable<PenaltyGroup> penaltyGroup = GetPaneltyGroups(penaltyArticles,
                ReadDataHelper.GetAll<PenaltyGroup>());

            foreach (PenaltyGroup penaltyBlock in penaltyGroup)
            {
                var searchLinkParam = new SearchLinkParam(typeof (PenaltyPage), pivotId, penaltyBlock.Id);
                HyperlinkButton hyperlink = GetTitleHyperlink(penaltyBlock.Text, searchLinkParam, null);
                hyperlink.Click += MoveToSign;
                stackPanel.Children.Add(hyperlink);

                LayoutObjectFactory.AddPenalty(
                    penaltyArticles.Where(i => i.PenaltyGroupId == penaltyBlock.Id).ToList(), foundPenalties, stackPanel,
                    searchWord);
            }

            AddSearchResultsBlock(menuItemText, stackPanel);
        }

        private static List<Penalty> SearchPenalty(List<string> searchWords)
        {
            var result = new List<Penalty>();

            ObservableCollection<Penalty> penalties = ReadDataHelper.GetAll<Penalty>();
            foreach (Penalty item in penalties)
            {
                bool hasSearchWords = searchWords.Any(searchWord =>
                    LayoutObjectFactory.HasMatch(item.Text, searchWord) ||
                    LayoutObjectFactory.HasMatch(item.Fine, searchWord));

                if (hasSearchWords)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public static string RemoveSpecialCharacters(string str)
        {
            var sb = new StringBuilder();
            foreach (char t in str.Where(t => Char.IsDigit(t) || Char.IsLetter(t)))
            {
                sb.Append(t);
            }

            return sb.ToString();
        }

        public static string RemoveAllSpaces(string str)
        {
            var sb = new StringBuilder();
            foreach (char t in str.Where(t => t != ' '))
            {
                sb.Append(t);
            }

            return sb.ToString();
        }

        public static string RemoveReduntantSpaces(string str)
        {
            str = str.TrimEnd(' ').TrimStart(' ');
            var trimmer = new Regex(@"\s\s+");
            return trimmer.Replace(str, " ");
        }

        private void DisplaySearchResults(SearchBlock searchResults, List<string> searchWords)
        {
            int pivotId = 0;
            if (searchResults.RulesFound != null && searchResults.RulesFound.Count > 0)
            {
                DisplayRulesFound("Правила", searchResults.RulesFound, searchWords);
                pivotId++;
            }

            if (searchResults.SignsFound != null && searchResults.SignsFound.Count > 0)
            {
                DisplayPivotItmesFound("Знаки", searchResults.SignsFound, ReadDataHelper.GetAll<SignGroup>(),
                    typeof (SignsPage), searchWords, pivotId);
                pivotId++;
            }

            if (searchResults.MarkingsFound != null && searchResults.MarkingsFound.Count > 0)
            {
                DisplayPivotItmesFound("Разметка", searchResults.MarkingsFound, ReadDataHelper.GetAll<MarkingGroup>(),
                    typeof (MarkingsPage), searchWords, pivotId);
                pivotId++;
            }

            if (searchResults.TrafficLightsFound != null && searchResults.TrafficLightsFound.Count > 0)
            {
                DisplayPivotItmesFound("Светофоры", searchResults.TrafficLightsFound,
                    ReadDataHelper.GetAll<TrafficLightGroup>(), typeof (TrafficLightsPage), searchWords, pivotId);
                pivotId++;
            }

            if (searchResults.PointsmanFound != null && searchResults.PointsmanFound.Count > 0)
            {
                DisplayItmesFound("Регулировщик", searchResults.PointsmanFound, typeof (PointsmanPage), searchWords,
                    pivotId);
                pivotId++;
            }

            if (searchResults.PenaltiesFound != null && searchResults.PenaltiesFound.Count > 0)
            {
                DisplayPenaltyFound("Штрафы", searchResults.PenaltiesFound, searchWords, pivotId);
                pivotId++;
            }

            if (pivotId == 0)
            {
                NothingFound.Visibility = Visibility.Visible;
            }
        }

        private static List<string> GetSearchWords(string searchText)
        {
            List<string> searchWords =
                RemoveReduntantSpaces(searchText).ToLower().Split(new[] {" "}, StringSplitOptions.None).ToList();
            return
                searchWords.Select(RemoveSpecialCharacters)
                    .Where(word => word.Length > 1 || (word.Length == 1 && IsNumber(word)))
                    .ToList();
        }

        private SearchBlock GetSearchResults(List<string> searchWords)
        {
            var searchResults = new SearchBlock();

            foreach (MenuItem menuItem in MenuStructure.GetMenuItems())
            {
                switch (menuItem.Id)
                {
                    case 0:
                        searchResults.RulesFound = SearchRules(searchWords);
                        break;
                    case 1:
                        searchResults.SignsFound = SearchItems(searchWords, ReadDataHelper.GetAll<Sign>());
                        break;
                    case 2:
                        searchResults.MarkingsFound = SearchItems(searchWords, ReadDataHelper.GetAll<Marking>());
                        break;
                    case 3:
                        searchResults.TrafficLightsFound = SearchItems(searchWords,
                            ReadDataHelper.GetAll<TrafficLight>());
                        break;
                    case 5:
                        searchResults.PointsmanFound = SearchItems(searchWords, ReadDataHelper.GetAll<Pointsman>());
                        break;
                    case 6:
                        searchResults.PenaltiesFound = SearchPenalty(searchWords);
                        break;
                }
            }

            return searchResults;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SearchTitle.Foreground = LayoutObjectFactory.GetThemeColor();
            LayoutObjectFactory.AddBottomAppBar(this);

            if (!SearchResult.LastSearchResult.BackToPrevSearch)
            {
                return;
            }

            if (SearchResult.LastSearchResult.SearchText != null)
            {
                SearchTextBox.Text = SearchResult.LastSearchResult.SearchText;
            }

            try
            {
                DisplaySearchResults(SearchResult.LastSearchResult.BlocksFound,
                    SearchResult.LastSearchResult.SearchWords);
                SearchResults.SelectedIndex = int.Parse(SearchResult.LastSearchResult.PrevPivotItem.ToString());
                SearchResult.LastSearchResult.BackToPrevSearch = false;
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }

        private static bool IsNumber(string str)
        {
            int n;
            return int.TryParse(str, out n);
        }

        private static bool IsEmptySearch(string searchString)
        {
            string cleanString = RemoveAllSpaces(RemoveSpecialCharacters(searchString));
            return cleanString.Length < 2 && (cleanString.Length != 1 || !IsNumber(cleanString));
        }

        private async void SearchEnterKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter || _isSearchInProgress || IsEmptySearch(SearchTextBox.Text))
            {
                return;
            }

            try
            {
                _isSearchInProgress = true;
                SearchResults.Focus(FocusState.Programmatic);
                ClearSearchResults();
                NothingFound.Visibility = Visibility.Collapsed;
                Preloader.Visibility = Visibility.Visible;

                string searchText = SearchTextBox.Text;
                List<string> searchWords = GetSearchWords(searchText);
                SearchBlock searchResult = await Task.Run(() => GetSearchResults(searchWords));

                Preloader.Visibility = Visibility.Collapsed;
                DisplaySearchResults(searchResult, searchWords);

                SearchResult.LastSearchResult.SearchText = searchText;
                SearchResult.LastSearchResult.SearchWords = searchWords;
                SearchResult.LastSearchResult.BlocksFound = searchResult;

                _isSearchInProgress = false;
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }

        private void MoveToRule(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (RuleChapterPage), ((HyperlinkButton) sender).Tag);
            SearchResult.LastSearchResult.BackToPrevSearch = true;
            SearchResult.LastSearchResult.PrevPivotItem = 0;
        }

        private void MoveToSign(object sender, RoutedEventArgs e)
        {
            var searchLinkParam = (SearchLinkParam) ((HyperlinkButton) sender).CommandParameter;
            if (searchLinkParam == null)
            {
                return;
            }
            SearchResult.LastSearchResult.BackToPrevSearch = true;
            SearchResult.LastSearchResult.PrevPivotItem = searchLinkParam.PivotId;
            Frame.Navigate(searchLinkParam.PageType, searchLinkParam.NavigateToPivotId - 1);
        }
    }
}
