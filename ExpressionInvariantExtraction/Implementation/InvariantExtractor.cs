using ExpressionInvariantExtraction.Extensions;
using ExpressionInvariantExtraction.Interface;
using Object.Build.Implementation;
using System;
using System.Linq.Expressions;

namespace ExpressionInvariantExtraction.Implementation
{
    public class InvariantExtractor : IInvariantExtractor
    {
        public TObject ExtractInvariants<TObject>(
            Expression<Func<TObject, bool>> inputExpression, 
            out Expression<Func<TObject, bool>> invariantExpression,
            TObject cloneSource = default(TObject))
        {
            var visitor = new InvariantFindingExpressionVisitor();

            invariantExpression = visitor
                .Visit(inputExpression) 
                as Expression<Func<TObject, bool>>;

            var invariantObjectBuilder = cloneSource.IsDefaultValue() ?
                new Builder<TObject>() :
                new Builder<TObject>(cloneSource);

            foreach (var memberValue in visitor.MemberValues)
            {
                var propertyInfo = memberValue.Item1;

                var value = memberValue.Item2;

                invariantObjectBuilder.Set(propertyInfo.Name, value);
            }

            return invariantObjectBuilder.Build();
        }
    }
}
