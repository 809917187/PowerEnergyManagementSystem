namespace IAMS.AttributeTag {
    [AttributeUsage(AttributeTargets.Property)]
    public class PointIndexAttribute : Attribute {
        public int Index { get; }

        public PointIndexAttribute(int index) {
            Index = index;
        }
    }
}
