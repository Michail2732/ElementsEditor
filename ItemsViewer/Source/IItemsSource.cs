using System;
using System.Collections.Generic;
using System.Text;

namespace ItemsViewer.Source
{
    public interface IItemsSource
    {
        long GetCount(Query query);
        Task<long> GetCountAsync(Query query, CancellationToken ct);
        TElement[] GetElements(Query query);
        Task<TElement[]> GetElementsAsync(Query query, CancellationToken ct);
        void SaveChanges(IReadOnlyList<TElement> changesElements);
        Task SaveChangesAsync(IReadOnlyList<TElement> changesElements, CancellationToken ct);
    }
}
