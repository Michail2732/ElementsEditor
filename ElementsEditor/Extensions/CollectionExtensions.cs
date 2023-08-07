using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElementsEditor
{
    public static class CollectionExtensions
    {
        public static TCollection? NullIfEmpty<TCollection>(this TCollection collection)
            where TCollection: class, ICollection            
        {
            if (collection.Count == 0)
                return (TCollection?)null;
            return collection;
        }


        
    }


}
