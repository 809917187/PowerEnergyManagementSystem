function deleteUser(userId) {
    if (confirm("确定要删除该用户吗？")) {
        var formData = {
            id: userId
        };
        // 提交表单数据到服务器
        $.ajax({
            url: deleteUserUrl, // 后端接口地址
            type: 'POST',
            data: formData,
            success: function (response) {
                // 处理服务器返回的结果
                if (response.success) {
                    alert(response.message);
                    // 关闭模态框
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
    }
}