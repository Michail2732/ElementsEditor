namespace ElementsEditor
{
    public interface IPropertyFilter
	{
        Logic Logic { get; }        
        ConditionOperatioin Operation { get; }        
        ValueType ValueType { get; }
        string? PropertyName { get; }
        TResult Execute<TResult>(Element element, IPropertyFilterExecutor<TResult> executor);
        object? GetValue();
    }

}
