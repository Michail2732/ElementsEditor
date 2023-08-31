namespace ElementsEditor
{
    public interface IPropertyFilterConverter<TResult>        
    {
        TResult Convert(StringPropertyFilter filter);
        TResult Convert(IntPropertyFilter filter);
        TResult Convert(DoublePropertyFilter filter);
        TResult Convert(DecimalPropertyFilter filter);
        TResult Convert(DateTimePropertyFilter filter);
        TResult Convert(BoolPropertyFilter filter);        
    }

}
