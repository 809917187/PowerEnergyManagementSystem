$(document).ready(function () {
   
    $('#pills-tab a').on('shown.bs.tab', function (e) {
        if ($(e.target).attr('href') === '#pills-range') {
            setTimeout(function () {
                myChart.resize(); // 延迟调整图表尺寸
            }, 100);
        }
    });


    // 初始化 ECharts 图表
    var chartDom = document.getElementById('single_station_chart_by_range');
    var myChart = echarts.init(chartDom);
    
    // 设置图表的配置项
    var option = {
        title: {
            text: '示例图表'
        },
        tooltip: {
            trigger: 'axis'
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        xAxis: {
            type: 'category',
            data: ['一', '二', '三', '四', '五', '六', '七']
        },
        yAxis: {
            type: 'value'
        },
        series: [{
            name: '数据',
            type: 'line',
            data: [1, 2, 3, 4, 5, 6, 7]
        }]
    };

    // 设置图表的选项
    myChart.setOption(option);

    

    
});