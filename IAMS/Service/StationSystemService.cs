using Dapper;
using IAMS.Models.PriceTemplate;
using IAMS.Models.StationSystem;
using MySql.Data.MySqlClient;
using System.Data;

namespace IAMS.Service {
    public class StationSystemService : IStationSystemService {
        private string _connectionString;
        public StationSystemService(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("gq");
        }
        public EnergyStorageStackControlInfo GetEnergyStorageStackControlInfo(string sn) {
            EnergyStorageStackControlInfo ret = new EnergyStorageStackControlInfo() {
                Sn = sn
            };

            try {
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    // 定义 SQL 查询
                    string query = "SELECT " +
                        "id,name,device_type,device_id,sn,is_active,daily_charge_capacity,daily_discharge_capacity,total_charge_capacity," +
                        "total_discharge_capacity,soc,max_charge_power,max_discharge_power,max_load_power,upload_time,power_station_id " +
                        "FROM " +
                        "device_energy_storage_stack_control_info " +
                        "WHERE sn=@sn " +
                        "order by upload_time desc " +
                        "LIMIT 1 ";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
                        cmd.Parameters.AddWithValue("@sn", sn);
                        var read = cmd.ExecuteReader();
                        while (read.Read()) {
                            ret.Id = read.GetInt32("id");
                            ret.DevName = read.GetString("name");
                            ret.DevId = read.GetString("device_id");
                            ret.DeviceOnline = read.GetBoolean("is_active");
                            ret.DailyChargeCapacity = read.GetDouble("daily_charge_capacity");
                            ret.DailyDischargeCapacity = read.GetDouble("daily_discharge_capacity");
                            ret.CumulativeChargeCapacity = read.GetDouble("total_charge_capacity");
                            ret.CumulativeDischargeCapacity = read.GetDouble("total_discharge_capacity");
                            ret.StateOfCharge = read.GetDouble("soc");
                            ret.MaximumAllowedChargePower = read.GetDouble("max_charge_power");
                            ret.MaximumAllowedDischargePower = read.GetDouble("max_discharge_power");
                            ret.UploadTime = read.GetDateTime("upload_time");
                            ret.PowerStationId = read.GetInt32("power_station_id");
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return ret;
        }

        public bool SaveEnergyStorageStackControlInfo(List<EnergyStorageStackControlInfo> energyStorageStackControlInfos) {
            if (energyStorageStackControlInfos == null || energyStorageStackControlInfos.Count == 0) {
                return false;
            }

            try {
                using (var connection = new MySqlConnection(_connectionString)) {
                    connection.Open();
                    const string insertQuery = @"
                        INSERT INTO EnergyStorageStackControlInfo (
upload_time,dev_type,dev_name,dev_id,sn,is_in_use,is_active
                            daily_charge_capacity, daily_discharge_capacity, total_charge_capacity, total_discharge_capacity,
                            soc, max_charge_power, max_discharge_power, max_load_power, total_voltage, max_voltage,
                            min_voltage, total_current, soh, soe, remaining_charge, remaining_discharge,
                            average_temperature, average_voltage, insulation, positive_insulation, negative_insulation,
                            max_temperature, min_temperature
                        )
                        VALUES (
@UploadTime,@DevType,@DevName,@DevId,@Sn,@IsInUse,@IsActive,
                            @DailyChargeCapacity, @DailyDischargeCapacity, @TotalChargeCapacity, @TotalDischargeCapacity,
                            @SOC, @MaxChargePower, @MaxDischargePower, @MaxLoadPower, @TotalVoltage, @MaxVoltage,
                            @MinVoltage, @TotalCurrent, @SOH, @SOE, @RemainingCharge, @RemainingDischarge,
                            @AverageTemperature, @AverageVoltage, @Insulation, @PositiveInsulation, @NegativeInsulation,
                            @MaxTemperature, @MinTemperature
                        );";
                    using (var transaction = connection.BeginTransaction()) {
                        connection.Execute(insertQuery, energyStorageStackControlInfos, transaction: transaction);
                        transaction.Commit();
                    }
                    return true;
                }
            } catch (Exception ex) {
                return false;
            }

        }


    }
}
