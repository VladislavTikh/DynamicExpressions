using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionLogic.FilteredProperty
{
    public interface IComplexFilteredProperty
    {
        List<PropertyInfo> GetProperties();
    }
}
