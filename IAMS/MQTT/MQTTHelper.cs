using ClickHouse.Client.Copy;
using Dapper;
using IAMS.Models.DeviceInfo;
using IAMS.Models.EmsControl;
using IAMS.MQTT.Model;
using IAMS.Service;
using MQTTnet.Client;
using MQTTnet;
using MySql.Data.MySqlClient;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text;
using JsonSerializer = System.Text.Json.JsonSerializer;
using MySqlX.XDevAPI;
using System;

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


		private TaskCompletionSource<TestModeModel> _completionSource;
		private IMqttClient _mqttClient;
		private string _read_mode_topic;
		private string _reply_mode_topic;
		public async Task<TestModeModel> GetTestModeModel(string sn) {
			int UUID = new Random().Next(10000, 100000);
			var obj = new {
				transaction = UUID,
				timeStamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
				runMode = 0
			};
			string json = JsonSerializer.Serialize(obj);
			string reply = await this.GetReplyDataStr(sn, json, UUID, 1);
			TestModeModel ret = JsonSerializer.Deserialize<TestModeModel>(reply);
			ret.sn = sn;
			return ret;
		}
		public async Task<TestModeModel> SendTestModeModel(TestModeModel model) {

			int UUID = new Random().Next(10000, 100000);
			model.transaction = UUID;
			string json = System.Text.Json.JsonSerializer.Serialize(model);
			string reply = await this.GetReplyDataStr(model.sn, json, UUID, 2);
			TestModeModel ret = JsonSerializer.Deserialize<TestModeModel>(reply);
			return ret;
		}

		public async Task<PvStorageModel> SendPvStorageModel(PvStorageModel model) {

			int UUID = new Random().Next(10000, 100000);
			model.transaction = UUID;
			string json = System.Text.Json.JsonSerializer.Serialize(model);
			string reply = await this.GetReplyDataStr(model.sn, json, UUID, 2);
			PvStorageModel ret = JsonSerializer.Deserialize<PvStorageModel>(reply);
			return ret;
		}
		public async Task<PowerUsageModel> SendPowerUsageModel(PowerUsageModel model) {

			int UUID = new Random().Next(10000, 100000);
			model.transaction = UUID;
			string json = System.Text.Json.JsonSerializer.Serialize(model);
			string reply = await this.GetReplyDataStr(model.sn, json, UUID, 2);
			PowerUsageModel ret = JsonSerializer.Deserialize<PowerUsageModel>(reply);
			return ret;
		}

		public async Task<ProtectSettingModel> SendProtectSettingModel(ProtectSettingModel model) {
			int UUID = new Random().Next(10000, 100000);
			model.transaction = UUID;
			string json = System.Text.Json.JsonSerializer.Serialize(model);
			string reply = await this.GetReplyDataStr(model.sn, json, UUID, 2);
			ProtectSettingModel ret = JsonSerializer.Deserialize<ProtectSettingModel>(reply);
			return ret;
		}


		public async Task<PowerUsageModel> GetPowerUsageModel(string sn) {
			int UUID = new Random().Next(10000, 100000);
			var obj = new {
				transaction = UUID,
				timeStamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
				runMode = 1
			};
			string json = JsonSerializer.Serialize(obj);
			string reply = await this.GetReplyDataStr(sn, json, UUID, 1);
			PowerUsageModel ret = JsonSerializer.Deserialize<PowerUsageModel>(reply);
			ret.sn = sn;
			return ret;
		}

		public async Task<PvStorageModel> GetPvStorageModel(string sn) {
			int UUID = new Random().Next(10000, 100000);
			var obj = new {
				transaction = UUID,
				timeStamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
				runMode = 2
			};
			string json = JsonSerializer.Serialize(obj);
			string reply = await this.GetReplyDataStr(sn, json, UUID, 1);
			PvStorageModel ret = JsonSerializer.Deserialize<PvStorageModel>(reply);
			ret.sn = sn;
			return ret;
		}

		public async Task<ProtectSettingModel> GetProtectSettingModel(string sn) {
			int UUID = new Random().Next(10000, 100000);
			var obj = new {
				transaction = UUID,
				timeStamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
				runMode = 100
			};
			string json = JsonSerializer.Serialize(obj);
			string reply = await this.GetReplyDataStr(sn, json, UUID, 1);
			ProtectSettingModel ret = JsonSerializer.Deserialize<ProtectSettingModel>(reply);
			ret.sn = sn;
			return ret;
		}

		/*topicCode：1==read,2==control*/
		private async Task<string> GetReplyDataStr(string sn, string jsonContent, int UUID, int topicCode) {
			if (topicCode == 1) {
				_read_mode_topic = $"bluesun/ems/read/{sn}";
				_reply_mode_topic = $"bluesun/ems/read/reply/{sn}";
			} else if (topicCode == 2) {
				_read_mode_topic = $"bluesun/ems/control/{sn}";
				_reply_mode_topic = $"bluesun/ems/control/reply/{sn}";
			}


			var factory = new MqttFactory();
			_mqttClient = factory.CreateMqttClient();

			var options = new MqttClientOptionsBuilder()
				.WithTcpServer(MqttSubscribeService._host, MqttSubscribeService._port)
				.WithCredentials(MqttSubscribeService.username, MqttSubscribeService.password)
				.WithKeepAlivePeriod(TimeSpan.FromSeconds(30))
				.WithClientId("MqttClient_" + Guid.NewGuid().ToString("N").Substring(0, 8))
				.Build();

			var tcs = new TaskCompletionSource<string>();

			Func<MqttApplicationMessageReceivedEventArgs, Task> handler = e => {
				if (e.ApplicationMessage.Topic == _reply_mode_topic) {
					string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
					if (payload.Contains(UUID.ToString())) {
						tcs.TrySetResult(payload);
					}
				}
				return Task.CompletedTask;
			};

			_mqttClient.ApplicationMessageReceivedAsync += handler;

			await _mqttClient.ConnectAsync(options);
			await _mqttClient.SubscribeAsync(_reply_mode_topic);

			var message = new MqttApplicationMessageBuilder()
				.WithTopic(_read_mode_topic)
				.WithPayload(jsonContent)
				.WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
				.Build();

			await _mqttClient.PublishAsync(message);

			var timeoutTask = Task.Delay(10000);
			var finished = await Task.WhenAny(tcs.Task, timeoutTask);

			_mqttClient.ApplicationMessageReceivedAsync -= handler;

			if (finished == timeoutTask)
				return null; // 超时

			string reply = await tcs.Task;

			return reply;
		}
	}


}
