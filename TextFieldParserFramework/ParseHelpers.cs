using System;
using System.Runtime.CompilerServices;

namespace TextFieldParserFramework
{
    public static class ParseHelpers
    {
        public static void SetValue<T>(ref T t, object value, Action<object, object> setValue)
        {
            if (t.GetType().IsValueType)
            {
                var box = RuntimeHelpers.GetObjectValue(t);
                setValue(box, value);
                t = (T)box;
                return;
            }
            setValue(t, value);
        }

        //public static void SetValue<T>(ref T t, FieldInfo fieldInfo, object propertyValue)
        //{
        //    if (t.GetType().IsValueType)
        //    {
        //        var box = RuntimeHelpers.GetObjectValue(t);
        //        fieldInfo.SetValue(box, propertyValue);
        //        t = (T)box;
        //        return;
        //    }
        //    fieldInfo.SetValue(t, propertyValue);
        //}
    }
}
