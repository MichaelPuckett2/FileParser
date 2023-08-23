using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using TripleG3.ExpressionExtensions;

namespace TextFieldParser;

public static class Extensions
{
    public static bool TrySetPropertyFromString<TItem>(this TItem item, PropertyInfo propertyInfo, string stringValue)
        where TItem : notnull
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
        where TItem : notnull
    {
        var propertyInfo = item.GetType().GetProperty(propertyName);
        if (propertyInfo == null) return false;
        return item.TrySetPropertyFromString(propertyInfo, stringValue);
    }

    public static bool TrySetPropertyFromString<TItem>(this TItem item, Expression<Func<TItem, object>> getPropertyName, string stringValue)
    where TItem : notnull
    {
        var propertyName = getPropertyName.GetMemberName();
        return item.TrySetPropertyFromString(propertyName, stringValue);
    }

    public static bool TryGetStringFromProperty<TItem>(this TItem item, PropertyInfo propertyInfo, out string stringValue)
    where TItem : notnull
    {
        try
        {
            var nullableStringValue = propertyInfo.PropertyType == typeof(string)
                                    ? (string?)propertyInfo.GetValue(item)
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
        where TItem : notnull
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
    where TItem : notnull
    {
        var propertyName = getPropertyName.GetMemberName();
        return item.TryGetStringFromProperty(propertyName, out stringValue);
    }
}
