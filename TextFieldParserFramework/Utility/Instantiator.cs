using System;
using System.Linq;
using System.Runtime.Serialization;

namespace TextFieldParserFramework.Utility
{
    public class Instantiator
    {
        public static T GetInstanceOf<T>()
            => typeof(T).GetConstructors().Any(x => x.GetParameters().Length == 0)
             ? Activator.CreateInstance<T>()
             : (T)FormatterServices.GetUninitializedObject(typeof(T));

        public static void GetInstanceOf<T>(out T t)
            => t = typeof(T).GetConstructors().Any(x => x.GetParameters().Length == 0)
             ? Activator.CreateInstance<T>()
             : (T)FormatterServices.GetUninitializedObject(typeof(T));
    }
}
