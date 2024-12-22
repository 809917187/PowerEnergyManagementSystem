namespace IAMS.Models.PriceTemplate {



    public class PriceTemplateInfo {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; }
        public int CreaterId { get; set; }
        public string CreaterName { get; set; }
        public List<TimeFrameInfo> timeFrameInfos { get; set; } = new List<TimeFrameInfo>();
        public Dictionary<int, decimal> TimeFrame2BuyPrice { get; set; } = new Dictionary<int, decimal>();
        public Dictionary<int, decimal> TimeFrame2SalePrice { get; set; } = new Dictionary<int, decimal>();

        public Dictionary<int, string> TimeFrameTypeCode2Name { get;  }=new Dictionary<int, string> {
            { 1,"尖"},{ 2,"峰"},{ 3,"平"},{ 4,"谷"},{ 5,"深谷"}
        };

        public PriceTemplateInfo() {
            foreach (var map in this.TimeFrameTypeCode2Name) {
                TimeFrame2BuyPrice.Add(map.Key, 0);
                TimeFrame2SalePrice.Add(map.Key, 0);
            }
        }
    }
    public class TimeFrameInfo {
        public TimeSpan StartTime { get; set; }
        public string StartTimeStr { get; set; }
        public TimeSpan EndTime { get; set; }
        public string EndTimeStr { get; set; }
        public int TimeFrameType { get; set; }//尖1,峰2,平3,谷4,深谷5
        public string TimeFrameTypeName { get; set; }//尖1,峰2,平3,谷4,深谷5
    }




}
