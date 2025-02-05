function loadChargeDischargeChart(InitChargeDischargeChart) {
    $.ajax({
        url: InitChargeDischargeChart,
        type: 'GET', // 请求方式为 GET
        success: function (response) { // 请求成功时的回调函数
            var chartData = response; // 假设 response 是一个包含多个 SeriesData 的数组

            // ECharts 配置
            var chartDom = document.getElementById('charge_discharge_chart');
            var myChart = echarts.init(chartDom);

            // 动态构建 series 配置
            var series = chartData.map(function (seriesData) {
                return {
                    name: seriesData.name, // 设置系列名称
                    type: 'bar', // 设置为柱状图
                    data: seriesData.data.map(item => [item[0], item[1]]) // 确保数据格式为 [时间, 数值]
                };
            });

            var option = {
                title: {
                    text: '充放电统计',
                    left: 'center'
                },
                tooltip: {
                    trigger: 'axis',
                    formatter: function (params) {
                        // 提取 X 轴日期
                        var xAxisDate = params[0].axisValue;

                        // 格式化日期为 "YYYY-MM-DD"
                        var date = new Date(xAxisDate);
                        var year = date.getFullYear();
                        var month = ('0' + (date.getMonth() + 1)).slice(-2); // 补零
                        var day = ('0' + date.getDate()).slice(-2); // 补零
                        var formattedDate = `${year}-${month}-${day}`; // 输出格式为 "YYYY-MM-DD"

                        // 构建 Tooltip 显示内容
                        var content = `${formattedDate}<br>`; // 首行显示时间
                        content += params
                            .map(
                                (param) => `${param.seriesName}: ${param.data[1]}` // 显示系列名称和数值
                            )
                            .join('<br>'); // 逐行显示各系列数据

                        return content; // 返回完整内容
                    }
                },
                legend: {
                    top: 'top', // 图例显示在顶部
                    left: 'left', // 居中对齐
                    data: chartData.map(seriesData => seriesData.name) // 图例名称
                },
                xAxis: {
                    type: 'category', // 横轴为时间轴
                    name: '时间',
                    axisLabel: {
                        formatter: function (value) {
                            // 格式化时间，只显示年月日
                            var date = new Date(value);
                            var year = date.getFullYear();
                            var month = ('0' + (date.getMonth() + 1)).slice(-2); // 补零
                            var day = ('0' + date.getDate()).slice(-2); // 补零
                            return `${year}-${month}-${day}`; // 输出格式为 "YYYY-MM-DD"
                        }
                    }
                },
                yAxis: {
                    type: 'value',
                    name: 'kWh' // 纵轴单位为 kWh
                },
                series: series // 多个系列的配置
            };

            myChart.setOption(option);

            // 监听标签页切换事件
            $('#nav-tab button').on('shown.bs.tab', function () {
                setTimeout(function () {
                    myChart.resize(); // 延迟调整图表尺寸
                }, 100); // 确保 DOM 渲染完成后调整
            });
        },
        error: function (xhr, status, error) { // 请求失败时的回调函数
            alert('获取数据失败，状态码: ' + status + ' 错误信息: ' + error);
        }
    });
}
