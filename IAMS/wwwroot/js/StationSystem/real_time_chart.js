$(document).ready(function () {
    $.ajax({
        url: InitRealTimeTrend,
        type: 'GET',  // 请求方式为 GET
        success: function (response) {  // 请求成功时的回调函数
            // ECharts 配置
            var chartDom = document.getElementById('real_time_chart');
            var myChart = echarts.init(chartDom);

            // 动态生成 series 配置
            var seriesData = [];
            var legendData = [];

            response.forEach(function (series) {
                seriesData.push({
                    name: series.name,  // 每个 series 的 name
                    type: 'line',  // 线图类型
                    data: series.data,  // 每个 series 的数据
                    smooth: true  // 平滑曲线
                });
                legendData.push(series.name);  // 收集所有 series 的名称，用于图例显示
            });

            var option = {
                /*title: {
                    text: '实时趋势',
                    left: 'center'
                },*/
                tooltip: {
                    trigger: 'axis',
                    formatter: function (params) {
                        return params
                            .map(function (param) {
                                return `${param.seriesName}<br>${param.data[0]}: ${param.data[1]}`;
                            })
                            .join('<br>');
                    }
                },
                legend: {
                    top: 'top',  // 图例显示在顶部
                    left: 'left',  // 图例左对齐
                    //orient: 'vertical', 
                    data: legendData,  // 图例数据，使用从 response 中获取的 series 名称
                    scroll: true,// 图例项之间的间隔
                    itemWidth: 10,  // 图例项的宽度
                    itemHeight: 10  // 图例项的高度
                    //itemGap: 30,  // 启用图例滚动
                },
                grid: {
                    top: '20%',  // 调整图表的上边距，确保图表和图例之间有间距
                    left: '10%',
                    right: '10%',
                    bottom: '15%'
                },
                xAxis: {
                    type: 'time',
                    name: '时间'
                },
                yAxis: {
                    type: 'value',
                    name: '数据量'
                },
                series: seriesData  // 动态生成的 series 数据
            };

            myChart.setOption(option);
        },
        error: function (xhr, status, error) {  // 请求失败时的回调函数
            alert('获取数据失败，状态码: ' + status + ' 错误信息: ' + error);
        }
    });
});
