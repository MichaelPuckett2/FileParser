using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace TextFieldParserFramework
{
    public static class Extensions
    {
        public static bool TrySetPropertyFromString<TItem>(this TItem item, PropertyInfo propertyInfo, string stringValue)
        {
            try
            {
                var propertyValue = TypeDescriptor.GetConverter(propertyInfo.PropertyType).ConvertFromInvariantString(stringValue);
                propertyInfo.SetValue(item, propertyValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TrySetPropertyFromString<TItem>(this TItem item, string propertyName, string stringValue)
        {
            var propertyInfo = item.GetType().GetProperty(propertyName);
            if (propertyInfo == null) return false;
            return item.TrySetPropertyFromString(propertyInfo, stringValue);
        }

        public static bool TrySetPropertyFromString<TItem>(this TItem item, Expression<Func<TItem, object>> getPropertyName, string stringValue)
        {
            var propertyName = getPropertyName.GetMemberName();
            return item.TrySetPropertyFromString(propertyName, stringValue);
        }

        public static bool TryGetStringFromProperty<TItem>(this TItem item, PropertyInfo propertyInfo, out string stringValue)
        {
            try
            {
                var nullableStringValue = propertyInfo.PropertyType == typeof(string)
                                        ? (string)propertyInfo.GetValue(item)
                                        : TypeDescriptor.GetConverter(propertyInfo.PropertyType).ConvertToInvariantString(item);
                if (nullableStringValue == null)
                {
                    stringValue = string.Empty;
                    return false;
                }
                stringValue = nullableStringValue;
                return true;
            }
            catch
            {
                stringValue = string.Empty;
                return false;
            }
        }

        public static bool TryGetStringFromProperty<TItem>(this TItem item, string propertyName, out string stringValue)
        {
            var propertyInfo = item.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
            {
                stringValue = string.Empty;
                return false;
            }
            return item.TryGetStringFromProperty(propertyInfo, out stringValue);
        }

        public static bool TryGetStringFromProperty<TItem>(this TItem item, Expression<Func<TItem, object>> getPropertyName, out string stringValue)
        {
            var propertyName = getPropertyName.GetMemberName();
            return item.TryGetStringFromProperty(propertyName, out stringValue);
        }

        public static string GetMemberName(this LambdaExpression lambdaExpression)
        {
            MemberExpression memberExpression;
            if (lambdaExpression?.Body is UnaryExpression unaryExpression)
            {
                if (unaryExpression?.Operand is MemberExpression memberExpression2)
                {
                    memberExpression = memberExpression2;
                    goto IL_0054;
                }
            }

            if (lambdaExpression?.Body is MemberExpression memberExpression3)
            {
                memberExpression = memberExpression3;
                goto IL_0054;
            }

            throw new Exception("Could not get member name " + (lambdaExpression.Name ?? string.Empty));
        IL_0054:
            return memberExpression?.Member?.Name;
        }
    }
}
