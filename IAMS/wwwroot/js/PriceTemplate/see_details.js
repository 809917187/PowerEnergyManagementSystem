$(document).ready(function () {
    // 监听模态框的显示事件
    $('#detailModal').on('show.bs.modal', function (event) {
        // 获取触发按钮
        var button = $(event.relatedTarget);

        // 获取 data-* 属性中的值
        var Id = button.data('id');
        var requestUrl = seeDetailsUrl + '?id=' + Id;
        // 发送 AJAX 请求获取详细信息
        $.ajax({
            url: requestUrl, // 使用动态生成的请求 URL
            method: 'GET', // 请求方式
            success: function (response) {
                // 假设返回的数据结构是 { name: "电站名称", imageUrl: "图片链接" }
                $('#tenplateName').text(response.name);
                $('#price_template_details tbody').empty();

                // 遍历返回的列表
                response.timeFrameInfos.forEach(function (item) {
                    // 创建新的表格行
                    var row = `
                        <tr>
                            <td>${item.startTimeStr}</td>
                            <td>${item.endTimeStr}</td>
                            <td>${response.timeFrameTypeCode2Name[item.timeFrameType]}</td>
                            <td>${response.timeFrame2BuyPrice[item.timeFrameType]}</td>
                            <td>${response.timeFrame2SalePrice[item.timeFrameType]}</td>
                        </tr>
                        `;
                    // 将行添加到表格的 tbody
                    $('#price_template_details tbody').append(row);
                });
            },
            error: function (error) {
                console.log('获取数据失败:', error);
            }
        });
    });
});
