
using ExpressionLogic.FilteredProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionLogic
{
    public class DynamicFilter<T>
    {
        private ParameterExpression ParameterExpression { get; set; }
        private List<Expression> Predicts { get; set; } = new List<Expression>();

        public DynamicFilter()
        {
            ParameterExpression = Expression.Parameter(typeof(T), "player"); // round =>
        }

        public void AddPredict(Prediction prediction)
        {
            var left = Expression.Property(ParameterExpression, prediction.PropertyName);
            if (prediction.nestedProperty != null)
            {
                left = Expression.Property(left, prediction.nestedProperty);
            }
            var right = Expression.Constant(prediction.RightValue);
            Expression expression = null;

            switch (prediction.CompareAction)
            {
                case CompareAction.Less:
                    expression = Expression.LessThan(left, right);
                    break;
                case CompareAction.More:
                    expression = Expression.GreaterThan(left, right);
                    break;
                case CompareAction.Equal:
                    expression = Expression.Equal(left, right);
                    break;
                case CompareAction.Contains:
                    expression = Expression.IsTrue(StringContains(left, right));
                    break;
                case CompareAction.StartsWith:
                    expression = Expression.IsTrue(StringStartsWith(left, right));
                    break;
                case CompareAction.EndsWith:
                    expression = Expression.IsTrue(StringEndsWith(left, right));
                    break;
                case CompareAction.Adult:
                    expression = Expression.IsTrue(AdultAge(left));
                    break;
                case CompareAction.Teenager:
                    expression = Expression.IsFalse(AdultAge(left));
                    break;
                case CompareAction.Even:
                    expression = Expression.Equal(Expression.Modulo(left, Expression.Constant(2))
                        , Expression.Constant(0));
                    break;
                case CompareAction.Odd:
                    expression = Expression.NotEqual(Expression.Modulo(left, Expression.Constant(2))
                        , Expression.Constant(0));
                    break;
            }

            Predicts.Add(expression);
        }

        private Expression AdultAge(MemberExpression left)
        {
            return Expression.GreaterThan(Expression.Constant(DateTime.Now.Year - 18),
                Expression.Property(left, nameof(DateTime.Year)));
        }

        public Expression<Func<T, bool>> GetLambda()
        {
            var finalPredict = GetDummy();
            foreach (var predict in Predicts)
            {
                finalPredict = Expression.And(finalPredict, predict);
            }

            return Expression.Lambda<Func<T, bool>>(finalPredict, new ParameterExpression[] { ParameterExpression });
        }

        public IQueryable<T> Filter(List<T> list)
        {
            var lambda = GetLambda();
            return list.AsQueryable().Where(lambda);
        }

        private Expression GetDummy()
        {
            var constOne = Expression.Constant(1);
            return Expression.Equal(constOne, constOne);// 1 == 1
        }

        private Expression StringContains(Expression left, Expression right)
        {
            return Expression.Call(left, typeof(string).GetMethod("Contains"), right);
        }

        private Expression StringStartsWith(Expression left, Expression right)
        {
            return Expression.Call(left, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), right);
        }

        private Expression StringEndsWith(Expression left, Expression right)
        {
            return Expression.Call(left, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), right);
        }
    }
}
