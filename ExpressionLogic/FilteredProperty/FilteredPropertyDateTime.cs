using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ExpressionLogic.FilteredProperty
{
    public class FilteredPropertyDateTime : FilteredPropertyBase<DateTime>, IComplexFilteredProperty
    {
        public FilteredPropertyDateTime(string filedName) : base(filedName)
        {
            AvailableActions.Add(CompareAction.Adult);
            AvailableActions.Add(CompareAction.Teenager);
        }

        public List<PropertyInfo> GetProperties()
        {
            return new List<PropertyInfo> { typeof(DateTime).GetProperty(nameof(DateTime.Year))
            ,typeof(DateTime).GetProperty(nameof(DateTime.Month))
            ,typeof(DateTime).GetProperty(nameof(DateTime.Day))};
        }
    }
}
