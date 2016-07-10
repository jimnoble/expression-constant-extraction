using System;
using System.Linq.Expressions;

namespace ExpressionInvariantExtraction.Interface
{
    public interface IInvariantExtractor
    {
        TObject ExtractInvariants<TObject>(
            Expression<Func<TObject, bool>> inputExpression,
            out Expression<Func<TObject, bool>> invariantExpression)
            where TObject : new();
    }
}
