
    $(document).ready(function() {
        $('#submitPassword').on('click', function () {
            var newPassword = $('#newPassword').val();
            var newPasswordDC = $('#newPasswordDC').val();
            if (newPassword != newPasswordDC) {
                alert("两次密码不一样");
                return;
            }

            // 获取表单数据
            var formData = {
                oldPassword: $('#oldPassword').val(),
                newPassword: newPassword,
                newPasswordDC: newPasswordDC
            };

            // 提交表单数据到服务器
            $.ajax({
                url: changePasswordUrl, // 后端接口地址
                type: 'POST',
                data: formData,
                success: function (response) {
                    // 处理服务器返回的结果
                    if (response.success) {
                        alert(response.message);
                        // 关闭模态框
                        $('#changePassword').modal('hide');
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
