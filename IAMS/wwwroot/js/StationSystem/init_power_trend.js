$(document).ready(function () {
    $.ajax({
        url: InitPowerTrend,  
        type: 'GET',            // 请求方式为 GET
        success: function (response) {  // 请求成功时的回调函数
            // ECharts 配置
            var chartDom = document.getElementById('power_trend');
            var myChart = echarts.init(chartDom);

            var option = {
                title: {
                    text: '功率趋势',
                    left: 'center'
                },
                tooltip: {
                    trigger: 'axis',
                    formatter: function (params) {
                        return params
                            .map(
                                (param) =>
                                    `${param.seriesName}<br>${param.data[0]}: ${param.data[1]}`
                            )
                            .join('<br>');
                    }
                },
                legend: {
                    top: 'top', // 图例显示在顶部
                    left: 'left', // 居中对齐
                    data: ['电网', '储能'] // 图例名称，需与 series.name 对应
                },
                xAxis: {
                    type: 'time',
                    name: '时间'
                },
                yAxis: {
                    type: 'value',
                    name: '数据量'
                },
                series: [
                    {
                        name: '电网',
                        type: 'line',
                        data: response.chartDataOfElectricGrid,  // 使用获取到的 data1
                        smooth: true
                    },
                    {
                        name: '储能',
                        type: 'line',
                        data: response.chartDataOfEnergyStorage,  // 使用获取到的 data2
                        smooth: true
                    }
                ]
            };

            myChart.setOption(option);
        },
        error: function (xhr, status, error) {  // 请求失败时的回调函数
            alert('获取数据失败，状态码: ' + status + ' 错误信息: ' + error);
        }
    });


});
