namespace ElementsEditor
{
    public interface IPropertyFilterExecutor<TElement, TResult>
        where TElement: Element
    {
        TResult Execute(TElement element, StringPropertyFilter filter);
        TResult Execute(TElement element, IntPropertyFilter filter);
        TResult Execute(TElement element, DoublePropertyFilter filter);
        TResult Execute(TElement element, DecimalPropertyFilter filter);
        TResult Execute(TElement element, DateTimePropertyFilter filter);
        TResult Execute(TElement element, BoolPropertyFilter filter);        
    }

}
