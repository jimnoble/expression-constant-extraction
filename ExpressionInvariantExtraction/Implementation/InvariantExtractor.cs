using ExpressionInvariantExtraction.Interface;
using System;
using System.Linq.Expressions;

namespace ExpressionInvariantExtraction.Implementation
{
    public class InvariantExtractor : IInvariantExtractor
    {
        public TObject ExtractInvariants<TObject>(
            Expression<Func<TObject, bool>> inputExpression, 
            out Expression<Func<TObject, bool>> invariantExpression)
            where TObject : new()
        {
            var visitor = new InvariantFindingExpressionVisitor();

            invariantExpression = visitor
                .Visit(inputExpression) 
                as Expression<Func<TObject, bool>>;

            var invariantObject = new TObject();

            foreach(var memberValue in visitor.MemberValues)
            {
                var propertyInfo = memberValue.Item1;

                propertyInfo.SetValue(
                    invariantObject, 
                    memberValue.Item2);
            }

            return invariantObject;
        }
    }
}
