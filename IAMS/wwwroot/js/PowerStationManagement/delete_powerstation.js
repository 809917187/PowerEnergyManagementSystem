function deleteStation(id) {
    if (confirm("确定删除该电站吗？")) {
        var ajaxData = {};
        ajaxData.id = id;
        $.ajax({
            url: deletePowerStationUrl,
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
