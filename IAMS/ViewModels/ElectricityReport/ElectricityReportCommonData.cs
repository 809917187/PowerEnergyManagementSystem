namespace IAMS.ViewModels.ElectricityReport {
    public class ElectricityReportCommonData {
        public double PeakForwardActiveEnergy { get; set; } = 0;
        public double PeakReverseActiveEnergy { get; set; } = 0;
        public double FlatForwardActiveEnergy { get; set; } = 0;
        public double FlatReverseActiveEnergy { get; set; } = 0;
        public double NormalForwardActiveEnergy { get; set; } = 0;
        public double NormalReverseActiveEnergy { get; set; } = 0;
        public double ValleyForwardActiveEnergy { get; set; } = 0;
        public double ValleyReverseActiveEnergy { get; set; } = 0;
        public double TotalForwardActiveEnergy {
            get {
                return PeakForwardActiveEnergy + FlatForwardActiveEnergy + NormalForwardActiveEnergy + ValleyForwardActiveEnergy;
            }
        }
        public double TotalReverseActiveEnergy {
            get {
                return PeakReverseActiveEnergy + FlatReverseActiveEnergy + NormalReverseActiveEnergy + ValleyReverseActiveEnergy;
            }
        }
        public double Efficiency {
            get {

                if (TotalForwardActiveEnergy == 0) {
                    return 0;
                } else {
                    return Math.Round(TotalReverseActiveEnergy / TotalForwardActiveEnergy, 2);
                }
            }
        }
    }
}
