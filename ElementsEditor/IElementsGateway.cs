using System;
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
    public interface IElementsGateway        
    {                
        long GetCount(Query query);                
        Task<long> GetCountAsync(Query query, CancellationToken ct);
        Element[] GetElements(Query query);
        Task<Element[]> GetElementsAsync(Query query, CancellationToken ct);
        void SaveChanges(IReadOnlyList<Element> changesElements);
        Task SaveChangesAsync(IReadOnlyList<Element> changesElements, CancellationToken ct);
    }
}
