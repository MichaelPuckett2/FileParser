namespace TextFieldParser.Delimited;

[AttributeUsage(AttributeTargets.Property)]
public class IndexAttribute : Attribute
{
    public IndexAttribute(int index)
    {
        Index = index;
    }
    public int Index { get; }
}
