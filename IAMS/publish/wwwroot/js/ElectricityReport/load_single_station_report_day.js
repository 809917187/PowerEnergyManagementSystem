function loadSingleStationReportTable(LoadSingleStationReportTable) {
    // AJAX 请求后端数据
    $.ajax({
        url: LoadSingleStationReportTable, // 后端接口
        method: 'GET',
        success: function (data) {
            // 遍历数据，生成表格行
            var tableBody = '';

            for (const [key, value] of Object.entries(data)) {
                tableBody += `
                    <tr>
                        <td>${key.split('T')[0]}</td>
                        <td>${value.peakForwardActiveEnergy.toFixed(2)}</td>
                        <td>${value.flatForwardActiveEnergy.toFixed(2)}</td>
                        <td>${value.normalForwardActiveEnergy.toFixed(2)}</td>
                        <td>${value.valleyForwardActiveEnergy.toFixed(2)}</td>
                        <td>${value.totalForwardActiveEnergy.toFixed(2)}</td>
                        <td>${value.peakReverseActiveEnergy.toFixed(2)}</td>
                        <td>${value.flatReverseActiveEnergy.toFixed(2)}</td>
                        <td>${value.normalReverseActiveEnergy.toFixed(2)}</td>
                        <td>${value.valleyReverseActiveEnergy.toFixed(2)}</td>
                        <td>${value.totalReverseActiveEnergy.toFixed(2)}</td>
                        <td>${(value.efficiency * 100).toFixed(0) }%</td>
                    </tr>
                `;
            }

            // 将生成的行插入表格
            $('#StationReportByDay tbody').html(tableBody);
        },
        error: function () {
            alert('数据加载失败！');
        }
    });
};
