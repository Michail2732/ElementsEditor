using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ElementsEditor
{
    public class ElementPropertySolver<TElement, TProperty>
        where TElement : Element
    {
        private static SortedDictionary<string, Func<TElement, TProperty>> _propertyResolver;

        static ElementPropertySolver()
        {
            _propertyResolver = new SortedDictionary<string, Func<TElement, TProperty>>();
        }

        public static void AddPropertySolver(Type type, string propertyName, Func<TElement, TProperty> solver)
        {
            string key = GetSolverKey(type, propertyName);
            _propertyResolver[key] = solver;
        }

        public static TProperty SolveProperty(TElement element, string propertyName)
        {            
            string key = GetSolverKey(element.GetType(), propertyName);
            return _propertyResolver[key](element);
        }

        public static bool TrySolveProperty(TElement element, string propertyName, out TProperty? result)
        {
            result = default;
            string key = GetSolverKey(element.GetType(), propertyName);
            if (_propertyResolver.TryGetValue(key, out var solver))
            {
                result = solver(element);
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetSolverKey(Type type, string propertyName)
        {
            return type.FullName + propertyName;
        }
    }
}
