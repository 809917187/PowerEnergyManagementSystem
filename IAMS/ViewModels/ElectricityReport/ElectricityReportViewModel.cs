using IAMS.Models.PowerStation;

namespace IAMS.ViewModels.ElectricityReport {
    public class ElectricityReportViewModel {

        public List<PowerStationInfo> powerStationInfos { set; get; } = new List<PowerStationInfo>();  
    }

}
