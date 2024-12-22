$(document).ready(function () {
    // 初始化 Flatpickr
    flatpickr("#StartTime", {
        dateFormat: "Y-m-d",

    });

    $("#StationImages").fileinput({
        theme: "bs5",
        uploadUrl: '#',
        allowedFileExtensions: ['jpg', 'png'],
        initialPreviewAsData: true,
        maxFileSize: 5000,
        maxFileCount: 5,
        showUpload: false,
        initialPreview: stationImageUrls,
        initialPreviewConfig: stationImageUrls.map((url, index) => ({
            caption: `picture-${index + 1}.jpg`,
            key: index,
            showRemove: true,  // 不显示删除按钮
            showUpload: false  // 不显示上传按钮
        })),
        fileActionSettings: {
            showRemove: true, // 显示移除按钮
            showUpload: false // 隐藏上传按钮
        }
    });

    $("#StationInstallImages").fileinput({
        theme: "bs5",
        uploadUrl: '#',
        allowedFileExtensions: ['jpg', 'png'],
        initialPreviewAsData: true,
        maxFileSize: 5000,
        maxFileCount: 5,
        showUpload: false,
        initialPreview: stationInstallImageUrls,
        initialPreviewConfig: stationInstallImageUrls.map((url, index) => ({
            caption: `picture-${index + 1}.jpg`,
            key: index,
            showRemove: true,  // 不显示删除按钮
            showUpload: false  // 不显示上传按钮
        })),
        fileActionSettings: {
            showRemove: true, // 显示移除按钮
            showUpload: false // 隐藏上传按钮
        }
    });

    //$("#StationImages").fileinput({
    //    theme: "bs5",
    //    uploadUrl: "#",
    //    allowedFileExtensions: ['jpg', 'png'],
    //    overwriteInitial: false,
    //    initialPreviewAsData: true,
    //    maxFileSize: 5000,
    //    maxFileCount: 5,
    //    removeFromPreviewOnError: true,
    //    showPreview: true,
    //    showUpload: false,
    //    showCaption: false,
    //    uploadAsync: false,
    //    autoReplace: true,
    //    initialPreview: stationImageUrls,
    //    initialPreviewConfig: stationImageUrls.map((url, index) => ({
    //        caption: `picture-${index + 1}.jpg`,
    //        showRemove: false,  // 不显示删除按钮
    //        showUpload: false  // 不显示上传按钮
    //    })),
    //    fileActionSettings: {
    //        showRemove: true,  // 显示删除按钮
    //        showUpload: false  // 隐藏上传按钮
    //    }
    //}).on('fileupload', function (event, data, previewId, index) {
    //    // 通过一个空函数来禁用上传行为
    //    event.preventDefault();
    //});


    

    $('#NetworkInfo').val(selectedNetworkValue);

    $('.removeUpload div').each(function () {
        // 查找 div 下所有的 a 标签
        $(this).find('a').each(function () {
            // 查找 a 标签下的 span，判断 span 的文本是否包含 "Upload"
            var span = $(this).find('span');
            if (span.text().includes('Upload')) {
                $(this).remove(); // 删除包含 "Upload" 的 a 标签
            }
        });
    });


    $('form').submit(function (event) {
        event.preventDefault();  // 阻止表单提交
        if (deleteImages.length > 0) {
            $("#deleteImages").val(JSON.stringify(deleteImages));
        }
        var formData = new FormData(this);  // 获取表单数据

        $.ajax({
            url: editPowerStationUrl,
            type: 'POST',
            data: formData,
            processData: false,  // 必须设置为 false，才能正确发送 FormData
            contentType: false,  // 必须设置为 false，才能正确发送 FormData
            success: function (response) {
                if (response.message) {
                    // 在页面上显示返回的消息
                    alert(response.message);
                }
                if (response.status != '500') {
                    window.location.href = powerStationIndexUrl;
                }
            },
            error: function () {
                alert('上传失败');
            }
        });
    });


});
