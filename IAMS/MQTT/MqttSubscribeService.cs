using IAMS.Models.EmsControl;
using MQTTnet;
using MQTTnet.Client;
using System.Text;

namespace IAMS.MQTT {
    public class MqttSubscribeService: BackgroundService {
        private readonly ILogger<MqttSubscribeService> _logger;
        private IMqttClient _mqttClient;
        private MqttClientOptions _options;

        private string _topic = "bluesun/ems/period/+";
        public static string _host = "47.120.14.45";
		public static int _port = 3011;
		public static string username = "Bluesun";
		public static string password = "Bluesun007";

        public MqttSubscribeService(ILogger<MqttSubscribeService> logger) {
            _logger = logger;
            InitializeMqttClient();

        }

        private void InitializeMqttClient() {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
            _logger.LogWarning("_mqttClient = factory.CreateMqttClient();");

            _options = new MqttClientOptionsBuilder()
                .WithTcpServer(_host, _port) // 改成你的MQTT服务器地址和端口
                .WithCredentials(username, password)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(30))
                .WithClientId("MqttClient_" + Guid.NewGuid().ToString("N").Substring(0, 8))
                .Build();

            _mqttClient.ConnectedAsync += async e => {
                _logger.LogWarning("ConnectedAsync MQTT 准备连接");
                await _mqttClient.SubscribeAsync(_topic);
                _logger.LogWarning("ConnectedAsync 已订阅主题 " + _topic);
            };


            _mqttClient.DisconnectedAsync += async e => {
                _logger.LogWarning("DisconnectedAsync MQTT 已断开");
                await Task.CompletedTask;
            };

            _mqttClient.ApplicationMessageReceivedAsync += e => {
                var topic = e.ApplicationMessage.Topic;
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                _logger.LogWarning("获取到数据" + payload);
                MQTTHelper.SaveMqttPeriodDataToDB(payload);
                return Task.CompletedTask;
            };

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                if (!_mqttClient.IsConnected) {
                    try {
                        _logger.LogWarning($"ExecuteAsync 重新连接");
                        using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, timeoutCts.Token);

                        await _mqttClient.ConnectAsync(_options, linkedCts.Token);
                        _logger.LogInformation("MQTT 连接成功");
                    } catch (Exception ex) {
                        _logger.LogError($"ExecuteAsync MQTT连接失败: {ex.Message}");
                        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                    }
                }

                try {
                    await Task.Delay(5000, stoppingToken);
                } catch (Exception ex) {
                    _logger.LogError("Task.Delay(5000, stoppingToken) 失败");
                }
            }

            if (_mqttClient.IsConnected) {
                _logger.LogError("跳出while await _mqttClient.DisconnectAsync()");
                await _mqttClient.DisconnectAsync();
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken) {
            _logger.LogWarning("MqttSubscribeService 停止中...");
            if (_mqttClient.IsConnected) {
                await _mqttClient.DisconnectAsync();
            }
            await base.StopAsync(cancellationToken);
        }

	}
}
