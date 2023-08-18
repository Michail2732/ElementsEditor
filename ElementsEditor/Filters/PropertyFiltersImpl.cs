using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ElementsEditor
{
    public sealed class StringPropertyFilter : PropertyFilter<string>
    {
        public StringPropertyFilter(
            TryGetPropertyValueDelegate<string> propertyExtractor,              
            string propertyName,
            string? value = null,
            Logic logic = Logic.Or,
            ConditionOperatioin operation = ConditionOperatioin.Equals, 
            Func<string?, bool>? valueValidator = null,
            IEnumerable<string>? awailableValues = null) 
            : base(propertyExtractor, ValueType.String, propertyName, value, logic, operation, valueValidator, awailableValues)
        {
            if ((int)operation >= 2 && (int)operation <= 5)
                throw new ArgumentException("String filter must have ConditionOperation - {Equals, NotEquals, Contains, StartWith, EndWith}");
        }

        public override TResult Execute<TResult>(Element element, IPropertyFilterExecutor<TResult> executor)
        {
            return executor.Execute(element, this);
        }
    }

    public sealed class IntPropertyFilter : PropertyFilter<int>
    {
        public IntPropertyFilter(
            TryGetPropertyValueDelegate<int> propertyExtractor,
            string propertyName,
            int value = 0,
            Logic logic = Logic.Or,
            ConditionOperatioin operation = ConditionOperatioin.Equals,
            Func<int, bool>? valueValidator = null,
            IEnumerable<int>? awailableValues = null)
            : base(propertyExtractor, ValueType.Integer, propertyName, value, logic, operation, valueValidator, awailableValues)
        {
            if ((int)operation >= 6 && (int)operation <= 8)
                throw new ArgumentException("Int filter must have ConditionOperation - {Equals, NotEquals, Large, LargeOrEquals, Less, LessOrEquals,}");
        }

        public override TResult Execute<TResult>(Element element, IPropertyFilterExecutor<TResult> executor)
        {
            return executor.Execute(element, this);
        }
    }

    public sealed class DoublePropertyFilter : PropertyFilter<double>
    {
        public DoublePropertyFilter(
            TryGetPropertyValueDelegate<double> propertyExtractor,
            string propertyName,
            double value = 0,
            Logic logic = Logic.Or,
            ConditionOperatioin operation = ConditionOperatioin.Equals,
            Func<double, bool>? valueValidator = null,
            IEnumerable<double>? awailableValues = null)
            : base(propertyExtractor, ValueType.Double, propertyName, value, logic, operation, valueValidator, awailableValues)
        {
            if ((int)operation >= 6 && (int)operation <= 8)
                throw new ArgumentException("Double filter must have ConditionOperation - {Equals, NotEquals, Large, LargeOrEquals, Less, LessOrEquals,}");
        }

        public override TResult Execute<TResult>(Element element, IPropertyFilterExecutor<TResult> executor)
        {
            return executor.Execute(element, this);
        }
    }

    public sealed class DecimalPropertyFilter : PropertyFilter<decimal>
    {
        public DecimalPropertyFilter(
            TryGetPropertyValueDelegate<decimal> propertyExtractor,
            string propertyName,
            decimal value = 0,
            Logic logic = Logic.Or,
            ConditionOperatioin operation = ConditionOperatioin.Equals,
            Func<decimal, bool>? valueValidator = null,
            IEnumerable<decimal>? awailableValues = null)
            : base(propertyExtractor, ValueType.Decimal, propertyName, value, logic, operation, valueValidator, awailableValues)
        {
            if ((int)operation >= 6 && (int)operation <= 8)
                throw new ArgumentException("Decimal filter must have ConditionOperation - {Equals, NotEquals, Large, LargeOrEquals, Less, LessOrEquals,}");
        }

        public override TResult Execute<TResult>(Element element, IPropertyFilterExecutor<TResult> executor)
        {
            return executor.Execute(element, this);
        }
    }

    public sealed class BoolPropertyFilter : PropertyFilter<bool>
    {
        public BoolPropertyFilter(
            TryGetPropertyValueDelegate<bool> propertyExtractor,
            string propertyName,
            bool value = false,
            Logic logic = Logic.Or,
            ConditionOperatioin operation = ConditionOperatioin.Equals,
            Func<bool, bool>? valueValidator = null)
            : base(propertyExtractor, ValueType.Boolean, propertyName, value, logic, operation, valueValidator)
        {
            if ((int)operation != 0 && (int)operation != 1)
                throw new ArgumentException("Bool filter must have ConditionOperation - {Equals, NotEquals}");
        }

        public override TResult Execute<TResult>(Element element, IPropertyFilterExecutor<TResult> executor)
        {
            return executor.Execute(element, this);
        }
    }

    public sealed class DateTimePropertyFilter : PropertyFilter<DateTime>
    {
        public DateTimePropertyFilter(
            TryGetPropertyValueDelegate<DateTime> propertyExtractor,
            string propertyName,
            DateTime value = default,
            Logic logic = Logic.Or,
            ConditionOperatioin operation = ConditionOperatioin.Equals,
            Func<DateTime, bool>? valueValidator = null,
            IEnumerable<DateTime>? awailableValues = null)
            : base(propertyExtractor, ValueType.DateTime, propertyName, value, logic, operation, valueValidator, awailableValues)
        {
            if ((int)operation >= 6 && (int)operation <= 8)
                throw new ArgumentException("DateTime filter must have ConditionOperation - {Equals, NotEquals, Large, LargeOrEquals, Less, LessOrEquals}");
        }

        public override TResult Execute<TResult>(Element element, IPropertyFilterExecutor<TResult> executor)
        {
            return executor.Execute(element, this);
        }
    }

    public sealed class CustomPropertyFilter : PropertyFilter<object>
    {
        public CustomPropertyFilter(
            TryGetPropertyValueDelegate<object> propertyExtractor,              
            string propertyName,
            object? value = null,
            Logic logic = Logic.Or,
            ConditionOperatioin operation = ConditionOperatioin.Equals, 
            Func<object?, bool>? valueValidator = null,
            IEnumerable<object>? awailableValues = null) 
            : base(propertyExtractor, ValueType.Custom, propertyName, value, logic, operation, valueValidator, awailableValues)
        {            
        }

        public override TResult Execute<TResult>(Element element, IPropertyFilterExecutor<TResult> executor)
        {
            return executor.Execute(element, this);
        }
    }

}
