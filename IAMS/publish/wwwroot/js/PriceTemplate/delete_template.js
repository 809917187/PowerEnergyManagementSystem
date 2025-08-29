function deletePriceTemplate(id) {
    if (confirm("确定删除该模板吗？")) {
        var ajaxData = {};
        ajaxData.id = id;
        $.ajax({
            url: deletePriceTemplateUrl,
            type: 'POST',
            data: ajaxData,
            success: function (response) {
                if (response.message) {
                    alert(response.message);
                }
                location.reload();
            },
            error: function () {
                alert('上传失败');
            }
        });
    }
}
