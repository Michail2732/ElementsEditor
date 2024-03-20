using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsViewer.Source
{
    public class Query
    {
        public int? Offset { get; }
        public int? Count { get; }
        public IReadOnlyList<string>? ExcludedIds { get; }
        public IReadOnlyList<IPropertyFilter>? Filters { get; }

        public Query(int? offset = null, int? count = null,
            IReadOnlyList<string>? excludedIds = null,
            IReadOnlyList<IPropertyFilter>? filters = null)
        {
            Offset = offset;
            Count = count;
            ExcludedIds = excludedIds;
            Filters = filters;
        }
    }
}
