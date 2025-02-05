using IAMS.ViewModels.ElectricityReport;

namespace IAMS.Service {
    public interface IElectricityReportService {
        public List<ElectricityReportStationSummaryViewModel> GetElectricityReportStationSummaryData(DateTime startDate, DateTime dateTime);
        public ElectricityReportViewModel GetElectricityReportViewModel();
        public Dictionary<DateTime, ElectricityReportByDay> GetSingleStationReportByDayData(int powerStationId, DateTime startDate, DateTime dateTime);
    }
}
