namespace IAMS.AttributeTag {
    [AttributeUsage(AttributeTargets.Property)]
    public class PointRangeAttribute : Attribute {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public PointRangeAttribute(int start, int end) {
            StartIndex = start;
            EndIndex = end;
        }
    }
}
