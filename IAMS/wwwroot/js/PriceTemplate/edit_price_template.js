$(document).ready(function () {
    generateTimeOptions();

    $("#sendRequest").click(function () {
        var sendData = {};
        sendData.Id = $("#templateId").val();
        sendData.Name = $("#Name").val();
        sendData.Tag = $("#Tag").val();

        var timeFrameInfos = [];
        $("#timeTable tbody tr").each(function () {
            var startTime = $(this).find(".start_time").val() + ":00";
            var endTime = $(this).find(".end_time").val();
            if (endTime == "24:00") {
                endTime = "1.00:00:00";
            } else {
                endTime = endTime + ":00";
            }
            var timeFrameType = $(this).find(".time_type").val();

            // 构造 TimeFrameInfo 对象
            var TimeFrameInfo = {};
            TimeFrameInfo.StartTime = startTime;
            TimeFrameInfo.EndTime = endTime;
            TimeFrameInfo.TimeFrameType = parseInt(timeFrameType);

            timeFrameInfos.push(TimeFrameInfo);
        });
        sendData.timeFrameInfos = timeFrameInfos;

        var TimeFrame2BuyPrice = {};
        var TimeFrame2SalePrice = {};
        $("#priceTable tbody tr").each(function () {
            var time_frame_type = $(this).find(".time_frame_type").val();
            var buy_price = $(this).find(".buy_price").val();
            var sale_price = $(this).find(".sale_price").val();

            TimeFrame2BuyPrice[time_frame_type] = buy_price;
            TimeFrame2SalePrice[time_frame_type] = sale_price;
        });
        sendData.TimeFrame2BuyPrice = TimeFrame2BuyPrice;
        sendData.TimeFrame2SalePrice = TimeFrame2SalePrice;

        $.ajax({
            url: EditTemplate,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(sendData),
            success: function (response) {
                if (response.message) {
                    // 在页面上显示返回的消息
                    alert(response.message);
                }
                if (response.status != '500') {
                    window.location.href = TemplateIndex;
                }
            },
            error: function (xhr, status, error) {
                alert('添加失败');
            }
        });
    });
});