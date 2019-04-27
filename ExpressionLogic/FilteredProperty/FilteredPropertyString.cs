
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExpressionLogic.FilteredProperty
{
    public class FilteredPropertyString : FilteredPropertyBase<string>, IComplexFilteredProperty
    {
        public FilteredPropertyString(string fieldName) : base(fieldName)
        {
            AvailableActions.Add(CompareAction.Equal);
            AvailableActions.Add(CompareAction.Contains);
            AvailableActions.Add(CompareAction.StartsWith);
            AvailableActions.Add(CompareAction.EndsWith);
        }

        public List<PropertyInfo> GetProperties()
        {
            return new List<PropertyInfo> { typeof(string).GetProperty(nameof(string.Length)) };
        }
    }
}
