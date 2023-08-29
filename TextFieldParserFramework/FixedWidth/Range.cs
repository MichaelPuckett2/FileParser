namespace TextFieldParserFramework.FixedWidth
{
    public struct Range
    {
        public int Index { get; set; }
        public int Length { get; set; }

        public Range(int index, int length)
        {
            Index = index;
            Length = length;
        }

        public override bool Equals(object obj)
        {
            return obj is Range other &&
                   Index == other.Index &&
                   Length == other.Length;
        }

        public override int GetHashCode()
        {
            int hashCode = 1293871672;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + Length.GetHashCode();
            return hashCode;
        }

        public void Deconstruct(out int index, out int length)
        {
            index = Index;
            length = Length;
        }
    }
}
