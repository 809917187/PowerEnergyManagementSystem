namespace IAMS.Models.PowerStation {
    public class BindRequestModel {
        public int powerStationId { get; set; }
        public List<int> cabinetIds { get; set; }
        public List<int> userIds { get; set; }
    }
}
