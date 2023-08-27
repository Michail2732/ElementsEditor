namespace ElementsEditor
{
    public interface IPropertyFilter
	{
        Logic Logic { get; }        
        ConditionOperation Operation { get; }                
        TResult Execute<TElement, TResult>(TElement element, IPropertyFilterExecutor<TElement, TResult> executor) where TElement : Element;        
    }

}
