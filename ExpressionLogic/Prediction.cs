using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionLogic
{
    public class Prediction
    {
        public string PropertyName { get; set; }
        public CompareAction CompareAction { get; set; }
        public object RightValue { get; set; }
        public string nestedProperty { get; set; }
        public bool NeedRight()
        {
            return CompareAction == CompareAction.Less
                || CompareAction == CompareAction.More
                || CompareAction == CompareAction.Contains
                || CompareAction == CompareAction.Equal
                || CompareAction == CompareAction.StartsWith
                || CompareAction == CompareAction.EndsWith;
        }
    }
}
