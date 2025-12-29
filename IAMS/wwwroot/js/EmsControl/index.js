
$(document).ready(function () {

    $("#test_mode_submit").on("click", function () {
        var sn = $("#energy_storage_cabinet_name").val();
        // 从页面读取值
        var activePower = parseInt($("#activePower").val());
        var onOff = $("#test_onOff").is(":checked") ? 1 : 0;
        var reactivePower = parseInt($("#reactivePower").val());
        var protectSwitch = $("#protectSwitch").is(":checked") ? 1 : 0;

        // 组合为 TestModeLogicCfg
        var logicCfg = {
            activePower: activePower,
            onOff: onOff,
            reactivePower: reactivePower,
            protectSwitch: protectSwitch
        };

        // 组合为 TestMode（transaction、timestamp、runmode 视情况调整）
        var data = {
            sn: sn,
            transaction: 6666,
            timeStamp: Math.floor(Date.now() / 1000),
            runMode: 0,
            logicCfg: logicCfg
        };

        // AJAX 提交
        $.ajax({
            url: testModeSubmitUrl,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            success: function (res) {
                alert(res.msg);
                location.reload();
            },
            error: function (err) {
                alert("失败");
            }
        });
    });

    $("#protect_setting_submit").on("click", function () {
        var sn = $("#energy_storage_cabinet_name").val();
        // 从页面读取值
        var transCapacity = parseInt($("#transCapacity").val());
        var maxPower = parseInt($("#maxPower").val());
        var overLoadSwitch = $("#overLoadSwitch").is(":checked") ? 1 : 0;
        var olWarnLimitVal = parseInt($("#olWarnLimitVal").val());
        var olShutdownVal = parseInt($("#olShutdownVal").val());
        var demandSwitch = $("#demandSwitch").is(":checked") ? 1 : 0;
        var targetDemand = parseInt($("#targetDemand").val());
        var deWarnLimitVal = parseInt($("#deWarnLimitVal").val());
        var deShutdownVal = parseInt($("#deShutdownVal").val());
        var backflowSwitch = $("#backflowSwitch").is(":checked") ? 1 : 0;
        var bfWarnLimitVal = parseInt($("#bfWarnLimitVal").val());
        var bfShutdownVal = parseInt($("#bfShutdownVal").val());
        var socForbidCharge = parseInt($("#socForbidCharge").val());
        var socForbidDischarge = parseInt($("#socForbidDischarge").val());

        // 组合为 TestModeLogicCfg
        var logicCfg = {
            transCapacity: transCapacity,
            maxPower: maxPower,
            overLoadSwitch: overLoadSwitch,
            olWarnLimitVal: olWarnLimitVal,
            olShutdownVal: olShutdownVal,
            demandSwitch: demandSwitch,
            targetDemand: targetDemand,
            deWarnLimitVal: deWarnLimitVal,
            deShutdownVal: deShutdownVal,
            backflowSwitch: backflowSwitch,
            bfWarnLimitVal: bfWarnLimitVal,
            bfShutdownVal: bfShutdownVal,
            socForbidCharge: socForbidCharge,
            socForbidDischarge: socForbidDischarge
        };

        // 组合为 TestMode（transaction、timestamp、runmode 视情况调整）
        var data = {
            sn: sn,
            transaction: 888,
            timeStamp: Math.floor(Date.now() / 1000),
            runMode: 100,
            logicCfg: logicCfg
        };

        // AJAX 提交
        $.ajax({
            url: protectSettingSubmitUrl,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            success: function (res) {
                alert(res.msg);
                location.reload();
            },
            error: function (err) {
                alert("失败");
            }
        });
    });

    
    $("#pv_storage_submit").on("click", function () {
        var sn = $("#energy_storage_cabinet_name").val();
        // 从页面读取值
        var pvInverterBrand = parseInt($("#pvInverterBrand").val());
        var pcsBrand = parseInt($("#pcsBrand").val());
        var pvCouplingMethod = parseInt($("#pvCouplingMethod").val());
        var onThreshold = parseFloat($("#onThreshold").val());
        var offThreshold = parseFloat($("#offThreshold").val());
        var pvMaxPower = parseInt($("#pvMaxPower").val());
        var maxGridPower = parseInt($("#maxGridPower").val());
        var transRedunPower = parseInt($("#transRedunPower").val());

        var logicCfg = {
            pvInverterBrand: pvInverterBrand,
            pcsBrand: pcsBrand,
            pvCouplingMethod: pvCouplingMethod,
            onThreshold: onThreshold,
            offThreshold: offThreshold,
            pvMaxPower: pvMaxPower,
            maxGridPower: maxGridPower,
            transRedunPower: transRedunPower
        };

        var data = {
            sn: sn,
            transaction: 852,
            timeStamp: Math.floor(Date.now() / 1000),
            runMode: 2,
            logicCfg: logicCfg
        };

        // AJAX 提交
        $.ajax({
            url: pvStorageControlSubmitUrl,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            success: function (res) {
                alert(res.msg);
                location.reload();
            },
            error: function (err) {
                alert("失败");
            }
        });
    });
    $("#btn_powerusage_submit").click(function () {
        var sn = $("#energy_storage_cabinet_name").val();
        let templates = [];
        $("#accordionContainer .accordion-item").each(function (index) {

            let accordionItem = $(this);

            // 模板名称
            let tltName = accordionItem.find("input[type=text]").first().val();

            // 获取日期表 applyDates[]
            let applyDates = [];
            accordionItem.find("table").eq(0).find("tbody tr").each(function () {
                let sDate = $(this).find(".start_date").val();
                let eDate = $(this).find(".end_date").val();
                if (sDate && eDate) {
                    applyDates.push({
                        sDate: sDate,
                        eDate: eDate
                    });
                }
            });

            // 获取时间段表 sTab[]
            let sTab = [];
            accordionItem.find("table").eq(1).find("tbody tr").each(function () {
                let sSec = $(this).find("input[type=time]").eq(0).val();
                let eSec = $(this).find("input[type=time]").eq(1).val();
                let pwrKw = $(this).find("input[type=text]").val();

                if (sSec && eSec) {
                    sTab.push({
                        sSec: timeToSecond(sSec),
                        eSec: timeToSecond(eSec),
                        pwrKw: parseFloat(pwrKw)
                    });
                }
            });

            // push 1 个模板
            templates.push({
                tltId: index + 1,
                tltName: tltName,
                sTabN: sTab.length,
                sTab: sTab,
                applyDates: applyDates
            });
        });

        // 组合后端需要的 JSON 结构
        let postData = {
            sn:sn,
            transaction: 9999,
            timeStamp: Math.floor(Date.now() / 1000),
            runMode: 1,
            pcsChgPlanPower: 0,
            transRedunPower: 0,
            logicCfg: {
                pvTab: {
                    template: templates
                }
            }
        };

        console.log("最终发送：", postData);

        // AJAX 提交
        $.ajax({
            url: powerUsageSubmitUrl,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(postData),
            success: function (res) {
                alert(res.msg);
                location.reload();
            },
            error: function (err) {
                alert("失败");
            }
        });
    })

    initDataPicker();

    $(document).on("click", ".btn-delete-row", function () {
        // 弹出确认框
        if (confirm("确定要删除该模板吗？")) {
            // 找到当前按钮所在的 accordion-item 并删除
            $(this).closest(".accordion-item").remove();
        }
    });

    // 新增使用电量行
    $(document).on("click", ".btn-add-data-row", function () {
        const $table = $(this).closest("table");
        const $tbody = $table.find("tbody");

        const newRow = `
					<tr>
						<td><input type="time" class="form-control" required></td>
						<td><input type="time" class="form-control" required></td>
						<td><input type="text" class="form-control" value="200" required></td>
						<td>
							<button type="button" class="btn btn-primary btn-add-data-row">新增</button>
							<button type="button" class="btn btn-danger btn-delete-data-row">删除</button>
						</td>
					</tr>
				`;
        $tbody.append(newRow);
    });

    // 删除使用电量行
    $(document).on("click", ".btn-delete-data-row", function () {
        $(this).closest("tr").remove();
    });

    // 新增日期行
    $(document).on("click", ".btn-add-date-row", function () {
        const $table = $(this).closest("table");
        const $tbody = $table.find("tbody");

        const newRow = `
					<tr>
						<td><input class="form-control date_without_year start_date" type="text" placeholder="MM-DD"></td>
						<td><input class="form-control date_without_year end_date" type="text" placeholder="MM-DD"></td>
						<td>
							<button type="button" class="btn btn-primary btn-add-date-row">新增</button>
							<button type="button" class="btn btn-danger btn-delete-date-row">删除</button>
						</td>
					</tr>
				`;
        $tbody.append(newRow);

        // 初始化新添加的日期选择器
        initDataPicker();
    });

    // 删除日期行
    $(document).on("click", ".btn-delete-date-row", function () {
        $(this).closest("tr").remove();
    });

    let templateCount = 100;
    // 添加新模板按钮
    $(document).on("click", ".add_new_template", function () {
        templateCount++;

        // 拼接新的 accordion-item HTML
        let newItem = `
				<div class="accordion-item">
					<button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapse${templateCount}" aria-expanded="false">
						Accordion Item NEW
					</button>

					<div id="collapse${templateCount}" class="accordion-collapse collapse">
						<div class="accordion-body">
							<div class="input-group mb-3">
								<span class="input-group-text" id="basic-addon1">模板名称</span>
								<input type="text" class="form-control" value="Accordion Item NEW">
							</div>
							<table class="table time-table">
								<thead>
									<tr>
										<th scope="col">开始日期</th>
										<th scope="col">结束日期</th>
										<th>#</th>
									</tr>
								</thead>
								<tbody>
									<tr>
										<td><input class="form-control date_without_year start_date" type="text" placeholder="MM-DD"></td>
										<td><input class="form-control date_without_year end_date" type="text" placeholder="MM-DD"></td>
										<td>
											<button type="button" class="btn btn-primary btn-add-date-row">新增</button>
											<button type="button" class="btn btn-danger btn-delete-date-row">删除</button>
										</td>
									</tr>
								</tbody>
							</table>

							<table class="table time-table mt-4">
								<thead>
								<tr>
									<th scope="col">开始时间</th>
									<th scope="col">结束时间</th>
									<th scope="col">使用电量</th>
									<th>#</th>
								</tr>
								</thead>
								<tbody>
								<tr>
									<td><input type="time" class="form-control" required></td>
									<td><input type="time" class="form-control" required></td>
									<td><input type="text" class="form-control" value="200" required></td>
									<td>
										<button type="button" class="btn btn-primary btn-add-data-row">新增</button>
										<button type="button" class="btn btn-danger btn-delete-data-row">删除</button>
									</td>
								</tr>
								</tbody>
							</table>
							<button type="button" class="btn btn-danger btn-delete-row">删除模板</button>
						</div>

					</div>
				</div>
				`;

        // 添加到 accordion 容器里
        $("#accordionContainer").append(newItem);
        initDataPicker();
    });
});