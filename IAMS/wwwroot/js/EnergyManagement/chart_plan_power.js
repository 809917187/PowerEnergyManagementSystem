$(document).ready(function () {
    var chartDom = document.getElementById('chart_plan_power');
    var myChart = echarts.init(chartDom);
    var option;

    // 基础日期为今天
    let base = new Date();
    base.setHours(0, 0, 0, 0); // 设置为当天的00:00:00
    let oneHour = 3600 * 1000; // 一小时的毫秒数
    let data = [];

    // 随机生成 00:00 到 24:00 的每小时数据
    for (let i = 0; i <= 24; i++) {
        let currentTime = new Date(base.getTime() + i * oneHour);
        let value = Math.round(Math.random() * 100); // 随机值0-100
        data.push([+currentTime, value]);
    }

    option = {
        tooltip: {
            trigger: 'axis',
            position: function (pt) {
                return [pt[0], '10%'];
            }
        },
        title: {
            left: 'center',
            text: 'Hourly Data Chart'
        },
        toolbox: {
            feature: {
                dataZoom: {
                    yAxisIndex: 'none'
                },
                restore: {},
                saveAsImage: {}
            }
        },
        xAxis: {
            type: 'time',
            boundaryGap: false,
            axisLabel: {
                formatter: function (value) {
                    return echarts.format.formatTime('hh:mm', value); // 显示为小时:分钟
                }
            }
        },
        yAxis: {
            type: 'value',
            boundaryGap: [0, '10%']
        },
        dataZoom: [
            {
                type: 'inside',
                start: 0,
                end: 100
            },
            {
                start: 0,
                end: 100
            }
        ],
        series: [
            {
                name: 'Hourly Data',
                type: 'line',
                smooth: true,
                symbol: 'circle',
                areaStyle: {},
                data: data
            }
        ]
    };

    option && myChart.setOption(option);
});
