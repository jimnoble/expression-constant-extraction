using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionInvariantExtraction.Implementation
{
    class InvariantFindingExpressionVisitor : ExpressionVisitor
    {
        List<Tuple<PropertyInfo, object>> _propertyValues = 
            new List<Tuple<PropertyInfo, object>>();

        public IReadOnlyList<Tuple<PropertyInfo, object>> MemberValues
        {
            get { return _propertyValues; }
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if(node.NodeType == ExpressionType.Equal)
            {
                // If this binary operator is an equals...

                if (node.Left.NodeType == ExpressionType.MemberAccess)
                {
                    // If the left side is a member access...

                    var memberExpression = node.Left as MemberExpression;

                    var propertyInfo = memberExpression.Member as PropertyInfo;

                    if(propertyInfo != null)
                    {
                        // If the member accessed is a property...

                        if (memberExpression.Expression.NodeType == ExpressionType.Parameter)
                        {
                            // If the expression of the member access is a lambda parameter...

                            var value = EvaluateExpressionValue(node.Right);

                            // Add the property's info and value to the list.

                            _propertyValues.Add(Tuple.Create(
                                propertyInfo,
                                value));

                            // Return a binary equals with a guaranteed constant value on the right.

                            return Expression.MakeBinary(
                                ExpressionType.Equal,
                                node.Left,
                                Expression.Constant(value, node.Right.Type));
                        }
                    }
                }
            }

            // Else business as usual.

            return base.VisitBinary(node);
        }

        static object EvaluateExpressionValue(Expression valueExpression)
        {
            var lambda = Expression.Lambda(valueExpression);

            var compiled = lambda.Compile();

            return compiled.DynamicInvoke();
        }
    }
}
