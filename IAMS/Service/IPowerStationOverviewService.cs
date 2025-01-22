using IAMS.Common;
using IAMS.ViewModels.PowerStationOverview;

namespace IAMS.Service {
    public interface IPowerStationOverviewService {
        public PowerStationOverviewViewModel GetPowerStationOverviewViewModel(int id);
        public SeriesData GetEarnSummaryData(int powerStationId,DateTime startDate = default, DateTime endDate = default);
    }
}
