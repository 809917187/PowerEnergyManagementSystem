namespace IAMS.MQTT {
    public class TimedBackgroundService : BackgroundService {
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                /*MQTTHelper.SaveMqttPeriodDataToDB("1.json");
                MQTTHelper.SaveMqttPeriodDataToDB("3.json");
                MQTTHelper.SaveMqttPeriodDataToDB("5.json");*/
                await Task.Delay(TimeSpan.FromSeconds(8), stoppingToken);
            }
        }

        private Task DoWorkAsync() {
            // 在这里实现你的任务逻辑
            Console.WriteLine("执行任务...");
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken stoppingToken) {
            Console.WriteLine("服务已停止");
            return base.StopAsync(stoppingToken);
        }
    }
}
