function loadStationSummaryTable(LoadStationSummaryTable) {
    // AJAX 请求后端数据
    $.ajax({
        url: LoadStationSummaryTable, // 后端接口
        method: 'GET',
        success: function (data) {
            // 遍历数据，生成表格行
            var tableBody = '';
            data.forEach(function (item) {
                tableBody += `
                    <tr>
                        <td>${item.powerStationName}</td>
                        <td>${item.peakForwardActiveEnergy.toFixed(2)}</td>
                        <td>${item.highForwardActiveEnergy.toFixed(2)}</td>
                        <td>${item.flatForwardActiveEnergy.toFixed(2)}</td>
                        <td>${item.valleyForwardActiveEnergy.toFixed(2)}</td>
                        <td>${item.totalForwardActiveEnergy.toFixed(2)}</td>
                        <td>${item.peakReverseActiveEnergy.toFixed(2)}</td>
                        <td>${item.highReverseActiveEnergy.toFixed(2)}</td>
                        <td>${item.flatReverseActiveEnergy.toFixed(2)}</td>
                        <td>${item.valleyReverseActiveEnergy.toFixed(2)}</td>
                        <td>${item.totalReverseActiveEnergy.toFixed(2)}</td>
                        <td>${item.efficiency.toFixed(2) * 100 }%</td>
                    </tr>
                `;
            });
            // 将生成的行插入表格
            $('#stationSummary tbody').html(tableBody);
        },
        error: function () {
            alert('数据加载失败！');
        }
    });
};
