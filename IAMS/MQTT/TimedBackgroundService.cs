namespace IAMS.MQTT {
    public class TimedBackgroundService : BackgroundService {
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                MQTTHelper.SaveMqttPeriodDataToDB();
                MQTTHelper.SaveRootDataToDBInfo();
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
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
