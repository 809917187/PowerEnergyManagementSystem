using Dapper;
using IAMS.Models.PriceTemplate;
using IAMS.Models.StationSystem;
using MySql.Data.MySqlClient;

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
                    using (MySqlCommand cmd = new MySqlCommand(query,connection)) {
                        cmd.Parameters.AddWithValue("@sn",sn);
                        var read = cmd.ExecuteReader();
                        while (read.Read()) {
                            ret.Id = read.GetInt32("id");
                            ret.DevName = read.GetString("name");
                            ret.DevId = read.GetString("device_id");
                            ret.IsActive = read.GetBoolean("is_active");
                            ret.DailyChargeCapacity = read.GetDouble("daily_charge_capacity");
                            ret.DailyDischargeCapacity = read.GetDouble("daily_discharge_capacity");
                            ret.TotalChargeCapacity = read.GetDouble("total_charge_capacity");
                            ret.TotalDischargeCapacity = read.GetDouble("total_discharge_capacity");
                            ret.SOC = read.GetDouble("soc");
                            ret.MaxChargePower = read.GetDouble("max_charge_power");
                            ret.MaxDischargePower = read.GetDouble("max_discharge_power");
                            ret.MaxLoadPower = read.GetDouble("max_load_power");
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

        public bool SaveEnergyStorageStackControlInfo(EnergyStorageStackControlInfo energyStorageStackControlInfo) {
            throw new NotImplementedException();
        }
    }
}
