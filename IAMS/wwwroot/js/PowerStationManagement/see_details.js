$(document).ready(function () {
    // 监听模态框的显示事件
    $('#detailModal').on('show.bs.modal', function (event) {
        // 获取触发按钮
        var button = $(event.relatedTarget);

        // 获取 data-* 属性中的值
        var powerStationId = button.data('id');
        var requestUrl = seeDetailsUrl+'?id=' + powerStationId;
        // 发送 AJAX 请求获取详细信息
        $.ajax({
            url: requestUrl, // 使用动态生成的请求 URL
            method: 'GET', // 请求方式
            success: function (response) {
                // 假设返回的数据结构是 { name: "电站名称", imageUrl: "图片链接" }
                $('#stationName').text(response.name);
                $('#installPower').text(response.installedPower);
                $('#installedCapacity').text(response.installedCapacity);
                $('#address').text(response.locationDetails);
                $('#phone').text(response.phone);

                // 遍历图片 URL 列表并渲染到页面中
                var imageContainer = $('#powerStationImages'); 
                imageContainer.empty();
                response.stationImagesFilePath.forEach(function (imageUrl) {
                    var imgTag = $('<img />', {
                        src: imageUrl,   // 设置图片的 src 属性
                        alt: 'Power Station Image',  // 可选的 alt 属性
                        class: 'img-fluid'  // 可选的样式
                    });
                    imageContainer.append(imgTag); // 将 img 标签添加到 div#Image 中
                });

                // 遍历图片 URL 列表并渲染到页面中
                imageContainer = $('#powerStationInstallImages');
                imageContainer.empty();
                response.stationInstallImagesFilePath.forEach(function (imageUrl) {
                    var imgTag = $('<img />', {
                        src: imageUrl,   // 设置图片的 src 属性
                        alt: 'Power Station Image',  // 可选的 alt 属性
                        class: 'img-fluid'  // 可选的样式
                    });
                    imageContainer.append(imgTag); // 将 img 标签添加到 div#Image 中
                });
                $('#networkInfo').text(response.networkInfo);
                $('#transformerInfo').text(response.transformerInfo);
                $('#transformerCapacity').text(response.transformerCapacity);
                $('#installer').text(response.installer);
                $('#installerPhone').text(response.installerPhone);
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
