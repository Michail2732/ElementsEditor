using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ElementsEditor
{	

    public abstract class PropertyFilter<TProperty> : IPropertyFilter
    {
		private readonly TryGetPropertyValueDelegate<TProperty> _propertyExtractor;

        public PropertyFilter(
            TryGetPropertyValueDelegate<TProperty> propertyExtractor,
            string propertyName,
            TProperty? value, Logic logic,
            ConditionOperation operation)
        {
            _propertyExtractor = propertyExtractor;
            Logic = logic;
            Value = value;
            Operation = operation;
            PropertyName = propertyName;
        }

        public Logic Logic { get; }				
		public ConditionOperation  Operation { get; }
        public string PropertyName { get; }
		public TProperty? Value { get; }                

        public bool TryGetPropertyValue(Element element, out TProperty? value)
        {
			return _propertyExtractor(element, out value);            
        }

        public abstract TResult Convert<TResult>(IPropertyFilterConverter<TResult> executor);			   
    }

    public sealed class StringPropertyFilter : PropertyFilter<string>
    {
        public StringPropertyFilter(
            TryGetPropertyValueDelegate<string> propertyExtractor,
            string propertyName, string? value, Logic logic,
            ConditionOperation operation)
            : base(propertyExtractor, propertyName, value, logic, operation)
        {
            if ((int)operation >= 2 && (int)operation <= 5)
                throw new ArgumentException("String filter must have ConditionOperation - {Equals, NotEquals, Contains, StartWith, EndWith}");
        }

        public override TResult Convert<TResult>(IPropertyFilterConverter<TResult> executor)
        {
            return executor.Convert(this);
        }
    }

    public sealed class IntPropertyFilter : PropertyFilter<int>
    {
        public IntPropertyFilter(
            TryGetPropertyValueDelegate<int> propertyExtractor,
            string propertyName, int value, Logic logic,
            ConditionOperation operation)
            : base(propertyExtractor, propertyName, value, logic, operation)
        {
            if ((int)operation >= 6 && (int)operation <= 8)
                throw new ArgumentException("Int filter must have ConditionOperation - {Equals, NotEquals, Large, LargeOrEquals, Less, LessOrEquals,}");
        }

        public override TResult Convert<TResult>(IPropertyFilterConverter<TResult> executor)
        {
            return executor.Convert(this);
        }
    }

    public sealed class DoublePropertyFilter : PropertyFilter<double>
    {
        public DoublePropertyFilter(
            TryGetPropertyValueDelegate<double> propertyExtractor,
            string propertyName, double value, Logic logic,
            ConditionOperation operation)
            : base(propertyExtractor, propertyName, value, logic, operation)
        {
            if ((int)operation >= 6 && (int)operation <= 8)
                throw new ArgumentException("Double filter must have ConditionOperation - {Equals, NotEquals, Large, LargeOrEquals, Less, LessOrEquals,}");
        }

        public override TResult Convert<TResult>(IPropertyFilterConverter<TResult> executor)
        {
            return executor.Convert(this);
        }
    }

    public sealed class DecimalPropertyFilter : PropertyFilter<decimal>
    {
        public DecimalPropertyFilter(
            TryGetPropertyValueDelegate<decimal> propertyExtractor,
            string propertyName, decimal value, Logic logic,
            ConditionOperation operation)
            : base(propertyExtractor, propertyName, value, logic, operation)
        {
            if ((int)operation >= 6 && (int)operation <= 8)
                throw new ArgumentException("Decimal filter must have ConditionOperation - {Equals, NotEquals, Large, LargeOrEquals, Less, LessOrEquals,}");
        }

        public override TResult Convert<TResult>(IPropertyFilterConverter<TResult> executor)
        {
            return executor.Convert(this);
        }
    }

    public sealed class BoolPropertyFilter : PropertyFilter<bool>
    {
        public BoolPropertyFilter(
            TryGetPropertyValueDelegate<bool> propertyExtractor,
            string propertyName, bool value, Logic logic,
            ConditionOperation operation)
            : base(propertyExtractor, propertyName, value, logic, operation)
        {
            if ((int)operation != 0 && (int)operation != 1)
                throw new ArgumentException("Bool filter must have ConditionOperation - {Equals, NotEquals}");
        }

        public override TResult Convert<TResult>(IPropertyFilterConverter<TResult> executor)
        {
            return executor.Convert(this);
        }
    }

    public sealed class DateTimePropertyFilter : PropertyFilter<DateTime>
    {
        public DateTimePropertyFilter(
            TryGetPropertyValueDelegate<DateTime> propertyExtractor,
            string propertyName, DateTime value, Logic logic,
            ConditionOperation operation)
            : base(propertyExtractor, propertyName, value, logic, operation)
        {
            if ((int)operation >= 6 && (int)operation <= 8)
                throw new ArgumentException("DateTime filter must have ConditionOperation - {Equals, NotEquals, Large, LargeOrEquals, Less, LessOrEquals}");
        }

        public override TResult Convert<TResult>(IPropertyFilterConverter<TResult> executor)
        {
            return executor.Convert(this);
        }
    }

}
