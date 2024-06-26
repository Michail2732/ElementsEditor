﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElementsEditor
{
    public interface IElementsGateway<TElement>
        where TElement : Element
    {                
        long GetCount(Query query);                
        Task<long> GetCountAsync(Query query, CancellationToken ct);
        TElement[] GetElements(Query query);
        Task<TElement[]> GetElementsAsync(Query query, CancellationToken ct);
        void Remove(IEnumerable<TElement> elements);
        Task RemoveAsync(IEnumerable<TElement> elements, CancellationToken ct);
        void Add(TElement element);
        Task AddAsync(TElement element, CancellationToken ct);
        void SaveChanges(IEnumerable<TElement> changesElements);
        Task SaveChangesAsync(IEnumerable<TElement> changesElements, CancellationToken ct);
    }
}
