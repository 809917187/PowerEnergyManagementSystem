$(document).ready(function () {

    $("#bindUserOK").click(function () {
        var sendData = {};
        let userIds = [];
        $("#checkboxUser input[type='checkbox']:checked").each(function () {
            userIds.push(parseInt($(this).val(), 10)); // 将值转换为整数
        });
        sendData.userIds = userIds;
        sendData.powerStationId = parseInt($("#selectedPsId").val(), 10);
        $.ajax({
            url: bindPowerStationToUserUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(sendData),
            success: function (response) {
                if (response.message) {
                    // 在页面上显示返回的消息
                    alert(response.message);
                }
            },
            error: function (xhr, status, error) {
                alert('绑定失败');
            }
        });

    });

    // 监听模态框的显示事件
    $('#bindUserModal').on('show.bs.modal', function (event) {
        // 获取触发按钮
        var button = $(event.relatedTarget);

        // 获取 data-* 属性中的值
        var powerStationId = button.data('id');
        $("#selectedPsId").val(powerStationId);
        var powerStationName = button.data('name');
        var requestUrl = getAllUserListUrl + "?powerStationId=" + powerStationId;
        // 发送 AJAX 请求获取详细信息
        $.ajax({
            url: requestUrl, // 使用动态生成的请求 URL
            method: 'GET', // 请求方式
            success: function (response) {
                $('.bindModalStationName').text(powerStationName);
                // 确保 response 是数组
                if (Array.isArray(response)) {
                    let checkboxContainer = $("#checkboxUser"); // 获取复选框容器
                    checkboxContainer.empty(); // 清空已有内容，避免重复加载

                    response.forEach(function (item) {
                        let isChecked = item.isChecked ? "checked" : "";
                        let checkboxHtml = `
                    <div class="form-check">
                        <input class="form-check-input" type="checkbox" id="option${item.id}" value="${item.id}" ${isChecked}>
                        <label class="form-check-label" for="option${item.id}">${item.name}</label>
                    </div>`;
                        checkboxContainer.append(checkboxHtml); // 添加到容器中
                    });
                } else {
                    console.log("返回数据格式错误:", response);
                }
            },
            error: function (error) {
                console.log('获取数据失败:', error);
            }
        });
        //// 更新模态框内容
        //$('#modal-user-id').text(userId);
        //$('#modal-user-name').text(decodeURIComponent(userName.replace(/\+/g, ' ')));
    });
});
