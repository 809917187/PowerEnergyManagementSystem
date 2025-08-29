using ClickHouse.Client.Copy;
using Dapper;
using IAMS.Models.DeviceInfo;
using IAMS.MQTT.Model;
using IAMS.Service;
using MySql.Data.MySqlClient;
using System.Text.Json;

namespace IAMS.MQTT {
    public class MQTTHelper {
        private static string _connectionString_clickhouse;
        private static string _connectionString_mysql;
        public static void SetConnectionString(string connectionString_mysql, string connectionString_clickhouse) {
            _connectionString_mysql = connectionString_mysql;
            _connectionString_clickhouse = connectionString_clickhouse;
        }



        public static bool SaveMqttPeriodDataToDB(string json) {
            try {
                //string json = MQTTHelper.GetPeriodData(fileName);
                var rootObject = JsonSerializer.Deserialize<DeviceDataFromMqtt>(json);
                if (rootObject != null) {
                    foreach (var devData in rootObject.devData) {
                        devData.sn = devData.sn + "_" + rootObject.emsSn;
                        if (DeviceStaticInfo.devType2DbTableAndPointLength.ContainsKey(devData.devType)) {
                            int dataLength = DeviceStaticInfo.devType2DbTableAndPointLength[devData.devType].Item2;
                            string targetDbTable = DeviceStaticInfo.devType2DbTableAndPointLength[devData.devType].Item1;

                            //DateTime UploadTime = DateTimeOffset.FromUnixTimeSeconds(rootObject.timeStamp).LocalDateTime;
                            DateTime UploadTime = DateTime.Now;
                            MQTTHelper.SaveBatteryClusterInfoAsync(devData, UploadTime, targetDbTable, dataLength);
                            MQTTHelper.SaveDeviceEmsBindingInfo(devData.sn, rootObject.emsSn, devData.devType);
                        }

                    }

                }


            } catch (Exception e) {
                return false;
            }

            return true;
        }

        public static bool SaveDeviceEmsBindingInfo(string devSn, string emsSn, int devType) {
            try {
                using (var connection = new MySqlConnection(_connectionString_mysql)) {
                    connection.Open();
                    const string sql = "" +
                        "INSERT INTO device_ems_ps_binding_info (device_sn,ems_sn,device_type) " +
                        "VALUES (@device_sn,@ems_sn,@device_type) " +
                        "ON DUPLICATE KEY UPDATE " +
                        "ems_sn = @ems_sn";
                    using (MySqlCommand cmd = new MySqlCommand(sql, connection)) {
                        cmd.Parameters.AddWithValue("@device_sn", devSn);
                        cmd.Parameters.AddWithValue("@ems_sn", emsSn);
                        cmd.Parameters.AddWithValue("@device_type", devType);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public static async Task<bool> SaveBatteryClusterInfoAsync(devData info, DateTime UploadTime, string targetDbTable, int dataLength) {
            if (info == null) {
                return false;
            }
            try {
                using var bulkCopyInterface = new ClickHouseBulkCopy(_connectionString_clickhouse) {
                    DestinationTableName = targetDbTable,
                    BatchSize = 100000
                };
                await bulkCopyInterface.InitAsync();
                List<object[]> input = new List<object[]>();
                float[] data = new float[dataLength];
                int index;
                foreach (var vk in info.data) {
                    if (vk.Key.Contains("_") && int.TryParse(vk.Key.Split('_')[1], out index)) {
                        data[index] = vk.Value;
                    }
                }
                input.Add(new object[] {
                    info.sn,
                    UploadTime,
                    info.devType,
                    info.devName,
                    info.devId,
                    data
                });

                await bulkCopyInterface.WriteToServerAsync(input);

                return true;
            } catch (Exception e) {
                return false;
            }

        }





        private static string GetDataKeyInMqtt(string deviceName, int num) {
            return deviceName + "_" + num;
        }

        public static string GetPeriodData(string fileName) {
            string filePath = Path.Combine(AppContext.BaseDirectory, "Assets", "JsonFile", fileName);
            // 读取文件内容
            if (File.Exists(filePath)) {
                return File.ReadAllText(filePath).Replace("\r\n", "").Replace("\n", ""); ;
            } else {
                return String.Empty;
            }
        }

        public static string GetRootData(string rootPath) {
            // 读取文件内容
            if (File.Exists(rootPath)) {
                return File.ReadAllText(rootPath).Replace("\r\n", "").Replace("\n", ""); ;
            } else {
                return String.Empty;
            }
        }
    }


}
