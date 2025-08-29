$(document).ready(function () {
    
    $('#addUserSubmit').on('click', function () {
        var newPassword = $('#Password').val();
        var newPasswordDC = $('#PasswordDC').val();
        if (newPassword != newPasswordDC) {
            alert("两次密码不一样");
            return;
        }

        // 获取表单数据
        var formData = {
            Email: $('#Email').val(),
            Password: newPassword,
            Name: $('#Name').val(),
            PhoneNumber: $('#PhoneNumber').val(),
            RoleCode: $('#RoleCode').val()
        };

        // 校验属性是否为空
        for (var key in formData) {
            if (!formData[key]) { // 检查值是否为空、undefined 或 null
                //alert(key + ' 不能为空！');
                return;
            }
        }

        // 提交表单数据到服务器
        $.ajax({
            url: addUserUrl, // 后端接口地址
            type: 'POST',
            data: formData,
            success: function (response) {
                // 处理服务器返回的结果
                if (response.success) {
                    alert(response.message);
                    // 关闭模态框
                    $('#addUser').modal('hide');
                    location.reload();
                } else {
                    alert("错误: " + response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("请求失败: ", error);
                alert("提交失败，请稍后重试。");
            }
        });
    })

});