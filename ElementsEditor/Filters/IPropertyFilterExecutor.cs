namespace ElementsEditor
{
    public interface IPropertyFilterExecutor<TResult>
    {
        TResult Execute(Element element, StringPropertyFilter filter);
        TResult Execute(Element element, IntPropertyFilter filter);
        TResult Execute(Element element, DoublePropertyFilter filter);
        TResult Execute(Element element, DecimalPropertyFilter filter);
        TResult Execute(Element element, DateTimePropertyFilter filter);
        TResult Execute(Element element, BoolPropertyFilter filter);
        TResult Execute(Element element, CustomPropertyFilter filter);
    }

}
