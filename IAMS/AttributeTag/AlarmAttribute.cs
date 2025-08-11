namespace IAMS.AttributeTag {
    [AttributeUsage(AttributeTargets.Property)]
    public class AlarmAttribute : Attribute {
        public int Index { get; }
        public AlarmAttribute(int index) {
            Index = index;
        }
    }
}
