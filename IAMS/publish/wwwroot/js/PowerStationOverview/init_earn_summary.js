function loadEarnChart(InitEarnChart) {
    $.ajax({
        url: InitEarnChart,
        type: 'GET',            // 请求方式为 GET
        success: function (response) {  // 请求成功时的回调函数
            // 假设 response.data 是你通过类似 List<object[]> 得到的日期和功率数据
            var chartData = response.data; // 例如 [{ date: "2023-01-01", value: 100 }, ...]

            // 提取 xAxis（时间）和 yAxis（功率）的数据
            var times = chartData.map(item => item[0]);  // 只提取时间
            var values = chartData.map(item => item[1]); // 只提取功率（decimal）

            // ECharts 配置
            var chartDom = document.getElementById('earn_chart');
            var myChart = echarts.init(chartDom);

            var option = {
                title: {
                    text: '收益统计',
                    left: 'center'
                },
                tooltip: {
                    trigger: 'axis',
                    formatter: function (params) {
                        return params
                            .map(
                                (param) => `${param.data}`  // 只显示数值，不显示时间或其他信息
                            )
                            .join('<br>');
                    }
                },
                legend: {
                    top: 'top', // 图例显示在顶部
                    left: 'left', // 居中对齐
                    data: ['收益统计'] // 图例名称
                },
                xAxis: {
                    type: 'category', // 横轴为类目轴
                    name: '时间',
                    data: times, // 时间数据
                    axisLabel: {
                        formatter: function (value) {
                            // 格式化时间，只显示年月日
                            var date = new Date(value);
                            var year = date.getFullYear();
                            var month = ('0' + (date.getMonth() + 1)).slice(-2);  // 补零
                            var day = ('0' + date.getDate()).slice(-2);  // 补零
                            return `${year}-${month}-${day}`;  // 输出格式为 "YYYY-MM-DD"
                        }
                    }
                },
                yAxis: {
                    type: 'value',
                    name: '元'
                },
                series: [
                    {
                        name: '收益统计', // 系列名称
                        type: 'bar', // 设置为柱状图
                        data: values,  // 功率数据
                    }
                ]
            };

            myChart.setOption(option);
        },
        error: function (xhr, status, error) {  // 请求失败时的回调函数
            alert('获取数据失败，状态码: ' + status + ' 错误信息: ' + error);
        }
    });
}
