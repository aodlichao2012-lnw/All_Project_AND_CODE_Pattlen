let cbostatus2 = null;
let fucshowtel2 = null;
let id_serivce;
let name_serivce;
let table2;



$("#current_date").val = Date.now()
$("#sms_select").hide()
$("#btn_add_ser").hide()
$("#btn_ser_set_active").prop('disabled', true);
/*$("#btn_ser_remove").prop('disabled', true);*/

//$(window).on('beforeunload', function (e) {
//    return clearAllCookies();
//});

//$(window).on('load',
//    clearAllCookies()
//);


$("#btn_search").on('click', function (e) {
    window.location.href = '/FrmSearchNumber/Index';
})
$("#btn_Report").on('click', function (e) {
    window.location.href = '/FrmReportTel/Index';
})

$('#date1').datetimepicker({
    format: 'd/m/Y',
    formatDate: 'd/m/Y',

});


$('#date_tel').datetimepicker({
    format: 'd/m/Y H:i',
    formatTime: 'H:i',
    formatDate: 'd/m/Y',
});

//$('#date_tel_input').on('change', function (e) {
//    $("date_tel").val($('#date_tel_input').text())
//})
//$('#date1_input').on('change', function (e) {
//    $("date1").val($('#date1_input').text())
//})



$("#select_st").on('load',
    fuc_select_status()
)
$("#cbocity").on('load',
    fuc_select_status_2()
)
$("#select_st").on('change', function (e) {
    fuc_select_change_reson()
}

)
$(document).on('load', getfucLoad())
//$("#browsers").on('load',
//    fuc_select_status()
//)
//$("#browsers4").on('load',
//    fuc_select_status_2()
//)
//$("#select_st").on('change', function (e) {
//    fuc_select_change_reson()
//})


function fuc_select_change_reson() {
    let variable = "data-val"
    let reson_select = $('#select_st').val()
    /*  let select = $('#browsers [value="' + reson_select + '"]').data('val')*/
    $.ajax({
        url: "/FrmDetail/cboStatus_SelectedIndexChanged?cboStatus=" + reson_select + "&res_code=" + reson_select
        , type: "GET",
        data: null,
        success: function (e) {
            let values = JSON.parse(e)
            if (values[0] === "เครือข่าย") {
                let htmls = `  <select  id="cboDeny"  style="width:200px;height:25px;" >`
                for (i = 0; i < values.length; i++) {
                    htmls += `  <option value = "` + values[i] + `" >` + values[i] + `</option>`
                }
                htmls += ` </select>`
                console.log(htmls)
                $("#cboDeny").html(htmls)
            }
            else {
                let htmls = `  <select  id="select_rs" style="width:200px;height:25px;" >`
                for (i = 0; i < values.length; i++) {
                    htmls += `  <option value="` + values[i].DENY_CODE + `" data-val="` + values[i].DENY_CODE + `" >` + values[i].DENY_NAME + `</option>`
                }
                htmls += ` </select>`
                console.log(htmls)
                $("#select_rs").html(htmls)
                if (reson_select === "01") {
                    $("#btn_add_ser").show()
                } else {
                    $("#btn_add_ser").hide()
                }
            }
        }
    })
}

function fuc_select_status() {
    $.ajax({
        url: "/FrmDetail/setcboStatus"
        , type: "GET",
        data: null,
        success: function (e) {
            if (cbostatus2 === null) {
                let values = JSON.parse(e)
                let htmls = ` <select  id="select_st"  style="width:200px;height:25px;" >`

                for (i = 0; i < values.length; i++) {
                    if (values[i].RES_CODE === "01") {
                        htmls += `  <option  value = "` + values[i].RES_CODE + `" data-val="` + values[i].RES_CODE + `" selected >` + values[i].RES_NAME + `</option>`
                    }
                    else {
                        htmls += `  <option  value = "` + values[i].RES_CODE + `" data-val="` + values[i].RES_CODE + `" >` + values[i].RES_NAME + `</option>`
                    }
                }
                htmls += ` </select >`
                console.log(htmls)
                $("#select_st").html(htmls)
                if ($("#select_st").find(":selected").val() === "01") {
                    $("#btn_add_ser").show()
                } else {
                    $("#btn_add_ser").hide()
                }
                clearInterval()
            }

        }
    })
}

function fuc_select_status_2() {
    $.ajax({
        url: "/FrmDetail/showCity"
        , type: "GET",
        data: null,
        success: function (e) {
            let values = JSON.parse(e)
            let htmls = ` <select    style="width:200px;height:25px;"  id="cbocity" >`
            for (i = 0; i < values.length; i++) {
                htmls += `  <option  value = "` + values[i].CITY_CODE + `" >` + values[i].CITY_NAME_T + `</option>`
            }
            htmls += ` </select>`
            console.log(htmls)
            $("#cbocity").html(htmls)
        }
    })
}

function clearCookie(cookieName) {
    document.cookie = cookieName + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
}
function clearAllCookies() {
    var cookies = document.cookie.split(";");
    for (var i = 0; i < cookies.length; i++) {
        var cookieParts = cookies[i].split("=");
        var cookieName = cookieParts[0];
        clearCookie(cookieName);
    }
}
function fuclogout() {

    $.ajax({
        url: "/FrmDetail/SingOut",
        type: 'GET',
        success: function (e) {
            clearAllCookies();
            window.location.href = '/FrmLogin/Index'
        }
    })

}

function formatDate(date) {
    return (
        [
            date.getFullYear(),
            padTo2Digits(date.getMonth() + 1),
            padTo2Digits(date.getDate()),
        ].join('-') +
        ' ' +
        [
            padTo2Digits(date.getHours()),
            padTo2Digits(date.getMinutes()),
            // padTo2Digits(date.getSeconds()),  // 👈️ can also add seconds
        ].join(':')
    );
}
function getfucLoad() {
    $.ajax({
        url: '/FrmDetail/LoadEdit',
        type: "GET",
        data: null,
        contentType: 'text/plain',
        success: function (e) {
            let valutf8 = decodeURIComponent(e);
            let values = JSON.parse(valutf8)
            $("#select_st").html(` <select  id="select_st"  style="width:200px;height:25px;" >
                                <option>

                                </option>
                            </select>`)
            $("#select_st").append(`<option value="` + values.res_code + `" selected> ` + values.cboStatus + `</option>`)
            $("#select_rs").append(`<option value="` + values.strDenycode + `" selected> ` + values.strDeny + `</option>`)
            $("#txt_tel").val(values.txtTel_No)
            $("#cname").val(values.txtName)
            $("#csname").val(values.txtSName)
            $("#sex").val(values.cboSex)
            $("#date_tel").val(values.txtDate_Tel)
            $("#date1").val(values.cboDate + "/" + values.cboMouth + "/" + values.txtYear)
            $("#cbocity").val(values.cbocity)
            /*   $("#status").text(e)*/
            cbostatus2 = values.cboStatus;
            if (values.cboStatus === "สมัคร") {

                $("#btn_add_ser").show()
            } else {
                $("#btn_add_ser").hide()
            }
            fuc_select_change_reson()
        }
    })
}
function loadser_sms() {
    var cookies = document.cookie.split(";");
    for (var i = 0; i < cookies.length; i++) {
        var cookieParts = cookies[i].split("=");
        var cookieName = cookieParts[0];
        var cookie_values = cookieParts[1];
        $("#btn_add_ser").css("display", "block")
    }
}

//function fuc_ser_remove_confirm() {
//    var checkedCount = $("input[type='checkbox']:checked").length;
//    var checkedvalues = $("input[type='checkbox']:checked");
//    let data1 = new FormData();
//    let visible_data = '';
//    visible_data += "'"
//    for (i = 0; i < checkedCount; i++) {
//        let service_var = checkedvalues[i].id
//        let variable_service_ = "#" + service_var;
//        $(variable_service_).hide();
//        visible_data += "" + service_var + "" + ','
//    }
//    visible_data += "'"
//    data1.append("VISIBLE", visible_data)
//    data1.append("txtTel_No", $("#txt_tel").val())
//    $.ajax({
//        url: '/FrmDetail/SetVisible',
//        type: 'POST',
//        processData: false,
//        contentType: false,
//        data: data1,
//        success: function (e) {
//            alert(e);
//            if (e === "บันทึกสำเร็จ") {
//                alert(e)
//                fuc_insert_ser();
//            }
//            else {
//                alert(e)
//            }
//        }
//    })
//}
let column_name_ser = "";
let column_name_id_ser = "";
function fuc_edit_sms(sms) {
    let datas = null;
    if ($("#txt_tel").val() == null) {
        datas = null
    } else {
        datas = $("#txt_tel").val()
    }
    $.ajax({
        url: '/FrmDetail/list_sms?txt_tel=' + datas,
        type: 'GET',
        data: datas
        , success: function (e) {
            let values = JSON.parse(e)
            console.log(values)
            let htmls = ``
            for (i = 0; i < values.length; i++) {
                if (i <= 10) {
                    if (i === 0) {
                        htmls += `<div style="padding:1%;" >`
                    }
                    column_name_id_ser += `SERVICE_` + values[i].SER_ID + `,`
                    column_name_ser += values[i].SER_NAME + ","
                    htmls += ` <label id="lbserSERVICE_` + values[i].SER_ID + `" > บริการ ` + values[i].SER_NAME + ` </label>  <input type="checkbox" style="padding-left:5px;"  id="SERVICE_` + values[i].SER_ID + `" placeholder="" class="input" />  `

                    if (i === 10) {
                        htmls += `</div>`
                    }
                }
                else if (i <= 20) {
                    if (i === 11) {
                        htmls += `<div style="padding:1%;" >`
                    }
                    column_name_id_ser += `SERVICE_` + values[i].SER_ID + `,`
                    column_name_ser += values[i].SER_NAME + ","
                    htmls += ` <label id="lbserSERVICE_` + values[i].SER_ID + `" > บริการ ` + values[i].SER_NAME + ` </label>  <input type="checkbox" style="padding-left:5px;"  id="SERVICE_` + values[i].SER_ID + `" placeholder="" class="input" />  `
                    if (i === 20) {
                        htmls += `</div>`
                    }
                } else if (i <= 30) {
                    if (i === 21) {
                        htmls += `<div style="padding:1%;" >`
                    }
                    column_name_id_ser += `SERVICE_` + values[i].SER_ID + `,`
                    column_name_ser += values[i].SER_NAME + ","
                    htmls += ` <label id="lbserSERVICE_` + values[i].SER_ID + `" > บริการ ` + values[i].SER_NAME + ` </label>  <input type="checkbox" style="padding-left:5px;"  id="SERVICE_` + values[i].SER_ID + `" placeholder="" class="input" />  `
                    if (i === 30) {
                        htmls += `</div>`
                    }
                }
                else if (i <= 39) {
                    if (i === 31) {
                        htmls += `<div style="padding:1%;" >`
                    }
                    column_name_id_ser += `SERVICE_` + values[i].SER_ID + `,`
                    column_name_ser += values[i].SER_NAME + ","
                    htmls += ` <label id="lbserSERVICE_` + values[i].SER_ID + `" > บริการ ` + values[i].SER_NAME + ` </label>  <input type="checkbox" style="padding-left:5px;"  id="SERVICE_` + values[i].SER_ID + `" placeholder="" class="input" />  `
                    if (i === 39) {
                        htmls += `</div>`
                    }
                }

            }
            $(sms).html(htmls)
        }
    })
}
function fuc_insert_ser() {
    let datas = null;
    if ($("#txt_tel").val() == null) {
        datas = null
    } else {
        datas = $("#txt_tel").val()
    }
    $.ajax({
        url: '/FrmDetail/SetVisible_remove?txt_tel=' + datas,
        type: 'GET'
        , data: datas
        , success: function (e) {
            let htmls2 = ``
            let values = JSON.parse(e)

            table2 = $("#sms_select").dataTable({
                destroy: true,
                //"columnDefs": [
                //    {
                //        "targets": "_all", // เปลี่ยนทุกคอลัมน์
                //        "className": "center_td" // ใช้คลาส CSS ที่คุณสร้าง
                //    }
                //],  // ปิดการค้นหา
                searching: true,
                lengthChange: false ,
                columns: [
                    { data: 'SER_ID', render: function (data, type, row) { return '<input type="checkbox" id="addSERVICE_' + data + '" value="' + data + '" data-active="' + row["IS_ACTIVE"]+'" class="editor-active">' } },
                    { data: 'SER_ID', title: 'หมายเลขบริการ' },
                    { data: 'SER_NAME', render: function (data, type, row) { return '<span hidden>' + data + '</span>' } },
                    { data: 'SER_NAME', render: function (data, type, row) { return '<input type="text" id="editSERVICE_' + row["SER_ID"] + '" value="' + data + '" class="editor-active2 m-3 " disabled><button id="btn_ser_save' + row["SER_ID"] + '" class="btn btn-primary " >บันทึก</button>' } },
                    { data: 'IS_ACTIVE', render: function (data, type, row) { return `<span id="ch` + row["SER_ID"] + `">` + data + `</span>` } },

                ],
                data: values,
                "createdRow": function (row, data, dataIndex) {
                    // ตรวจสอบเงื่อนไขของข้อมูลในแต่ละคอลัมน์
                    // ในตัวอย่างนี้เราจะตรวจสอบคอลัมน์ที่ 2 (index 1)
                    var columnValue = data.IS_ACTIVE;
                    console.log(columnValue)// เปลี่ยนตามคอลัมน์ที่คุณต้องการ
                    if (columnValue == 'True') {
                        // กำหนดพื้นหลัง (background color) สำหรับแถวนี้
                        $(row).css('background-color', 'lightgreen');
                    } else {
                        $(row).css('background-color', 'lightred');
                    }
                }
            })
            table2.addClass("table")
            table2.removeAttr('style')
            $("#sms_select").show()
        }

    })

}
//$("#sms_select").on('draw', function () {

//        fuc_edit_sms("#sms")
//        $("#btn_sms_add_save").prop('disabled', true);
//        $("#btn_ser_remove").prop('disabled', true);
//    })
$("#sms_select tbody").on('click', 'input[type="checkbox"]', function () {
    let isChecked = $(this).prop('checked');
    let $row = $(this).closest('tr');
    let $span = $row.find('span');
    let spanText
    $span.each(function () {
        spanText = $(this).text();
        if (isChecked) {
            $("#btn_ser_set_active").prop('disabled', false);
        }

        else {
            $("#btn_ser_set_active").prop('disabled', true);
        }
    });

})

$("#sms_select tbody").on('click', '[id^="btn_ser_save"]', function () {
    let $closestTd = $(this).closest('td');
    let $textInput = $closestTd.find('input[type="text"]');
    let textInputId = $textInput.attr('id');
    let textInputname = $textInput.val();
    $textInput.prop('disabled', true);
    btn_ser_save(textInputname, textInputId)
    table2.draw();

})

$("#sms_select tbody").on('click', 'td', function (e) {
    var inputField = $(this).find('input[type="text"][disabled]');
    if (inputField.length > 0) {
        inputField.attr('disabled', false);
    }
})
$("#sms_select tbody").on('input', 'td', function (e) {

    if ($(this).find('input').length > 0) {
        var value = $(this).find('input');
        id_serivce = value.attr('id');
        name_serivce = value.val();
    }
})
function btn_ser_save(value, id) {
    console.log("Save : " + id + " : " + value)
    $.ajax({
        url: '/FrmDetail/Save_service?id=' + id + "&values=" + value,
        type: 'GET',
        data: null,
        success: function (e) {
            alert(e)
            fuc_edit_sms("#sms")
            $("#btn_ser_set_active").prop('disabled', true);
        }
    })
}
function fuc_insert_ser_confirm() {

    let checkedvalues = $('#sms_select tbody td');
    let active;
    let bool_active = "";
    let data_active = "";
    let span_active_name = "";
    let span_active_name_val = "";
    let is_checked;

    checkedvalues.each(function (e) {
        active = $(this).find("input[type='checkbox']:checked")
        is_checked = active.prop("checked")
        data_active = active.attr("data-active")
        span_active_name_val = active.val()
        if (is_checked) {
            bool_active += data_active + ","
            span_active_name += span_active_name_val + ","
        }
   
   
    })
    let data1 = new FormData();
    data1.append("IsActive", bool_active)
    data1.append("Service_id_name", span_active_name)
    $.ajax({
        url: '/FrmDetail/SetVisible_Unvisible_Enable',
        type: 'POST',
        processData: false,
        contentType: false,
        data: data1,
        success: function (e) {
            alert(e);
            fuc_insert_ser();
        }
    })
}
function fuc_sms_ser_show(sms) {
    let htmls2 = ``
    let nameid = column_name_id_ser.split(',')
    let name = column_name_ser.split(',')
    console.log(name)

    htmls2 += `<div style="padding:1%;" >`
    for (i = 0; i < name.length - 1; i++) {
        if ($("#" + nameid[i] + "").is(':checked')) {
            htmls2 += `<label style="padding:1%;"> <label id="lbsersh` + nameid[i] + `" > บริการ ` + name[i] + ` </label></label>   <input type="checkbox" id="lb` + nameid[i] + `" placeholder="" class="input" />  `
        }

    }
    htmls2 += `</div style="padding:1%;" >`
    $(sms).html(htmls2)
}
$("#btn_logout").on('click', function (e) { fuclogout() })

$("#btn_reload").on('click', function (e) {
    fuc_edit_sms("#sms")
})
$("#btn_add_ser").on('click', function (e) { fuc_edit_sms("#sms") })
$("#btn_ser_add").on('click', function (e) { fuc_insert_ser() })
$("#btn_ser_set_active").on('click', function (e) { fuc_insert_ser_confirm() })


$("#btn_sms_save").on('click', function (e) {
    fuc_sms_ser_show("#sms2")
})
$("#btn_ser_remove").on('click', function (e) {

    fuc_ser_remove_confirm();
})


$("#btn_ser_remove_save").on('click',
    function (e) {
        let cons = confirm("คุณต้องการลบใช่หรือไม่ ?");
        if (con) {
            fuc_ser_remove_confirm()
        } else {

        }
    }
)

let table;
let htmls;
let table_sub;
let table_sub2;

function cbostatus() {
    $.ajax({
        url: '/FrmReportTel/FrmReportTel_Load',
        type: 'GET',
        data: null,
        success: function (e) {
            table = JSON.parse(e)
            table_sub = JSON.parse(table[1])
            htmls += '   <select name="" id="select_reson" style="width:100%;" >'
            for (i = 0; i < table_sub.length; i++) {
                if (table_sub[i] != "undefined") {
                    htmls += '<option '
                    htmls += 'value="' + table_sub[i].RES_CODE + '" >'
                    htmls += table_sub[i].RES_NAME
                    htmls += '</option >'
                } else {
                    alert("ไม่มี object ")
                }
            }
            htmls += '</select>'
            $("#select_reson").html(htmls);
        }
    })
}
function sum(table) {
    if (table[0].SUM != "") {
        $("#Label8").text(table[0].SUM)
        $("#lblS021").text(table[0].SER12)
        $("#lblS011").text(table[0].SER13)
    }

}
function tableload(tables) {

    $("#Label4").text(tables.length)
    $('#tb_1').DataTable().destroy();
    $('#tb_1').DataTable({
        columns: [
            { data: 'ANUMBER', title: 'เบอร์โทรศัพท์' },
            { data: 'CUST_NAME', title: 'ชื่อ' },
            { data: 'CUST_SNAME', title: 'นามสกุล' },
            { data: 'SERVICE_01', title: 'Service' },
            { data: 'SERVICE_02', title: 'Service' },
            { data: 'SERVICE_03', title: 'Service' },
            { data: 'SERVICE_04', title: 'Service' },
            { data: 'SERVICE_05', title: 'Service' },
            { data: 'SERVICE_06', title: 'Service' },
            { data: 'SERVICE_07', title: 'Service' },
            { data: 'SERVICE_08', title: 'Service' },
            { data: 'SERVICE_09', title: 'Service' },
            { data: 'SERVICE_10', title: 'Service' },
            { data: 'SERVICE_11', title: 'Service' },
            { data: 'SERVICE_12', title: 'Service' },
            { data: 'SERVICE_13', title: 'Service' },
            { data: 'SERVICE_14', title: 'Service' },
            { data: 'SERVICE_15', title: 'Service' },
            { data: 'SERVICE_16', title: 'Service' },
            { data: 'SERVICE_17', title: 'Service' },
            { data: 'SERVICE_18', title: 'Service' },
            { data: 'SERVICE_19', title: 'Service' },
            { data: 'SERVICE_20', title: 'Service' },
            { data: 'SERVICE_21', title: 'Service' },
            { data: 'SERVICE_22', title: 'Service' },
            { data: 'SERVICE_23', title: 'Service' },
            { data: 'SERVICE_24', title: 'Service' },
            { data: 'SERVICE_25', title: 'Service' },
            { data: 'SERVICE_26', title: 'Service' },
            { data: 'SERVICE_27', title: 'Service' },
            { data: 'SERVICE_28', title: 'Service' },
            { data: 'SERVICE_29', title: 'Service' },
            { data: 'SERVICE_30', title: 'Service' },
            { data: 'SERVICE_31', title: 'Service' },
            { data: 'SERVICE_32', title: 'Service' },
            { data: 'SERVICE_33', title: 'Service' },
            { data: 'SERVICE_34', title: 'Service' },
            { data: 'SERVICE_35', title: 'Service' },
            { data: 'SERVICE_36', title: 'Service' },
            { data: 'SERVICE_37', title: 'Service' },
            { data: 'SERVICE_38', title: 'Service' },
            { data: 'SERVICE_39', title: 'Service' },
            { data: 'SERVICE_40', title: 'Service' }
        ],
        data: tables
    }).draw()
}


function btnreport_click() {
    let reson = $("#select_reson").val()
    let date = $("#date_reson").val()
    let datas = new FormData();
    datas.append("res_code", reson)
    datas.append("Day", date)
    $.ajax({
        url: '/FrmReportTel/btnReport_Click',
        processData: false,
        contentType: false,
        type: 'POST',
        data: datas,
        success: function (e) {
            if (e === "ไม่มีข้อมูลที่คุณค้นหา") {
                alert("ไม่มีข้อมูลที่คุณค้นหา")
            }
            //else if (JSON.parse(e)[0].RES_CODE == "01") {
            //  let table3 =   JSON.parse(e)
            //     sum(table3)
            //    console.log("0111111111")
            //    tableload(table3)
            //}
            else {
                table = JSON.parse(e)

                table_sub = JSON.parse(table[0])
                table_sub2 = JSON.parse(table[1])
                tableload(table_sub2)
                sum(table_sub)
            }
        }
    })
}
function fucback() {
    window.location.href = '/FrmDetail/Index'
}


$(document).on('load', cbostatus())
$("#btn_report").on('click', function (e) { btnreport_click() })
$("#btn_logout").on('click', function (e) { fuclogout() })
$("#btn_back").on('click', function (e) { fucback() })


$(document).ready(function () {
    let tables;
    var data = [
        {
            anumber: '',
            status: '',
            cust_name: '',
            cust_sname: '',
            cust_sex: '',
            service_01: '',
            service_02: '',
            service_03: '',
            service_04: '',
            service_05: '',
            service_06: '',
            service_07: '',
            service_08: '',
            service_09: '',
            service_10: '',
            service_14: '',
            lead_call_date: '',
        }
        // เพิ่มข้อมูลอื่น ๆ ที่คุณต้องการ
    ];
    function gettable() {
        tables = $("#tb_2").DataTable({
            ajax: {
                url: '/FrmSearchNumber/FrmSearchNumber_Load',
                dataSrc: ''
            },

            columns: [
                { data: 'anumber', Title: 'หมายเลข        ' },
                { data: 'status', Title: 'สถานะ        ' },
                { data: 'cust_name', Title: 'ชื่อ            ' },
                { data: 'cust_sname', Title: 'นามสกุล        ' },
                { data: 'cust_sex', Title: 'เพศ' },
                { data: 'lead_call_date', Title: 'lead_call_date        ' }

            ]
        });
    }

    $(document).on('load', gettable())





    $("#btn_logout").on('click', fuclogout)


    tables.on('click', 'tbody tr', (e) => {
        let classList = e.currentTarget.classList;

        if (classList.contains('selected')) {
            classList.remove('selected');

            classList.add('new-background-color2');
            classList.remove('new-background-color');



        }
        else {


            /* tables.rows('.selected').nodes().each((row) => row.classList.remove('selected'));*/
            classList.add('selected');


            classList.add('new-background-color');
            classList.remove('new-background-color2');

            var rowData = $('#tb_1').DataTable().rows('.new-background-color').data()[0];


        }
    });
    //table2.on('draw', function () {

    //    fuc_edit_sms("#sms")
    //    $("#btn_sms_add_save").prop('disabled', true);
    //    $("#btn_ser_remove").prop('disabled', true);
    //})
    $("#btn_edit").on('click', function (e) {
        let datas = new FormData();
        datas.append('status', $('#tb_2').DataTable().rows('.new-background-color').data()[0].status)
        datas.append('anumber', $('#tb_2').DataTable().rows('.new-background-color').data()[0].anumber)
        $.ajax({
            url: '/FrmDetail/FrmDetail_Load',
            type: 'POST',
            data: datas,
            contentType: false,
            processData: false,
            success: function (e) {
                window.location.href = '/FrmDetail/Index'
            }
        })
    })
})

