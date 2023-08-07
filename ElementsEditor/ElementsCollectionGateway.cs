using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElementsEditor
{
    public class ElementsCollectionGateway<TElement> : IElementsGateway
        where TElement: Element
    {
        private readonly IList<TElement> _elements;
        private int DebugDelay { get; set; }

        public ElementsCollectionGateway(IEnumerable<TElement> elements)
        {
            _elements = new List<TElement>(elements);
        }

        public long GetCount(Query query)
        {
            Func<TElement, bool>? filter1 = null;
            Func<TElement, bool>? filter2 = null;
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

        public Element[] GetElements(Query query)
        {
            IEnumerable<TElement> elements = _elements;
            
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

        public async Task<Element[]> GetElementsAsync(Query query, CancellationToken ct)
        {
            await Task.Delay(DebugDelay);
            return GetElements(query);
        }        

        public void SaveChanges(IReadOnlyList<Element> changesElements)
        {
            foreach (TElement element in changesElements)
            {               
                if (element.State == ElementState.Removed)
                    _elements.Remove(element);
                else if (element.State == ElementState.New)
                    _elements.Add(element);                
            }
        }

        public async Task SaveChangesAsync(IReadOnlyList<Element> changesElements, CancellationToken ct)
        {
            await Task.Delay(DebugDelay);
            SaveChanges(changesElements);
        }

        private Func<TElement, bool> BuildFuncFilter(IReadOnlyList<IFilter> filters)
        {
            return new FuncFilterExecutor<TElement>(filters).Execute;
        }
    }
}
