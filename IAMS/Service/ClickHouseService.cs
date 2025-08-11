using ClickHouse.Client.ADO;
using IAMS.AttributeTag;
using IAMS.Models;
using IAMS.Models.DeviceInfo;
using System.Reflection;

namespace IAMS.Service {
    public class ClickHouseService : IClickHouseService {
        private string _connectionStringClickHouse;
        public ClickHouseService(IConfiguration configuration) {
            _connectionStringClickHouse = configuration.GetConnectionString("ems");
        }

        

        public List<OrignialClickHouseData> GetOrignialClickHouseDatasBySn(int devType, List<string> snList, DateTime startDateTime, DateTime endDateTime = default) {
            if (endDateTime == default) {
                endDateTime = startDateTime;
            }
            List<OrignialClickHouseData> ret = new List<OrignialClickHouseData>();
            try {
                string dbTable = DeviceStaticInfo.devType2DbTableAndPointLength[devType].devName;
                using (ClickHouseConnection connection = new ClickHouseConnection(_connectionStringClickHouse)) {
                    connection.Open();

                    // 定义 SQL 查询
                    using (var command = connection.CreateCommand()) {
                        var snListLiteral = string.Join(",", snList.Select(sn => $"'{sn.Replace("'", "''")}'"));

                        command.CommandText = $@"
                            SELECT sn, upload_time, device_type, device_name, device_id, data 
                            FROM {dbTable} 
                            WHERE sn IN ({snListLiteral})
                                AND upload_time > '{startDateTime:yyyy-MM-dd HH:mm:ss}' 
                                AND upload_time < '{endDateTime.Date.AddDays(1):yyyy-MM-dd HH:mm:ss}'
                            ORDER BY upload_time DESC";

                        using (var reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                ret.Add(new OrignialClickHouseData {
                                    Sn = reader.GetString(reader.GetOrdinal("sn")),
                                    UploadTime = reader.GetDateTime(reader.GetOrdinal("upload_time")),
                                    DeviceType = reader.GetString(reader.GetOrdinal("device_type")),
                                    DeviceName = reader.GetString(reader.GetOrdinal("device_name")),
                                    DeviceId = reader.GetString(reader.GetOrdinal("device_id")),
                                    PointData = (int[])reader.GetValue(reader.GetOrdinal("data"))
                                });
                            }

                        }

                    }

                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return ret;
        }

        public List<PccModel001> GetPccModel001s(List<string> snList, DateTime startDateTime, DateTime endDateTime = default) {
            var o = this.GetOrignialClickHouseDatasBySn(1, snList, startDateTime, endDateTime);
            return this.PraseDeviceInfo<PccModel001>(o);
        }

        public List<BsuModel003> GetBsuModel003s(List<string> snList, DateTime startDateTime, DateTime endDateTime = default) {
            var o = this.GetOrignialClickHouseDatasBySn(3, snList, startDateTime, endDateTime);
            return this.PraseDeviceInfo<BsuModel003>(o);
        }

        public List<PcsModel005> GetPcsModel005s(List<string> snList, DateTime startDateTime, DateTime endDateTime = default) {
            var o = this.GetOrignialClickHouseDatasBySn(5, snList, startDateTime, endDateTime);
            return this.PraseDeviceInfo<PcsModel005>(o);
        }

        public List<T> PraseDeviceInfo<T>(List<OrignialClickHouseData> orignialClickHouseDatas) where T : new() {
            List<T> ret = new List<T>();

            foreach (OrignialClickHouseData orignialClickHouseData in orignialClickHouseDatas) {
                T model = new T();
                SetModelPropertiesByMap(model, orignialClickHouseData.PointData);

                ret.Add(model);
            }

            return ret;
        }

        public void SetModelPropertiesByMap<T>(T model, int[] values) {
            var props = typeof(T).GetProperties();

            foreach (var prop in props) {
                if (Attribute.IsDefined(prop, typeof(NotPointDataAttribute)))
                    continue;

                if (Attribute.IsDefined(prop, typeof(PointRangeAttribute))) {
                    var attrRange = prop.GetCustomAttribute<PointRangeAttribute>();

                    prop.SetValue(model, values.Skip(attrRange.StartIndex).Take(attrRange.EndIndex - attrRange.StartIndex + 1).ToArray());
                }

                var attr = prop.GetCustomAttribute<PointIndexAttribute>();
                if (attr != null && attr.Index < values.Length) {
                    var value = values[attr.Index];
                    if (value != null && prop.CanWrite) {
                        object converted = Convert.ChangeType(value, prop.PropertyType);
                        prop.SetValue(model, converted);
                    }
                }
            }
        }
    }
}
