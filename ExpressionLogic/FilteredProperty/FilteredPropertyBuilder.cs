using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionLogic.FilteredProperty
{
    public class FilteredPropertyBuilder
    {
        public Type BaseType { get; set; }

        public FilteredPropertyBuilder(Type baseType)
        {
            BaseType = baseType;
        }

        public IFilteredProperty Build(string propertyName)
        {
            var property = BaseType.GetProperty(propertyName);
            var propertyType = property.PropertyType;

            switch (Type.GetTypeCode(propertyType))
            {
                case TypeCode.Int32:
                    return new FilteredPropertyInt(propertyName);
                case TypeCode.String:
                    return new FilteredPropertyString(propertyName);
                case TypeCode.DateTime:
                    return new FilteredPropertyDateTime(propertyName);
            }

            return null;
        }

        public IFilteredProperty Build(Type propertyType)
        {
            var propertyName = propertyType.Name;

            switch (Type.GetTypeCode(propertyType))
            {
                case TypeCode.Int32:
                    return new FilteredPropertyInt(propertyName);
                case TypeCode.String:
                    return new FilteredPropertyString(propertyName);
            }

            return null;
        }
    }
}
