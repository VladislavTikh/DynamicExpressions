using ExpressionLogic.FilteredProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionLogic
{
    public class ChainCreator
    {
        private FilteredPropertyBuilder builder;
        public ChainCreator(FilteredPropertyBuilder _builder) => builder = _builder;

        public  void CreateChain(Prediction prediciton, IFilteredProperty filteredField, PropertyInfo propertyInfo)
        {
            var newField = builder.Build(propertyInfo.PropertyType);
            prediciton.nestedProperty = propertyInfo.Name;
            filteredField.AvailableActions = newField.AvailableActions;
            filteredField.FieldType = newField.FieldType;
        }
    }
}
