using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElementsEditor
{
    public class ElementsCollectionGateway<TELement> : IElementsGateway<TELement>
        where TELement : Element
    {
        private readonly List<TELement> _elements;
        internal List<TELement> Elements => _elements;

        public int DebugDelay { get; set; }

        public ElementsCollectionGateway(IEnumerable<TELement> elements)
        {
            _elements = new List<TELement>(elements);
        }

        public long GetCount(Query query)
        {
            Func<TELement, bool>? filter1 = null;
            Func<TELement, bool>? filter2 = null;
            if (query.Filters != null)
                filter1 = BuildFuncFilter(query.Filters);
            if (query.ExcludedIds != null)
                filter2 = a => !query.ExcludedIds.Contains(a.Id);
            return _elements.Count(a => (filter1 == null || filter1(a)) && (filter2 == null || filter2(a)));
        }

        public async Task<long> GetCountAsync(Query query, CancellationToken ct)
        {
            await Task.Delay(DebugDelay);
            return GetCount(query);
        }

        public TELement[] GetElements(Query query)
        {
            IEnumerable<TELement> elements = _elements;
            
            if (query.Filters != null)
            {
                var filter = BuildFuncFilter(query.Filters);
                elements = elements.Where(filter);
            }                
            if (query.ExcludedIds != null)
                elements = elements.Where(a => !query.ExcludedIds.Contains(a.Id));
            if (query.Offset.HasValue)
                elements = elements.Skip(query.Offset.Value);
            if (query.Count != null)
                elements = elements.Take(query.Count.Value);

            return elements.ToArray();
        }

        public async Task<TELement[]> GetElementsAsync(Query query, CancellationToken ct)
        {
            await Task.Delay(DebugDelay);
            return GetElements(query);
        }        

        public void SaveChanges(IReadOnlyList<TELement> changesElements)
        {
            foreach (TELement element in changesElements)
            {               
                if (element.State == ElementState.Removed)
                    _elements.Remove(element);
                else if (element.State == ElementState.New)
                    _elements.Add(element);                
            }
        }

        public async Task SaveChangesAsync(IReadOnlyList<TELement> changesElements, CancellationToken ct)
        {
            await Task.Delay(DebugDelay);
            SaveChanges(changesElements);
        }

        private Func<TELement, bool> BuildFuncFilter(IReadOnlyList<IPropertyFilter> filters)
        {
            return new FuncFilterExecutor<TELement>(filters).Execute;
        }
    }
}
