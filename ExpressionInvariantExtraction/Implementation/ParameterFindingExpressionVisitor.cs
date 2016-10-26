using System.Linq.Expressions;

namespace ExpressionInvariantExtraction.Implementation
{
    class ParameterFindingExpressionVisitor : ExpressionVisitor
    {
        ParameterExpression _parameter;

        public ParameterExpression FindFirstParameter(Expression expression)
        {
            var result = Visit(expression);

            return _parameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if(_parameter == null)
            {
                _parameter = node;
            }
            
            return base.VisitParameter(node);
        }
    }
}
