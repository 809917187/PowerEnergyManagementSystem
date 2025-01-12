namespace IAMS.Common {
    public class SeriesData {
        public string Name { get; set; }  // series 的名称
        public List<object[]> Data { get; set; }  // 数据点，格式为 [时间, 数值]
    }
}
