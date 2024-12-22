// 删除当前行
function deleteRowForTime(button) {
    const row = button.closest('tr');
    row.remove();
}


function addRowForTime() {
    const table = document.getElementById('timeTable').getElementsByTagName('tbody')[0];
    const newRow = table.insertRow();

    newRow.innerHTML = `
         <td>
           <select class="form-select time_select start_time"  onchange="validateTime(this)">
             <!-- Time options will be added dynamically -->
           </select>
         </td>
         <td>
           <select class="form-select time_select end_time"  onchange="validateTime(this)">
             <!-- Time options will be added dynamically -->
           </select>
         </td>
         <td>
           <select class="form-select time_type ">
             <option value="1">尖</option>
             <option value="2">峰</option>
             <option value="3">平</option>
             <option value="4">谷</option>
             <option value="5">深谷</option>
           </select>
         </td>
         <td>
           <button class="btn btn-danger btn-sm"  type="button" onclick="deleteRowForTime(this)">删除</button>
         </td>
       `;

    // 重新生成时间选项
    generateTimeOptions();
}


// 生成时间选项的函数
function generateTimeOptions() {
    const timeSelects = document.querySelectorAll('.time_select');
    const timeOptions = [];

    // 生成时间选项（从 00:00 到 23:45，间隔 15 分钟）
    for (let hour = 0; hour < 24; hour++) {
        for (let minute = 0; minute < 60; minute += 15) {
            const hourString = hour.toString().padStart(2, '0');
            const minuteString = minute.toString().padStart(2, '0');
            const time = `${hourString}:${minuteString}`;
            timeOptions.push(time);
        }
    }
    // 添加 24:00 时间点
    timeOptions.push("23:59");

    // 填充每个时间选择框
    timeSelects.forEach(select => {
        timeOptions.forEach(time => {
            const option = document.createElement('option');
            option.value = time;
            option.textContent = time;
            select.appendChild(option);
        });
    });
}

// 校验时间，确保结束时间 > 开始时间
function validateTime(element) {
    const row = element.closest("tr");
    const startTimeSelect = row.querySelector("td:nth-child(1) select");
    const endTimeSelect = row.querySelector("td:nth-child(2) select");

    const startTime = startTimeSelect.value;
    const endTime = endTimeSelect.value;

    if (startTime && endTime && startTime >= endTime) {
        //alert("结束时间必须晚于开始时间！");
        endTimeSelect.value = ""; // 重置结束时间
    }

    // 2. 校验时间范围是否与其他行重叠
    const allRows = document.querySelectorAll("#timeTable tbody tr");
    for (const otherRow of allRows) {
        if (otherRow === row) continue; // 跳过当前行

        const otherStartTime = otherRow.querySelector("td:nth-child(1) select").value;
        const otherEndTime = otherRow.querySelector("td:nth-child(2) select").value;

        if (otherStartTime && otherEndTime) {
            // 检查时间段是否重叠
            if (
                (startTime >= otherStartTime && startTime < otherEndTime) || // 起始时间在其他时间段内
                (endTime > otherStartTime && endTime <= otherEndTime) || // 结束时间在其他时间段内
                (startTime <= otherStartTime && endTime >= otherEndTime) // 当前时间段完全覆盖其他时间段
            ) {
                alert("时间段不能重叠，请重新选择时间！");
                row.querySelector("td:nth-child(1) select").value = "";
                row.querySelector("td:nth-child(2) select").value = "";
                return;
            }
        }
    }
}