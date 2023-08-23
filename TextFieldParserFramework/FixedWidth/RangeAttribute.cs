using System;

namespace TextFieldParserFramework.FixedWidth
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RangeAttribute : Attribute
    {
        public RangeAttribute(int index, int length)
        {
            Index = index;
            Length = length;
        }
        public int Index { get; }
        public int Length { get; }
    }
}