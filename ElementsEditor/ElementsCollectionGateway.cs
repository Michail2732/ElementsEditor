using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElementsEditor
{
    public class ElementsCollectionGateway<TElement> : IElementsGateway<TElement>
        where TElement : Element
    {
        private readonly List<TElement> _elements;
        private readonly PropertyFilterToFuncConverter<TElement> _filterConverter;
        internal List<TElement> Elements => _elements;

        public int DebugDelay { get; set; }

        public ElementsCollectionGateway(IEnumerable<TElement> elements)
        {
            _elements = new List<TElement>(elements);
            _filterConverter = new PropertyFilterToFuncConverter<TElement>();
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

        public TElement[] GetElements(Query query)
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

        public async Task<TElement[]> GetElementsAsync(Query query, CancellationToken ct)
        {
            await Task.Delay(DebugDelay);
            return GetElements(query);
        }

        public void SaveChanges(IEnumerable<TElement> changesElements)
        {
            
        }

        public async Task SaveChangesAsync(IEnumerable<TElement> changesElements, CancellationToken ct)
        {
            await Task.Delay(DebugDelay);
            SaveChanges(changesElements);
        }

        private Func<TElement, bool> BuildFuncFilter(IReadOnlyList<IPropertyFilter> filters)
        {
            return _filterConverter.Convert(filters);
        }

        public void Remove(IEnumerable<TElement> elements)
        {
            foreach (var element in elements)
            {
                _elements.Remove(element);
            }
        }

        public async Task RemoveAsync(IEnumerable<TElement> elements, CancellationToken ct)
        {
            await Task.Delay(DebugDelay);
            Remove(elements);
        }

        public void Add(TElement element)
        {
            _elements.Add(element);
        }

        public async Task AddAsync(TElement element, CancellationToken ct)
        {
            await Task.Delay(DebugDelay);
            Add(element);
        }        
    }
}
