using System.Collections.Generic;

namespace PDD.Models
{
    internal class SearchResult
    {
        public string SearchText;
        public List<string> SearchWords;
        public bool BackToPrevSearch;
        public int? PrevPivotItem;
        public SearchBlock BlocksFound;

        public SearchResult(string searchText, List<string> searchWords, bool backToPrevSearch, int prevPivotItem,
            SearchBlock blocksFound)
        {
            SearchText = searchText;
            SearchWords = searchWords;
            BackToPrevSearch = backToPrevSearch;
            PrevPivotItem = prevPivotItem;
            BlocksFound = blocksFound;
        }

        public static SearchResult LastSearchResult = new SearchResult(null, null, false, 1, new SearchBlock());
    }
}
