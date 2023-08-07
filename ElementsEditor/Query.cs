using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElementsEditor
{
    public class Query
    {
        public int? Offset { get; }
        public int? Count { get; }
        public IReadOnlyList<string>? ExcludedIds { get; }
        public IReadOnlyList<IFilter>? Filters { get; }

        public Query(int? offset = null, int? count = null,
            IReadOnlyList<string>? excludedIds = null,
            IReadOnlyList<IFilter>? filters = null)
        {
            Offset = offset;
            Count = count;
            ExcludedIds = excludedIds;
            Filters = filters;
        }
    }
}
