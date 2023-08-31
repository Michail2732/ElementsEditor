namespace ElementsEditor
{
    public interface IPropertyFilter
	{
        Logic Logic { get; }        
        ConditionOperation Operation { get; }                
        TResult Convert<TResult>(IPropertyFilterConverter<TResult> executor);        
    }

}
