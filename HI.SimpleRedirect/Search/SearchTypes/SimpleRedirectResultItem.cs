using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HI.SimpleRedirect.Search.SearchTypes
{
    class SimpleRedirectResultItem : SearchResultItem
    {
        [IndexField("Source")]
        public string Source { get; set; }

        [IndexField("Target")]
        public string Target { get; set; }
    }
}
