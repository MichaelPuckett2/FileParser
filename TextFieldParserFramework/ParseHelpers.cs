using System.Reflection;
using System.Runtime.CompilerServices;

namespace TextFieldParserFramework
{
    public static class ParseHelpers
    {
        public static void SetValue<T>(ref T t, PropertyInfo propertyInfo, object propertyValue)
        {
            if (t.GetType().IsValueType)
            {
                var box = RuntimeHelpers.GetObjectValue(t);
                propertyInfo.SetValue(box, propertyValue);
                t = (T)box;
                return;
            }
            propertyInfo.SetValue(t, propertyValue);
        }

        public static void SetValue<T>(ref T t, FieldInfo fieldInfo, object propertyValue)
        {
            if (t.GetType().IsValueType)
            {
                var box = RuntimeHelpers.GetObjectValue(t);
                fieldInfo.SetValue(box, propertyValue);
                t = (T)box;
                return;
            }
            fieldInfo.SetValue(t, propertyValue);
        }
    }
}
