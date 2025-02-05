using IAMS.Common;
using IAMS.ViewModels.MultiStationOverview;

namespace IAMS.Service {
    public interface IMultiSatationOverviewService {
        public MultiStationOverviewViewModel GetMultiStationOverviewViewModel();
        public SeriesData GetEarnSummaryData(DateTime startDate = default, DateTime endDate = default);
        public SeriesData GetEarnRankData(DateTime startDate = default, DateTime endDate = default);
        public List<SeriesData> GetChargeAdnDischargeChartData(DateTime startDate = default, DateTime endDate = default);
    }
}
