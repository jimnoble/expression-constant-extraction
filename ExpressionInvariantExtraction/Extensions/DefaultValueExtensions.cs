using System;

namespace ExpressionInvariantExtraction.Extensions
{
    public static class DefaultValueExtensions
    {
        public static bool IsDefaultValue<TValue>(this TValue argument)
        {
            // Normal scenarios

            if (argument == null) return true;

            if (object.Equals(argument, default(TValue))) return true;

            // Non-null nullables

            Type methodType = typeof(TValue);

            if (Nullable.GetUnderlyingType(methodType) != null) return false;

            // Boxed value types

            Type argumentType = argument.GetType();

            if (argumentType.IsValueType && argumentType != methodType)
            {
                var obj = Activator.CreateInstance(argument.GetType());

                return obj.Equals(argument);
            }

            return false;
        }
    }
}
