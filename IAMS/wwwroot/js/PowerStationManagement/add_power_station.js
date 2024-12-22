$(document).ready(function () {
    // 初始化 Flatpickr
    flatpickr("#StartTime", {
        dateFormat: "Y-m-d",

    });

    $("#StationImages").fileinput({
        theme: "bs5",
        uploadUrl: "",
        allowedFileExtensions: ['jpg', 'png'],
        overwriteInitial: false,
        initialPreviewAsData: true,
        maxFileSize: 5000,
        removeFromPreviewOnError: true,
        showPreview: true,
        showUpload: false,
        showCaption: false,
        uploadAsync: false,
        autoReplace: false
    });

    $("#StationInstallImages").fileinput({
        theme: "bs5",
        uploadUrl: "",
        allowedFileExtensions: ['jpg', 'png'],
        overwriteInitial: false,
        initialPreviewAsData: true,
        maxFileSize: 5000,
        removeFromPreviewOnError: true,
        showPreview: true,
        showUpload: false,
        showCaption: false,
        uploadAsync: false,
        autoReplace: false
    });

    const geoNamesBaseURL = "http://api.geonames.org/";
    const username = "zhangwei"; // 请替换为你的 GeoNames 用户名

    const $countrySelect = $('#Country');
    const $stateSelect = $('#State');
    const $citySelect = $('#City');
    const $regionSelect = $('#Region');

    // 1. 获取国家列表
    $.ajax({
        url: `${geoNamesBaseURL}countryInfoJSON?username=${username}&lang=zh`,
        method: 'GET',
        success: function (data) {
            $.each(data.geonames, function (index, country) {
                $countrySelect.append(`<option value="${country.geonameId}">${country.countryName}</option>`);
            });
        }
    });

    // 2. 国家选择事件，加载省/州
    $countrySelect.on('change', function () {
        const countryId = $(this).val();
        $stateSelect.html('<option value="">选择省/州</option>');
        $citySelect.html('<option value="">选择市</option>');
        $regionSelect.html('<option value="">选择区</option>');

        if (countryId) {
            $.ajax({
                url: `${geoNamesBaseURL}childrenJSON?geonameId=${countryId}&username=${username}&lang=zh`,
                method: 'GET',
                success: function (data) {
                    $.each(data.geonames, function (index, state) {
                        $stateSelect.append(`<option value="${state.geonameId}">${state.name}</option>`);
                    });
                }
            });
        }
    });

    // 3. 省/州选择事件，加载市
    $stateSelect.on('change', function () {
        const stateId = $(this).val();
        $citySelect.html('<option value="">选择市</option>');
        $regionSelect.html('<option value="">选择区</option>');

        if (stateId) {
            $.ajax({
                url: `${geoNamesBaseURL}childrenJSON?geonameId=${stateId}&username=${username}&lang=zh`,
                method: 'GET',
                success: function (data) {
                    $.each(data.geonames, function (index, city) {
                        $citySelect.append(`<option value="${city.geonameId}">${city.name}</option>`);
                    });
                }
            });
        }
    });

    // 4. 市选择事件，加载区
    $citySelect.on('change', function () {
        const cityId = $(this).val();
        $regionSelect.html('<option value="">选择区</option>');

        if (cityId) {
            $.ajax({
                url: `${geoNamesBaseURL}childrenJSON?geonameId=${cityId}&username=${username}&lang=zh`,
                method: 'GET',
                success: function (data) {
                    $.each(data.geonames, function (index, region) {
                        $regionSelect.append(`<option value="${region.geonameId}">${region.name}</option>`);
                    });
                }
            });
        }
    });


    $('form').submit(function (event) {
        event.preventDefault();  // 阻止表单提交

        var formData = new FormData(this);  // 获取表单数据

        $.ajax({
            url: addPowerStationUrl,
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
