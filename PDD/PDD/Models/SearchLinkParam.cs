using System;

namespace PDD.Models
{
    internal class SearchLinkParam
    {
        public Type PageType;
        public int? PivotId;
        public int? NavigateToPivotId;

        public SearchLinkParam(Type pageType, int? pivotId, int? navigateToPivotId)
        {
            PageType = pageType;
            PivotId = pivotId;
            NavigateToPivotId = navigateToPivotId;
        }
    }
}
