let cbostatus2 = null;
let fucshowtel2 = null;
let id_serivce;
let name_serivce;
let table2;

$("#valid1").hide()
$("#valid2").hide()
$("#valid3").hide()
$("#valid4").hide()
$("#valid5").hide()


const currentDate = new Date();
const year = currentDate.getFullYear();
// Get the current month (0-11, where 0 is January and 11 is December)
const month = (currentDate.getMonth() + 1).toString().padStart(2, '0'); // Adding 1 because months are zero-based
// Get the current day of the month
const day = currentDate.getDate();

$("#current_date").val(`` + day + `/` + month + `/` + year + ``)
//$(window).ready(function (e) {
//    let i = 0;
//    let bools = true;
//    while (bools) {
//        if (i < 100000) {
//            console.log(i)
//             window.location.replace('/FrmDetail/Index'
//            i++;
//        } else {
//            bools = false;
//        }

//    }
//})

$("#btn_save2").hide()
function alert2(txt) {
    Swal.fire({
        title: 'แจ้งเตือน!',
        text: txt,
        confirmButtonText: 'OK',
        showCancelButton: true,
        customClass: {
            confirmButton: '    my-custom-button' // ใช้คลาส CSS ที่คุณสร้าง
        }
    });
}
$("#svg1").on('click', function (e) {
     window.location.replace('/FrmDetail/Index')
})
$("#svg2").on('click', function (e) {
     window.location.replace('/FrmDetail/Index')
})
$(function () {
    //Reference the DropDownList.
    var ddlYears = $("#year_thai");

    //Determine the Current Year.
    var currentYear = (new Date()).getFullYear();



    //Loop and add the Year values to DropDownList.
    for (var i = 2500; i <= currentYear + 543; i++) {
        var option = $("<option />");
        option.html(i);
        option.val(i);
        ddlYears.append(option);
    }

    $("#btn_clear").on('click', function (e) {
        $("#sms_select").hide()
        $("#select_st").val(``)
        $("#select_rs").val(``)
        $("#select_rs").val(``)
        $("#current_date").val(``)
        $("#cname").val(``)
        $("#cbocity").val(``)
        $("#cboDeny").val(``)
        $("#csname").val(``)
        $("#txt_tel").val(``)
        $("#date_num").val(``)
        $("#date_tel").val(``)
        $("#date_thai").val(``)
        $("#year_thai").val(``)
        $("#mouth_thai").val(``)
        $.ajax({
            url: '/FrmDetail/Clear_edit',
            type: 'GET',
            data: null,
            success: function (e) {
                cbostatus2 = null;
                fuc_select_status();
                fuc_select_status_2();
                fuc_edit_sms("#sms")
                fucshowtel3(true);
            }
        })
    })
    $("#mouth_thai").on('change', function (e) {
        let mounth_ = $("#mouth_thai option:selected").text()
        if (mounth_.endsWith("ยน")) {
            $("#date_num").html(`<select style="width: 200px; height: 25px; position: absolute;" id="date_num" name="">
                                <option value="" selected>-- โปรดเลือก วันที่เกิด --</option>
                                <option value="1">1</option>
                                <option value="2">2</option>
                                <option value="3">3</option>
                                <option value="4">4</option>
                                <option value="5">5</option>
                                <option value="6">6</option>
                                <option value="7">7</option>
                                <option value="8">8</option>
                                <option value="9">9</option>
                                <option value="10">10</option>
                                <option value="11">11</option>
                                <option value="12">12</option>
                                <option value="13">13</option>
                                <option value="14">14</option>
                                <option value="15">15</option>
                                <option value="16">16</option>
                                <option value="17">17</option>
                                <option value="18">18</option>
                                <option value="19">19</option>
                                <option value="20">20</option>
                                <option value="21">21</option>
                                <option value="22">22</option>
                                <option value="23">23</option>
                                <option value="24">24</option>
                                <option value="25">25</option>
                                <option value="26">26</option>
                                <option value="27">27</option>
                                <option value="28">28</option>
                                <option value="29">29</option>
                                <option value="30">30</option>
                            </select>`)
        }
        else if (mounth_.endsWith("พันธ์")) {
            $("#date_num").html(`<select style="width: 200px; height: 25px; position: absolute;" id="date_num" name="">
                                <option value="" selected>-- โปรดเลือก วันที่เกิด --</option>
                                <option value="1">1</option>
                                <option value="2">2</option>
                                <option value="3">3</option>
                                <option value="4">4</option>
                                <option value="5">5</option>
                                <option value="6">6</option>
                                <option value="7">7</option>
                                <option value="8">8</option>
                                <option value="9">9</option>
                                <option value="10">10</option>
                                <option value="11">11</option>
                                <option value="12">12</option>
                                <option value="13">13</option>
                                <option value="14">14</option>
                                <option value="15">15</option>
                                <option value="16">16</option>
                                <option value="17">17</option>
                                <option value="18">18</option>
                                <option value="19">19</option>
                                <option value="20">20</option>
                                <option value="21">21</option>
                                <option value="22">22</option>
                                <option value="23">23</option>
                                <option value="24">24</option>
                                <option value="25">25</option>
                                <option value="26">26</option>
                                <option value="27">27</option>
                                <option value="28">28</option>
                                <option value="29">29</option>
                            </select>`)
        }
        else if (mounth_.endsWith("คม")) {
            $("#date_num").html(`<select style="width: 200px; height: 25px; position: absolute;" id="date_num" name="">
                                <option value="" selected>-- โปรดเลือก วันที่เกิด --</option>
                                <option value="1">1</option>
                                <option value="2">2</option>
                                <option value="3">3</option>
                                <option value="4">4</option>
                                <option value="5">5</option>
                                <option value="6">6</option>
                                <option value="7">7</option>
                                <option value="8">8</option>
                                <option value="9">9</option>
                                <option value="10">10</option>
                                <option value="11">11</option>
                                <option value="12">12</option>
                                <option value="13">13</option>
                                <option value="14">14</option>
                                <option value="15">15</option>
                                <option value="16">16</option>
                                <option value="17">17</option>
                                <option value="18">18</option>
                                <option value="19">19</option>
                                <option value="20">20</option>
                                <option value="21">21</option>
                                <option value="22">22</option>
                                <option value="23">23</option>
                                <option value="24">24</option>
                                <option value="25">25</option>
                                <option value="26">26</option>
                                <option value="27">27</option>
                                <option value="28">28</option>
                                <option value="29">29</option>
                                <option value="30">30</option>
                                <option value="31">31</option>
                            </select>`)
        }
    })
});
pdfMake.fonts = {
    THSarabun: {
        normal: 'THSarabun.ttf',
        bold: 'THSarabun-Bold.ttf',
        italics: 'THSarabun-Italic.ttf',
        bolditalics: 'THSarabun-BoldItalic.ttf'
    }
}
/*$(function () { getfucLoad() })*/
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

$("#sms").on('load', fuc_edit_sms("#sms"));
$("#btn_search").on('click', function (e) {

     window.location.replace('/FrmSearchNumber/Index')
})
$("#btn_Report").on('click', function (e) {

     window.location.replace('/FrmReportTel/Index')
})

$('#current_date').datetimepicker({
    format: 'd/m/Y',
    formatDate: 'd/m/Y',

});


$('#date_tel').datetimepicker({
    format: 'd/m/Y',
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




//$("#browsers").on('load',
//    fuc_select_status()
//)
//$("#browsers4").on('load',
//    fuc_select_status_2()
//)
//$("#select_st").on('change', function (e) {
//    fuc_select_change_reson()
//})




function fuc_select_status() {
    $.ajax({
        url: "/FrmDetail/setcboStatus"
        , type: "GET",
        data: null,
        success: function (e) {
            console.log(e)
            if (e === "server มี ปัญหา" || e === "<empty string>") {
                alert2("server มี ปัญหา กำลัง reload ในอีกสักครู่")
                fuc_select_status();
            }
            if (cbostatus2 === null) {
                let values = JSON.parse(e)
                if (values != "" || values != null || e != "[]") {
                    let htmls = ` <select  id="select_st"  style="width:200px;height:25px;" >   <option value="" selected>
                                    -- โปรดเลือกสถานะ --
                                </option>`

                    for (i = 0; i < values.length; i++) {
                        if (values[i].RES_CODE === "01") {
                            htmls += `  <option  value="` + values[i].RES_CODE + `"  >` + values[i].RES_NAME + `</option>`
                        }
                        else {
                            htmls += `  <option  value="` + values[i].RES_CODE + `"  >` + values[i].RES_NAME + `</option>`
                        }
                    }
                    htmls += ` </select >`
                    console.log(htmls)
                    $("#select_st").html(htmls)

                    clearInterval()
                }
                else {
                    fuc_select_status() 
                }
               
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
            if (e === "server มี ปัญหา") {
                alert2("server มี ปัญหา กำลัง reload ในอีกสักครู่");
                fuc_select_status_2();
            }
            let values = JSON.parse(e)
            if (values != null || values != "" || e != "[]") {
                let htmls = ` <select style="width:200px;height:25px;"  id="cbocity" > `
                for (i = 0; i < values.length; i++) {
                    htmls += `  <option  value="` + values[i].CITY_CODE + `" >` + values[i].CITY_NAME_T + `</option>`
                }
                htmls += ` </select>`
                console.log(htmls)
                $("#cbocity").html(htmls)
            } else {
                fuc_select_status_2() 
            }
           
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
            if (e === "server มี ปัญหา") {
                clearAllCookies();

                window.location.replace('/FrmLogin/Index')
            }
            clearAllCookies();
       
             window.location.replace('/FrmLogin/Index')
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
            if (e !== null) {
                let valutf8 = decodeURIComponent(e);

                let values = JSON.parse(valutf8)
                if (values != null) {
                    if (values.SERVICE_1 !== null && values.SERVICE_1 == true) {
                        $("#SERVICE_01").attr("checked", true)
                    }
                    if (values.SERVICE_2 !== null && values.SERVICE_2 == true) {
                        $("#SERVICE_02").attr("checked", true)
                    }
                    if (values.SERVICE_3 !== null && values.SERVICE_3 == true) {
                        $("#SERVICE_03").prop("checked", true)

                    } if (values.SERVICE_4 !== null && values.SERVICE_4 == true) {
                        $("#SERVICE_04").prop("checked", true)

                    } if (values.SERVICE_5 !== null && values.SERVICE_5 == true) {
                        $("#SERVICE_05").prop("checked", true)

                    } if (values.SERVICE_6 !== null && values.SERVICE_6 == true) {
                        $("#SERVICE_06").prop("checked", true)

                    } if (values.SERVICE_7 !== null && values.SERVICE_7 == true) {
                        $("#SERVICE_07").prop("checked", true)

                    } if (values.SERVICE_8 !== null && values.SERVICE_8 == true) {
                        $("#SERVICE_08").prop("checked", true)

                    } if (values.SERVICE_9 !== null && values.SERVICE_9 == true) {
                        $("#SERVICE_09").prop("checked", true)

                    } if (values.SERVICE_10 !== null && values.SERVICE_10 == true) {
                        $("#SERVICE_10").prop("checked", true)

                    } if (values.SERVICE_11 !== null && values.SERVICE_11 == true) {
                        $("#SERVICE_11").prop("checked", true)

                    } if (values.SERVICE_12 !== null && values.SERVICE_12 == true) {
                        $("#SERVICE_12").prop("checked", true)

                    } if (values.SERVICE_13 !== null && values.SERVICE_13 == true) {
                        $("#SERVICE_13").prop("checked", true)

                    } if (values.SERVICE_14 !== null && values.SERVICE_14 == true) {
                        $("#SERVICE_14").prop("checked", true)

                    } if (values.SERVICE_15 !== null && values.SERVICE_15 == true) {
                        $("#SERVICE_15").prop("checked", true)

                    } if (values.SERVICE_16 !== null && values.SERVICE_16 == true) {
                        $("#SERVICE_16").prop("checked", true)

                    } if (values.SERVICE_17 !== null && values.SERVICE_17 == true) {
                        $("#SERVICE_17").prop("checked", true)

                    } if (values.SERVICE_18 !== null && values.SERVICE_18 == true) {
                        $("#SERVICE_18").prop("checked", true)

                    } if (values.SERVICE_19 !== null && values.SERVICE_19 == true) {
                        $("#SERVICE_19").prop("checked", true)

                    } if (values.SERVICE_20 !== null && values.SERVICE_20 == true) {
                        $("#SERVICE_20").prop("checked", true)

                    } if (values.SERVICE_21 !== null && values.SERVICE_21 == true) {
                        $("#SERVICE_21").prop("checked", true)

                    } if (values.SERVICE_22 !== null && values.SERVICE_22 == true) {
                        $("#SERVICE_22").prop("checked", true)

                    } if (values.SERVICE_23 !== null && values.SERVICE_23 == true) {
                        $("#SERVICE_23").prop("checked", true)

                    } if (values.SERVICE_24 !== null && values.SERVICE_24 == true) {
                        $("#SERVICE_24").prop("checked", true)

                    } if (values.SERVICE_25 !== null && values.SERVICE_25 == true) {
                        $("#SERVICE_25").prop("checked", true)

                    } if (values.SERVICE_26 !== null && values.SERVICE_26 == true) {
                        $("#SERVICE_26").prop("checked", true)

                    } if (values.SERVICE_27 !== null && values.SERVICE_27 == true) {
                        $("#SERVICE_27").prop("checked", true)

                    } if (values.SERVICE_28 !== null && values.SERVICE_28 == true) {
                        $("#SERVICE_28").prop("checked", true)

                    } if (values.SERVICE_29 !== null && values.SERVICE_29 == true) {
                        $("#SERVICE_29").prop("checked", true)

                    }
                    //$("#select_st").html(` <select  id="select_st"  style="width:200px;height:25px;" >
                    //                <option>

                    //                </option>
                    //            </select>`)
                    $("#select_st").append(`<option value="` + values.res_code + `" selected> ` + values.cboStatus + `</option>`)
                    $("#select_rs").append(`<option value="` + values.strDenycode + `" selected> ` + values.strDeny + `</option>`)
                    $("#txt_tel").val(values.txtTel_No)
                    $("#cname").val(values.txtName)
                    $("#csname").val(values.txtSName)
                    if (values.cboSex === "F") {
                        $("#sex2").append(`<option value="F" selected>หญิง</option>`)
                    }
                    else if (values.cboSex === "N") {
                        $("#sex2").append(`<option value="N" selected>ไม่ระบุ</option>`)
                    }
                    else {
                        $("#sex2").append(`<option value="M" selected>ชาย</option>`)
                    }
                    $("#date_tel").val(values.txtDate_Tel)
                    $("#date_thai").val(values.Date_thai)
                    $("#date_num").val(values.cboDate_No)
                    $("#mouth_thai").val(values.cboMouth)
                    $("#year_thai").val(values.txtYear)
                    $("#cbocity").val(values.cbocity)
                    /*   $("#status").text(e)*/
                    cbostatus2 = values.cboStatus;
                    if (values.cboStatus === "สมัคร") {

                        $("#btn_add_ser").show()
                    } else {
                        $("#btn_add_ser").hide()
                    }
                    fuc_select_change_reson()
                    $("#btn_save").hide()
                    $("#btn_save2").show()
                }
            }
            else {
                getfucLoad()
            }
      
         
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
//            alert2(e);
//            if (e === "บันทึกสำเร็จ") {
//                alert2(e)
//                fuc_insert_ser();
//            }
//            else {
//                alert2(e)
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
            if (e !== null) {
                let values = JSON.parse(e)
                console.log(values)
                let htmls = ``
                for (i = 0; i < values.length; i++) {
                    if (i <= 10) {

                        htmls += `<div style="padding:1%;" >`

                        column_name_id_ser += `SERVICE_` + values[i].SER_ID + `,`
                        column_name_ser += values[i].SER_NAME + ","
                        htmls += `<span style="padding:1%;"><input type="checkbox"  class="chk_service" id="SERVICE_` + values[i].SER_ID + `" placeholder="" class="input" />  <span class="lb_service" id="lbserSERVICE_` + values[i].SER_ID + `" > บริการ ` + values[i].SER_NAME + ` </span> </span>  `


                        htmls += `</div>`

                    }
                    else if (i <= 20) {

                        htmls += `<div style="padding:1%;" >`

                        column_name_id_ser += `SERVICE_` + values[i].SER_ID + `,`
                        column_name_ser += values[i].SER_NAME + ","
                        htmls += ` <span class="lb_service" id="lbserSERVICE_` + values[i].SER_ID + `" > บริการ ` + values[i].SER_NAME + ` </span>  <input type="checkbox" class="chk_service"  id="SERVICE_` + values[i].SER_ID + `" placeholder="" class="input" />  `

                        htmls += `</div>`

                    } else if (i <= 30) {

                        htmls += `<div style="padding:1%;" >`

                        column_name_id_ser += `SERVICE_` + values[i].SER_ID + `,`
                        column_name_ser += values[i].SER_NAME + ","
                        htmls += ` <span class="lb_service" id="lbserSERVICE_` + values[i].SER_ID + `" > บริการ ` + values[i].SER_NAME + ` </span>  <input type="checkbox"  class="chk_service" id="SERVICE_` + values[i].SER_ID + `" placeholder="" class="input" />  `

                        htmls += `</div>`

                    }
                    else if (i <= 39) {

                        htmls += `<div style="padding:1%;" >`

                        column_name_id_ser += `SERVICE_` + values[i].SER_ID + `,`
                        column_name_ser += values[i].SER_NAME + ","
                        htmls += ` <span class="lb_service" id="lbserSERVICE_` + values[i].SER_ID + `" > บริการ ` + values[i].SER_NAME + ` </span>  <input type="checkbox"  class="chk_service"  id="SERVICE_` + values[i].SER_ID + `" placeholder="" class="input" />  `

                        htmls += `</div>`

                    }

                }
                $(sms).html(htmls)
                getfucLoad();
            }

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
                lengthChange: false ,
                columns: [
                    { data: 'SER_ID', render: function (data, type, row) { return '<input type="checkbox" id="addSERVICE_' + data + '" value="' + data + '" data-active="' + row["IS_ACTIVE"]+'" class="editor-active">' } },
                    { data: 'SER_ID', title: 'หมายเลขบริการ' },
                    { data: 'IS_ACTIVE', render: function (data, type, row) { return `<span id="ch` + row["SER_ID"] + `">` + data + `</span>` } },
                    {
                        data: 'SER_NAME', render: function (data, type, row) {
                            return '<input type="text" id="editSERVICE_' + row["SER_ID"] + '" value="' + data + '" class="editor-active2 m-3 " disabled style="width:100%;"><button id="btn_ser_save' + row["SER_ID"] + '" class="btn btn-primary " >บันทึก</button>' } },   
                    { data: 'SER_NAME', render: function (data, type, row) { return '<span style="display: none;">' + data + '</span>' } }
                   
    

                ],
                data: values,
                "createdRow": function (row, data, dataIndex) {
                    // ตรวจสอบเงื่อนไขของข้อมูลในแต่ละคอลัมน์
                    // ในตัวอย่างนี้เราจะตรวจสอบคอลัมน์ที่ 2 (index 1)
                    var columnValue = data.IS_ACTIVE;
                    console.log(columnValue)// เปลี่ยนตามคอลัมน์ที่คุณต้องการ
                    if (columnValue == 'เปิดให้ใช้บริการ') {
                        // กำหนดพื้นหลัง (background color) สำหรับแถวนี้
                        $(row).css('background-color', 'lightgreen');
                    } else if (columnValue == 'ปิดการใช้บริการ') {
                        $(row).css('background-color', 'lightcoral');
                    }
                }
            })
   /*         table2.addClass("table-responsive-xl")*/
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
            alert2(e)
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
            alert2(e);
/*            fuc_insert_ser();*/
        }
    })
}
function fuc_sms_ser_show(sms) {
    //$("#sms2").remove()
    $("#sms2").remove()
    $("#sms_modal").html(`<div id="sms2" style=" width: 1000px; right: 200px; flex-wrap: wrap;"></div>`)
    let htmls2 = ``;
    let nameid = column_name_id_ser.split(',')
    let name = column_name_ser.split(',')
    console.log(name)

    htmls2 += `<div style="padding:1%;" >`
    for (i = 0; i < name.length - 1; i++) {
        if ($("#" + nameid[i] + "").is(':checked')) {
            htmls2 += `  <input type="checkbox" id="lb` + nameid[i] + `" placeholder="" class="input" /> <span style="padding:1%;"> <span id="lbsersh` + nameid[i] + `" > บริการ ` + name[i] +`</span>`
        }

    }
    htmls2 += `</div style="padding:1%;" >`
    column_name_id_ser = ``;
    column_name_ser = ``;
    $(sms).html(htmls2)
}
$("#btn_logout").on('click', function (e) { fuclogout() })

$("#btn_reload").on('click', function (e) {
    fuc_edit_sms("#sms")
})
$("#btn_add_ser").on('click', function (e) { fuc_edit_sms("#sms") })
$("#btn_ser_add").on('click', function (e) { fuc_insert_ser() })
$("#btn_ser_set_active").on('click', function (e) { fuc_insert_ser_confirm() })


//$("#btn_sms_save").on('click', function (e) {

//    fuc_sms_ser_show("#sms2")
//})

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
            if (e === "server มี ปัญหา") {
                alert2("server มีปัญหา กำลัง reload ในอักสักครู่")
                cbostatus()
            }
            else {
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
                        alert2("ไม่มี object ")
                    }
                }
                htmls += '</select>'
                $("#select_reson").html(htmls);
            }
          
        }
    })
}
function sum(table) {
    if (table[0].SUM != "") {
        $("#Label8").text(table[0].SUM)
        $("#Label9").text(table[0].SER01)
        $("#Label10").text(table[0].SER02)
        $("#Label11").text(table[0].SER03)
        $("#Label12").text(table[0].SER04)
    }

}
function formatNumber(number, decimalPlaces) {
    let numbers = number.torelative(decimalPlaces);
    return numbers.replace('0', 'X').replace('.', '').replace('1', 'X').replace('2','X').replace('4','X').replace('5','X').replace('7','X').replace('8','X').replace('9','X')
}
function tableload(tables) {
   
    $("#Label4").text(tables.length)
    $('#tb_1').DataTable().destroy();
    $('#tb_1').DataTable({
        searching: false,
        lengthChange: false,
        dom: 'Bfrtip',
        buttons: [
            //{
            //    extend: 'copy',
            //    text: 'Copy',
            //    exportOptions: {
            //        encoding: 'utf8',
            //        orientation: 'landscape'
            //    }
            //},
            {
                extend: 'csv',
                text: 'CSV',
                exportOptions: {
                    encoding: 'utf8'
                }
            },
            {
                extend: 'excel',
                text: 'Excel',
                exportOptions: {
                    encoding: 'utf8'
                }
            },
            //{
            //    extend: 'pdf',
            //    text: 'PDF',
            //    exportOptions: {
            //        encoding: 'utf8',
            //        orientation: 'landscape'
            //    },

            //    customize: function (doc) {
            //        // กำหนดแบบตัวอักษรที่รองรับภาษาไทย
            //        //doc.fonts = {
            //        //    Sarabun: {
            //        //        normal: 'THSarabunNew.ttf',
            //        //        bold: 'THSarabunNew-Bold',
            //        //        italics: 'THSarabunNew-Italic.ttf',
            //        //        bolditalics: 'THSarabunNew-BoldItalic.ttf'
            //        //    }
            //        //    // เพิ่มฟอนต์เพิ่มเติมตามความต้องการ
            //        //};
                 
            //        doc.defaultStyle = {
            //            font: 'THSarabun',
            //            fontSize: 16
            //        };
            //        // ปรับแต่งรายละเอียดเพิ่มเติมตามความต้องการ
            //        // เช่น ขนาดตัวอักษร, การจัดหน้า, สีพื้นหลัง, ขนาดกรอบ, ฯลฯ
            //    }
            //},
            //{
            //    extend: 'print',
            //    text: 'Print',
            //    exportOptions: {
            //        encoding: 'utf8'
            //        , orientation: 'landscape'
            //    }
            //}
        ],
         columns: [
             { data: 'ANUMBER', title: 'เบอร์โทรศัพท์', render: function (data, type, row) { return `<span>` +   data.substring(0, 3) +`-XXX-XXXX<span>` } },
            { data: 'CUST_NAME', title: 'ชื่อ' },
            { data: 'CUST_SNAME', title: 'นามสกุล' },
             { data: 'SERVICE_21', title: 'บริการ SMS ดวงไพ่ยิปซี อ.เนตรทิพย์' },
             { data: 'SERVICE_11', title: 'บริการ VDO HORO รวมโหรดัง' },
             { data: 'SERVICE_12', title: 'SMS เสริมทรัพทย์เสริมโชค' },
             { data: 'SERVICE_13', title: 'VDO ดวง อ.เก่งกาจ' }
            //{ data: 'SERVICE_04', title: 'Service 04' },
            //{ data: 'SERVICE_05', title: 'Service 05' },
            //{ data: 'SERVICE_06', title: 'Service 06' },
            //{ data: 'SERVICE_07', title: 'Service 07' },
            //{ data: 'SERVICE_08', title: 'Service 08' },
            //{ data: 'SERVICE_09', title: 'Service 09' },
            //{ data: 'SERVICE_10', title: 'Service 10' },
            //{ data: 'SERVICE_11', title: 'Service 11' },
            //{ data: 'SERVICE_12', title: 'Service 12' },
            //{ data: 'SERVICE_13', title: 'Service 13' },
            //{ data: 'SERVICE_14', title: 'Service 14' },
            //{ data: 'SERVICE_15', title: 'Service 15' },
            //{ data: 'SERVICE_16', title: 'Service 16' },
            //{ data: 'SERVICE_23', title: 'Service 23' },

        ],
        data: tables
     }).draw()
}

let set_interval;
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
                    if (e === null || e === "") {
                        btnreport_click();
                    }
                    if (e === "ไม่มีข้อมูลที่คุณค้นหา") {
                        alert2("ไม่มีข้อมูลที่คุณค้นหา")
                    }
                    else if (e === "ไม่สามารถแสดงข้อมูลได้ เนื่องจากมีข้อผิดพลาด") {
                    }
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

     window.location.replace('/FrmDetail/Index')
}


$(document).on('load', cbostatus())
$("#btn_report").on('click', function (e) {
        btnreport_click()
})
$("#btn_logout").on('click', function (e) { fuclogout() })
$("#btn_back").on('click', function (e) { fucback() })




 /*   $(document).on('load', gettable())*/



    //$('#exportBtn').on('click', function () {
    //    console.log(tables.rows())
    //    var csv = tables
    //        .rows()
    //        .data()
    //        .map(function (row) {
    //            return row.join(',');
    //        })
    //        .join('\n');

    //    // Create a Blob object and trigger a download
    //    var blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
    //    var link = document.createElement('a');

    //    if (link.download !== undefined) {
    //        var url = URL.createObjectURL(blob);
    //        link.setAttribute('href', url);
    //        link.setAttribute('download', 'datatable.csv');
    //        link.style.visibility = 'hidden';
    //        document.body.appendChild(link);
    //        link.click();
    //        document.body.removeChild(link);
    //    }
    //});

    $("#btn_logout").on('click', fuclogout)


    //tables.on('click', 'tbody tr', (e) => {
    //    let classList = e.currentTarget.classList;

    //    if (classList.contains('selected')) {
    //        classList.remove('selected');

    //        classList.add('new-background-color2');
    //        classList.remove('new-background-color');



    //    }
    //    else {


    //        /* tables.rows('.selected').nodes().each((row) => row.classList.remove('selected'));*/
    //        classList.add('selected');


    //        classList.add('new-background-color');
    //        classList.remove('new-background-color2');

    //        var rowData = $('#tb_1').DataTable().rows('.new-background-color').data()[0];


    //    }
    //});
    //table2.on('draw', function () {

    //    fuc_edit_sms("#sms")
    //    $("#btn_sms_add_save").prop('disabled', true);
    //    $("#btn_ser_remove").prop('disabled', true);
    //})
   
